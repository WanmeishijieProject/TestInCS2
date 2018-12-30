using GalaSoft.MvvmLight;
using CameraWindow.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using CameraWindow.Vision;

namespace CameraWindow.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Field
        private HalconVision Vision = new HalconVision();
        #endregion

        #region Construct
        public MainViewModel()
        {
            var CamData = Vision.FindCamera(EnumCamType.GigEVision, new List<string>() { "CamTOp,CamBack" }, out List<string> ErrorList);
            CameraCollection = new ObservableCollection<string>();

            foreach (var data in CamData)
                CameraCollection.Add(data.UserName);
            CameraCollection.Add("1");
            //CameraCollection.Add("2");
            //初始化样式
            GridDataModelCollect = new ObservableCollection<CameraGridDataModel>();
            SetGridData(CameraCollection.Count);
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
                    int IndexBaseZero = int.Parse(str) - 1;
                    if (IndexBaseZero >= 0 && IndexBaseZero < CameraCollection.Count)
                        SetMaxCameraView(IndexBaseZero);
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