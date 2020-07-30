using System.Reflection;
using System.Runtime.CompilerServices;

//
// 有关程序集的常规信息是通过下列
// 属性集控制的。更改这些属性值可修改与程序集
// 关联的信息。
//
[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

//
// 程序集的版本信息由下列 4 个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 您可以指定所有这些值，也可以使用“修订号”和“内部版本号”的默认值，方法是按
// 如下所示使用 '*':

[assembly: AssemblyVersion("1.0.11.6")]
//TODO  When setup wrong port number in SerialPort (Ex: the port number doesn't exist) and ResetSettings, 
//          the PortNumber doesn't save in SerialPort object (since when setting new port value, it throws exception)


//Version List 
//  1.0.11.0
//  Add: English support
//  Fix: A Generic bug for CANBus
//  Add: Send DefaultTemperature to Rcu by TcpIp 3
//  Add: Book command in TcpIpDataProvider, updating its version to 1.1.1
//  Fix: Remove duplicated CurrentRoom, FloorSet, Settings fields in CANBus and ScannerCOM 5 6
//      The duplicated fields may cause CurrentRoom not updated as correctly.

//  1.0.10.0
//  Improve ConnectionBase and ConnectionScannerBase class
//  Add: TcpRcu Diag App
//  Add: TcpRcuConnection and SetupForm
//  Fix: Time data index in TcpRcuDiag broadcast data   2
//  Fix: Time data in TcpRcuConnection  2
//  Add: Listen to FloorSet HotelUsingStatusChanged event and notify to RCU 3
//  Add: TCP/IP: No response SendRoomInformationRequest 4
//  Rev: Change the KeyStatus parser protocol in TCP/IP 4
//  Fix: TCP/IP bugs when receiving data with header(3) 5
//  Fix: TCP/IP bugs when parsering KeyStatusType   5
//  Add: DefaultRunningStatus into TCP/IP  5
//  Add: Remove dependences on VB RS232 Control
//  Fix: TCP/IP bugs when parsering Room from IPAddress 6
//  Add: TCP/IP Connect ATM when receiving instance data    7
//  Add: CANBus Timing0 and Timing1 in SetupForm    8
//  Fix: TCP/IP bugs when the network is disabled   9
//  Fix: TCP/IP TcpClient Multi-thread synchronization (Lock) 10



//  1.0.9.0
//  Change the TcpipReceiver to TcpipConversation and fix some related problem in TcpipServer
//  Add TcpipConnectionForm
//  Notify TCP/IP Client about Cleaning information 1
//  Add CAN Interface with Light signal 2
//  Add CAN Interface with WakeUp (postponed, waiting for new protocol)4
//  Refactor the TCP/IP Data Provider   5
//  Revise ScannerCOM protocol (version 1.0.2) to make consistence with CANBus 6
//  Use Custom Attribute to get the class info of connections   7
//  Add more Debug output for ScannerCOM    8
//  BUGFIX: Correct the Parity of th ScannerCOM   9

//  1.0.8.0
//  Port RS232 Component from the 3Party Component to .NET Framework 2.0 Component
//  Fix a small bugs on RS232 Connection

// 1.0.7.0
// Localization and Globalization

//1.0.6.0
// Support user-setup Multi Channel in CANBus
// Revise the TCP/IP Data Provider add VERSION cmd and change the GetRoom return format 3

//1.0.5.0
//Add changed DefaultRunningStatusOfAirCon On when HotelUsingStatus is Maintanence using CANBus 0
//Add changed DefaultRunningStatusOfAirCon On when HotelUsingStatus is Booked using CANBus 0
//Change DefaultRunningStatusOfAirCon is unchanged when HUS is Maintanence and Vancane 1


//1.0.4.0
//Change New Method to Query RS232 data information 0
//Identify the Repairer on CANBus Protocol 2
//Add changed DefaultRunningStatusOfAirCon On when HotelUsingStatus is Rented using CANBus 3
//Add changed DefaultRunningStatusOfAirCon Off when HotelUsingStatus is Empty using CANBus 5
//Add changed DefaultRunningStatusOfAirCon Off when HotelUsingStatus is CheckOut using CANBus 6

//1.0.3.0
//Upgrade the ConnectionManager Working Method
//Add KeyType Identity
//Add the support for TCPIP Client and Data Provider of Cleaner Key Status

//1.0.2.8
//Add change DefaultRunningStatusOfAirCon when a room put into vacant status using CANBus 

//(Deleted)
//1.0.2.7
//Fix a bug for sending season data to atm in CANBus(reverse the data)

//1.0.2.6
//Fix a bug for TCPIPClient Send Data when disconnect

//1.0.2.5
//Fix few bug

//1.0.2.4
//Add QuitRoom to TCPIPClient and TCPIPDataProvider

//1.0.2.3
//Add QuitRoom to Text Connection


//1.0.2.2
//Extent Disable Temperature Detect to Disable Temp , AirCon and Season for CANBus

//1.0.2.1
//Add AcceptInterval Future to CANBus

//1.0.2.0
//Add Disable Temperature Detect For CANBus Protocal (for so RCU don't detect it but send random number to PC)

//1.0.1.9
//When Season Info to ATM when Season Changed (CANBus Protocal)

//1.0.1.8
//Add Protocal for TCPIPClient and TCPIPDataProvider for instance moniter and new Room.Refrigerator and Room.Service.IsChecking
//Fix a bug for TCPIPDataProvider when Remote Shutdown Session

//1.0.1.7
//Add Room.Refrigerator and Room.Service.Checking Future

//1.0.1.6
//Fix CAN ResetSetting bug

//1.0.1.5
//Add Use All CAN Ind
//1.0.1.4
//Fix a bug for CANBus ScanDelay Setup Form
//Fix a bug for CANBus MultiChannel Setup Form

//1.0.1.3
//Use RS232.Rs232 Class to Instead of MSCommClass (COM Interop)

//1.0.1.2
//Add ScanDelay On CANBus SetupUp Form
//Fix a Bug For Airconditioner Running Status Protocol bug


//
// 要对程序集进行签名，必须指定要使用的密钥。有关程序集签名的更多信息，请参考 
// Microsoft .NET Framework 文档。
//
// 使用下面的属性控制用于签名的密钥。
//
// 注意:
//   (*) 如果未指定密钥，则程序集不会被签名。
//   (*) KeyName 是指已经安装在计算机上的
//      加密服务提供程序(CSP)中的密钥。KeyFile 是指包含
//       密钥的文件。
//   (*) 如果 KeyFile 和 KeyName 值都已指定，则 
//       发生下列处理:
//       (1) 如果在 CSP 中可以找到 KeyName，则使用该密钥。
//       (2) 如果 KeyName 不存在而 KeyFile 存在，则 
//           KeyFile 中的密钥安装到 CSP 中并且使用该密钥。
//   (*) 要创建 KeyFile，可以使用 sn.exe(强名称)实用工具。
//       在指定 KeyFile 时，KeyFile 的位置应该相对于
//       项目输出目录，即
//       %Project Directory%\obj\<configuration>。例如，如果 KeyFile 位于
//       该项目目录，应将 AssemblyKeyFile 
//       属性指定为 [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) “延迟签名”是一个高级选项 - 有关它的更多信息，请参阅 Microsoft .NET Framework
//       文档。
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
