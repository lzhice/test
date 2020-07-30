using System;

namespace ATM3300.Connection
{
    /// <summary>
    /// Utility 的摘要说明。
    /// </summary>
    public class Utility
    {
        public Utility()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static int BCD(int value)
        {
            int r = 0, tempvalue = value, stepn = 1; ;
            do
            {
                r += (tempvalue % 10) * stepn;
                tempvalue = tempvalue / 10;
                stepn *= 16;
            } while (tempvalue != 0);
            return r;
        }

        public static uint BCD(uint value)
        {
            uint r = 0, tempvalue = value, stepn = 1; ;
            do
            {
                r += (tempvalue % 10) * stepn;
                tempvalue = tempvalue / 10;
                stepn *= 16;
            } while (tempvalue != 0);
            return r;
        }

        public static uint UNBCD(uint value)
        {
            uint r = 0, tempvalue = value, stepn = 1;
            do
            {
                r += (tempvalue % 16) * stepn;
                tempvalue = tempvalue / 16;
                stepn *= 10;
            } while (tempvalue != 0);
            return r;
        }

        public static bool IsCheckSumCorrect(byte[] CheckData)		//this checksum maybe diff from common checksum the sum is 256 differ
        {
            int value = 0;
            for (int i = 0; i <= CheckData.Length - 2; i++) value += CheckData[i];
            return ((byte)(value % 256) + CheckData[CheckData.Length - 1]) == 256;
        }

        public static byte MakeCheckSumValue(byte[] CheckData)
        {
            int value = 0;
            for (int i = 0; i <= CheckData.Length - 1; i++) value += CheckData[i];
            return (byte)(256 - value % 256);
        }

        public static bool ArrayDataEquals(byte[] e1, byte[] e2)
        {
            if (e1.Length != e2.Length) return false;

            for (int i = 0; i <= e1.Length - 1; i++) if (e1[i] != e2[i]) return false;

            return true;
        }

    }
}
