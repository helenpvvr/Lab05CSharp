using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Lab05_Pyvovar.Models;
using Lab05_Pyvovar.Tools;
using Lab05_Pyvovar.Tools.Managers;
using Lab05_Pyvovar.Tools.Navigation;

namespace Lab05_Pyvovar.ViewModel
{
    internal class InfoViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<SystemProcess> _processes;
        private Thread _workingThread;
        private Thread _workingThreadRefreshList;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;

        private SortFields _sortField;
        
        private RelayCommand<object> _sortCommand;
        private RelayCommand<object> _showDetailCommand;
        private RelayCommand<object> _stopProcessCommand;
        private RelayCommand<object> _openFolderCommand;
        
        public ICommand ShowDetailCommand
        {
            get
            {
                return _showDetailCommand ?? (_showDetailCommand = new RelayCommand<object>(
                           ShowDetailImplementation, CanActionExecute));
            }
        }

        public ICommand StopProcessCommand
        {
            get
            {
                return _stopProcessCommand ?? (_stopProcessCommand = new RelayCommand<object>(
                           StopProcessImplementation, CanActionExecute));
            }
        }

        public ICommand OpenFolderCommand
        {
            get
            {
                return _openFolderCommand ?? (_openFolderCommand = new RelayCommand<object>(
                           OpenFolderImplementation, CanActionOpenExecute));
            }
        }

        public ICommand SortCommand
        {
            get
            {
                return _sortCommand ?? (_sortCommand = new RelayCommand<object>(
                           SortImplementation));
            }
        }

        private bool CanActionExecute(object obj)
        {
            return StationManager.CurrentProcess != null;
        }

        private bool CanActionOpenExecute(object obj)
        {
            return StationManager.CurrentProcess != null && !String.IsNullOrEmpty(StationManager.CurrentProcess.PathToFile);
        }

        internal InfoViewModel()
        {
            _processes = new ObservableCollection<SystemProcess>(StationManager.DataStorage.ProcessList);

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            StationManager.StopThreads += StopWorkingThread;
        }

        private void SortImplementation(object obj)
        {
            Enum.TryParse(obj.ToString(), false, out _sortField);
            StationManager.DataStorage.Sort(_sortField);
            Processes = new ObservableCollection<SystemProcess>(StationManager.DataStorage.ProcessList);
        }

        private void ShowDetailImplementation(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.Detail);
        }

        private void StopProcessImplementation(object obj)
        {
            Process.GetProcessById(StationManager.CurrentProcess.Id).Kill();
            StationManager.DataStorage.RemoveProcess(StationManager.CurrentProcess);
            Processes = new ObservableCollection<SystemProcess>(StationManager.DataStorage.ProcessList);
            StationManager.CurrentProcess = null;
        }

        private void OpenFolderImplementation(object obj)
        {
            int index = StationManager.CurrentProcess.PathToFile.LastIndexOf("\\");
            if (index == -1)
                return;
            Process.Start(StationManager.CurrentProcess.PathToFile.Substring(0, index));
        }

        private void CloseImplementation(object obj)
        {
            StationManager.CloseApp();
        }
        

        public ObservableCollection<SystemProcess> Processes
        {
            get { return _processes; }
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        public SystemProcess SelectedProcess
        {
            private get { return StationManager.CurrentProcess; }
            set
            {
                StationManager.CurrentProcess = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcessUpdateInfo);
            _workingThreadRefreshList = new Thread(RefreshListProcess);
            _workingThread.Start();
            _workingThreadRefreshList.Start();
        }

        private Process GetProcByID(int id)
        {
            Process[] processlist = Process.GetProcesses();
            return processlist.FirstOrDefault(pr => pr.Id == id);
        }

        private SystemProcess IfSystemProcessExast(int idSystemProcess)
        {
            return Processes.FirstOrDefault(pr => pr.Id == idSystemProcess);
        }

        private void RefreshListProcess()
        {
            int i = 0;
            while (!_token.IsCancellationRequested)
            {
                var systemProcesses = StationManager.DataStorage.ProcessList;
                var resSystemProcesses = StationManager.DataStorage.ProcessList;

                foreach (var systemProcess in systemProcesses)
                {
                    if (GetProcByID(systemProcess.Id) == null)
                        resSystemProcesses.Remove(systemProcess);
                }

                foreach (var process in Process.GetProcesses())
                {
                    if(IfSystemProcessExast(process.Id) == null)
                        resSystemProcesses.Add(new SystemProcess(process));
                }
                
                StationManager.DataStorage.ProcessList = resSystemProcesses.ToList();
                if (_sortField != SortFields.Nothing)
                {
                    StationManager.DataStorage.Sort(_sortField);
                    _sortField = SortFields.Nothing;
                }
                Processes = new ObservableCollection<SystemProcess>(StationManager.DataStorage.ProcessList);

                if (_token.IsCancellationRequested)
                    break;

                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(500);
                    if (_token.IsCancellationRequested)
                        break;
                }
                if (_token.IsCancellationRequested)
                    break;
                i++;

            }
        }

        private void WorkingThreadProcessUpdateInfo()
        {
            while (!_token.IsCancellationRequested)
            {
                foreach (var process in Processes)
                {
                    try
                    {
                        if (Process.GetProcessById(process.Id) != null)
                            process.RefreshInfo(Process.GetProcessById(process.Id));
                    }
                    catch (Exception e)
                    { }
                    
                    if (_token.IsCancellationRequested)
                        break;
                }

                StationManager.DataStorage.ProcessList = Processes.ToList();
                if (_sortField != SortFields.Nothing)
                {
                    StationManager.DataStorage.Sort(_sortField);
                    _sortField = SortFields.Nothing;
                }
                Processes = new ObservableCollection<SystemProcess>(StationManager.DataStorage.ProcessList);

                if (_token.IsCancellationRequested)
                    break;

                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(200);
                    if (_token.IsCancellationRequested)
                        break;
                }
                if (_token.IsCancellationRequested)
                    break;
            }
        }

        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThreadRefreshList.Join(2000);
            _workingThread.Abort();
            _workingThreadRefreshList.Abort();
            _workingThread = null;
            _workingThreadRefreshList = null;

        }
    }
}