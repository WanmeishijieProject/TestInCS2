using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
namespace CameraWindow.ImageOperator
{
    public interface IImageOperator : ISerializable
    {
        HObject ImageIn { get; set; }
        HObject ImageOut { get; set; }
        string Summary { get; set; }

        /// <summary>
        /// 用来标识这个操作的目标
        /// </summary>
        string CamName { get; set; }
        void Run();
    }
}
