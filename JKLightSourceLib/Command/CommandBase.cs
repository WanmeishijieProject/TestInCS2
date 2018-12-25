using JKLightSourceLib.Check;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
   
    public abstract class CommandBase
    {
        private const int DEFAULT_TIMEOUT = 1000;
        private byte HEADER = (byte)'#';
        protected AutoResetEvent SyncEvent = new AutoResetEvent(false);
        public abstract EnumCommand Cmd { get; }
        public abstract EnumChannel Channel { get; set; }
        public abstract int ExpectResultLength { get; }
        public virtual UInt16 Value
        {
            get { return 0; }
            set { }
        }

        protected BinaryWriter Writer;
        public byte[] ToByteArray()
        {
            using (var MemoryStream = new MemoryStream())
            {
                Writer = new BinaryWriter(MemoryStream);
                Writer.Write(HEADER);
                Writer.Write((byte)Cmd);
                var data = Uint16ToString(Value);
                Writer.Write(data[0]);
                Writer.Write(data[1]);
                Writer.Write(data[2]);
                var Xor = CheckXor.GetStringXOR(MemoryStream.ToArray());
                Writer.Write((byte)Xor[0]);
                Writer.Write((byte)Xor[1]);
                return MemoryStream.ToArray();
            }
        }
        public virtual void FromByteArray(byte[] RawData)
        {

        }

        private string Uint16ToString(UInt16 Value)
        {
            return string.Format("{0:X3}",Value);
        }

        
    }
}
