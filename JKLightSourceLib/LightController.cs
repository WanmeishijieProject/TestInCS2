using JKLightSourceLib.Command;
using JKLightSourceLib.Package;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib
{
    
    public class JKLightSource
    {
        private SerialPort Comport=null;
        private object _lock = new object();

        public JKLightSource(int ComportNO, int Baudrate)
        {
            Comport = new SerialPort();
            Comport.PortName = $"COM{ComportNO}";
            Comport.BaudRate = Baudrate;
            Comport.ReadTimeout = 1000;
            Comport.WriteTimeout = 1000;
            Comport.DataBits = 8;
            Comport.StopBits = StopBits.One;
            Comport.Parity = Parity.None;
        }
        public void Open()
        {
            if (Comport.IsOpen)
                Comport.Close();
            Comport.Open();
        }

        public void Close()
        {
            Comport.Close();
        }

        public UInt16 ReadValue(EnumChannel channel)
        {
            var Cmd = new CommandReadValue()
            {
                Channel = channel,
            };
            SendCmd(Cmd);
            Recieve(out RxPackage pkg);
            Cmd.FromByteArray(pkg.RawData);
            return Cmd.QChannelValue;
           
        }

        public void WriteValue(EnumChannel Channel, UInt16 Value)
        {
            var Cmd = new CommandWriteValue()
            {
                Channel = Channel,
                Value = Value,
            };
            SendCmd(Cmd);
            Recieve(out RxPackage pkg);
        }

        public void OpenChannelLight(EnumChannel Channel, UInt16 InitValue)
        {
            var Cmd = new CommandOpenLight()
            {
                Value = InitValue,
                Channel = Channel
            };
            SendCmd(Cmd);
            Recieve(out RxPackage pkg);
        }

        public void CloseChannelLight(EnumChannel Channel)
        {
            var Cmd = new CommandCloseLight()
            {
                Channel = Channel
            };
            SendCmd(Cmd);
            Recieve(out RxPackage pkg);
        }
        private void SendCmd(CommandBase Cmd)
        {
            lock (_lock)
            {
                var data = Cmd.ToByteArray();
                Comport.Write(data,0,data.Length);
            }
        }

        private void Recieve(out RxPackage pkg, int TimeOut=1000)
        {
            pkg = new RxPackage()
            {
                PackageSize = 1
            };
            var StartTime = DateTime.Now.Ticks;
            int len = Comport.BytesToRead;
            for (int i = 0; i < len; i++)
                pkg.AddByte((byte)Comport.ReadByte());
            while (true)
            {
                if (TimeSpan.FromTicks((DateTime.Now.Ticks - StartTime)).TotalSeconds >= (TimeOut / 1000.0))
                    throw new Exception("time for waiting result");
                if (pkg.IsPackageFind)
                    break;
            }
        }
       

    }
}
