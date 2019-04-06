using System.Collections.Generic;
using System.Linq;
using Lab05_Pyvovar.Models;
using Lab05_Pyvovar.Tools.Managers;

namespace Lab05_Pyvovar
{
    internal static class Sorting
    {
        internal static List<SystemProcess> SortBy(SortFields property)
        {
            switch (property)
            {
                case SortFields.ProcessNameSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.NameProcess).ToList();
                case SortFields.IdSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.Id).ToList();
                case SortFields.IsActiveSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.IsActive).ToList();
                case SortFields.PercentageCPUSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.PercentageCPU).ToList();
                case SortFields.CapacityRAMSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.CapacityRAM).ToList();
                case SortFields.NumberThreadsSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.NumberThread).ToList();
                case SortFields.NameUserSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.NameUser).ToList();
                case SortFields.PathToFileSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.PathToFile).ToList();
                case SortFields.StartDateTimeSort:
                    return StationManager.DataStorage.ProcessList.OrderBy(p => p.StartDateTime).ToList();
                default:
                    return StationManager.DataStorage.ProcessList;
            }
        }
    }
}