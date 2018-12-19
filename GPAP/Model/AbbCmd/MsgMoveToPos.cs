using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPAP.Model.AbbCmd
{
    public class MsgMoveToPos : RobotCmdBase
    {
        public double I_X { get; set; }
        public double I_Y { get; set; }
        public double I_Z { get; set; }
        protected override void SetProfile()
        {
            I_Cmd=Definetions.EnumRobotCmd.MoveToPos;
            Paras[0] = I_X.ToString();
            Paras[1] = I_Y.ToString();
            Paras[2] = I_Z.ToString();
            Paras[3] = I_Speed.ToString();
        }
        public override object GenEmptyCmd()
        {
            return new MsgMoveToPos() { I_Cmd = this.I_Cmd };
        }
        protected override void ReadProfile()
        {
            
        }
    }
}
