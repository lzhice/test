using System;
using System.Collections.Generic;
using System.Text;

namespace ATM3300.Connection
{
    [AttributeUsage(AttributeTargets.Class , Inherited = false)]
    public class ConnectionVersionInfoAttribute : Attribute
    {
        private ClassInfo _ClassInfo;
        private Type _ShowSetupForm;

        public Type ShowSetupForm
        {
            get { return _ShowSetupForm; }
        }

        public ClassInfo ClassInfo
        {
            get { return _ClassInfo; }
        }

        public ConnectionVersionInfoAttribute(string name , string guid , string version , string comment  , Type showSetupForm)
        {
            _ClassInfo = new ClassInfo(name, guid, new Version(version), comment);
            _ShowSetupForm = showSetupForm;
        }
    }
}
