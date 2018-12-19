using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib
{
    public enum EnumState
    {
        CurrentState,
        MotionState,
        TemState,
    }
    public class MotionStateChangedArgs
    {
        public EnumState StateType { get; set; }
        public object State { get; set; }
    }
}
