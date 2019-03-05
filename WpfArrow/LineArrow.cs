using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

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
            context.ArcTo(new Point(End.X + Radius, End.Y), new Size(Radius, Radius), 0, true, SweepDirection.Clockwise, true, true);
            context.ArcTo(new Point(End.X - Radius, End.Y), new Size(Radius, Radius), 0, true, SweepDirection.Clockwise, true, true);
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


    }
}
