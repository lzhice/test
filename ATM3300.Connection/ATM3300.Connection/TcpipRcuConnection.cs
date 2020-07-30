using System;
using System.Collections.Generic;
using System.Text;
using ATM3300.Connection.Forms;
using System.Net.Sockets;
using System.Threading;
using ATM3300.Common;
using System.Diagnostics;
using System.Net;
using System.Collections;

namespace ATM3300.Connection
{
    [ConnectionVersionInfo("TCP/IP - RCU 连接", 
        "{56928512-3ED4-40b3-9FE0-68DCD6665372}",
        "1.0.0",
        "通过TCP/IP协议连接酒店客房RCU",
        typeof(TcpipRcuSetupForm))]
    public class TcpipRcuConnection : ConnectionScannerBase
    {
        #region Fields
        protected UdpClient _UdpClient = null;
        protected int _RetryTimesBeforeDisconnect = 3;
        protected Thread _ScannerThread = null;
        protected int _RetryTimesEachScan = 3;
        protected int _ScanTimeout = 50;        // ms

        protected int _SendBroadcastDataToRcuInterval = 30;     // Seconds
        protected System.Timers.Timer _SendBroadcastDataTimer = new System.Timers.Timer();
        protected bool _SendBroadcastDataToRcu = true;

        protected Dictionary<Room, int> _DisconnectCounter = new Dictionary<Room, int>();
        protected byte _NetworkID1 = 88;
        protected byte _NetworkID2 = 88;
        protected int _LocalPortNumber = 20000;
        protected int _RemotePortNumber = 20001;
        protected IPEndPoint _RecieveIPEndPoint = null; 
        private byte[] _OutsideTemperatureAddress = new byte[2] { 99, 99};
        private bool _NeedBroadcastDataTimer = true;
        private object _Lock = new object();

        #endregion
        
        public TcpipRcuConnection(FloorSet floorSet, SettingsBase settings)
            :base(floorSet , settings)
        {
            // TODO Listen to FloorSet events and send to RCU

            _FloorSet.HotelUsingStatusChanged += new HotelUsingStatusChangedEventHandler(_FloorSet_HotelUsingStatusChanged);
            _FloorSet.DefaultTemperatureChanged += new DefaultTemperatureChangedEventHandler(_FloorSet_DefaultTemperatureChanged);
            _FloorSet.DefaultRunningStatusChanged += new DefaultRunningStatusChangedEventHandler(_FloorSet_DefaultRunningStatusChanged);
            _SendBroadcastDataTimer.Elapsed += new System.Timers.ElapsedEventHandler(_SendBroadcastDataTimer_Elapsed);

            ResetSetting();
        }




        

        #region Override methods
        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("TCP/IP - RCU 连接",
                "{56928512-3ED4-40b3-9FE0-68DCD6665372}",
                new Version("1.0.0"),
                "通过TCP/IP协议连接酒店客房RCU");
        }



        public override void ResetSetting()
        {
            base.ResetSetting();
            bool lastRunningStatus = _IsRunning;
            if (_IsRunning)
            {
                this.Disconnnect();
            }

            // Parser settings
            int.TryParse(_Settings["RetryTimesBeforeDisconnect", 3].ToString(),
                out _RetryTimesBeforeDisconnect);
            int.TryParse(_Settings["SendBroadcastDataToRcuInterval", 30].ToString(),
                out _SendBroadcastDataToRcuInterval);
            int.TryParse(_Settings["RetryTimesEachScan", 3].ToString(),
                out _RetryTimesEachScan);
            byte.TryParse(_Settings["NetworkID1", 88].ToString(),
                out _NetworkID1);
            byte.TryParse(_Settings["NetworkID2", 88].ToString(),
                out _NetworkID2);
            int.TryParse(_Settings["LocalPort", 20000].ToString(),
                out _LocalPortNumber);
            int.TryParse(_Settings["RemotePort", 20001].ToString(),
                out _RemotePortNumber);
            int.TryParse(_Settings["ScanTimeout", 50].ToString(),
                out _ScanTimeout);

            _SendBroadcastDataTimer.Interval = _SendBroadcastDataToRcuInterval * 1000;
            _SendBroadcastDataTimer.Enabled = _SendBroadcastDataToRcu;


            if (lastRunningStatus)
            {
                // Wait 200ms for _UdpClient has been closed
                Thread.Sleep(200);
                this.Connect();
            }
        }

