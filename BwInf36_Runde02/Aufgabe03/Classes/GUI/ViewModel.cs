using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Aufgabe03.Annotations;

namespace Aufgabe03.Classes.GUI
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<PositionTab> _positionTabs;

        public ObservableCollection<PositionTab> PositionTabs
        {
            get => _positionTabs;
            set
            {
                _positionTabs = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel()
        {
            PositionTabs = new ObservableCollection<PositionTab>();
        }
    }
}