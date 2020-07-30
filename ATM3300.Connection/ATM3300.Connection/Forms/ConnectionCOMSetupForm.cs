using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ATM3300.Common;
using ATM3300.Connection.Properties;
using System.IO.Ports;


namespace ATM3300.Connection
{
    /// <summary>
    /// COMSetupForm 的摘要说明。
    /// </summary>
    public class ConnectionCOMSetupForm : System.Windows.Forms.Form, IShowSetupFormInterface
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.ComboBox comboBox1;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private PaneCaption paneCaption1;
        private PaneCaption paneCaption2;
        private System.Windows.Forms.Panel panel2;
        private PaneCaption paneCaption3;
        private System.Windows.Forms.ComboBox comboBox2;


        private SettingsBase Setting;

        public SettingsBase Settings
        {
            get { return this.Setting; }
            set
            {
                this.Setting = (Settings)value;

                //检查看看选项是否存在
                if (this.Setting["CommPort"] == null) this.Setting["CommPort"] = 1;
                if (this.Setting["Settings"] == null) this.Setting["Settings"] = "2400,e,8,1";
                if (this.Setting["ReceiveDataRepeatTimes"] == null) this.Setting["ReceiveDataRepeatTimes"] = 5;
                if (this.Setting["ReceiveDataCheckDelay"] == null) this.Setting["ReceiveDataCheckDelay"] = 50;
                if (this.Setting["ReceiveDataCheckRepeatTimes"] == null) this.Setting["ReceiveDataCheckRepeatTimes"] = 5;
                if (this.Setting["DisconnectRetryTimes"] == null) this.Setting["DisconnectRetryTimes"] = 3;
                //if (this.Setting["WhetherSendTimeToATM"]==null)	this.Setting["WhetherSendTimeToATM"]="True";
                if (this.Setting["SendTimeToATMFrequency"] == null) this.Setting["SendTimeToATMFrequency"] = 5;

                numericUpDown1.Value = Convert.ToInt32(Setting["ReceiveDataRepeatTimes"].ToString());
                numericUpDown2.Value = Convert.ToInt32(Setting["ReceiveDataCheckDelay"].ToString());
                numericUpDown3.Value = Convert.ToInt32(Setting["DisconnectRetryTimes"].ToString());
                numericUpDown4.Value = Convert.ToInt32(Setting["ReceiveDataCheckRepeatTimes"].ToString());
                comboBox2.Text = Setting["Settings"].ToString().Replace(",e,8,1", "");
                
                // Setup comboBox1
                this.comboBox1.Text = comboBox1.Items[Convert.ToInt32(Setting["CommPort"]) - 1].ToString();
            }

        }

