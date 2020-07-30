using System;
using ATM3300.Common;
using System.Collections;


namespace ATM3300.Connection
{
    /// <summary>
    /// This CANBus connection extent from 7 bytes protocol to 8 bytes protocol
    /// </summary>
    [ConnectionVersionInfo(
        "新标准CANBus连接协议", "{4D60FABD-49D1-42ed-B1BF-E55AD811EE57}", "1.0.0", "通过标准CANBus获取房间信息。这个版本的获取的字节为8",
        typeof(ConnectionCANBusSetupForm))]
    public class ConnectionNewCANBus : ConnectionCANBus
    {

        public ConnectionNewCANBus(FloorSet aFloorSet, SettingsBase ConnectionSettings)
            : base(aFloorSet, ConnectionSettings)
        {
        }

        #region 继承

        public override ClassInfo ClassInfo()
        {
            return new ClassInfo("新标准CANBus连接协议", "{4D60FABD-49D1-42ed-B1BF-E55AD811EE57}", new Version("1.0.0"), "通过标准CANBus获取房间信息。这个版本的获取的字节为8");
        }

        #endregion


        protected override void mConnection_NewFramesReceived(object sender, EventArgs e)
        {
            lock (_Connection.ReceivedFrames)
            {
                WriteLog("New开始：[" + DateTime.Now.ToShortTimeString() + "]");
                WriteLog(_Connection.ReceivedFrames.Length.ToString());
                for (int i = 0; i < _Connection.ReceivedFrames.Length ; i++)
                {
                    WriteLog("Frames：" + i);
                    WriteLog(_Connection.ReceivedFrames[i].DataLength.ToString());
#if !DEBUG
                    if (_Connection.ReceivedFrames[i].DataLength >= 8)
#else
                    if (_Connection.ReceivedFrames[i].DataLength >= 6)
#endif   
                    {
                        //Use CheckSum to Check the data
                        int CheckSum = 0;
                        for (int j = 0; j < _Connection.ReceivedFrames[i].DataLength - 1; j++)
                        {
                            CheckSum += _Connection.ReceivedFrames[i].Data[j];
                            WriteLog("data" + j + _Connection.ReceivedFrames[i].Data[j].ToString());
                        }
                        WriteLog("CheckSum:" + CheckSum.ToString());

                        WriteLog(_Connection.ReceivedFrames[i].Data[_Connection.ReceivedFrames[i].DataLength - 1].ToString());

                        if (((byte)(CheckSum % 256) == _Connection.ReceivedFrames[i].Data[_Connection.ReceivedFrames[i].DataLength - 1]) )		//Received Success
                        {

                            // Retrieve floorNumber and roomNumber from received data
                            int floorNum, roomNum;
                            floorNum = (int)Utility.UNBCD((_Connection.ReceivedFrames[i].FrameID >> 21));
                            roomNum = (int)Utility.UNBCD(((_Connection.ReceivedFrames[i].FrameID >> 13) % 256));

                            WriteLog("FrameID:" + _Connection.ReceivedFrames[i].FrameID);
                            WriteLog("Room:" + floorNum + roomNum);
                            if ((_FloorSet[floorNum] != null) && (_FloorSet[floorNum].GetRoomByRoomNumber(roomNum) != null))
                            {
                                Room room = _FloorSet[floorNum].GetRoomByRoomNumber(roomNum);
                                
                                // Reset Disconnect value
                                _DisconnectList[room.ToString()] = 0;

                                if (AcceptToReceive(room))
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

                                    ////Repair Flag
                                    //if (aData[8] == true)
                                    //    room.HotelUsingStatus = HotelUsingStatusType.Maintanent;
                                    //else
                                    //{
                                    //    if (room.HotelUsingStatus == HotelUsingStatusType.Maintanent)
                                    //        room.HotelUsingStatus = HotelUsingStatusType.Vacant;
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

                           ////         room.IsVIP = aData[14];
                           //         if (aData[14] == true)
                           //             room.HotelUsingStatus = HotelUsingStatusType.VIP;
                                   

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
                                    //TODO Other Key Type
                                    if (_Connection.ReceivedFrames[i].Data[4] > 0)
                                    {
                                        int CardType = _Connection.ReceivedFrames[i].Data[4];
                                        if (CardType == 3)
                                            room.KeyInsert(KeyStatusType.Leader);
                                        else if (CardType == 6)
                                            room.KeyInsert(KeyStatusType.Servant);
                                        else if (CardType == 8)
                                            room.KeyInsert(KeyStatusType.Cleaner);
                                        else
                                            //if (aData[14] == true)
                                            //    room.KeyInsert(KeyStatusType.VIPGuest);
                                            //else
                                            room.KeyInsert(KeyStatusType.Guest);
                                    }
                                    else
                                        room.KeyPullOut();
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

                                baseSetting["OutsideTemperature"] = _Connection.ReceivedFrames[i].Data[3].ToString();

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
                                SendFeedback(data);
                            }
                        }

                    }
                    WriteLog("End Frames：" + i);
                }
            }
        }

    }
}
