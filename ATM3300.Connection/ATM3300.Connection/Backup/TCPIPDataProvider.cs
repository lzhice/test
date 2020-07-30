//  Version History
//  1.0.1 Add Wakeup feature
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ATM3300.Common;
using ATM3300.Connection;
using Log;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;


namespace ATM3300.Connection
{
    //	public delegate string MyDelegate(string s);
    //	public delegate string MyDelegate2();


    /// <summary>
    /// Create a new TcpipConversation and create a new thread to run it
    /// </summary>
    public class TcpConversation
    {
        #region Fields
        private EventLogger _Log = new EventLogger();
        private Socket _CurrentSocket = null;
        private TcpServer _Server;

        private delegate string _ProcessCommandHandler(string[] args);
        private Dictionary<string, _ProcessCommandHandler> _CommandTable = new Dictionary<string, _ProcessCommandHandler>();

        private FloorSet _FloorSet = null;
        public RoomView _RoomView;
        private Thread _CurrentThread;
        private DateTime _CreateTime;
        private DateTime _LastActivityTime;
        private bool _IsRunning = true;
        private string ERROR_INCORRECT_ROOM_NUMBER = "Error: The room number is not correct";
        private string ERROR_NOT_ENOUGH_ARGUMENTS = "Error: Not enough arguments";
        private string ERROR_FLOOR_NUMBER_NOT_EXIST = "Error: The floor number does not exist";
        private string ERROR_INCORRECT_FLOOR_NUMBER = "Error: The floor number is not correct";
        private string ERROR_COMMAND_NOT_SUPPORTED = "Error: The command is not supported";
        private string ERROR_ARGUMENT_NOT_NUMBER = "Error: The argument is not a number";
        private string ERROR_SEASON_INVALID = "Error: The Season is invalid";

        private delegate string _SetRoomPropertyDelegate(Room SetRoom, string NewValue);

        private Hashtable _SetRoomPropertyCommands = new Hashtable();


        private Thread CurrentReceiveThread;

        #endregion


        #region Constructor
        /// <summary>
        /// Constructor of TcpConversation
        /// </summary>
        /// <param name="currentSocket"></param>
        /// <param name="tcpIPServer"></param>
        public TcpConversation(Socket currentSocket, TcpServer tcpIPServer)
        {
            _CreateTime = DateTime.Now;
            _LastActivityTime = DateTime.Now;

            _CurrentSocket = currentSocket;
            _Server = tcpIPServer;
            _CurrentSocket.Blocking = true;

            this._CommandTable.Add("GETFLOORSET", new _ProcessCommandHandler(this.GetFloorSet));
            this._CommandTable.Add("COUNTFLOOR", new _ProcessCommandHandler(this.CountFloor));
            this._CommandTable.Add("GETROOM", new _ProcessCommandHandler(this.GetRoom));
            this._CommandTable.Add("GETROOMNUMBER", new _ProcessCommandHandler(this.GetRoomNumber));
            this._CommandTable.Add("DISCONNECT", new _ProcessCommandHandler(this.Disconnect));
            this._CommandTable.Add("CHECKOUT", new _ProcessCommandHandler(this.CheckOut));
            this._CommandTable.Add("SETAIRCONON", new _ProcessCommandHandler(this.SetAirconOn));
            this._CommandTable.Add("SETAIRCONOFF", new _ProcessCommandHandler(this.SetAirconOff));
            this._CommandTable.Add("EMPTY", new _ProcessCommandHandler(this.Empty));
            this._CommandTable.Add("RENT", new _ProcessCommandHandler(this.Rent));
            this._CommandTable.Add("VERSION", new _ProcessCommandHandler(this.Version));
            this._CommandTable.Add("WAKEUP", new _ProcessCommandHandler(this.WakeUp));
            this._CommandTable.Add("BOOK", new _ProcessCommandHandler(this.Book));
            this._CommandTable.Add("MAINTANENT", new _ProcessCommandHandler(this.Maintanent));
            this._CommandTable.Add("VIP", new _ProcessCommandHandler(this.VIP));
            this._CommandTable.Add("VACANT", new _ProcessCommandHandler(this.Vacant));
            this._CommandTable.Add("SETLIGHTSYSTEMOFF", new _ProcessCommandHandler(this.SetLightSystemOff));
            this._CommandTable.Add("SETLIGHTSYSTEMON", new _ProcessCommandHandler(this.SetLightSystemOn));
            this._CommandTable.Add("SETSEASON", new _ProcessCommandHandler(this.SetAllRoomSeason));

            _FloorSet = _Server.FloorSet;
            _RoomView = new RoomView(_FloorSet);


            _CurrentThread = new Thread(new ThreadStart(ConversationMain));
            _CurrentThread.IsBackground = true;
            _CurrentThread.Start();



        }
        #endregion


