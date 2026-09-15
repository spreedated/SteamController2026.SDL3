# neXn.SteamController2026.SDL3

[![NuGet](https://img.shields.io/nuget/v/neXn.SteamController2026.SDL3?style=flat-square&logo=nuget&label=NuGet)](https://www.nuget.org/packages/neXn.SteamController2026.SDL3)
[![NuGet Downloads](https://img.shields.io/nuget/dt/neXn.SteamController2026.SDL3?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/neXn.SteamController2026.SDL3)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12-512BD4?style=flat-square&logo=csharp)
![SDL3](https://img.shields.io/badge/SDL-3-0A84FF?style=flat-square)
[![License](https://img.shields.io/github/license/spreedated/SteamController2026.SDL3?style=flat-square)](https://github.com/spreedated/SteamController2026.SDL3/blob/main/LICENSE)
	
[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://buymeacoffee.com/spreed)

A lightweight .NET library for detecting the 2026 Steam Controller and 
reading its battery percentage and power state through SDL3.

Supports USB, Bluetooth, Proteus wireless puck, and Nereid wireless puck connections.

## Features

- Detects the **Steam Controller 2026**
- Reads the current **battery percentage**
- Reads the current **power state**
  - `OnBattery`
  - `Charging`
  - `Charged`
  - `Unknown`
  - `Disconnected`
- Automatic controller discovery through SDL3
- Handles controller disconnects and reconnects
- Supports the known 2026 Steam Controller connection variants:
  - USB
  - Bluetooth
  - Proteus wireless puck
  - Nereid wireless puck
- Handles the controller's delayed initial battery report
- Thread-safe status queries
- Supports `CancellationToken`
- Implements both `IDisposable` and `IAsyncDisposable`
- Optional `Microsoft.Extensions.Logging` integration
- Small API surface with SDL-specific implementation details hidden from consumers

## Usage

```csharp
using neXn.SteamController2026.SDL3;

await using SteamControllerClient controller = new();

if (!controller.Initialize())
{
    Console.WriteLine("SDL3 could not be initialized.");
    return;
}

SteamControllerStatus status = await controller.GetStatusAsync();

if (!status.IsConnected)
{
    Console.WriteLine("Steam Controller is disconnected.");
    return;
}

Console.WriteLine($"Battery: {status.BatteryPercentage}%");
Console.WriteLine($"Power state: {status.PowerState}");
```

Example outputs:

```
Battery: 82%
Power state: OnBattery
```

```
Battery: 64%
Power state: Charging
```


## Initialization

SDL's gamepad subsystem must be initialized before requesting controller data:

```csharp
SteamControllerClient controller = new();

bool initialized = controller.Initialize();
```

`Initialize()` should be called from the application's main thread.

After initialization, `GetStatusAsync()` can be used for asynchronous battery polling.

Calling `GetStatusAsync()` before `Initialize()` results in an
InvalidOperationException.

## Battery reporting

The 2026 Steam Controller does not necessarily report its battery state
immediately after being connected.

SDL may initially report:

```
PowerState: Unknown
BatteryPercentage: -1
```

The controller eventually sends its battery telemetry and SDL caches the
result.

`SteamControllerClient` handles this automatically by retrying the battery
query for a limited period.

```csharp
Retries = 50;
DelayBeforeRetry = 250;
```

These values can be changed:

```csharp
SteamControllerClient controller = new()
{
    Retries = 60,
    DelayBeforeRetry = 300
};
```

Once SDL has received a valid battery report, subsequent calls normally
return immediately.

## Supported connections

The library recognizes the known 2026 Steam Controller device IDs:

|Connection|Vendor ID|Product ID|
|---|---|---|
|USB|0x28DE|0x1302|
|Bluetooth|0x28DE|0x1303|
|Proteus wireless puck|0x28DE|0x1304|
|Nereid wireless puck|0x28DE|0x1305|

## SDL3

This library uses SDL3 for controller discovery and battery-state reporting.

SDL handles the device-specific HID communication and exposes the battery
information through its gamepad API.

Conceptually:

```
Steam Controller 2026
        │
        ▼
USB / Bluetooth / Wireless Puck
        │
        ▼
SDL3
        │
        ▼
SDL3-CS
        │
        ▼
neXn.SteamController2026.SDL3
        │
        ▼
Your .NET application
```

## License

This project is licensed under the [MIT License](LICENSE.txt).

## Disclaimer

This is an independent open-source project and is not affiliated with, endorsed by, or sponsored by Valve Corporation.
Steam and Steam Controller are trademarks of their respective owner.

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://buymeacoffee.com/spreed)