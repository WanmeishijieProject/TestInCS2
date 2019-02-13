using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CameraWindow.Classes;
using HalconDotNet;

namespace CameraWindow.ImageOperator.Implement
{
    [Serializable]
    public class RotateImage : ImageOperatorBase
    {
        public RotateImage() { }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="content"></param>
        public RotateImage(SerializationInfo info, StreamingContext content) : base(info, content)
        {
            if (info != null)
            {
                angle = (EnumRotateAngle)info.GetValue("Angle", typeof(EnumRotateAngle));
            }
        }

        EnumRotateAngle angle;

        [Description("设置图像旋转的角度")]
        public EnumRotateAngle Angle
        {
            get { return angle; }
            set
            {
                if (angle != value)
                {
                    angle = value;
                    RaisePropertyChanged("Summary");
                    RaisePropertyChanged();
                }
            }
        }
        public override void Run()
        {
            HOperatorSet.RotateImage(ImageIn, out imageOut, (double)Angle%360, "constant");
            ImageIn.Dispose();
        }

        public override void Run(HObject ImageIn,out HObject ImageOut)
        {
            ImageOut = null;
            HOperatorSet.RotateImage(ImageIn, out ImageOut, (double)Angle % 360, "constant");
            ImageIn.Dispose();
        }

        [Browsable(false)]
        public override string Summary
        {
            get
            {
                return $"相机{CamName} 旋转图像{Angle}度";
            }
            set { }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Angle", Angle);
            base.GetObjectData(info, context);
        }
    }
}
