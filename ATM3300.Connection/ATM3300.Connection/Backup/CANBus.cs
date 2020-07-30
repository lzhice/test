using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CAN_Connection;
using ATM3300.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ATM3300.Connection
{
    /// <summary>
    /// CANBus 的摘要说明。
    /// </summary>
    [ConnectionVersionInfo(
         "标准CANBus连接协议",
                "{0140FA7F-80ED-4f54-AFC7-C4D1C49404DE}",
                "1.0.0",
                "通过标准CANBus获取房间信息。" ,
        typeof(ConnectionCANBusSetupForm))]
    public class ConnectionCANBus : ConnectionScannerBase
    {
        public event System.EventHandler CurrentScanRoomChanged;

        protected int mDisconnectRetryTimes = 2;
        protected int mScanDelay = 1000;

        protected Dictionary<string, int> _DisconnectList = new Dictionary<string, int>();
        protected Hashtable _LastReceivedTime = new Hashtable();
        protected System.Timers.Timer _ScannerTimeout = new System.Timers.Timer();
        protected System.Timers.Timer _SendTimeToATMTimer = new System.Timers.Timer();
        protected CANBusConnection _Connection;
        protected CANBusInitConfig _Config;

        protected bool _WhetherSendTimeToATM = true;
        protected int _SendTimeToATMFrequency = 30;		//Seconds;
        protected bool _DisableDetectTemperature = false;
        protected int _AcceptInterval = 1;
        protected int _RoomStatusChangerFloorNum = 79;
        protected int _RoomStatusChangerRoomNum = 79;
        protected Settings baseSetting;
        //protected bool mMultiChannel;		//Whether Use 0 1 Channel


        public ConnectionCANBus(FloorSet floorSet, SettingsBase connectionSettings)
            : base(floorSet, connectionSettings)
        {
            //Load Default Options
            _Settings = connectionSettings;
            _FloorSet = floorSet;
            _FloorSet.WakingGuestUp += new WakingGuestUpEventHandler(_FloorSet_WakingGuestUp);
            ResetSetting();

            baseSetting = _Settings.ParentSettings.ParentSettings as Settings;
            //Connection Config
            //mConfig=new CANBusInitConfig();
            //mConfig.AcceptCode=0x00000000;
            //mConfig.MaskCode=0xFFFFFFFF;
            //mConfig.RunningMode=RunningMode.Normal;
            //mConfig.Timing0=0x67;
            //mConfig.Timing1=0x2f;

            //Load User Define Settings

            //mConnection=new CANBusConnection(mConfig);
            //mConnection.DeviceType=(CANDeviceType)(Convert.ToInt32(ConnectionSettings["DeviceType",0]));
            //mConnection.DeviceInd=Convert.ToInt32(ConnectionSettings["DeviceInd",0]);
            //mConnection.CANInd=Convert.ToInt32(ConnectionSettings["CANInd",0]);
            //mConnection.UseAllCANInd = Convert.ToBoolean(ConnectionSettings["MultiChannel",false]);

            //this.mWhetherSendTimeToATM=
            //mDisconnectRetryTimes=Convert.ToInt32(ConnectionSettings["DisconnectRetryTimes",2]);
            //mScanDelay=Convert.ToInt32(ConnectionSettings["ScanDelay",1000]);
            //mSendTimeToATMFrequency=Convert.ToInt32(ConnectionSettings["SendTimeToATMFrequency",30]);


            //Apply Options

            _ScannerTimeout.Elapsed += new System.Timers.ElapsedEventHandler(mScannerTimeout_Elapsed);
            _SendTimeToATMTimer.Elapsed += new System.Timers.ElapsedEventHandler(mSendTimeToATMTimer_Elapsed);
            //mScannerTimeout.Interval=mScanDelay;
            //mSendTimeToATMTimer.Interval=mSendTimeToATMFrequency;


            // Connect All Rooms
            foreach (int floorNumber in _FloorSet.FloorNumbers)
            {
                foreach (Room room in _FloorSet[floorNumber].GetRooms())
                {
                    room.MyATM.Connect();
                }
            }

            // Handle Vacant Information of the Room
            floorSet.HotelUsingStatusChanged += new HotelUsingStatusChangedEventHandler(aFloorSet_HotelUsingStatusChanged);
        }

        #region 继承
        public override void ResetSetting()
        {
            base.ResetSetting();
            bool lastRunningStatus = base._IsRunning;

            if (base._IsRunning) this.Disconnnect();

            //Apply Settings

            // Load Default Options
            // Connection Config
            _Config = new CANBusInitConfig();
            _Config.AcceptCode = 0x00000000;
            _Config.MaskCode = 0xFFFFFFFF;
            _Config.RunningMode = RunningMode.Normal;
            byte t0, t1;
            byte.TryParse(_Settings["Timing0", 0x67].ToString(), out t0);
            byte.TryParse(_Settings["Timing1", 0x2f].ToString(), out t1);
            _Config.Timing0 = t0;
            _Config.Timing1 = t1;
            _Settings["Timing0"] = t0;
            _Settings["Timing1"] = t1;

            if (_Connection == null)
            {
                _Connection = new CANBusConnection(_Config);
                _Connection.NewFramesReceived += new EventHandler(mConnection_NewFramesReceived);
            }
            else
            {
                _Connection.InitConfig = _Config;
            }

            // Load User Define Settings
            _Connection.DeviceType = (CANDeviceType)Convert.ToInt32(_Settings["DeviceType", 0]);
            _Connection.DeviceInd = Convert.ToInt32(_Settings["DeviceInd", 0]);

            if (_Settings["Channels", string.Empty].ToString() == string.Empty)
            {
                // The channels is not unavailable
                // Try to use older options
                _Connection.CANInd = Convert.ToInt32(_Settings["CANInd", 0]);
                _Connection.UseAllCANInd = Convert.ToBoolean(_Settings["MultiChannel", false]);
            }
            else
            {
                // Paser the channels data
                string[] chns = _Settings["Channels"].ToString().Split(';');

                List<int> chnls = new List<int>();
                for (int i = 0; i < chns.Length; i++)
                {
                    try
                    {
                        chnls.Add(Convert.ToInt32(chns[i]));
                    }
                    catch
                    {
                    }
                }

                _Connection.Channels = chnls.ToArray();
            }



            _DisableDetectTemperature = Convert.ToBoolean(_Settings["DisableDetectTemperature", false]);
            _AcceptInterval = Convert.ToInt32(_Settings["AcceptInterval", 1]);

            // this.mWhetherSendTimeToATM=
            mDisconnectRetryTimes = Convert.ToInt32(_Settings["DisconnectRetryTimes", 2]);
            mScanDelay = Convert.ToInt32(_Settings["ScanDelay", 1000]);
            _SendTimeToATMFrequency = Convert.ToInt32(_Settings["SendTimeToATMFrequency", 5]);

            // Apply Options

            _ScannerTimeout.Interval = mScanDelay;
            _SendTimeToATMTimer.Interval = _SendTimeToATMFrequency * 1000;

            // Other
            CurrentRoom = _FloorSet.FirstFloor.FirstRoom;

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

        public SeasonType Season
        {
            get
            {
                return this._FloorSet.FirstFloor.FirstRoom.MyAirConditioner.Season;
            }
        }


        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("标准CANBus连接协议",
                "{0140FA7F-80ED-4f54-AFC7-C4D1C49404DE}",
                new Version("1.0.0"),
                "通过标准CANBus获取房间信息。");
        }

        public override bool ShowSetupForm()
        {
            ConnectionCANBusSetupForm aForm = new ConnectionCANBusSetupForm();
            aForm.Settings = this._Settings;
            aForm.ShowDialog();

            return aForm.DialogResult == DialogResult.OK;
        }

        public override void Connect()
        {
            if (!this.IsRunning)
            {
                _Connection.Disconnect();
                try
                {
                    _Connection.Connect();
                    System.Threading.Thread.Sleep(100);
                    _Connection.StartCAN();
                }
                catch
                {
                    //Open CANBus Device Error
                    //TODO Add the Error to LOG
                    throw;                       
                }

                base.Connect();
                if (_WhetherSendTimeToATM) _SendTimeToATMTimer.Start();

                _FloorSet.DefaultRunningStatusChanged += new DefaultRunningStatusChangedEventHandler(mFloorSet_DefaultRunningStatusChanged);
                _FloorSet.HotelUsingStatusChanged += new HotelUsingStatusChangedEventHandler(_FloorSet_HotelUsingStatusChanged);
                _ScannerTimeout.Start();

                _IsRunning = true;

            }
        }


        public override void Disconnnect()
        {
            if (this._IsRunning)
            {
                base.Disconnnect();
                _ScannerTimeout.Stop();
                _SendTimeToATMTimer.Stop();
                _Connection.Disconnect();
                _IsRunning = false;
                _FloorSet.DefaultRunningStatusChanged -= new DefaultRunningStatusChangedEventHandler(mFloorSet_DefaultRunningStatusChanged);
                _FloorSet.HotelUsingStatusChanged -= new HotelUsingStatusChangedEventHandler(_FloorSet_HotelUsingStatusChanged);
            }
        }

        #endregion

        #region Receive Part
        //Check a valid-information room'data is too fast to accept
        protected bool AcceptToReceive(Room room)
        {
            if ((!_LastReceivedTime.ContainsKey(room)) || (((System.DateTime.Now - (DateTime)_LastReceivedTime[room]).TotalMilliseconds) >= _AcceptInterval))
            {
                _LastReceivedTime[room] = System.DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void WriteLog(string log)
        {
            //StreamWriter writer = new StreamWriter("c:\\ATMLOG.txt",true);
            //writer.WriteLine(log);
           // writer.Close();
        }

        //a new frame has been received
        //check the frame is correct and when is ok , parse it and apply to floorset
        protected virtual void mConnection_NewFramesReceived(object sender, EventArgs e)
        {
            lock (_Connection.ReceivedFrames)
            {
                WriteLog("开始：");
                WriteLog(_Connection.ReceivedFrames.Length.ToString());
                for (int i = 0; i < _Connection.ReceivedFrames.Length ; i++)
                {
                    WriteLog("Frames：");
                    WriteLog(_Connection.ReceivedFrames[i].DataLength.ToString());
#if !DEBUG
                    if (_Connection.ReceivedFrames[i].DataLength >= 8)
#else
                    if (_Connection.ReceivedFrames[i].DataLength >= 6)
#endif                    
                    {
                        int CheckSum = 0;
                        for (int j = 0; j < _Connection.ReceivedFrames[i].DataLength - 1; j++)
                        {
                            CheckSum += _Connection.ReceivedFrames[i].Data[j];
                            WriteLog("data" + j +_Connection.ReceivedFrames[i].Data[j].ToString());
                        }

                        WriteLog("CheckSum:" + CheckSum.ToString());

                        WriteLog(_Connection.ReceivedFrames[i].Data[_Connection.ReceivedFrames[i].DataLength - 1].ToString());
                        if (((byte)(CheckSum % 256) == _Connection.ReceivedFrames[i].Data[_Connection.ReceivedFrames[i].DataLength - 1]) )		//Received Success
                        {
                            int floorNum, roomNum;

                            WriteLog("FrameID:" + _Connection.ReceivedFrames[i].FrameID);
                            WriteLog("FrameID >> 21:" + (_Connection.ReceivedFrames[i].FrameID >> 21));
                            WriteLog("FrameID >> 13:" + (_Connection.ReceivedFrames[i].FrameID >> 13));
                            floorNum = (int)Utility.UNBCD((_Connection.ReceivedFrames[i].FrameID >> 21));
                            roomNum = (int)Utility.UNBCD(((_Connection.ReceivedFrames[i].FrameID >> 13) % 256));
                            
                            WriteLog("Room:" + floorNum + "_" + roomNum);

                            if ((_FloorSet[floorNum] != null) && (_FloorSet[floorNum].GetRoomByRoomNumber(roomNum) != null))
                            {
                                Room room = _FloorSet[floorNum].GetRoomByRoomNumber(roomNum);
                                
                                Trace.WriteLine(
                                    string.Format("Room {0}  can data frame received!", room),
                                    "Connection");

                                // Erase Disconnect List
                                if (_DisconnectList.ContainsKey(room.ToString()))
                                {
                                    _DisconnectList.Remove(room.ToString());
                                }

                                if ((room != null) && AcceptToReceive(room))
                                {

                                    //Apply Data
                                    BitArray aData = new BitArray(_Connection.ReceivedFrames[i].Data);

                                    //Key
                                    if (aData[0] == true)	//Clean
                                        room.MyGuestService.Clean();
                                    else
                                        room.MyGuestService.CancelService(ServiceType.Clean);

                                    if (aData[1] == true)	//Call
                                        room.MyGuestService.Call();
                                    else
                                        room.MyGuestService.CancelService(ServiceType.Call);

                                    if (aData[2] == true)	//DontDisturb
                                        room.MyGuestService.DontDisturb();
                                    else
                                        room.MyGuestService.CancelService(ServiceType.DontDisturb);

                                    //TODO Quit Room aData[3];
                                    if (aData[3] == true)
                                        room.MyGuestService.QuitRoom();
                                    else
                                        room.MyGuestService.CancelService(ServiceType.QuitRoom);

                                    //Emergency
                                    if (aData[4] == true)
                                        room.MyGuestService.Emergency();
                                    else
                                        room.MyGuestService.EmergencyCancel();

                                    //Coffer Open Close
                                    if (aData[5] == true)
                                        room.CofferOpen();
                                    else
                                        room.CofferClose();

                                    //IceBox
                                    if (aData[6] == true)
                                        room.RefrigeratorOpen();
                                    else
                                        room.RefrigeratorClose();

                                    //Door Open Close
                                    if (aData[7] == true)
                                        room.DoorOpen();
                                    else
                                        room.DoorClose();

                                    //// Repair Flag
                                    //if (aData[8] == true)
                                    //{
                                    //    room.HotelUsingStatus = HotelUsingStatusType.Maintanent;
                                    //}
                                    //else
                                    //{
                                    //    if (room.HotelUsingStatus == HotelUsingStatusType.Maintanent)
                                    //    {
                                    //        room.HotelUsingStatus = HotelUsingStatusType.Vacant;
                                    //    }
                                    //}

                                    //Problem Flag
                                    if (aData[9] == true)
                                        room.MyATM.ProblemCaused();
                                    else
                                        room.MyATM.ProblemRepaired();

                                    //Cleaning
                                    if (aData[10] == true)
                                        room.MyGuestService.Cleaning();
                                    else
                                        room.MyGuestService.CancelService(ServiceType.Cleaning);

                                    //TODO Check Room 
                                    if (aData[11] == true)
                                        room.MyGuestService.StartChecking();
                                    else
                                        room.MyGuestService.StopChecking();

                                    // 总制
                                    room.LightSystemOff = aData[12];

                                    if (aData[13] == true)
                                        room.MyGuestService.Wash();
                                    else
                                        room.MyGuestService.CancelService(ServiceType.Wash);

                               ////     room.IsVIP = aData[14];
                               //     if (aData[14] == true)
                               //         room.HotelUsingStatus = HotelUsingStatusType.VIP;

                                    

                                    if (!_DisableDetectTemperature)	//Disable Detect Air Con and Season And Temperature
                                    {
                                        //AirConditioner Speed
                                        switch (_Connection.ReceivedFrames[i].Data[2] % 4)
                                        {
                                            case 0:
                                                room.MyAirConditioner.Speed = AirConditionerSpeedType.Off;
                                                break;
                                            case 1:
                                                room.MyAirConditioner.Speed = AirConditionerSpeedType.Low;
                                                break;
                                            case 2:
                                                room.MyAirConditioner.Speed = AirConditionerSpeedType.Mid;
                                                break;
                                            case 3:
                                                room.MyAirConditioner.Speed = AirConditionerSpeedType.High;
                                                break;
                                        }

                                        if ((_Connection.ReceivedFrames[i].Data[2] / 4) % 4 <= 1)
                                            room.MyAirConditioner.TurnOn();
                                        else
                                            room.MyAirConditioner.TurnOff();


                                        //if ((_Connection.ReceivedFrames[i].Data[2] / 16) % 2 == 0)
                                        //    room.MyAirConditioner.Season = SeasonType.Summer;
                                        //else
                                        //    room.MyAirConditioner.Season = SeasonType.Winter;


                                        switch (_Connection.ReceivedFrames[i].Data[2] / 32)
                                        {
                                            case 1:
                                                room.HotelUsingStatus = HotelUsingStatusType.Rented;
                                                break;
                                            case 2:
                                                room.HotelUsingStatus = HotelUsingStatusType.Vacant;
                                                break;
                                            case 3:
                                                room.HotelUsingStatus = HotelUsingStatusType.Cleaning;
                                                break;
                                            case 4:
                                                room.HotelUsingStatus = HotelUsingStatusType.Booked;
                                                break;
                                            case 5:
                                                room.HotelUsingStatus = HotelUsingStatusType.Maintanent;
                                                break;
                                            case 6:
                                                room.HotelUsingStatus = HotelUsingStatusType.Empty;
                                                break;
                                            case 7:
                                                room.HotelUsingStatus = HotelUsingStatusType.VIP;
                                                break;
                                            case 0:
                                                break;
                                        }

                                        //Temperature
                                        room.Temperature = _Connection.ReceivedFrames[i].Data[3];
                                    }
                                    else
                                        room.Temperature = 20;		//Default Temperature

                                    //Key
                                    if (_Connection.ReceivedFrames[i].Data[4] > 0)
                                    {
                                        int CardType = _Connection.ReceivedFrames[i].Data[4];
                                        if (CardType == 3)
                                            room.KeyInsert(KeyStatusType.Leader);
                                        else if (CardType == 6)
                                            room.KeyInsert(KeyStatusType.Servant);
                                        else if (CardType == 8)
                                            room.KeyInsert(KeyStatusType.Cleaner);
                                        else if (CardType == 7)
                                            room.KeyInsert(KeyStatusType.Repairer);
                                        else
                                            //if (aData[14] == true)
                                            //    room.KeyInsert(KeyStatusType.VIPGuest);
                                            //else
                                            room.KeyInsert(KeyStatusType.Guest);
                                    }
                                    else
                                    {
                                        room.KeyPullOut();
                                    }
#if !DEBUG
                                    room.KeyID = _Connection.ReceivedFrames[i].Data[5].ToString();

                                    room.LightOnNumber = _Connection.ReceivedFrames[i].Data[6];
#endif
                                    
                                }

                                //Connect ATM
                                room.MyATM.Connect();


                            }
                            else if (floorNum == _RoomStatusChangerFloorNum && roomNum == _RoomStatusChangerRoomNum)
                            {

                                byte[] data = _Connection.ReceivedFrames[i].Data;
                                floorNum = data[0];
                                roomNum = data[1];

                               // WriteLog(string.Format("Outside Temperature is {0}.", outsideTemperature));
                                
                                baseSetting["OutsideTemperature"] = _Connection.ReceivedFrames[i].Data[3].ToString();

                                WriteLog("ChangeRoom:" + floorNum + "_" + roomNum);

                                if ((_FloorSet[floorNum] != null) && (_FloorSet[floorNum].GetRoomByRoomNumber(roomNum) != null))
                                {
                                    Room room = _FloorSet[floorNum].GetRoomByRoomNumber(roomNum);
                                    switch (data[2])
                                    {
                                        case 1:
                                            room.HotelUsingStatus = HotelUsingStatusType.Rented;
                                            break;
                                        case 2:
                                            room.HotelUsingStatus = HotelUsingStatusType.Vacant;
                                            break;
                                        case 3:
                                            room.HotelUsingStatus = HotelUsingStatusType.CheckOut;
                                            break;
                                        case 4:
                                            room.HotelUsingStatus = HotelUsingStatusType.Booked;
                                            break;
                                        case 5:
                                            room.HotelUsingStatus = HotelUsingStatusType.Maintanent;
                                            break;
                                        case 6:
                                            room.HotelUsingStatus = HotelUsingStatusType.Empty;
                                            break;
                                        case 7:
                                            room.HotelUsingStatus = HotelUsingStatusType.VIP;
                                            break;
                                        default:
                                            break;
                                    }

                                }
                                WriteLog("SendFeedback" + floorNum + "_" + roomNum);
                                SendFeedback(data);
                            }
                            else
                            {
                                WriteLog("No match room for " + floorNum + "_" + roomNum);
                            }
                        }

                    }
                }
            }
        }

        #endregion

        #region Send Part

        private void _FloorSet_WakingGuestUp(object sender, BaseRoomChangedEventArgs e)
        {
            //TODO WakeGuestUp feature
            //SendDataToATM(e.ChangedRoom, true);
        }

        protected void mScannerTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_DisconnectList.ContainsKey(_CurrentRoom.ToString()) &&
                (_DisconnectList[_CurrentRoom.ToString()] > mDisconnectRetryTimes))
            {
                _CurrentRoom.MyATM.Disconnect();
            }

            // Turn To Next Room
            CurrentRoom = NextRoom;

            // Set Disconnect Flag
            if (!_DisconnectList.ContainsKey(_CurrentRoom.ToString()))
            {
                _DisconnectList.Add(_CurrentRoom.ToString(), 0);
            }

            _DisconnectList[_CurrentRoom.ToString()] ++;

            // Send Data To CANBus
            SendDataToATM(_CurrentRoom);

        }


        protected void SendFeedback(byte[] feedbackData)
        {
            CANBusDataFrame aFrame = new CANBusDataFrame(CANSendType.Single, CANFrameType.Extern, CANFrameFormat.Data,
                (uint)((Utility.BCD(Convert.ToUInt32(_RoomStatusChangerFloorNum))) << 21) + (Utility.BCD(Convert.ToUInt32(_RoomStatusChangerRoomNum) << (int)13)));
            aFrame.DataLength = Convert.ToByte(feedbackData.Length);
            for (int i = 0; i < aFrame.DataLength; i++)
            {
                aFrame.Data[i] = feedbackData[i];
            }
            
            try
            {
                _Connection.SendFrame(aFrame);
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// Send Data to ATM (a room) include the rooms default temperature and air conditioner default running status
        /// </summary>
        /// <param name="aRoom"></param>
        /// <param name="wakeUp"></param>
        protected virtual void SendDataToATM(Room aRoom)
        {
            CANBusDataFrame aFrame = new CANBusDataFrame(CANSendType.Single, CANFrameType.Extern, CANFrameFormat.Data,
                (uint)((Utility.BCD(Convert.ToUInt32(aRoom.MyFloor.Number)) << 21) + (Utility.BCD(Convert.ToUInt32(aRoom.Number)) << (int)13)));

            aFrame.DataLength = 8;
            aFrame.Data[0] = (byte)Utility.BCD(Convert.ToUInt32(aRoom.MyFloor.Number));
            aFrame.Data[1] = (byte)(Utility.BCD(Convert.ToUInt32(aRoom.Number)));
            aFrame.Data[2] = (byte)(Convert.ToByte(aRoom.MyAirConditioner.DefaultTemperature) +
                ((aRoom.MyAirConditioner.DefaultRunningStatus ? 0 : 1) * 128));
            BitArray array = new BitArray(8);
            switch (aRoom.HotelUsingStatus)
            {
                case HotelUsingStatusType.CheckOut:
                    array[1] = true;
                    break;
                case HotelUsingStatusType.Rented:
                    array[0] = true;
                    break;
                case HotelUsingStatusType.Vacant:
                    array[2] = true;
                    break;
                case HotelUsingStatusType.Empty:
                    array[3] = true;
                    break;
                case HotelUsingStatusType.Booked:
                    array[4] = true;
                    break;
                case HotelUsingStatusType.VIP:
                    array[5] = true;
                    break;
                case HotelUsingStatusType.Maintanent:
                    array[7] = true;
                    break;
                default:
                    break;
            }
            // 睡眠模式设置 
            array[6] = aRoom.AutoSleep && aRoom.IsSleepTime;
       //     array[7] = aRoom.MyAirConditioner.ApplyRunning;//空调开关设置
            array.CopyTo(aFrame.Data, 3);
            aFrame.Data[4] = 0;//(byte)aRoom.MyAirConditioner.RunningType; //每小时开关时间分布
            aFrame.Data[5] = aRoom.AutoSleep ? (byte)aRoom.ChangeTemperature : (byte)0;//睡眠温度调节: 1~5度范围调节
            aFrame.Data[6] = (aRoom.HotelUsingStatus == HotelUsingStatusType.Empty || aRoom.HotelUsingStatus == HotelUsingStatusType.Vacant) ? (byte)0 : (byte)128;


            int CheckSum = 0;
            for (int i = 0; i <= 6; i++) CheckSum += aFrame.Data[i];
            aFrame.Data[7] = (byte)(CheckSum % 256);

            try
            {
                _Connection.SendFrame(aFrame);
            }
            catch
            {

            }
        }

        private CANBusDataFrame _SendTimeFrame = new CANBusDataFrame(CANSendType.Single, CANFrameType.Extern, CANFrameFormat.Data, 0x00000000);
        // Broadcast Time and Season Information to RCU
        protected virtual void mSendTimeToATMTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _SendTimeFrame.DataLength = 7;
               // _SendTimeFrame.DataLength = 4;
                DateTime currentTime = System.DateTime.Now;
                _SendTimeFrame.Data[0] = (byte)Utility.BCD(currentTime.Hour);
                _SendTimeFrame.Data[1] = (byte)Utility.BCD(currentTime.Minute);
                _SendTimeFrame.Data[2] = (byte)(Season == SeasonType.Summer ? 0 : 1);
                _SendTimeFrame.Data[3] = (byte)Utility.BCD(currentTime.Year - 2000);
                _SendTimeFrame.Data[4] = (byte)Utility.BCD(currentTime.Month);
                _SendTimeFrame.Data[5] = (byte)Utility.BCD(currentTime.Day);

                int CheckSum = 0;
                for (int i = 0; i <= 5; i++) CheckSum += _SendTimeFrame.Data[i];
               // for (int i = 0; i <= 2; i++) CheckSum += _SendTimeFrame.Data[i];

                _SendTimeFrame.Data[6] = (byte)(CheckSum % 256);
              //  _SendTimeFrame.Data[3] = (byte)(CheckSum % 256);

                if (_Connection.IsRunning) _Connection.SendFrame(_SendTimeFrame);
            }
            catch (ApplicationException )
            {
            }
        }
        #endregion

        private void mFloorSet_DefaultRunningStatusChanged(object sender, DefaultRunningStatusChangedEventArgs e)
        {
            SendDataToATM(e.ChangedRoom);
        }

        void _FloorSet_HotelUsingStatusChanged(object sender, HotelUsingStatusChangedEventArgs e)
        {
            SendDataToATM(e.ChangedRoom);
        }

        protected void aFloorSet_HotelUsingStatusChanged(object sender, HotelUsingStatusChangedEventArgs e)
        {

            
        }
    }

}
