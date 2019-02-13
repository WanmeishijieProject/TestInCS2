using GalaSoft.MvvmLight.Command;
using HalconDotNet;
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

    public partial class UC_CamPanel : UserControl
    {
        public UC_CamPanel()
        {
            InitializeComponent();
        }
        public string StringDisplay { get; set; }

        public const string CamWindowHandlePropertyName = "CamWindowHandle";
        public HWindow CamWindowHandle
        {
            get
            {
                return (GetValue(CamWindowHandleProperty) as HWindow);
            }
            set
            {
                SetValue(CamWindowHandleProperty, value);
            }
        }
        public static readonly DependencyProperty CamWindowHandleProperty = DependencyProperty.Register(
            CamWindowHandlePropertyName,
            typeof(HWindow),
            typeof(UC_CamPanel));


        //public const string OnControlSizeChangedCommandPropertyName = "OnControlSizeChangedCommand";
        //public RelayCommand<string> OnControlSizeChangedCommand
        //{
        //    get
        //    {
        //        return (RelayCommand<string>)GetValue(OnControlSizeChangedCommandProperty);
        //    }
        //    set
        //    {
        //        SetValue(OnControlSizeChangedCommandProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty OnControlSizeChangedCommandProperty = DependencyProperty.Register(
        //    OnControlSizeChangedCommandPropertyName,
        //    typeof(RelayCommand<string>),
        //    typeof(UC_CamPanel)
        //    );


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> {
                Application.Current.Dispatcher.Invoke(() => {
                    var StartTime = DateTime.Now.Ticks;
                    while (true)
                    {
                        Task.Delay(100).Wait();
                        CamWindowHandle = CamPanel.HalconWindow;
                        Console.WriteLine(CamWindowHandle.Handle);
                        if ((int)CamWindowHandle.Handle != -1 || TimeSpan.FromTicks(DateTime.Now.Ticks-StartTime).TotalSeconds>5)
                            return;
                    }
                });
            });
            
        }
        //private void CamPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (OnControlSizeChangedCommand != null)
        //        OnControlSizeChangedCommand.Execute(this.Name);
        //}
    }
}
