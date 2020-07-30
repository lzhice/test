using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ATM3300.Common;

namespace ATM3300.Connection.Forms
{
    public partial class TcpipRcuSetupForm : Form, IShowSetupFormInterface
    {
        private SettingsBase _Settings = null;
        
        public TcpipRcuSetupForm()
        {
            InitializeComponent();
        }

        private void TcpipRcuSetupForm_Load(object sender, EventArgs e)
        {

        }

        #region IShowSetupFormInterface Members

        public ATM3300.Common.SettingsBase Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
                if (_Settings != null)
                {
                    numericUpDown3.Value = ParseSettings("RetryTimesBeforeDisconnect", 3);
                    numericUpDown1.Value = ParseSettings("RetryTimesEachScan", 3);
                    txtNetworkID1.Text = _Settings["NetworkID1",80].ToString();
                    txtNetworkID2.Text = _Settings["NetworkID2",80].ToString();
                    txtPortNumber.Text = _Settings["LocalPort", 20000].ToString();
                    txtRemotePortNumber.Text = _Settings["RemotePort", 20001].ToString();
                    txtDelay.Text = _Settings["ScanTimeout", 50].ToString();
                }
            }
        }

        private int ParseSettings(string key, int defaultValue)
        {
            int v = 0;
            int.TryParse(_Settings[key, defaultValue].ToString(),
                out v);
            return v;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            _Settings["RetryTimesBeforeDisconnect"] = numericUpDown3.Value;
            _Settings["RetryTimesEachScan"] = numericUpDown1.Value;
            _Settings["NetworkID1"] = txtNetworkID1.Text;
            _Settings["NetworkID2"] = txtNetworkID2.Text;
            _Settings["LocalPort"] = txtPortNumber.Text;
            _Settings["RemotePort"] = txtRemotePortNumber.Text;
            _Settings["ScanTimeout"] = txtDelay.Text;
            
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}