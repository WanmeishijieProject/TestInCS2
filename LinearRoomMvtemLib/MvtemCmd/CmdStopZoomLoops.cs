using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdStopZoomLoops : MvtemCmdBase
    {
        protected override CmdDefinen.EnumMvtemCmd Cmd => CmdDefinen.EnumMvtemCmd.StopZoomLoops;

        protected override void WriteProfile()
        {
            Bwr.Write(EmptyValue);
        }
    }
}
