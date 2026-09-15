using neXn.SteamController2026.SDL3;
using neXn.SteamController2026.SDL3.Models;
using NUnit.Framework.Internal;
using SDL3;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class SteamControllerClientTests
    {
        [Test]
        public async Task GetStatusAsync_BatteryInitiallyUnknown_EventuallyReturnsBattery()
        {
            FakeSdlGamepadApi sdl = new();

            sdl.PowerStates.Enqueue((SDL.PowerState.Unknown, -1));
            sdl.PowerStates.Enqueue((SDL.PowerState.Unknown, -1));
            sdl.PowerStates.Enqueue((SDL.PowerState.Charged, 100));

            SteamControllerStatus result = null;

            using (SteamControllerClient sut = new())
            {
                sut._sdl = sdl;
                sut._delay = (_, _) => Task.CompletedTask;

                Assert.That(sut.Initialize(), Is.True);

                result = await sut.GetStatusAsync();
            }

            Assert.Multiple(() =>
            {
                Assert.That(result.IsConnected, Is.True);
                Assert.That(result.BatteryPercentage, Is.EqualTo(100));
                Assert.That(result.PowerState, Is.EqualTo(SteamControllerPowerState.Charged));
            });
        }

        [Test]
        public async Task GetStatusAsync_NoGamepads_ReturnsDisconnected()
        {
            var sdl = new FakeSdlGamepadApi
            {
                Gamepads = []
            };

            SteamControllerStatus result = null;

            using (SteamControllerClient sut = new())
            {
                sut._sdl = sdl;
                sut._delay = (_, _) => Task.CompletedTask;
                sut.Initialize();

                result = await sut.GetStatusAsync();
            }

            Assert.Multiple(() =>
            {
                Assert.That(result.IsConnected, Is.False);
                Assert.That(result.BatteryPercentage, Is.EqualTo(-1));
                Assert.That(
                    result.PowerState,
                    Is.EqualTo(SteamControllerPowerState.Disconnected));
            });
        }

        [Test]
        public async Task GetStatusAsync_NonValveController_ReturnsDisconnected()
        {
            FakeSdlGamepadApi sdl = new()
            {
                Vendor = 0x045E, // something other than Valve
                Product = 0x0001
            };

            SteamControllerStatus result = null;

            using (SteamControllerClient sut = new())
            {
                sut._sdl = sdl;
                sut._delay = (_, _) => Task.CompletedTask;

                sut.Initialize();

                result = await sut.GetStatusAsync();
            }

            Assert.That(result.IsConnected, Is.False);
        }

        [TestCase((ushort)0x28DE, (ushort)0x1302, true)]
        [TestCase((ushort)0x28DE, (ushort)0x1303, true)]
        [TestCase((ushort)0x28DE, (ushort)0x1304, true)]
        [TestCase((ushort)0x28DE, (ushort)0x1305, true)]
        [TestCase((ushort)0x28DE, (ushort)0x9999, false)]
        [TestCase((ushort)0x045E, (ushort)0x1302, false)]
        public void IsSteamController_ReturnsExpectedResult(ushort vendor, ushort product, bool expected)
        {
            bool actual = SteamControllerClient.IsSteamController(vendor, product);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
