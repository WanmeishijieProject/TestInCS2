using CameraWindow.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CameraWindow.UserCtrls
{
    /// <summary>
    /// WindowImageSetting.xaml 的交互逻辑
    /// </summary>
    public partial class WindowImageSetting : Window
    {
        SettingViewModel SettingVM = SimpleIoc.Default.GetInstance<SettingViewModel>();
        public WindowImageSetting()
        {
            InitializeComponent();
        }

        private void ListBox_ValabileOperator_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var op=(sender as ListBox).SelectedValue;
            if (op != null)
            {
                SettingVM.AvalibleOperatorCommand.Execute(op.ToString());
            }
        }
    }
}
