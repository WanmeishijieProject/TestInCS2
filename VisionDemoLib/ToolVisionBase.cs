using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionLib;

namespace VisionDemoLib
{
    public abstract class ToolVisionBase
    {
        public void Run()
        {
            ParseParaFromFile();
            Process();
        }
        protected abstract void ParseParaFromFile();
        protected abstract void Process();
        protected HalconVision VisionCommon = HalconVision.Instance;
    }
}
