using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagrimListBox.Model
{
    public class SnapPot : DragableObject
    {
        public SnapPot(String Name,Node ParentNode, double Angle) : base(Name)
        {
            this.ParentNode = ParentNode;
            this.Angle = Angle;
        }
        public double Angle { get; set; }
        public Node ParentNode { get; set; }
        public int IndexBaseZero { get; set; }
        public void Recalculate()
        {
            if (this.IndexBaseZero == 0)
            {
                this.Location.X = ParentNode.Location.X + ParentNode.NodeWidth / 2;
                this.Location.Y = ParentNode.Location.Y;
            }
            else
            {
                this.Location.X = ParentNode.Location.X + ParentNode.NodeWidth / 2;
                this.Location.Y = ParentNode.Location.Y + ParentNode.NodeHeight;
            }
        }

    }
}
