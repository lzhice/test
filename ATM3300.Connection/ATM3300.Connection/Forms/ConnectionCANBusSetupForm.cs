using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ATM3300.Common;

namespace ATM3300.Connection
{
	/// <summary>
	/// ConnectionCANBusSetupForm 的摘要说明。
	/// </summary>
	public class ConnectionCANBusSetupForm : System.Windows.Forms.Form,IShowSetupFormInterface	
	{
		private PaneCaption paneCaption3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Panel panel2;
		private PaneCaption paneCaption1;
        private PaneCaption paneCaption2;
		private System.Windows.Forms.ComboBox cmbDeviceInd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbDeviceType;
		private System.Windows.Forms.Label label1;
		private SettingsBase _Settings;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label 查询速度;
		private System.Windows.Forms.CheckBox chkDisableDetectTemperature;
		private System.Windows.Forms.Label labReceivedInterval;
		private System.Windows.Forms.TextBox txtAcceptInterval;
        private Label label3;
        private CheckedListBox chkBoxChannels;
        private TextBox txtTiming1;
        private Label label5;
        private TextBox txtTiming0;
        private Label label4;
        private TextBox txtSendTimeToATMInteval;
        private Label labSendTimeToATMInterval;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConnectionCANBusSetupForm()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionCANBusSetupForm));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtSendTimeToATMInteval = new System.Windows.Forms.TextBox();
            this.labSendTimeToATMInterval = new System.Windows.Forms.Label();
            this.txtTiming1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTiming0 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkBoxChannels = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAcceptInterval = new System.Windows.Forms.TextBox();
            this.labReceivedInterval = new System.Windows.Forms.Label();
            this.chkDisableDetectTemperature = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.查询速度 = new System.Windows.Forms.Label();
            this.cmbDeviceInd = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.paneCaption3 = new ATM3300.Connection.PaneCaption();
            this.paneCaption1 = new ATM3300.Connection.PaneCaption();
            this.paneCaption2 = new ATM3300.Connection.PaneCaption();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
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
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.txtSendTimeToATMInteval);
            this.panel2.Controls.Add(this.labSendTimeToATMInterval);
            this.panel2.Controls.Add(this.txtTiming1);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtTiming0);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.chkBoxChannels);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtAcceptInterval);
            this.panel2.Controls.Add(this.labReceivedInterval);
            this.panel2.Controls.Add(this.chkDisableDetectTemperature);
            this.panel2.Controls.Add(this.numericUpDown1);
            this.panel2.Controls.Add(this.查询速度);
            this.panel2.Controls.Add(this.cmbDeviceInd);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cmbDeviceType);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Font = null;
            this.panel2.Name = "panel2";
            // 
            // txtSendTimeToATMInteval
            // 
            this.txtSendTimeToATMInteval.AccessibleDescription = null;
            this.txtSendTimeToATMInteval.AccessibleName = null;
            resources.ApplyResources(this.txtSendTimeToATMInteval, "txtSendTimeToATMInteval");
            this.txtSendTimeToATMInteval.BackgroundImage = null;
            this.txtSendTimeToATMInteval.Font = null;
            this.txtSendTimeToATMInteval.Name = "txtSendTimeToATMInteval";
            // 
            // labSendTimeToATMInterval
            // 
            this.labSendTimeToATMInterval.AccessibleDescription = null;
            this.labSendTimeToATMInterval.AccessibleName = null;
            resources.ApplyResources(this.labSendTimeToATMInterval, "labSendTimeToATMInterval");
            this.labSendTimeToATMInterval.Font = null;
            this.labSendTimeToATMInterval.Name = "labSendTimeToATMInterval";
            // 
            // txtTiming1
            // 
            this.txtTiming1.AccessibleDescription = null;
            this.txtTiming1.AccessibleName = null;
            resources.ApplyResources(this.txtTiming1, "txtTiming1");
            this.txtTiming1.BackgroundImage = null;
            this.txtTiming1.Font = null;
            this.txtTiming1.Name = "txtTiming1";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            this.label5.Tag = "Timing0";
            // 
            // txtTiming0
            // 
            this.txtTiming0.AccessibleDescription = null;
            this.txtTiming0.AccessibleName = null;
            resources.ApplyResources(this.txtTiming0, "txtTiming0");
            this.txtTiming0.BackgroundImage = null;
            this.txtTiming0.Font = null;
            this.txtTiming0.Name = "txtTiming0";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            this.label4.Tag = "Timing0";
            // 
            // chkBoxChannels
            // 
            this.chkBoxChannels.AccessibleDescription = null;
            this.chkBoxChannels.AccessibleName = null;
            resources.ApplyResources(this.chkBoxChannels, "chkBoxChannels");
            this.chkBoxChannels.BackgroundImage = null;
            this.chkBoxChannels.Font = null;
            this.chkBoxChannels.FormattingEnabled = true;
            this.chkBoxChannels.Items.AddRange(new object[] {
            resources.GetString("chkBoxChannels.Items"),
            resources.GetString("chkBoxChannels.Items1"),
            resources.GetString("chkBoxChannels.Items2"),
            resources.GetString("chkBoxChannels.Items3"),
            resources.GetString("chkBoxChannels.Items4"),
            resources.GetString("chkBoxChannels.Items5"),
            resources.GetString("chkBoxChannels.Items6"),
            resources.GetString("chkBoxChannels.Items7")});
            this.chkBoxChannels.Name = "chkBoxChannels";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // txtAcceptInterval
            // 
            this.txtAcceptInterval.AccessibleDescription = null;
            this.txtAcceptInterval.AccessibleName = null;
            resources.ApplyResources(this.txtAcceptInterval, "txtAcceptInterval");
            this.txtAcceptInterval.BackgroundImage = null;
            this.txtAcceptInterval.Font = null;
            this.txtAcceptInterval.Name = "txtAcceptInterval";
            // 
            // labReceivedInterval
            // 
            this.labReceivedInterval.AccessibleDescription = null;
            this.labReceivedInterval.AccessibleName = null;
            resources.ApplyResources(this.labReceivedInterval, "labReceivedInterval");
            this.labReceivedInterval.Font = null;
            this.labReceivedInterval.Name = "labReceivedInterval";
            // 
            // chkDisableDetectTemperature
            // 
            this.chkDisableDetectTemperature.AccessibleDescription = null;
            this.chkDisableDetectTemperature.AccessibleName = null;
            resources.ApplyResources(this.chkDisableDetectTemperature, "chkDisableDetectTemperature");
            this.chkDisableDetectTemperature.BackgroundImage = null;
            this.chkDisableDetectTemperature.Font = null;
            this.chkDisableDetectTemperature.Name = "chkDisableDetectTemperature";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.AccessibleDescription = null;
            this.numericUpDown1.AccessibleName = null;
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Font = null;
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // 查询速度
            // 
            this.查询速度.AccessibleDescription = null;
            this.查询速度.AccessibleName = null;
            resources.ApplyResources(this.查询速度, "查询速度");
            this.查询速度.Font = null;
            this.查询速度.Name = "查询速度";
            this.查询速度.Click += new System.EventHandler(this.label4_Click);
            // 
            // cmbDeviceInd
            // 
            this.cmbDeviceInd.AccessibleDescription = null;
            this.cmbDeviceInd.AccessibleName = null;
            resources.ApplyResources(this.cmbDeviceInd, "cmbDeviceInd");
            this.cmbDeviceInd.BackgroundImage = null;
            this.cmbDeviceInd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeviceInd.Font = null;
            this.cmbDeviceInd.Items.AddRange(new object[] {
            resources.GetString("cmbDeviceInd.Items"),
            resources.GetString("cmbDeviceInd.Items1"),
            resources.GetString("cmbDeviceInd.Items2"),
            resources.GetString("cmbDeviceInd.Items3"),
            resources.GetString("cmbDeviceInd.Items4"),
            resources.GetString("cmbDeviceInd.Items5"),
            resources.GetString("cmbDeviceInd.Items6"),
            resources.GetString("cmbDeviceInd.Items7")});
            this.cmbDeviceInd.Name = "cmbDeviceInd";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.AccessibleDescription = null;
            this.cmbDeviceType.AccessibleName = null;
            resources.ApplyResources(this.cmbDeviceType, "cmbDeviceType");
            this.cmbDeviceType.BackgroundImage = null;
            this.cmbDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeviceType.Font = null;
            this.cmbDeviceType.Items.AddRange(new object[] {
            resources.GetString("cmbDeviceType.Items"),
            resources.GetString("cmbDeviceType.Items1"),
            resources.GetString("cmbDeviceType.Items2"),
            resources.GetString("cmbDeviceType.Items3"),
            resources.GetString("cmbDeviceType.Items4"),
            resources.GetString("cmbDeviceType.Items5"),
            resources.GetString("cmbDeviceType.Items6"),
            resources.GetString("cmbDeviceType.Items7")});
            this.cmbDeviceType.Name = "cmbDeviceType";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
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
            // ConnectionCANBusSetupForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.paneCaption3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.paneCaption1);
            this.Controls.Add(this.paneCaption2);
            this.Font = null;
            this.Icon = null;
            this.Name = "ConnectionCANBusSetupForm";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			_Settings["DeviceType"] = cmbDeviceType.SelectedIndex+1;
			_Settings["DeviceInd"] = cmbDeviceInd.SelectedIndex;
			//mSettings["CANInd"] = cmbCanInd.SelectedIndex;
			_Settings["ScanDelay"] = numericUpDown1.Value;
			//mSettings["MultiChannel"] = chkMultiChannel.Checked;
			_Settings["DisableDetectTemperature"] = chkDisableDetectTemperature.Checked;
			_Settings["AcceptInterval"] = Convert.ToInt32(txtAcceptInterval.Text);
            _Settings["SendTimeToATMFrequency"] = Convert.ToInt32(txtSendTimeToATMInteval.Text);
            _Settings["Timing0"] = txtTiming0.Text;
            _Settings["Timing1"] = txtTiming1.Text;
            
            // Save Channels Settings as string
            string chn = string.Empty;

            for (int i = 0; i < chkBoxChannels.CheckedIndices.Count; i++)
            {
                if (i != 0)
                {
                    chn += ";";
                }
                chn += chkBoxChannels.CheckedIndices[i].ToString();
            }

            _Settings["Channels"] = chn;
            
			this.DialogResult=DialogResult.OK;
			Close();
		}

		private void label4_Click(object sender, System.EventArgs e)
		{
		
		}

		private void chkMultiChannel_CheckedChanged(object sender, System.EventArgs e)
		{
			//cmbCanInd.Enabled = !chkMultiChannel.Checked;
		}

		#region IShowSetupFormInterface 成员

		public ATM3300.Common.SettingsBase Settings
		{
			get
			{
				return this._Settings;
			}
			set
			{
				this._Settings=value;

				//Apply Option To UI
				cmbDeviceType.SelectedIndex = (int)_Settings["DeviceType",1] - 1;
				cmbDeviceInd.SelectedIndex = (int)_Settings["DeviceInd",0];
				//cmbCanInd.SelectedIndex = (int)mSettings["CANInd",0];
				numericUpDown1.Value = Convert.ToInt32(_Settings["ScanDelay",1000]);		//milliseconds = 1 seconds
				//chkMultiChannel.Checked = Convert.ToBoolean(mSettings["MultiChannel", false]);

                // Paser Channels Data
                string[] chns = _Settings["Channels" , string.Empty].ToString().Split(';');

                for (int i = 0; i < chns.Length; i++)
                {
                    try
                    {
                        chkBoxChannels.SetItemChecked(Convert.ToInt32(chns[i]),true);
                    }
                    catch
                    {
                    }
                }

				chkDisableDetectTemperature.Checked = Convert.ToBoolean(_Settings["DisableDetectTemperature" , false]);
				txtAcceptInterval.Text = _Settings["AcceptInterval" ,1].ToString();
                txtSendTimeToATMInteval.Text = _Settings["SendTimeToATMFrequency", 5].ToString();
                txtTiming0.Text = _Settings["Timing0", 0x67].ToString();
                txtTiming1.Text = _Settings["Timing1", 0x2f].ToString();
			}
		}

		#endregion
	}
}