        public ConnectionCOMSetupForm()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();

            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //Setting["ReceiveDataRepeatTimes"]=numericUpDown1.Value.ToString()

        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionCOMSetupForm));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.paneCaption1 = new ATM3300.Connection.PaneCaption();
            this.paneCaption2 = new ATM3300.Connection.PaneCaption();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.paneCaption3 = new ATM3300.Connection.PaneCaption();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.AccessibleDescription = null;
            this.button1.AccessibleName = null;
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackgroundImage = null;
            this.button1.Font = null;
            this.button1.Name = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AccessibleDescription = null;
            this.button2.AccessibleName = null;
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackgroundImage = null;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = null;
            this.button2.Name = "button2";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.AccessibleDescription = null;
            this.numericUpDown1.AccessibleName = null;
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.AccessibleDescription = null;
            this.numericUpDown2.AccessibleName = null;
            resources.ApplyResources(this.numericUpDown2, "numericUpDown2");
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.AccessibleDescription = null;
            this.numericUpDown3.AccessibleName = null;
            resources.ApplyResources(this.numericUpDown3, "numericUpDown3");
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBox1
            // 
            this.comboBox1.AccessibleDescription = null;
            this.comboBox1.AccessibleName = null;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.BackgroundImage = null;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1"),
            resources.GetString("comboBox1.Items2"),
            resources.GetString("comboBox1.Items3"),
            resources.GetString("comboBox1.Items4"),
            resources.GetString("comboBox1.Items5"),
            resources.GetString("comboBox1.Items6"),
            resources.GetString("comboBox1.Items7"),
            resources.GetString("comboBox1.Items8")});
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AccessibleDescription = null;
            this.label6.AccessibleName = null;
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.AccessibleDescription = null;
            this.numericUpDown4.AccessibleName = null;
            resources.ApplyResources(this.numericUpDown4, "numericUpDown4");
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // paneCaption1
            // 
            this.paneCaption1.AccessibleDescription = null;
            this.paneCaption1.AccessibleName = null;
            this.paneCaption1.AllowActive = false;
            resources.ApplyResources(this.paneCaption1, "paneCaption1");
            this.paneCaption1.AntiAlias = false;
            this.paneCaption1.BackColor = System.Drawing.Color.Black;
            this.paneCaption1.BackgroundImage = null;
            this.paneCaption1.InactiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.paneCaption1.InactiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            this.paneCaption1.Name = "paneCaption1";
            this.paneCaption1.Load += new System.EventHandler(this.paneCaption1_Load);
            // 
            // paneCaption2
            // 
            this.paneCaption2.AccessibleDescription = null;
            this.paneCaption2.AccessibleName = null;
            this.paneCaption2.AllowActive = false;
            resources.ApplyResources(this.paneCaption2, "paneCaption2");
            this.paneCaption2.AntiAlias = false;
            this.paneCaption2.BackgroundImage = null;
            this.paneCaption2.ForeColor = System.Drawing.Color.Transparent;
            this.paneCaption2.InactiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.paneCaption2.InactiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            this.paneCaption2.Name = "paneCaption2";
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Font = null;
            this.panel2.Name = "panel2";
            // 
            // comboBox2
            // 
            this.comboBox2.AccessibleDescription = null;
            this.comboBox2.AccessibleName = null;
            resources.ApplyResources(this.comboBox2, "comboBox2");
            this.comboBox2.BackgroundImage = null;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Items.AddRange(new object[] {
            resources.GetString("comboBox2.Items"),
            resources.GetString("comboBox2.Items1"),
            resources.GetString("comboBox2.Items2"),
            resources.GetString("comboBox2.Items3"),
            resources.GetString("comboBox2.Items4"),
            resources.GetString("comboBox2.Items5"),
            resources.GetString("comboBox2.Items6"),
            resources.GetString("comboBox2.Items7"),
            resources.GetString("comboBox2.Items8"),
            resources.GetString("comboBox2.Items9"),
            resources.GetString("comboBox2.Items10"),
            resources.GetString("comboBox2.Items11"),
            resources.GetString("comboBox2.Items12"),
            resources.GetString("comboBox2.Items13"),
            resources.GetString("comboBox2.Items14"),
            resources.GetString("comboBox2.Items15"),
            resources.GetString("comboBox2.Items16"),
            resources.GetString("comboBox2.Items17")});
            this.comboBox2.Name = "comboBox2";
            // 
            // paneCaption3
            // 
            this.paneCaption3.AccessibleDescription = null;
            this.paneCaption3.AccessibleName = null;
            this.paneCaption3.AllowActive = false;
            resources.ApplyResources(this.paneCaption3, "paneCaption3");
            this.paneCaption3.AntiAlias = false;
            this.paneCaption3.BackgroundImage = null;
            this.paneCaption3.Name = "paneCaption3";
            this.paneCaption3.Load += new System.EventHandler(this.paneCaption3_Load);
            // 
            // ConnectionCOMSetupForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.paneCaption3);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.paneCaption1);
            this.Controls.Add(this.paneCaption2);
            this.Font = null;
            this.Icon = null;
            this.Name = "ConnectionCOMSetupForm";
            this.Load += new System.EventHandler(this.COMSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void COMSetupForm_Load(object sender, System.EventArgs e)
        {

        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            int flag = 0;

            Setting["Settings"] = comboBox2.Text + ",e,8,1";
            Setting.Save();

            Setting["CommPort"] = comboBox1.Text.ToString().Substring(3, 1);

            if (numericUpDown1.Value != 0)
            {
                Setting["ReceiveDataRepeatTimes"] = numericUpDown1.Value.ToString();
            }
            else
            {
                MessageBox.Show(Resources.ConnectionCOMSetupFormCheckTimesCouldNotBeZero);
                flag += 1;
            }

            if (numericUpDown2.Value != 0)
            {
                Setting["ReceiveDataCheckDelay"] = numericUpDown2.Value.ToString();
            }
            else
            {
                MessageBox.Show(Resources.ConnectionCOMSetupFormCheckDelayCouldNotBeZero);
                flag += 1;
            }

            if (numericUpDown3.Value != 0)
            {
                Setting["DisconnectRetryTimes"] = numericUpDown3.Value.ToString();
            }
            else
            {
                MessageBox.Show(Resources.ConnectionCOMSetupFormDisconnectCouldNotBeZero);
                flag += 1;
            }

            if (numericUpDown4.Value != 0)
            {
                Setting["ReceiveDataCheckRepeatTimes"] = numericUpDown4.Value.ToString();
            }
            else
            {
                MessageBox.Show(Resources.ConnectionCOMSetupFormCheckRepeatCouldNotBeZero);
                flag += 1;
            }

            if (flag == 0)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void paneCaption1_Load(object sender, System.EventArgs e)
        {

        }

        private void paneCaption3_Load(object sender, EventArgs e)
        {

        }
    }
}
