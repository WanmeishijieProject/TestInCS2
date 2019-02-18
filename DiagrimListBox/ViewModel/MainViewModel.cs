using DiagrimListBox.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace DiagrimListBox.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Node> NodeCollect { get; set; }
        public ObservableCollection<Connector> ConnectorCollect { get; set; }

        public MainViewModel()
        {
            NodeCollect = new ObservableCollection<Node>();
            ConnectorCollect = new ObservableCollection<Connector>();

            for (int i = 0; i < 5; i++)
            {
                NodeCollect.Add(new Node($"Node{i+1}"));
                ConnectorCollect.Add(new Connector($"ConnectorCollect{i + 1}"));
            }
        }
    }
}