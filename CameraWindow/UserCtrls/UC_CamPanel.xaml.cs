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

        public int X { get; set; }


        /// <summary>
        /// The <see cref="CamWindowHandle" /> dependency property's name.
        /// </summary>
        public const string CamWindowHandlePropertyName = "CamWindowHandle";
        /// <summary>
        /// Gets or sets the value of the <see cref="WindowHandle" />
        /// property. This is a dependency property.
        /// </summary>
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
        /// <summary>
        /// Identifies the <see cref="CamWindowHandle" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CamWindowHandleProperty = DependencyProperty.Register(
            CamWindowHandlePropertyName,
            typeof(HWindow),
            typeof(UC_CamPanel));


        /// <summary>
        /// The <see cref="OnControlSizeChangedCommand" /> dependency property's name.
        /// </summary>
        public const string OnControlSizeChangedCommandPropertyName = "OnControlSizeChangedCommand";
        /// <summary>
        /// Gets or sets the value of the <see cref="OnControlSizeChangedCommand" />
        /// property. This is a dependency property.
        /// </summary>
        public RelayCommand<string> OnControlSizeChangedCommand
        {
            get
            {
                return (RelayCommand<string>)GetValue(OnControlSizeChangedCommandProperty);
            }
            set
            {
                SetValue(OnControlSizeChangedCommandProperty, value);
            }
        }
        /// <summary>
        /// Identifies the <see cref="OnControlSizeChangedCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnControlSizeChangedCommandProperty = DependencyProperty.Register(
            OnControlSizeChangedCommandPropertyName,
            typeof(RelayCommand<string>),
            typeof(UC_CamPanel)
            );


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> {
                Application.Current.Dispatcher.Invoke(() => {
                    var StartTime = DateTime.Now.Ticks;
                    while (true)
                    {
                        Task.Delay(20).Wait();
                        CamWindowHandle = CamPanel.HalconWindow;
                        Console.WriteLine(CamWindowHandle.Handle);
                        if ((int)CamWindowHandle.Handle != -1 || TimeSpan.FromTicks(DateTime.Now.Ticks-StartTime).TotalSeconds>5)
                            return;
                    }
                });
            });
            
        }
        private void CamPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (OnControlSizeChangedCommand != null)
                OnControlSizeChangedCommand.Execute(this.Name);
        }
    }
}
