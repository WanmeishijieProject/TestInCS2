using DiagrimListBox.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiagrimListBox.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        DragableObject selectedObject;
        public ObservableCollection<Node> NodeCollect { get; set; }
        public ObservableCollection<Connector> ConnectorCollect { get; set; }
        public ObservableCollection<LineBez> LineCollect { get; set; }
        public ObservableCollection<SnapPot> SnapPotCollect { get; set; }

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
            
            for (int i = 0; i < 3; i++)
            {
                var node=new Node($"Node{i + 1}");
                node.SnapPotList=new System.Collections.Generic.List<SnapPot>(
                    new[] {
                        new  SnapPot($"SnapSpot{i+1}",node, 0){  IndexBaseZero=0,},
                         new  SnapPot($"SnapSpot{i+1}",node, 180){ IndexBaseZero=1},
                    }
                    );
                NodeCollect.Add(node);
                //ConnectorCollect.Add(new Connector($"ConnectorCollect{i + 1}"));
            }

            SnapPotCollect = new ObservableCollection<SnapPot>(NodeCollect.SelectMany(x => x.SnapPotList));
            LineCollect.Add(new LineBez($"Line1", NodeCollect[0].SnapPotList[1].Location, NodeCollect[1].SnapPotList[0].Location));
            LineCollect.Add(new LineBez($"Line1", NodeCollect[1].SnapPotList[1].Location, NodeCollect[2].SnapPotList[0].Location));
        
        }
    }
}