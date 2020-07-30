#region Version Information
//Version alpha1
//Written BY ZQ
//The first version of ScannerCOM
//Need Real ATM to Test
//Some Different with Old ATM3300(see GetCOMInf()).Don't send AirCon and RoomStatus to ATM
//Has Get and Check Data Retry Disconnect and Send Time to ATM and Apply Data
//Maybe has some bug
//add a CurrentScanRoomChanged Event
//[3/27/2004]
//Version alpha2
//Add HotelUsingStatus
//Use Utility to Check Sum
//Written BY ZQ
//[3/28/2004]
//Version alpha3
//The list of test has pass
//	Door Key GuestService Temperature CutOff AirConSpeed 
//Fix a bug for ScannerTimeOut and Add a Delay between two room scan
//Written BY ZQ
//Date [4/25/2004]
//Version alhpa4
//Fix a bug for MultiGuestService
//Date  [5/15/2004]
//Edit BY ZQ
//Fix a bug for connect,disconnect
//EDIT BY ZQ
//Date  [5/28/2004]
//Fix a bug when port has been used (connect)
//Date  [5/29/2004]
//EDIT BY ZQ
//Use RS232.Rs232 Class to Instead Of MSCommClass(COM Interop)
//DATA [1/12/2004]
//EDIT BY ZQ
#endregion
using System;
using System.Threading;
using System.Collections;
using ATM3300.Common;
using ATM3300.Connection;
using System.Windows.Forms;
#if RS2323party
using RS232;
#endif
using System.IO.Ports;
using System.Diagnostics;
using System.Collections.Generic;

namespace ATM3300.Connection
{

    /// <summary>
    /// ScannerCOM 的摘要说明。
    /// </summary>
    [ConnectionVersionInfo("标准串口连接协议",
                "{3F5F27CB-A661-4882-A29C-DD99E456CAC0}",
                "1.0.2",
                "通过标准串口获取房间信息。", typeof(ConnectionCOMSetupForm))]
    public class ConnectionScannerCOM : ConnectionScannerBase
    {
        public event System.EventHandler CurrentScanRoomChanged;

        //protected MSCommClass mComm=new MSCommClass();

        protected SerialPort _Port = new SerialPort();

        protected int _ReceiveDataCheckRepeatTimes = 3;
        protected int _CurrentReceiveDataCheckRepeatTimes = 0;
        protected int _ReceiveDataCheckDelay = 50; //millionseconds

        protected int _ReceiveDataRepeatTimes = 5;
        protected int _CurrentReceiveDataRepeatTimes = 0;

        protected byte[] _ReceiveCheckData = new byte[5];

        protected int _DisconnectRetryTimes = 3;
        protected Hashtable _DisconnectList = new Hashtable();
        protected System.Timers.Timer _SendTimeToATMTimer = new System.Timers.Timer();

        protected bool _WhetherSendTimeToATM = true;
        protected int _SendTimeToATMFrequency = 30;	//seconds

        protected Thread _QueryThread = null;

