using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Classes
{
    public class ComboItemConvertForCam : ComboBoxItemTypeConvert
    {
        private Hashtable hash;
        public override void GetConvertHash()
        {
            try
            {
                myhash = hash;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        public void MyComboItemConvert(string str)
        {
            hash = new Hashtable();
            string[] stest = str.Split(',');
            for (int i = 0; i < stest.Length; i++)
            {
                hash.Add(i, stest[i]);
            }
            GetConvertHash();
            value = 0;
        }

        public int value { get; set; }

        public void MyComboItemConvert(string str, int s)
        {
            hash = new Hashtable();
            string[] stest = str.Split(',');
            for (int i = 0; i < stest.Length; i++)
            {
                hash.Add(i, stest[i]);
            }
            GetConvertHash();
            value = s;
        }
    }
}

