using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionLib;
using VisionLib.DataModel;
using Newtonsoft;
using VisionDemoLib.ToolParaConfig.ToolFindLineParaManager;
using Newtonsoft.Json;

namespace VisionDemoLib
{
    public class ToolFindLine : ToolVisionBase
    {
        public ToolFindLine()
        {

        }
        public ToolFindLine(string ParaFile, HObject Image)
        {
            In_ParaFile = ParaFile;
            In_Image = Image;
        }

        private LineParaManager ParaManager = null;

        public string In_ParaFile { get; set; }
        public HObject In_Image { get; set; }
        

        /// <summary>
        /// 根据参数文件生成的Region
        /// </summary>
        public List<HObject> Out_RegionList { get; private set; }
        /// <summary>
        /// 图片处理以后得到的线
        /// </summary>
        public List<VisionLineData> Out_LineList { get; private set; }

        protected override void Process()
        {
            Out_LineList = new List<VisionLineData>();
            Out_RegionList = new List<HObject>();
            if (ParaManager != null)
            {
                foreach (var linePara in ParaManager.LineParas)
                {
                    var roi = linePara.RoiData;
                    VisionCommon.FindLine(In_Image, linePara.Polarity, linePara.SelectType, linePara.CaliperNumber, linePara.Contrast, roi.RowCenter,
                                            roi.ColCenter, roi.Phi, roi.L1, roi.L2, out HTuple RowStart, out HTuple ColStart, out HTuple RowEnd, out HTuple ColEnd);
                    HOperatorSet.GenRectangle2(out HObject RoiRect2, roi.RowCenter,roi.ColCenter, roi.Phi, roi.L1, roi.L2);
                    Out_RegionList.Add(RoiRect2);
                    if (RowStart.Length != 0)
                    {
                        Out_LineList.Add(new VisionLineData(RowStart, ColStart, RowEnd, ColEnd));
                    }
                }
            }
        }

        protected override void ParseParaFromFile()
        {
            if (string.IsNullOrEmpty(In_ParaFile))
                throw new Exception($"参数文件不能为空");
            try
            {
                var JasonString = File.ReadAllText(In_ParaFile);
                ParaManager = JsonConvert.DeserializeObject<LineParaManager>(JasonString);
            }
            catch(Exception ex)
            {
                throw new Exception($"解析{GetType().ToString()}文件{In_ParaFile}出错:{ex.Message}");
            }
        }
    }
}
