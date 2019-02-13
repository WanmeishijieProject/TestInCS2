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
    public enum EnumMirrorType
    {
        MirrorX,
        MirrorY,
    }
    public enum EnumRotateAngle
    {
        Angle90=90,
        Angle180=180,
        Angle270=270,
    }

    [Serializable]
    public class MirrorImage : ImageOperatorBase
    {
        public MirrorImage() { }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MirrorImage(SerializationInfo info, StreamingContext context):base(info,context)
        {
            if (info != null)
            {
                mirrorType = (EnumMirrorType)info.GetValue("MirrorType", typeof(EnumMirrorType));
            }
        }


        EnumMirrorType mirrorType;


        [Description("选择图形镜像类型")]
        public EnumMirrorType MirrorType
        {
            get { return mirrorType; }
            set
            {
                if (mirrorType != value)
                {
                    mirrorType = value;
                    RaisePropertyChanged("Summary");
                    RaisePropertyChanged();
                }
            }
        }
        public override void Run()
        {
            switch (MirrorType)
            {
                case EnumMirrorType.MirrorX:
                    HOperatorSet.MirrorImage(ImageIn, out imageOut, "row");
                    ImageIn.Dispose();
                    break;
                case EnumMirrorType.MirrorY:
                    HOperatorSet.MirrorImage(ImageIn, out imageOut, "column");
                    ImageIn.Dispose();
                    break;
                default:
                    break;
            } 
        }
        public override void Run(HObject ImageIn, out HObject ImageOut)
        {
            ImageOut = null;
            switch (MirrorType)
            {
                case EnumMirrorType.MirrorX:
                    HOperatorSet.MirrorImage(ImageIn, out ImageOut, "row");
                    ImageIn.Dispose();
                    break;
                case EnumMirrorType.MirrorY:
                    HOperatorSet.MirrorImage(ImageIn, out ImageOut, "column");
                    ImageIn.Dispose();
                    break;
                default:
                    break;
            }
        }


        [Browsable(false)]
        public override string Summary
        {
            get {
                return $"相机{CamName} 翻转图像{MirrorType}";
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
            info.AddValue("MirrorType", MirrorType);
            base.GetObjectData(info, context);
        }

    }
}
