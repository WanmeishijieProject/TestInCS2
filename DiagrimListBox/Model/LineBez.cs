using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiagrimListBox.Model
{
    public class LineBez : DragableObject
    {

        public LineBez(string Name, BindablePoint P1, BindablePoint P2) : base(Name)
        {
            this.StartPoint = P1;
            this.EndPoint = P2;
            MidPoint = (P1 + P2) / 2;
        }

        /// <summary>
        /// 起点，中间点，终点的改变都会改变PointV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MidPoint_OnPointChanged(object sender, BindablePoint e)
        {
            RaisePropertyChanged("PointV");
        }


        BindablePoint startPoint;
        public BindablePoint StartPoint
        {
            get { return startPoint; }
            set
            {
                if (startPoint != value)
                {
                    startPoint = value;
                    startPoint.OnPointChanged -= MidPoint_OnPointChanged;
                    startPoint.OnPointChanged += MidPoint_OnPointChanged;
                }
            }
        }

        BindablePoint endPoint;
        public BindablePoint EndPoint
        {
            get { return endPoint; }
            set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    endPoint.OnPointChanged -= MidPoint_OnPointChanged;
                    endPoint.OnPointChanged += MidPoint_OnPointChanged;
                }
            }
        }

        /// <summary>
        /// 实际要经过的点
        /// </summary>
        BindablePoint midPoint;
        public BindablePoint MidPoint
        {
            get { return midPoint; }
            set
            {
                if (midPoint != value)
                {
                    midPoint = value;
                    midPoint.OnPointChanged -= MidPoint_OnPointChanged;
                    midPoint.OnPointChanged += MidPoint_OnPointChanged;
                }
            }
        }

        /// <summary>
        /// 虚拟的控制点
        /// </summary>
        public BindablePoint PointV
        {
            get {
                var p1 = StartPoint.PointValue - MidPoint.PointValue;
                var p2 = EndPoint.PointValue - MidPoint.PointValue;

                var d1 = Math.Sqrt(p1.X*p1.X+p1.Y*p1.Y);
                var d2= Math.Sqrt(p2.X * p2.X + p2.Y * p2.Y);

                var pc = MidPoint.PointValue - (Math.Sqrt(d1 * d2) / 2)*(p1 / d1 + p2 / d2);
                return new BindablePoint(pc.X,pc.Y);
            }
        }

    }
}
