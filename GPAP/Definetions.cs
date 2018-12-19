using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPAP
{
    public class Definetions
    {
        public enum EnumRobotCmd
        {
            Init=1,
            Calibration,
            MoveToPos,
            Rotate,
            StopRobot,
            Test,

            NONE=999,
        }
        public enum EnumRobotSpeed
        {
            V5=1,
            V10,
            V20,
            V30,
            V40,
            V50,
            V60,
            V80,
            V100,
            V150,
            V200,
            V300,
            V400,
            V500,
            V600,
            V800,
            V1000,
            V1500,
            V2000,
        }
        public enum EnumRobotTool
        {
            Tool0=1,
            Tool1,
            Tool2,
            Tool3,
            Tool4,
            Tool5,
        }
    }
}
