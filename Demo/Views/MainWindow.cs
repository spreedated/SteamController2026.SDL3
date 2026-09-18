using Demo.ViewModels;
using System.Windows.Forms;

namespace Demo.Views
{
    public partial class MainWindow : Form
    {
        private readonly MainWindowViewModel _vm;

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
        }
    }
}
