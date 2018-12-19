using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.Package
{
    public class PackageRecievedArgs : EventArgs
    {
        public EnumMvtemCmd Cmd { get; set; }
        public Int32 Value { get; set; }
        public byte[] RawData { get; set; }
    }
}
