using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdZoomLoops : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.ZoomLoops;
        public Int16 LoopCount { get; set; }
        protected override void WriteProfile()
        {
            Bwr.Write((byte)((LoopCount >> 8) & 0xFF));
            Bwr.Write((byte)(LoopCount & 0xFF));
        }
    }
}
