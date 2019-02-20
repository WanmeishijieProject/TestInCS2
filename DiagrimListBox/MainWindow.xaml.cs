using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using DiagrimListBox.Model;
using DiagrimListBox.ViewModel;

namespace DiagrimListBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as DragableObject;
            if (node == null)
                return;
            node.Location.X += e.HorizontalChange;
            node.Location.Y += e.VerticalChange;
            //node.Location.Value = Point.Add(node.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));
        }

        private void StartThum_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as LineBez;
            if (node == null)
                return;
            node.StartPoint.X += e.HorizontalChange;
            node.StartPoint.Y += e.VerticalChange;
        }

        private void MidThum_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as LineBez;
            if (node == null)
                return;
            node.MidPoint.X += e.HorizontalChange;
            node.MidPoint.Y += e.VerticalChange;
            Console.WriteLine($"{node.MidPoint.X},{node.MidPoint.Y}");
        }

        private void EndThum_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as LineBez;
            if (node == null)
                return;
            node.EndPoint.X += e.HorizontalChange;
            node.EndPoint.Y += e.VerticalChange;
        }

        private void ListBox_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void ListBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}