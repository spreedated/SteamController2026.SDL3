using Microsoft.Extensions.Logging;
using neXn.SteamController2026.SDL3.Models;
using SDL3;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace neXn.SteamController2026.SDL3
{
    public sealed class SteamControllerClient : IDisposable, IAsyncDisposable
    {
        public const ushort VALVE_VENDOR_ID = 0x28DE;
        public const ushort USB = 0x1302;
        public const ushort BLUETOOTH = 0x1303;
        public const ushort PROTEUS_PUCK = 0x1304;
        public const ushort NEREID_PUCK = 0x1305;

        internal ISdlGamepadApi _sdl;
        internal Func<int, CancellationToken, Task> _delay;
        private readonly ILogger _logger;
        private readonly SemaphoreSlim _semagate = new(1, 1);

        private nint _gamepad = nint.Zero;
        private bool _sdlInitialized;

        private int _retries = 50;

        /// <summary>
        /// Retries before reporting status Unknown<br/>
        /// minimum value is 50.
        /// </summary>
        public int Retries
        {
            get
            {
                return _retries;
            }
            set
            {
                _retries = Math.Max(50, value);
            }
        }

        private int _delayBeforeRetry = 250;

        /// <summary>
        /// Delay in ms before the next retry<br/>
        /// minimum value is 250.
        /// </summary>
        public int DelayBeforeRetry
        {
            get
            {
                return _delayBeforeRetry;
            }
            set
            {
                _delayBeforeRetry = Math.Max(250, value);
            }
        }

        private static SteamControllerPowerState ConvertPowerState(SDL.PowerState state)
        {
            return state switch
            {
                SDL.PowerState.OnBattery => SteamControllerPowerState.OnBattery,
                SDL.PowerState.Charging => SteamControllerPowerState.Charging,
                SDL.PowerState.Charged => SteamControllerPowerState.Charged,
                _ => SteamControllerPowerState.Unknown
            };
        }

        private void CloseGamepadConnection()
        {
            if (_gamepad == nint.Zero)
            {
                return;
            }

            _sdl.CloseGamepad(_gamepad);
            _gamepad = nint.Zero;

            _logger?.LogTrace("Steam Controller connection closed.");
        }

        private bool OpenConnectionToGamepad(IEnumerable<uint> gamepads)
        {
            foreach (uint id in gamepads)
            {
                ushort vendor = _sdl.GetGamepadVendorForID(id);
                ushort product = _sdl.GetGamepadProductForID(id);

                if (!IsSteamController(vendor, product))
                {
                    continue;
                }

                _gamepad = _sdl.OpenGamepad(id);

                if (_gamepad == nint.Zero)
                {
                    _logger?.LogError("Couldn't open Steam Controller: {Error}", _sdl.GetError());
                    continue;
                }

                _logger?.LogTrace("Steam Controller connected: {Name}", _sdl.GetGamepadName(_gamepad));

                return true;
            }

            _logger?.LogDebug("Steam Controller not found.");
            return false;
        }

        private bool OpenGamepadSdl()
        {
            if (_gamepad == nint.Zero)
            {
                uint[] gamepads = _sdl.GetGamepads(out int count);

                if (count == 0)
                {
                    _logger?.LogWarning("No gamepads found.");
                    return false;
                }

                return this.OpenConnectionToGamepad(gamepads);
            }

            return true;
        }

        private async Task<SteamControllerStatus> GetSteamControllerStatus(CancellationToken cancellationToken = default)
        {
            int retries = this.Retries;
            int percentage = -1;

            SDL.PowerState state = SDL.PowerState.Unknown;

            while (retries-- > 0)
            {
                _sdl.UpdateGamepads();

                if (!_sdl.GamepadConnected(_gamepad))
                {
                    this.CloseGamepadConnection();

                    return new SteamControllerStatus
                    {
                        IsConnected = false,
                        BatteryPercentage = -1,
                        PowerState = SteamControllerPowerState.Disconnected
                    };
                }

                state = _sdl.GetGamepadPowerInfo(_gamepad, out percentage);

                if (state != SDL.PowerState.Unknown && state != SDL.PowerState.Error)
                {
                    return new SteamControllerStatus
                    {
                        IsConnected = true,
                        BatteryPercentage = percentage,
                        PowerState = ConvertPowerState(state)
                    };
                }

                if (retries > 0)
                {
                    await _delay(this.DelayBeforeRetry, cancellationToken);
                }
            }

            return new SteamControllerStatus
            {
                IsConnected = true,
                BatteryPercentage = percentage,
                PowerState = ConvertPowerState(state)
            };
        }

        #region Ctor
        public SteamControllerClient(ILogger logger = null)
        {
            _sdl = new SdlGamepadApi();
            _logger = logger;
            _delay = Task.Delay;
        }
        #endregion

        public static bool IsSteamController(ushort vendor, ushort product)
        {
            return vendor == VALVE_VENDOR_ID && product is USB
                       or BLUETOOTH
                       or PROTEUS_PUCK
                       or NEREID_PUCK;
        }

        /// <summary>
        /// Initializes SDL's gamepad subsystem.<br/>
        /// Call this from the application's main thread.
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            ObjectDisposedException.ThrowIf(this.IsDisposed, this);

            _semagate.Wait();

            if (_sdlInitialized)
            {
                return true;
            }

            if (!_sdl.Init(SDL.InitFlags.Gamepad))
            {
                _logger?.LogError("SDL3 couldn't initialize: {Error}", SDL.GetError());
                return false;
            }

            _sdlInitialized = true;
            _semagate.Release();

            return true;
        }

        public async Task<SteamControllerStatus> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(this.IsDisposed, this);

            if (!_sdlInitialized)
            {
                _logger?.LogWarning("SDL3 is not initialized. Call Initialize() first.");
                throw new InvalidOperationException("SDL3 is not initialized. Call Initialize() first.");
            }

            await _semagate.WaitAsync(cancellationToken);

            try
            {
                ObjectDisposedException.ThrowIf(this.IsDisposed, this);

                _sdl.UpdateGamepads();

                if (!this.OpenGamepadSdl())
                {
                    return new()
                    {
                        IsConnected = false,
                        BatteryPercentage = -1,
                        PowerState = SteamControllerPowerState.Disconnected
                    };
                }

                return await this.GetSteamControllerStatus(cancellationToken);
            }
            finally
            {
                _semagate.Release();
            }
        }

        #region Dispose

        private int _disposeState;

        private bool IsDisposed => Volatile.Read(ref _disposeState) != 0;

        private void DisposeCore()
        {
            this.CloseGamepadConnection();

            if (_sdlInitialized)
            {
                _sdl.QuitSubSystem(SDL.InitFlags.Gamepad);
                _sdlInitialized = false;
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposeState, 1) != 0)
            {
                return;
            }

            _semagate.Wait();

            try
            {
                this.DisposeCore();
            }
            finally
            {
                _semagate.Release();
            }

            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposeState, 1) != 0)
            {
                return;
            }

            await _semagate.WaitAsync().ConfigureAwait(false);

            try
            {
                this.DisposeCore();
            }
            finally
            {
                _semagate.Release();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
