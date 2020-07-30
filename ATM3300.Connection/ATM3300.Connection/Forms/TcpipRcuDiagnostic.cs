// TODO Add Humidity controls
// TODO Add Power control
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using ATM3300.Common;
using System.Collections;

namespace ATM3300.Connection.Forms
{
    public partial class TcpipRcuDiagnostic : Form
    {
        private UdpClient _UdpClient = null;
        private bool _IsRunning = false;
        private int _TotalTimes = 0;
        private int _SuccessTimes = 0;
        private int _FailedTimes = 0;
        private bool _IsScanning = false;


        public bool IsScanning
        {
            get { return _IsScanning; }
            set { 
                _IsScanning = value;
                if (_IsScanning)
                {
                    btnScan.Text = "停止查询";
                }
                else
                {
                    btnScan.Text = "开始查询";
                }
            }
        }
	

        
        public TcpipRcuDiagnostic()
        {
            InitializeComponent();

            // Init HotelUsingStatus Controls
            cmbRoomStatus.Items.Clear();
            cmbRoomStatus.Items.Add("0 - 忽略");
            foreach (object o in Enum.GetValues(typeof(HotelUsingStatusType)))
            {
                cmbRoomStatus.Items.Add(((int)o  + 1)+ " - " + o.ToString());
            }
            cmbRoomStatus.SelectedIndex = 0;

            cmbSeason.SelectedIndex = 0;
            cmbClientType.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsScanning = !IsScanning;

            if (_IsScanning)
            {
                Thread scannerThread = new Thread(new ThreadStart(ScannerEntry));
                scannerThread.IsBackground = true;
                scannerThread.Start();
            }
        }


        private void ScannerEntry()
        {
            int floorNumber = Convert.ToInt32(txtFloor.Text);
            int roomNumber = Convert.ToInt32(txtRoom.Text);
            _TotalTimes = 0;
            _SuccessTimes = 0;
            _FailedTimes = 0;


            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                        Convert.ToInt32(txtRemotePortNumber.Text));

            
            _UdpClient.Client.ReceiveTimeout = Convert.ToInt32(txtDelay.Text);

            while (true)
            {
                UpdateProgressTime();
                if (!_IsScanning)
                {
                    return;
                }

                // Start new query
                _TotalTimes++;
                byte[] bytesToSend = GenerateScanData();

                try
                {
                    _UdpClient.Send(bytesToSend, bytesToSend.Length, remoteEndPoint);
                }
                catch (SocketException )
                {
                }
                OutputDataInfo(true , bytesToSend, remoteEndPoint);

                IPEndPoint receiveEndPoint = new IPEndPoint(
                    IPAddress.Parse("127.0.0.1"), 
                    Convert.ToInt32(txtPortNumber.Text));

                byte[] receiveData = null;
                bool receivedReplyData = false;

                while (!receivedReplyData)
                {
                    try
                    {
                        receiveData = _UdpClient.Receive(ref receiveEndPoint);
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine("{1}: {0}", er.Message, er.GetType());
                        _FailedTimes++;
                        break;
                    }

                    if (receiveData != null)
                    {
                        OutputDataInfo(false, receiveData, receiveEndPoint);

                        if ((receiveData.Length == 24) &&
                            (receiveData[0] == 2) &&
                            (receiveEndPoint.Address.GetAddressBytes()[2] == floorNumber) &&
                            (receiveEndPoint.Address.GetAddressBytes()[3] == roomNumber))
                        {
                            receivedReplyData = true;
                            ApplyData(receiveData);
                        }
                    }
                }
            }
            
        }

        private void ApplyData(byte[] receiveData)
        {
            Invoke(new ThreadStart(delegate()
            {
                BitArray dataBit = new BitArray(
                    new byte[] { receiveData[3], receiveData[4] });

                txtCardType.Text = receiveData[1].ToString();
                txtCardNum.Text = receiveData[2].ToString();
                chkClean.Checked = dataBit[0];
                chkCall.Checked = dataBit[1];
                chkDontDisturb.Checked = dataBit[2];
                chkCheckout.Checked = dataBit[3];
                chkSOS.Checked = dataBit[4];
                chkCoffer.Checked = dataBit[5];
                chkRefrig.Checked = dataBit[6];
                chkDoor.Checked = dataBit[7];
                chkRepair.Checked = dataBit[8];
                chkProblem.Checked = dataBit[9];
                chkCleaningCompleted.Checked = dataBit[10];
                chkCheckRoom.Checked = dataBit[11];
                chkLightsOn.Checked = dataBit[12];
                chkKey.Checked = dataBit[13];
                chkTV.Checked = dataBit[14];

                txtClientTemp.Text = receiveData[5].ToString();
                txtTemperature.Text = receiveData[6].ToString();
                txtClientHumidity.Text = receiveData[7].ToString();
                txtHumidity.Text = receiveData[8].ToString();

                txtSpeed.Text = receiveData[9].ToString();
                txtPlayingVoice.Text = receiveData[12].ToString();

            }));
        }

