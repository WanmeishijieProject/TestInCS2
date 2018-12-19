using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib
{
    public enum EnumPuseChangedType
    {
        HomePuseError,
        ToDesPuseError,
        CurPuseError,
    }
    public class PuseChangedArgs
    {
        public EnumPuseChangedType PuseType { get; set; }
        public Int16 CurrentValue { get; set; }
    }
}
