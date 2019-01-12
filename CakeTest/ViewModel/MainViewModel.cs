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
        HardwareInfo Devie = new HardwareInfo();
        string FILE_CONFIG = "./Config/SystemConfig.json";
        #endregion

        #region Constructor
        public MainViewModel()
        {
            LoadConfig();
            SystemErrorMessageCollection = new ObservableCollection<MessageItem>();
            SystemErrorMessageCollection.CollectionChanged += SystemErrorMessageCollection_CollectionChanged;
            try
            {
                var CamList=VisionCommonFunc.FindCamera(EnumCamType.GigEVision, out List<string> ErrorList);
                if (CamList.Count < 1)
                    throw new Exception("未发现相机");
                VisionCommonFunc.OpenCamera(CamList[0].NameForVision);
                VisionCommonFunc.SetCameraPara(ParaSetting.Para);
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


        #endregion


        #region Private method
        private int ThreadFunc(object o)
        {
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    SystemState = EnumSystemState.Running;
                    VisionCommonFunc.TestCake(WindowHandle);
                }
                SystemState = EnumSystemState.Idle;
                return 0;
            }
            catch (Exception ex)
            {
                ShowErrorinfo($"运行中出现错误,程序中止运行:{ex.Message}");
                t = null;
                SystemState = EnumSystemState.Idle;
                return -1;
            }
        }

        private void ShowErrorinfo(string ErrorMsg)
        {
            Application.Current.Dispatcher.Invoke(()=> {
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
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
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}