using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using CakeTest.UserCtrls;
using CakeTest.ViewModel;

namespace CakeTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        string _daysLeft;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Closing += (s, e) => ViewModelLocator.Cleanup();
                Register();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} : {ex.StackTrace}");
                Close();
            }
           
        }

        public string DaysLeft
        {
            get { return _daysLeft; }
            set
            {
                if (_daysLeft != value)
                {
                    _daysLeft = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("DaysLeft"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RibbonPopupMenuButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (File.Exists("zrd.data"))
            {
                File.Delete("zrd.data");
            }
        }

        private void Register()
        {
            int Days = 0;
            if (!Window_Register.CheckFile(out Days))
            {
                Window_Register RegisterWindow = new Window_Register();
                RegisterWindow.ShowDialog();
                if (!RegisterWindow.IsCheckSuccess)
                {
                    MessageBox.Show("注册失败", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    Close();
                }
                else
                {
                    Days = RegisterWindow.DaysLeft;
                }

            }
            if (Days > 100)
                DaysLeft = "已注册";
            else
                DaysLeft = $"{Days}天试用期";
        }
    }
}