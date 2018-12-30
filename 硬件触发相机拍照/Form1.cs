using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace 硬件触发相机拍照
{
    public partial class Form1 : Form
    {
        private HTuple hv_AcqHandle;
        static HalconAPI.HFramegrabberCallback FrameCallback;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "progressive",
                -1, "default", -1, "false", "default", "CamTop", 0, -1, out hv_AcqHandle);
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSelector", "FrameStart");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", 10000);
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line1");
            FrameCallback += FrameCallback;
            //HOperatorSet.get
        }
        public static int MyCallback(IntPtr AcqHandle, IntPtr Context, IntPtr UserContext)
        {
            int Content = UserContext.ToInt32();
            Console.WriteLine("--------进入回调函数--------");
            return 0;
        }

        private void Btn_Grab_Click(object sender, EventArgs e)
        {
            try
            {
                int myContent = 2;
                HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "available_callback_types", out HTuple value);
                foreach (var it in value.SArr)
                    Console.WriteLine(it);
                int len = value.SArr.Length;
                IntPtr Ptr = Marshal.GetFunctionPointerForDelegate(FrameCallback);
                HOperatorSet.SetFramegrabberCallback(hv_AcqHandle, value.SArr[len-4], Ptr, myContent);
                HOperatorSet.GetFramegrabberCallback(hv_AcqHandle, value.SArr[len - 4], out HTuple callFunc, out HTuple content);
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                HOperatorSet.GrabImageAsync(out HObject image, hv_AcqHandle, -1);
                
            }
            catch(Exception ex)
            {

            }
        }
    }
}
