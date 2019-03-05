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
        List<SnapPot> snapPotList = new List<SnapPot>();
        public Node(string Name):base(Name)
        {
            this.Name = Name;
            Location.OnPointChanged += Location_OnPointChanged;
            Size.OnPointChanged += Size_OnPointChanged;
        }

        private void Size_OnPointChanged(object sender, BindablePoint e)
        {
            RecalculateSnaps();
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
    }
}
