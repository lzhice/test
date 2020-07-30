using System;
using ATM3300.Common;

namespace ATM3300.Connection
{
    public abstract class ConnectionBase
    {

        protected bool _IsRunning = false;
        protected FloorSet _FloorSet = null;
        protected SettingsBase _Settings = null;

        /// <summary>
        /// ¸ÃAdapterµÄ°æ±¾ºÅ
        /// </summary>

        public virtual ClassInfo ClassInfo()
        {
            return null;
        }

        public ConnectionBase(FloorSet floorSet, SettingsBase setting)
        {
            _FloorSet = floorSet;
            _Settings = setting;
        }

        /// <summary>
        /// Connect to the connection
        /// </summary>
        /// <exception cref="Exception"></exception>
        public virtual void Connect()
        {
            _IsRunning = true;
        }

        /// <summary>
        /// Disconnection to the connection. No exception will be thrown
        /// </summary>
        public virtual void Disconnnect()
        {
            _IsRunning = false;
        }

        public virtual bool IsRunning
        {
            get
            {
                return _IsRunning;
            }
        }


        public virtual SettingsBase Settings
        {
            get
            {
                return _Settings;
            }
        }

        public virtual FloorSet FloorSet
        {
            get
            {
                return _FloorSet;
            }
        }

        public virtual bool ShowSetupForm()
        {
            return false;
        }

        /// <summary>
        /// Apply new settings if changed
        /// </summary>
        /// <exception cref="Exception"></exception>
        public virtual void ResetSetting()
        {

        }
    }

    public abstract class ConnectionScannerBase : ConnectionBase
    {
        protected Room _CurrentRoom = null;
        public event EventHandler<EventArgs> CurrentScanRoomChanged;

        protected void OnCurrentScanRoomChanged(EventArgs args)
        {
            if (CurrentScanRoomChanged != null)
            {
                CurrentScanRoomChanged(this, args);
            }
        }    


        
        public virtual Room CurrentRoom
        {
            get
            {
                return _CurrentRoom;
            }
            protected set
            {
                _CurrentRoom = value;
                OnCurrentScanRoomChanged(EventArgs.Empty);
            }
        }

        public virtual Room NextRoom
        {
            get
            {
                if (_CurrentRoom == _CurrentRoom.MyFloor.LastRoom)
                {
                    return _FloorSet.NextFloor(_CurrentRoom.MyFloor).FirstRoom;
                }
                else
                {
                    return _CurrentRoom.MyFloor.GetNextRoom(_CurrentRoom.Number); 
                }
            }
        }

        public ConnectionScannerBase(FloorSet floorSet, SettingsBase setting)
            : base(floorSet, setting)
        {
            _CurrentRoom = _FloorSet.FirstFloor.FirstRoom;
        }



    }



}
