using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GPAP.Definetions;

namespace GPAP.Model.AbbCmd
{
    public class MsgInit : RobotCmdBase
    {
        public MsgInit()
        {
            I_Cmd = EnumRobotCmd.Init;
        }   

        protected override void SetProfile()
        {
            
        }
        public override object GenEmptyCmd()
        {
            return new MsgInit() { I_Cmd = this.I_Cmd };
        }
        protected override void ReadProfile()
        {

        }
    }
}
