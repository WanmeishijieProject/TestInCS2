using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GPAP.Model.AbbCmd;
using GPAP.TcpClient;
using GPAP.ViewModel;
using SuperSocket.ClientEngine;
using static GPAP.Definetions;

namespace GPAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , INotifyPropertyChanged
    {
        private int IndexCalib = 1;
        private EasyClient Client;
        private MsgMoveToPos CmdMoveToPos = new MsgMoveToPos();
        private MsgCalibrate CmdCalib = new MsgCalibrate();
        private MsgStopRobot CmdStop = new MsgStopRobot();
        private MsgRotate CmdRotate = new MsgRotate();
        private MyReceiveFilter Filter = new MyReceiveFilter();
        object _lock = new object();
        bool _isNotBusy;
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=> {
                try
                {
                    Client = new EasyClient();
                    Client.Initialize(Filter, request =>
                    {
                        // handle the received request
                        Console.WriteLine(request.Body);

                    });
                    // Connect to the server
                    Filter.OnPackageReceived += OnMessageReceived;
                    Connect();
                    IsNotBusy = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
           
        }
        private void OnMessageReceived(object sender,string Msg)
        {
            var type=RobotCmdBase.GetCmdTypeFromString(Msg);
            RobotCmdBase cmd = null;
            switch (type)
            {
                case EnumRobotCmd.Calibration:
                    cmd = CmdCalib;
                    break;
                case EnumRobotCmd.MoveToPos:
                    cmd = CmdMoveToPos;
                    break;
                case EnumRobotCmd.Rotate:
                    cmd = CmdRotate;
                    break;
                case EnumRobotCmd.StopRobot:
                    cmd = CmdStop;
                    break;
                default:
                    break;
            }
            if (cmd != null)
            {
                cmd.O_ReturnObj = Msg;
                cmd.SetMessageState();
                IsNotBusy = true;
            }
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=> {
                if (Client.IsConnected)
                {
                    CmdMoveToPos.I_X = 10;
                    CmdMoveToPos.I_Y = 10;
                    CmdMoveToPos.I_Z = 10;
                    CmdMoveToPos.I_Speed = EnumRobotSpeed.V10;
                    CmdMoveToPos.I_Tool = EnumRobotTool.Tool0;
                }
                var cmdBak = ExcuteCmd(CmdMoveToPos);
            });
        }

        private async void BtnCalib_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=> {
                CmdCalib.Index = IndexCalib;
                var ret = ExcuteCmd(CmdCalib) as MsgCalibrate;
                if (ret != null)
                {
                    Console.WriteLine($"x坐标:{ret.Q_X},Y坐标:{ret.Q_Y},Z坐标:{ret.Q_Z}");
                    if (IndexCalib++ >= 9)
                        IndexCalib = 1;
                }
            });
           
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            var ret=ExcuteCmd(CmdStop) as MsgStopRobot;
        }

        /// <summary>
        /// 阻塞函数
        /// </summary>
        /// <param name="cmd">返回的也是RobotCmdBase类型</param>
        /// <returns></returns>
        private RobotCmdBase ExcuteCmd(RobotCmdBase cmd)
        {
            try
            {
                byte[] RecvBuffer = new byte[256];
                if (!Client.IsConnected)
                {
                    Connect();
                }
                else
                {
                    cmd.ResetMessageState();
                    lock (_lock)
                    {
                        IsNotBusy = false;
                        Client.Send(cmd.ToByteArray());
                    }
                    RobotCmdBase cmdClone = cmd.GenEmptyCmd() as RobotCmdBase;
                    if (cmd.WaitCmdRecved(30000))
                    {
                        if(cmd.O_ReturnObj!=null)
                            cmdClone.FromString(cmd.O_ReturnObj.ToString());
                        else
                            throw new Exception($"Error Msg received:{cmd.I_Cmd.ToString()}");
                    }
                    else
                    {
                        throw new Exception($"Timeout for waiting for Message: {cmd}");
                    }
                    return cmdClone;
                }
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        private async void BtnRotate_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=> {
                try
                {
                    CmdRotate.I_Speed = EnumRobotSpeed.V5;
                    CmdRotate.I_Tool = EnumRobotTool.Tool1;
                    CmdRotate.Z = 100;
                    var ret = ExcuteCmd(CmdRotate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
         
        }

        private async void BtnRotateBack_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=> {
                try
                {
                    CmdRotate.I_Speed = EnumRobotSpeed.V5;
                    CmdRotate.I_Tool = EnumRobotTool.Tool1;
                    CmdRotate.Z = -100;
                    var ret = ExcuteCmd(CmdRotate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
          
        }

        private async void Connect()
        {
            var Connected = await Client.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.125.1"), 4000));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public bool IsNotBusy
        {
            get { return _isNotBusy; }
            set {
                if (_isNotBusy != value)
                {
                    _isNotBusy = value;
                    RaisePropertyChanged();
                }
            }
        }







        private async void BtnTestAsync_Click(object sender, RoutedEventArgs e)
        {
            BtnTestAsync.Content = "测试前";
            var task1 = Task.Run(()=> {
                try
                {
                    Thread.Sleep(5000);
                    int y = 0;
                    int x = 5 / y;
                    return "Button'Result";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            });
            task1.ConfigureAwait(false);    //不需要操作UI，也就不需要切换同步上下文，这时候设置为false可以执行的更快
            var t = await task1;

            task1.ContinueWith(tt=> {
                Thread.Sleep(10000);
                if (tt.Result.Contains("Error"))
                {
                    Console.WriteLine("TTTTTTT1");
                    return 4;
                }
                else
                {
                    Console.WriteLine("TTTTTTT2");
                    return 5;
                }
            });
            
            BtnTestAsync.Content = t;

            //BeginCallA

        }



        //异步调用
        private Func<string, string, int> foo = A;

        

        void BeginCallA(string AA, string BB, AsyncCallback callback, object state)
        {
            //前面是参数，后面是返回值
            
            foo.BeginInvoke(AA, BB,callback,state);
        }

        void EndCallA(IAsyncResult result)
        {
            foo.EndInvoke(result);
        }

        private static int A(string a, string b)
        {
            Thread.Sleep(4000);
            Console.WriteLine($"{a}&{b}");
            return 4;
        }

       
    }
}