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
        private long TimeStart = 0;
        private HardwareInfo info = new HardwareInfo();
        private static string RGName = "mk";
        private static string TUName = "tu";
        private static string SubKey = "smx";
#if REG
        private RegEditOp RegEditor = new RegEditOp(SubKey,RGName,TUName);
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
            TimeStart = DateTime.Now.Ticks;
            if (RegEditor.IsRegistryKeyExist(TUName))
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
            var NowTicks = DateTime.Now.Ticks;
            if (NowTicks - TimeStart > 0)
            {
               // MessageBox.Show(TimeTicksUsed.ToString());
                TimeTicksUsed += (NowTicks - TimeStart);
                //MessageBox.Show((NowTicks - TimeStart).ToString());
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
                RegisterKey = info.GetRNum(ClientMachineKey.HardwareID, false),
                TimeLimit = Timeout,    //1
                Timestamp5 = ClientMachineKey.Timestamp5,   //5
                RegistTimeTicks = ClientMachineKey.Timestamp20, //20
                //BFEBFBFF000906EA52176B8A006368747630831805632834385645
            };
            //MessageBox.Show($"生成的注册码:{ClientMachineKey.HardwareID}\n{registerKeyModel.RegisterKey}");
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

            //MessageBox.Show($"{ModelIn.Timestamp5}\n{MachineModelIn.Timestamp5}\n");


            if (ModelIn.Timestamp5 != MachineModelIn.Timestamp5)
            {
                
                throw new Exception("已经过时的注册码");
            }

            string ClacRegisterKey = info.GetRNum(MachineModelIn.HardwareID, true);

            //MessageBox.Show($"获取的注册码:{MachineModelIn.HardwareID}\n{ClacRegisterKey}");
            //说明从注册信息里读的是正确的，但是再次获取机器码就不对了
            //MessageBox.Show($"4\n{ModelIn.RegisterKey}\n{ClacRegisterKey}\n{ModelIn.RegisterKey.Length}\n{ClacRegisterKey.Length}");
            if (ModelIn.RegisterKey.Trim() == ClacRegisterKey.Trim())
            {
                //注册成功的时候写入文件
                

                using (var s = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(s, ModelIn);
                    RegEditor.WriteRegisterValue(RegisterKey);

                    RegEditor.WriteRemainTime("0");
                    DaysLeft= TimeArr[(int)ModelIn.TimeLimit];
                    return true;
                }
            }
            return false;  
        }

        public bool CheckFile(out double DaysLeft)
        {
            DaysLeft = 0;
            if (File.Exists(FilePath) && RegEditor.IsRegistryKeyExist(RGName))
            {
                //MessageBox.Show("10");
                using (var s = new FileStream(FilePath, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    RegisterKeyModel ModelInFile= bf.Deserialize(s) as RegisterKeyModel;
                    var ModelInReg =RegisterKeyModel.FromString(RegEditor.ReadRegisterValue());

                    //MessageBox.Show($"10\n{ModelInFile.ToString()}\n{ModelInReg.ToString()}");
                    if (ModelInFile == ModelInReg)
                    {
                        //MessageBox.Show("11");
                        //总时间-(读取Now-注册时间)
                        if (!ModelInFile.IsTimeoutFromNow(out double DaysLeft1))
                        {
                            //MessageBox.Show("12");
                            long ticks = long.Parse(RegEditor.ReadUsedTime());

                            //以下是防止客户篡改系统时间
                            if (TimeSpan.FromTicks(ticks).TotalDays <= TimeArr[(int)ModelInFile.TimeLimit] )
                            {
                                //MessageBox.Show("13");
                                //这个时间是剩余的使用时间
                                var DaysLeft2 = TimeArr[(int)ModelInFile.TimeLimit]-TimeSpan.FromTicks(ticks).TotalDays;
                                DaysLeft = Math.Min(DaysLeft1,DaysLeft2);

                                //按照最大的使用时间计算已经使用的Ticks
                                TimeTicksUsed = TimeSpan.FromDays(TimeArr[(int)ModelInFile.TimeLimit]- DaysLeft).Ticks;
                                RegEditor.WriteRemainTime(TimeTicksUsed.ToString());
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
     
        }

        public double DaysLeft { get; private set; }

        private long TimeTicksUsed { get; set; }

    }
}