        #region Thread entry of converstaion
        private void ConversationMain()
        {
            byte[] buffer = new byte[1024];
            string command = string.Empty;

            while (_CurrentSocket.Connected)
            {
                try
                {
                    int revBytesNumber = _CurrentSocket.Receive(buffer, SocketFlags.None);

                    if (revBytesNumber == 0)
                    {
                        CloseSession();
                        return;
                    }

                    command += Encoding.ASCII.GetString(buffer, 0, revBytesNumber);

                    if (command.IndexOf("\n") != -1)
                    {
                        ProcessCommand(command.Split(new char[] { '\n', '\r' } , StringSplitOptions.RemoveEmptyEntries));
                        command = string.Empty;
                    }
                }
                catch (Exception e)
                {
                    _Log.WriteLog("具体信息： " + e.Message.ToString() + e.Source.ToString());

                    CloseSession();
                    return;
                }
            }
        }
        #endregion


        #region Public properties
        public IPEndPoint RemoteAddress
        {
            get
            {
                return _CurrentSocket.RemoteEndPoint as IPEndPoint;
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return _CreateTime;
            }
        }

        public DateTime LastActivityTime
        {
            get
            {
                return _LastActivityTime;
            }
        }
        #endregion


        #region SendMessage & CloseSession
        /// <summary>
        /// Send message to remote client. The empty or null message will not be sent
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                try
                {
                    byte[] byteData = System.Text.Encoding.ASCII.GetBytes(message + "\r\n");
                    //异步开始发送信息
                    _CurrentSocket.Send(byteData);

                }
                catch (Exception ee)
                {
                    _Log.WriteLog(ee.Message.ToString() + ee.Source.ToString());

                }
            }
        }

        /// <summary>
        /// Close the TcpConversation and announce the TcpServer
        /// </summary>
        public void CloseSession()
        {
            if (_IsRunning)
            {
                _CurrentSocket.Close();

                _Server.RemoveConversation(this);

                _IsRunning = false;
                _CurrentThread.Abort();

            }
        }
        #endregion


        #region Command respones functions
        /// <summary>
        /// Process client commands and deliver the correspond command to correct _ProcessCommandHandler
        /// </summary>
        /// <param name="commands"></param>
        private void ProcessCommand(string[] commands)
        {
            foreach (string command in commands)
            {
                string[] tokens = command.Split(new char[] { ' ' } , StringSplitOptions.RemoveEmptyEntries);
                string retMsg = string.Empty;

                if (tokens.Length == 0)
                {
                    // Do nothing
                }
                else
                {
                    if (!_CommandTable.ContainsKey(tokens[0].ToUpper()))
                    {
                        retMsg = ERROR_COMMAND_NOT_SUPPORTED;
                    }
                    else
                    {
                        // Obtain args array
                        string[] args = new string[tokens.Length - 1];
                        Array.Copy(tokens, 1, args, 0, tokens.Length - 1);

                        // Call command handler to process
                        retMsg = _CommandTable[tokens[0].ToUpper()].Invoke(args);
                    }

                    // Send Response Message
                    SendMessage(retMsg);
                }
            };
        }

        /// <summary>
        /// Obtain temperature in the 0-99
        /// </summary>
        /// <param name="temperature"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the room status information string according to floorNumber and roomNumber
        /// </summary>
        /// <param name="floorNumber"></param>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        public string GetRoomStatusString(Room room)
        {
            string ss = string.Empty;
            //int floorNumber = room.MyFloor.Number;
            //int roomNumber = room.Number;
            //            Room aRoom = _FloorSet[Convert.ToInt32(floorNumber)][Convert.ToInt32(roomNumber)];
            //空格是留给TimeOut的

            string roomNunber = (room.ToFullNumber() < 1000) ? "0" + room.ToFullNumber().ToString() : room.ToFullNumber().ToString();
            string serviceType = (((int)room.MyGuestService.Service.ServiceValue) < 10) ?
               "0" + ((int)room.MyGuestService.Service.ServiceValue).ToString() : ((int)room.MyGuestService.Service.ServiceValue).ToString();
            string lightOnNumber = (room.LightOnNumber > 99) ? "99" : room.LightOnNumber.ToString("D2");
            ss = "#" + roomNunber +
                ((int)room.DoorStatus).ToString() +
                ((int)room.KeyStatus).ToString() +
                ((int)room.HotelUsingStatus).ToString() +
                (Convert.ToInt32((room.MyGuestService.IsEmergency))).ToString() +
                serviceType +
                (Convert.ToInt32((room.LightSystemOff)) * 4 +
                    Convert.ToInt32((room.IsRefrigeratorOpen)) * 2 +
                        Convert.ToInt32((room.IsCofferOpen))).ToString() +
                //move lightCount to the last
                0 +//lightOnNumber.ToString() +
                ((int)room.MyAirConditioner.Speed).ToString() +
                (Convert.ToInt32((room.MyAirConditioner.IsRunning))).ToString() +
                (Convert.ToInt32((room.MyAirConditioner.Season))).ToString() +
                (Convert.ToInt32((room.MyATM.IsHasProblem))).ToString() +
                (Convert.ToInt32((room.MyATM.IsConnected))).ToString() +

                (GetTemperature(room.Temperature)) +
                lightOnNumber +
                "00" +
                "* ";
            return ss;

        }

        /// <summary>
        /// Determine if a roomName is validate
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        private Room IsRoomValidate(string roomName)
        {
            int roomNumber;
            bool flag = int.TryParse(roomName, out roomNumber);
            if (!flag)
            {
                return null;
            }

            int floorNumber = roomNumber / 100;
            int roomIndex = roomNumber % 100;

            if ((_FloorSet[floorNumber] != null))
            {
                return _FloorSet[floorNumber].GetRoomByRoomNumber(roomNumber);
            }

            return null;
        }

        /// <summary>
        /// Obtain a room object according to string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        Room ConvertStringToRoom(string str)
        {
            return _FloorSet.Parse(str);
        }

        #region Process Command Handlers
        /// <summary>
        /// Process GetRoom command of client
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetRoom(string[] args)
        {
            string retString = string.Empty;
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }
            else
            {
                if (args[0].Contains("?"))
                {
                    // Return floor information
                    int floorNumber;

                    if (!int.TryParse(args[0].Substring(args[0].IndexOf("?") - 1), out floorNumber))
                    {
                        return ERROR_INCORRECT_FLOOR_NUMBER;
                    }

                    if (_FloorSet[floorNumber] == null)
                    {
                        return ERROR_FLOOR_NUMBER_NOT_EXIST;
                    }

                    foreach (Room room in _FloorSet[floorNumber].GetRooms())
                    {
                        retString += GetRoomStatusString(room);
                    }
                }
                else
                {
                    // Return one room information
                    // TODO Need Test
                    int floorNumber, roomNumber;
                    if ((args[0].Length <= 2) || (args[0].Length >= 5) ||
                        (!int.TryParse(args[0].Substring(args[0].Length - 2), out roomNumber)) ||
                        (!int.TryParse(args[0].Substring(0, args[0].Length - 2), out floorNumber)) ||
                        (_FloorSet[floorNumber] == null) ||
                        (!_FloorSet[floorNumber].Contains(roomNumber)))
                    {
                        return ERROR_INCORRECT_ROOM_NUMBER;
                    }

                    retString += GetRoomStatusString(_FloorSet[floorNumber].GetRoomByRoomNumber(roomNumber));
                }


                //try
                //{
                //    string roomNumber = args[0];

                //    string floorNumber = null;
                //    string Rlength = null;


                //    if (roomNumber.Length > 4 || roomNumber.Length < 3)
                //    {
                //        return ERROR_INCORRECT_ROOM_NUMBER;
                //    }

                //    if (roomNumber.Length == 3)
                //    {
                //        roomNumber = "0" + roomNumber;
                //    }
                //    char[] num = roomNumber.ToCharArray(0, roomNumber.Length);
                //    for (int i = 0; i < 2; i++)
                //    {
                //        if ((int)num[i] < 48 || ((int)num[i] > 58))
                //            return ERROR_INCORRECT_ROOM_NUMBER;
                //    }
                //    for (int i = 2; i < 4; i++)
                //    {
                //        if ((int)num[i] < 48 || ((int)num[i] > 58 && (int)num[i] != 63))
                //            return ERROR_INCORRECT_ROOM_NUMBER;
                //    }

                //    floorNumber = (num[0].ToString() + num[1].ToString());
                //    Rlength = (num[0].ToString() + num[1].ToString());
                //    if (_FloorSet[Convert.ToInt32(floorNumber)] == null)
                //    {
                //        return "Error: The floor number does not exist";
                //    }


                //    // Obtain the floor information
                //    if (num[2] == '?' && num[3] == '?')
                //    {
                //        for (int i = 1; i <= _FloorSet[Convert.ToInt32(Rlength)].MaxRoomNumber; i++)
                //        {
                //            retString += GetRoomStatusString(floorNumber, i.ToString());
                //        }
                //    }

                //    // Obtain the room information
                //    if (num[2] == '?' && num[3] != '?')
                //    {
                //        int num11 = _FloorSet[Convert.ToInt32(Rlength)].Length / 10;
                //        int num22 = _FloorSet[Convert.ToInt32(Rlength)].Length % 10;
                //        if (Convert.ToInt32(Convert.ToString(num[3])) > num22)
                //        {
                //            for (int ii = 0; ii < num11; ii++)
                //            {
                //                retString += GetRoomStatusString(floorNumber, ii.ToString() + num[3].ToString());
                //            }
                //        }
                //        else
                //        {
                //            int ii = 0;
                //            if (Convert.ToInt32(Convert.ToString(num[3])) == 0)
                //            {
                //                ii = 1;
                //            }
                //            for (; ii <= num11; ii++)
                //            {
                //                retString += GetRoomStatusString(floorNumber, ii.ToString() + num[3].ToString());
                //            }
                //        }
                //    }

                //    if (num[2] != '?' && num[3] == '?')
                //    {
                //        int num1 = _FloorSet[Convert.ToInt32(Rlength)].Length / 10;
                //        int num2 = _FloorSet[Convert.ToInt32(Rlength)].Length % 10;
                //        int i = 0;

                //        if (Convert.ToInt32(Convert.ToString(num[2])) == 0)
                //        {
                //            i = 1;
                //        }
                //        if (Convert.ToInt32(Convert.ToString(num[2])) == num1)
                //        {
                //            for (; i <= num2; i++)
                //            {
                //                retString += GetRoomStatusString(floorNumber, (string)(num[2].ToString() + Convert.ToString(i)));
                //            }
                //        }
                //        if (Convert.ToInt32(Convert.ToString(num[2])) > num1)
                //        {
                //            return ERROR_INCORRECT_ROOM_NUMBER;
                //        }
                //        if (Convert.ToInt32(Convert.ToString(num[2])) < num1)
                //        {
                //            for (; i < 10; i++)
                //            {
                //                retString += GetRoomStatusString(floorNumber, num[2].ToString() + Convert.ToString(i));
                //            }
                //        }
                //    }

                //    if (num[2] != '?' && num[3] != '?')
                //    {

                //        string num3 = (Convert.ToString(num[2]) + Convert.ToString(num[3]));

                //        if (_FloorSet[Convert.ToInt32(floorNumber)].Length >= Convert.ToInt32(num3) && Convert.ToInt32(num3) > 0)
                //        {

                //            retString += GetRoomStatusString(floorNumber, num3);
                //        }
                //        else
                //        {
                //            return ERROR_INCORRECT_ROOM_NUMBER;
                //        }
                //    }
                //}
                //catch
                //{
                //    return "Error: Some unexpected error has occured";
                //}
                //finally
                //{

                //}
                //change # to % as a new request for getroom 
                retString = retString.Replace("#", "%");
                return retString;
            }
        }

        /// <summary>
        /// Process Disconnect command of client
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        string Disconnect(string[] args)
        {
            CloseSession();
            return string.Empty;
        }

        /// <summary>
        /// Process GetRoomNumber command of client
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetRoomNumber(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            string condition = args[0];
            string retString = null;
            try
            {
                _RoomView.Filter = condition;
                for (int i = 0; i <= _RoomView.RoomList.Count - 1; i++)
                {
                    retString += ((Room)_RoomView.RoomList.Values[i]).ToString() + " ";
                }
            }
            catch
            {
                retString = null;
            }

            if (retString == null)
            {
                retString = ERROR_INCORRECT_ROOM_NUMBER;
            }
            return retString;
        }

        string SetAllRoomSeason(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }
            string targetSeason = args[0].ToUpper().Trim();
            if (targetSeason == SeasonType.Spring.ToString().ToUpper())
            {
                return SetAllRoomSeason(SeasonType.Spring);
            }
            else if (targetSeason == SeasonType.Summer.ToString().ToUpper())
            {
                return SetAllRoomSeason(SeasonType.Summer);
            }
            else if (targetSeason == SeasonType.Autumn.ToString().ToUpper())
            {
                return SetAllRoomSeason(SeasonType.Autumn);
            }
            else if (targetSeason == SeasonType.Winter.ToString().ToUpper())
            {
                return SetAllRoomSeason(SeasonType.Winter);
            }
            else
            {
                return ERROR_SEASON_INVALID;
            }
        }

        string SetAllRoomSeason(SeasonType season)
        {
            Floor floor;
            foreach (int j in _FloorSet.FloorNumbers)
            {
                floor = _FloorSet[j];
                foreach (Room room in ((Floor)floor).GetRooms())
                {
                    room.MyAirConditioner.Season = season;
                }
            }
            return "OK";
        }

        string SetAirconOn(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.MyAirConditioner.SetOn();
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }

        string SetAirconOff(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.MyAirConditioner.SetOff();
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }

        string SetLightSystemOff(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.LightSystemOff = true;
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }



        string SetLightSystemOn(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.LightSystemOff = false;
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }

        string CheckOut(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.HotelUsingStatus = HotelUsingStatusType.CheckOut;
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }

        string Empty(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.HotelUsingStatus = HotelUsingStatusType.Empty;
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }

        string Rent(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room aRoom = ConvertStringToRoom(args[0]);
            if (aRoom != null)
            {
                aRoom.HotelUsingStatus = HotelUsingStatusType.Rented;
                return "OK";
            }
            else return ERROR_INCORRECT_ROOM_NUMBER;
        }

        string Book(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_INCORRECT_ROOM_NUMBER;
            }

            Room room = ConvertStringToRoom(args[0]);
            if (room != null)
            {
                room.HotelUsingStatus = HotelUsingStatusType.Booked;
                return "OK";
            }
            else return ERROR_INCORRECT_FLOOR_NUMBER;
        }
        string Maintanent(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_INCORRECT_ROOM_NUMBER;
            }

            Room room = ConvertStringToRoom(args[0]);
            if (room != null)
            {
                room.HotelUsingStatus = HotelUsingStatusType.Maintanent;
                return "OK";
            }
            else return ERROR_INCORRECT_FLOOR_NUMBER;
        }

        string Vacant(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_INCORRECT_ROOM_NUMBER;
            }

            Room room = ConvertStringToRoom(args[0]);
            if (room != null)
            {
                room.HotelUsingStatus = HotelUsingStatusType.Vacant;
                return "OK";
            }
            else return ERROR_INCORRECT_FLOOR_NUMBER;
        }

        //string Season(string[] args)
        //{
        //    if (args.Length < 1)
        //    {
        //        return ERROR_INCORRECT_ROOM_NUMBER;
        //    }

        //    switch (args[0].ToUpper())
        //    {
        //        case SeasonType.Autumn.ToString().ToUpper():
                    
        //            break;
        //        default:
        //            break;
        //    }
        //    if (args[0].ToUpper() == SeasonType.Summer)
        //    {
                
        //    }
        //    if (room != null)
        //    {
        //        room.HotelUsingStatus = HotelUsingStatusType.VIP;
        //        return "OK";
        //    }
        //    else return ERROR_INCORRECT_FLOOR_NUMBER;
        //}

        string VIP(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_INCORRECT_ROOM_NUMBER;
            }

            Room room = ConvertStringToRoom(args[0]);
            if (room != null)
            {
                room.HotelUsingStatus = HotelUsingStatusType.VIP;
                return "OK";
            }
            else return ERROR_INCORRECT_FLOOR_NUMBER;
        }

        string GetFloorSet(string[] args)
        {
            string mFloorNumbers = "@";
            foreach (int i in _FloorSet.FloorNumbers)
            {
                mFloorNumbers += i.ToString() + " ";
            }
            return mFloorNumbers;
        }


        string CountFloor(string[] args)
        {
            if (args.Length == 0)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            int floorNumber;
            bool flag = int.TryParse(args[0], out floorNumber);

            if (!flag)
            {
                return ERROR_ARGUMENT_NOT_NUMBER;
            }

            string s = null;
            if (_FloorSet[floorNumber] == null)
            {
                s = ERROR_FLOOR_NUMBER_NOT_EXIST;
            }
            else
            {
                s = "Floor " + floorNumber + " Rooms " + _FloorSet[floorNumber].MaxRoomNumber.ToString();
            }

            return s;

        }

        string Version(string[] args)
        {
            return "VERSION " + _Server.ClassInfo.Version.ToString();
        }

        string WakeUp(string[] args)
        {
            if (args.Length < 1)
            {
                return ERROR_NOT_ENOUGH_ARGUMENTS;
            }

            Room room = _FloorSet.Parse(args[0]);
            if (room == null)
            {
                return ERROR_INCORRECT_ROOM_NUMBER;
            }
            else
            {
                room.MyGuestService.WakeUp();
                return "OK";
            }

        }
        #endregion

        //TODO :处理房间状态设置
        string SetRoomStatus(string Status)
        {
            return null;
        }

        #endregion



    }

    //[ConnectionVersionInfo( "TCP/IP数据提供协议",
    //                "{30B1CB3E-895B-4e63-90F8-A147665AD2EA}",
    //                new Version("1.0.1"),
    //                "通过TCP/IP方式提供数据的程序。")]

    /*
        public class TaskTimer : System.Timers.Timer
        {
            private TcpConversation tcpcon;
            #region <变量>
            /// 
            /// 定时器id
            /// 
            private int id;
            /// 
            /// 定时器参数
            /// 

            private string param;
            #endregion

            #region <属性>
            /// 
            /// 定时器id属性
            /// 
            public TcpConversation TCPCON
            {
                set { tcpcon = value; }
                get { return tcpcon; }
            }
            public int ID
            {
                set { id = value; }
                get { return id; }
            }
            /// 
            /// 定时器参数属性
            /// 
            public string Param
            {
                set { param = value; }
                get { return param; }
            }
            #endregion

            /// 
            /// 构造函数
            /// 
            public TaskTimer()
                : base()
            {

            }

        }
        */
    public class TcpServer : DataProviderBase
    {
        private static TcpServer _DefaultInstance = null;
        //    private System.Timers.Timer _ConnectTime=new System.Timers.Timer(4000);
        //    TaskTimer tasktime = new TaskTimer();
        public static TcpServer DefaultInstance
        {
            get
            {
                return _DefaultInstance;
            }
        }


        #region Public events
        public delegate void ConversationChangedEventHandler(object sender, TcpConversation conversation);
        public event ConversationChangedEventHandler ConversationAdded;
        public event ConversationChangedEventHandler ConversationRemoved;
        #endregion


        private IPAddress _IP = IPAddress.Parse("0.0.0.0");
        private IPEndPoint _ServerIPEndPoint;
        private Socket _CurrentSocket;
        private Thread _ListenerThread;
        private FloorSet _FloorSet;

        private Settings _TCPIPAddressSetting;
        protected SettingsBase _Settings;

        private EventLogger aLog = new EventLogger();
        private List<TcpConversation> _Conversations = new List<TcpConversation>();

        public FloorSet FloorSet
        {
            get
            {
                return _FloorSet;
            }
        }

        public override ClassInfo ClassInfo
        {
            get
            {
                return new ClassInfo("TCP/IP数据提供协议",
                    "{30B1CB3E-895B-4e63-90F8-A147665AD2EA}",
                    new Version("1.1.1"),
                    "通过TCP/IP方式提供数据的程序。");
            }
        }

        public ReadOnlyCollection<TcpConversation> GetAllConversations()
        {
            return _Conversations.AsReadOnly();
        }

        public override bool ShowSetupForm()
        {
            DataProviderTCPIPSetupForm aForm = new DataProviderTCPIPSetupForm();
            aForm.Settings = this.Settings;
            aForm.ShowDialog();

            return aForm.DialogResult == DialogResult.OK;
        }





        public override SettingsBase Settings
        {
            get
            {
                return _Settings;
            }
        }

        public override bool IsRunning
        {
            get
            {
                return mIsRunning;
            }
        }




        public TcpServer(FloorSet aFloorSet, SettingsBase theSetting)
            : base(aFloorSet, theSetting)
        {
            _FloorSet = aFloorSet;
            _Settings = theSetting;

            if (theSetting == null)
            {
                //从注册表的TCPIPAddress中获取数据源，若TCPIPAddress不存在则创建它并给数据赋初值
                _TCPIPAddressSetting = (Settings)theSetting;
                _TCPIPAddressSetting["IP"] = "0.0.0.0";
                _TCPIPAddressSetting["Port"] = "15000";
            }
            else
            {
                _TCPIPAddressSetting = (Settings)theSetting;
                if (_TCPIPAddressSetting["IP"] == null)
                {
                    _TCPIPAddressSetting["IP"] = "0.0.0.0";
                    theSetting.Save();
                }
                if (_TCPIPAddressSetting["Port"] == null)
                {
                    _TCPIPAddressSetting["Port"] = "15000";
                }
            }


            _IP = IPAddress.Parse(_TCPIPAddressSetting["IP"].ToString());
            _ServerIPEndPoint = new IPEndPoint(_IP, Int32.Parse(_TCPIPAddressSetting["Port"].ToString()));
            // Hashtable CmdHT=new Hashtable();

            HandleEvents();

            _DefaultInstance = this;
        }

        public override void Open()
        {
            try
            {
                base.Open();

                // Create a listener thread
                _ListenerThread = new Thread(new ThreadStart(StartListen));
                _ListenerThread.IsBackground = true;
                _ListenerThread.Start();

                mIsRunning = true;
            }
            catch (Exception e)
            {
                Close();
                aLog.WriteLog("TCPIP启动错误，端口可能已被占用。具体信息： " + e.Message.ToString() + e.Source.ToString());
            }
        }

        public override void Close()
        {
            base.Close();
            StopListen();  //Stop
        }
        /*
         private static int listcon=0;
         protected void mScannerTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
         {

         //    if(_roomlist>conversation._RoomView.RoomList.Count)
                // _ConnectTime.Enabled = false;
             lock (this)
             {
                 TaskTimer tt = (TaskTimer)sender;


                 Room rom = tt.TCPCON._RoomView.RoomList.Values[listcon];
                 string str = tt.TCPCON.GetRoomStatusString(rom);
                 SendMessageToAllClient(str);


                 if (tt.TCPCON._RoomView.RoomList.Count <= listcon+1)
                 {
                     tasktime.Enabled = false;
                     listcon = 0;
                     return;
                 }

                 listcon = listcon + 1;
                 return;
             }

         }
          * */
        public void StartListen()
        {



            try
            {
                _CurrentSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _CurrentSocket.Bind(_ServerIPEndPoint);
            }
            catch (SocketException)
            {
                StopListen();
            }

            while (true)
            {
                try
                {

                    _CurrentSocket.Listen(200);
                    _CurrentSocket.Blocking = true;

                    //Thread thread = new Thread(new ThreadStart(AcceptConnection));
                    //thread.Start();
                    Socket newSocket = _CurrentSocket.Accept();

                    // _roomlist = 0;
                    // Create a conversation
                    TcpConversation conversation = new TcpConversation(newSocket, this);




                    // Save the conversation
                    _Conversations.Add(conversation);
                    OnConversationAdded(conversation);
                    /*
                    for (int ii = 0; ii < conversation._RoomView.RoomList.Count; ii++)
                    {
                        Room rom = conversation._RoomView.RoomList.Values[ii];
                        string str = conversation.GetRoomStatusString(rom);
                        SendMessageToAllClient(str);
                    }
                    */
                   Room rom = conversation._RoomView.RoomList.Values[0];
                        string str = conversation.GetRoomStatusString(rom);
                        SendMessageToAllClient(str);
                    //  _ConnectTime.Enabled = true;
                    //   _ConnectTime.Elapsed += new System.Timers.ElapsedEventHandler(mScannerTimeout_Elapsed);
                  
                   // tasktime.Elapsed +=new System.Timers.ElapsedEventHandler(mScannerTimeout_Elapsed);
                    //tasktime.Interval = 3000;
                   // tasktime.TCPCON = conversation;
                   // tasktime.Enabled = true;
                   
                    //  tasktime.ID = 0;

                    //     listcon = 0;


                }
                catch (Exception ee)
                {
                    aLog.WriteLog(ee.Message.ToString() + ee.Source.ToString());
                    Console.WriteLine(ee.Message.ToString() + ee.Source.ToString());
                    //MessageBox.Show(ee.Message);
                }
            }
        }
       
        private void AcceptConnection()
        {
            Socket newSocket = null;
            //while (true)
            //  {
            try
            {
                //开始接收
                newSocket = _CurrentSocket.Accept();

            }
            catch (Exception ee)
            {
                aLog.WriteLog(ee.Message.ToString() + ee.Source.ToString());
                //MessageBox.Show(ee.Message);
            }
            //}
        }

        internal void RemoveConversation(TcpConversation conversation)
        {
            _Conversations.Remove(conversation);
            OnConversationRemoved(conversation);
        }

        public void StopListen()
        {
            try
            {
                //Close all Threads and Disconnect all Socket
                while (_Conversations.Count != 0)
                {
                    _Conversations[0].CloseSession();
                }

                _ListenerThread.Abort();
                _CurrentSocket.Close();
            }
            catch
            {
                aLog.WriteLog("监听未开始,无效");
                //	MessageBox.Show("监听未开始,无效");
            }
        }


        private void SendMessageToAllClient(string aMsg)
        {
            if ((aMsg != null) && (mIsRunning))
            {
                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(aMsg + "\r\n");

                for (int i = 0; i < _Conversations.Count; i++)
                {
                    /*if (((Socket)SocketList[i]).Connected)
                    {
                        try
                        {
                            ((Socket)SocketList[i]).Send(byteData);
                        }
                        catch (Exception ee)
                        {
                            aLog.WriteLog(ee.Message.ToString() + ee.Source.ToString());

                        }
                    }*/
                    _Conversations[i].SendMessage(aMsg);
                }
            }
        }

        #region Observe the changed of special room status and send the information to Client
        private void HandleEvents()
        {
            this._FloorSet.EmergencyChanged += new EmergencyChangedEventHandler(mFloorSet_Emergencychange);
            this._FloorSet.RefrigeratorStatusChanged += new RefrigeratorStatusChangedEventHandler(mFloorSet_RefrigeratorStatusChanged);
            this._FloorSet.CofferStatusChanged += new CofferStatusChangedEventHandler(mFloorSet_CofferStatusChanged);
            this._FloorSet.ATMProblemChanged += new ATMProblemChangedEventHandler(mFloorSet_ATMProblemChanged);
            this._FloorSet.ATMConnectionChanged += new ATMConnectionChangedEventHandler(mFloorSet_ATMConnectionChanged);
            this._FloorSet.HotelUsingStatusChanged += new HotelUsingStatusChangedEventHandler(mFloorSet_HotelUsingStatusChanged);
            this._FloorSet.ServiceChanged += new ServiceChangedEventHandler(mFloorSet_ServiceChanged);
            this._FloorSet.KeyChanged += new KeyChangedEventHandler(mFloorSet_KeyChanged);
            this._FloorSet.DoorChanged += new DoorChangedEventHandler(mFloorSet_DoorChanged);
            this._FloorSet.SpeedChanged += new SpeedChangedEventHandler(mFloorSet_SpeedChanged);
            //this._FloorSet.TemperatureChanged += new TemperatureChangedEventHandler(mFloorSet_temperatureChanged);
            this._FloorSet.RoomStatusChanged += new RoomPropertyChangedEventHandler(_FloorSet_RoomStatusChanged);
            this._FloorSet.ServantCheckRoomStatusChanged += new ServantCheckRoomStatusChangedEventHandler(_FloorSet_ServantCheckRoomStatusChanged);
            //this._FloorSet.SeasonChanged += new SeasonChangedEventHandler(_FloorSet_SeasonChanged);
        }

        void _FloorSet_SeasonChanged(object sender, SeasonChangedEventArgs e)
        {
            SendMessageToAllClient("Season is " + e.ChangedRoom.MyAirConditioner.Season);
        }

        void _FloorSet_ServantCheckRoomStatusChanged(object sender, ServantCheckRoomStatusChangedEventArgs e)
        {
            SendMessageToAllClient("CheckRoom " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.IsChecking);
        }

        void _FloorSet_RoomStatusChanged(object sender, RoomPropertyChangedEventArgs e)
        {
            if (e.PropertyName == RoomPropertyChangeName.LightOnNumber)
            {
                SendMessageToAllClient("LampNumber " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.LightOnNumber);
            }
            
        }

        private void mFloorSet_RefrigeratorStatusChanged(object sender, RefrigeratoryStatusChangedEventArgs e)
        {
            SendMessageToAllClient("Refrigerator " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.IsRefrigeratorOpen);
        }

        private void mFloorSet_CofferStatusChanged(object sender, CofferStatusChangedEventArgs e)
        {
            SendMessageToAllClient("Coffer " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.IsCofferOpen);
        }

        private void mFloorSet_ATMProblemChanged(object sender, ATMProblemChangedEventArgs e)
        {
            SendMessageToAllClient("Problem " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyATM.IsHasProblem);
        }

        private void mFloorSet_ATMConnectionChanged(object sender, ATMConnectionChangedEventArgs e)
        {
            SendMessageToAllClient("Connection " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyATM.IsConnected);
        }

        private void mFloorSet_HotelUsingStatusChanged(object sender, HotelUsingStatusChangedEventArgs e)
        {
            SendMessageToAllClient("RoomStatus " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.HotelUsingStatus.ToString());
        }

        private void mFloorSet_Emergencychange(object sender, EmergencyChangedEventArgs e)
        {
            //   SendMessageToAllClient("Coffer " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.IsCofferOpen);
            SendMessageToAllClient("Emergency " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.IsEmergency);

        }
        private void mFloorSet_DoorChanged(object sender, DoorChangedEventArgs e)
        {
            //   SendMessageToAllClient("Coffer " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.IsCofferOpen);
            SendMessageToAllClient("DoorChanged " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.DoorStatus.ToString());

        }
        //风速改变
        private void mFloorSet_SpeedChanged(object sender, SpeedChangedEventArgs e)
        {
            //   SendMessageToAllClient("Coffer " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.IsCofferOpen);
            SendMessageToAllClient("SpeedChanged " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyAirConditioner.Speed.ToString());

        }

        //温度改变
        private void mFloorSet_temperatureChanged(object sender, TemperatureChangedEventArgs e)
        {
            //   SendMessageToAllClient("Coffer " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.IsCofferOpen);
            SendMessageToAllClient("temperatureChanged " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.Temperature.ToString());

        }

        private void mFloorSet_ServiceChanged(object sender, ServiceChangedEventArgs e)
        {
            if (e.LastService.HasService(ServiceType.QuitRoom) != e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.QuitRoom))	//QuitRoom Status Occur
                SendMessageToAllClient("QuitRoom " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.QuitRoom));

            if (e.LastService.HasService(ServiceType.Clean) != e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.Clean))	//Cleaning Status Occur
                SendMessageToAllClient("Clean " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.Clean));
            if (e.LastService.HasService(ServiceType.DontDisturb) != e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.DontDisturb))
                SendMessageToAllClient("DontDisturb " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.DontDisturb));
            //DONTDISTURB
            if (e.LastService.HasService(ServiceType.Call) != e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.Call))
                SendMessageToAllClient("Call " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.Call));
            if (e.LastService.HasService(ServiceType.Wash) != e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.Wash))
                SendMessageToAllClient("WashClothes " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.MyGuestService.Service.HasService(ServiceType.Wash));
        }

        private void mFloorSet_KeyChanged(object sender, KeyChangedEventArgs e)
        {
            SendMessageToAllClient("Key " + e.ChangedRoom.ToString() + " " + e.ChangedRoom.KeyStatus.ToString());
        }
        #endregion


        #region Events
        protected void OnConversationAdded(TcpConversation conversation)
        {
            if (ConversationAdded != null)
            {
                ConversationAdded(this, conversation);
            }
        }

        protected void OnConversationRemoved(TcpConversation conversation)
        {
            if (ConversationRemoved != null)
            {
                ConversationRemoved(this, conversation);
            }
        }
        #endregion
    }
}
