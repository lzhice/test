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
using RS232;

namespace ATM3300.Connection
{

    /// <summary>
    /// ScannerCOM 的摘要说明。
    /// </summary>
    public class ConnectionScannerCOM : ConnectionScannerBase
    {
        public event System.EventHandler CurrentScanRoomChanged;

        //protected MSCommClass mComm=new MSCommClass();

        protected RS232.Rs232 _RS232 = new RS232.Rs232();

        protected int _ReceiveDataCheckRepeatTimes = 3;
        protected int mCurrentReceiveDataCheckRepeatTimes = 0;
        protected int mReceiveDataCheckDelay = 50; //millionseconds

        protected int _ReceiveDataRepeatTimes = 5;
        protected int mCurrentReceiveDataRepeatTimes = 0;

        protected byte[] mReceiveCheckData = new byte[5];

        protected int mDisconnectRetryTimes = 3;
        protected Hashtable mDisconnectList = new Hashtable();
        protected System.Timers.Timer mSendTimeToATMTimer = new System.Timers.Timer();

        protected Room mCurrentRoom;
        protected FloorSet mFloorSet;
        protected bool mWhetherSendTimeToATM = true;
        protected int mSendTimeToATMFrequency = 30;	//seconds
        protected SettingsBase mSettings;

        protected Thread _QueryThread = null;

