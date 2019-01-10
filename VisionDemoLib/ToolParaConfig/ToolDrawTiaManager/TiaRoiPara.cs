using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionLib.DataModel;

namespace VisionDemoLib.ToolParaConfig.ToolDrawTiaManager
{
    public class TiaRoiPara
    {
        public string ParaName { get; set; }
        public VisionRectangle1Data RoiData { get; set; }
    }
}
