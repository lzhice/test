using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ATM3300.Common;
using System.Net;
using ATM3300.Connection.Properties;
using ATM3300.Connection.Forms;

namespace ATM3300.Connection
{
    /// <summary>
    /// COMForm 的摘要说明。
    /// </summary>
    public class DataProviderTCPIPSetupForm : System.Windows.Forms.Form, IShowSetupFormInterface
    {
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        private PaneCaption paneCaption1;
        private PaneCaption paneCaption2;
        private System.Windows.Forms.Panel panel1;
        private Button button3;
        private SettingsBase Setting;
        public SettingsBase Settings
        {
            get
            {
                return this.Setting;
            }

            set
            {
                this.Setting = (Settings)value;
                if (Setting["IP"] == null) { Setting["IP"] = "0.0.0.0"; }
                if (Setting["Port"] == null) { Setting["Port"] = "15000"; }

                textBox1.Text = Setting["IP"].ToString();
                textBox2.Text = Setting["Port"].ToString();
            }

        }

        public DataProviderTCPIPSetupForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataProviderTCPIPSetupForm));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.paneCaption1 = new ATM3300.Connection.PaneCaption();
            this.paneCaption2 = new ATM3300.Connection.PaneCaption();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label2.Name = "label2";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.label1.Name = "label1";
            // 
            // paneCaption1
            // 
            this.paneCaption1.AllowActive = false;
            this.paneCaption1.AntiAlias = false;
            resources.ApplyResources(this.paneCaption1, "paneCaption1");
            this.paneCaption1.InactiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.paneCaption1.InactiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            this.paneCaption1.Name = "paneCaption1";
            // 
            // paneCaption2
            // 
            this.paneCaption2.AllowActive = false;
            this.paneCaption2.AntiAlias = false;
            resources.ApplyResources(this.paneCaption2, "paneCaption2");
            this.paneCaption2.Name = "paneCaption2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DataProviderTCPIPSetupForm
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.button2;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.paneCaption2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.paneCaption1);
            this.Name = "DataProviderTCPIPSetupForm";
            this.Load += new System.EventHandler(this.COMForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void COMForm_Load(object sender, System.EventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            int flag = 0;
            try
            {
                IPAddress.Parse(textBox1.Text);
                Setting["IP"] = textBox1.Text;
                Setting.Save();
            }
            catch
            {
                MessageBox.Show(Resources.InvalidedIPAddress);
                textBox1.Text = string.Empty;
                flag += 1;
            }

            if (Convert.ToInt32(textBox2.Text) >= 1 && Convert.ToInt32(textBox2.Text) <= 65535)
            {
                Setting["Port"] = textBox2.Text;
                Setting.Save();
            }
            else
            {
                MessageBox.Show(Resources.InvalidedIPPort);
                textBox2.Text = string.Empty;
                flag += 1;
            }
            if (flag == 0)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            TcpServer server = TcpServer.DefaultInstance;
            if ((server == null) || (!server.IsRunning))
            {
                MessageBox.Show("TCP/IP数据服务没有启动，无法查看网络连接!");
                return;
            }

            ViewTcpConversationsForm form = new ViewTcpConversationsForm(server);
            form.ShowDialog();
        }



    }
}
