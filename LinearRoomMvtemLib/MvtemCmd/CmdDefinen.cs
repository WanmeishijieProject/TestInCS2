using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRoomMvtemLib.MvtemCmd
{
    public class CmdDefinen
    {
        public enum EnumMvtemCmd
        {
            ZoomRandom=0x11,
            SubZoomStep,
            AddZoomStep,
            ZoomLoops,
            StopZoomLoops,
            Home=0xE5,
            ReadCurZoomRate=0x21,       //读取当前倍率
            ReadCurPuseError,           //读取当前脉冲误差
            ReadCurCurrentState,        //读取当前电流状态
            ReadCurTempState,           //读取当前温度状态
            ReadCurMotionState,         //读取当前点击状态

            //接收
            ResPuseErrorToHome=0x51,    //回零误差
            ResPuseErrorToDes,          //当前倍率误差
            ResCurZoomRate,             //当前倍率
            ResPuseError,               //误差
            ResCurrentState,            //返回当前电机状态
            ResTemState,                //返回当前温度状态
            ResMotionState,             //返回当前电机状态
        }

        public enum EnumCurrentState
        {
            Normal = 0x00,
            Error = 0xE1,
        }
        public enum EnumTemState
        {
            Normal = 0x00,
            Error = 0xE1,
        }
        public enum EnumMotionState
        {
            Normal = 0x00,
            Stack = 0xE1,
            Stop = 0xE2,
        }
    }
}
