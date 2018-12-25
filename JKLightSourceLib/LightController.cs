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
        CommandReadValue CmdReadValue = null;
        RxPackage PackageOperate = new RxPackage();

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
            PackageOperate.OnPackageRecieved += PackageOperate_OnPackageRecieved;
            Comport.DataReceived += Comport_DataReceived;
        }


        public void Close()
        {
            Comport.Close();
        }

        public UInt16 ReadValue(EnumChannel channel)
        {
            CmdReadValue = new CommandReadValue()
            {
                 Channel=channel
            };
            SendCmd(CmdReadValue);
            CmdReadValue.WaitForResult();
            return CmdReadValue.QChannelValue;
           
        }

        public void WriteValue(EnumChannel Channel, UInt16 Value)
        {
            SendCmd(new CommandWriteValue() {
                 Channel=Channel,
                 Value=Value,
            });
        }

        public void OpenChannelLight(EnumChannel Channel, UInt16 InitValue)
        {
            SendCmd(new CommandOpenLight() {
                 Value=InitValue,
                 Channel=Channel
            });
        }

        public void CloseChannelLight(EnumChannel Channel)
        {
            SendCmd(new CommandCloseLight() {
                Channel=Channel
            });
        }
        private void SendCmd(CommandBase Cmd)
        {
            lock (_lock)
            {
                var data = Cmd.ToByteArray();
                Comport.Write(data,0,data.Length);
            }
        }

        private void Comport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int len = Comport.BytesToRead;
            for (int i = 0; i < len; i++)
                PackageOperate.AddByte((byte)Comport.ReadByte());
        }

        private void PackageOperate_OnPackageRecieved(object sender, PackageRecieveArgs e)
        {
            switch (e.Cmd)
            {
                case EnumCommand.GetLightValue:
                    CmdReadValue.FromByteArray(e.RawData);
                    CmdReadValue.SetCmdState();
                    break;
                default:
                    break;
            }
        }
    }
}
