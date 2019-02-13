using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Classes
{
    public  class ComboBoxItemTypeConvert : TypeConverter
    {
        public Hashtable myhash = null;
        public ComboBoxItemTypeConvert()
        {
            myhash = new Hashtable();
            GetConvertHash();
        }
        public virtual void GetConvertHash()
        {
            return;
        }

        //是否支持选择列表的编辑
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        //重写combobox的选择列表
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            int[] ids = new int[myhash.Values.Count];
            int i = 0;
            foreach (DictionaryEntry myDE in myhash)
            {
                ids[i++] = (int)(myDE.Key);
            }
            return new StandardValuesCollection(ids);
        }
        //判断转换器是否可以工作
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);

        }

        //重写转换器，将选项列表（即下拉菜单）中的值转换到该类型的值
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object obj)
        {
            if (obj is string)
            {
                foreach (DictionaryEntry myDE in myhash)
                {
                    if (myDE.Value.Equals((obj.ToString())))
                        return myDE.Key;
                }
            }
            return base.ConvertFrom(context, culture, obj);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);

        }

        //重写转换器将该类型的值转换到选择列表中
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object obj, Type destinationType)
        {

            if (destinationType == typeof(string))
            {
                foreach (DictionaryEntry myDE in myhash)
                {
                    if (myDE.Key.Equals(obj))
                        return myDE.Value.ToString();
                }
                return "";
            }
            return base.ConvertTo(context, culture, obj, destinationType);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
