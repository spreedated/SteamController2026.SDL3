using SDL3;

namespace neXn.SteamController2026.SDL3
{
    internal interface ISdlGamepadApi
    {
        bool Init(SDL.InitFlags flags);
        void QuitSubSystem(SDL.InitFlags flags);

        uint[] GetGamepads(out int count);

        ushort GetGamepadVendorForID(uint id);
        ushort GetGamepadProductForID(uint id);

        nint OpenGamepad(uint id);
        void CloseGamepad(nint gamepad);
        bool GamepadConnected(nint gamepad);

        void UpdateGamepads();

        SDL.PowerState GetGamepadPowerInfo(
            nint gamepad,
            out int percentage);

        string GetGamepadName(nint gamepad);
        string GetError();
    }
}
