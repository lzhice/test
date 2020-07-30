#region Version information
//Version Alpha1
//Use File Watcher And Text to Get Data
//Written By ZQ
//Use TestOfConnectionText Tested but not Use NUnit yet
//Date  [3/20/2004]
//Version Alpha2
//Use try to avoid error data.Add HotelUsingStatus Command
//Written BY ZQ
//Date [3/28/2004]
//Version Beta1
//Changed a lot interface with ConnectionBase
//Connect Room When New
//EDIT BY ZQ
//Date [5/25/2004]
#endregion
using System;
using ATM3300.Common;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
namespace ATM3300.Connection
{

    [ConnectionVersionInfo("文件连接协议",
                "{2E0112ED-9D51-423a-9C8C-A564B61C11B3}",
                "1.0.0",
                "通过监视一个文件的内容改变来获取房间信息。" , typeof(ConnectionFILESetupForm))]
    public class ConnectionTriggerFileText : ConnectionBase
    {
        protected string _FileName;
        protected System.IO.StreamReader mFileReader;
        protected System.IO.FileSystemWatcher _FileWatcher = new FileSystemWatcher();
        private int ReadFileFailureRetryTimes = 0;
        private int MaxFileFailureRetryTimes = 10;
        private Log.EventLogger ErrorLog = new Log.EventLogger();

        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("文件连接协议",
                "{2E0112ED-9D51-423a-9C8C-A564B61C11B3}",
                new Version("1.0.0"),
                "通过监视一个文件的内容改变来获取房间信息。");
        }

        public override bool ShowSetupForm()
        {
            ConnectionFILESetupForm aForm = new ConnectionFILESetupForm();
            aForm.Settings = this.Settings;
            aForm.ShowDialog();

            return aForm.DialogResult == DialogResult.OK;
        }

        public ConnectionTriggerFileText(FloorSet floorSet, SettingsBase setting)
            : base(floorSet, setting)
        {
            //获取初始化数据
            _FloorSet = floorSet;
            _Settings = setting;
            _FileName = (string)setting["FileName"];
            _FileWatcher.Filter = Path.GetFileName(_FileName);
            _FileWatcher.Path = Path.GetPathRoot(_FileName);
            _FileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _FileWatcher.Changed += new FileSystemEventHandler(mFileWatcher_Changed);
            
            //设置所有的Room Connect
            foreach (int FloorNumber in _FloorSet.FloorNumbers)
            {
                foreach (Room room in _FloorSet[FloorNumber].GetRooms())
                {
                    room.MyATM.Connect();
                }
            }
        }

        public override void Connect()
        {
            base.Connect();
            ReadFileFailureRetryTimes = 0;
            _FileWatcher.EnableRaisingEvents = true;
        }
        public override void Disconnnect()
        {
            base.Disconnnect();
            _FileWatcher.EnableRaisingEvents = false;
        }

        public override FloorSet FloorSet
        {
            get
            {
                return this._FloorSet;
            }
        }

        public override SettingsBase Settings
        {
            get
            {
                return this._Settings;
            }
        }

