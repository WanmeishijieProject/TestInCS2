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
            OutputLogicNG = EnumOutputLogic.True;
        }

        [CategoryAttribute("参数设置"), DescriptionAttribute("干燥剂 L1 边的最小值")]
        public double MinL1 { get; set; }
        [CategoryAttribute("参数设置"), DescriptionAttribute("干燥剂 L1 边的最大值")]
        public double MaxL1 { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("干燥剂 L2 边的最小值")]
        public double MinL2 { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("干燥剂 L2 边的最大值")]
        public double MaxL2 { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("是否使用Output输出")]
        public EnumUseOutput UseOutput { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("当NG时的输出逻辑")]
        public EnumOutputLogic OutputLogicNG { get; set; }

        [CategoryAttribute("参数设置"), DescriptionAttribute("设置硬件触发方式")]
        public EnumSignalType TriggerType { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
