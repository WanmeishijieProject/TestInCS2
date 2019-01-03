using GalaSoft.MvvmLight;
using CameraWindow.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using VisionLib;
using static VisionLib.VisionDefinitions;
using HalconDotNet;
using VisionLib.CommonVisionStep;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace CameraWindow.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Field
        HalconVision Vision = HalconVision.Instance;
        List<CameraInfoModel> CamInfoDataList = null;
        #endregion

        #region Construct
        public MainViewModel()
        {

            CamInfoDataList = Vision.FindCamera(EnumCamType.GigEVision, new List<string>() { "Cam_Up" }, out List<string> ErrorList);
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
                            SetMaxCameraView(IndexBaseZero);
                            var data = CamInfoDataList[0];
                            if (!Vision.IsCamOpen(data.CamID))
                                Vision.OpenCam(data.CamID);
                            Vision.AttachCamWIndow(data.CamID, "Cam1", WindowsList[data.CamID]);
                            var StartTime = DateTime.Now.Ticks;
                            while (true)
                            {
                                Vision.GrabImage(data.CamID);
                                //Vision.disp_message(WindowsList[data.CamID], data.ActualName, "image", 10, 10, "red", "false");
                                if (TimeSpan.FromTicks(DateTime.Now.Ticks - StartTime).TotalSeconds > 50)
                                    break;
                            }
                            //Vision.DisplayLines(data.CamID, new List<VisionLineData>() { new VisionLineData(0, 0, 200, 200) });
                            Vision.SaveImage(data.CamID, EnumImageType.Window, "C:\\公司文件资料", "1234567.jpg", WindowsList[data.CamID]);

                            Vision.CloseCam(data.CamID);
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
                    ResumeGridDataList();
                });
            }
        }

        public RelayCommand<string> CommandOnSizeChanged
        {
            get {
                return new RelayCommand<string>(str =>
                {
                    try
                    {
                        int nCamID = int.Parse(str.Substring(str.Length - 1))-1;
                        var Cam = from list in CamInfoDataList where list.CamID == nCamID select list;
                        if (Cam.Count() != 0)
                        {
                            Vision.GetSyncSp(out AutoResetEvent Se, out object Lock, nCamID);
                            if (Lock != null)
                            {
                                lock (Lock)
                                {
                                    if (Se != null)
                                        Se.Set();
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
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
        #endregion

    }
}