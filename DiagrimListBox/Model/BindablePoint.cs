using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiagrimListBox.Model
{
    public class BindablePoint : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        double x;
        double y;
        BindablePoint pointValue{get;set;}
        public double X
        {
            get { return x; }
            set {
                if (x != value)
                {
                    x = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("PointValue");
                }
            }
        }
        public double Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    RaisePropertyChanged();
                }
            }
        }


    }
}
