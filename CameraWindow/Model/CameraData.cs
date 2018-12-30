using CameraWindow.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Model
{
    public class CameraData
    {
        /// <summary>
        /// 用户自定义名称，用于模糊查找
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 视觉用的名称
        /// </summary>
        public string NameForVision { get; set; }

        /// <summary>
        /// 相机类型
        /// </summary>
        public EnumCamType CameType { get; set; }
    }
}