        public override bool ShowSetupForm()
        {
            TcpipRcuSetupForm form = new TcpipRcuSetupForm();
            form.Settings = _Settings;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override void Connect()
        {
            base.Connect();
            try
            {
                _RecieveIPEndPoint = new IPEndPoint(
                    IPAddress.Parse("127.0.0.1"),
                    _LocalPortNumber);

                _UdpClient = new UdpClient(_LocalPortNumber);
                _UdpClient.Client.ReceiveTimeout = _ScanTimeout;
                _ScannerThread = new Thread(new ThreadStart(ScannerMain));
                _ScannerThread.IsBackground = true;
                _ScannerThread.Start();
            }
            catch
            {
                // Exception throw
                // Return to disconnect status and rethrow the exception
                this.Disconnnect();
                throw;
            }
        } 
        #endregion

        /// <summary>
        /// Thread entry of scanner
        /// </summary>
        private void ScannerMain()
        {
            while (true)
            {
                bool successRetrieveData = false;
                for (int i = 0; i < _RetryTimesEachScan; i++)
                {
                    if (!_IsRunning)
                    {
                        lock (_UdpClient)
                        {
                            _UdpClient.Close();
                            _UdpClient = null;
                        }

                        // Exit thread
                        return;
                    }

                    bool successReceive = SendAndRecieveScanRoomData();

                    if (successReceive)
                    {
                        successRetrieveData = true;
                        break;
                    }
                }

                if (!successRetrieveData)
                {
                    CountDisconnectCurrentRoom();
                }
                else
                {
                    // Remove disconnect counter if existed
                    if (_DisconnectCounter.ContainsKey(CurrentRoom))
                    {
                        _DisconnectCounter.Remove(CurrentRoom);
                    }
                    CurrentRoom.MyATM.Connect();
                }

                // Move to next room
                CurrentRoom = NextRoom;
            }
        }

        #region Send and Receive data
        private bool SendAndRecieveScanRoomData()
        {
            SendRequestRoomInformationData();


            // Wait for response data
            byte[] receiveData = null;
            bool successReceive = false;
            IPEndPoint receiveEndPoint = new IPEndPoint(
                   IPAddress.Parse("127.0.0.1"),
                   _LocalPortNumber);

            while (true)
            {
                try
                {
                    lock (_UdpClient)
                    {
                        receiveData = _UdpClient.Receive(ref receiveEndPoint);
                    }
                }
                catch (Exception er)
                {
                    Trace.WriteLine(string.Format(
                        "TCP/RCU: {0}: {1}",
                        er.GetType(),
                        er.Message), "Connection");
                    break;
                }

                if (receiveData != null)
                {
                    TraceDataInfo(false, receiveData, receiveEndPoint);

                    if ((receiveData.Length == 24) &&
                        ((receiveData[0] == 2) || (receiveData[0] == 3)))
                    {
                        // Check if receive from outside temperature
                        byte[] addressBytes = receiveEndPoint.Address.GetAddressBytes();
                        if ((addressBytes[2] == _OutsideTemperatureAddress[0]) &&
                            (addressBytes[3] == _OutsideTemperatureAddress[1]))
                        {
                            // The data comes from outside ATM
                            // Save the temperature data
                            Trace.WriteLine(
                                string.Format("TcpipRcu: Received outside temperature:{0}",
                                    receiveData[6]),
                                "Connection");
                            _Settings["OutsideTemperature"] = receiveData[6];
                        }
                        else
                        {
                            // Obtain the receive rooom
                            Room room = ObtainRoomByIPAddress(
                                receiveEndPoint.Address);

                            if (room != null)
                            {
                                ApplyData(room, receiveData);
                            }

                            // Validate if is response data
                            if ((receiveData[0] == 2) &&
                                (receiveEndPoint.Address.GetAddressBytes()[2] == CurrentRoom.MyFloor.Number) &&
                                (receiveEndPoint.Address.GetAddressBytes()[3] == CurrentRoom.Number))
                            {
                                successReceive = true;
                                break;
                            }
                        }
                    }
                }
            }
            return successReceive;
        }

        private Room ObtainRoomByIPAddress(IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();
            if (bytes.Length == 4 &&
                _FloorSet.ContainsFloor(bytes[2]))
            {
                return _FloorSet[bytes[2]].GetRoomByRoomNumber(bytes[3]);
            }
            else
            {
                return null;
            }
        }

        private void SendRequestRoomInformationData()
        {
            SendRequestRoomInformationData(_CurrentRoom);
        }

        private void SendRequestRoomInformationData(Room roomToRequest, bool needResponse)
        {
            if (_IsRunning && _UdpClient != null && roomToRequest != null)
            {
                //IPEndPoint remoteEndPoint = new IPEndPoint(
                //    new IPAddress(
                //        new byte[]{
                //            _NetworkID1,
                //            _NetworkID2,
                //            (byte)roomToRequest.MyFloor.Number,
                //            (byte)roomToRequest.Number}),
                //        _RemotePortNumber);

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                        _RemotePortNumber);

                byte[] bytesToSend = GenerateRoomScanData(roomToRequest);

                bytesToSend[0] = (byte)(needResponse ? 0x01 : 0x04);

                try
                {
                    SendDataThreadSafety(_UdpClient, bytesToSend, bytesToSend.Length, remoteEndPoint);
                    //_UdpClient.Send(bytesToSend, bytesToSend.Length, remoteEndPoint);
                }
                catch (SocketException )
                {
                }
                TraceDataInfo(true, bytesToSend, remoteEndPoint);
            }
        }

