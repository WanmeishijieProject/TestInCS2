using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPAP.TcpClient
{
    public class RecvMessageInfo : IPackageInfo
    {
        public string Cmd { get; set; }

    }
}
