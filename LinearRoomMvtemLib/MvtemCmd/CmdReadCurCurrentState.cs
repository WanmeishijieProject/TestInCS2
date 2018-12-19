using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdReadCurCurrentState : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.ReadCurCurrentState;
        public EnumCurrentState QCurCurrentState { get; private set; }
        protected override void WriteProfile()
        {
            Bwr.Write(EmptyValue);
        }
        public override MvtemCmdBase FromByteArray(byte[] RawData)
        {
            if (Enum.IsDefined(typeof(EnumCurrentState), (int)RawData[3]))
            {
                this.QCurCurrentState = (EnumCurrentState)RawData[3];
                SyncEvent.Set();
                return this;
            }
            else
                throw new Exception("Wrong state found when parse state of current!");
        }
    }
}
