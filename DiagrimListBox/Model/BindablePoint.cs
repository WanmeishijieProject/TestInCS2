using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiagrimListBox.Model
{
    public class BindablePoint : INotifyPropertyChanged
    {
        public event EventHandler<BindablePoint> OnPointChanged;
        public BindablePoint() { }
        public BindablePoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        double x;
        double y;
        public Point PointValue
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                this.X = value.X; this.Y = value.Y;
            }
        }
        public double X
        {
            get { return x; }
            set {
                if (x != value)
                {
                    x = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("PointValue");
                    OnPointChanged?.Invoke(this, this);
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
                    RaisePropertyChanged("PointValue");
                    OnPointChanged?.Invoke(this, this);
                }
            }
        }


        public static BindablePoint operator /(BindablePoint bp, double c)
        {
            if (c != 0)
            {
                return new BindablePoint(bp.X/c,bp.Y/c);
            }
            else
                throw new DivideByZeroException();

        }
        public static BindablePoint operator +(BindablePoint bp1, BindablePoint bp2)
        {
            return new BindablePoint(bp1.X+bp2.X,bp1.Y+bp2.Y);
        }
  
        public BindablePoint Copy()
        {
            return new BindablePoint(this.X, this.Y);
        }
       
    }
}
