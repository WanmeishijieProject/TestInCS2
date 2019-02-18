using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraWindow.Classes
{
    public class DragDropData
    {
        private Int32 dataInt32;
        private string dataStr;
        private object dataObj;

        public Int32 DataInt32
        {
            set {
                dataInt32 = value;
                AvilableDataType = typeof(Int32);
            }
            get { return dataInt32; }
        }
        public string DataStr
        {
            set
            {
                dataStr = value;
                AvilableDataType = typeof(string);
            }
            get { return dataStr; }
        }
        public object DataObj
        {
            set
            {
                dataObj = value;
                AvilableDataType = typeof(object);
            }
            get { return dataObj; }
        }

        public Type AvilableDataType { get; private set; }
       
    }
}
