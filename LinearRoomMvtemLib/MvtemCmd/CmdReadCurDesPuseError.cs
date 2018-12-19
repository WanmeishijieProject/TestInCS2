using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdReadCurDesPuseError : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.ResPuseErrorToDes;

        /// <summary>
        /// 当前倍率剩余脉冲数
        /// </summary>
        public UInt16 QCurDesPuseError { get; private set; }
        protected override void WriteProfile()
        {
            Bwr.Write(EmptyValue);
        }
        public override MvtemCmdBase FromByteArray(byte[] RawData)
        {
            QCurDesPuseError = (UInt16)(RawData[2] * 256 + RawData[3]);
            SyncEvent.Set();
            return this;
        }
    }
}
