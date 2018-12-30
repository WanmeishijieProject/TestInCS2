using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CameraWindow.Model
{
    public class CameraGridDataModel : INotifyPropertyChanged
    {
        private int _row;
        private int _col;
        private int _rowSpan;
        private int _colSpan;
        private Visibility _visible;

        public int Row
        {
            get { return _row; }
            set {
                if (_row != value)
                {
                    _row = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int Col
        {
            get { return _col; }
            set
            {
                if (_col != value)
                {
                    _col = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int RowSpan
        {
            get { return _rowSpan; }
            set
            {
                if (_rowSpan != value)
                {
                    _rowSpan = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ColSpan
        {
            get { return _colSpan; }
            set
            {
                if (_colSpan != value)
                {
                    _colSpan = value;
                    RaisePropertyChanged();
                }
            }
        }
        public Visibility Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(PropertyName));
        }
    }
}
