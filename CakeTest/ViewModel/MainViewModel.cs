using CakeTest.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using HalconDotNet;
using CakeTest.Class;
using Com.Netframe.Computer.Handware;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CakeTest.Config.ParaSettingManager;
using System.IO;
using Newtonsoft.Json;

namespace CakeTest.ViewModel
{
    public enum EnumSystemState
    {
        Running,
        Pause,
        Idle,
    }
    public class MainViewModel : ViewModelBase
    {
        #region Field
        private EnumSystemState _systemState = EnumSystemState.Idle;
        private int _viewIndex = 1;
        private int _errorCount = 0;
        string _snakeLastError;
        Task t = null;
        CancellationTokenSource cts = new CancellationTokenSource();
        HalconFunc VisionCommonFunc = new HalconFunc();
        bool _showSnakeInfoBar = false;
        string LastError = "";
        string FILE_CONFIG = "./Config/SystemConfig.json";
        #endregion

        #region Constructor
        public MainViewModel()
        {
            LoadConfig();
            VisionCommonFunc.SetCameraPara(ParaSetting.Para);
            SystemErrorMessageCollection = new ObservableCollection<MessageItem>();
            SystemErrorMessageCollection.CollectionChanged += SystemErrorMessageCollection_CollectionChanged;
            try
            {
                var CamList=VisionCommonFunc.FindCamera(EnumCamType.GigEVision, out List<string> ErrorList);
                if (CamList.Count < 1)
                    throw new Exception("未发现相机");
                
                VisionCommonFunc.OpenCamera(CamList[0].NameForVision);
            }
            catch (Exception ex)
            {
                SystemErrorMessageCollection.Add(new MessageItem()
                {
                    MsgType = EnumMessageType.Error,
                    StrMsg = $"打开相机出错:{ex.Message}",
                });
            }

        }
        ~MainViewModel()
        {
            VisionCommonFunc.CloseCamera();
           
        }
        #endregion


        #region Property
        public EnumSystemState SystemState
        {
            set
            {
                if (_systemState != value)
                {
                    _systemState = value;
                    RaisePropertyChanged();
                }
            }
            get { return _systemState; }
        }

        public int ErrorCount
        {
            get { return _errorCount; }
            set
            {
                if (_errorCount != value)
                {
                    _errorCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int ViewIndex
        {
            get { return _viewIndex; }
            set
            {
                if (_viewIndex != value)
                {
                    _viewIndex = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool ShowSnakeInfoBar
        {
            get { return _showSnakeInfoBar; }
            set
            {
                if (_showSnakeInfoBar != value)
                {
                    _showSnakeInfoBar = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SnakeLastError
        {
            get { return _snakeLastError; }
            set
            {
                if (_snakeLastError != value)
                {
                    _snakeLastError = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<MessageItem> SystemErrorMessageCollection
        {
            get;
            set;
        }

        public SettingParaManager ParaSetting
        {
            get;set;
        }

        public HWindow WindowHandle
        {
            get;set;
        }
        #endregion


        #region Command
        /// <summary>
        /// 错误信息弹出框的响应按钮
        /// </summary>
        public RelayCommand SnackBarActionCommand
        {
            get
            {
                return new RelayCommand(() => ShowSnakeInfoBar = false);
            }
        }
        public RelayCommand ClearMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SystemErrorMessageCollection.Clear();
                    LastError = "";
                });
            }
        }
        public RelayCommand StartStationCommand
        {
            get
            {
                return new RelayCommand(() => {
                    if (t == null || t.Status == TaskStatus.Canceled || t.Status == TaskStatus.RanToCompletion)
                    {
                        cts = new CancellationTokenSource();
                        t = new Task(() => ThreadFunc(this), cts.Token);
                        t.Start();
                    }
                });
            }
        }
        public RelayCommand StopStationCommand
        {
            get
            {
                return new RelayCommand(() => {
                    cts.Cancel();
                });
            }
        }
        public RelayCommand BtnHomeCommand
        {
            get
            {
                return new RelayCommand(() => {
                    ViewIndex = 1;
                });
            }
        }
        public RelayCommand BtnWarningCommand
        {
            get
            {
                return new RelayCommand(() => {
                    ViewIndex = 2;
                });
            }
        }
        public RelayCommand CommandSavePara
        {
            get
            {
                return new RelayCommand(() => {
                    VisionCommonFunc.SetCameraPara(ParaSetting.Para);
                    SaveConfig();
                });
            }
        }
        public RelayCommand CommandSaveImage
        {
            get
            {
                return new RelayCommand(() => {
                    try
                    {

                        VisionCommonFunc.SaveImage(0, EnumImageType.image, "ImageSaved", WindowHandle);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"保存图片错误，详细信息:{ex.Message}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                });
            }
        }
        
        #endregion


        #region Private method
        private int ThreadFunc(object o)
        {
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    SystemState = EnumSystemState.Running;
                    //VisionCommonFunc.TestCake(WindowHandle);
                    VisionCommonFunc.Action(WindowHandle);
                }
                catch (Exception ex)
                {
                    ShowErrorinfo($"运行中出现错误,程序中止运行:{ex.Message}");
                }
            }
            SystemState = EnumSystemState.Idle;
            return 0;
        }

        private void ShowErrorinfo(string ErrorMsg)
        {
            Application.Current.Dispatcher.Invoke(()=> {
                if (!string.IsNullOrEmpty(ErrorMsg) && LastError!=ErrorMsg)
                {
                    LastError = ErrorMsg;
                    SnakeLastError = $"{DateTime.Now.GetDateTimeFormats()[35]}: {ErrorMsg}";
                    ShowSnakeInfoBar = true;
                    SystemErrorMessageCollection.Add(new MessageItem() { MsgType = EnumMessageType.Error, StrMsg = SnakeLastError });
                }
            });        
        }

        private void SystemErrorMessageCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var colls = from item in SystemErrorMessageCollection where item.MsgType == EnumMessageType.Error select item;
            if (colls != null)
                ErrorCount = colls.Count();
        }


        private void LoadConfig()
        {
            try
            {
                var jsonStr = File.ReadAllText(FILE_CONFIG);
                ParaSetting = JsonConvert.DeserializeObject<SettingParaManager>(jsonStr);

            }
            catch (Exception ex)
            {
                ShowErrorinfo("加载配置文件出错");
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveConfig()
        {
            try
            {
                var jsonStr = JsonConvert.SerializeObject(ParaSetting);
                File.WriteAllText(FILE_CONFIG,jsonStr);
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}