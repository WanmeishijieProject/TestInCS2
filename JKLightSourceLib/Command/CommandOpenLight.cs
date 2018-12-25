using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public class CommandOpenLight : CommandBase
    {
        public override EnumCommand Cmd => EnumCommand.OpenLight;
        public override int ExpectResultLength => 1;
        public override EnumChannel Channel { get; set; }
        public override UInt16 Value { get; set; }
    }
}
