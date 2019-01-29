using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using CameraWindow.ViewModel;
using VisionLib;
using ResizableWIndow;
namespace CameraWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CanResizableVisionWindowWpf
    {
        public MainWindow()
        {
            InitializeComponent();
            OnResizeEventHandler += MainWindow_OnResizeEventHandler;
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void MainWindow_OnResizeEventHandler(object sender, bool e)
        {
            (DataContext as MainViewModel).SetResizingFlag(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CommandWindowLoaded.Execute(null);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as MainViewModel).CommandCloseWindow.Execute(null);
        }


    
    }
}