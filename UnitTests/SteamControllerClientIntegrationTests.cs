using neXn.SteamController2026.SDL3;
using neXn.SteamController2026.SDL3.Models;
using NUnit.Framework.Internal;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class SteamControllerClientIntegrationTests
    {
        [Test]
        [Category("Hardware")]
        [Explicit("Requires an actual Steam Controller 2026")]
        public async Task RealController_ReturnsBatteryStatus()
        {
            await using var sut = new SteamControllerClient();

            Assert.That(sut.Initialize(), Is.True);

            SteamControllerStatus status = await sut.GetStatusAsync();

            Assert.Multiple(() =>
            {
                Assert.That(status.IsConnected, Is.True);
                Assert.That(status.BatteryPercentage, Is.InRange(0, 100));
                Assert.That(status.PowerState, Is.Not.EqualTo(SteamControllerPowerState.Unknown));
            });
        }
    }
}
