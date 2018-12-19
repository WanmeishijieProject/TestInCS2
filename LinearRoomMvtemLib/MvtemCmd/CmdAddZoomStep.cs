using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdAddZoomStep : MvtemCmdBase
    {
        protected override EnumMvtemCmd Cmd => EnumMvtemCmd.AddZoomStep;
        public double Step { get; set; }
       
        protected override void WriteProfile()
        {
            Int16 nStep = (Int16)(Step * 100);
            Bwr.Write(nStep);
        }
    }
}
