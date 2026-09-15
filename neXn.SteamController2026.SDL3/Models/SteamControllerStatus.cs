namespace neXn.SteamController2026.SDL3.Models
{
    public sealed record SteamControllerStatus
    {
        public bool IsConnected { get; init; }

        public int BatteryPercentage { get; init; }

        public SteamControllerPowerState PowerState { get; init; }
    }
}
