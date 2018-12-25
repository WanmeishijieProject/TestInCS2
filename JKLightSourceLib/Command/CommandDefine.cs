using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public enum EnumCommand
    {
        OpenLight=0x01,
        CloseLight=0x02,
        GetLightValue=0x04,
        SetLightValue=0x03,
    }
    public enum EnumChannel
    {
        CH1=1,
        CH2,
        CH3,
        CH4,
    }

}
