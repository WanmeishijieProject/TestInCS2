using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Com.Netframe.Computer.Handware;
using CommonFile.Model;

namespace CommonFile
{

    public class CommonSoftware
    {
        private long TimeStart = DateTime.Now.Ticks;
        private HardwareInfo info = new HardwareInfo();
#if REG
        private RegEditOp RegEditor = new RegEditOp();
#endif
       
        string FilePath = "zrd.data";
        double[] TimeArr = {2,7,15,30,9999 };
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
#if REG
            //TimeTicksUsed=
            if (RegEditor.IsRegistryKeyExist("smx_Remain"))
            {
                var strTimeUsed = RegEditor.ReadUsedTime();
                if (long.TryParse(strTimeUsed, out long result))
                {
                    TimeTicksUsed = result;
#if TEST
                    MessageBox.Show(TimeTicksUsed.ToString());
#endif
                }
            }
            else
            {
                RegEditor.WriteRemainTime("0");
            }
#endif

        }

        ~CommonSoftware()
        {
#if REG
            if (DateTime.Now.Ticks - TimeStart > 0)
            {
                TimeTicksUsed += DateTime.Now.Ticks - TimeStart;
                RegEditor.WriteRemainTime(TimeTicksUsed.ToString());
            }
#endif
        }

        public string GetMachineKey()
        {
            MachineNumberModel Model = new MachineNumberModel
            {
                HardwareID = info.GetMNum(),
            };
            return Model.ToString();
   
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="MachineKey"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public string GenRegisterKey(string MachineKey,EnumTimeOut Timeout)
        {
            var ClientMachineKey=MachineNumberModel.FromString(MachineKey);
            var registerKeyModel = new RegisterKeyModel()
            {
                RegisterKey = info.GetRNum(),
                TimeLimit = Timeout,
                RegistTimeTicks = DateTime.Now.Ticks,
                Timestamp5 = ClientMachineKey.Timestamp5,
            };
            return registerKeyModel.ToString();
        }

        /// <summary>
        /// 点击注册的时候操作
        /// </summary>
        /// <param name="RegisterKey"></param>
        /// <returns></returns>
        public bool CheckRegisterKey(string MachineKey, string RegisterKey)
        {
            var MachineModelIn = MachineNumberModel.FromString(MachineKey);
            var ModelIn = RegisterKeyModel.FromString(RegisterKey);
            if (ModelIn.Timestamp5 != MachineModelIn.Timestamp5)
                throw new Exception("已经过时的注册码");

            if (ModelIn.RegisterKey == info.GetRNum())
            {
                using (var s = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(s, ModelIn);
                    RegEditor.WriteRegisterValue(RegisterKey);
                    RegEditor.WriteRemainTime("0");
                    DaysLeft= TimeArr[(int)ModelIn.TimeLimit];
                }
            }
            return true;  
            #region 废弃
            ////去掉后面的随机数     
            //EnumTimeOut TimeOutType = EnumTimeOut.TwoDay;
            //var CalcKwy = GenPswd(GetMachineKey(), EnumTimeOut.TwoDay);
            //if (RegisterKey.Length!= CalcKwy.Length)
            //    throw new Exception("请输入正确的注册码");
            //if (CalcKwy.Substring(0, CalcKwy.Length - 5) != RegisterKey.Substring(0, RegisterKey.Length - 5))
            //    throw new Exception("请输入正确的注册码");


            //RegisterKey = RegisterKey.Substring(0, RegisterKey.Length - 4);

            //if (int.TryParse(RegisterKey.Substring(RegisterKey.Length - 1, 1), out int nTimeout))
            //{
            //    if (Enum.IsDefined(typeof(EnumTimeOut), nTimeout))
            //{
            //        TimeOutType = (EnumTimeOut)nTimeout;
            //        var Content = $"{RegisterKey.Substring(0, RegisterKey.Length - 1)}&{(int)TimeOutType}&{DateTime.Now.Ticks}";
            //        File.WriteAllText(FilePath, Content);

            //        this.DaysLeft = TimeArr[nTimeout];
            //        return true;
            //    }
            //    else
            //        throw new Exception("请输入正确的注册码");
            //}
            //else
            //    throw new Exception("请输入正确的注册码");
            #endregion
        }

        public bool CheckFile(out double DaysLeft)
        {

            DaysLeft = 0;
            if (File.Exists(FilePath) && RegEditor.IsRegistryKeyExist("smx"))
            {
                using (var s = new FileStream(FilePath, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    RegisterKeyModel ModelInFile= bf.Deserialize(s) as RegisterKeyModel;
                    var ModelInReg =RegisterKeyModel.FromString(RegEditor.ReadRegisterValue());
                    if (ModelInFile == ModelInReg)
                    {
                        if (!ModelInFile.IsTimeoutFromNow())
                        {
                            long ticks = long.Parse(RegEditor.ReadUsedTime());
                            if (TimeSpan.FromTicks(ticks).TotalDays <= TimeArr[(int)ModelInFile.TimeLimit] )
                            {
                                DaysLeft = TimeArr[(int)ModelInFile.TimeLimit]-TimeSpan.FromTicks(ticks).TotalDays;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
            #region 废弃
//            DaysLeft = 0;
//#if REG
//            if (File.Exists(FilePath) && RegEditor.IsRegistryKeyExist("smx"))
//#endif
//            {
//#if TEST
//                MessageBox.Show("1");
//#endif
//                var content = File.ReadAllText(FilePath);
//#if REG
//                var Msg = RegEditor.ReadRegisterValue();
//#endif
//#if TEST
//                MessageBox.Show(Msg);
//#endif
//#if REG
//                if (content == Msg)
//#endif
//                {
//                    var List = content.Split('&');
//                    if (List.Count() == 3)
//                    {
//#if TEST
//                        MessageBox.Show("2");
//#endif
//                        var regKey = List[0];
//                        var timeout = List[1];
//                        var starttime = List[2];
//                        var CalcKwy = this.GenRegisterKey(GetMachineKey(), EnumTimeOut.TwoDay);

//                        CalcKwy = CalcKwy.Substring(0, CalcKwy.Length - 5);
//#if TEST
//                        MessageBox.Show($"{CalcKwy}\n{regKey}");
//#endif
//                        if (CalcKwy == regKey)
//                        {
//#if TEST
//                            MessageBox.Show("3");
//#endif
//                            if (long.TryParse(starttime, out long nStartTicks))
//                            {
//#if TEST
//                                MessageBox.Show("4");
//#endif
//                                if (int.TryParse(timeout, out int nTimeout))
//                                {
//#if TEST
//                                    MessageBox.Show("5");
//#endif
//                                    var Days = TimeSpan.FromTicks(DateTime.Now.Ticks - nStartTicks).TotalDays;
//                                    DaysLeft = TimeArr[nTimeout];
//                                    this.DaysLeft = TimeArr[nTimeout];
//                                    if (Days <= DaysLeft)
//                                    {
//#if REG
//                                        long ticks = long.Parse(RegEditor.ReadUsedTime());
//#else
//                                        long ticks = 0;
//#endif
//                                        if (TimeSpan.FromTicks(ticks).TotalDays <= DaysLeft)
//                                        {
//                                            DaysLeft = DaysLeft - TimeSpan.FromTicks(ticks).TotalDays;
//                                            //MessageBox.Show($"使用天数{DaysLeft}");
//                                            return true;
//                                        }
//#if TEST
//                                        MessageBox.Show($"使用天数{DaysLeft}");
//#endif
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            return false;
            #endregion
        }

        public double DaysLeft { get; private set; }

        private long TimeTicksUsed { get; set; }

    }
}
