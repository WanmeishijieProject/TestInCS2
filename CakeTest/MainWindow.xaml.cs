using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using CakeTest.UserCtrls;
using CakeTest.ViewModel;
using ResizableWIndow;
namespace CakeTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CanResizableVisionWindowWpf ,INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        double _daysLeft;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Closing += (s, e) => ViewModelLocator.Cleanup();
                OnResizeEventHandler += MainWindow_OnResizeEventHandler;
                Register();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} : {ex.StackTrace}");
                Close();
            }
           
        }

        private void MainWindow_OnResizeEventHandler(object sender, bool e)
        {
            (DataContext as MainViewModel).SetResizingFlag(e);
        }

        public double DaysLeft
        {
            get { return _daysLeft; }
            set
            {
                if (_daysLeft != value)
                {
                    _daysLeft = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("DaysLeft"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RibbonPopupMenuButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (File.Exists("zrd.data"))
            {
                File.Delete("zrd.data");
            }
        }

        private void Register()
        {
            double Days = 0;
            if (!Window_Register.CheckFile(out Days))
            {
                Window_Register RegisterWindow = new Window_Register();
                RegisterWindow.ShowDialog();
                if (!RegisterWindow.IsCheckSuccess)
                {
                    MessageBox.Show("注册失败", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    Close();
                }
                else
                {
                    Days = RegisterWindow.DaysLeft;
                }
            }
            DaysLeft = Days;
        }

        private void CanResizableVisionWindowWpf_Closing(object sender, CancelEventArgs e)
        {
            (DataContext as MainViewModel).CommandClosingWindow.Execute(null);
        }

        //private void Window_SourceInitialized(object sender, EventArgs e)
        //{
        //    HwndSource source = HwndSource.FromVisual(this) as HwndSource;

        //    if (source != null)
        //    {
        //        // 注册窗口消息处理函数
        //        source.AddHook(new HwndSourceHook(WinProc));

        //    }
        //}
        //public const Int32 WM_EXITSIZEMOVE = 0x0232;
        //public const Int32 WM_ENTERSIZEMOVE = 0x0231;
        //public const Int32 WM_SIZE = 0x0005;
        //public const Int32 WM_WINDOWPOSCHANGING = 0x0046;
        //public const Int32 WM_SYSCOMMAND = 0x0112;

        //public const Int32 SC_MAXIMIZE = 0xF030;
        //public const Int32 SC_MINIMIZE = 0xF020;
        //public const Int32 SC_STORE = 0xF012;
        //public const Int32 SC_RESTORE = 0xF120;

        //private IntPtr WinProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        //{
        //    IntPtr result = IntPtr.Zero;
        //    switch (msg)
        //    {
        //        case WM_ENTERSIZEMOVE:
        //            (DataContext as MainViewModel).SetResizingFlag(true);
        //            break;

        //        case WM_EXITSIZEMOVE:
        //            (DataContext as MainViewModel).SetResizingFlag(false);
        //            break;

        //        case WM_SYSCOMMAND:
        //            if ((int)wParam == SC_MAXIMIZE || (int)wParam == SC_MINIMIZE ||
        //                (int)wParam == SC_RESTORE || (int)wParam == SC_STORE)
        //            {
        //                (DataContext as MainViewModel).SetResizingFlag(true);
        //            }
        //            break;

        //    }
        //    return result;
        //}

        //private void Window_StateChanged(object sender, EventArgs e)
        //{
        //    (DataContext as MainViewModel).SetResizingFlag(false);
        //}

    }
}