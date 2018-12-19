using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdSubZoomStep : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.SubZoomStep;
        public double Step { get; set; }
        protected override void WriteProfile()
        {
            Int16 nStep=(Int16)(Step*100);
            Bwr.Write((byte)((nStep >> 8) & 0xFF));
            Bwr.Write((byte)(nStep & 0xFF));
        }
    }
}
