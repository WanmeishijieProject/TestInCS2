using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public class CommandWriteValue : CommandBase
    {
        public override EnumCommand Cmd =>EnumCommand.SetLightValue;
        public override EnumChannel Channel { get; set; }
        public override UInt16 Value { set; get; }

        public override int ExpectResultLength => 1;
    }
}
