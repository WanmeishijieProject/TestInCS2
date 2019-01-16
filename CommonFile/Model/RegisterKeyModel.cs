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
        double[] TimeLimitArr = { 2, 7, 15, 30, 9999 };
        public string RegisterKey { get; set; }
        public string RandomKey
        {
            get
            {
                Random r = new Random();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    sb.Append(r.Next(0,9).ToString());
                }
                return sb.ToString();
            }
        }
        public EnumTimeOut TimeLimit { get; set; }
        public long RegistTimeTicks { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RegisterKey);
            sb.Append(RandomKey);
            sb.Append(((int)TimeLimit).ToString());
            sb.Append(string.Format("{0:D20}", RegistTimeTicks));
            return sb.ToString();
        }

        public static RegisterKeyModel FromString(string strRegisterKey)
        {
            var Model = new RegisterKeyModel();
            if (string.IsNullOrEmpty(strRegisterKey))
                throw new Exception("注册码不能为空");
            if (strRegisterKey.Length <= (Model.RandomKey.Length + 21))
                throw new Exception("错误的注册码");
            string timeRegister = strRegisterKey.Substring(strRegisterKey.Length - 20,20);
            string timelimit = strRegisterKey.Substring(strRegisterKey.Length-21, 1);
            string randomStr = strRegisterKey.Substring(strRegisterKey.Length- Model.RandomKey.Length-21, Model.RandomKey.Length);
            string registerKey = strRegisterKey.Substring(0, strRegisterKey.Length - 21 - Model.RandomKey.Length);
            if (Enum.TryParse<EnumTimeOut>(timelimit, out EnumTimeOut timeout))
            {
                if (long.TryParse(timeRegister, out long timeStart))
                {
                    Model.TimeLimit = timeout;
                    Model.RegisterKey = registerKey;
                    Model.RegistTimeTicks = timeStart;
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
                    m1.TimeLimit == m2.TimeLimit;
        }
        public static bool operator !=(RegisterKeyModel m1, RegisterKeyModel m2)
        {
            return !(m1 == m2);
        }

        public bool IsTimeoutFromNow()
        {
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