        private bool PaserSettings(string Settings)
        {
            string[] SStr = Settings.Split(',');
            string PStr = "noem";

            try
            {
                _Port.BaudRate = Convert.ToInt32(SStr[0]);
                _Port.Parity = (RS232.Rs232.DataParity)(PStr.IndexOf(SStr[1]));
                _Port.DataBit = Convert.ToInt32(SStr[2]);
                _Port.StopBit = (Rs232.DataStopBit)Convert.ToInt32(SStr[3]);
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
            _Port.Port = 1;
            _Port.Dtr = true;
            _Port.Rts = false;
            _Port.DataBit = 8;
            _Port.Timeout = 500;	//ms
            _Port.WorkingMode = Rs232.Mode.NonOverlapped;
            _Port.BufferSize = 1024;

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
            mSettings = ConnectionSetting;
            if (ConnectionSetting["CommPort"] == null) ConnectionSetting["CommPort"] = 1;
            if (ConnectionSetting["Settings"] == null) ConnectionSetting["Settings"] = "2400,e,8,1";
            if (ConnectionSetting["ReceiveDataRepeatTimes"] == null) ConnectionSetting["ReceiveDataRepeatTimes"] = 5;
            if (ConnectionSetting["ReceiveDataCheckDelay"] == null) ConnectionSetting["ReceiveDataCheckDelay"] = 50;
            if (ConnectionSetting["ReceiveDataCheckRepeatTimes"] == null) ConnectionSetting["ReceiveDataCheckRepeatTimes"] = 5;
            if (ConnectionSetting["DisconnectRetryTimes"] == null) ConnectionSetting["DisconnectRetryTimes"] = 3;
            //if (ConnectionSetting["WhetherSendTimeToATM"]==null)	ConnectionSetting["WhetherSendTimeToATM"]="True";
            if (ConnectionSetting["SendTimeToATMFrequency"] == null) ConnectionSetting["SendTimeToATMFrequency"] = 5;

            //应用选项
            _Port.Port = Convert.ToInt16(ConnectionSetting["CommPort"]);
            if (!PaserSettings(ConnectionSetting["Settings"].ToString())) PaserSettings("2400,e,8,1");
            _ReceiveDataRepeatTimes = Convert.ToInt32(ConnectionSetting["ReceiveDataRepeatTimes"]);
            mReceiveDataCheckDelay = Convert.ToInt32(ConnectionSetting["ReceiveDataCheckDelay"]);
            mSendTimeToATMFrequency = Convert.ToInt32(ConnectionSetting["SendTimeToATMFrequency"]);

            _ReceiveDataCheckRepeatTimes = Convert.ToInt32(ConnectionSetting["ReceiveDataCheckRepeatTimes"]);
            mDisconnectRetryTimes = Convert.ToInt32(ConnectionSetting["DisconnectRetryTimes"]);

            //mComm.OnComm+=new DMSCommEvents_OnCommEventHandler(mComm_OnComm);
            _Port.Timeout = mReceiveDataCheckDelay;

            if (mWhetherSendTimeToATM == true)
            {
                mSendTimeToATMTimer.Interval = mSendTimeToATMFrequency * 1000;
                mSendTimeToATMTimer.Elapsed += new System.Timers.ElapsedEventHandler(mSendTimeToATMTimer_Elapsed);
            }

            //其他
            mFloorSet = aFloorSet;
            _CurrentRoom = mFloorSet.FirstFloor.FirstRoom;

            //设置所有的Room Connect
            foreach (int FloorNumber in mFloorSet.FloorNumbers)
            {
                for (int RoomNumber = 1; RoomNumber <= mFloorSet[FloorNumber].Length; RoomNumber++)
                    mFloorSet[FloorNumber][RoomNumber].MyATM.Connect();
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
            bool LastRunningStatus = base.mIsRunning;
            if (this.mIsRunning)
                this.Disconnnect();

            //Apply Setting
            _Port.Port = Convert.ToInt16(mSettings["CommPort"]);
            if (!PaserSettings(mSettings["Settings"].ToString())) PaserSettings("2400,e,8,1");
            _ReceiveDataRepeatTimes = Convert.ToInt32(mSettings["ReceiveDataRepeatTimes"]);
            mReceiveDataCheckDelay = Convert.ToInt32(mSettings["ReceiveDataCheckDelay"]);
            _Port.Timeout = mReceiveDataCheckDelay;
            mSendTimeToATMFrequency = Convert.ToInt32(mSettings["SendTimeToATMFrequency"]);

            _ReceiveDataCheckRepeatTimes = Convert.ToInt32(mSettings["ReceiveDataCheckRepeatTimes"]);
            mDisconnectRetryTimes = Convert.ToInt32(mSettings["DisconnectRetryTimes"]);

            mCurrentReceiveDataCheckRepeatTimes = 0;
            mCurrentReceiveDataRepeatTimes = 0;

            _CurrentRoom = mFloorSet.FirstFloor.FirstRoom;

            if (LastRunningStatus) this.Connect();
        }

        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("标准串口连接协议",
                "{3F5F27CB-A661-4882-A29C-DD99E456CAC0}",
                new Version("0.0.2"),
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
            if (!base.mIsRunning)
            {
                try
                {
                    _Port.Open();  //Open port
                }
                catch
                {
                    //Can't not open the Port
                    //TODO Add the Error to LOG
                    return;
                }

                base.Connect();
                if (mWhetherSendTimeToATM == true) mSendTimeToATMTimer.Start();

                //Start Query Thread
                _QueryThread = new Thread(new ThreadStart(QueryDataMain));
                _QueryThread.Start();

                mIsRunning = true;
            }
        }

        public override void Disconnnect()
        {
            if (base.mIsRunning)
            {

                base.Disconnnect();
                mSendTimeToATMTimer.Stop();

                //Stop Query Thread
                _QueryThread.Abort();

                _Port.Close();	//Close Port
                mIsRunning = false;
            }
        }

        public override FloorSet FloorSet
        {
            get
            {
                return mFloorSet;
            }
        }

        public override SettingsBase Settings
        {
            get
            {
                return this.mSettings;
            }
        }


        #endregion

        public override Room CurrentRoom
        {
            get
            {
                return _CurrentRoom;
            }
        }

        public override Room NextRoom
        {
            get
            {
                if (_CurrentRoom == _CurrentRoom.MyFloor.LastRoom)
                {
                    return mFloorSet.NextFloor(_CurrentRoom.MyFloor).FirstRoom;	//Goto the next Floor first room
                }
                else
                {
                    return _CurrentRoom.MyFloor[_CurrentRoom.Number + 1];	//Goto the next Room of the Floor
                }
            }
        }

        public bool WhetherSendTimeToATM
        {
            get
            {
                return mWhetherSendTimeToATM;
            }
            set
            {
                mWhetherSendTimeToATM = value;
                if (mWhetherSendTimeToATM == true) mSendTimeToATMTimer.Start(); else mSendTimeToATMTimer.Stop();
            }
        }

        private void SendRequestRoomInformationData()
        {
            byte[] SendData = new byte[4];
            SendData[0] = (byte)(0x80 + (byte)_CurrentRoom.Number);
            SendData[1] = (byte)_CurrentRoom.MyFloor.Number;
            //TODO Maybe add AirCon And RoomStatus to Data
            SendData[3] = Utility.MakeCheckSumValue(SendData);

            //Send Data To ATM
            _Port.Write(SendData);

        }

        //This is the thread to query data
        private void QueryDataMain()
        {
            while (true)
            {
                int i = 0;
                for (i = 0; i < this._ReceiveDataRepeatTimes; i++)
                {
                    int j;
                    for (j = 0; j < this._ReceiveDataCheckRepeatTimes; j++)
                    {
                        bool CheckReceivedSuccess = false;
                        SendRequestRoomInformationData();	//Send Data to RCU

                        //Wait and Receive request data
                        byte[] ReceiveData = null;
                        try
                        {
                            int RevNum = _Port.Read(5);
                            if (RevNum == 5)
                                ReceiveData = _Port.InputStream;
                        }
                        catch
                        {
                        }
                        finally
                        {
                            _Port.ClearInputBuffer();
                        }

                        //Get the data
                        if ((ReceiveData != null) && (ReceiveData.Length == 5))
                        {
                            //Special CheckSum校验
                            int CheckSumFlag = 0;
                            for (int k = 1; k <= ReceiveData.Length - 2; k++) CheckSumFlag += ReceiveData[k];

                            if ((byte)(CheckSumFlag % 256) == ReceiveData[ReceiveData.Length - 1])
                            {
                                if ((256 - (_CurrentRoom.Number + _CurrentRoom.MyFloor.Number) % 256) == ReceiveData[0])
                                {
                                    if (j == 0)
                                    {
                                        mReceiveCheckData = ReceiveData;
                                        CheckReceivedSuccess = true;
                                    }
                                    else
                                    {
                                        //判断是不是与前几次相同
                                        if (Utility.ArrayDataEquals(mReceiveCheckData, ReceiveData))
                                        {
                                            CheckReceivedSuccess = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (!CheckReceivedSuccess) break;
                    }

                    if (j >= this._ReceiveDataCheckRepeatTimes)
                    {
                        //数据都正确
                        ApplyData();
                        //判断是否断网
                        if (mDisconnectList[_CurrentRoom.ToString()] != null)
                            mDisconnectList.Remove(_CurrentRoom.ToString());
                        _CurrentRoom.MyATM.Connect();

                        break;	//Query next room
                    }
                }

                if (i >= this._ReceiveDataRepeatTimes)
                    DisconnectCause();

                //Move to next room
                _CurrentRoom = NextRoom;

            }
        }

        /// <summary>
        /// 解析数据并操作FloorSet
        /// </summary>
        protected void ApplyData()
        {
            BitArray aData = new BitArray(mReceiveCheckData);

            //Key
            if (aData[8] == true)
            {
                switch (mReceiveCheckData[3] % 8)
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
                _CurrentRoom.DoorOpen();
            else
                _CurrentRoom.DoorClose();


            //Temperature
            _CurrentRoom.Temperature = mReceiveCheckData[2];

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

            //Problem
            if (aData[24 + 4] == true)
                _CurrentRoom.MyATM.ProblemCaused();
            else
                _CurrentRoom.MyATM.ProblemRepaired();

            //Emergency
            if (aData[24 + 6] == true)
                _CurrentRoom.MyGuestService.Emergency();
            else
                _CurrentRoom.MyGuestService.EmergencyCancel();

            //Check out
            if (aData[24 + 5] == true)
                _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.CheckOut;
            else
            {
                //if (mCurrentRoom.HotelUsingStatus==HotelUsingStatusType.CheckOut)
                //	mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Cleaning;
            }

            //HotelUsingStatus- Cleaning
            if (aData[24 + 3] == true)
            { _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.Cleaning; }
            else
            {
                if (_CurrentRoom.HotelUsingStatus == HotelUsingStatusType.Cleaning)
                    _CurrentRoom.HotelUsingStatus = HotelUsingStatusType.Vacant;
            }

        }

        /// <summary>
        /// 发送时间信息给ATM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mSendTimeToATMTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Broadcast flag
            byte[] aData = new byte[2] { 0xf0, 0xf };
            _Port.Write(aData);

            //Send Minutes and Hour to ATM
            byte[] TimeData = new byte[6];
            TimeData[0] = (byte)Utility.BCD(DateTime.Now.Minute);
            TimeData[1] = (byte)Utility.BCD(DateTime.Now.Hour);
            TimeData[2] = (byte)_CurrentRoom.MyAirConditioner.DefaultTemperature;
            if (_CurrentRoom.MyAirConditioner.Season == SeasonType.Winter) TimeData[2] += 0x40;
            //Repeat data
            for (int i = 3; i <= 5; i++) TimeData[i] = TimeData[i - 3];

            _Port.Write(TimeData);
        }

        /// <summary>
        /// 当某一次查询时断网
        /// </summary>
        private void DisconnectCause()
        {
            if (mDisconnectList[_CurrentRoom.ToString()] == null)
            {
                mDisconnectList.Add(_CurrentRoom.ToString(), 0);
            }

            mDisconnectList[_CurrentRoom.ToString()] = (int)mDisconnectList[_CurrentRoom.ToString()] + 1;
            if ((int)mDisconnectList[_CurrentRoom.ToString()] > mDisconnectRetryTimes)
                _CurrentRoom.MyATM.Disconnect();
        }

        protected void OnCurrentScanRoomChanged(System.EventArgs e)
        {
            if (CurrentScanRoomChanged != null)
                CurrentScanRoomChanged(this, e);
        }

    }
}
