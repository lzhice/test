using System;

namespace ATM3300.Connection
{
    /// <summary>
    /// IClassInfo 的摘要说明。
    /// </summary>
    public interface IClassInfo
    {
        string Name { get;}
        string GUID { get;}
        Version Version { get;}
        string Comment { get;}
    }

    public class ClassInfo : IClassInfo
    {
        private string mName;
        private string mGUID;
        private Version mVersion;
        private string mComment;
        public ClassInfo(string Name, string GUID, Version Version, string Comment)
        {
            mName = Name;
            mGUID = GUID;
            mVersion = Version;
            mComment = Comment;
        }

        #region IClassInfo 成员

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public string GUID
        {
            get
            {
                return mGUID;
            }
        }

        public Version Version
        {
            get
            {
                return mVersion;
            }
        }

        public string Comment
        {
            get
            {
                return mComment;
            }
        }

        #endregion

        public static bool operator ==(ClassInfo Info1, ClassInfo Info2)
        {
            if ((Info1 == null) || (Info2 == null))
            {
                if ((Info1 == null) && (Info2 == null))
                    return true;
                else
                    return false;
            }
            else
            {
                return Info1.Equals(Info2);
            }
        }

        public static bool operator !=(ClassInfo Info1, ClassInfo Info2)
        {
            return !(Info1 == Info2);
        }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is ClassInfo))
            {
                return ((ClassInfo)obj).GUID == GUID;
            }
            return false;
        }



    }
}
