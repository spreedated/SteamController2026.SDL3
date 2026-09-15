using neXn.SteamController2026.SDL3;
using SDL3;
using System.Collections.Generic;

namespace UnitTests
{
    internal sealed class FakeSdlGamepadApi : ISdlGamepadApi
    {
        public bool InitResult { get; set; } = true;

        public uint[] Gamepads { get; set; } = [1];

        public ushort Vendor { get; set; } = SteamControllerClient.VALVE_VENDOR_ID;

        public ushort Product { get; set; } = SteamControllerClient.USB;

        public nint GamepadHandle { get; set; } = 123;

        public bool Connected { get; set; } = true;

        public Queue<(SDL.PowerState State, int Percentage)> PowerStates { get; } = new();

        public bool Init(SDL.InitFlags flags) => InitResult;

        public void QuitSubSystem(SDL.InitFlags flags)
        {
        }

        public uint[] GetGamepads(out int count)
        {
            count = Gamepads.Length;
            return Gamepads;
        }

        public ushort GetGamepadVendorForID(uint id) => Vendor;

        public ushort GetGamepadProductForID(uint id) => Product;

        public nint OpenGamepad(uint id) => GamepadHandle;

        public void CloseGamepad(nint gamepad)
        {
        }

        public bool GamepadConnected(nint gamepad) => Connected;

        public void UpdateGamepads()
        {
        }

        public SDL.PowerState GetGamepadPowerInfo(
            nint gamepad,
            out int percentage)
        {
            if (PowerStates.Count == 0)
            {
                percentage = -1;
                return SDL.PowerState.Unknown;
            }

            var result = PowerStates.Dequeue();

            percentage = result.Percentage;

            return result.State;
        }

        public string GetGamepadName(nint gamepad) => "Steam Controller";

        public string GetError() => "Fake SDL error";
    }
}
