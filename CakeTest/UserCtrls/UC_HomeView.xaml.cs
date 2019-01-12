using CakeTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CakeTest.UserCtrls
{
    /// <summary>
    /// UC_HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class UC_HomeView : UserControl
    {
        public UC_HomeView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> {
                Task.Delay(500).Wait();
                Application.Current.Dispatcher.Invoke(()=> {
                    (DataContext as MainViewModel).WindowHandle = Cam1.HalconWindow;
                });
            });
            
        }
    }
}
