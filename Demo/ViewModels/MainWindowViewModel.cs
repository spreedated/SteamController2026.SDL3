using CommunityToolkit.Mvvm.ComponentModel;
using neXn.SteamController2026.SDL3;
using neXn.SteamController2026.SDL3.Models;
using System;
using System.Drawing;
using Serilog.Extensions.Logging;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;
using Serilog;
using System.Text;
using System.Threading;
using CommunityToolkit.Mvvm.Input;

namespace Demo.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private const char LIGHT_SHADE_CHAR = '\u2591';
        private const char FULL_BLOCK_CHAR = '\u2588';
        private const string BUTTON_TEXT_START = "&Start polling";
        private const string BUTTON_TEXT_STOP = "&Stop polling";

        private readonly Form _instance;
        private readonly SteamControllerClient _client;
        private readonly System.Timers.Timer _pollingTimer;
        private readonly System.Timers.Timer _logOutputTimer;
        private readonly System.Timers.Timer _animationTimer;
        private int _animationPosition = 0;
        private bool _animationDirection = false;

        [ObservableProperty]
        public partial string ControllerState { get; set; } = "Unknown";

        [ObservableProperty]
        public partial Color ColorState { get; set; } = Color.Red;

        [ObservableProperty]
        public partial string BatteryLoad { get; set; } = new string(LIGHT_SHADE_CHAR, 10);

        [ObservableProperty]
        public partial string BatteryPercentage { get; set; } = "--- %";

        [ObservableProperty]
        public partial string LastUpdate { get; set; } = "---";

        [ObservableProperty]
        public partial string LogOutput { get; set; } = "---";

        [ObservableProperty]
        public partial string ButtonText { get; set; } = BUTTON_TEXT_START;

        [ObservableProperty]
        public partial bool IsRunning { get; set; }

        partial void OnIsRunningChanged(bool value)
        {
            if (!value)
            {
                _animationTimer.Stop();
                this.AnimationLabelText = null;
                return;
            }

            _animationTimer.Start();
        }

        [ObservableProperty]
        public partial string AnimationLabelText { get; set; }

        private void OnPollingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            SteamControllerStatus res = _client.GetStatusAsync().Result;
            sw.Stop();

            Log.Information("Polling duration: {Duration}", sw.Elapsed.ToString("mm':'ss':'fff"));

            if (_instance.IsDisposed || _instance.Disposing || !_instance.IsHandleCreated)
            {
                return;
            }

            try
            {
                _instance.BeginInvoke(() =>
                {
                    this.BatteryPercentage = res.PowerState == SteamControllerPowerState.Unknown || res.PowerState == SteamControllerPowerState.Disconnected ? "--- %" : $"{res.BatteryPercentage} %";
                    this.ControllerState = res.PowerState.ToString();
                    this.ColorState = res.PowerState == SteamControllerPowerState.Unknown || res.PowerState == SteamControllerPowerState.Disconnected ? Color.Red : Color.Green;
                    this.BatteryLoad = res.PowerState == SteamControllerPowerState.Unknown || res.PowerState == SteamControllerPowerState.Disconnected ? new string(LIGHT_SHADE_CHAR, 10) : new string(FULL_BLOCK_CHAR, (int)Math.Ceiling((res.BatteryPercentage / (float)100) * 10)).PadRight(10, LIGHT_SHADE_CHAR);
                    this.LastUpdate = DateTime.Now.ToString("u");
                });
            }
            catch (ObjectDisposedException)
            {
                //noop
            }
        }

        private void OnLogOutputTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Program.LogWriter.Flush();
            StringBuilder sb = new();

            string[] loglines = Program.LogOutput.ToString().Split("\n");

            if (loglines != null && loglines.Length >= 2)
            {
                loglines.Reverse();
            }

            sb.Append(string.Concat(loglines));

            if (Program.LogOutput.Length > 5000)
            {
                Program.LogOutput.Clear();
            }

            if (_instance.IsDisposed || _instance.Disposing || !_instance.IsHandleCreated)
            {
                return;
            }

            try
            {
                _instance.BeginInvoke(() =>
                {
                    this.LogOutput = sb.ToString();
                });
            }
            catch (ObjectDisposedException)
            {
                //noop
            }
        }

        private void OnAnimationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_animationDirection)
            {
                _animationPosition++;
            }
            else
            {
                _animationPosition--;
            }

            if (_animationPosition > 16 || _animationPosition < 0)
            {
                _animationDirection ^= true;
            }

            if (_animationPosition > 16)
            {
                _animationPosition = 16;
            }

            if (_animationPosition < 0)
            {
                _animationPosition = 0;
            }

            if (_instance.IsDisposed || _instance.Disposing || !_instance.IsHandleCreated)
            {
                return;
            }

            try
            {
                _instance.BeginInvoke(() => this.AnimationLabelText = new string('-', 16).Insert(_animationPosition, (!_animationDirection ? ">" : "<")));
            }
            catch (ObjectDisposedException)
            {
                //noop
            }
        }

        [RelayCommand]
        private void ButtonToggle()
        {
            if (this.IsRunning)
            {
                this.Stop();
                return;
            }

            this.Start();
        }

        private void Start()
        {
            if (this.IsRunning)
            {
                return;
            }

            this.IsRunning = true;

            _pollingTimer.Start();
            _logOutputTimer.Start();

            this.ButtonText = BUTTON_TEXT_STOP;
        }

        private void Stop()
        {
            if (!this.IsRunning)
            {
                return;
            }

            _pollingTimer.Stop();
            _logOutputTimer.Stop();

            this.ButtonText = BUTTON_TEXT_START;
            this.BatteryPercentage = "--- %";
            this.BatteryLoad = new string(LIGHT_SHADE_CHAR, 10);
            this.ColorState = Color.Red;
            this.ControllerState = "Unknown";
            this.LastUpdate = "---";

            this.IsRunning = false;
        }

        #region Ctor
        public MainWindowViewModel(Form form)
        {
            _instance = form;

            _client = new(new SerilogLoggerFactory().CreateLogger("SteamControllerStatus"));
            _client.Initialize();

            _pollingTimer = new()
            {
                Interval = 1500
            };
            _pollingTimer.Elapsed += this.OnPollingTimerElapsed;
            

            _logOutputTimer = new()
            {
                Interval = 1000
            };
            _logOutputTimer.Elapsed += this.OnLogOutputTimerElapsed;

            _animationTimer = new()
            {
                Interval = 50
            };
            _animationTimer.Elapsed += this.OnAnimationTimerElapsed;
        }
        #endregion
    }
}
