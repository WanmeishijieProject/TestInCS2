using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdReadCurTemState : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.ReadCurTempState;
        public EnumTemState QTemState { get; private set; }
        protected override void WriteProfile()
        {
            Bwr.Write(EmptyValue);
        }
        public override MvtemCmdBase FromByteArray(byte[] RawData)
        {
            if (Enum.IsDefined(typeof(EnumTemState), (int)RawData[3]))
            {
                QTemState = (EnumTemState)RawData[3];
                SyncEvent.Set();
                return this;
            }
            else
                throw new Exception("Wrong state of tempture state!");
        }
    }
}