        public override void ResetSetting()
        {
            base.ResetSetting();
            bool lastRunningStatus = base._IsRunning;
            if (this._IsRunning)
                this.Disconnnect();
            _FileName = (string)this._Settings["FileName"];

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


        public string TestTCPIPGetRoomStatusString(Room room)
        {
            string ss = string.Empty;
            //int floorNumber = room.MyFloor.Number;
            //int roomNumber = room.Number;
            //            Room aRoom = _FloorSet[Convert.ToInt32(floorNumber)][Convert.ToInt32(roomNumber)];
            //空格是留给TimeOut的

            string roomNunber = (room.ToFullNumber() < 1000) ? "0" + room.ToFullNumber().ToString() : room.ToFullNumber().ToString();
            string serviceType = (((int)room.MyGuestService.Service.ServiceValue) < 10) ?
               "0" + ((int)room.MyGuestService.Service.ServiceValue).ToString() : ((int)room.MyGuestService.Service.ServiceValue).ToString();
            ss = "#" + roomNunber +
                ((int)room.DoorStatus).ToString() +
                ((int)room.KeyStatus).ToString() +
                ((int)room.HotelUsingStatus).ToString() +
                (Convert.ToInt32((room.MyGuestService.IsEmergency))).ToString() +
                serviceType +
                (Convert.ToInt32((room.MyGuestService.MyRoom.IsLightOn)) * 2 +
                    Convert.ToInt32((room.MyGuestService.MyRoom.IsCofferOpen))).ToString() +
                (Convert.ToInt32((room.MyGuestService.MyRoom.IsRefrigeratorOpen))).ToString() +
                ((int)room.MyAirConditioner.Speed).ToString() +
                (Convert.ToInt32((room.MyAirConditioner.IsRunning))).ToString() +
                (Convert.ToInt32((room.MyAirConditioner.Season))).ToString() +
                (Convert.ToInt32((room.MyATM.IsHasProblem))).ToString() +
                (Convert.ToInt32((room.MyATM.IsConnected))).ToString() +

                (GetTemperature(room.Temperature)) +
                "* ";
            return ss;

        }

        private string GetTemperature(int temperature)
        {
            if (temperature < 0)
            {
                temperature = 0;
            }

            if (temperature > 99)
            {
                temperature = 99;
            }

            if (temperature < 10)
            {
                return "0" + temperature.ToString();
            }
            else
            {
                return temperature.ToString();
            }
        }

        private void TestTCPIPGetRoom(string answer)
        {
            try
            {
                if (answer.Length != 19)
                {
                    return;
                }
                char[] AnswerList = answer.ToCharArray();

                // Retrieve the room
                int floorNum = Convert.ToInt32(answer.Substring(0, 2));
                int roomNum = Convert.ToInt32(answer.Substring(2, 2));
                Room room = _FloorSet[floorNum].GetRoomByRoomNumber(roomNum);
                if (room == null)
                {
                    return;
                }

                //    room.KeyPullOut();
                switch (AnswerList[4])
                {
                    case '1':
                        //    room.DoorClose();
                        room.DoorOpen();
                        break;
                    case '0':
                        //    room.DoorOpen();
                        room.DoorClose();
                        break;
                    default: break;
                }


                switch (AnswerList[5])
                {

                    case '0': room.KeyPullOut();
                        break;
                    case '1': room.KeyInsert((KeyStatusType)1);
                        break;
                    case '2': room.KeyInsert((KeyStatusType)2);
                        break;
                    case '3': room.KeyInsert((KeyStatusType)3);
                        break;
                    case '4': room.KeyInsert((KeyStatusType)4);
                        break;
                    default: break;
                }
                room.HotelUsingStatus = (HotelUsingStatusType)(Convert.ToInt32(AnswerList[6].ToString()));

                switch (AnswerList[7])
                {
                    case '0': room.MyGuestService.EmergencyCancel();
                        break;
                    case '1': room.MyGuestService.Emergency();
                        break;
                    default: break;
                }
                //mFloorSet[FloorNum][RoomNum].MyGuestService.IsEmergency=Convert.ToBoolean(AnswerList[8]);


                int services = int.Parse(answer.Substring(8, 2));
                if (services != 0)
                {
                    if ((services & ServiceType.Call.ServiceValue) != 0)
                    {
                        room.MyGuestService.Call();
                    }
                    else
                    {
                        room.MyGuestService.CancelService(ServiceType.Call);
                    }

                    if ((services & ServiceType.Clean.ServiceValue) != 0)
                    {
                        room.MyGuestService.Clean();
                    }
                    else
                    {
                        room.MyGuestService.CancelService(ServiceType.Clean);
                    }

                    if ((services & ServiceType.DontDisturb.ServiceValue) != 0)
                    {
                        room.MyGuestService.DontDisturb();
                    }
                    else
                    {
                        room.MyGuestService.CancelService(ServiceType.DontDisturb);
                    }

                    if ((services & ServiceType.QuitRoom.ServiceValue) != 0)
                    {
                        room.MyGuestService.QuitRoom();
                    }
                    else
                    {
                        room.MyGuestService.CancelService(ServiceType.QuitRoom);
                    }

                    if ((services & ServiceType.Cleaning.ServiceValue) != 0)
                    {
                        room.MyGuestService.Cleaning();
                    }
                    else
                    {
                        room.MyGuestService.CancelService(ServiceType.Cleaning);
                    }

                    if ((services & ServiceType.Wash.ServiceValue) != 0)
                    {
                        room.MyGuestService.Wash();
                    }
                    else
                    {
                        room.MyGuestService.CancelService(ServiceType.Wash);
                    }
                }
                else
                {
                    room.MyGuestService.NoService();
                }

                switch (AnswerList[10])
                {
                    case '0':
                        room.CofferClose();
                        room.RefrigeratorClose();
                        room.LightSystemOff = false;
                        break;
                    case '1':
                        room.CofferOpen();
                        room.RefrigeratorClose();
                        room.LightSystemOff = false;
                        break;
                    case '2':
                        room.CofferClose();
                        room.RefrigeratorOpen();
                        room.LightSystemOff = false;
                        break;
                    case '3':
                        room.CofferOpen();
                        room.RefrigeratorOpen();
                        room.LightSystemOff = false;
                        break;
                    case '4':
                        room.CofferClose();
                        room.RefrigeratorClose();
                        room.LightSystemOff = true;
                        break;
                    case '5':
                        room.CofferOpen();
                        room.RefrigeratorClose();
                        room.LightSystemOff = true;
                        break;
                    case '6':
                        room.CofferClose();
                        room.RefrigeratorOpen();
                        room.LightSystemOff = true;
                        break;
                    case '7':
                        room.CofferOpen();
                        room.RefrigeratorOpen();
                        room.LightSystemOff = true;
                        break;
                    default: break;
                }

                room.LightOnNumber = int.Parse(AnswerList[11].ToString());

                switch (AnswerList[12])
                {
                    case '0': room.MyAirConditioner.TurnOn();
                        break;
                    case '1': room.MyAirConditioner.Speed = (AirConditionerSpeedType)1;
                        break;
                    case '2': room.MyAirConditioner.Speed = (AirConditionerSpeedType)2;
                        break;
                    case '3': room.MyAirConditioner.Speed = (AirConditionerSpeedType)3;
                        break;
                    default: break;
                }

                switch (AnswerList[13])
                {
                    case '0':
                        room.MyAirConditioner.TurnOff();
                        break;
                    case '1':
                        room.MyAirConditioner.TurnOn();
                        break;
                    default: break;
                }

                if ((AnswerList[14] < '4') && (AnswerList[14] >= '0'))
                    room.MyAirConditioner.Season = (SeasonType)((int)(AnswerList[14] - '0'));

                switch (AnswerList[15])
                {
                    case '0':
                        room.MyATM.ProblemRepaired();
                        break;
                    case '1':
                        room.MyATM.ProblemCaused();
                        break;
                    default: break;
                }

                switch (AnswerList[16])
                {
                    case '0':
                        room.MyATM.Disconnect();
                        break;
                    case '1':
                        room.MyATM.Connect();
                        break;
                    default: break;
                }

                room.Temperature = int.Parse(answer.Substring(17, 2));

            }
            catch
            {
            }

        }

        private void TestCanBasSend(Room aRoom)
        {
            DateTime start = DateTime.Now;
            byte[] data = new byte[8];
            data[0] = (byte)Utility.BCD(Convert.ToUInt32(aRoom.MyFloor.Number));
            data[1] = (byte)(Utility.BCD(Convert.ToUInt32(aRoom.Number)));
            data[2] = (byte)(Convert.ToByte(aRoom.MyAirConditioner.DefaultTemperature) +
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
                default:
                    break;
            }
            // 睡眠模式设置 
            array[6] = aRoom.AutoSleep && aRoom.IsSleepTime;
            array[7] = aRoom.MyAirConditioner.ApplyRunning;//空调开关设置
            array.CopyTo(data, 3);
            data[4] = (byte)aRoom.MyAirConditioner.RunningType; //每小时开关时间分布
            data[5] = aRoom.AutoSleep ? (byte)aRoom.ChangeTemperature : (byte)0;//睡眠温度调节: 1~5度范围调节
            data[6] = (aRoom.HotelUsingStatus == HotelUsingStatusType.Empty || aRoom.HotelUsingStatus == HotelUsingStatusType.Vacant) ? (byte)0 : (byte)128;


            int CheckSum = 0;
            for (int i = 0; i <= 6; i++) CheckSum += data[i];
            data[7] = (byte)(CheckSum % 256);

            int time = DateTime.Now.Millisecond - start.Millisecond;
            long t = DateTime.Now.Ticks - start.Ticks;
            return;
        }

        private bool ParseFileData(string datas, out byte[] result)
        {
            int dataLength = 6;
            result = new byte[dataLength];

            if (string.Empty == datas )
            {
                return false;
            }

            string[] data = datas.Split(new char[] { ';' });
            if (data.Length == dataLength )
            {
                for (int i = 0; i < dataLength; i++)
                {
                    try
                    {
                        result[i] = Convert.ToByte(data[i]);
                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        protected void WriteLog(string log)
        {
          //  StreamWriter writer = new StreamWriter("c:\\ATMLOG.txt", true);
          //  writer.WriteLine(log);
          //  writer.Close();
        }

        private void TestCanBusReceived(string datas)
        {

            int _RoomStatusChangerFloorNum = 79;
            int _RoomStatusChangerRoomNum = 79;
            string[] dataArray = datas.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            uint frameID = Convert.ToUInt32(dataArray[0]);

            byte[] _Data = new byte[dataArray.Length - 1];
            for (int i = 0; i < _Data.Length; i++)
            {
                _Data[i] = Convert.ToByte(dataArray[i + 1]);
            }


            WriteLog("Frames：");

            int CheckSum = 0;
            for (int j = 0; j < _Data.Length - 1; j++)
            {
                CheckSum += _Data[j];
                WriteLog("data" + j + _Data[j].ToString());
            }

            WriteLog("CheckSum:" + CheckSum.ToString());

            WriteLog(_Data[_Data.Length - 1].ToString());
            if (((byte)(CheckSum % 256) == _Data[_Data.Length - 1]))		//Received Success
            {
                int floorNum, roomNum;

                WriteLog("FrameID:" + frameID);
                WriteLog("FrameID >> 21:" + (frameID >> 21));
                WriteLog("FrameID >> 13:" + (frameID >> 13));
                floorNum = (int)Utility.UNBCD((frameID >> 21));
                roomNum = (int)Utility.UNBCD(((frameID >> 13) % 256));

                WriteLog("Room:" + floorNum + "_" + roomNum);

                if ((_FloorSet[floorNum] != null) && (_FloorSet[floorNum].GetRoomByRoomNumber(roomNum) != null))
                {
                    Room room = _FloorSet[floorNum].GetRoomByRoomNumber(roomNum);

                    Trace.WriteLine(
                        string.Format("Room {0}  can data frame received!", room),
                        "Connection");


                    if ((room != null) )
                    {

                        //Apply Data
                        BitArray aData = new BitArray(_Data);

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

                        // Repair Flag
                        if (aData[8] == true)
                        {
                            room.HotelUsingStatus = HotelUsingStatusType.Maintanent;
                        }
                        else
                        {
                            if (room.HotelUsingStatus == HotelUsingStatusType.Maintanent)
                            {
                                room.HotelUsingStatus = HotelUsingStatusType.Vacant;
                            }
                        }

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

                        //     room.IsVIP = aData[14];
                        if (aData[14] == true)
                            room.HotelUsingStatus = HotelUsingStatusType.VIP;



                        if (true)	//Disable Detect Air Con and Season And Temperature
                        {
                            //AirConditioner Speed
                            switch (_Data[2] % 4)
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

                            if ((_Data[2] / 4) % 4 <= 1)
                                room.MyAirConditioner.TurnOn();
                            else
                                room.MyAirConditioner.TurnOff();

                            //Season
                            //if (aData[20]==true)	aRoom.MyAirConditioner.Season=SeasonType.Summer;
                            //else aRoom.MyAirConditioner.Season=SeasonType.Winter;

                            //Temperature
                            room.Temperature = _Data[3];
                        }
                        else
                            room.Temperature = 20;		//Default Temperature

                        //Key
                        if (_Data[4] > 0)
                        {
                            int CardType = _Data[4];
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
                        room.KeyID = _Data[5].ToString();

                        room.LightOnNumber = _Data[6];


                    }

                    //Connect ATM
                    room.MyATM.Connect();


                }
                else if (floorNum == _RoomStatusChangerFloorNum && roomNum == _RoomStatusChangerRoomNum)
                {

                    byte[] data = _Data;
                    floorNum = data[0];
                    roomNum = data[1];

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
                }
                else
                {
                    WriteLog("No match room for " + floorNum + "_" + roomNum);
                }
            }

        }

        private void mFileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string sCmd, sAction, sValue = "";
            string[] sSplit;
            int floorNum, roomNum;
            try
            {
                mFileReader = new StreamReader(File.Open(_FileName, FileMode.Open,FileAccess.Read , FileShare.None));
            }
            catch (Exception er)
            {
                ReadFileFailureRetryTimes++;
                ErrorLog.WriteLog("文件连接协议错误!错误信息为:" + er.Message + " 错误来源:" + er.Source
                    + " 重复连接次数:" + ReadFileFailureRetryTimes.ToString());
                if (ReadFileFailureRetryTimes >= MaxFileFailureRetryTimes)
                    this.Disconnnect();
                return;
            }

            while ((sCmd = mFileReader.ReadLine()) != null)  //not eof
            {
                sSplit = sCmd.Split(" ".ToCharArray(), 4);
                if (sSplit.Length >= 3)	//如果正确
                {
                    try
                    {
                        floorNum = Convert.ToInt32(sSplit[0]);
                        roomNum = Convert.ToInt32(sSplit[1]);
                        Room room = _FloorSet[floorNum].GetRoomByRoomNumber(roomNum);
                        if ((_FloorSet[floorNum] != null) && (room != null))
                        {
                            sAction = sSplit[2];
                            if (sSplit.Length >= 4) sValue = sSplit[3];
                            //TestCanBasSend(room);
                            TestTCPIPGetRoomStatusString(room);
                            switch (sAction) //apply action
                            {
                                //Room Action
                                case "DoorOpen": room.DoorOpen(); break;
                                case "DoorClose": room.DoorClose(); break;
                                case "KeyPullOut": room.KeyPullOut(); break;
                                case "KeyInsertGuest": room.KeyInsert(KeyStatusType.Guest); break;
                                case "KeyInsertServant": room.KeyInsert(KeyStatusType.Servant); break;
                                case "KeyInsertLeader": room.KeyInsert(KeyStatusType.Leader); break;
                   //             case "KeyInsertVIPGuest": room.KeyInsert(KeyStatusType.VIPGuest); break;
                                case "Temperature": room.Temperature = Convert.ToInt32(sValue); break;
                                case "CheckOut": room.HotelUsingStatus = HotelUsingStatusType.CheckOut; break;
                                case "Rented": room.HotelUsingStatus = HotelUsingStatusType.Rented; break;
                                case "Empty": room.HotelUsingStatus = HotelUsingStatusType.Empty; break;
                                case "Maintanent": room.HotelUsingStatus = HotelUsingStatusType.Maintanent; break;
                                case "Vacant": room.HotelUsingStatus = HotelUsingStatusType.Vacant; break;
                                case "Booked": room.HotelUsingStatus = HotelUsingStatusType.Booked; break;
                                case "Cleaning": room.HotelUsingStatus = HotelUsingStatusType.Cleaning; break;
                                case "VIP": room.HotelUsingStatus = HotelUsingStatusType.VIP; break;
                                case "RefrigeratorOpen": room.RefrigeratorOpen(); break;
                                case "RefrigeratorClose": room.RefrigeratorClose(); break;
                                case "CofferOpen": room.CofferOpen(); break;
                                case "CofferClose": room.CofferClose(); break;
                                case "LightOnNumber": room.LightOnNumber = Convert.ToInt32(sValue); break;
                                case "LightSystemOff": room.LightSystemOff = true; break;
                                case "LightSystemOn": room.LightSystemOff = false; break;
                               //case "LightClose": room.IsLightOn = false; break;
                                //case "IsVIP": room.IsVIP = true; break;
                                //case "IsNotVIP": room.IsVIP = false; break;

                                //AirConditioner
                                case "AirConditionerOn": room.MyAirConditioner.TurnOn(); break;
                                case "AirConditionerOff": room.MyAirConditioner.TurnOff(); break;
                                case "Season": room.MyAirConditioner.Season = (SeasonType)Convert.ToInt32(sValue); break;
                                case "Speed": room.MyAirConditioner.Speed = (AirConditionerSpeedType)Convert.ToInt32(sValue); break;
                                case "DefaultTemperature": room.MyAirConditioner.DefaultTemperature = Convert.ToInt32(sValue); break;
                                //ATM
                                case "Connect": room.MyATM.Connect(); break;
                                case "Disconnect": room.MyATM.Disconnect(); break;
                                case "ProblemCaused": room.MyATM.ProblemCaused(); break;
                                case "ProblemRepaired": room.MyATM.ProblemRepaired(); break;
                                //GuestService
                                case "Clean": room.MyGuestService.Clean(); break;
                                case "Call": room.MyGuestService.Call(); break;
                                case "Wash": room.MyGuestService.Wash(); break;
                                case "ServantCleaning": room.MyGuestService.Cleaning(); break;
                                case "CancelServantCleaning": room.MyGuestService.CancelService(ServiceType.Cleaning); break;
                                case "DontDisturb": room.MyGuestService.DontDisturb(); break;
                                case "QuitRoom": room.MyGuestService.QuitRoom(); break;
                                case "NoService": room.MyGuestService.NoService(); break;
                                case "Emergency": room.MyGuestService.Emergency(); break;
                                case "EmergencyCancel": room.MyGuestService.EmergencyCancel(); break;
                                case "StartChecking": room.MyGuestService.StartChecking(); break;
                                case "StopChecking": room.MyGuestService.StopChecking(); break;
                                default: /*TestTCPIPGetRoom(sAction);*/ TestCanBusReceived( sAction);  break;
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        Trace.WriteLine("文件连接协议错误!错误信息为:" + er.Message + " 错误来源:" + er.Source);
                    }
                    finally
                    {
                    }
                }

            }
            mFileReader.Close();
            try
            {
                File.Delete(_FileName);
            }
            catch
            {
            }
        }


    }
}
