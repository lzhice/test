using System;
using System.Collections;
using ATM3300.Common;
using ATM3300.Connection;
using System.Windows.Forms;

namespace ATM3300.Connection
{
	/// <summary>
	/// DataProviderManager 的摘要说明。
	/// </summary>
	/// 


	#region 消息委托

	public delegate void HaveStartServerEventHandler(object sender,System.EventArgs e);
	public delegate void HaveEndServerEventHandler(object sender,System.EventArgs e);
	#endregion

	public class DataProviderManager
	{
		private FloorSet mFloorSet;
		private SettingsBase mSetting;
		private Hashtable myAL = new Hashtable();
		
	#region 事件声明
		public event HaveStartServerEventHandler HaveStartServer;
		public event HaveEndServerEventHandler HaveEndServer;

		public void OnHaveStartServer(System.EventArgs e)
		{
			if(HaveStartServer!=null)
				HaveStartServer(this,e);
		}
		public void OnHaveEndServer(System.EventArgs e)
		{
			if(HaveStartServer!=null)
				HaveEndServer(this,e);
		}
		#endregion


		public DataProviderManager(SettingsBase theSetting,FloorSet aFloorSet)
		{
			mSetting=theSetting;
			mFloorSet=aFloorSet;
		}

		public  Hashtable GetDataProviders()
		{
			return myAL;
		}

		public void StartService()
		{
						
			if (mSetting.SubSettings["TCPIP"]==null) 
			{
				//从注册表的Database中获取数据源，若Database不存在则创建它并给数据赋初值
				mSetting.NewSubSettings("TCPIP");
				mSetting.SubSettings["TCPIP"]["IP"]="0.0.0.0";
				mSetting.SubSettings["TCPIP"]["Port"]="15000";
				mSetting.SubSettings["TCPIP"]["Type"]="TCPIP";
				mSetting.SubSettings["TCPIP"]["IsRunning"]=false;

				mSetting.Save();
				TcpServer TCPIPServer=new TcpServer(mFloorSet,mSetting.SubSettings["TCPIP"]);
				this.myAL.Add("TCPIP",TCPIPServer);
				
				if(mSetting.SubSettings["TCPIP"]["IsRunning"].ToString()=="true") TCPIPServer.Open();

			}
			else 
			{
				if(mSetting.SubSettings["TCPIP"]["IP"]==null){mSetting.SubSettings["TCPIP"]["IP"]="0.0.0.0";}
				if(mSetting.SubSettings["TCPIP"]["Port"]==null){mSetting.SubSettings["TCPIP"]["Port"]="15000";}
				if(mSetting.SubSettings["TCPIP"]["Type"]==null){mSetting.SubSettings["TCPIP"]["Type"]="TCPIP";}
				if(mSetting.SubSettings["TCPIP"]["IsRunning"]==null) mSetting.SubSettings["TCPIP"]["IsRunning"]=false;

				TcpServer TCPIPServer=new TcpServer(mFloorSet,mSetting.SubSettings["TCPIP"]);
				this.myAL.Add("TCPIP",TCPIPServer);
				
				if ((Convert.ToBoolean(mSetting.SubSettings["TCPIP"]["IsRunning",false])==true)&& (!TCPIPServer.IsRunning))
					TCPIPServer.Open();
			}


			if (mSetting.SubSettings["COM"]==null) 
			{
				//从注册表的Database中获取数据源，若Database不存在则创建它并给数据赋初值
				mSetting.NewSubSettings("COM");
				mSetting.SubSettings["COM"]["CommPort"]=1;
				mSetting.SubSettings["COM"]["Settings"]="2400,e,8,1";
				mSetting.SubSettings["COM"]["Timeout"]=100;
				mSetting.SubSettings["COM"]["Type"]="COM";
				mSetting.SubSettings["COM"]["IsRunning"]=false;

				mSetting.Save();
				DataProviderATMSimulator COMServer =new DataProviderATMSimulator(mFloorSet,mSetting.SubSettings["COM"]);
				this.myAL.Add("COM",COMServer);
				if(mSetting.SubSettings["COM"]["IsRunning"].ToString()=="true") COMServer.Open();
			}
			else 
			{
				if (mSetting.SubSettings["COM"]["CommPort"]==null)	mSetting.SubSettings["COM"]["CommPort"]=2;
				if (mSetting.SubSettings["COM"]["Settings"]==null)	mSetting.SubSettings["COM"]["Settings"]="2400,e,8,1";
				if (mSetting.SubSettings["COM"]["Timeout"]==null)	mSetting.SubSettings["COM"]["Timeout"]=100;
				if (mSetting.SubSettings["COM"]["IsRunning"]==null) mSetting.SubSettings["COM"]["IsRunning"]=false;
				DataProviderATMSimulator COMServer =new DataProviderATMSimulator(mFloorSet,mSetting.SubSettings["COM"]);
				this.myAL.Add("COM",COMServer);
				if ((Convert.ToBoolean(mSetting.SubSettings["COM"]["IsRunning",false])==true)&& (!COMServer.IsRunning))
					COMServer.Open();
			}			
			OnHaveStartServer(new System.EventArgs());							
		}

		public void End()
		{
			foreach (string s in mSetting.SubSettings.Keys)
			{				
				mSetting.SubSettings[s]["IsRunning"]=((DataProviderBase)(myAL[s])).IsRunning;
				if 	(	(  (DataProviderBase) (myAL[s])  ).IsRunning    )
					(  (DataProviderBase) (myAL[s]) ).Close();
			}
			OnHaveEndServer(new System.EventArgs());
		}

	}
}
