using System;
using ATM3300.Common;
using System.Xml;
namespace ATM3300.Connection
{

	public class ConnectionTriggerFileXml : ConnectionBase
	{
		protected FloorSet mFloorSet;
		protected string mXmlFileName;
		protected System.Xml.XmlTextReader mXmlReader;

		
		public override ClassInfo ClassInfo()
		{
			return new ClassInfo("Connection.Trigger.File.XML",
				"{21362665-77B1-4086-AC2B-4100990F497F}",
				new Version("0.0.1"),
				"This Connection Use File Watcher and XML to run.");
		}

		public ConnectionTriggerFileXml(FloorSet aFloorSet,SettingsBase aSetting):base(aFloorSet,aSetting)
		{
			//获取初始化数据
			mFloorSet=aFloorSet;
			mXmlFileName=(string)aSetting["XmlFileName"];
	
			mXmlReader=new XmlTextReader(mXmlFileName);
			mXmlReader.MoveToFirstAttribute();
			

		}
	}
}
