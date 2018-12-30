using CameraWindow.Model;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Vision
{
    public enum EnumCamType
    {
        GigEVision,
        DirectShow,
        uEye,
        HuaRay
    }
    public class HalconVision
    {
        public List<CameraData> FindCamera(EnumCamType camType, List<string> acturalNameList, out List<string> ErrorList)
        {
            var CameraDataList = new List<CameraData>();
            ErrorList = new List<string>();
#if TEST
            CameraDataList.Add(new CameraData() {
                 CameType=EnumCamType.DirectShow,
                  UserName="DirectShow",
                   NameForVision= "Integrated Camera"
               });
            return CameraDataList;
#endif
            try
            {
                HOperatorSet.InfoFramegrabber(camType.ToString(), "info_boards", out HTuple hv_Information, out HTuple hv_ValueList);
                if (0 == hv_ValueList.Length)
                    return CameraDataList;
                for (int i = 0; i < acturalNameList.Count; i++)
                {
                    bool bFind = false;
                    foreach (var dev in hv_ValueList.SArr)
                    {
                        var listAttr = dev.Split('|').Where(a => a.Contains("device:"));
                        if (listAttr != null && listAttr.Count() > 0)
                        {
                            string Name = listAttr.First().Trim().Replace("device:", "");
                            if (Name.Contains(acturalNameList[i]))
                            {
                                CameraDataList.Add(new CameraData() {
                                    CameType = camType,
                                    NameForVision= Name.Trim(),
                                    UserName= acturalNameList[i]
                                });
                               
                                bFind = true;
                                break;
                            }
                        }
                    }
                    if (!bFind)
                        ErrorList.Add($"相机:{ acturalNameList[i]}未找到硬件，请检查硬件连接或者配置");
                }
                return CameraDataList;
            }
            catch (Exception ex)
            {
                ErrorList.Add($"FIndCamera error:{ex.Message}");
                return CameraDataList;
            }

        }
    }
}
