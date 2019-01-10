using GalaSoft.MvvmLight;
using CameraWindow.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using VisionLib;
using static VisionLib.VisionDefinitions;
using HalconDotNet;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;
using VisionDemoLib;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using CameraWindow.Config;

namespace CameraWindow.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Field
        HalconVision Vision = HalconVision.Instance;
        List<CameraInfoModel> CamInfoDataList = null;
        ManualResetEvent mr = new ManualResetEvent(true);
        CancellationTokenSource cts = null;
        Task MonitorTask = null;
        bool _isRunning = false;
        CameraConfigManager CamConfigMgr = null;
        string FILE_CONFIG = Directory.GetCurrentDirectory()+ @"/Config/SystemConfig.json";
        #endregion

        #region Construct
        public MainViewModel()
        {
            LoadConfig();

            CamInfoDataList = Vision.FindCamera(EnumCamType.GigEVision, new List<string>() { "CamBack","CamUp" }, out List<string> ErrorList);
            CameraCollection = new ObservableCollection<string>();
            Vision.SetVisionSync(ref mr);   //设置同步mr

            foreach (var data in CamInfoDataList)
            {
                CameraCollection.Add(data.ActualName);
            }
            CameraCollection.Add("1");
            //初始化样式
            GridDataModelCollect = new ObservableCollection<CameraGridDataModel>();
            SetGridData(CameraCollection.Count);
            WindowsList = new List<HWindow>() {
                new HWindow(),new HWindow(), new HWindow(), new HWindow()
            };
        }
        #endregion

        #region Command
        public RelayCommand CommandWindowLoaded
        {
            get { return new RelayCommand(()=> {

            }); }
        }
        public RelayCommand<string> CommandMaxCamera
        {
            get
            {
                return new RelayCommand<string>(str => {
                    Task.Run(() =>
                    {
                        int IndexBaseZero = int.Parse(str) - 1;
                        if (IndexBaseZero >= 0 && IndexBaseZero < CameraCollection.Count)
                        {
                            mr.Reset();
                            SetMaxCameraView(IndexBaseZero);
                            mr.Set();
                        }
                    });
                });
            }
        }
        public RelayCommand CommandResumeCamera
        {
            get
            {
                return new RelayCommand(() => {
                    mr.Reset();
                    ResumeGridDataList();
                    mr.Set();
                });
            }
        }


        public RelayCommand CommandStartMonitor
        {
            get { return new RelayCommand(()=> {
                if (MonitorTask == null || MonitorTask.IsCanceled || MonitorTask.IsCompleted)
                {
                    cts = new CancellationTokenSource();
                    MonitorTask = new Task(()=> {
                        var data = CamInfoDataList[0];
                        if (!Vision.IsCamOpen(data.CamID))
                            Vision.OpenCam(data.CamID);
                        Vision.AttachCamWIndow(data.CamID, "Cam1", WindowsList[data.CamID]);
                        var StartTime = DateTime.Now.Ticks;
                        while (!cts.IsCancellationRequested)
                        {
                            IsRunning = true;
                            Vision.GrabImage(data.CamID, true, true);
                            //Vision.disp_message(WindowsList[data.CamID], data.ActualName, "image", 10, 10, "red", "false");
                            if (TimeSpan.FromTicks(DateTime.Now.Ticks - StartTime).TotalSeconds > 100)
                                break;
                        }
                        IsRunning = false;
                        Vision.CloseCam(data.CamID);
                    }, cts.Token);
                    MonitorTask.Start();
                    
                }
            }); }
        }

        public RelayCommand CommandStopMonitor
        {
            get { return new RelayCommand(()=> {
                cts.Cancel();
                MonitorTask.Wait();
            }); }
        }
       

        #endregion

        #region Property
        public ObservableCollection<string> CameraCollection
        {
            get;
            set;
        }

        public ObservableCollection<CameraGridDataModel> GridDataModelCollect
        {
            get;
            set;
        }

        public List<HWindow> WindowsList
        {
            get;
            set;
        }

        public bool IsRunning {
            get { return _isRunning; }
            set {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Private method
        private void SetGridData(int CamCount)
        {
            switch (CamCount)
            {
                case 1:
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 2, ColSpan = 2, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    break;
                case 2:
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 2, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 1, RowSpan = 2, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    break;
                case 3:
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 2, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 1, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    break;
                case 4:
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 1, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 1, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible });
                    break;
                default:
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    GridDataModelCollect.Add(new CameraGridDataModel() { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden });
                    break;
            }
        }
        private void ResetGridData(int CamCount)
        {
            if (GridDataModelCollect.Count == 4)
            {
                switch (CamCount)
                {
                    case 1:
                        GridDataModelCollect[0] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 2, ColSpan = 2, Visible = Visibility.Visible };
                        GridDataModelCollect[1] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden };
                        GridDataModelCollect[2] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden };
                        GridDataModelCollect[3] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden };
                        break;
                    case 2:
                        GridDataModelCollect[0] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 2, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[1] = new CameraGridDataModel { Row = 0, Col = 1, RowSpan = 2, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[2] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden };
                        GridDataModelCollect[3] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden };
                        break;
                    case 3:
                        GridDataModelCollect[0] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 2, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[1] = new CameraGridDataModel { Row = 0, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[2] = new CameraGridDataModel { Row = 1, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[3] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Hidden };
                        break;
                    case 4:
                        GridDataModelCollect[0] = new CameraGridDataModel { Row = 0, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[1] = new CameraGridDataModel { Row = 0, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[2] = new CameraGridDataModel { Row = 1, Col = 0, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible };
                        GridDataModelCollect[3] = new CameraGridDataModel { Row = 1, Col = 1, RowSpan = 1, ColSpan = 1, Visible = Visibility.Visible };
                        break;
                    default:
                        break;
                }
            }
        }
        private void SetMaxCameraView(int IndexBaseZero)
        {
            for (int i = 0; i < GridDataModelCollect.Count; i++)
            {
                if (i == IndexBaseZero)
                {
                    GridDataModelCollect[i].Row = 0;
                    GridDataModelCollect[i].Col = 0;
                    GridDataModelCollect[i].RowSpan = 2;
                    GridDataModelCollect[i].ColSpan = 2;
                    GridDataModelCollect[i].Visible = Visibility.Visible;
                }
                else
                {
                    GridDataModelCollect[i].Visible = Visibility.Hidden;
                }

            }
        }
        private void ResumeGridDataList()
        {
            ResetGridData(CameraCollection.Count);
        }

        private void LoadConfig()
        {
            try
            {
                var jsonStr = File.ReadAllText(FILE_CONFIG);
                CamConfigMgr = JsonConvert.DeserializeObject<CameraConfigManager>(jsonStr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        public void SetSyncState(bool IsSet = true)
        {
            if (IsSet)
                mr.Set();
            else
                mr.Reset();
        }

    }
}