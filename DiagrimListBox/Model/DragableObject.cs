using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiagrimListBox.Model
{
    public class DragableObject : INotifyPropertyChanged
    {
        BindablePoint location;
        BindablePoint size;

        public DragableObject(string Name)
        {
            this.Name = Name;
            Location = new BindablePoint() { X = 0, Y = 0, };
            Size = new BindablePoint() { X = 100, Y = 100, };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


        public string Name { get; set; }
        public BindablePoint Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BindablePoint Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
