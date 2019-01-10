using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionLib.DataModel;
using static VisionLib.VisionDefinitions;

namespace VisionDemoLib.ToolParaConfig.ToolFindLineParaManager
{
    public class LinePara
    {
        public string ParaName { get; set; }
        public EnumLinePolarityType Polarity { get; set; }
        public double Contrast { get; set; }
        public UInt16 CaliperNumber { get; set; }
        public EnumSelectType SelectType { get; set; }
        public VisionRectangle2Data RoiData { get; set; }
    }
}
