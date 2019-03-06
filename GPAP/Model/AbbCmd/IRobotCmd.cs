using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GPAP.Definetions;

namespace GPAP.Model.AbbCmd
{
    public interface IRobotCmd
    {
        EnumRobotCmd I_Cmd { get; set; }
        object GenEmptyCmd();
    }
}
