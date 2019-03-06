using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Matrix = System.Windows.Media.Matrix;

namespace WpfArrow
{
    public class LineArrow : Shape
    {
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start",typeof(Point),typeof(LineArrow), new FrameworkPropertyMetadata(default(Point),FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public Point Start {
            get { return (Point)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(Point), typeof(LineArrow),new FrameworkPropertyMetadata(default(Point),FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public Point End
        {
            get { return (Point)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        protected override Geometry DefiningGeometry {
            get {
                StreamGeometry sg = new StreamGeometry();
                sg.FillRule = FillRule.EvenOdd;
                using (var context = sg.Open())
                {
                    DrawGeometry(context);
                }
                sg.Freeze();
                return sg;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            int Radius = 20;
            context.BeginFigure(Start, false, false);
            context.LineTo(End, true, true);
            context.BeginFigure(new Point(End.X - Radius, End.Y), false, false);
            //context.ArcTo(new Point(End.X + Radius, End.Y), new Size(Radius, Radius), 0, true, SweepDirection.Clockwise, true, true);
            //context.ArcTo(new Point(End.X - Radius, End.Y), new Size(Radius, Radius), 0, true, SweepDirection.Clockwise, true, true);
            DrawArrow(context, Start, End);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var defaultTypeface = new Typeface(SystemFonts.StatusFontFamily, SystemFonts.StatusFontStyle,
                        SystemFonts.StatusFontWeight, new FontStretch());
            FormattedText text = new FormattedText("Hello", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, defaultTypeface,
                            30, new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            
            drawingContext.DrawText(text,new Point((End.X+Start.X- text.Width) /2, (End.Y+Start.Y)/2));
        }

       
        private void DrawArrow(StreamGeometryContext context, Point startPoint, Point endPoint)
        {
            var ArrowLength = 20;
            var ArrowAngle = 60;
            if (context != null)
            {
                var matx = new Matrix();
                Vector vect = startPoint - endPoint;
                //获取单位向量
                vect.Normalize();
                vect *= ArrowLength;
                //旋转夹角的一半
                matx.Rotate(ArrowAngle / 2);
                //计算上半段箭头的点

                context.BeginFigure(endPoint + vect * matx, true, false);
                context.LineTo(endPoint, true, true);

                matx.Rotate(-ArrowAngle);
                context.LineTo(endPoint + vect * matx, true, true);
            }
        }


    }
}
