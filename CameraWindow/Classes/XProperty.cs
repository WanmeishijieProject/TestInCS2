using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Classes
{
    public class XProperty
    {
        private string theId = ""; //属性Id，我的项目中需要，大家可以忽略

        private string theCategory = "Common"; //属性所属类别

        private string theName = "";     //属性名称

        private bool theReadOnly = false;  //属性的只读性，true为只读

        private string theDescription = ""; //属性的描述内容

        private object theValue = null;    //值

        private System.Type theType = null; //类型

        private bool theBrowsable = true;  //显示或隐藏，true为显示

        TypeConverter theConverter = null;  //类型转换

        public string Id
        {
            get { return theId; }

            set { theId = value; }
        }

        public string Category
        {
            get { return theCategory; }

            set { theCategory = value; }
        }
        public bool ReadOnly
        {
            get { return theReadOnly; }

            set { theReadOnly = value; }
        }
        public string Name
        {
            get { return this.theName; }

            set { this.theName = value; }
        }
        public object Value
        {
            get { return this.theValue; }

            set { this.theValue = value; }
        }
        public string Description
        {
            get { return theDescription; }

            set { theDescription = value; }
        }

        public System.Type ProType
        {
            get { return theType; }

            set { theType = value; }
        }

        public bool Browsable
        {
            get { return theBrowsable; }
            set { theBrowsable = value; }
        }

        public virtual TypeConverter Converter
        {
            get { return theConverter; }
            set { theConverter = value; }
        }

    }
}
