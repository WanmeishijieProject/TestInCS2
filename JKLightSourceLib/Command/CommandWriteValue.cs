using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public class CommandWriteValue : CommandBase
    {
        public override EnumCommand Cmd =>EnumCommand.GetLightValue;
        public override EnumChannel Channel { get; set; }
    }
}
