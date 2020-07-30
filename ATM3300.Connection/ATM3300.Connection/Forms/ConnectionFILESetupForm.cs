using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ATM3300.Common;

namespace ATM3300.Connection
{
	/// <summary>
	/// FILEForm 的摘要说明。
	/// </summary>
	public class ConnectionFILESetupForm : System.Windows.Forms.Form,IShowSetupFormInterface
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private PaneCaption paneCaption1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox textBox1;
		private PaneCaption paneCaption2;
		private PaneCaption paneCaption3;
		private PaneCaption paneCaption4;
		private SettingsBase Setting;
		public SettingsBase Settings
		{
			get{return this.Setting;}
			set
			{
				this.Setting=(Settings)value;
				if (Setting["FileName"]==null)
					Setting["FileName"]="C:\\Room.inout";
				textBox1.Text=Setting["FileName"].ToString();
			}

		}

		public ConnectionFILESetupForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionFILESetupForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.paneCaption3 = new ATM3300.Connection.PaneCaption();
            this.paneCaption4 = new ATM3300.Connection.PaneCaption();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // textBox1
            // 
            this.textBox1.AccessibleDescription = null;
            this.textBox1.AccessibleName = null;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.BackgroundImage = null;
            this.textBox1.Name = "textBox1";
            // 
            // paneCaption3
            // 
            this.paneCaption3.AccessibleDescription = null;
            this.paneCaption3.AccessibleName = null;
            this.paneCaption3.AllowActive = false;
            resources.ApplyResources(this.paneCaption3, "paneCaption3");
            this.paneCaption3.AntiAlias = false;
            this.paneCaption3.BackgroundImage = null;
            this.paneCaption3.InactiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.paneCaption3.InactiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            this.paneCaption3.Name = "paneCaption3";
            this.paneCaption3.Load += new System.EventHandler(this.paneCaption3_Load);
            // 
            // paneCaption4
            // 
            this.paneCaption4.AccessibleDescription = null;
            this.paneCaption4.AccessibleName = null;
            this.paneCaption4.AllowActive = false;
            resources.ApplyResources(this.paneCaption4, "paneCaption4");
            this.paneCaption4.AntiAlias = false;
            this.paneCaption4.BackgroundImage = null;
            this.paneCaption4.Name = "paneCaption4";
            // 
            // ConnectionFILESetupForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.paneCaption4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.paneCaption3);
            this.Font = null;
            this.Icon = null;
            this.Name = "ConnectionFILESetupForm";
            this.Load += new System.EventHandler(this.FILEForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void FILEForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Setting["FileName"]=textBox1.Text;
			this.DialogResult=DialogResult.OK;
			Close();

		}

		private void paneCaption3_Load(object sender, System.EventArgs e)
		{
		
		}
	
		

	}
}