        private bool PaserSettings(string Settings)
        {
            string[] SStr = Settings.Split(',');

            Dictionary<string, Parity> pcov = new Dictionary<string, Parity>();
            pcov.Add("n", Parity.None);
            pcov.Add("e", Parity.Even);
            pcov.Add("o", Parity.Odd);
            pcov.Add("s", Parity.Space);
            pcov.Add("m", Parity.Mark);

            try
            {
                _Port.BaudRate = Convert.ToInt32(SStr[0]);
                if (pcov.ContainsKey(SStr[1]))
                {
                    _Port.Parity = pcov[SStr[1]];
                }
                else
                {
                    _Port.Parity = Parity.Even;
                }

                _Port.DataBits = Convert.ToInt32(SStr[2]);
                double v = Convert.ToDouble(SStr[3]);
                if (v == 1)
                {
                    _Port.StopBits = StopBits.One;
                }
                else if (v == 2)
                {
                    _Port.StopBits = StopBits.Two;
                }
                else if (v == 1.5)
                {
                    _Port.StopBits = StopBits.OnePointFive;
                }
                else
                {
                    _Port.StopBits = StopBits.None;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public ConnectionScannerCOM(FloorSet aFloorSet, SettingsBase ConnectionSetting)
            : base(aFloorSet, ConnectionSetting)
        {
            //默认选项
            _Port.PortName = "COM1";
            _Port.DtrEnable = true;
            _Port.RtsEnable = false;
            _Port.DataBits = 8;
            _Port.ReadTimeout = 500;	//ms
 

            PaserSettings("2400,e,8,1");
            //mComm.EOFEnable=false;
            //			mComm.Handshaking=HandshakeConstants.comNone;	//None Handshakeing
            //			mComm.InputMode=InputModeConstants.comInputModeBinary;
            //mComm.NullDiscard=false;
            //mComm.ParityReplace="?";
            //mComm.RThreshold=5;	//R 闸值
            //mComm.SThreshold=0;	//S 闸值

            //载入自定义选项
            //检查看看选项是否存在
            _Settings = ConnectionSetting;
            if (ConnectionSetting["CommPort"] == null) ConnectionSetting["CommPort"] = 1;
            if (ConnectionSetting["Settings"] == null) ConnectionSetting["Settings"] = "2400,e,8,1";
            if (ConnectionSetting["ReceiveDataRepeatTimes"] == null) ConnectionSetting["ReceiveDataRepeatTimes"] = 5;
            if (ConnectionSetting["ReceiveDataCheckDelay"] == null) ConnectionSetting["ReceiveDataCheckDelay"] = 50;
            if (ConnectionSetting["ReceiveDataCheckRepeatTimes"] == null) ConnectionSetting["ReceiveDataCheckRepeatTimes"] = 5;
            if (ConnectionSetting["DisconnectRetryTimes"] == null) ConnectionSetting["DisconnectRetryTimes"] = 3;
            //if (ConnectionSetting["WhetherSendTimeToATM"]==null)	ConnectionSetting["WhetherSendTimeToATM"]="True";
            if (ConnectionSetting["SendTimeToATMFrequency"] == null) ConnectionSetting["SendTimeToATMFrequency"] = 5;

            //应用选项
            _Port.PortName = "COM" + Convert.ToInt16(ConnectionSetting["CommPort"]);
            if (!PaserSettings(ConnectionSetting["Settings"].ToString())) PaserSettings("2400,e,8,1");
            _ReceiveDataRepeatTimes = Convert.ToInt32(ConnectionSetting["ReceiveDataRepeatTimes"]);
            _ReceiveDataCheckDelay = Convert.ToInt32(ConnectionSetting["ReceiveDataCheckDelay"]);
            _SendTimeToATMFrequency = Convert.ToInt32(ConnectionSetting["SendTimeToATMFrequency"]);

            _ReceiveDataCheckRepeatTimes = Convert.ToInt32(ConnectionSetting["ReceiveDataCheckRepeatTimes"]);
            _DisconnectRetryTimes = Convert.ToInt32(ConnectionSetting["DisconnectRetryTimes"]);

            //mComm.OnComm+=new DMSCommEvents_OnCommEventHandler(mComm_OnComm);
            _Port.ReadTimeout = _ReceiveDataCheckDelay;

            if (_WhetherSendTimeToATM == true)
            {
                _SendTimeToATMTimer.Interval = _SendTimeToATMFrequency * 1000;
                _SendTimeToATMTimer.Elapsed += new System.Timers.ElapsedEventHandler(mSendTimeToATMTimer_Elapsed);
            }

            //其他
            _FloorSet = aFloorSet;
            _CurrentRoom = _FloorSet.FirstFloor.FirstRoom;

            //设置所有的Room Connect
            foreach (int floorNumber in _FloorSet.FloorNumbers)
            {
                foreach (Room room in _FloorSet[floorNumber].GetRooms())
                {
                    room.MyATM.Connect();
                }
            }



        }

        public ConnectionScannerCOM(FloorSet aFloorSet, SettingsBase ConnectionSetting, SettingsBase ProgramSetting)
            : base(aFloorSet, ConnectionSetting)
        {
        }



        #region 继承
        public override void ResetSetting()
        {
            base.ResetSetting();
            bool lastRunningStatus = base._IsRunning;
            if (this._IsRunning)
            {
                this.Disconnnect();
            }

            // Apply Setting
            if (!PaserSettings(_Settings["Settings"].ToString())) PaserSettings("2400,e,8,1");
            _ReceiveDataRepeatTimes = Convert.ToInt32(_Settings["ReceiveDataRepeatTimes"]);
            _ReceiveDataCheckDelay = Convert.ToInt32(_Settings["ReceiveDataCheckDelay"]);
            _Port.ReadTimeout = _ReceiveDataCheckDelay;
            _SendTimeToATMFrequency = Convert.ToInt32(_Settings["SendTimeToATMFrequency"]);

            _ReceiveDataCheckRepeatTimes = Convert.ToInt32(_Settings["ReceiveDataCheckRepeatTimes"]);
            _DisconnectRetryTimes = Convert.ToInt32(_Settings["DisconnectRetryTimes"]);

            _CurrentReceiveDataCheckRepeatTimes = 0;
            _CurrentReceiveDataRepeatTimes = 0;

            _CurrentRoom = _FloorSet.FirstFloor.FirstRoom;

            // TODO catch exceptions
            _Port.PortName = "COM" + Convert.ToInt16(_Settings["CommPort"]);

            if (lastRunningStatus)
            {
                try
                {
                    this.Connect();
                }
                catch
                {
                }
            }
        }

        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("标准串口连接协议",
                "{3F5F27CB-A661-4882-A29C-DD99E456CAC0}",
                new Version("1.0.2"),
                "通过标准串口获取房间信息。");
        }

        public override bool ShowSetupForm()
        {
            ConnectionCOMSetupForm aForm = new ConnectionCOMSetupForm();
            aForm.Settings = this.Settings;
            aForm.ShowDialog();

            return aForm.DialogResult == DialogResult.OK;
        }

        public override void Connect()
        {
            if (!base._IsRunning)
            {
                try
                {
                    Trace.WriteLine(
                        string.Format("RS232: Opening port:{0} baudrate:{1} databits:{2} parity:{3} stopbits:{4}",
                            _Port.PortName,
                            _Port.BaudRate,
                            _Port.DataBits,
                            _Port.Parity,
                            _Port.StopBits), 
                        "Connection");
                    _Port.Open();  //Open port
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(
                        string.Format("RS232: Open failed! Reason:{0}",
                            ex.Message),
                        "Connection");
                    //Can't not open the Port
                    //TODO Add the Error to LOG
                    return;
                }

                base.Connect();
                if (_WhetherSendTimeToATM == true)
                {
                    _SendTimeToATMTimer.Start();
                }

                //Start Query Thread
                _QueryThread = new Thread(new ThreadStart(QueryDataMain));
                _QueryThread.Start();

                _IsRunning = true;
            }
        }

        public override void Disconnnect()
        {
            if (base._IsRunning)
            {

                base.Disconnnect();
                _SendTimeToATMTimer.Stop();

                //Stop Query Thread
                //_QueryThread.Abort();

                //_Port.Close();	//Close Port
                _IsRunning = false;
            }
        }

        #endregion

        public bool WhetherSendTimeToATM
        {
            get
            {
                return _WhetherSendTimeToATM;
            }
            set
            {
                _WhetherSendTimeToATM = value;
                if (_WhetherSendTimeToATM == true) _SendTimeToATMTimer.Start(); else _SendTimeToATMTimer.Stop();
            }
        }

        private void SendRequestRoomInformationData()
        {
            Trace.WriteLine(string.Format("RS232: Sending request data to room: {0}", _CurrentRoom.ToString()), "Connection");

            byte[] sendData = new byte[4];
            sendData[0] = (byte)(0x80 + (byte)_CurrentRoom.Number);
            sendData[1] = (byte)_CurrentRoom.MyFloor.Number;
            //TODO Maybe add AirCon And RoomStatus to Data
            sendData[3] = Utility.MakeCheckSumValue(sendData);

            //Send Data To ATM
            _Port.Write(sendData , 0 , sendData.Length);

        }

        // This is the thread to query data
        private void QueryDataMain()
        {
            while (true)
            {
                int currentRepeatTimes = 0;
                for (currentRepeatTimes = 0; currentRepeatTimes < this._ReceiveDataRepeatTimes; currentRepeatTimes++)
                {
                    Trace.WriteLine(
                        string.Format(
                            "RS232: Starting new room receive-check procedure with rep times {0}...", 
                            currentRepeatTimes),
                        "Connection");
                    int j;
                    for (j = 0; j < this._ReceiveDataCheckRepeatTimes; j++)
                    {
                        // Check if needs exit the thread
                        if (!_IsRunning)
                        {
                            _Port.Close();
                            return;
                        }
                        
                        bool checkReceivedSuccess = false;

                        _Port.DiscardInBuffer();
                        SendRequestRoomInformationData();	//Send Data to RCU

                        // Wait and Receive request data
                        byte[] revBytes = null;
                        bool receiveSuccess = true;
                        try
                        {
                            revBytes = new byte[5];

                            int count = 0;
                            while (count < revBytes.Length)
                            {
                                int nr = _Port.Read(revBytes, count, revBytes.Length - count);
                                count += nr;

                                // count += _Port.Read(revData, count, revData.Length - count);
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(string.Format("RS232: Receive exception - {0}", ex.Message), "Connection");
                            receiveSuccess = false;
                        }
                        finally
                        {
                            _Port.DiscardInBuffer();
                        }

                        string dataFormat = ConvertByteArrayToString(revBytes, 0 , revBytes.Length);

                        Trace.WriteLine(
                            string.Format("RS232: Received {0} bytes data ...  {1}", revBytes == null ? 0 : revBytes.Length , dataFormat) ,
                            "Connection");

                        // Get the data
                        if ((receiveSuccess) &&(revBytes != null) && (revBytes.Length == 5))
                        {
                            //Special CheckSum校验
                            int checkSumValue = 0;
                            for (int k = 1; k <= revBytes.Length - 2; k++)
                            {
                                checkSumValue += revBytes[k];
                            }

                            if ((byte)(checkSumValue % 256) == revBytes[revBytes.Length - 1])
                            {
                                if ((256 - (_CurrentRoom.Number + _CurrentRoom.MyFloor.Number) % 256) == revBytes[0])
                                {
                                    if (j == 0)
                                    {
                                        _ReceiveCheckData = revBytes;
                                        checkReceivedSuccess = true;
                                    }
                                    else
                                    {
                                        //判断是不是与前几次相同
                                        if (Utility.ArrayDataEquals(_ReceiveCheckData, revBytes))
                                        {
                                            checkReceivedSuccess = true;
                                        }
                                        else
                                        {
                                            Trace.WriteLine("RS232: Unequal data check", "Connection");
                                        }
                                    }
                                }
                                else
                                {
                                    Trace.WriteLine("RS232: Unequal room number of received data", "Connection");
                                }
                            }
                            else
                            {
                                Trace.WriteLine("RS232: Checksum error", "Connection");
                            }
                        }

                        if (!checkReceivedSuccess)
                        {
                            Trace.WriteLine("RS232: Check data failed", "Connection");
                            break;
                        }
                    }

                    if (j >= this._ReceiveDataCheckRepeatTimes)
                    {
                        //数据都正确
                        ApplyData();

                        //判断是否断网
                        if (_DisconnectList.ContainsKey(_CurrentRoom.ToString()))
                        {
                            _DisconnectList.Remove(_CurrentRoom.ToString());
                        }

                        _CurrentRoom.MyATM.Connect();

                        break;	//Query next room
                    }
                }

                if (currentRepeatTimes >= this._ReceiveDataRepeatTimes)
                {
                    CountDisconnectCurrentRoom();
                }

                //Move to next room
                CurrentRoom = NextRoom;
                Trace.WriteLine(
                    string.Format(
                        "RS232: Moving to new room - {0}",
                        NextRoom.ToString()),
                    "Connection");
            }
        }

        // TODO Test
        private static string ConvertByteArrayToString(byte[] revData, int index , int length)
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

        /// <summary>
        /// 解析数据并操作FloorSet
        /// </summary>
        protected void ApplyData()
        {
            Trace.WriteLine(string.Format("Applying data for room:{0}", _CurrentRoom), "Connection");
            
            BitArray aData = new BitArray(_ReceiveCheckData);

            //Key
            if (aData[8] == true)
            {
                switch (_ReceiveCheckData[3] % 8)
                {
                    case 1:
                        _CurrentRoom.KeyInsert(KeyStatusType.Guest);
                        break;
                    case 2:
                        _CurrentRoom.KeyInsert(KeyStatusType.Leader);
                        break;
                    case 3:
                        _CurrentRoom.KeyInsert(KeyStatusType.Servant);
                        break;
                    default:
                        break;
                }
            }
            else
                _CurrentRoom.KeyPullOut();

            //Service
            if ((aData[8 + 3] == true) && (aData[8 + 4] == true))
            { _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.Empty; }
            else
            {
                if (_CurrentRoom.HotelUsingStatus == HotelUsingStatusType.Empty)	//上一次状态为空房
                    _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.Vacant;

                //Apply Service
                if (aData[8 + 2] == true) _CurrentRoom.MyGuestService.Call();
                else _CurrentRoom.MyGuestService.CancelService(ServiceType.Call);
                if (aData[8 + 3] == true) _CurrentRoom.MyGuestService.Clean();
                else _CurrentRoom.MyGuestService.CancelService(ServiceType.Clean);
                if (aData[8 + 4] == true) _CurrentRoom.MyGuestService.DontDisturb();
                else _CurrentRoom.MyGuestService.CancelService(ServiceType.DontDisturb);
            }

            //Repair 
            if (aData[8 + 5] == true)
                _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.Maintanent;
            else
            {
                if (_CurrentRoom.HotelUsingStatus == HotelUsingStatusType.Maintanent)
                    _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.Vacant;
            }

            //Door
            if (aData[8 + 1] == true)
            {
                _CurrentRoom.DoorOpen();
            }
            else
            {
                _CurrentRoom.DoorClose();
            }


            //Temperature
            _CurrentRoom.Temperature = _ReceiveCheckData[2];

            //AirConditionerSpeed
            if ((aData[8 + 6] == true) && (aData[8 + 7] == true))
            {
                _CurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.High;
            }
            else if ((aData[8 + 6] == false) && (aData[8 + 7] == true))
            {
                _CurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.Mid;
            }
            else if ((aData[8 + 6] == true) && (aData[8 + 7] == false))
            {
                _CurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.Low;
            }
            else if ((aData[8 + 6] == false) && (aData[8 + 7] == false))
            {
                _CurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.Off;
            }

            //HotelUsingStatus- Cleaning
            if (aData[24 + 3] == true)
            {
                _CurrentRoom.MyGuestService.Cleaning();    
            }
            else
            {
                _CurrentRoom.MyGuestService.CancelService(ServiceType.Cleaning);
            }

            // Problem
            if (aData[24 + 4] == true)
                _CurrentRoom.MyATM.ProblemCaused();
            else
                _CurrentRoom.MyATM.ProblemRepaired();

            // Check out
            if (aData[24 + 5] == true)
            {
                _CurrentRoom.MyGuestService.QuitRoom();
            }
            else
            {
                _CurrentRoom.MyGuestService.CancelService(ServiceType.QuitRoom);
                //if (mCurrentRoom.HotelUsingStatus==HotelUsingStatusType.CheckOut)
                //	mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Cleaning;
            }

            // Emergency
            if (aData[24 + 6] == true)
            {
                _CurrentRoom.MyGuestService.Emergency();
            }
            else
            {
                _CurrentRoom.MyGuestService.EmergencyCancel();
            }




        }

        /// <summary>
        /// 发送时间信息给ATM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mSendTimeToATMTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_Port.IsOpen)
            {
                Trace.WriteLine("RS232: Boardcasting time data", "Connection");
                // Broadcast flag
                byte[] aData = new byte[2] { 0xf0, 0xf };
                _Port.Write(aData, 0, aData.Length);

                // Send Minutes and Hour to ATM
                byte[] timeData = new byte[6];
                timeData[0] = (byte)Utility.BCD(DateTime.Now.Minute);
                timeData[1] = (byte)Utility.BCD(DateTime.Now.Hour);
                timeData[2] = (byte)_CurrentRoom.MyAirConditioner.DefaultTemperature;
                if (_CurrentRoom.MyAirConditioner.Season == SeasonType.Winter) timeData[2] += 0x40;
                // Repeat data
                for (int i = 3; i <= 5; i++) timeData[i] = timeData[i - 3];

                _Port.Write(timeData, 0, timeData.Length);
            }
        }

        /// <summary>
        /// 当某一次查询时断网
        /// </summary>
        private void CountDisconnectCurrentRoom()
        {
            if (_DisconnectList[_CurrentRoom.ToString()] == null)
            {
                _DisconnectList.Add(_CurrentRoom.ToString(), 0);
            }

            _DisconnectList[_CurrentRoom.ToString()] = (int)_DisconnectList[_CurrentRoom.ToString()] + 1;
            if ((int)_DisconnectList[_CurrentRoom.ToString()] > _DisconnectRetryTimes)
            {
                _CurrentRoom.MyATM.Disconnect();
            }
        }

        protected void OnCurrentScanRoomChanged(System.EventArgs e)
        {
            if (CurrentScanRoomChanged != null)
                CurrentScanRoomChanged(this, e);
        }

    }
}
