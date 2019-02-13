using CameraWindow.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Classes
{
    public  class CameraTypeConvert : TypeConverter
    {
        //true enable,false disable
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var MainVM = SimpleIoc.Default.GetInstance<MainViewModel>();
            List<string> list = new List<string>();
            list.Add("未设置");
            foreach (var it in MainVM.CameraCollection)
                list.Add(it);
            return new StandardValuesCollection(list.ToArray());
        }

        //true: disable text editting.    false: enable text editting;
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
