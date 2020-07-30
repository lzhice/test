using System;
using ATM3300.Common;
using ATM3300.Connection;

namespace ATM3300.Connection
{
	
	public abstract class DataProviderBase
	{
		protected bool mIsRunning=false;
		/// <summary>
		/// ¸ÃAdapterµÄ°æ±¾ºÅ
		/// </summary>
	
		
		public virtual ClassInfo ClassInfo
		{
			get
			{
				return null;
			}
		}

		public virtual SettingsBase Settings
		{
			get
			{
				return null;
			}
		}


		public DataProviderBase(FloorSet aFloorSet,SettingsBase aSetting)
		{
		}

		public virtual void Open()
		{
			mIsRunning=true;
		}

		public virtual void Close()
		{
			mIsRunning=false;
		}

		public virtual bool IsRunning
		{
			get
			{	return mIsRunning;
			}
		}

		public virtual bool ShowSetupForm()
		{
			return false;
		}
	}

}
