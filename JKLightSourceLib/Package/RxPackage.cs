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
        public bool IsHeaderFind { get; private set; }
        public bool IsPackageFind { get; private set; }
        public byte[] RawData { private set; get; }
        private byte PACKAGE_HEADER = (byte)'#';
        public int PackageSize { private get; set; }
        public event EventHandler<PackageRecieveArgs> OnPackageRecieved;
        public RxPackage()
        {
            IsHeaderFind = false;
            IsPackageFind = false;
        }
        public void AddByte(byte bt)
        {
            if (!IsHeaderFind && bt == PACKAGE_HEADER)
            {
                IsHeaderFind = true;
                DataRecieveQueue.Enqueue(bt);
                if (PackageSize == 1)
                {
                    IsPackageFind = true;
                    return;
                }
            }
            else if (IsHeaderFind)
            {
                DataRecieveQueue.Enqueue(bt);
                if (DataRecieveQueue.Count == PackageSize)
                {

                    RawData = DataRecieveQueue.ToArray();
                    var CalcXOR = CheckXor.GetStringXOR(RawData, 0, RawData.Length - 2);

                    //Clear queue
                    foreach (var it in DataRecieveQueue)
                        DataRecieveQueue.Dequeue();

                    if (RawData[PackageSize - 1] == CalcXOR[1] && RawData[PackageSize - 2] == CalcXOR[0])
                    {
                       
                        OnPackageRecieved?.Invoke(this, new PackageRecieveArgs()
                        {
                            RawData = this.RawData,
                            Cmd=(EnumCommand)RawData[1]
                        });
                        IsPackageFind = true;
                    }
                    else
                    {
                        IsHeaderFind = false;
                        IsPackageFind = false;
                    }
                }
            }
        }
      
    }
}
