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
        public void WriteRegisterValue(string KeyValue)
        {
            
            if (IsRegistryKeyExist("smx"))
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
                hkSoftWare.SetValue("smx", KeyValue, RegistryValueKind.String);
                hklm.Close();
                hkSoftWare.Close();
            }
            else
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
                hkSoftWare.CreateSubKey("smx");
                hklm.SetValue("smx", KeyValue, RegistryValueKind.String);
                hklm.Close();
                hkSoftWare.Close();
            }
        }

        public string ReadRegisterValue()
        {
            if (IsRegistryKeyExist("smx"))
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
                var str = hkSoftWare.GetValue("smx");
                hklm.Close();
                hkSoftWare.Close();
                if(str!=null)
                    return str.ToString();
            }
            return "";
        }

        public bool IsRegistryKeyExist(string sKeyName)
        {
            string[] sKeyNameColl;
            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
            sKeyNameColl = hkSoftWare.GetSubKeyNames(); //获取SOFTWARE下所有的子项
            foreach (string sName in sKeyNameColl)
            {
                if (sName == sKeyName)
                {
                    hklm.Close();
                    hkSoftWare.Close();
                    return true;
                }
            }
            hklm.Close();
            hkSoftWare.Close();
            return false;
        }


        public void WriteRemainTime(string KeyValue)
        {
            if (IsRegistryKeyExist("smx_Remain"))
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
                hkSoftWare.SetValue("smx_Remain", KeyValue, RegistryValueKind.String);
                hklm.Close();
                hkSoftWare.Close();
            }
            else
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
                hkSoftWare.CreateSubKey("smx_Remain");
                hklm.SetValue("smx_Remain", KeyValue, RegistryValueKind.String);
                hklm.Close();
                hkSoftWare.Close();
            }
        }

        public string ReadUsedTime()
        {
            if (IsRegistryKeyExist("smx_Remain"))
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software", true);
                var str = hkSoftWare.GetValue("smx_Remain");
                hklm.Close();
                hkSoftWare.Close();
                if (str != null)
                    return str.ToString();
            }
            return "";
        }
    }
}
