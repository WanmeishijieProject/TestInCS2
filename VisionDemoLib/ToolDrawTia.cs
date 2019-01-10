using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDemoLib.ToolParaConfig.ToolDrawTiaManager;

namespace VisionDemoLib
{
    public class ToolDrawTia : ToolVisionBase
    {
        public ToolDrawTia() { }
        public ToolDrawTia(HObject Image, string ParaFile)
        {
            In_Image = Image;
            In_ParaFile = ParaFile;
        }
        private TiaRoiPara RoiPara;
        public HObject In_Image { get; set; }
        public string In_ParaFile { get; set; }

        public HObject Out_Region { get; set; }
        public HObject Out_Roi { get; set; }

        protected override void Process()
        {
            HOperatorSet.GenEmptyObj(out HObject RegionOut);
            // Local iconic variables 
            HObject ho_Image1, ho_Rectangle, ho_ImageReduced;
            HObject ho_Regions, ho_ConnectedRegions, ho_SelectedRegions;
            HObject ho_SelectedRegions1, ho_SortedRegions, ho_RegionTrans;
            HObject ho_Rectangle1 = null;


            // Local control variables 
            HTuple hv_Area, hv_Row, hv_Column;
            HTuple hv_Phi, hv_Row3, hv_Column3, hv_Phi1, hv_Length1;
            HTuple hv_Length2, hv_PhiSum, hv_Index1, hv_L2Mean, hv_L1Mean;
            HTuple hv_PhiMean, hv_DeltaRow, hv_DeltaCol, hv_NewRow;
            HTuple hv_NewCol, hv_Index;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);

            ho_Rectangle.Dispose();
            var data = RoiPara.RoiData;
            HOperatorSet.GenRectangle1(out ho_Rectangle, data.RowLT, data.ColLT, data.RowRB, data.ColRB);
            Out_Roi = ho_Rectangle.SelectObj(1);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(In_Image, ho_Rectangle, out ho_ImageReduced);
            HOperatorSet.ScaleImageMax(ho_ImageReduced, out HObject ImageScaleMax);
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ImageScaleMax, out ho_Regions, 0, 40);
            ImageScaleMax.Dispose();
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", 2201.39, 4500);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions1, "height", "and", 20, 79.94);
            if (ho_SelectedRegions1.CountObj() != 3)
            {
                throw new Exception($"没有发现足够的区域完成计算,当前区域个数为{ho_SelectedRegions1.CountObj()}");
            }
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_SelectedRegions1, out ho_SortedRegions, "character", "true", "row");

            ho_RegionTrans.Dispose();
            HOperatorSet.ShapeTrans(ho_SelectedRegions1, out ho_RegionTrans, "rectangle2");
            HOperatorSet.AreaCenter(ho_RegionTrans, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.OrientationRegion(ho_RegionTrans, out hv_Phi);
            HOperatorSet.SmallestRectangle2(ho_RegionTrans, out hv_Row3, out hv_Column3,
                out hv_Phi1, out hv_Length1, out hv_Length2);
            hv_PhiSum = 0;
            for (hv_Index1 = 1; (int)hv_Index1 <= (int)(new HTuple(hv_Phi1.TupleLength())); hv_Index1 = (int)hv_Index1 + 1)
            {
                if ((int)(new HTuple(((hv_Phi1.TupleSelect(hv_Index1 - 1))).TupleLess(0))) != 0)
                {
                    hv_Phi1[hv_Index1 - 1] = (hv_Phi1.TupleSelect(hv_Index1 - 1)) + ((new HTuple(90)).TupleRad());
                }
                hv_PhiSum = hv_PhiSum + (hv_Phi1.TupleSelect(hv_Index1 - 1));
            }

            HOperatorSet.TupleMean(hv_Length2, out hv_L2Mean);
            HOperatorSet.TupleMean(hv_Length1, out hv_L1Mean);
            hv_PhiMean = hv_PhiSum / (new HTuple(hv_Phi1.TupleLength()));

            hv_DeltaRow = ((hv_Row.TupleSelect(2)) - (hv_Row.TupleSelect(0))) / 4;
            hv_DeltaCol = ((hv_Column.TupleSelect(2)) - (hv_Column.TupleSelect(0))) / 4;

            hv_NewRow = (hv_Row.TupleSelect(2)) + hv_DeltaRow;
            hv_NewCol = (hv_Column.TupleSelect(2)) + hv_DeltaCol;
            for (hv_Index = 1; (int)hv_Index <= 4; hv_Index = (int)hv_Index + 1)
            {
                ho_Rectangle1.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_NewRow - ((2 * (hv_Index - 1)) * hv_DeltaRow),
                    hv_NewCol - ((2 * (hv_Index - 1)) * hv_DeltaCol) + 3, hv_PhiMean, hv_L1Mean - 60, 1000);
                HOperatorSet.Union2(RegionOut, ho_Rectangle1, out RegionOut);
            }
            Out_Region = RegionOut.SelectObj(1);
            RegionOut.Dispose();
        }

        protected override void ParseParaFromFile()
        {
            if (string.IsNullOrEmpty(In_ParaFile))
                throw new Exception($"参数文件不能为空");
            try
            {
                var JasonString = File.ReadAllText(In_ParaFile);
                RoiPara = JsonConvert.DeserializeObject<TiaRoiPara>(JasonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"解析{GetType().ToString()}文件{In_ParaFile}出错:{ex.Message}");
            }
        }
    }
}
