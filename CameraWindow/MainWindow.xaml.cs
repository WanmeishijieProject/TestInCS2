using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using CameraWindow.ViewModel;

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
    }
}