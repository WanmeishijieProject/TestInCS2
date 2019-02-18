using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CameraWindow.Classes
{
    public class DragableListBox : ListBox
    {
        public DragableListBox()
        {
            this.AllowDrop = true;
            this.MouseMove += DragableListBox_MouseMove;
            this.Drop += DragableListBox_Drop;
        }

        private void DragableListBox_Drop(object sender, DragEventArgs e)
        {
            if (CommandOnDrop != null)
                if(e.Data!=null)
                    CommandOnDrop.Execute(e.Data.GetData(typeof(DragDropData)));
        }

        private void DragableListBox_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (this.SelectedItem != null)
                {
                    var dragDropData = new DragDropData()
                    {
                        DataObj = SelectedItem,
                    };
                    DataObject draObj = new DataObject(dragDropData);
                    DragDrop.DoDragDrop(this, draObj, DragDropEffects.Copy);
                }
            }
        }


        public const string CommandOnDropPropertyName = "CommandOnDrop";
        public RelayCommand<DragDropData> CommandOnDrop
        {
            get
            {
                return (RelayCommand<DragDropData>)GetValue(CommandOnDropProperty);
            }
            set
            {
                SetValue(CommandOnDropProperty, value);
            }
        }
        public static readonly DependencyProperty CommandOnDropProperty = DependencyProperty.Register(
            CommandOnDropPropertyName,
            typeof(RelayCommand<DragDropData>),
            typeof(DragableListBox));
    }
}
