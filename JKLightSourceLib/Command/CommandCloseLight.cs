using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public class CommandCloseLight : CommandBase
    {
        public override EnumChannel Channel { get; set; }
        public override EnumCommand Cmd => EnumCommand.CloseLight;
    }
}
