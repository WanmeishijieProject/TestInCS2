using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public class CommandReadValue : CommandBase
    {
        public override EnumCommand Cmd => EnumCommand.GetLightValue;
        public override EnumChannel Channel { get; set; }
        public UInt16 QChannelValue { get; set; }
        public override void FromByteArray(byte[] RawData)
        {
            string strValue = Encoding.ASCII.GetString(new byte[] { RawData[3], RawData[4], RawData[5] });
            NumberFormatInfo ni = new NumberFormatInfo();
            strValue = "0x" + strValue;
            QChannelValue = Convert.ToUInt16(strValue, 16);
        }
    }
}
