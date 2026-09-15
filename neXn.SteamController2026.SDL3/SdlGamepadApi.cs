using SDL3;

namespace neXn.SteamController2026.SDL3
{
    public sealed class SdlGamepadApi : ISdlGamepadApi
    {
        public bool Init(SDL.InitFlags flags) => SDL.Init(flags);

        public void QuitSubSystem(SDL.InitFlags flags) => SDL.QuitSubSystem(flags);

        public uint[] GetGamepads(out int count) => SDL.GetGamepads(out count);

        public ushort GetGamepadVendorForID(uint id) => SDL.GetGamepadVendorForID(id);

        public ushort GetGamepadProductForID(uint id) => SDL.GetGamepadProductForID(id);

        public nint OpenGamepad(uint id) => SDL.OpenGamepad(id);

        public void CloseGamepad(nint gamepad) => SDL.CloseGamepad(gamepad);

        public bool GamepadConnected(nint gamepad) => SDL.GamepadConnected(gamepad);

        public void UpdateGamepads() => SDL.UpdateGamepads();

        public SDL.PowerState GetGamepadPowerInfo(nint gamepad, out int percentage) => SDL.GetGamepadPowerInfo(gamepad, out percentage);

        public string GetGamepadName(nint gamepad) => SDL.GetGamepadName(gamepad);

        public string GetError() => SDL.GetError();
    }
}
