using JKLightSourceLib.Check;
using JKLightSourceLib.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKLightSourceLib.Package
{
    public class RxPackage
    {
        Queue<byte> DataRecieveQueue = new Queue<byte>();
        bool IsHeaderFind = false;
        private byte PACKAGE_HEADER = (byte)'#';
        private int PACKAGE_SIZE = 8;
        public event EventHandler<PackageRecieveArgs> OnPackageRecieved;
        public void AddByte(byte bt)
        {
            if (!IsHeaderFind && bt == PACKAGE_HEADER)
            {
                IsHeaderFind = true;
                DataRecieveQueue.Enqueue(bt);
            }
            else if (IsHeaderFind)
            {
                DataRecieveQueue.Enqueue(bt);
                if (DataRecieveQueue.Count == PACKAGE_SIZE)
                {
                   
                    var data = DataRecieveQueue.ToArray();
                    var CalcXOR = CheckXor.GetStringXOR(data, 0, data.Length - 2);

                    //Clear queue
                    foreach (var it in DataRecieveQueue)
                        DataRecieveQueue.Dequeue();

                    if (data[PACKAGE_SIZE - 1] == CalcXOR[1] && data[PACKAGE_SIZE - 2] == CalcXOR[0])
                    {
                       
                        OnPackageRecieved?.Invoke(this, new PackageRecieveArgs()
                        {
                            RawData = data,
                            Cmd=(EnumCommand)data[1]
                        });
                    }
                    else
                    {
                        IsHeaderFind = false;
                    }
                }
            }
        }
    }
}
