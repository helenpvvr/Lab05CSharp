using Lab05_Pyvovar.Models;
using System.Collections.Generic;

namespace Lab05_Pyvovar.Tools.DataStorage
{
    internal interface IDataStorage
    {
        void AddProcess(SystemProcess process);
        void RemoveProcess(SystemProcess process);
        List<SystemProcess> ProcessList { get; set; }

        void Sort(SortFields sortField);
    }
}