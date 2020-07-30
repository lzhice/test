#region	 Version Information
//Version Alpha1
//Written BY ZQ
//Not Test yet.Maybe has some bugs.
//Date  [3/28/2004]
//Version Alpha2
//Written BY ZQ
//Pass COM Test (Use 2 computer)
//Date [4/24/2004]
#endregion
using System;
using System.Collections;
using Microsoft.VisualBasic;
using System.Diagnostics;
using ATM3300.Common;
using System.Windows.Forms;
using Log;
using System.IO.Ports;


namespace ATM3300.Connection
{
    /// <summary>
    /// DataProviderATMSimulator 的摘要说明。
    /// </summary>
    public class DataProviderATMSimulator : DataProviderBase
    {
        //protected RS232.Rs232 _RS232 = new Rs232();
        protected SerialPort _RS232 = new SerialPort();
        protected int mTimeoutDelay = 100;	//millionseconds

        protected System.Timers.Timer mTimeOutTimer = new System.Timers.Timer();

        protected Queue DataCache = new Queue();

        protected FloorSet _FloorSet;
        protected SettingsBase ProviderSetting;
        private EventLogger aLog = new EventLogger();

        private bool PaserSettings(string Settings)
        {
            string[] SStr = Settings.Split(',');
            string PStr = "noems";
            Parity[] parr = new Parity[]{
                Parity.None, Parity.Odd, Parity.Even , Parity.Mark , Parity.Space};

            try
            {
                _RS232.BaudRate = Convert.ToInt32(SStr[0]);
                _RS232.Parity = parr[PStr.IndexOf(SStr[1])];
                _RS232.DataBits = Convert.ToInt32(SStr[2]);
                switch (SStr[3].Trim())
                {
                    case "0":
                        _RS232.StopBits = StopBits.None;
                        break;
                    case "1":
                        _RS232.StopBits = StopBits.One;
                        break;
                    case "1.5":
                        _RS232.StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        _RS232.StopBits = StopBits.Two;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public DataProviderATMSimulator(FloorSet aFloorSet, SettingsBase aSetting)
            : base(aFloorSet, aSetting)
        {
            ProviderSetting = aSetting;
            //默认选项			
            _RS232.PortName = "COM2";
            _RS232.DtrEnable = false;
            _RS232.RtsEnable = false;
            _RS232.DataBits = 8;
            _RS232.ReadTimeout = 500;	//ms
            //_RS232. = Rs232.Mode.NonOverlapped;
            _RS232.ReadBufferSize = 1024;
            _RS232.WriteBufferSize = 1024;

            //mComm.EOFEnable=false;
            //mComm.Handshaking=HandshakeConstants.comNone;	//None Handshakeing
            //mComm.InputMode=InputModeConstants.comInputModeBinary;
            //mComm.NullDiscard=false;
            //mComm.ParityReplace="?";
            //mComm.RThreshold=1;	//R 闸值
            //mComm.SThreshold=0;	//S 闸值


            //应用选项
            _RS232.PortName = "COM" + Convert.ToInt16(ProviderSetting["CommPort", 2]);
            if (!PaserSettings(ProviderSetting["Settings", "2400,e,8,1"].ToString())) PaserSettings("2400,e,8,1");
            mTimeoutDelay = Convert.ToInt32(ProviderSetting["Timeout", 100]);

            mTimeOutTimer.Elapsed += new System.Timers.ElapsedEventHandler(mTimeOutTimer_Elapsed);

            _RS232.DataReceived += new SerialDataReceivedEventHandler(_RS232_DataReceived);

            _FloorSet = aFloorSet;
        }

        void _RS232_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            mTimeOutTimer.Stop();


            //接受数据
            byte[] RevData = new byte[_RS232.BytesToRead];
            _RS232.Read(RevData, 0, RevData.Length);

            for (int i = 1; i <= RevData.Length; i++)
                DataCache.Enqueue(RevData[i - 1]);

            Trace.WriteLine("RS232 Received:" + RevData.Length.ToString() + "bytes", "Connection");

            while (DataCache.Count > 0)
            {
                //判断是不是查询楼层
                if (IsScanningRoom())
                {
                    DealWithScanningRoom();
                }
                else if (IsSettingTime())
                {
                    DealWithSettingTime();
                }
                else
                {
                    DataCache.Dequeue();
                }
            }

            mTimeOutTimer.Start();
        }



        #region 继承的


        public override ClassInfo ClassInfo
        {
            get
            {
                return new ClassInfo("标准端口数据提供协议",
                    "{18F5C2D4-70A5-4285-98C6-B33C03E9D087}",
                    new Version("0.0.2"),
                    "这是通过标准端口提供数据的程序。");
            }
        }

        public override void Close()
        {
            base.Close();
            if (_RS232.IsOpen) _RS232.Close();
        }

        public override void Open()
        {
            try
            {
                base.Open();
                _RS232.Open();
            }
            catch (Exception e)
            {
                base.Close();
                aLog.WriteLog("COM启动错误，端口可能已被占用。具体信息： " + e.Message.ToString() + e.Source.ToString());
            }
        }

        public override bool IsRunning
        {
            get
            {
                return base.IsRunning;
            }
        }

        public override SettingsBase Settings
        {
            get
            {
                return ProviderSetting;
            }
        }

        public override bool ShowSetupForm()
        {
            DataProviderCOMSetupForm aForm = new DataProviderCOMSetupForm();
            aForm.Settings = this.Settings;
            aForm.ShowDialog();

            return aForm.DialogResult == DialogResult.OK;
        }

        #endregion

        /// <summary>
        /// 但接受数据超时时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTimeOutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _RS232.DiscardInBuffer();
        }

        private void ChangeSeason(SeasonType newSeason)
        {
            foreach (int floorNumber in _FloorSet.FloorNumbers)
            {
                foreach (Room room in _FloorSet[floorNumber].GetRooms())
                {
                    room.MyAirConditioner.Season = newSeason;
                }
            }
        }

        private void ChangeDefaultTemperature(int value)
        {
            foreach (int floorNumber in _FloorSet.FloorNumbers)
            {
                foreach (Room room in _FloorSet[floorNumber].GetRooms())
                {
                    room.MyAirConditioner.DefaultTemperature = value;
                }
            }
        }

        private bool IsScanningRoom()
        {
            if (DataCache.Count < 4) return false;

            byte[] aData = new byte[4];
            Array.Copy(DataCache.ToArray(), aData, 4);
            //判断数据
            if ((aData[0] < 0x80) || (aData[0] >= 0xf0) || (!Utility.IsCheckSumCorrect(aData)))
            {
                return false;
            }

            return true;
        }



        private void DealWithScanningRoom()
        {
            byte[] aData = new byte[4];
            Array.Copy(DataCache.ToArray(), aData, 4);

            for (int i = 1; i <= 4; i++)		//Delete Cache Data
                DataCache.Dequeue();

            int RoomNumber = aData[0] - 0x80, FloorNumber = aData[1];
            //TODO Apply BYTE3 Data(HotelUsingStatusType)

            //If the Room isn't exist
            if ((_FloorSet[FloorNumber] == null) || (_FloorSet[FloorNumber].GetRoomByRoomNumber(RoomNumber) == null))
            {
                return;
            }

            //return data
            byte[] RetData = new byte[5];
            RetData[0] = (byte)(256 - (RoomNumber + FloorNumber) % 256);
            Room theRoom = _FloorSet[FloorNumber].GetRoomByRoomNumber(RoomNumber);
            if (theRoom.MyATM.IsConnected == false) return;

            if (theRoom.KeyStatus != KeyStatusType.NotInserted) RetData[1] += 1;
            if (theRoom.DoorStatus == DoorStatusType.Open) RetData[1] += 2;
            if (theRoom.MyGuestService.Service == ServiceType.Call) RetData[1] += 4;
            if (theRoom.MyGuestService.Service == ServiceType.Clean) RetData[1] += 8;
            if (theRoom.MyGuestService.Service == ServiceType.DontDisturb) RetData[1] += 16;
            if (theRoom.HotelUsingStatus == HotelUsingStatusType.Maintanent) RetData[1] += 32;
            if (theRoom.MyAirConditioner.Speed == AirConditionerSpeedType.Off) RetData[1] += 0;
            if (theRoom.MyAirConditioner.Speed == AirConditionerSpeedType.Low) RetData[1] += 64;
            if (theRoom.MyAirConditioner.Speed == AirConditionerSpeedType.Mid) RetData[1] += 128;
            if (theRoom.MyAirConditioner.Speed == AirConditionerSpeedType.High) RetData[1] += 192;

            RetData[2] = (byte)theRoom.Temperature;
            if (theRoom.KeyStatus == KeyStatusType.Guest) RetData[3] += 1;
            if (theRoom.KeyStatus == KeyStatusType.Leader) RetData[3] += 2;
            if (theRoom.KeyStatus == KeyStatusType.Servant) RetData[3] += 3;

            if (theRoom.HotelUsingStatus == HotelUsingStatusType.Cleaning) RetData[3] += 8;

            if (theRoom.MyATM.IsHasProblem == true) RetData[3] += 16;

            for (int i = 1; i <= 3; i++) RetData[4] = (byte)((RetData[4] + RetData[i]) % 256);	//this is a special CheckSum
            //RetData[4]=Utility.MakeCheckSumValue(RetData);

            //输出数据
            _RS232.Write(RetData, 0, RetData.Length);
        }

        private bool IsSettingTime()
        {
            if (DataCache.Count < 8) return false;

            byte[] aData = new byte[8];
            Array.Copy(DataCache.ToArray(), aData, 8);

            //判断数据
            if ((aData[0] != 0xF0) || (aData[1] != 0xf))
            {
                return false;
            }
            for (int i = 2; i <= 4; i++)
                if (aData[i] != aData[i + 2])
                { return false; }
            return true;
        }

        private void DealWithSettingTime()
        {
            byte[] aData = new byte[8];
            Array.Copy(DataCache.ToArray(), aData, 8);

            for (int i = 1; i <= 8; i++)		//Delete Cache Data
                DataCache.Dequeue();

            int Hour, Minute;
            Hour = (aData[2] % 16) + (aData[2] / 16);
            Minute = (aData[3] % 16) + (aData[3] / 16);

            DateAndTime.TimeString = Hour.ToString() + ":" + Minute.ToString();
            if (aData[4] >= 0x40)
            {
                ChangeSeason(SeasonType.Winter);
                aData[4] -= 0x40;
            }
            else
            {
                ChangeSeason(SeasonType.Summer);
            }
            ChangeDefaultTemperature(aData[4]);

            return;
        }

    }
}
