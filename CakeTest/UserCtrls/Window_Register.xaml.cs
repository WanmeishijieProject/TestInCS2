using Com.Netframe.Computer.Handware;
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
using System.Windows.Shapes;
using CommonFile;
using System.ComponentModel;

namespace CakeTest.UserCtrls
{
    /// <summary>
    /// Window_Register.xaml 的交互逻辑
    /// </summary>
    public partial class Window_Register : Window , INotifyPropertyChanged
    {
        private string _machineNum = "";
        private string _registerKey = "";
        static CommonSoftware software = new CommonSoftware();

        public Window_Register()
        {
            IsCheckSuccess = false;
            InitializeComponent();
            MachineNum = software.GetMachineKey();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCheckSuccess { get; private set; }

        public string MachineNum
        {
            get {
                return _machineNum;
            }
            set {
                if (_machineNum != value)
                {
                    _machineNum = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("MachineNum"));
                }
            }
        }

        public string RegisterKey
        {
            get
            {
                return _registerKey;
            }
            set
            {
                if (_registerKey != value)
                {
                    _registerKey = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegisterKey"));
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsCheckSuccess = software.CheckRegisterKey(RegisterKey);
                MessageBox.Show("注册成功");
                Close();
            }
            catch
            {
                IsCheckSuccess = false;
            }
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static bool CheckFile(out double DaysLeft)
        {
        
            var Ret= software.CheckFile(out DaysLeft);
            return Ret;
        }

        public double DaysLeft { get { return Math.Round(software.DaysLeft,2); } private set { } }



    }
}
