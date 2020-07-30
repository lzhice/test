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
#endregion
using System;
using System.Collections;
using ATM3300.Common;
using ATM3300.Connection;
using MSCommLib;
using System.Windows.Forms;

namespace ATM3300.Connection
{

    /// <summary>
    /// ScannerCOM 的摘要说明。
    /// </summary>
    public class ConnectionScannerCOM : ConnectionScannerBase
    {
        public event System.EventHandler CurrentScanRoomChanged;

        protected MSCommClass mComm = new MSCommClass();

        //		protected RS232.Rs232 _RS232 = new RS232.Rs232();

        protected int mReceiveDataCheckRepeatTimes = 3;
        protected int mCurrentReceiveDataCheckRepeatTimes = 0;
        protected int mReceiveDataCheckDelay = 50; //millionseconds

        protected int mReceiveDataRepeatTimes = 5;
        protected int mCurrentReceiveDataRepeatTimes = 0;

        protected byte[] mReceiveCheckData = new byte[5];

        protected int mDisconnectRetryTimes = 3;
        protected Hashtable mDisconnectList = new Hashtable();
        protected System.Timers.Timer mScannerTimeout = new System.Timers.Timer();
        protected System.Timers.Timer mSendTimeToATMTimer = new System.Timers.Timer();
        protected System.Timers.Timer mScanNextRoomDelay = new System.Timers.Timer();

        protected Room mCurrentRoom;
        protected FloorSet mFloorSet;
        protected bool mWhetherSendTimeToATM = true;
        protected int mSendTimeToATMFrequency = 30;	//seconds
        protected SettingsBase mSettings;

        public ConnectionScannerCOM(FloorSet aFloorSet, SettingsBase ConnectionSetting)
            : base(aFloorSet, ConnectionSetting)
        {
            //默认选项
            mComm.CommPort = 1;
            mComm.DTREnable = false;
            mComm.EOFEnable = false;
            mComm.Handshaking = HandshakeConstants.comNone;	//None Handshakeing
            mComm.InBufferSize = 1024;
            mComm.InputMode = InputModeConstants.comInputModeBinary;
            mComm.NullDiscard = false;
            mComm.OutBufferSize = 512;
            mComm.ParityReplace = "?";
            mComm.RThreshold = 5;	//R 闸值
            mComm.RTSEnable = false;
            mComm.Settings = "2400,e,8,1";
            mComm.SThreshold = 0;	//S 闸值

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
            mComm.CommPort = Convert.ToInt16(ConnectionSetting["CommPort"]);
            mComm.Settings = (string)ConnectionSetting["Settings"];
            mReceiveDataRepeatTimes = Convert.ToInt32(ConnectionSetting["ReceiveDataRepeatTimes"]);
            mReceiveDataCheckDelay = Convert.ToInt32(ConnectionSetting["ReceiveDataCheckDelay"]);
            mSendTimeToATMFrequency = Convert.ToInt32(ConnectionSetting["SendTimeToATMFrequency"]);

            mReceiveDataCheckRepeatTimes = Convert.ToInt32(ConnectionSetting["ReceiveDataCheckRepeatTimes"]);
            mDisconnectRetryTimes = Convert.ToInt32(ConnectionSetting["DisconnectRetryTimes"]);

            mScannerTimeout.Interval = mReceiveDataCheckDelay;
            mScannerTimeout.Elapsed += new System.Timers.ElapsedEventHandler(mScannerTimeout_Elapsed);
            mScanNextRoomDelay.Interval = mReceiveDataCheckDelay;
            mScanNextRoomDelay.Elapsed += new System.Timers.ElapsedEventHandler(mScanNextRoomDelay_Elapsed);

            mComm.OnComm += new DMSCommEvents_OnCommEventHandler(mComm_OnComm);

            if (mWhetherSendTimeToATM == true)
            {
                mSendTimeToATMTimer.Interval = mSendTimeToATMFrequency * 1000;
                mSendTimeToATMTimer.Elapsed += new System.Timers.ElapsedEventHandler(mSendTimeToATMTimer_Elapsed);
            }

            //其他
            mFloorSet = aFloorSet;
            mCurrentRoom = mFloorSet.FirstFloor.FirstRoom;

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
            mComm.CommPort = Convert.ToInt16(mSettings["CommPort"]);
            mComm.Settings = (string)mSettings["Settings"];
            mReceiveDataRepeatTimes = Convert.ToInt32(mSettings["ReceiveDataRepeatTimes"]);
            mReceiveDataCheckDelay = Convert.ToInt32(mSettings["ReceiveDataCheckDelay"]);
            mSendTimeToATMFrequency = Convert.ToInt32(mSettings["SendTimeToATMFrequency"]);

            mReceiveDataCheckRepeatTimes = Convert.ToInt32(mSettings["ReceiveDataCheckRepeatTimes"]);
            mDisconnectRetryTimes = Convert.ToInt32(mSettings["DisconnectRetryTimes"]);

            mScannerTimeout.Interval = mReceiveDataCheckDelay;
            mScanNextRoomDelay.Interval = mReceiveDataCheckDelay;
            mCurrentReceiveDataCheckRepeatTimes = 0;
            mCurrentReceiveDataRepeatTimes = 0;

            mCurrentRoom = mFloorSet.FirstFloor.FirstRoom;

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
                    mComm.PortOpen = true;  //Open port
                }
                catch (Exception e)
                {
                    //Can't not open the Port
                    //TODO Add the Error to LOG
                    return;
                }

