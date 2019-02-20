using DiagrimListBox.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace DiagrimListBox.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        DragableObject selectedObject;


        public ObservableCollection<Node> NodeCollect { get; set; }
        public ObservableCollection<Connector> ConnectorCollect { get; set; }
        public ObservableCollection<LineBez> LineCollect { get; set; }

        
        public DragableObject SelectedObject {
            get {
                return selectedObject;
            }
            set {
                if (selectedObject != value)
                {
                    selectedObject = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            NodeCollect = new ObservableCollection<Node>();
            ConnectorCollect = new ObservableCollection<Connector>();
            LineCollect = new ObservableCollection<LineBez>();
            for (int i = 0; i < 5; i++)
            {
                NodeCollect.Add(new Node($"Node{i + 1}"));
                ConnectorCollect.Add(new Connector($"ConnectorCollect{i + 1}"));
                LineCollect.Add(new LineBez($"Line{i+1}")
                {
                    StartPoint = new BindablePoint(100, 100),
                    MidPoint = new BindablePoint(300, 200),
                    EndPoint = new BindablePoint(500, 800),
                });
            }

           
        }
    }
}