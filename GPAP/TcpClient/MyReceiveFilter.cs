using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ProtoBase;

namespace GPAP.TcpClient
{
    public class MyReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
    {
        //通知
        public EventHandler<string> OnPackageReceived;
        public MyReceiveFilter() : base(Encoding.ASCII.GetBytes("]")) // package terminator
        {
        }
        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            long len = bufferStream.Length;
            string StrRecv = bufferStream.ReadString((int)len, Encoding.ASCII);
            OnPackageReceived?.Invoke(this,StrRecv);
            bufferStream.Clear();
            return null;
        }
       
    }
}
