using Lab05_Pyvovar.Tools;
using System.ComponentModel;
using System.Diagnostics;
using Lab05_Pyvovar.Tools.Managers;
using System.Windows.Input;
using Lab05_Pyvovar.Tools.Navigation;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Lab05_Pyvovar.ViewModel
{
    internal class DetailViewModel : INotifyPropertyChanged
    {
        private ProcessModuleCollection _modulesList;
        private ProcessThreadCollection _threadsList;

        private RelayCommand<object> _backCommand;

        internal DetailViewModel()
        {
            RefreshInfo();
        }

        public ProcessModuleCollection ModulesList
        {
            get { return _modulesList; }
            private set
            {
                _modulesList = value;
                OnPropertyChanged();
            }
        }

        public ProcessThreadCollection ThreadsList
        {
            get { return _threadsList; }
            private set
            {
                _threadsList = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand<object>(
                           BackImplementation));
            }
        }

        private void BackImplementation(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.Info);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void RefreshInfo()
        {
            ModulesList = StationManager.CurrentProcess.Modules;
            ThreadsList = StationManager.CurrentProcess.Threads;
        }

        private void CloseImplementation(object obj)
        {
            StationManager.CloseApp();
        }
    }
}