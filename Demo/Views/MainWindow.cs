using Demo.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Views
{
    public partial class MainWindow : Form
    {
        private readonly MainWindowViewModel _vm;
        bool closing = false;
        bool cleanupFinished = false;

        public MainWindow()
        {
            this.InitializeComponent();
            _vm = new(this);

            this.Lbl_State.DataBindings.Add("Text", _vm, "ControllerState");
            this.Pbx_ConnectionState.DataBindings.Add("BackColor", _vm, "ColorState");
            this.Lbl_Load.DataBindings.Add("Text", _vm, "BatteryLoad");
            this.Lbl_Percentage.DataBindings.Add("Text", _vm, "BatteryPercentage");
            this.Lbl_LastUpdateTime.DataBindings.Add("Text", _vm, "LastUpdate");
            this.Rtb_Log.DataBindings.Add("Text", _vm, "LogOutput");
            this.Btn_Toggle.DataBindings.Add("Text", _vm, "ButtonText");
            this.Btn_Toggle.Command = _vm.ButtonToggleCommand;
            this.Lbl_Animation.DataBindings.Add("Text", _vm, "AnimationLabelText");

            base.FormClosing += this.MainWindow_FormClosing;
        }

        private async void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanupFinished)
                return;

            e.Cancel = true;

            if (closing)
                return;

            closing = true;
            this.Enabled = false;

            try
            {
                await _vm.ShutdownAsync();

                cleanupFinished = true;
                this.BeginInvoke(new Action(base.Close));
            }
            catch (Exception ex)
            {
                closing = false;
                this.Enabled = true;
                MessageBox.Show(this, ex.Message, "Shutdown failed");
            }
        }
    }
}
