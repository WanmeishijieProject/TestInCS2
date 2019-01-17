using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFile.Model
{
    public class MachineNumberModel
    {
        public string HardwareID { get; set; }
        public long Timestamp20
        {
            get { return DateTime.Now.Ticks; }
        }
        private string RandomKey10
        {
            get
            {
                Random r = new Random();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    sb.Append(r.Next(0, 9).ToString());
                }
                return sb.ToString();
            }
        }

        public string Timestamp5 { get; private set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HardwareID);
            var strTimestamp = String.Format("{0:D20}", Timestamp20);
            sb.Append(strTimestamp);
            sb.Append(RandomKey10);
            return sb.ToString();
        }

        public static MachineNumberModel FromString(string MachineKey)
        {
            var len = MachineKey.Length;
            if (len <= 30)
                throw new Exception("错误的机器码格式");

            var randomKey10 = MachineKey.Substring(len - 10, 10);
            var timestamp20 = MachineKey.Substring(len - 30, 20);
            var hardwareID = MachineKey.Substring(0, len - 30);
            if (!long.TryParse(timestamp20, out long nTimestamp20))
                throw new Exception("错误的机器码格式");
            return new MachineNumberModel()
            {
                HardwareID = hardwareID,
                Timestamp5 = timestamp20.Substring(15, 5),
            };
        }
    }
}
