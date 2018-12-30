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

namespace CameraWindow.UserCtrls
{
    /// <summary>
    /// UC_CamPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UC_CamPanel : UserControl
    {
        public UC_CamPanel()
        {
            InitializeComponent();
        }
        public string StringDisplay { get; set; }

        private void HWindowControlWPF_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Down");
        }
    }
}
