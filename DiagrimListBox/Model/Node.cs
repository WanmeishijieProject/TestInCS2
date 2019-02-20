using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiagrimListBox.Model
{
    public class Node : DragableObject
    {
        double width;
        double height;
        List<SnapPot> snapPotList = new List<SnapPot>();
        public Node(string Name):base(Name)
        {
           
            this.Name = Name;
            this.NodeHeight = 200;
            this.NodeWidth = 300;
            Location.OnPointChanged += Location_OnPointChanged;
        }

        private void Location_OnPointChanged(object sender, BindablePoint e)
        {
            RecalculateSnaps();
        }

        public List<SnapPot> SnapPotList
        {
            get { return snapPotList; }
            set {
                if (snapPotList != value)
                {
                    snapPotList = value;
                    RaisePropertyChanged();
                    RecalculateSnaps();
                }
            }
        }

        private void RecalculateSnaps()
        {
            SnapPotList.ForEach(x => x.Recalculate());

        }

        public double NodeWidth {
            get { return width; }
            set {
                if (width != value)
                {
                    width = value;
                    RecalculateSnaps();
                }
            }
        }
        public double NodeHeight
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    RecalculateSnaps();
                }
            }
        }
    }
}
