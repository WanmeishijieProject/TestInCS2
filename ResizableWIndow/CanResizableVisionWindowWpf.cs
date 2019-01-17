using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ResizableWIndow
{


    public class CanResizableVisionWindowWpf : Window
    {
        protected const Int32 WM_EXITSIZEMOVE = 0x0232;
        protected const Int32 WM_ENTERSIZEMOVE = 0x0231;
        protected const Int32 WM_SIZE = 0x0005;
        protected const Int32 WM_WINDOWPOSCHANGING = 0x0046;
        protected const Int32 WM_SYSCOMMAND = 0x0112;

        protected const Int32 SC_MAXIMIZE = 0xF030;
        protected const Int32 SC_MINIMIZE = 0xF020;
        protected const Int32 SC_STORE = 0xF012;
        protected const Int32 SC_RESTORE = 0xF120;

        /// <summary>
        /// 设置视觉Instance的ResizeFlag，防止连续采集过程中拖动窗口造成崩溃
        /// 调用窗口调用 OnResizeEventHandler += MainWindow_OnResizeEventHandler;
        /// 然后如下：
        ///public void SetResizingFlag(bool IsSizing = true)
        ///{
        ///   lock (VisionCommonFunc.SyncData.VisionLock)
        ///   {
        ///      VisionCommonFunc.SyncData.IsNewSizing = IsSizing;
        ///       Console.WriteLine(IsSizing? "Sizing" : "Done");
        ///        if (IsSizing)
        ///           VisionCommonFunc.SyncData.IsOldSizing = IsSizing;
        ///     }
        /// }
        /// </summary>
        public event EventHandler<bool> OnResizeEventHandler;

        public CanResizableVisionWindowWpf()
        {
            SourceInitialized += CanResizableVisionWindowWpf_SourceInitialized;
            StateChanged += CanResizableVisionWindowWpf_StateChanged;
        }

        private void CanResizableVisionWindowWpf_StateChanged(object sender, EventArgs e)
        {
            OnResizeEventHandler?.Invoke(this, false);
        }

        private void CanResizableVisionWindowWpf_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = HwndSource.FromVisual(this) as HwndSource;
            if (source != null)
            {
                // 注册窗口消息处理函数
                source.AddHook(new HwndSourceHook(WinProc));

            }
        }

        private IntPtr WinProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            IntPtr result = IntPtr.Zero;
            switch (msg)
            {
                case WM_ENTERSIZEMOVE:
                    OnResizeEventHandler?.Invoke(this, true);
                    break;

                case WM_EXITSIZEMOVE:
                    OnResizeEventHandler?.Invoke(this, false);
                    break;

                case WM_SYSCOMMAND:
                    if ((int)wParam == SC_MAXIMIZE || (int)wParam == SC_MINIMIZE ||
                        (int)wParam == SC_RESTORE || (int)wParam == SC_STORE)
                    {
                        OnResizeEventHandler?.Invoke(this, true);
                    }
                    break;

            }
            return result;
        }


    }
}