        private void SendRequestRoomInformationData(Room roomToRequest)
        {
            SendRequestRoomInformationData(roomToRequest, true);
        }

        private void _SendBroadcastDataTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_IsRunning && _UdpClient != null)
            {
                lock (_Lock)
                {
                    _NeedBroadcastDataTimer = true;
                }
                ////[3/16] pingke 将广播信息放入房间扫描
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                        _RemotePortNumber);

                byte[] bytesToSend = new byte[24];
                bytesToSend[0] = 0; // Setup broadcast flag
                bytesToSend[1] = (byte)Utility.BCD(DateTime.Now.Hour);
                bytesToSend[2] = (byte)Utility.BCD(DateTime.Now.Minute);
                bytesToSend[3] = (byte)
                    (_FloorSet.FirstFloor.FirstRoom.MyAirConditioner.Season == SeasonType.Winter ?
                    0 : 1);
                bytesToSend[4] = (byte)Utility.BCD(DateTime.Now.Year % 100);
                bytesToSend[5] = (byte)Utility.BCD(DateTime.Now.Month);
                bytesToSend[6] = (byte)Utility.BCD(DateTime.Now.Day);


                try
                {
                    SendDataThreadSafety(_UdpClient, bytesToSend, bytesToSend.Length, remoteEndPoint);
                }
                catch (SocketException )
                {

                }
                TraceDataInfo(true, bytesToSend, remoteEndPoint);

                // Send temperature request
                SendDataThreadSafety(_UdpClient, new byte[24], 24,
                    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 
                        _RemotePortNumber));
                // TODO May cause multi-thread problem
            }
        }

        private void SendDataThreadSafety(UdpClient udpClient, byte[] data, int length, IPEndPoint endPoint)
        {
            lock (udpClient)
            {
                udpClient.Send(data, length, endPoint);
            }
        }

        private void TraceDataInfo(bool send, byte[] data, IPEndPoint endPoint)
        {
            if (send)
            {
                Trace.WriteLine(string.Format(
                        "TCP/RCU: Sending data to {0}:{1} - {2}",
                        endPoint.Address.ToString(),
                        endPoint.Port,
                        ConvertByteArrayToString(data, 0, data.Length)), "Connection");
            }
            else
            {
                Trace.WriteLine(string.Format(
                        "TCP/RCU: Receiving data from {0}:{1} - {2}",
                        endPoint.Address.ToString(),
                        endPoint.Port,
                        ConvertByteArrayToString(data, 0, data.Length)), "Connection");
            }
        }

        /// <summary>
        /// Generate scan data according to CurrentRoom
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateCurrentRoomScanData()
        {
            return GenerateRoomScanData(_CurrentRoom);
        }

        private byte[] GenerateRoomScanData(Room room)
        {
            byte[] scanBytes = new byte[24];

            
            scanBytes[1] = 0;       // txtLeaveTemperature.Text);
            
            scanBytes[1] = (byte)Utility.BCD(Convert.ToUInt32(room.MyFloor.Number));
            scanBytes[2] = (byte)(Utility.BCD(Convert.ToUInt32(room.Number)));
            scanBytes[3] = (byte)(Convert.ToByte(room.MyAirConditioner.DefaultTemperature) +
                ((room.MyAirConditioner.DefaultRunningStatus ? 0 : 1) * 128));
            BitArray array = new BitArray(8);
            switch (room.HotelUsingStatus)
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
            array[6] = room.AutoSleep && room.IsSleepTime;
        //    array[7] = room.MyAirConditioner.ApplyRunning;//空调开关设置
            array.CopyTo(scanBytes, 4);
            scanBytes[5] = 0;//(byte)aRoom.MyAirConditioner.RunningType; //每小时开关时间分布
            scanBytes[6] = room.AutoSleep ? (byte)room.ChangeTemperature : (byte)0;//睡眠温度调节: 1~5度范围调节
            scanBytes[7] = (room.HotelUsingStatus == HotelUsingStatusType.Empty || room.HotelUsingStatus == HotelUsingStatusType.Vacant) ? (byte)0 : (byte)128;

            ///[3/16/2011] add the broadcast content in the scan room
            scanBytes[8] = (byte)Utility.BCD(DateTime.Now.Hour);
            scanBytes[9] = (byte)Utility.BCD(DateTime.Now.Minute);
            scanBytes[10] = (byte)
                (_FloorSet.FirstFloor.FirstRoom.MyAirConditioner.Season == SeasonType.Summer ? 0 : 1);
            scanBytes[11] = (byte)Utility.BCD(DateTime.Now.Year % 100);
            scanBytes[12] = (byte)Utility.BCD(DateTime.Now.Month);
            scanBytes[13] = (byte)Utility.BCD(DateTime.Now.Day);
            scanBytes[14] = (byte)(_NeedBroadcastDataTimer ? 0x8f : 0x00);

            lock (_Lock)
            {
                _NeedBroadcastDataTimer = false;
            }
            

            return scanBytes;
        }
        #endregion

        /// <summary>
        /// Apply data for the specific room and connect the room atm
        /// </summary>
        /// <param name="room"></param>
        /// <param name="receiveData"></param>
        private void ApplyData(Room room, byte[] receiveData)
        {
            Trace.WriteLine(string.Format(
                "TCP/RCU: Applying data (header {0}) for Room{1}",
                receiveData[0],
                room.ToString()), "Connection");
            BitArray aData = new BitArray(
                    new byte[] { receiveData[1], receiveData[2] });

          //  BitArray aData = new BitArray(_Connection.ReceivedFrames[i].Data);

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
            //if (aData[14] == true)
            //    room.HotelUsingStatus = HotelUsingStatusType.VIP;



            //AirConditioner Speed
            switch (receiveData[3] % 4)
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

            if ((receiveData[3] / 4) % 4 <= 1)
                room.MyAirConditioner.TurnOn();
            else
                room.MyAirConditioner.TurnOff();

            //if ((receiveData[3] / 16) % 2 == 0)
            //    room.MyAirConditioner.Season = SeasonType.Summer;
            //else
            //    room.MyAirConditioner.Season = SeasonType.Winter;


            switch (receiveData[3] / 32)
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
            room.Temperature = receiveData[4];


            //Key
            if (receiveData[5] > 0)
            {
                int CardType = receiveData[5];
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
            room.KeyID = receiveData[6].ToString();

            room.LightOnNumber = receiveData[7];
#endif
                                    

           
/* original code
            room.KeyID = receiveData[2].ToString();
            
            if (dataBit[0])
            {
                room.MyGuestService.Clean();
            }
            else
            {
                room.MyGuestService.CancelService(ServiceType.Clean);
            }

            if (dataBit[1])
            {
                room.MyGuestService.Call();
            }
            else
            {
                room.MyGuestService.CancelService(ServiceType.Call);
            }

            if (dataBit[2])
            {
                room.MyGuestService.DontDisturb();
            }
            else
            {
                room.MyGuestService.CancelService(ServiceType.DontDisturb);
            }

            if (dataBit[3])
            {
                room.MyGuestService.QuitRoom();
            }
            else
            {
                room.MyGuestService.CancelService(ServiceType.QuitRoom);
            }

            if (dataBit[4])
            {
                room.MyGuestService.Emergency();
            }
            else
            {
                room.MyGuestService.EmergencyCancel();
            }

            if (dataBit[5])
            {
                room.CofferOpen();
            }
            else
            {
                room.CofferClose();
            }

            if (dataBit[6])
            {
                room.RefrigeratorOpen();
            }
            else
            {
                room.RefrigeratorClose();
            }

            if (dataBit[7])
            {
                room.DoorOpen();
            }
            else
            {
                room.DoorClose();
            }

            // Repair flag
            if (dataBit[8])
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

            if (dataBit[9])
            {
                room.MyATM.ProblemCaused();
            }
            else
            {
                room.MyATM.ProblemRepaired();
            }

            if (dataBit[10])
            {
                room.MyGuestService.Cleaning();
            }
            else
            {
                room.MyGuestService.CancelService(ServiceType.Cleaning);
            }

            if (dataBit[11])
            {
                room.MyGuestService.StartChecking();
            }
            else
            {
                room.MyGuestService.StopChecking();
            }
            
            //room.IsLightOn = dataBit[12];

            room.Temperature = receiveData[6];
            if (dataBit[13] && 
               (receiveData[1] >= 1) &&
               (receiveData[1] <= 5))
            {
                room.KeyInsert((KeyStatusType)(receiveData[1]));
            }
            else
            {
                room.KeyPullOut();
            }

            if (receiveData[9] <= 3)
            {
                room.MyAirConditioner.Speed =
                    (AirConditionerSpeedType)receiveData[9];
            }

            if (receiveData[10] == 0)
            {
                room.MyAirConditioner.TurnOff();
            }
            else
            {
                room.MyAirConditioner.TurnOn();
            }

            // Remove disconnect counter if existed
            if (_DisconnectCounter.ContainsKey(room))
            {
                _DisconnectCounter.Remove(room);
            }
            room.MyATM.Connect();

            //chkTV.Checked = dataBit[14];

            //txtClientTemp.Text = receiveData[5].ToString();
            //txtClientHumidity.Text = receiveData[7].ToString();
            //txtHumidity.Text = receiveData[8].ToString();
            //txtPlayingVoice.Text = receiveData[12].ToString();
 * */
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


        /// <summary>
        /// Count the disconnect times for each scan
        /// </summary>
        private void CountDisconnectCurrentRoom()
        {
            Room room = CurrentRoom;
            if (!_DisconnectCounter.ContainsKey(room))
            {
                _DisconnectCounter.Add(room, 0);
            }
            _DisconnectCounter[room]++;

            if (_DisconnectCounter[room] > _RetryTimesBeforeDisconnect)
            {
                room.MyATM.Disconnect();
            }
        }

        void _FloorSet_DefaultRunningStatusChanged(object sender, DefaultRunningStatusChangedEventArgs e)
        {
            if (_IsRunning && _UdpClient != null)
            {
                try
                {
                    SendRequestRoomInformationData(e.ChangedRoom, false);
                }
                catch
                {
                }
            }
        }

        void _FloorSet_DefaultTemperatureChanged(object sender, DefaultTemperatureChangedEventArgs e)
        {
            if (_IsRunning && _UdpClient != null)
            {
                try
                {
                    SendRequestRoomInformationData(e.ChangedRoom, false);
                }
                catch
                {
                }
            }
        }


        void _FloorSet_HotelUsingStatusChanged(object sender, HotelUsingStatusChangedEventArgs e)
        {
            if (_IsRunning && _UdpClient != null)
            {
                try
                {
                    SendRequestRoomInformationData(e.ChangedRoom, false);
                }
                catch
                {
                }
            }
        }

    }
}
