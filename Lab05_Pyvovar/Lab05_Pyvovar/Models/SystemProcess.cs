using System;
using System.Diagnostics;
using System.Management;

namespace Lab05_Pyvovar.Models
{
    internal class SystemProcess
    {
        private readonly string _nameProcess;
        private readonly int _id;
        private readonly long _totalMemory = PerformanceInfo.GetTotalMemoryInMiB() * 10000;
        private bool _isActive;
        private double _percentageCPU;
        private double _percentageRAM;
        private double _capacityRAM;
        private int _numberThread;
        private string _nameUser;
        private string _pathToFile;
        private DateTime _startDateTime;
        private string _startDateTimeString;
        private PerformanceCounter performanceCounter;

        private ProcessModuleCollection _modules;
        private ProcessThreadCollection _threads;

        public SystemProcess(Process process)
        {
            _nameProcess = process.ProcessName;
            _id = process.Id;
            performanceCounter = new PerformanceCounter("Process", "% Processor Time", NameProcess, true);
            RefreshInfo(process);
        }

        public string NameProcess => _nameProcess;
        public int Id => _id;

        public bool IsActive
        {
            get { return _isActive; }
            private set { _isActive = value; }
        }
        
        public double PercentageCPU
        {
            get { return _percentageCPU; }
            private set { _percentageCPU = value; }
        }

        public double PercentageRAM
        {
            get { return _percentageRAM; }
            private set { _percentageRAM = value; }
        }

        public double CapacityRAM
        {
            get { return _capacityRAM; }
            private set { _capacityRAM = value; }
        }
        
        public int NumberThread
        {
            get { return _numberThread; }
            private set { _numberThread = value; }
        }
        
        public string NameUser
        {
            get { return _nameUser; }
            private set { _nameUser = value; }
        }
        
        public string PathToFile
        {
            get { return _pathToFile; }
            private set { _pathToFile = value; }
        }
        
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            private set { _startDateTime = value; }
        }

        public string StartDateTimeString
        {
            get { return _startDateTimeString; }
            private set { _startDateTimeString = value; }
        }

        public ProcessModuleCollection Modules
        {
            get { return _modules; }
            private set { _modules = value; }
        }

        public ProcessThreadCollection Threads
        {
            get { return _threads; }
            private set { _threads = value; }
        }

        internal void RefreshInfo(Process process)
        {
            try
            {
                IsActive = process.Responding;
            }
            catch (Exception e)
            { }
            GetNameUser();

            try
            {
                PercentageCPU = performanceCounter.NextValue() / Environment.ProcessorCount;
            }
            catch (Exception e)
            { }

            GetRAM();
            
            try
            {
                NumberThread = process.Threads.Count;
            }
            catch (Exception ex)
            { }

            try
            {
                PathToFile = process.MainModule.FileName;
            }
            catch (Exception ex)
            {
                PathToFile = "";
            }

            try
            {
                StartDateTime = process.StartTime;
                StartDateTimeString = StartDateTime.ToString();
            }
            catch (Exception ex)
            { }

            try
            {
                Threads = process.Threads;
            }
            catch (Exception ex)
            { }

            try
            {
                Modules = process.Modules;
            }
            catch (Exception ex)
            { }

        }
        

        private void GetRAM()
        {
            try
            {
                CapacityRAM = new PerformanceCounter("Process", "Working Set", NameProcess, true).NextValue();
                PercentageRAM = CapacityRAM / _totalMemory;
            }
            catch (Exception e)
            { }
        }

        private void GetNameUser()
        {
            string query = "Select * From Win32_Process Where ProcessID = " + Id;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    NameUser = argList[0];
                    return;
                }
            }

            NameUser = "NO NAME";
            return;
        }
    }
}