using LinearRoomMvtemLib.MvtemCmd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinearRoomMvtemLib.MvtemCmd.CmdDefinen;

namespace LinearRoomMvtemLib.Package
{
    public class PackageOperate
    {
        private Queue<byte> ByteQueue = new Queue<byte>();
        private int PACKAGE_LENGTH=5;
        public EventHandler<PackageRecievedArgs> OnPackageRecieved;
        public bool IsPackageFound { get; set; }
        public bool IsHeaderRecieved { get; set; }
       
        public void AddByte(byte bt)
        {
            if (IsHeaderRecieved == false && bt == (byte)EnumPackageDefine.HEAD)
            {
                ByteQueue.Enqueue(bt);
                IsHeaderRecieved = true;
            }
            else if (IsHeaderRecieved)
            {
                ByteQueue.Enqueue(bt);
                if (ByteQueue.Count == PACKAGE_LENGTH)
                {
                    var list = ByteQueue.ToArray();

                    //Clear memory
                    for(int i=0;i< list.Length;i++)
                        ByteQueue.Dequeue();

                    if (list[PACKAGE_LENGTH - 1] == (byte)EnumPackageDefine.TAIL)
                    {
                        var arg = new PackageRecievedArgs();
                        arg.Cmd = (EnumMvtemCmd)list[1];
                        arg.Value = list[2] * 256 + list[3];
                        arg.RawData = list;

                        OnPackageRecieved?.Invoke(this, arg);
                        IsPackageFound = true;
                    }
                    else
                    {
                        IsHeaderRecieved = false;
                        IsPackageFound = false;
                    }
                }

            }
        }
    }
}
