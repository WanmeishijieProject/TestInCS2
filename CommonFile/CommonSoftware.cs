using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Com.Netframe.Computer.Handware;
namespace CommonFile
{

    public class CommonSoftware
    {
        private string UserKey = "";
        private HardwareInfo info = new HardwareInfo();
#if REG
        private RegEditOp RegEditor = new RegEditOp();
#endif

        private string LookTable = "PpVvWwXxQqRrSsTtUuJjKk2345LlMmNnAaBbCcDdEeFfGgHhIiOoYyZz016789!@#$%^&*()";
        string FilePath = "zrd.data";
        int[] TimeArr = {2,7,15,30,9999 };
        public enum EnumTimeOut
        {
            TwoDay,
            OneWeek,
            HalfMonth,
            OneMonth,
            Free,
        }

       

        public CommonSoftware()
        {
            
        }

        public string GetMachineKey()
        {
            UserKey = info.GetCpuID();
            UserKey += info.GetDiskID();
            StringBuilder sb = new StringBuilder();
            foreach (var it in UserKey)
            {
                if (LookTable.Contains(it))
                    sb.Append(it);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="MachineKey"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public string GenPswd(string MachineKey,EnumTimeOut Timeout)
        {
            Random r = new Random(DateTime.Now.Second);
            int x = (int)MachineKey[MachineKey.Length - 1];
            int Seed = x % 10;
            StringBuilder sb = new StringBuilder();
            foreach (var it in MachineKey)
            {
                int i=LookTable.IndexOf(it)+Seed;
                sb.Append(i.ToString());
            }
            sb.Append(((int)Timeout).ToString());
            sb.Append(r.Next(1, 9).ToString());
            sb.Append(r.Next(1, 9).ToString());
            sb.Append(r.Next(1, 9).ToString());
            sb.Append(r.Next(1, 9).ToString());
            return sb.ToString();
        }

        public bool CheckPswd(string RegisterKey)
        {
            //去掉后面的随机数     
            EnumTimeOut TimeOutType = EnumTimeOut.TwoDay;
            var CalcKwy = GenPswd(GetMachineKey(), EnumTimeOut.TwoDay);
            if (RegisterKey.Length!= CalcKwy.Length)
                throw new Exception("请输入正确的注册码");
            if (CalcKwy.Substring(0, CalcKwy.Length - 5) != RegisterKey.Substring(0, RegisterKey.Length - 5))
                throw new Exception("请输入正确的注册码");

            RegisterKey = RegisterKey.Substring(0, RegisterKey.Length - 4);

            if (int.TryParse(RegisterKey.Substring(RegisterKey.Length - 1, 1), out int nTimeout))
            {
                if (Enum.IsDefined(typeof(EnumTimeOut), nTimeout))
                {
                    TimeOutType = (EnumTimeOut)nTimeout;
                    var Content = $"{RegisterKey.Substring(0,RegisterKey.Length - 1)}&{(int)TimeOutType}&{DateTime.Now.Ticks}";
                    File.WriteAllText(FilePath, Content);
#if REG
                    RegEditor.WriteRegisterValue(Content);
#endif
                    this.DaysLeft = TimeArr[nTimeout];
                    return true;
                }
                else
                    throw new Exception("请输入正确的注册码");
            }
            else
                throw new Exception("请输入正确的注册码");
        }

        public bool CheckFile(out int DaysLeft)
        {
            DaysLeft = 0;
#if REG
            if (File.Exists(FilePath) && RegEditor.IsRegistryKeyExist("smx"))
#endif
            {
#if TEST
                MessageBox.Show("1");
#endif
                var content = File.ReadAllText(FilePath);
#if REG
                var Msg = RegEditor.ReadRegisterValue();
#endif
#if TEST
                MessageBox.Show(Msg);
#endif
#if REG
                if (content == Msg)
#endif
                {
                    var List = content.Split('&');
                    if (List.Count() == 3)
                    {
#if TEST
                        MessageBox.Show("2");
#endif
                        var regKey = List[0];
                        var timeout = List[1];
                        var starttime = List[2];
                        var CalcKwy = GenPswd(GetMachineKey(), EnumTimeOut.TwoDay);

                        CalcKwy = CalcKwy.Substring(0, CalcKwy.Length - 5);
#if TEST
                        MessageBox.Show($"{CalcKwy}\n{regKey}");
#endif
                        if (CalcKwy == regKey)
                        {
#if TEST
                            MessageBox.Show("3");
#endif
                            if (long.TryParse(starttime, out long nStartTicks))
                            {
#if TEST
                                MessageBox.Show("4");
#endif
                                if (int.TryParse(timeout, out int nTimeout))
                                {
#if TEST
                                    MessageBox.Show("5");
#endif
                                    var Days = TimeSpan.FromTicks(DateTime.Now.Ticks - nStartTicks).TotalDays;
                                    DaysLeft = TimeArr[nTimeout];
                                    this.DaysLeft = TimeArr[nTimeout];
                                    if (Days <= DaysLeft)
                                    {
#if TEST
                                        MessageBox.Show($"使用天数{DaysLeft}");
#endif
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return false;

        }

        public int DaysLeft { get; private set; }


    }
}
