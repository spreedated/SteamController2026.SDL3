using System.Drawing;
using System.Windows.Forms;

namespace Demo.Views
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.Pbx_ConnectionState = new PictureBox();
            this.groupBox1 = new GroupBox();
            this.Lbl_State = new Label();
            this.groupBox2 = new GroupBox();
            this.Lbl_Load = new Label();
            this.Lbl_Percentage = new Label();
            this.Lbl_LastUpdateTime = new Label();
            this.Rtb_Log = new RichTextBox();
            this.groupBox3 = new GroupBox();
            this.Btn_Toggle = new Button();
            this.Lbl_Animation = new Label();
            ((System.ComponentModel.ISupportInitialize)this.Pbx_ConnectionState).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(209, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(253, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "You need the actual Hardware";
            // 
            // Pbx_ConnectionState
            // 
            this.Pbx_ConnectionState.Location = new Point(6, 24);
            this.Pbx_ConnectionState.Name = "Pbx_ConnectionState";
            this.Pbx_ConnectionState.Size = new Size(16, 16);
            this.Pbx_ConnectionState.TabIndex = 1;
            this.Pbx_ConnectionState.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lbl_State);
            this.groupBox1.Controls.Add(this.Pbx_ConnectionState);
            this.groupBox1.Location = new Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(161, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection state";
            // 
            // Lbl_State
            // 
            this.Lbl_State.AutoSize = true;
            this.Lbl_State.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Lbl_State.Location = new Point(28, 24);
            this.Lbl_State.Name = "Lbl_State";
            this.Lbl_State.Size = new Size(78, 16);
            this.Lbl_State.TabIndex = 2;
            this.Lbl_State.Text = "###state###";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.groupBox2.Controls.Add(this.Lbl_Load);
            this.groupBox2.Controls.Add(this.Lbl_Percentage);
            this.groupBox2.Location = new Point(179, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(269, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Battery";
            // 
            // Lbl_Load
            // 
            this.Lbl_Load.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Lbl_Load.Font = new Font("Monotxt_IV25", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Lbl_Load.Location = new Point(6, 34);
            this.Lbl_Load.Name = "Lbl_Load";
            this.Lbl_Load.Size = new Size(257, 29);
            this.Lbl_Load.TabIndex = 1;
            this.Lbl_Load.Text = "██████████";
            this.Lbl_Load.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Lbl_Percentage
            // 
            this.Lbl_Percentage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.Lbl_Percentage.AutoSize = true;
            this.Lbl_Percentage.Location = new Point(194, 76);
            this.Lbl_Percentage.Name = "Lbl_Percentage";
            this.Lbl_Percentage.Size = new Size(69, 13);
            this.Lbl_Percentage.TabIndex = 0;
            this.Lbl_Percentage.Text = "###load###";
            // 
            // Lbl_LastUpdateTime
            // 
            this.Lbl_LastUpdateTime.AutoSize = true;
            this.Lbl_LastUpdateTime.Location = new Point(12, 9);
            this.Lbl_LastUpdateTime.Name = "Lbl_LastUpdateTime";
            this.Lbl_LastUpdateTime.Size = new Size(35, 13);
            this.Lbl_LastUpdateTime.TabIndex = 4;
            this.Lbl_LastUpdateTime.Text = "label2";
            // 
            // Rtb_Log
            // 
            this.Rtb_Log.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Rtb_Log.BorderStyle = BorderStyle.None;
            this.Rtb_Log.Location = new Point(6, 19);
            this.Rtb_Log.Name = "Rtb_Log";
            this.Rtb_Log.Size = new Size(432, 135);
            this.Rtb_Log.TabIndex = 5;
            this.Rtb_Log.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.groupBox3.Controls.Add(this.Rtb_Log);
            this.groupBox3.Location = new Point(18, 145);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(444, 160);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // Btn_Toggle
            // 
            this.Btn_Toggle.Location = new Point(34, 95);
            this.Btn_Toggle.Name = "Btn_Toggle";
            this.Btn_Toggle.Size = new Size(115, 23);
            this.Btn_Toggle.TabIndex = 7;
            this.Btn_Toggle.Text = "&Start polling";
            this.Btn_Toggle.UseVisualStyleBackColor = true;
            // 
            // Lbl_Animation
            // 
            this.Lbl_Animation.Font = new Font("Monotxt_IV25", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Lbl_Animation.Location = new Point(12, 121);
            this.Lbl_Animation.Name = "Lbl_Animation";
            this.Lbl_Animation.Size = new Size(161, 13);
            this.Lbl_Animation.TabIndex = 8;
            this.Lbl_Animation.Text = "---";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(474, 317);
            this.Controls.Add(this.Lbl_Animation);
            this.Controls.Add(this.Btn_Toggle);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.Lbl_LastUpdateTime);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimumSize = new Size(490, 356);
            this.Name = "MainWindow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Steam Controller 2026 - Demo";
            ((System.ComponentModel.ISupportInitialize)this.Pbx_ConnectionState).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox Pbx_ConnectionState;
        private GroupBox groupBox1;
        private Label Lbl_State;
        private GroupBox groupBox2;
        private Label Lbl_Percentage;
        private Label Lbl_Load;
        private Label Lbl_LastUpdateTime;
        private RichTextBox Rtb_Log;
        private GroupBox groupBox3;
        private Button Btn_Toggle;
        private Label Lbl_Animation;
    }
}
