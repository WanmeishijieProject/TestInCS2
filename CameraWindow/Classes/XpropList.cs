using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Classes
{
    public class XPropList : List<XProperty>, ICustomTypeDescriptor
    {

        #region ICustomTypeDescriptor 成员



        public AttributeCollection GetAttributes()

        {
            return TypeDescriptor.GetAttributes(this, true);

        }



        public string GetClassName()

        {

            return TypeDescriptor.GetClassName(this, true);

        }



        public string GetComponentName()

        {

            return TypeDescriptor.GetComponentName(this, true);

        }



        public TypeConverter GetConverter()

        {

            return TypeDescriptor.GetConverter(this, true);

        }



        public EventDescriptor GetDefaultEvent()

        {

            return TypeDescriptor.GetDefaultEvent(this, true);

        }



        public PropertyDescriptor GetDefaultProperty()

        {

            return TypeDescriptor.GetDefaultProperty(this, true);

        }



        public object GetEditor(System.Type editorBaseType)

        {

            return TypeDescriptor.GetEditor(this, editorBaseType, true);

        }



        public EventDescriptorCollection GetEvents(System.Attribute[] attributes)

        {

            return TypeDescriptor.GetEvents(this, attributes, true);

        }



        public EventDescriptorCollection GetEvents()

        {

            return TypeDescriptor.GetEvents(this, true);

        }



        public PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            ArrayList props = new ArrayList();
            for (int i = 0; i < this.Count; i++)
            {  //判断属性是否显示

                if (this[i].Browsable == true)
                {
                    XPropDiscriptor psd = new XPropDiscriptor(this[i], attributes);
                    props.Add(psd);
                }
            }
            PropertyDescriptor[] propArray = (PropertyDescriptor[])props.ToArray(typeof(PropertyDescriptor));
            return new PropertyDescriptorCollection(propArray);
        }



        public PropertyDescriptorCollection GetProperties()

        {

            return TypeDescriptor.GetProperties(this, true);

        }



        public object GetPropertyOwner(PropertyDescriptor pd)

        {

            return this;

        }



        #endregion



        public override string ToString()

        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.Count; i++)

            {

                sb.Append("[" + i + "] " + this[i].ToString() + System.Environment.NewLine);

            }

            return sb.ToString();

        }

    }
}
