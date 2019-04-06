using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lab05_Pyvovar.Models;

namespace Lab05_Pyvovar.Tools.DataStorage
{
    internal class ProcessDataStorage : IDataStorage
    {
        private List<SystemProcess> _systemProcesses;

        internal ProcessDataStorage()
        {
            _systemProcesses = new List<SystemProcess>();
            foreach (var pr in Process.GetProcesses())
            {
                _systemProcesses.Add(new SystemProcess(pr));
            }
        }

        public void AddProcess(SystemProcess process)
        {
            _systemProcesses.Add(process);
        }

        public void RemoveProcess(SystemProcess process)
        {
            _systemProcesses.Remove(process);
        }

        public void Sort(SortFields property)
        {
            _systemProcesses = Sorting.SortBy(property);
        }

        public List<SystemProcess> ProcessList
        {
            get { return _systemProcesses.ToList(); }
            set { _systemProcesses = value; }
        }
    }
}