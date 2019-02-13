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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as MainViewModel).CommandCloseWindow.Execute(null);
        }

    }
}