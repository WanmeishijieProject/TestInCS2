using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPAP.Model.AbbCmd
{
    public class MsgRotate : RobotCmdBase
    {    
        /// <summary>
        /// 绕X轴旋转的角度
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 绕Y轴旋转的角度
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 绕Z轴旋转的角度
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 输入时填的参数
        /// </summary>
        /// <returns></returns>
        protected override void SetProfile()
        {
            I_Cmd = Definetions.EnumRobotCmd.Rotate;
            Paras[0] = X.ToString();
            Paras[1] = Y.ToString();
            Paras[2] = Z.ToString();
            SetSpeedAndTool(I_Speed, I_Tool);
        }

    
        public override object GenEmptyCmd()
        {
            return new MsgRotate() { I_Cmd = this.I_Cmd };
        }

        /// <summary>
        /// 从结果中解析
        /// </summary>
        protected override void ReadProfile()
        {

        }
    }
}
