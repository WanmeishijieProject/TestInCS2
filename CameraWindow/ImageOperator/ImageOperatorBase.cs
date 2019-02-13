using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CameraWindow.Classes;
using CameraWindow.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using HalconDotNet;

namespace CameraWindow.ImageOperator
{
    [Serializable()]
    public class ImageOperatorBase : IImageOperator , INotifyPropertyChanged
    {
        public ImageOperatorBase()
        {

        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ImageOperatorBase(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                camName = info.GetString("CamName");
                summary = info.GetString("Summary");
            }
        }

        string camName="未设置";
        string summary="";

        [NonSerialized()]
        protected HObject imageIn = null;
        [NonSerialized()]
        protected HObject imageOut = null;


        [Browsable(false)]
        public HObject ImageIn
        {
            get { return imageIn; }
            set { imageIn = value; }
        }


        [Browsable(false)]
        public HObject ImageOut
        {
            get { return imageOut; }
            set { imageOut = value; }
        }


        [Browsable(false)]
        public virtual string Summary
        {
            get { return summary; }
            set
            {
                if (summary != value)
                {
                    summary = value;
                    RaisePropertyChanged();
                }
            }
        }

   
        [TypeConverter(typeof(CameraTypeConvert))]
        [Description("设置相机源")]
        public string CamName
        {
            get { return camName; }
            set {
                if (camName != value)
                {
                    camName = value;

                    RaisePropertyChanged();
                    RaisePropertyChanged("Summary");
                }
            }
        }

        [ReadOnly(true)]
        public string OperatorName{
            get
            {
                return GetType().ToString().Split('.').Last();
            }
            set { }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(PropertyName));
        }

        public virtual void Run()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CamName",CamName);
            info.AddValue("Summary", Summary);
        }

        public virtual void Run(HObject ImageIn, out HObject ImageOut)
        {
            throw new NotImplementedException();
        }
    }
}
