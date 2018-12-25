using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using LinearRoomMvtemLib.MvtemCmd;
using LinearRoomMvtemLib.Package;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib
{
    public class MvtemZoom
    {
        #region Field
        private SerialPort Comport = new SerialPort();
        private int PortNo, Baudrate;
        private object ComportLock = new object();
        private const int DEFAULT_TIMEOUT = 3000;
        private PackageOperate PackageOp = new PackageOperate();


        CmdReadCurCurrentState cmdReadCurCurrentState = new CmdReadCurCurrentState();
        CmdReadCurDesPuseError cmdReadCurDesPuseError = new CmdReadCurDesPuseError();
        CmdReadCurMotionState cmdReadCurMotionState = new CmdReadCurMotionState();
        CmdReadCurTemState cmdReadCurTemState = new CmdReadCurTemState();
        CmdReadCurZoomRate cmdReadCurZoomRate = new CmdReadCurZoomRate();
        CmdReadHomePuseError cmdReadHomePuseError = new CmdReadHomePuseError();
        CmdReadPuseError cmdReadPuseError = new CmdReadPuseError();

        public event EventHandler<PuseChangedArgs> OnPuseChanged;
        public event EventHandler<MotionStateChangedArgs> OnStateChanged;
        #endregion


        #region API
        public MvtemZoom(int PortNo, int Baudrate)
        {
            this.PortNo = PortNo;
            this.Baudrate = Baudrate;
            Comport.PortName = $"COM{PortNo}";
            Comport.BaudRate = Baudrate;
            Comport.Parity = Parity.None;
            Comport.StopBits = StopBits.One;
            Comport.DataBits = 8;
            Comport.ReadTimeout = 1000;
            Comport.WriteTimeout = 1000;
            PackageOp.OnPackageRecieved += PackageRecieved;
        }
        public bool Open()
        {
            if (Comport.IsOpen)
                Comport.Close();
            Comport.Open();
            Comport.DataReceived += Comport_DataReceived;
            return Comport.IsOpen;
        }

 

        public void Home()
        {
            lock (ComportLock)
            {
                Send(new CmdHome());
            }
        }

        public void ZoomRandom(double ZoomRarte)
        {
            lock (ComportLock)
            {
                Send(new CmdZoomRandom() {
                    ZoomRate = ZoomRarte
                });
            }
        }

        public void AddZoomStep(double ZoomStep)
        {
            lock (ComportLock)
            {
                Send(new CmdAddZoomStep()
                {
                    Step = ZoomStep
                });
            }
        }

        public void SubZoomStep(double ZoomStep)
        {
            lock (ComportLock)
            {
                Send(new CmdSubZoomStep()
                {
                    Step = ZoomStep
                });
            }
        }

        public void StartZoomLoops(Int16 Count)
        {
            lock (ComportLock)
            {
                Send(new CmdZoomLoops()
                {
                    LoopCount = Count
                });
            }
        }

        public void StopZoomLoops(Int16 Count)
        {
            lock (ComportLock)
            {
                Send(new CmdStopZoomLoops());
            }
        }

        public double ReadCurZoomRate()
        {
            lock (ComportLock)
            {
                Send(cmdReadCurZoomRate);
                cmdReadCurZoomRate.WaitResult(DEFAULT_TIMEOUT);
                return cmdReadCurZoomRate.QCurZoomRate;
            }
            
        }
        public UInt16 ReadCurDesPuseError()
        {
            lock (ComportLock)
            {          
                Send(cmdReadCurDesPuseError);
                cmdReadCurDesPuseError.WaitResult(DEFAULT_TIMEOUT);
                return cmdReadCurDesPuseError.QCurDesPuseError;

            }
        }

        [Obsolete("此方法暂时不提供调用",true)]
        public EnumCurrentState ReadCurCurrentState()
        {
            lock (ComportLock)
            {
                Send(cmdReadCurCurrentState);
                cmdReadCurCurrentState.WaitResult(DEFAULT_TIMEOUT);
                return cmdReadCurCurrentState.QCurCurrentState;
            }
        }

        public EnumMotionState ReadCurMotionState()
        {
            lock (ComportLock)
            {
                Send(cmdReadCurMotionState);
                cmdReadCurMotionState.WaitResult(DEFAULT_TIMEOUT);
                return cmdReadCurMotionState.QMotionState;
            }
        }

        [Obsolete("此方法暂时不提供调用",true)]
        public EnumTemState ReadCurTempState()
        {
            lock (ComportLock)
            {
                Send(cmdReadCurTemState);
                cmdReadCurTemState.WaitResult(DEFAULT_TIMEOUT);
                return cmdReadCurTemState.QTemState;
            }
        }
        #endregion


        #region private
        private void Send(MvtemCmdBase Cmd)
        {
            var ByteArray = Cmd.ToByteArray();
            Comport.Write(ByteArray, 0, ByteArray.Length);
        }

        private void Comport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int len = Comport.BytesToRead;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                    PackageOp.AddByte((byte)Comport.ReadByte());
            }
        }

        /// <summary>
        /// 接收到的包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PackageRecieved(object sender, PackageRecievedArgs args)
        {
            switch (args.Cmd)
            {
                case EnumMvtemCmd.ResCurrentState:
                    cmdReadCurCurrentState.FromByteArray(args.RawData);
                    OnStateChanged?.Invoke(this, new MotionStateChangedArgs() {StateType= EnumState.CurrentState, State=(EnumCurrentState)args.RawData[3]});
                    break;
                case EnumMvtemCmd.ResCurZoomRate:
                    cmdReadCurZoomRate.FromByteArray(args.RawData);
                    break;
                case EnumMvtemCmd.ResPuseError:
                    cmdReadPuseError.FromByteArray(args.RawData);
                    break;
                case EnumMvtemCmd.ResMotionState:
                    cmdReadCurMotionState.FromByteArray(args.RawData);
                    OnStateChanged?.Invoke(this, new MotionStateChangedArgs() {StateType= EnumState.MotionState, State = (EnumMotionState)args.RawData[3] });
                    break;
                case EnumMvtemCmd.ResPuseErrorToDes:
                    cmdReadCurDesPuseError.FromByteArray(args.RawData);
                    OnPuseChanged?.Invoke(this, new PuseChangedArgs() {
                        CurrentValue = (Int16)args.Value,
                        PuseType=EnumPuseChangedType.ToDesPuseError
                    });
                    break;
                case EnumMvtemCmd.ResPuseErrorToHome:
                    cmdReadHomePuseError.FromByteArray(args.RawData);
                    OnPuseChanged?.Invoke(this, new PuseChangedArgs()
                    {
                        CurrentValue = (Int16)args.Value,
                        PuseType = EnumPuseChangedType.HomePuseError
                    });
                    break;
                case EnumMvtemCmd.ResTemState:
                    cmdReadCurTemState.FromByteArray(args.RawData);
                    OnStateChanged?.Invoke(this, new MotionStateChangedArgs() {StateType= EnumState.TemState, State = (EnumTemState)args.RawData[3] });
                    break;
                default:
                    break;

            }
        }
        #endregion

    }
}
