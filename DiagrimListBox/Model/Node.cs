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
        public Node(string Name):base(Name)
        {
            this.Name = Name;
           
        }
    }
}
