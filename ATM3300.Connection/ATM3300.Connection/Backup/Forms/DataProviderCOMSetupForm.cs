using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ATM3300.Common;

namespace ATM3300.Connection
{
	/// <summary>
	/// FILESetupForm 的摘要说明。
	/// </summary>
	public class DataProviderCOMSetupForm : System.Windows.Forms.Form,IShowSetupFormInterface
	{
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private PaneCaption paneCaption1;
		private System.Windows.Forms.Panel panel1;
		private PaneCaption paneCaption2;
		private System.Windows.Forms.ComboBox comboBox2;
		private SettingsBase Setting;
		public SettingsBase Settings
		{
			get{return this.Setting;}
			set
			{
				this.Setting=(Settings)value;
				if (Setting["CommPort"]==null)	Setting["CommPort"]=1;
				if (Setting["Settings"]==null)	Setting["Settings"]="2400,e,8,1";
				if (Setting["Timeout"]==null)	Setting["Timeout"]=100;

				comboBox2.Text=Setting["Settings"].ToString().Replace(",e,8,1","");
				numericUpDown2.Value=Convert.ToInt32(Setting["Timeout"].ToString());
				this.comboBox1.Text=comboBox1.Items[Convert.ToInt32(Setting["CommPort"])-1].ToString();
			}

		}

		public DataProviderCOMSetupForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataProviderCOMSetupForm));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.paneCaption1 = new ATM3300.Connection.PaneCaption();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.paneCaption2 = new ATM3300.Connection.PaneCaption();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.AccessibleDescription = null;
            this.numericUpDown2.AccessibleName = null;
            resources.ApplyResources(this.numericUpDown2, "numericUpDown2");
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
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label4.Name = "label4";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label2.Name = "label2";
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
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.AccessibleDescription = null;
            this.button1.AccessibleName = null;
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackgroundImage = null;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = null;
            this.button1.Name = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label1.Name = "label1";
            // 
            // paneCaption1
            // 
            this.paneCaption1.AccessibleDescription = null;
            this.paneCaption1.AccessibleName = null;
            this.paneCaption1.AllowActive = false;
            resources.ApplyResources(this.paneCaption1, "paneCaption1");
            this.paneCaption1.AntiAlias = false;
            this.paneCaption1.BackgroundImage = null;
            this.paneCaption1.InactiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.paneCaption1.InactiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            this.paneCaption1.Name = "paneCaption1";
            this.paneCaption1.Load += new System.EventHandler(this.paneCaption1_Load);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
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
            // paneCaption2
            // 
            this.paneCaption2.AccessibleDescription = null;
            this.paneCaption2.AccessibleName = null;
            this.paneCaption2.AllowActive = false;
            resources.ApplyResources(this.paneCaption2, "paneCaption2");
            this.paneCaption2.AntiAlias = false;
            this.paneCaption2.BackgroundImage = null;
            this.paneCaption2.Name = "paneCaption2";
            // 
            // DataProviderCOMSetupForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.CancelButton = this.button2;
            this.Controls.Add(this.paneCaption2);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.paneCaption1);
            this.Font = null;
            this.Icon = null;
            this.Name = "DataProviderCOMSetupForm";
            this.Load += new System.EventHandler(this.FILESetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			int flag=0;
			
				Setting["Settings"]=comboBox2.Text+",e,8,1";
				Setting.Save();
			
			
			Setting["CommPort"]=comboBox1.Text.ToString().Substring(3,1);
						
			
			if(numericUpDown2.Value!=0)
			{
				Setting["Timeout"]=numericUpDown2.Value.ToString();
			}
			if(flag==0)
			{
				this.DialogResult=DialogResult.OK;
				Close();
			}
		}

		private void FILESetupForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private void label4_Click(object sender, System.EventArgs e)
		{
		
		}

		private void paneCaption1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
		
		}
	

	}
}
