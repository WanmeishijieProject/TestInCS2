using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonFile.CommonSoftware;

namespace CommonFile.Model
{
    [Serializable]
    public class RegisterKeyModel
    {
        public RegisterKeyModel()
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append(r.Next(0, 9).ToString());
            }
            RandomKey= sb.ToString();

            RegistTimeTicks = DateTime.Now.Ticks;
        }

        double[] TimeLimitArr = { 2, 7, 15, 30, 9999 };
        public string RegisterKey { get; set; }

        //与机器码一致的时间戳
        public string Timestamp5
        {
            get;
            set;
        }

        private string RandomKey
        {
            get;
            set;
        }

        public EnumTimeOut TimeLimit { get; set; }

        /// <summary>
        /// 要与生成机器码的时间保持一致
        /// </summary>
        public long RegistTimeTicks { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RegisterKey);     //x
            RandomKey = RandomKey.Substring(0, 5) + Timestamp5;
            sb.Append(RandomKey);                               //10
            sb.Append(((int)TimeLimit).ToString());             //1
            sb.Append(string.Format("{0:D20}", RegistTimeTicks));//20
            return sb.ToString();
        }

        public static RegisterKeyModel FromString(string strRegisterKey)
        {
            var len = strRegisterKey.Length;
            var Model = new RegisterKeyModel();
            if (string.IsNullOrEmpty(strRegisterKey))
                throw new Exception("注册码不能为空");
            if (strRegisterKey.Length <= (Model.RandomKey.Length + 21))
                throw new Exception("错误的注册码");
            string timeRegister = strRegisterKey.Substring(len - 20,20);
            string timelimit = strRegisterKey.Substring(len - 21, 1);
            string randomStr = strRegisterKey.Substring(len - Model.RandomKey.Length-21, Model.RandomKey.Length);
            string timeTamp = randomStr.Substring(5, 5);
            string registerKey = strRegisterKey.Substring(0, len - 21 - Model.RandomKey.Length);
            if (Enum.TryParse<EnumTimeOut>(timelimit, out EnumTimeOut timeout))
            {
                if (long.TryParse(timeRegister, out long timeStart))
                {
                    Model.TimeLimit = timeout;
                    Model.RegisterKey = registerKey;
                    Model.RegistTimeTicks = timeStart;
                    Model.RandomKey = randomStr;
                    Model.Timestamp5 = timeTamp;
                }
                else
                {
                    throw new Exception("错误的注册码");
                }
            }
            else
            {
                throw new Exception("错误的注册码");
            }
            return Model;
        }

        public static bool operator ==(RegisterKeyModel m1, RegisterKeyModel m2)
        {
            return  m1.RandomKey == m2.RandomKey &&
                    m1.RegisterKey == m2.RegisterKey &&
                    m1.RegistTimeTicks == m2.RegistTimeTicks &&
                    m1.TimeLimit == m2.TimeLimit &&
                    m1.Timestamp5 == m2.Timestamp5;
                    
        }
        public static bool operator !=(RegisterKeyModel m1, RegisterKeyModel m2)
        {
            return !(m1 == m2);
        }

        public bool IsTimeoutFromNow(out double DaysLeftFromRegister)
        {
            var dayLeft = TimeLimitArr[(int)TimeLimit] - TimeSpan.FromTicks(DateTime.Now.Ticks - RegistTimeTicks).TotalDays;
            DaysLeftFromRegister = dayLeft <= 0 ? 0 : dayLeft;
            return TimeSpan.FromTicks(DateTime.Now.Ticks - RegistTimeTicks).TotalDays > TimeLimitArr[(int)TimeLimit];
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
