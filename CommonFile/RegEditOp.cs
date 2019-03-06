using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
namespace CommonFile
{

    public class RegEditOp
    {
        string SubKey_MeachineKeyName = "smx";
        string Key_MeachineKeyName = "smx";
        string Key_RemainTimeName = "smx_Remain";

        public RegEditOp() { }
        public RegEditOp(string SubKey, string MKName, string RTName)
        {
            SubKey_MeachineKeyName = SubKey;
            Key_MeachineKeyName = MKName;
            Key_RemainTimeName = RTName;
        }

        public void WriteRegisterValue(string Value)
        {
            SetRegValue(SubKey_MeachineKeyName, Key_MeachineKeyName, Value);
        }

        public string ReadRegisterValue()
        {
            return GetRegValue(SubKey_MeachineKeyName, Key_MeachineKeyName);
        }

        public bool IsRegistryKeyExist(string ValueName)
        {
            return GetRegValue(SubKey_MeachineKeyName, Key_MeachineKeyName) !="";
        }


        public void WriteRemainTime(string Value)
        {
            SetRegValue(SubKey_MeachineKeyName, Key_RemainTimeName, Value);
        }

        public string ReadUsedTime()
        {
            return GetRegValue(SubKey_MeachineKeyName, Key_RemainTimeName);
        }
  

        private bool SetRegValue(string subKey, string Key, string Value)
        {
            bool bRet = false;
            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
            var SubKey = hkSoftWare.OpenSubKey(subKey, true);
            if (SubKey == null)
            {
                SubKey = hkSoftWare.CreateSubKey(subKey,RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            if (SubKey != null)
            {
                SubKey.SetValue(Key, Value);
                SubKey.Close();
                hkSoftWare.Close();
                hklm.Close();
                bRet = true;
            }
            return bRet;

        }
        private string GetRegValue(string subKey, string Key)
        {
            string strRet = "";
            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
            var SubKey = hkSoftWare.OpenSubKey(subKey, true);
            if (SubKey != null)
            {
                strRet = SubKey.GetValue(Key, "").ToString();
                SubKey.Close();
                hkSoftWare.Close();
                hklm.Close();
            }
            return strRet;
        }
    }
}
