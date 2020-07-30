//This part has stop
//The Next Version will use TCPIP
//Date  [4/4/2004]
/*
using System;

namespace ATM3300.Connection.ComputerCOM
{
	
	/// <summary>
	/// ScannerCOM ��ժҪ˵����
	/// </summary>
	public class AdapterComputerCOM : AdapterScannerBase
	{
		public event System.EventHandler CurrentScanRoomChanged;

		protected MSCommClass mComm=new MSCommClass();

		protected int mReceiveDataRepeatTimes=5;
		protected int mReceiveDataRepeatDelay=50;	//millionseconds
		protected int mCurrentReceiveDataRepeatTimes=0;
		

		protected byte[] mReceiveData=new byte[5];

		protected int mDisconnectRetryTimes=3;
		protected Hashtable mDisconnectList=new Hashtable();

		protected System.Timers.Timer mScannerTimeout=new System.Timers.Timer();
			
		protected Room mCurrentRoom;
		protected FloorSet mFloorSet;
		
		public AdapterComputerCOM(FloorSet aFloorSet,SettingsBase ConnectionSetting) :base(aFloorSet,ConnectionSetting)
		{
			//Ĭ��ѡ��
			mComm.CommPort=1;
			mComm.DTREnable=false;
			mComm.EOFEnable=false;
			mComm.Handshaking=HandshakeConstants.comNone;	//None Handshakeing
			mComm.InBufferSize=1024;
			mComm.InputMode=InputModeConstants.comInputModeBinary;
			mComm.NullDiscard=false;
			mComm.OutBufferSize=512;
			mComm.ParityReplace="?";
			mComm.RThreshold=0;	//R բֵ
			mComm.RTSEnable=false;
			mComm.Settings="115200,e,8,1";
			mComm.SThreshold=0;	//S բֵ

			//�����Զ���ѡ��
			//��鿴��ѡ���Ƿ����
			if (ConnectionSetting["CommPort"]==null)	ConnectionSetting["CommPort"]=1;
			if (ConnectionSetting["Settings"]==null)	ConnectionSetting["Settings"]="115200,e,8,1";
			if (ConnectionSetting["ReceiveDataRepeatTimes"]==null)	ConnectionSetting["ReceiveDataRepeatTimes"]=5;
			if (ConnectionSetting["DisconnectRetryTimes"]==null)	ConnectionSetting["DisconnectRetryTimes"]=3;
			if (ConnectionSetting["ReceiveDataRepeatDelay"]==null)	ConnectionSetting["ReceiveDataRepeatDelay"]=3;

			
			//Ӧ��ѡ��
			mComm.CommPort=(short)ConnectionSetting["CommPort"];
			mComm.Settings=(string)ConnectionSetting["Settings"];
			mReceiveDataRepeatTimes=(int)ConnectionSetting["ReceiveDataRepeatTimes"];
			mDisconnectRetryTimes=(int)ConnectionSetting["DisconnectRetryTimes"];
			mReceiveDataRepeatDelay=(int)ConnectionSetting["ReceiveDataRepeatDelay"];


			mScannerTimeout.Interval=mReceiveDataRepeatDelay;
			mScannerTimeout.Elapsed+=new System.Timers.ElapsedEventHandler(mScannerTimeout_Elapsed);

			mComm.OnComm+=new DMSCommEvents_OnCommEventHandler(mComm_OnComm);

			//����
			mFloorSet=aFloorSet;
			mCurrentRoom=mFloorSet.FirstFloor.FirstRoom;
		}

		public AdapterComputerCOM(FloorSet aFloorSet,SettingsBase ConnectionSetting,SettingsBase ProgramSetting) :base(aFloorSet,ConnectionSetting)
		{
		}

		

		#region �̳�

		public override string Version
		{
			get
			{
				return "0.0.1 alpha";
			}
		}

		public override string Name
		{
			get
			{
				return "Adapter.Computer.COM.Normal";
			}
		}

		public override string Comment
		{
			get
			{
				return "This is the protocol of ATM3300 computer communication.";
			}
		}

		public override string GUID
		{
			get
			{
				return "{9FC20386-B787-47ca-875E-E5044864D048}";
			}
		}

		public override void Connect()
		{
			base.Connect ();
			mComm.PortOpen=true;  //Open port
			StartRoomInformatioRequest();
			mIsRunning=true;
		}

		public override void Disconnnect()
		{
			base.Disconnnect();
			mScannerTimeout.Stop();
			mComm.PortOpen=false;  //Close Port
			mIsRunning=false;
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
				if (mCurrentRoom==mCurrentRoom.MyFloor.LastRoom)
				{
					return	mFloorSet.NextFloor(mCurrentRoom.MyFloor).FirstRoom;	//Goto the next Floor first room
				}
				else
				{
					return	 mCurrentRoom.MyFloor[mCurrentRoom.Number+1];	//Goto the next Room of the Floor
				}
			}
		}

		private void SendRequestRoomInformationData()
		{
			byte[] SendData=new byte[4];
			SendData[0]=(byte)64;	//@ Char
			SendData[1]=(byte)4;	//length of Data
			SendData[2]=(byte)0;	//Get Room Info Command
			SendData[3]=Utility.MakeCheckSumValue(SendData);
			//Send Data To ATM
			mComm.Output=SendData;
		}

		//TODO Send AirCon and HotelUsingStatus TO ATM()

		private void StartRoomInformatioRequest()
		{
			mCurrentReceiveDataRepeatTimes=0;
			mCurrentReceiveDataCheckRepeatTimes=0;
			mScannerTimeout.Start();
			OnCurrentScanRoomChanged(new EventArgs());
		}

		private void StartNextRoomInformationRequest()
		{
			mCurrentRoom=NextRoom;
			StartRoomInformatioRequest();
		}

		private void StartNextCheckInformationRequest()
		{
			mCurrentReceiveDataRepeatTimes++;
			if (mCurrentReceiveDataRepeatTimes>mReceiveDataRepeatTimes)
			{
				DisconnectCause();
				StartNextRoomInformationRequest();
			}
			else
			{
				mCurrentReceiveDataCheckRepeatTimes=0;
				SendRequestRoomInformationData();
				mScannerTimeout.Start();
			}
		}

		/// <summary>
		/// ���ӳ�ʱ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mScannerTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			StartNextRoomInformationRequest();
		}

		/// <summary>
		/// ���� Comm���¼������������¼�
		/// </summary>
		private void mComm_OnComm()
		{
			switch(mComm.CommEvent) 
			{
				case (short)OnCommConstants.comEvReceive:
			
					mScannerTimeout.Stop();
					//���ܵ�������
					//У������
					byte[]	ReceiveData;
					ReceiveData=(byte[])mComm.Input;

					if (ReceiveData[0]!='#'){StartNextCheckInformationRequest();return;};
					if (ReceiveData.Length)
					
					
					//CheckSumУ��
					if (!Utility.IsCheckSumCorrect(ReceiveData))
					{StartNextCheckInformationRequest();return;};
					if ((256-(mCurrentRoom.Number+mCurrentRoom.MyFloor.Number)%256)!=ReceiveData[0])	{StartNextCheckInformationRequest();return;};

					if (mCurrentReceiveDataCheckRepeatTimes==0)
					{
						mReceiveCheckData=ReceiveData;
					}
					else
					{
						//�ж��ǲ�����ǰ������ͬ
						if (mReceiveCheckData!=ReceiveData)
						{StartNextCheckInformationRequest();return;};
					}

					//��һ��
					mCurrentReceiveDataCheckRepeatTimes++;
					if (mCurrentReceiveDataCheckRepeatTimes>=mReceiveDataCheckRepeatTimes)
					{
						//���ݶ���ȷ
						ApplyData();
						//�ж��Ƿ����
						if (mDisconnectList[mCurrentRoom.ToString()]!=null)
							mDisconnectList.Remove(mCurrentRoom.ToString());
						mCurrentRoom.MyATM.Connect();

						StartNextRoomInformationRequest();
					}
					else
					{
						//��һ��У���ѯ
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
		/// �������ݲ�����FloorSet
		/// </summary>
		private void ApplyData()
		{
			BitArray aData=new BitArray(mReceiveCheckData);
			
			//Key
			if (aData[8]==true)
			{
				switch(mReceiveCheckData[3]%8) 
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
			if ((aData[8+3]==true)&&(aData[8+4]==true))
			{	 mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Empty;}
			else
			{
				if (mCurrentRoom.HotelUsingStatus==HotelUsingStatusType.Empty)	//��һ��״̬Ϊ�շ�
					mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Vacant;

				//Apply Service
				if (aData[8+2]==true)	mCurrentRoom.MyGuestService.Call();
				else if (aData[8+3]==true)	mCurrentRoom.MyGuestService.Clean();
				else if (aData[8+4]==true)	mCurrentRoom.MyGuestService.DontDisturb();
				else mCurrentRoom.MyGuestService.NoService();
			}

			//Door
			if (aData[8+1]==true)
				mCurrentRoom.DoorOpen();
			else
				mCurrentRoom.DoorClose();


			//Temperature
			mCurrentRoom.Temperature=mReceiveCheckData[2];

			//AirConditionerSpeed
			if ((aData[8+6]==true)&&(aData[8+7]==true))
			{
				mCurrentRoom.MyAirConditioner.Speed=AirConditionerSpeedType.High;
			}
			else if ((aData[8+6]==false)&&(aData[8+7]==true))
			{
				mCurrentRoom.MyAirConditioner.Speed=AirConditionerSpeedType.Mid;
			}
			else if ((aData[8+6]==true)&&(aData[8+7]==false))
			{
				mCurrentRoom.MyAirConditioner.Speed=AirConditionerSpeedType.Low;
			}
			else if ((aData[8+6]==false)&&(aData[8+7]==false))
			{
				mCurrentRoom.MyAirConditioner.Speed=AirConditionerSpeedType.Off;
			}

			//Problem
			if (aData[24+4]==true)
				mCurrentRoom.MyATM.ProblemCaused();
			else
				mCurrentRoom.MyATM.ProblemRepaired();

			//HotelUsingStatus- Cleaning
			if (aData[24+3]==true)
			{	mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Cleaning;}
			else
			{
				if (mCurrentRoom.HotelUsingStatus==HotelUsingStatusType.Cleaning)
					mCurrentRoom.HotelUsingStatus=HotelUsingStatusType.Vacant;
			}
			
		}

		/// <summary>
		/// ��ĳһ�β�ѯʱ����
		/// </summary>
		private void DisconnectCause()
		{
			if (mDisconnectList[mCurrentRoom.ToString()]==null)
			{
				mDisconnectList.Add(mCurrentRoom.ToString(),0);
			}

			mDisconnectList[mCurrentRoom.ToString()]=(int)mDisconnectList[mCurrentRoom.ToString()]+1;
			if ((int)mDisconnectList[mCurrentRoom.ToString()]>mDisconnectRetryTimes)
				mCurrentRoom.MyATM.Disconnect();
		}

		private void OnCurrentScanRoomChanged(System.EventArgs e)
		{
			if (CurrentScanRoomChanged!=null)
				CurrentScanRoomChanged(this,e);
		}
	}
}
*/