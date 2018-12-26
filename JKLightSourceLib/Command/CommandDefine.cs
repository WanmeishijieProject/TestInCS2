using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Command
{
    public enum EnumCommand
    {
        OpenLight='1',
        CloseLight='2',
        GetLightValue='4',
        SetLightValue='3',
    }
    public enum EnumChannel
    {
        CH1='1',
        CH2,
        CH3,
        CH4,
    }

}
