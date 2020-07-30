//Fix a Bug For END() Disconnect
using System;
using System.Collections;
using ATM3300.Common;
using ATM3300.Connection;
using System.Windows.Forms;
using Log;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;


namespace ATM3300.Connection
{
    /// <summary>
    /// ConnectionManager ��ժҪ˵����
    /// </summary>

    #region ��Ϣί��
    public delegate void HaveStartConnectionEventHandler(object sender, System.EventArgs e);
    public delegate void HaveEndConnectionEventHandler(object sender, System.EventArgs e);
    public delegate void HaveCreatConnectionEventHandler(object sender, System.EventArgs e);
    public delegate void HaveRemoveConnectionEventHandler(object sender, System.EventArgs e);
    #endregion

    public class ConnectionType
    {
        private Type _Type;
        private ClassInfo _Info;
        private Type _SetupForm;

        public ConnectionType(Type type, ClassInfo info, Type setupForm)
        {
            _Type = type;
            _Info = info;
            _SetupForm = setupForm;
        }

        public ConnectionBase CreateConnection(FloorSet floorSt, SettingsBase settings)
        {
            return (ConnectionBase)Activator.CreateInstance(_Type, new object[] { floorSt, settings });
        }

        public Form CreateSetupForm()
        {
            return (Form)Activator.CreateInstance(_SetupForm, new object[] { });
        }

        public ClassInfo ClassInfo
        {
            get
            {
                return _Info;
            }
        }

        public Type Type
        {
            get
            {
                return _Type;
            }
        }

    }

    public class ConnectionManager
    {
        private SettingsBase _Settings;
        private Dictionary<string, ConnectionBase> _Connections = new Dictionary<string, ConnectionBase>();
        private EventLogger _Log = new EventLogger();
        private Dictionary<string, ConnectionType> _ConnectionTypes = new Dictionary<string, ConnectionType>();
        private List<ConnectionType> _ConnectionTypeList = new List<ConnectionType>();

        #region �¼�����
        public event HaveStartConnectionEventHandler HaveStartConnection;
        public event HaveEndConnectionEventHandler HaveEndConnection;
        public event HaveCreatConnectionEventHandler HaveCreatConnection;
        public event HaveRemoveConnectionEventHandler HaveRemoveConnection;

        public void OnHaveStartConnection(System.EventArgs e)
        {
            if (HaveStartConnection != null)
                HaveStartConnection(this, e);
        }
        public void OnHaveEndConnection(System.EventArgs e)
        {
            if (HaveStartConnection != null)
                HaveEndConnection(this, e);
        }
        public void OnHaveCreatConnection(System.EventArgs e)
        {
            if (HaveStartConnection != null)
                HaveCreatConnection(this, e);
        }
        public void OnHaveRemoveConnection(System.EventArgs e)
        {
            if (HaveStartConnection != null)
                HaveRemoveConnection(this, e);
        }
        #endregion

        public ConnectionManager(SettingsBase theSetting)
        {
            _Settings = theSetting;
            InitConnecionTypeInfos();
            InitConnecionTypes();
        }

