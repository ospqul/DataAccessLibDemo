using Caliburn.Micro;
using RDTiffDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibDemo.ViewModels
{
    class ShellViewModel : Screen, IDisposable
    {
        public RDTiffDataFile.RDTiffDataFile dataFile { get; set; }

        public ShellViewModel()
        {
            FilePath = @"C:\Program Files\OlympusNDT\NDT Data Access Library 1.12\Samples\x64\DATA FILE - WELD.opd";
        }

        public void OpenFile()
        {
            RDTiffData rdtData = new RDTiffData();
            dataFile = rdtData.RDTiffDataFile;
            dataFile.OpenFile(FilePath);
        }

        public void Dispose()
        {
            dataFile.CloseFile();
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set 
            {
                _filePath = value;
                NotifyOfPropertyChange(() => FilePath);
            }
        }        
    }
}
