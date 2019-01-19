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
        CancellationTokenSource[] cts = new CancellationTokenSource[4];
        Task[] MonitorTask = new Task[4] ;
        bool _isRunning = false;
        CameraConfigManager CamConfigMgr = null;
        string FILE_CONFIG = AppDomain.CurrentDomain.BaseDirectory + "/Config/SystemConfig.json";
        string[] WindowNameArr = { "Cam1", "Cam2", "Cam3", "Cam4" };
        #endregion

        #region Construct
        public MainViewModel()
        {
            LoadConfig();

            var listName = new List<string>();
            foreach (var it in CamConfigMgr.Cameras)
                listName.Add(it.CamName);
            
            CamInfoDataList = Vision.FindCamera(EnumCamType.GigEVision, listName, out List<string> ErrorList);
            CameraCollection = new ObservableCollection<string>();
            foreach (var data in CamInfoDataList)
            {
                CameraCollection.Add(data.ActualName);
            }
           
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
                            SetResizingFlag(true);
                            SetMaxCameraView(IndexBaseZero);
                            SetResizingFlag(false);
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
                    SetResizingFlag(true);
                    ResumeGridDataList();
                    SetResizingFlag(false);
                });
            }
        }


        public RelayCommand CommandStartMonitor
        {
            get { return new RelayCommand(()=> {
                //打开相机
                foreach (var CamInfo in CamInfoDataList)
                {
                    if (!Vision.IsCamOpen(CamInfo.CamID))
                        Vision.OpenCam(CamInfo.CamID, EnumColorSpace.Rgb);
                    Vision.AttachCamWIndow(CamInfo.CamID, WindowNameArr[CamInfo.CamID], WindowsList[CamInfo.CamID]);
                }

                //取图
                foreach (var CamInfo in CamInfoDataList)
                {
                    if (MonitorTask[CamInfo.CamID] == null || MonitorTask[CamInfo.CamID].IsCanceled || MonitorTask[CamInfo.CamID].IsCompleted)
                    {
                        cts[CamInfo.CamID] = new CancellationTokenSource();
                        MonitorTask[CamInfo.CamID] = new Task(() => {
                            while (!cts[CamInfo.CamID].IsCancellationRequested)
                            {
                                try
                                {
                                    IsRunning = true;
                                    Vision.GrabImage(CamInfo, true, true);
                                    if (CamInfo.NameForVision == "CamTop")
                                    {
                                        HOperatorSet.RotateImage(CamInfo.Image, out HObject ImageRotate, 90, "constant");
                                        Vision.DisplayImage(WindowsList[CamInfo.CamID], ImageRotate, true);
                                    }
                                    else
                                    {
                                        HOperatorSet.MirrorImage(CamInfo.Image, out HObject ImageMirror, "row");
                                        Vision.DisplayImage(WindowsList[CamInfo.CamID], ImageMirror, true);
                                    }
                                }
                                catch
                                {

                                }
                                
                                //Vision.DisplayImage(CamInfo);
                            }
                            IsRunning = false;
                        }, cts[CamInfo.CamID].Token);
                        MonitorTask[CamInfo.CamID].Start();
                    }
                }
            }); }
        }

        public RelayCommand CommandStopMonitor
        {
            get { return new RelayCommand(()=> {
                foreach (var CamInfo in CamInfoDataList)
                {
                    cts[CamInfo.CamID].Cancel();
                }
                foreach (var CamInfo in CamInfoDataList)
                {
                    MonitorTask[CamInfo.CamID].Wait(1000);
                }
            }); }
        }
        public RelayCommand CommandCloseWindow
        {
            get
            {
                return new RelayCommand(() => {
                    Vision.CloseAllCamera();
                });
            }
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

        public void SetResizingFlag(bool IsSizing = true)
        {
            lock (Vision.SyncData.VisionLock)
            {
                Vision.SyncData.IsNewSizing = IsSizing;  
                Console.WriteLine(IsSizing? "Sizing" : "Done");
                if (IsSizing)
                    Vision.SyncData.IsOldSizing = IsSizing;
            }
        }

    }
}