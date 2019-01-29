using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestHalcon17Wpf.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
