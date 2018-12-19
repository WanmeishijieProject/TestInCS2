using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdReadCurZoomRate : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.ReadCurZoomRate;
        public double QCurZoomRate
        {
            get;
            private set;
        }
        protected override void WriteProfile()
        {
            Bwr.Write(EmptyValue);
        }
        public override MvtemCmdBase FromByteArray(byte[] RawData)
        {
            QCurZoomRate = (double)(RawData[2]*256 + RawData[3])/100.0;
            SyncEvent.Set();
            return this;
        }
    }
}
