using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace 硬件触发相机拍照
{
    public partial class Form1 : Form
    {
        private HTuple hv_AcqHandle;
        private int i = 0;
        static HalconAPI.HFramegrabberCallback FrameCallback;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "progressive",
                -1, "default", -1, "false", "default", "Cam_Up", 0, -1, out hv_AcqHandle);
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSelector", "FrameStart");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", 100000);
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "AcquisitionMode", "SingleFrame");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line1");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerActivation", "FallingEdge");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureMode", "Timed");
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "available_callback_types", out HTuple value);
            foreach (var it in value.SArr)
                Console.WriteLine(it);

            int myContent = 1;
            IntPtr ptr1 = GCHandle.Alloc(myContent, GCHandleType.Pinned).AddrOfPinnedObject();
            
            FrameCallback += MyCallback;
            IntPtr Ptr = Marshal.GetFunctionPointerForDelegate(FrameCallback);
            HOperatorSet.SetFramegrabberCallback(hv_AcqHandle, "ExposureEnd", Ptr, ptr1);
        }

        public int MyCallback(IntPtr AcqHandle, IntPtr Context, IntPtr UserContext)
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//显示错误
                return -1;
            }
        }

        private async void Btn_Grab_Click(object sender, EventArgs e)
        {
            await Task.Run(()=> {
                while (true)
                {
                    //HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                    HOperatorSet.GrabImage(out HObject img, hv_AcqHandle);
                    if (hWindowControl1.InvokeRequired)//线程亲和性判定
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            HOperatorSet.GetImageSize(img, out HTuple width, out HTuple height);
                            HOperatorSet.SetPart(this.hWindowControl1.HalconWindow, 0, 0, height, width);
                            HOperatorSet.DispObj(img, this.hWindowControl1.HalconWindow); img.Dispose();
                            HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 0, 0);
                            HOperatorSet.WriteString(this.hWindowControl1.HalconWindow, (i++).ToString());

                        })); //把图像显示出来（这里是委托方式显示)
                    }
                    else
                    {
                        HOperatorSet.DispObj(img, this.hWindowControl1.HalconWindow);//把图像显示出来
                        img.Dispose();
                    }
                }
            });
        }
    }
}
