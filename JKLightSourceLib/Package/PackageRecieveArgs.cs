using JKLightSourceLib.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Package
{
    public class PackageRecieveArgs
    {
        public EnumCommand Cmd { get; set; }
        public byte[] RawData { get; set; }
    }
}
