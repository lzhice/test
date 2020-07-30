namespace ATM3300.Connection.Forms
{
    partial class ViewTcpConversationsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewTcpConversationsForm));
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.lstvwConversations = new System.Windows.Forms.ListView();
            this.clnHdrIPAddress = new System.Windows.Forms.ColumnHeader();
            this.clnHdrPort = new System.Windows.Forms.ColumnHeader();
            this.clnHdrCreateTime = new System.Windows.Forms.ColumnHeader();
            this.clnHdrLastActivityTime = new System.Windows.Forms.ColumnHeader();
            this.paneCaption2 = new ATM3300.Connection.PaneCaption();
            this.paneCaption1 = new ATM3300.Connection.PaneCaption();
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
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.lstvwConversations);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // button2
            // 
            this.button2.AccessibleDescription = null;
            this.button2.AccessibleName = null;
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackgroundImage = null;
            this.button2.Font = null;
            this.button2.Name = "button2";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lstvwConversations
            // 
            this.lstvwConversations.AccessibleDescription = null;
            this.lstvwConversations.AccessibleName = null;
            resources.ApplyResources(this.lstvwConversations, "lstvwConversations");
            this.lstvwConversations.BackgroundImage = null;
            this.lstvwConversations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clnHdrIPAddress,
            this.clnHdrPort,
            this.clnHdrCreateTime,
            this.clnHdrLastActivityTime});
            this.lstvwConversations.Font = null;
            this.lstvwConversations.FullRowSelect = true;
            this.lstvwConversations.Name = "lstvwConversations";
            this.lstvwConversations.UseCompatibleStateImageBehavior = false;
            this.lstvwConversations.View = System.Windows.Forms.View.Details;
            // 
            // clnHdrIPAddress
            // 
            resources.ApplyResources(this.clnHdrIPAddress, "clnHdrIPAddress");
            // 
            // clnHdrPort
            // 
            resources.ApplyResources(this.clnHdrPort, "clnHdrPort");
            // 
            // clnHdrCreateTime
            // 
            resources.ApplyResources(this.clnHdrCreateTime, "clnHdrCreateTime");
            // 
            // clnHdrLastActivityTime
            // 
            resources.ApplyResources(this.clnHdrLastActivityTime, "clnHdrLastActivityTime");
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
            // ViewTcpConversationsForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.paneCaption2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.paneCaption1);
            this.Font = null;
            this.Icon = null;
            this.Name = "ViewTcpConversationsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ViewTcpConversationsForm_FormClosed);
            this.Load += new System.EventHandler(this.ViewTcpConversationsForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PaneCaption paneCaption1;
        private PaneCaption paneCaption2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lstvwConversations;
        private System.Windows.Forms.ColumnHeader clnHdrIPAddress;
        private System.Windows.Forms.ColumnHeader clnHdrPort;
        private System.Windows.Forms.ColumnHeader clnHdrCreateTime;
        private System.Windows.Forms.ColumnHeader clnHdrLastActivityTime;
    }
}