                base.Connect();
                if (mWhetherSendTimeToATM == true) mSendTimeToATMTimer.Start();
                StartRoomInformatioRequest();
                mIsRunning = true;
            }
        }

        public override void Disconnnect()
        {
            if (base.mIsRunning)
            {

                base.Disconnnect();
                mSendTimeToATMTimer.Stop();
                mScannerTimeout.Stop();
                mComm.PortOpen = false;  //Close Port
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
                return mCurrentRoom;
            }
        }

        public override Room NextRoom
        {
            get
            {
                if (mCurrentRoom == mCurrentRoom.MyFloor.LastRoom)
                {
                    return mFloorSet.NextFloor(mCurrentRoom.MyFloor).FirstRoom;	//Goto the next Floor first room
                }
                else
                {
                    return mCurrentRoom.MyFloor[mCurrentRoom.Number + 1];	//Goto the next Room of the Floor
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
            SendData[0] = (byte)(0x80 + (byte)mCurrentRoom.Number);
            SendData[1] = (byte)mCurrentRoom.MyFloor.Number;
            //TODO Maybe add AirCon And RoomStatus to Data
            SendData[3] = Utility.MakeCheckSumValue(SendData);
            //Send Data To ATM
            mComm.Output = SendData;
        }

        private void StartRoomInformatioRequest()
        {
            mCurrentReceiveDataRepeatTimes = 0;
            mCurrentReceiveDataCheckRepeatTimes = 0;
            SendRequestRoomInformationData();
            mScannerTimeout.Start();
            OnCurrentScanRoomChanged(new EventArgs());
        }

        private void StartNextRoomInformationRequest()
        {
            mCurrentRoom = NextRoom;
            StartRoomInformatioRequest();
        }

        private void StartNextCheckInformationRequest()
        {
            mCurrentReceiveDataRepeatTimes++;
            if (mCurrentReceiveDataRepeatTimes > mReceiveDataRepeatTimes)
            {
                DisconnectCause();
                StartNextRoomInformationRequest();
            }
            else
            {
                mCurrentReceiveDataCheckRepeatTimes = 0;
                SendRequestRoomInformationData();
                mScannerTimeout.Start();
            }
        }
        /// <summary>
        /// 连接超时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mScannerTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartNextCheckInformationRequest();
        }

        /// <summary>
        /// 触发 Comm的事件，包括接受事件
        /// </summary>
        private void mComm_OnComm()
        {
            switch (mComm.CommEvent)
            {
                case (short)OnCommConstants.comEvReceive:

                    mScannerTimeout.Stop();
                    //接受到了数据
                    //校验数据
                    if (mComm.InBufferCount != 5)
                    {
                        mComm.InBufferCount = 0;
                        StartNextCheckInformationRequest();
                        return;
                    }
                    byte[] ReceiveData = new byte[5];
                    ReceiveData = (byte[])mComm.Input;

                    //Special CheckSum校验
                    int CheckSumFlag = 0;
                    for (int i = 1; i <= ReceiveData.Length - 2; i++) CheckSumFlag += ReceiveData[i];

                    if ((byte)(CheckSumFlag % 256) != ReceiveData[ReceiveData.Length - 1])
                    { StartNextCheckInformationRequest(); return; };
                    if ((256 - (mCurrentRoom.Number + mCurrentRoom.MyFloor.Number) % 256) != ReceiveData[0]) { StartNextCheckInformationRequest(); return; };

                    if (mCurrentReceiveDataCheckRepeatTimes == 0)
                    {
                        mReceiveCheckData = ReceiveData;
                    }
                    else
                    {
                        //判断是不是与前几次相同
                        if (!Utility.ArrayDataEquals(mReceiveCheckData, ReceiveData))
                        { StartNextCheckInformationRequest(); return; };
                    }

                    //下一步
                    mCurrentReceiveDataCheckRepeatTimes++;
                    if (mCurrentReceiveDataCheckRepeatTimes >= mReceiveDataCheckRepeatTimes)
                    {
                        //数据都正确
                        ApplyData();
                        //判断是否断网
                        if (mDisconnectList[mCurrentRoom.ToString()] != null)
                            mDisconnectList.Remove(mCurrentRoom.ToString());
                        mCurrentRoom.MyATM.Connect();

                        //延时一段时间再查询下一个房间
                        mScanNextRoomDelay.Start();
                    }
                    else
                    {
                        //下一次校验查询
                        mScannerTimeout.Start();
                        SendRequestRoomInformationData();
                    }

                    break;
                case (short)OnCommConstants.comEvSend:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 解析数据并操作FloorSet
        /// </summary>
        private void ApplyData()
        {
            BitArray aData = new BitArray(mReceiveCheckData);

            //Key
            if (aData[8] == true)
            {
                switch (mReceiveCheckData[3] % 8)
                {
                    case 1:
                        mCurrentRoom.KeyInsert(KeyStatusType.Guest);
                        break;
                    case 2:
                        mCurrentRoom.KeyInsert(KeyStatusType.Leader);
                        break;
                    case 3:
                        mCurrentRoom.KeyInsert(KeyStatusType.Servant);
                        break;
                    default:
                        break;
                }
            }
            else
                mCurrentRoom.KeyPullOut();

            //Service
            if ((aData[8 + 3] == true) && (aData[8 + 4] == true))
            { mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.Empty; }
            else
            {
                if (mCurrentRoom.HotelUsingStatus == HotelUsingStatusType.Empty)	//上一次状态为空房
                    mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.Vacant;

                //Apply Service
                if (aData[8 + 2] == true) mCurrentRoom.MyGuestService.Call();
                else mCurrentRoom.MyGuestService.CancelService(ServiceType.Call);
                if (aData[8 + 3] == true) mCurrentRoom.MyGuestService.Clean();
                else mCurrentRoom.MyGuestService.CancelService(ServiceType.Clean);
                if (aData[8 + 4] == true) mCurrentRoom.MyGuestService.DontDisturb();
                else mCurrentRoom.MyGuestService.CancelService(ServiceType.DontDisturb);
            }

            //Repair 
            if (aData[8 + 5] == true)
                mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.Maintanent;
            else
            {
                if (mCurrentRoom.HotelUsingStatus == HotelUsingStatusType.Maintanent)
                    mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.Vacant;
            }

            //Door
            if (aData[8 + 1] == true)
                mCurrentRoom.DoorOpen();
            else
                mCurrentRoom.DoorClose();


            //Temperature
            mCurrentRoom.Temperature = mReceiveCheckData[2];

            //AirConditionerSpeed
            if ((aData[8 + 6] == true) && (aData[8 + 7] == true))
            {
                mCurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.High;
            }
            else if ((aData[8 + 6] == false) && (aData[8 + 7] == true))
            {
                mCurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.Mid;
            }
            else if ((aData[8 + 6] == true) && (aData[8 + 7] == false))
            {
                mCurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.Low;
            }
            else if ((aData[8 + 6] == false) && (aData[8 + 7] == false))
            {
                mCurrentRoom.MyAirConditioner.Speed = AirConditionerSpeedType.Off;
            }

            //Problem
            if (aData[24 + 4] == true)
                mCurrentRoom.MyATM.ProblemCaused();
            else
                mCurrentRoom.MyATM.ProblemRepaired();

            //Emergency
            if (aData[24 + 6] == true)
                mCurrentRoom.MyGuestService.Emergency();
            else
                mCurrentRoom.MyGuestService.EmergencyCancel();

            //Check out
            if (aData[24 + 5] == true)
                mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.CheckOut;
            else
            {
                //if (mCurrentRoom.HotelUsingStatus==HotelUsingStatusType.CheckOut)
                //	mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Cleaning;
            }

            //HotelUsingStatus- Cleaning
            if (aData[24 + 3] == true)
            { mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.Cleaning; }
            else
            {
                if (mCurrentRoom.HotelUsingStatus == HotelUsingStatusType.Cleaning)
                    mCurrentRoom.HotelUsingStatus = HotelUsingStatusType.Vacant;
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
            mComm.Output = aData;

            //Send Minutes and Hour to ATM
            byte[] TimeData = new byte[6];
            TimeData[0] = (byte)Utility.BCD(DateTime.Now.Minute);
            TimeData[1] = (byte)Utility.BCD(DateTime.Now.Hour);
            TimeData[2] = (byte)mCurrentRoom.MyAirConditioner.DefaultTemperature;
            if (mCurrentRoom.MyAirConditioner.Season == SeasonType.Winter) TimeData[2] += 0x40;
            //Repeat data
            for (int i = 3; i <= 5; i++) TimeData[i] = TimeData[i - 3];

            mComm.Output = TimeData;
        }

        /// <summary>
        /// 当某一次查询时断网
        /// </summary>
        private void DisconnectCause()
        {
            if (mDisconnectList[mCurrentRoom.ToString()] == null)
            {
                mDisconnectList.Add(mCurrentRoom.ToString(), 0);
            }

            mDisconnectList[mCurrentRoom.ToString()] = (int)mDisconnectList[mCurrentRoom.ToString()] + 1;
            if ((int)mDisconnectList[mCurrentRoom.ToString()] > mDisconnectRetryTimes)
                mCurrentRoom.MyATM.Disconnect();
        }

        protected void OnCurrentScanRoomChanged(System.EventArgs e)
        {
            if (CurrentScanRoomChanged != null)
                CurrentScanRoomChanged(this, e);
        }

        private void mScanNextRoomDelay_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartNextRoomInformationRequest();
            mScanNextRoomDelay.Stop();
        }
    }
}