        private static string ConvertByteArrayToString(byte[] revData, int index, int length)
        {
            string dataFormat = string.Empty;

            if (revData != null)
            {
                for (int k = index; k < length; k++)
                {
                    dataFormat = string.Format("{0} [{1:X}],", dataFormat, revData[k]);
                }
            }
            return dataFormat;
        }

        private byte[] GenerateScanData()
        {
            byte[] scanBytes = new byte[24];

            Invoke(new ThreadStart(delegate()
            {
                scanBytes[0] = 0x01;
                scanBytes[1] = Convert.ToByte(txtLeaveTemperature.Text);
                scanBytes[2] = Convert.ToByte(txtEmptyTemperature.Text);
                scanBytes[3] = Convert.ToByte(txtSleepTemperature.Text);
                // 4 - 6 Humidity

                scanBytes[7] = (byte)ObtainValue(cmbClientType.Text);
                scanBytes[8] = (byte)ObtainValue(cmbRoomStatus.Text);
                scanBytes[9] = Convert.ToByte(txtVoice.Text);
            }));
            return scanBytes;
        }

        private void OutputDataInfo(bool send, byte[] data, IPEndPoint endPoint)
        {
            if (send)
            {
                Console.WriteLine(
                        "Sending data to {0}:{1} - {2}",
                        endPoint.Address.ToString(),
                        endPoint.Port,
                        ConvertByteArrayToString(data, 0, data.Length));
            }
            else
            {
                Console.WriteLine(
                        "Receiving data from {0}:{1} - {2}",
                        endPoint.Address.ToString(),
                        endPoint.Port,
                        ConvertByteArrayToString(data, 0, data.Length));
            }
        }


        public void UpdateProgressTime()
        {
            Invoke(new ThreadStart(delegate()
            {
                labTotalTimes.Text = string.Format("总共次数:{0}", _TotalTimes);
                labSuccessTimes.Text = string.Format("成功次数:{0}", _SuccessTimes);
                labFailedTimes.Text = string.Format("失败次数:{0}", _TotalTimes - _SuccessTimes);
                if (_TotalTimes != 0)
                {
                    labSuccessPercent.Text = string.Format("成功比例:{0}", _SuccessTimes / (double)_TotalTimes);
                }
            }));
        }

        private void btnBroadcast_Click(object sender, EventArgs e)
        {
            byte[] bytesToSend = new byte[24];
            bytesToSend[1] = (byte)Utility.BCD(DateTime.Now.Year % 100);
            bytesToSend[2] = (byte)Utility.BCD(DateTime.Now.Month);
            bytesToSend[3] = (byte)Utility.BCD(DateTime.Now.Day);
            bytesToSend[4] = (byte)Utility.BCD(DateTime.Now.Hour);
            bytesToSend[5] = (byte)Utility.BCD(DateTime.Now.Minute);
            bytesToSend[6] = (byte)ObtainValue(cmbSeason.Text);

            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                Convert.ToInt32(txtRemotePortNumber.Text));

            try
            {
                _UdpClient.Send(bytesToSend, bytesToSend.Length, broadcastEndPoint);
            }
            catch (SocketException )
            {
            }
            OutputDataInfo(true, bytesToSend, broadcastEndPoint);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _IsRunning = !_IsRunning;
            if (_IsRunning)
            {
                try
                {
                    _UdpClient = new UdpClient(Convert.ToInt32(txtPortNumber.Text));
                    button2.Text = "关闭端口";
                }
                catch (Exception er)
                {
                    MessageBox.Show(string.Format(
                        "有错误发生，无法打开端口!" + Environment.NewLine +
                        "Error:{0}" + Environment.NewLine + "InnerError:{1}", er.ToString(), er.InnerException));
                    _IsRunning = false;
                }
            }
            else
            {
                IsScanning = false;
                _UdpClient.Close();
                button2.Text = "打开端口";
            }

            btnBroadcast.Enabled = _IsRunning;
            btnBroadcastScan.Enabled = _IsRunning;
            btnScan.Enabled = _IsRunning;
        }


        private static int ObtainValue(string s)
        {
            return Convert.ToInt32(s.Substring(0, s.IndexOf('-') - 1));
        }

        private void btnBroadcastScan_Click(object sender, EventArgs e)
        {
            byte[] bytesToSend = GenerateScanData();

            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                            Convert.ToInt32(txtRemotePortNumber.Text));

            try
            {
                _UdpClient.Send(bytesToSend, bytesToSend.Length, broadcastEndPoint);
            }
            catch (SocketException )
            {
            }
            OutputDataInfo(true, bytesToSend, broadcastEndPoint);
        }

        
    }
}