using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using CameraWindow.ViewModel;
using VisionLib;

namespace CameraWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CommandWindowLoaded.Execute(null);
        }

       

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = HwndSource.FromVisual(this) as HwndSource;

            if (source != null)
            {
                // 注册窗口消息处理函数
                source.AddHook(new HwndSourceHook(WinProc));
            }
        }
        public const Int32 WM_EXITSIZEMOVE = 0x0232;
        public const Int32 WM_ENTERSIZEMOVE = 0x0231;
        public const Int32 WM_SIZE = 0x0005;
        public const Int32 WM_WINDOWPOSCHANGING = 0x0046;
        public const Int32 WM_SYSCOMMAND = 0x0112;

        public const Int32 SC_MAXIMIZE = 0xF030;
        public const Int32 SC_MINIMIZE = 0xF020;
        public const Int32 SC_STORE = 0xF012;
        public const Int32 SC_RESTORE = 0xF120;

        private IntPtr WinProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            IntPtr result = IntPtr.Zero;
            switch (msg)
            {
                case WM_ENTERSIZEMOVE:
                    (DataContext as MainViewModel).SetResizingFlag(true);
                    break;

                case WM_EXITSIZEMOVE:
                    (DataContext as MainViewModel).SetResizingFlag(false);
                    break;

                case WM_SYSCOMMAND:
                    if ((int)wParam == SC_MAXIMIZE || (int)wParam == SC_MINIMIZE || 
                        (int)wParam == SC_RESTORE || (int)wParam == SC_STORE)
                    {
                        (DataContext as MainViewModel).SetResizingFlag(true);
                    }
                    break;

            }
            return result;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            (DataContext as MainViewModel).SetResizingFlag(false);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as MainViewModel).CommandCloseWindow.Execute(null);
        }
    }
}