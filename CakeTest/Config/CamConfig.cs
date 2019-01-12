using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CakeTest.Config
{
    public class CamConfig : INotifyPropertyChanged
    {
        bool _enable;
        string _camName;

        public bool Enable
        {
            set {
                if (value != _enable)
                {
                    _enable = value;
                    RaisePropertyChanged();
                }
            }
            get { return _enable; }
        }
        public string CamName
        {
            set
            {
                if (value != _camName)
                {
                    _camName = value;
                    RaisePropertyChanged();
                }
            }
            get { return _camName; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string PropertyName="")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(PropertyName));
        }
    }
}
