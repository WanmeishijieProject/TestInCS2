using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdReadHomePuseError : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.ResPuseErrorToHome;
        public UInt16 QHomePuseError { get; private set; }
        protected override void WriteProfile()
        {
            throw new NotImplementedException();
        }
        public override MvtemCmdBase FromByteArray(byte[] RawData)
        {
            QHomePuseError = (UInt16)(RawData[2] * 256 + RawData[3]);
            SyncEvent.Set();
            return this;
        }
    }
}
