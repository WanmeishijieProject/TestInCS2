using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Check
{
    public class CheckXor
    {
        public static string GetStringXOR(Byte[] data, int nStartPos = 0, int Length = -1)
        {
            int len = data.Length;
            if (len < 0)
                return "";
            if (Length != -1 && Length < len)
                len = Length;
            int nSum = data[0];
            for (int i = 1; i < len; i++)
                nSum ^= data[i];
            return string.Format("{0:X2}", nSum);
        }
        public static UInt16 GetWordXOR(Byte[] data, int nStartPos = 0, int Length = -1)
        {
            int len = data.Length;
            if (len < 0)
               throw new Exception("Wrong length of data");
            if (Length != -1 && Length < len)
                len = Length;
            UInt16 nSum = data[0];
            for (int i = 1; i < len; i++)
                nSum ^= data[i];
            return nSum;
        }

    }
}
