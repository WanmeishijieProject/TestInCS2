﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeTest.Model;
using HalconDotNet;
namespace CakeTest.Class
{
   

    public class HalconFunc
    {
        private HTuple hv_AcqHandle;
        private HObject ho_Image;
        HTuple hv_MLPHandle, hv_MLPHandle1;
        ParaModel ParaSetting;
        private void dev_open_window_fit_image(HObject ho_Image, HTuple hv_Row, HTuple hv_Column,
        
        HTuple hv_WidthLimit, HTuple hv_HeightLimit, out HTuple hv_WindowHandle)
        {



            // Local control variables 

            HTuple hv_MinWidth = new HTuple(), hv_MaxWidth = new HTuple();
            HTuple hv_MinHeight = new HTuple(), hv_MaxHeight = new HTuple();
            HTuple hv_ResizeFactor, hv_ImageWidth, hv_ImageHeight;
            HTuple hv_TempWidth, hv_TempHeight, hv_WindowWidth, hv_WindowHeight;

            // Initialize local and output iconic variables 

            //This procedure opens a new graphics window and adjusts the size
            //such that it fits into the limits specified by WidthLimit
            //and HeightLimit, but also maintains the correct image aspect ratio.
            //
            //If it is impossible to match the minimum and maximum extent requirements
            //at the same time (f.e. if the image is very long but narrow),
            //the maximum value gets a higher priority,
            //
            //Parse input tuple WidthLimit
            if ((int)((new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(0))).TupleOr(
                new HTuple(hv_WidthLimit.TupleLess(0)))) != 0)
            {
                hv_MinWidth = 500;
                hv_MaxWidth = 800;
            }
            else if ((int)(new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
                1))) != 0)
            {
                hv_MinWidth = 0;
                hv_MaxWidth = hv_WidthLimit.Clone();
            }
            else
            {
                hv_MinWidth = hv_WidthLimit[0];
                hv_MaxWidth = hv_WidthLimit[1];
            }
            //Parse input tuple HeightLimit
            if ((int)((new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(0))).TupleOr(
                new HTuple(hv_HeightLimit.TupleLess(0)))) != 0)
            {
                hv_MinHeight = 400;
                hv_MaxHeight = 600;
            }
            else if ((int)(new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
                1))) != 0)
            {
                hv_MinHeight = 0;
                hv_MaxHeight = hv_HeightLimit.Clone();
            }
            else
            {
                hv_MinHeight = hv_HeightLimit[0];
                hv_MaxHeight = hv_HeightLimit[1];
            }
            //
            //Test, if window size has to be changed.
            hv_ResizeFactor = 1;
            HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
            //First, expand window to the minimum extents (if necessary).
            if ((int)((new HTuple(hv_MinWidth.TupleGreater(hv_ImageWidth))).TupleOr(new HTuple(hv_MinHeight.TupleGreater(
                hv_ImageHeight)))) != 0)
            {
                hv_ResizeFactor = (((((hv_MinWidth.TupleReal()) / hv_ImageWidth)).TupleConcat(
                    (hv_MinHeight.TupleReal()) / hv_ImageHeight))).TupleMax();
            }
            hv_TempWidth = hv_ImageWidth * hv_ResizeFactor;
            hv_TempHeight = hv_ImageHeight * hv_ResizeFactor;
            //Then, shrink window to maximum extents (if necessary).
            if ((int)((new HTuple(hv_MaxWidth.TupleLess(hv_TempWidth))).TupleOr(new HTuple(hv_MaxHeight.TupleLess(
                hv_TempHeight)))) != 0)
            {
                hv_ResizeFactor = hv_ResizeFactor * ((((((hv_MaxWidth.TupleReal()) / hv_TempWidth)).TupleConcat(
                    (hv_MaxHeight.TupleReal()) / hv_TempHeight))).TupleMin());
            }
            hv_WindowWidth = hv_ImageWidth * hv_ResizeFactor;
            hv_WindowHeight = hv_ImageHeight * hv_ResizeFactor;
            //Resize window
            HOperatorSet.SetWindowAttr("background_color", "black");
            HOperatorSet.OpenWindow(hv_Row, hv_Column, hv_WindowWidth, hv_WindowHeight, 0, "", "", out hv_WindowHandle);
            HDevWindowStack.Push(hv_WindowHandle);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.SetPart(HDevWindowStack.GetActive(), 0, 0, hv_ImageHeight - 1, hv_ImageWidth - 1);
            }

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS
        private void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {


            // Local control variables 

            HTuple hv_OS, hv_Exception = new HTuple();
            HTuple hv_AllowedFontSizes = new HTuple(), hv_Distances = new HTuple();
            HTuple hv_Indices = new HTuple();

            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables 

            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple((((hv_OS.TupleStrFirstN(2)).TupleStrLastN(0))).TupleEqual(
                "Win"))) != 0)
            {
                //set font on Windows systems
                if ((int)((new HTuple((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    throw new HalconException(hv_Exception);
                }
            }
            else
            {
                //set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    throw new HalconException(hv_Exception);
                }
            }

            return;
        }

        public List<CameraInfoModel> FindCamera(EnumCamType camType, out List<string> ErrorList)
        {
            ErrorList = new List<string>();
            var CamInfoList = new List<CameraInfoModel>();
#if TEST
            CamInfoList.Add(new CameraInfoModel() {
                ActualName = "DirectShow",
                NameForVision = "Integrated Camera",
                Type = EnumCamType.DirectShow,
                CamID = 0
            });
            return CamInfoList;
#endif
            try
            {
                HOperatorSet.InfoFramegrabber(camType.ToString(), "info_boards", out HTuple hv_Information, out HTuple hv_ValueList);
                if (0 == hv_ValueList.Length)
                    return CamInfoList;
                    int i = 0;
                    foreach (var dev in hv_ValueList.SArr)
                    {
                        var listAttr = dev.Split('|').Where(a => a.Contains("device:"));
                        if (listAttr != null && listAttr.Count() > 0)
                        {
                            string Name = listAttr.First().Trim().Replace("device:", "");
                            CamInfoList.Add(new CameraInfoModel()
                            {
                                ActualName = Name.Trim(),
                                NameForVision = Name.Trim(),
                                Type = camType,
                                CamID = i++,
                            });
                        }
                    }
             
                return CamInfoList;
            }
            catch (Exception ex)
            {
                ErrorList.Add($"FIndCamera error:{ex.Message}");
                return CamInfoList;
            }

        }

        public void OpenCamera(HTuple CamName)
        {
            HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "progressive",
                        -1, "default", -1, "false", "default", CamName, 0, -1, out hv_AcqHandle);
            HOperatorSet.ReadClassMlp("mlp_1.gmc", out hv_MLPHandle);
            HOperatorSet.ReadClassMlp("mlp_2.gmc", out hv_MLPHandle1);
        }

        public void SetCameraPara(ParaModel Para)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSelector", "FrameStart");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", 400000);
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "AcquisitionMode", "SingleFrame");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line1");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerActivation", Para.TriggerType.ToString());
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureMode", "Timed");
            ParaSetting = Para;
        }


        public void CloseCamera()
        {

            HOperatorSet.CloseAllFramegrabbers();
        }

        public bool TestCake(HWindow WindowHandle)
        {
            // Local iconic variables 
            var StartTime = DateTime.Now.Ticks;
            HObject ho_Image1, ho_Image2, ho_Image3;
            HObject ho_ImageResult1, ho_ImageResult2, ho_ImageResult3;
            HObject ho_ClassRegionsNotRejected, ho_ObjectSelected, ho_RegionOpening2;
            HObject ho_ConnectedRegions, ho_SelectedRegions, ho_RegionUnion;
            HObject ho_RegionOpening, ho_RegionTrans, ho_ImageReduced;
            HObject ho_ObjectSelected1, ho_RegionOpening1, ho_RegionFillUp;
            HObject ho_RegionClosing, ho_ConnectedRegions1, ho_SelectedRegions1;
            HObject ho_Rectangle;

            // Local control variables 
            
            HTuple hv_Row1, hv_Column1, hv_Phi, hv_Length1, hv_Length2;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Image2);
            HOperatorSet.GenEmptyObj(out ho_Image3);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult2);
            HOperatorSet.GenEmptyObj(out ho_ImageResult3);
            HOperatorSet.GenEmptyObj(out ho_ClassRegionsNotRejected);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);

            try
            {
                ho_Image.Dispose();
                //HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, 100000);
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                //HOperatorSet.ReadImage(out ho_Image, @"C:\Code\Halcon\检测有无\图片1\13.bmp");
                HOperatorSet.GetImageSize(ho_Image, out HTuple width, out HTuple height);
                HOperatorSet.SetPart(WindowHandle, 0, 0, height, width);
               


                HOperatorSet.SetTposition(WindowHandle, 10, 10);
                set_display_font(WindowHandle, 50, "mono", "true", "false");

                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                HOperatorSet.Decompose3(ho_Image, out ho_Image1, out ho_Image2, out ho_Image3
                    );
                ho_ImageResult1.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImageResult3.Dispose();
                HOperatorSet.TransFromRgb(ho_Image1, ho_Image2, ho_Image3, out ho_ImageResult1,
                    out ho_ImageResult2, out ho_ImageResult3, "hsv");
               
                //HOperatorSet.DispObj(ho_Image3, WindowHandle);
                
               
                //HOperatorSet.SetColored(WindowHandle, 12);
                

                ho_ClassRegionsNotRejected.Dispose();
                HOperatorSet.ClassifyImageClassMlp(ho_Image, out ho_ClassRegionsNotRejected,
                    hv_MLPHandle, 0.5);

                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(ho_ClassRegionsNotRejected, out ho_ObjectSelected, 1);
                ho_RegionOpening2.Dispose();
                HOperatorSet.OpeningCircle(ho_ObjectSelected, out ho_RegionOpening2, 5);


                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening2, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", 6000.2, 159205);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);

                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionUnion, out ho_RegionOpening, 3.5);

                ho_RegionTrans.Dispose();
                HOperatorSet.ShapeTrans(ho_RegionOpening, out ho_RegionTrans, "circle");
    
                //HOperatorSet.SetDraw(WindowHandle, "margin");
                //HOperatorSet.ClearWindow(WindowHandle);
                //HOperatorSet.DispObj(ho_Image, WindowHandle);
                
                //HOperatorSet.DispObj(ho_RegionTrans, WindowHandle);
                //HOperatorSet.SetDraw(WindowHandle, "fill");
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionTrans, out ho_ImageReduced);
                ho_ClassRegionsNotRejected.Dispose();
                HOperatorSet.ClassifyImageClassMlp(ho_ImageReduced, out ho_ClassRegionsNotRejected,
                    hv_MLPHandle1, 0.5);

                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(ho_ClassRegionsNotRejected, out ho_ObjectSelected1,
                    4);

                ho_RegionOpening1.Dispose();
                HOperatorSet.OpeningCircle(ho_ObjectSelected1, out ho_RegionOpening1, 3.5);

                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionOpening1, out ho_RegionFillUp);

                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, 10);

                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions1);

                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions1, out ho_SelectedRegions1,
                    "max_area", 70);


                HOperatorSet.SmallestRectangle2(ho_SelectedRegions1, out hv_Row1, out hv_Column1,
                    out hv_Phi, out hv_Length1, out hv_Length2);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row1, hv_Column1, hv_Phi, hv_Length1,
                    hv_Length2);

                //HOperatorSet.AreaCenter(ho_RegionFillUp, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.SetDraw(WindowHandle, "margin");
                HOperatorSet.ClearWindow(WindowHandle);
                HOperatorSet.DispObj(ho_Image, WindowHandle);
                
                if (hv_Length1> ParaSetting.MinL1 && hv_Length1<ParaSetting.MaxL1 && hv_Length2>ParaSetting.MinL2 && hv_Length2<ParaSetting.MaxL2)
                {
                    HOperatorSet.SetColor(WindowHandle, "green");
                    HOperatorSet.DispObj(ho_Rectangle, WindowHandle);
                    HOperatorSet.WriteString(WindowHandle, "OK");
                    if (ParaSetting.UseOutput == EnumUseOutput.Use)
                    {
                        HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "UserOutputValue", ParaSetting.OutputLogicNG == EnumOutputLogic.False ? 1 : 0);
                    }
                }
                else
                {
                    HOperatorSet.SetColor(WindowHandle, "red");
                    HOperatorSet.WriteString(WindowHandle, "NG");
                    if (ParaSetting.UseOutput == EnumUseOutput.Use)
                    {
                        HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "UserOutputValue", ParaSetting.OutputLogicNG == EnumOutputLogic.False ? 0 : 1);
                    }
                }


            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();
                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImageResult3.Dispose();
                ho_ClassRegionsNotRejected.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionOpening2.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionTrans.Dispose();
                ho_ImageReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionOpening1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_Rectangle.Dispose();
                throw HDevExpDefaultException;
            }
            ho_Image.Dispose();
            ho_Image1.Dispose();
            ho_Image2.Dispose();
            ho_Image3.Dispose();
            ho_ImageResult1.Dispose();
            ho_ImageResult2.Dispose();
            ho_ImageResult3.Dispose();
            ho_ClassRegionsNotRejected.Dispose();
            ho_ObjectSelected.Dispose();
            ho_RegionOpening2.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionUnion.Dispose();
            ho_RegionOpening.Dispose();
            ho_RegionTrans.Dispose();
            ho_ImageReduced.Dispose();
            ho_ObjectSelected1.Dispose();
            ho_RegionOpening1.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionClosing.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_Rectangle.Dispose();
            var TimeElipps = TimeSpan.FromTicks(DateTime.Now.Ticks - StartTime).TotalMilliseconds;
            Console.WriteLine(TimeElipps);
            return true;
        }

    }
}
