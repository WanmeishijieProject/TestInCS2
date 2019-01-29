using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using WPFVidwo.ViewModel;

namespace WPFVidwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// 

        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            
        }
    }
}