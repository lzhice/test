using System;
//using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ATM3300.Common;
using System.Timers;
using ATM3300.Connection;
using Log;


namespace ATM3300.Connection
{
    /// <summary>
    /// Form1 的摘要说明。
    /// </summary>
    /// 


    //The Scanner Is Disabled
    [ConnectionVersionInfo("TCPIP连接",
                "{65F77E31-70B0-4d44-9805-439DA6B0A4C5}",
                "1.0.0",
                "通过TCPIP协议远程获取房间信息。", typeof(ConnectionTCPIPSetupForm))]
    public class Client : ConnectionScannerBase
    {

        private IPEndPoint MyServer;
        private Socket CurrentSock;
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);

        private IPAddress _LocalIP = IPAddress.Parse("127.0.0.1");

        private SettingsBase _TCPIPAddressSetting;

        private EventLogger _Logger = new EventLogger();
        private Thread CurrentReceiveThread;
        private delegate string _SetRoomPropertyDelegate(Room SetRoom, string NewValue);
        private Hashtable _SetRoomPropertyCommands = new Hashtable();
        private FileLogger _TCPICClientLog = new FileLogger();


        System.Timers.Timer _Timer = new System.Timers.Timer(1000);

        #region 继承
        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("TCPIP连接",
                "{65F77E31-70B0-4d44-9805-439DA6B0A4C5}",
                new Version("1.0.0"),
                "通过TCPIP协议远程获取房间信息。");
        }

        public override bool ShowSetupForm()
        {
            ConnectionTCPIPSetupForm aForm = new ConnectionTCPIPSetupForm();
            aForm.Settings = this.Settings;
            aForm.ShowDialog();

            return aForm.DialogResult == DialogResult.OK;
        }

        public override void Connect()
        {
            base.Connect();
            ClientConnect();//Start
            _IsRunning = true;
            HandleEvents();
        }

        public override void Disconnnect()
        {
            base.Disconnnect();
            _TCPICClientLog.WriteLog("ClientDisConnect is calling now!");
            ClientDisConnect();  //Stop
            _IsRunning = false;
            UnHandleEvents();
        }

        public override FloorSet FloorSet
        {
            get
            {
                return _FloorSet;
            }
        }
        public override void ResetSetting()
        {
            base.ResetSetting();
            bool lastRunningStatus = base._IsRunning;
            if (this._IsRunning)
                this.Disconnnect();

            _LocalIP = IPAddress.Parse(_TCPIPAddressSetting["IP"].ToString());
            MyServer = new IPEndPoint(_LocalIP, Int32.Parse(_TCPIPAddressSetting["Port"].ToString()));

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

        public override SettingsBase Settings
        {
            get
            {
                return _Settings;
            }
        }
        #endregion

        public Client(FloorSet floorSet, SettingsBase setting)
            : base(floorSet, setting)
        {
            _FloorSet = floorSet;
            _Settings = setting;
            if (setting == null)
            {
                _Logger.WriteLog("Setting为空，Client实例化失败");
            }
            else
            {
                _TCPIPAddressSetting = setting;
                if (_TCPIPAddressSetting["IP"] == null)
                {
                    _TCPIPAddressSetting["IP"] = "127.0.0.1";
                    setting.Save();
                }
                if (_TCPIPAddressSetting["Port"] == null)
                {
                    _TCPIPAddressSetting["Port"] = "15000";
                    setting.Save();
                }
            }
            _LocalIP = IPAddress.Parse(_TCPIPAddressSetting["IP"].ToString());
            MyServer = new IPEndPoint(_LocalIP, Int32.Parse(_TCPIPAddressSetting["Port"].ToString()));
            _CurrentRoom = floorSet.FirstFloor.FirstRoom;
            _TCPICClientLog.FileName = "TCPIPClient.log";

            // Create a new Timer with Interval set to 10 seconds.
            _Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            InitCommands();

            HandleEvents();

            //Connect All Rooms
            foreach (int floorNumber in _FloorSet.FloorNumbers)
            {
                foreach (Room room in _FloorSet[floorNumber].GetRooms())
                {
                    room.MyATM.Connect();
                }
            }
        }

        public void InitCommands()
        {
            _SetRoomPropertyCommands.Add("REFRIGERATOR", new _SetRoomPropertyDelegate(ProcessRefrigeratorCommand));
            _SetRoomPropertyCommands.Add("COFFER", new _SetRoomPropertyDelegate(ProcessCofferCommand));
            _SetRoomPropertyCommands.Add("CONNECTION", new _SetRoomPropertyDelegate(ProcessConnectionCommand));
            _SetRoomPropertyCommands.Add("PROBLEM", new _SetRoomPropertyDelegate(ProcessProblemCommand));
            _SetRoomPropertyCommands.Add("ROOMSTATUS", new _SetRoomPropertyDelegate(ProcessRoomStatusCommand));
            _SetRoomPropertyCommands.Add("QUITROOM", new _SetRoomPropertyDelegate(ProcessQuitRoomCommand));
            _SetRoomPropertyCommands.Add("KEY", new _SetRoomPropertyDelegate(ProcessKeyCommand));
            _SetRoomPropertyCommands.Add("DONTDISTURB", new _SetRoomPropertyDelegate(processdontdistrub));
            _SetRoomPropertyCommands.Add("LIGHTSYSTEMOFF", new _SetRoomPropertyDelegate(processlightsystemoff));
            _SetRoomPropertyCommands.Add("CALL", new _SetRoomPropertyDelegate(processcall));
            _SetRoomPropertyCommands.Add("WASHCLOTHES", new _SetRoomPropertyDelegate(processWashClothes));
            _SetRoomPropertyCommands.Add("CLEAN", new _SetRoomPropertyDelegate(processclean));
            _SetRoomPropertyCommands.Add("CHECHROOM", new _SetRoomPropertyDelegate(processCheckRoom));
            _SetRoomPropertyCommands.Add("LAMPNUMBER", new _SetRoomPropertyDelegate(processLampNumber));
            _SetRoomPropertyCommands.Add("EMERGENCY", new _SetRoomPropertyDelegate(processcEnergency));
            _SetRoomPropertyCommands.Add("DOORCHANGED", new _SetRoomPropertyDelegate(processcDoorchange));
            _SetRoomPropertyCommands.Add("SPEEDCHANGED", new _SetRoomPropertyDelegate(processcSpeedchange));
            _SetRoomPropertyCommands.Add("TEMPERATURECHANGED", new _SetRoomPropertyDelegate(processcTemperaturechange));

        }

        public void HandleEvents()
        {
            _FloorSet.HotelUsingStatusChanged += new HotelUsingStatusChangedEventHandler(mFloorSet_HotelUsingStatusChanged);
            _FloorSet.DefaultRunningStatusChanged += new DefaultRunningStatusChangedEventHandler(mFloorSet_DefaultRunningStatusChanged);
            _FloorSet.RoomStatusChanged += new RoomPropertyChangedEventHandler(mFloorSet_RoomStatusChanged);
        }

        public void UnHandleEvents()
        {
            _FloorSet.HotelUsingStatusChanged -= new HotelUsingStatusChangedEventHandler(mFloorSet_HotelUsingStatusChanged);
            _FloorSet.DefaultRunningStatusChanged -= new DefaultRunningStatusChangedEventHandler(mFloorSet_DefaultRunningStatusChanged);
            _FloorSet.RoomStatusChanged -= new RoomPropertyChangedEventHandler(mFloorSet_RoomStatusChanged);
        }

        #region 连接方法

        private void ClientDisConnect()
        {
            try
            {
                CurrentReceiveThread.Abort();
                CurrentSock.Close();
                _Timer.Stop();
            }
            catch (Exception ee)
            {
                _Logger.WriteLog(ee.Message.ToString() + ee.Source.ToString());
            }
        }

        public void ClientConnect()
        {

            try
            {
                connectDone.Reset();
                CurrentSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                CurrentSock.BeginConnect(MyServer, new AsyncCallback(ConnectCallback), CurrentSock);
                connectDone.WaitOne(1000, true);
                if (CurrentSock.Connected == false)
                {
                    Disconnnect();
                    //	MessageBox.Show("Do not connect yet.");
                    Console.WriteLine("Do not connect yet.");
                    _Logger.WriteLog("尚未连接");
                }
                else
                {
                    StartScan();
                }
            }
            catch (Exception ee)
            {
                _Logger.WriteLog("连接失败：" + ee.Message.ToString() + ee.Source.ToString());
                //	MessageBox.Show(ee.Message);
            }

        }


        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                //获取状态
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                //自动发送数据
                try
                {
                    byte[] byteData = System.Text.Encoding.ASCII.GetBytes(" ");
                    //异步开始发送
                    CurrentSock.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), CurrentSock);


                }
                catch (Exception ee)
                {
                    _Logger.WriteLog("发送失败：" + ee.Message.ToString() + ee.Source.ToString());
                }
                //定义线程
                CurrentReceiveThread = new Thread(new ThreadStart(DataReceiver));
                CurrentReceiveThread.Start();
                connectDone.Set();
            }
            catch
            {
            }
        }

        #endregion

        #region 发送数据方法
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                sendDone.Set();


            }
            catch (Exception ee)
            {
                //	MessageBox.Show(ee.Message);
                _Logger.WriteLog("发送失败：" + ee.Message.ToString() + ee.Source.ToString());
            }
        }


        public void SendCmd(string aCmd)
        {
            if (_IsRunning)
            {
                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(aCmd + "\r\n");
                //开始异步发送数据
                CurrentSock.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), CurrentSock);
            }
        }

        #endregion

        #region 处理由DataProvider返回的命令
        private string ProcessRefrigeratorCommand(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.RefrigeratorOpen();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.RefrigeratorClose();
            }
            return null;
        }

        private string ProcessCofferCommand(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.CofferOpen();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.CofferClose();
            }
            return null;
        }

        private string ProcessProblemCommand(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyATM.ProblemCaused();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyATM.ProblemRepaired();
            }
            return null;
        }

        private string ProcessConnectionCommand(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyATM.Connect();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyATM.Disconnect();
            }
            return null;
        }

        private string ProcessRoomStatusCommand(Room SetRoom, string NewValue)
        {
            try
            {
                SetRoom.HotelUsingStatus = (HotelUsingStatusType)Enum.Parse(typeof(HotelUsingStatusType), NewValue, true);
            }
            catch
            {
            }
            return null;
        }
        //温度
        private string processcTemperaturechange(Room SetRoom, string NewValue)
        {
            int value;
            if (int.TryParse(NewValue, out value))
            {
                SetRoom.Temperature = value;
            }
            return null;
        }

        //LampNumber
        private string processLampNumber(Room SetRoom, string NewValue)
        {
            int value;
            if (int.TryParse(NewValue, out value) && value >= 0)
            {
                SetRoom.LightOnNumber = value;
            }
            return null;
        }
        
        //退房
        private string ProcessQuitRoomCommand(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.QuitRoom();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.CancelService(ServiceType.QuitRoom);
            }
            return null;
        }
        //CheckRoom
        private string processCheckRoom(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.StartChecking ();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.StopChecking();
            }
            return null;
        }
        //勿扰
        private string processdontdistrub(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.DontDisturb();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.CancelService(ServiceType.DontDisturb);
            }
            return null;
        }
        //洗衣
        private string processWashClothes(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.Wash();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.CancelService(ServiceType.Wash);
            }
            return null;
        }
        //总制
        private string processlightsystemoff(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.LightSystemOff = true;
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.LightSystemOff = false;
            }
            return null;
        }
        //服务
        private string processcall(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.Call();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.CancelService(ServiceType.Call);
            }
            return null;
        }
        //空调风速
        private string processcSpeedchange(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("OFF", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyAirConditioner.TurnOff();
            }
            else if (NewValue.Equals("ON", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyAirConditioner.TurnOn();
            }
            else if (NewValue.Equals("LOW", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyAirConditioner.Speed = (AirConditionerSpeedType)1;
            }
            else if (NewValue.Equals("MID", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyAirConditioner.Speed = (AirConditionerSpeedType)2;
            }
            else if (NewValue.Equals("HIGH", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyAirConditioner.Speed = (AirConditionerSpeedType)3;
            }
            return null;
        }

        //房门
        private string processcDoorchange(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("OPEN", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.DoorOpen();
            }
            else if (NewValue.Equals("CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.DoorClose();
            }
            return null;
        }

        //清洁
        private string processclean(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.Clean();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.CancelService(ServiceType.Clean);
            }
            return null;
        }
        //紧急乎叫
        private string processcEnergency(Room SetRoom, string NewValue)
        {
            if (NewValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.Emergency();
            }
            else if (NewValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                SetRoom.MyGuestService.EmergencyCancel();
            }
            return null;
        }
        //
        private string ProcessKeyCommand(Room SetRoom, string NewValue)
        {
            if (Enum.IsDefined(typeof(KeyStatusType), NewValue))
            {
                if (NewValue == KeyStatusType.NotInserted.ToString())
                    SetRoom.KeyPullOut();
                else
                    SetRoom.KeyInsert((KeyStatusType)Enum.Parse(typeof(KeyStatusType), NewValue, true));
            }
            return null;
        }
        #endregion


        #region 获取数据方法



        private void DataReceiver()
        {
            byte[] Buffer = new byte[16384];
            Encoding ASCII = Encoding.ASCII;
            int RevBytes;
            string answer = "";


            while (Thread.CurrentThread.IsAlive)
            {
                try
                {
                    RevBytes = CurrentSock.Receive(Buffer, 0, CurrentSock.Available, SocketFlags.None);
                    answer += ASCII.GetString(Buffer, 0, RevBytes);
                    if (answer.IndexOf("\n") != -1)
                    {
                        answer = answer.Trim(new char[] { '\n', '\r' });
                        ReadAnswer(answer);
                        answer = "";
                    }
                }
                catch (Exception e)
                {
                    _Logger.WriteLog("具体信息： " + e.Message.ToString() + e.Source.ToString());
                    _TCPICClientLog.WriteLog("具体信息： " + e.Message.ToString() + e.Source.ToString());
                    _TCPICClientLog.WriteLog("Disconnnect");
                    Disconnnect();
                    Thread.CurrentThread.Abort();

                }

            }

        }

        private void ReadAnswer(string answer)
        {
            answer = answer.Trim();
            switch (answer[0])
            {
                case '@': if (answer.Length <= 5) this.RGetFloorSet(answer);
                    break;
                case '$': if (answer.Length <= 5) this.RCountFloor(answer);
                    break;
                case 'F':
                    string[] items = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (answer.StartsWith("Floor")  && items.Length == 4)
                    {
                        this.RCountFloor("$" + items[3]);
                    }
                    break;
                case '#':
                case '%':
                    {
                        //   string[] str;
                        string[] strs = answer.Split(new char[] { '#', '\r', '\n', '*',' ', '%' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (strs[i].Trim().Length == 23 )//&& strs[i].Trim().EndsWith("*"))
                            {
                                RGetRoom(strs[i].Trim());
                            }
                        }

                        break;
                    }

                default:
                    string[] Commands = null;
                    Commands = answer.Split(' ');
                    if (Commands.Length != 3) break;
                    Room SetRoom = _FloorSet.Parse(Commands[1]);
                    if ((SetRoom != null) && (_SetRoomPropertyCommands[Commands[0].ToUpper()] != null))
                    {
                        ((_SetRoomPropertyDelegate)(_SetRoomPropertyCommands[Commands[0].ToUpper()]))(SetRoom, Commands[2]);
                    }
                    break;
            }
        }
        private void RGetFloorSet(string answer)
        {
            //TODO
        }
        private void RCountFloor(string answer)
        {
            //TODO
        }
        private void RGetRoom(string answer)
        {
            try
            {
                if (answer.Length != 23)
                {
                    return;
                }
                char[] AnswerList = answer.ToCharArray();

                // Retrieve the room
                int floorNum = Convert.ToInt32(answer.Substring(0,2));
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

                //Move the lightOnNumber to the last
              //  room.LightOnNumber = int.Parse(AnswerList[11].ToString());

                //switch (AnswerList[11])
                //{
                //    case '0': room.RefrigeratorClose();
                //        break;
                //    case '1': room.RefrigeratorOpen();
                //        break;
                //    default: break;
                //}

                switch (AnswerList[12])
                {
                    case '0': room.MyAirConditioner.Speed = (AirConditionerSpeedType)0;
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
               
                room.Temperature = int.Parse(answer.Substring(17,2));

                room.LightOnNumber = int.Parse(answer.Substring(19, 2));

            }
            catch
            {
            }

        }

        #endregion

        public void StartScan()
        {

            // Only raise the event the first time Interval elapses.
            _Timer.Start();
            _CurrentRoom = _FloorSet.FirstFloor.FirstRoom;
            StartScanRoom();
        }


        private void StartScanRoom()
        {
            //TODO NOTE This Scan Room is disabled;
            SendCmd("GetRoom "+CurrentRoom.ToString()+"\r\n");
        }


        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (CurrentRoom)
            {
                _CurrentRoom = NextRoom;
            }
            StartScanRoom();
        }

        private void mFloorSet_RoomStatusChanged(object sender, RoomPropertyChangedEventArgs e)
        {
            if (e.PropertyName == RoomPropertyChangeName.LightSystemOff)
            {
                if (e.ChangedRoom.LightSystemOff == false)
                {
                    SendCmd("LightSystemOn" + e.ChangedRoom.ToString());
                }
                else
                {
                    SendCmd("LightSystemOff" + e.ChangedRoom.ToString());
                }
            }
        }

        private void mFloorSet_HotelUsingStatusChanged(object sender, HotelUsingStatusChangedEventArgs e)
        {
            if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.Rented)
                SendCmd("Rent " + e.ChangedRoom.ToString());
            else if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.Empty)
                SendCmd("Empty " + e.ChangedRoom.ToString());
            else if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.CheckOut)
                SendCmd("CheckOut " + e.ChangedRoom.ToString());
            else if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.Booked)
                SendCmd("Book " + e.ChangedRoom.ToString());
            else if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.Maintanent)
                SendCmd("Maintanent " + e.ChangedRoom.ToString());
            else if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.Vacant)
                SendCmd("Vacant " + e.ChangedRoom.ToString());
            else if (e.ChangedRoom.HotelUsingStatus == HotelUsingStatusType.VIP)
                SendCmd("VIP " + e.ChangedRoom.ToString());
        }

        private void mFloorSet_DefaultRunningStatusChanged(object sender, DefaultRunningStatusChangedEventArgs e)
        {
            if (e.ChangedRoom.MyAirConditioner.DefaultRunningStatus)
                SendCmd("SetAirConOn " + e.ChangedRoom.ToString());
            else
                SendCmd("SetAirConOff " + e.ChangedRoom.ToString());
        }
    }
}


