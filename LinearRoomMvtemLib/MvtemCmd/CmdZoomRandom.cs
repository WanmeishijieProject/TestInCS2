using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdZoomRandom : MvtemCmdBase
    {
        protected override EnumMvtemCmd Cmd=> EnumMvtemCmd.ZoomRandom;
        public double ZoomRate { get; set; }
        protected override void WriteProfile()
        {
            Int16 nZoomRate = (Int16)(ZoomRate * 100);
            Bwr.Write((byte)((nZoomRate >> 8) & 0xFF));
            Bwr.Write((byte)(nZoomRate & 0xFF));
        }
        
    }
}
