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

      
    }
}