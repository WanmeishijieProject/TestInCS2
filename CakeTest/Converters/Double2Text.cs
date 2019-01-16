using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeTest.Converters
{
    public class Double2Text : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var DaysLeft= Math.Round((double)value,int.Parse(parameter.ToString()));
            if (DaysLeft > 1000)
                return "已注册";
            else
                return $"还剩{DaysLeft}天";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
