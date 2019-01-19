using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFile.Model
{
    public class MachineNumberModel
    {
        public MachineNumberModel()
        {
            Timestamp20 = DateTime.Now.Ticks;
            var strTimestamp20 = String.Format("{0:D20}", Timestamp20);
            Timestamp5= strTimestamp20.Substring(15, 5);

            //
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append(r.Next(0, 9).ToString());
            }
            RandomKey10 = sb.ToString();

        }
        public string HardwareID { get; set; }

        /// <summary>
        /// 生成机器码的当前时间
        /// </summary>
        public long Timestamp20
        {
            get;
            private set;
        }


        private string RandomKey10
        {
            get;
            set;
        }

        public string Timestamp5 {
            get;
            private set;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HardwareID);
            var strTimestamp20 = String.Format("{0:D20}", Timestamp20);
            sb.Append(strTimestamp20);      //20
            sb.Append(RandomKey10);         //10
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
                RandomKey10= randomKey10,
                Timestamp20= nTimestamp20,
            };
        }
    }
}
