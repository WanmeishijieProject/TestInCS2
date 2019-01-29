using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using VisionLib;

namespace WPFVidwo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        HalconVision Vision = HalconVision.Instance;
        CameraInfoModel Cam0 = null;
        BitmapSource _bitmapSource;
        Task t;
        CancellationTokenSource cts;
        public MainViewModel()
        {
            var Cam = Vision.FindCamera(VisionDefinitions.EnumCamType.GigEVision, new List<string>() { "CamTop"}, out List<string> ErrorList);
            Cam0 = Cam[0];
        }
        ~MainViewModel()
        {
            Vision.CloseAllCamera();
        }
        public RelayCommand CommandPlay
        {
            get { return new RelayCommand(()=> {
                Vision.OpenCam(Cam0.CamID);
                cts = new CancellationTokenSource();
                Task.Run(()=> {
                    while (!cts.IsCancellationRequested)
                    {
                        long Start = DateTime.Now.Ticks;
                        HObject ImagObj = Vision.GrabImage(Cam0);
                        GenertateGrayBitmap(ImagObj, out Bitmap bitmap);
                        Console.WriteLine($"耗时{TimeSpan.FromTicks(DateTime.Now.Ticks-Start).TotalSeconds}");
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            bitmapSource = BitmapToBitmapSource(bitmap);
                        });
                    }
                },cts.Token);
            }); }
        }

        public RelayCommand CommandStop
        {
            get
            {
                return new RelayCommand(() => {
                    cts.Cancel();
                   
                });
            }
        }
        

        public BitmapSource bitmapSource
        {
            get { return _bitmapSource; }
            set {
                if (_bitmapSource != value)
                {
                    _bitmapSource = value;
                    RaisePropertyChanged();
                }
            }
        }


        [DllImport("Kernel32.dll")]
        internal static extern void CopyMemory(int dest, int source, int size);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        private void GenertateGrayBitmap(HObject image, out Bitmap res)
        {
            HTuple hpoint, type, width, height;
            const int Alpha = 255;
            int[] ptr = new int[2];
            HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);

            res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = res.Palette;
            for (int i = 0; i <= 255; i++)
            {
                pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
            }
            res.Palette = pal;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            ptr[0] = bitmapData.Scan0.ToInt32();
            ptr[1] = hpoint.I;
            if (width % 4 == 0)
                CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
            else
            {
                for (int i = 0; i < height - 1; i++)
                {
                    ptr[1] += width;
                    CopyMemory(ptr[0], ptr[1], width * PixelSize);
                    ptr[0] += bitmapData.Stride;
                }
            }
            res.UnlockBits(bitmapData);

        }
        private BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetHbitmap();
            BitmapSource result =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //release resource
            DeleteObject(ptr);
            return result;
        }

        
    }
}