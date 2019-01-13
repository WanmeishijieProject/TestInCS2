using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
namespace CakeTest.Class
{

    public class RegEditOp
    {
        RegistryKey Key = Registry.LocalMachine;
        public void WriteRegisterValue(string KeyValue)
        {
            if (IsRegistryKeyExist("smx"))
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"SOFTWARE", true);
                hkSoftWare.SetValue("smx", KeyValue, RegistryValueKind.String);
                hklm.Close();
                hkSoftWare.Close();
            }
            else
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"SOFTWARE");
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
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey hkSoftWare = hklm.OpenSubKey(@"SOFTWARE", true);
                var str = hkSoftWare.GetValue("smx");
                hklm.Close();
                hkSoftWare.Close();
                return str.ToString();
            }
            return "";
        }

        public bool IsRegistryKeyExist(string sKeyName)
        {
            string[] sKeyNameColl;
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftWare = hklm.OpenSubKey(@"SOFTWARE");
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

    }
}
