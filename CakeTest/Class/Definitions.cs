using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeTest.Class
{
    public enum EnumCamType
    {
        GigEVision,
        DirectShow,
        uEye,
        HuaRay
    }

    public enum EnumSignalType
    {
        RisingEdge,
        FailingEdge,
    }
    public enum EnumOutputLogic
    {
        False,
        True, 
    }
    public enum EnumUseOutput
    {
        Use,
        NotUse,
    }

    public enum EnumFontSize
    {
       Small=50,
       Normal=70,
       Big=90,
    }

    public enum EnumImageType
    {
        image,
        window,
    }

}
