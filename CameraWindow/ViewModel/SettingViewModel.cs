using CameraWindow.ImageOperator;
using CameraWindow.ImageOperator.Implement;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;



namespace CameraWindow.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        const string PATH_OPERATOR = @"..\..\ImageOperator\Implement";
        const string FILE_IMAGEOPERATOR = "ImageOperator.data";
        public ObservableCollection<IImageOperator> list { get; set; }
        public SettingViewModel()
        {
            UsedOperatorCollectRawData = new ObservableCollection<IImageOperator>();
            UsedOperatorCollect = new ObservableCollection<IImageOperator>();
            AvalibleImageOperatorCollect = new ObservableCollection<string>();
            DirectoryInfo folder = new DirectoryInfo(PATH_OPERATOR);
            foreach (FileInfo file in folder.GetFiles("*.cs"))
            {
                AvalibleImageOperatorCollect.Add(file.Name.Replace(".cs", ""));
            }

            //读取Image操作配置文件
            if (File.Exists(FILE_IMAGEOPERATOR))
            {
                using (Stream stream = File.OpenRead(FILE_IMAGEOPERATOR))
                {
                    var bfmt = new BinaryFormatter();
                    var len = stream.Length;
                    while (len>0 && stream.Position<len)
                    {
                        IImageOperator op = bfmt.Deserialize(stream) as IImageOperator;
                        UsedOperatorCollect.Add(op);
                    }
                }
            }
        }

        ~SettingViewModel()
        {
            //保存设置
            using (Stream stream = File.Create(FILE_IMAGEOPERATOR))
            {
                var bfmt = new BinaryFormatter();
                foreach (var it in UsedOperatorCollect)
                {
                    bfmt.Serialize(stream, it);
                }
            }

        }

        public ObservableCollection<string> AvalibleImageOperatorCollect { get; set; }

        /// <summary>
        /// 双击可用的算子
        /// </summary>
        public RelayCommand<string> AvalibleOperatorCommand
        {
            get { return new RelayCommand<string>(StrOperator=> {
                var type = Type.GetType(@"CameraWindow.ImageOperator.Implement." + StrOperator);
                IImageOperator op = Activator.CreateInstance(type) as IImageOperator;
                UsedOperatorCollectRawData.Add(op);
            }); }
        }

        public RelayCommand<IImageOperator> CommandDeleteOperatorUsed
        {
            get
            {
                return new RelayCommand<IImageOperator>(Operator => {
                    if (Operator != null)
                        UsedOperatorCollectRawData.Remove(Operator);
                });
            }
        }
        
        /// <summary>
        /// 存储已经选在的Operator
        /// </summary>
        private ObservableCollection<IImageOperator> UsedOperatorCollectRawData { get; set; }

        /// <summary>
        /// 这个是用户选择不同的相机显示的算子列表
        /// </summary>
        public ObservableCollection<IImageOperator> UsedOperatorCollect
        {
            get {
                ObservableCollection<IImageOperator> TempCollect = new ObservableCollection<IImageOperator>();
                if (CurSelectedCamName != null)
                {
                    foreach (var it in UsedOperatorCollectRawData)
                    {
                        if (it.CamName == CurSelectedCamName.ToString())
                        {
                            TempCollect.Add(it);
                        }
                    }
                }
               // else
                return UsedOperatorCollectRawData;
                //return TempCollect;
            }
            set { }
        }

        /// <summary>
        /// 当前选择的相机名称
        /// </summary>
        public object CurSelectedCamName { get; set; }

       

    }
}