        #region Register Connection Type Part
        private void InitConnecionTypes()
        {
            // Register all Types of Connection
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(ConnectionBase)))
                {
                   Attribute[] versionAttribute = type.GetCustomAttributes(typeof(ConnectionVersionInfoAttribute), false)  as Attribute[];
                   if ((versionAttribute!= null) && (versionAttribute.Length == 1))
                   {
                       // Find the connversionVersion attribute
                       ConnectionVersionInfoAttribute versionInfoAttribute = versionAttribute[0] as ConnectionVersionInfoAttribute;
                       _ConnectionTypes.Add(
                           versionInfoAttribute.ClassInfo.Name, 
                           new ConnectionType(type, versionInfoAttribute.ClassInfo, versionInfoAttribute.ShowSetupForm));

                       _ConnectionTypeList.Add(
                           new ConnectionType(type, versionInfoAttribute.ClassInfo, versionInfoAttribute.ShowSetupForm));
                   }
                }
            }

            //_ConnectionTypes.Add("TCPIP", new ConnectionType(typeof(Client), _Info[0], typeof(ConnectionTCPIPSetupForm)));
            //_ConnectionTypes.Add("COM", new ConnectionType(typeof(ConnectionScannerCOM), _Info[1], typeof(ConnectionCOMSetupForm)));
            //_ConnectionTypes.Add("FILE", new ConnectionType(typeof(ConnectionTriggerFileText), _Info[2], typeof(ConnectionFILESetupForm)));
            //_ConnectionTypes.Add("CANBUS", new ConnectionType(typeof(ConnectionCANBus), _Info[3], typeof(ConnectionCANBusSetupForm)));
            //_ConnectionTypes.Add("NEWCANBUS", new ConnectionType(typeof(ConnectionNewCANBus), _Info[4], typeof(ConnectionCANBusSetupForm)));
        }

        private void InitConnecionTypeInfos()
        {
            //_Info[0] = new ClassInfo("TCPIP����", "{65F77E31-70B0-4d44-9805-439DA6B0A4C5}", new Version("1.0.0"), "ͨ��TCPIPЭ��Զ�̻�ȡ������Ϣ��");
            //_Info[1] = new ClassInfo("��׼��������Э��", "{3F5F27CB-A661-4882-A29C-DD99E456CAC0}", new Version("1.0.2"), "ͨ����׼���ڻ�ȡ������Ϣ��");
            //_Info[2] = new ClassInfo("�ļ�����Э��", "{2E0112ED-9D51-423a-9C8C-A564B61C11B3}", new Version("1.0.0"), "ͨ������һ���ļ������ݸı�����ȡ������Ϣ��");
            //_Info[3] = new ClassInfo("��׼CANBus����Э��", "{0140FA7F-80ED-4f54-AFC7-C4D1C49404DE}", new Version("1.0.0"), "ͨ����׼CANBus��ȡ������Ϣ��");
            //_Info[4] = new ClassInfo("�±�׼CANBus����Э��", "{4D60FABD-49D1-42ed-B1BF-E55AD811EE57}", new Version("1.0.0"), "ͨ����׼CANBus��ȡ������Ϣ������汾�Ļ�ȡ���ֽ�Ϊ8");
        }
        #endregion

        public ConnectionType[] GetConnectionTypes()
        {
            return _ConnectionTypeList.ToArray();
        }

        public void CreateConnection(FloorSet floorSet, SettingsBase settings, ConnectionType type)
        {
            if (settings == null)
            {
                return;
            }

            // Get the FloorSet Information to Settings
            string floorInfo = string.Empty;
            foreach (int floorNumber in floorSet.FloorNumbers)
            {
                // Save each floor number
                floorInfo += ("F" + floorNumber);

                // Save each room number
                foreach (Room room in floorSet[floorNumber].GetRooms())
                {
                    floorInfo += ("R" + room.Number);
                }
            }

            settings["Type"] = type.ClassInfo.Name;
            ConnectionBase newConnection = type.CreateConnection(floorSet, settings);

            if (newConnection == null)
            {
                _Log.WriteLog("û�д�������ӷ�ʽ");
                //���û�������֣��ͱ���
                return;
            }
            else
            {
                //has created the new connection , add informations
                settings["FloorSet"] = floorInfo;
                this._Connections.Add(settings["Name"].ToString(), newConnection);

                if ((Convert.ToBoolean(settings["IsRunning", false]) == true) && (!newConnection.IsRunning))
                {
                    // Try to connect it
                    try
                    {
                        newConnection.Connect();
                    }
                    catch
                    {
                    }
                }

                OnHaveCreatConnection(EventArgs.Empty);
            }
        }

        public FloorSet StartConnection()
        {
            FloorSet allFloorSet = new FloorSet(_Settings);
            
            string errorSetting = "";
            List<string> errorList = new List<string>();

            if (_Settings == null)
            {
                _Settings.NewSubSettings("Connection");
                return allFloorSet;

            }

            // Create connection for each connection subSettings
            foreach (string s in _Settings.SubSettings.Keys)
            {
                SettingsBase connectionSetting = _Settings.SubSettings[s];
                try
                {
                    ConnectionBase newConnection = CreateConnection(allFloorSet, connectionSetting);

                    _Connections.Add(s, newConnection);

                    if ((Convert.ToBoolean(connectionSetting["IsRunning", false]) == true) && (!newConnection.IsRunning))
                    {
                        // Try to connect it
                        try
                        {
                            newConnection.Connect();
                        }
                        catch
                        {
                        }
                    }

                }
                catch
                {
                    errorList.Add(s);
                    errorSetting += s;
                }
            }
            if (!string.IsNullOrEmpty(errorSetting))
            {
                // Remove Error Connection
                for (int i = 0; i < errorList.Count; i++)
                {
                    Trace.WriteLine(string.Format("Removing {0} connection", errorList[i]), "Connection");
                    _Settings.SubSettings.Remove((string)errorList[i]);
                }
                Trace.WriteLine(errorSetting + " Connection error, unable to create");
                //_Log.WriteLog(errorSetting + "���ô����޷�������");
            }
            OnHaveStartConnection(EventArgs.Empty);
            return allFloorSet;
        }

        private ConnectionBase CreateConnection(FloorSet allFloorSet, SettingsBase connectionSetting)
        {
            // Parser the floor info from the settings
            FloorSet newConnectionFloorSet;
            string[] floorInfo;
            if (connectionSetting["FloorSet"].ToString().Contains("L"))
            {
                // Use old parser method
                floorInfo = ((string)connectionSetting["FloorSet"]).Split(new char[] { 'F', 'L' });

                // First Detect the FloorNumber has exist and the value is correct
                for (int i = 0, FloorNumber, FloorLength; i <= (floorInfo.Length / 2) - 1; i++)
                {
                    FloorNumber = Convert.ToInt32(floorInfo[i * 2 + 1]);
                    FloorLength = Convert.ToInt32(floorInfo[i * 2 + 2]);
                    if ((allFloorSet[FloorNumber] != null) || (FloorLength <= 0) || (FloorLength >= 100))
                    {
                        throw new ApplicationException("Settings�е�¥��ֵ�д���!");
                    }
                }

                // Start to create FloorSet
                newConnectionFloorSet = new FloorSet();
                for (int i = 0; i <= (floorInfo.Length / 2) - 1; i++)
                {
                    Floor floor = new Floor(Convert.ToInt32(floorInfo[i * 2 + 1]), Convert.ToInt32(floorInfo[i * 2 + 2]));
                    newConnectionFloorSet.Add(floor);
                    allFloorSet.Add(floor);
                }
            }
            else
            {
                // Use new parser method
                floorInfo = connectionSetting["FloorSet"].ToString().Split(new char[] { 'F' }, StringSplitOptions.RemoveEmptyEntries);

                // Parser each floor's room information
                Dictionary<int, int[]> roomNumberInfo = new Dictionary<int, int[]>();
                for (int floorIndex = 0; floorIndex < floorInfo.Length; floorIndex++)
                {
                    string[] roomInfo = floorInfo[floorIndex].Split('R');

                    // Obtain floor number
                    int floorNumber = int.Parse(roomInfo[0]);
                    if (allFloorSet[floorNumber] != null)
                    {
                        Trace.WriteLine(string.Format(
                            "Settings:{0} contains duplicated floor {1}",
                            connectionSetting["Name"],
                            floorNumber),
                            "Connection");
                        throw new ApplicationException("Settings�е�¥��ֵ�д���!");
                    }

                    // Obtain room numbers
                    List<int> rooms = new List<int>();
                    for (int roomIndex = 1; roomIndex < roomInfo.Length; roomIndex++)
                    {
                        int newRoomNumber = int.Parse(roomInfo[roomIndex]);
                        if ((newRoomNumber <= 0) || (newRoomNumber >= 100) || rooms.Contains(newRoomNumber))
                        {
                            Trace.WriteLine(string.Format(
                            "Settings:{0} contains illegal room {1}",
                            connectionSetting["Name"],
                            floorNumber),
                            "Connection");
                            throw new ApplicationException("Settings�е�¥��ֵ�д���!");
                        }

                        rooms.Add(newRoomNumber);
                    }

                    roomNumberInfo.Add(floorNumber, rooms.ToArray());
                }

                // Create new floor set for the new connection
                newConnectionFloorSet = new FloorSet();
                foreach (KeyValuePair<int, int[]> floorRoomInfo in roomNumberInfo)
                {
                    Floor floor = new Floor(floorRoomInfo.Key, floorRoomInfo.Value);
                    newConnectionFloorSet.Add(floor);
                    allFloorSet.Add(floor);
                }
            }




            // Create new Connection
            ConnectionBase newConnection = null;
            string newConnectionTypeName = connectionSetting["Type"].ToString();

            if (_ConnectionTypes[newConnectionTypeName] != null)
            {
                newConnection = ((ConnectionType)_ConnectionTypes[newConnectionTypeName]).CreateConnection(newConnectionFloorSet, connectionSetting);
            }
            else
            {
                throw new Exception("Settings�к���δ֪��Э��");
            }
            return newConnection;
        }

        public void EndAllConnection()
        {
            foreach (string s in _Settings.SubSettings.Keys)
            {
                _Settings.SubSettings[s]["IsRunning"] = ((ConnectionBase)_Connections[s]).IsRunning;
                if (((ConnectionBase)_Connections[s]).IsRunning)
                    ((ConnectionBase)(_Connections[s])).Disconnnect();
            }
            OnHaveEndConnection(EventArgs.Empty);
        }

        public void RemoveConnection(ConnectionBase connection)
        {
            connection.Disconnnect();
            string ConnectionName = string.Empty;

            foreach (string s in _Connections.Keys)
            {
                if (_Connections[s] == connection)
                {
                    ConnectionName = s;
                    break;
                }
            }

            if (ConnectionName == string.Empty)
            {
                throw new Exception("�Ҳ���������");
            }
            else
            {
                _Settings.SubSettings.Remove(ConnectionName);
                _Connections.Remove(ConnectionName);
                OnHaveRemoveConnection(EventArgs.Empty);
            }
        }

        public Dictionary<string, ConnectionBase> GetConnections()
        {
            return _Connections;
        }

        /// <summary>
        /// Use ClassInfo to locate a ConnectionType from ConnecionTypes
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public ConnectionType SearchType(ClassInfo Info)
        {
            foreach (KeyValuePair<string, ConnectionType> Entry in _ConnectionTypes)
            {
                if (Entry.Value.ClassInfo.GUID == Info.GUID)
                    return Entry.Value;
            }
            return null;
        }

        public bool ShowSetupForm(ClassInfo aInfo, SettingsBase aSetting)
        {
            System.Windows.Forms.Form aForm;

            if (SearchType(aInfo) != null)
            {
                aForm = SearchType(aInfo).CreateSetupForm();
                ((IShowSetupFormInterface)aForm).Settings = aSetting;
                aForm.ShowDialog();
                return aForm.DialogResult == DialogResult.OK;
            }
            return false;

        }
    }
}




