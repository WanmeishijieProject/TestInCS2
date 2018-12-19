using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GPAP.Definetions;

namespace GPAP.Model.AbbCmd
{
    public abstract class RobotCmdBase : IRobotCmd
    {
        private AutoResetEvent SyncEvent = new AutoResetEvent(false);
        public RobotCmdBase()
        {
            for (int i = 0; i < 6; i++)
                Paras.Add("");
            I_Speed = EnumRobotSpeed.V10;
            I_Tool = EnumRobotTool.Tool0;
        }
        public EnumRobotSpeed I_Speed { get; set; }
        public EnumRobotTool I_Tool { get; set; }
        public EnumRobotCmd I_Cmd { get; set; }
        public object O_ReturnObj { get; set; }
        protected List<string> Paras = new List<string>(6);
        protected abstract void SetProfile();
        protected abstract void ReadProfile();
        public virtual byte[] ToByteArray()
        {
            SetProfile();
            string strCmd = string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"]", (int)I_Cmd, Paras[0], Paras[1], Paras[2], Paras[3], Paras[4], Paras[5]);
            List<byte> ByteList = new List<byte>();
            foreach (var ch in strCmd)
            {
                ByteList.Add((byte)ch);
            }
            return ByteList.ToArray();
        }
        public void FromString(string OriginString)
        {
            var List = OriginString.Replace("]", "").Replace("[", "").Replace("\"", "").Split(',');
            if (List.Count() != 7)
                throw new Exception("Wrong number para recieved!");
            string strCmd = List[0];
            if (int.TryParse(strCmd, out int intCmd))
            {
                if (Enum.IsDefined(typeof(EnumRobotCmd), int.Parse(strCmd)))
                {
                    I_Cmd = (EnumRobotCmd)int.Parse(strCmd);
                    for (int i = 0; i < 6; i++)
                    {
                        Paras[i] = List[i + 1];
                    }
                    ReadProfile();
                }
                else
                    throw new Exception($"Wrong cmd found: {I_Cmd.ToString()}");
            }
            else
                throw new Exception("Wrong cmd found when parse intCmd");
        }
        public void FromByteArray(byte[] Buffer, int len)
        {
            StringBuilder sb = new StringBuilder();
            if (Buffer.Length < len)
                return;
            for (int i = 0; i < len; i++)
            {
                sb.Append((char)Buffer[i]);
            }
            FromString(sb.ToString());
        }

        public void ResetMessageState()
        {
            SyncEvent.Reset();
        }
        public void SetMessageState()
        {
            SyncEvent.Set();
        }
        public bool WaitCmdRecved(int MiliSecond)
        {
            return SyncEvent.WaitOne(MiliSecond);
        }

        public static EnumRobotCmd GetCmdTypeFromString(string OriginString)
        {
            var List = OriginString.Replace("]", "").Replace("[", "").Replace("\"", "").Split(',');
            if (List.Count() != 7)
                throw new Exception("Wrong number para recieved!");
            if (Enum.TryParse(List[0], out EnumRobotCmd cmd))
                return cmd;
            return EnumRobotCmd.NONE;
        }


        /// <summary>
        /// 设置移动速度与工具
        /// </summary>
        /// <param name="I_Speed">10代表V10，20代表V20</param>
        /// <param name="I_Tool"></param>
        protected void SetSpeedAndTool(EnumRobotSpeed I_Speed,EnumRobotTool I_Tool)
        {
            Paras[4] = ((int)I_Speed).ToString();
            Paras[5] = ((int)I_Tool).ToString();
        }
        public abstract object GenEmptyCmd();
    }
}
