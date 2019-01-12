using CakeTest.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CakeTest.Model
{
    public class ParaModel : INotifyPropertyChanged
    {


        public ParaModel()
        {
            MinL1 = 100;
            MinL2 = 100;
            MaxL1 = 200;
            MaxL2 = 200;
            TriggerType = EnumSignalType.RisingEdge;

        }

        [CategoryAttribute("参数设置"), DescriptionAttribute("Min value of L1")]
        public double MinL1 { get; set; }
        [CategoryAttribute("参数设置"), DescriptionAttribute("Max value of L1")]
        public double MaxL1 { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("Min value of L2")]
        public double MinL2 { get; set; }
        [CategoryAttribute("参数设置"), DescriptionAttribute("Max value of L2")]
        public double MaxL2 { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("设置硬件触发方式")]
        public EnumSignalType TriggerType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
