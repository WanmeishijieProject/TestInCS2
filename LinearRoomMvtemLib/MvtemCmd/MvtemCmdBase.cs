using LinearRoomMvtemLib.Package;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public abstract class MvtemCmdBase
    {
        protected BinaryWriter Bwr;
        protected abstract void WriteProfile();
        protected abstract EnumMvtemCmd Cmd { get;}
        protected Int16 EmptyValue = 0;
        protected AutoResetEvent SyncEvent = new AutoResetEvent(false);

        public byte[] ToByteArray()
        {
            using (MemoryStream MemStream = new MemoryStream())
            {
                Bwr=new BinaryWriter(MemStream);
                SyncEvent.Reset();
                Bwr.Write((byte)EnumPackageDefine.HEAD);
                Bwr.Write((byte)Cmd);
                WriteProfile();
                Bwr.Write((byte)EnumPackageDefine.TAIL);
                return MemStream.ToArray();
            }
        }
        public virtual MvtemCmdBase FromByteArray(byte[] RawData)
        {
            throw new NotImplementedException();
        }
        public void WaitResult(int TimeOut)
        {
            if (!SyncEvent.WaitOne(TimeOut))
                throw new Exception($"Time out for waiting result of cmd: {Cmd.ToString()}");
        }
        ~MvtemCmdBase()
        {
            if(Bwr!=null)
                Bwr.Close();
        }
    }
}
