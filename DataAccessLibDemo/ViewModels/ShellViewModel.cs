using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using RDTiffDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataAccessLibDemo.ViewModels
{
    class ShellViewModel : Screen, IDisposable
    {
        public RDTiffDataFile.RDTiffDataFile dataFile { get; set; }

        // ascan plot
        public PlotModel plotModel { get; set; }
        public LineSeries lineSeries { get; set; }

        public ShellViewModel()
        {
            FilePath = @"C:\Program Files\OlympusNDT\NDT Data Access Library 1.12\Samples\x64\DATA FILE - WELD.opd";
            InitializeAscanPlotting();
        }

        public void OpenFile()
        {
            RDTiffData rdtData = new RDTiffData();
            dataFile = rdtData.RDTiffDataFile;
            dataFile.OpenFile(FilePath);

            GetChannels();
            GetBeams();
            GetGates();
            GetDataGroups();
            PlotAscan();
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

        #region Channel Info

        public void GetChannels()
        {
            var channels = dataFile.Channels;
            for (int i = 1; i <= channels.Count; i++)
            {
                var channel = channels[i];
                ChannelList.Add(channel.Name);
            }

            // set first channel by default
            SelectedChannelIndex = 0;

            // Channel index starts from 1
            GetChannelInfo(SelectedChannelIndex + 1);
        }

        public void GetChannelInfo(int index)
        {
            var channel = dataFile.Channels[index];
            ChannelInfo = "";

            ChannelInfo += $"[Name]: { channel.Name }" + Environment.NewLine;
            ChannelInfo += $"[Type]: { channel.Type }" + Environment.NewLine;
            ChannelInfo += $"[Mode]: { channel.Mode }" + Environment.NewLine;
            ChannelInfo += $"[Averaging]: { channel.Averaging }" + Environment.NewLine;
            ChannelInfo += $"[Compression]: { channel.Compression }" + Environment.NewLine;
            ChannelInfo += $"[Digitizing Frequency]: { channel.DigitizingFrequency }" + Environment.NewLine;
            ChannelInfo += $"[Pulser Voltage]: { channel.PulserVoltage }" + Environment.NewLine;
            ChannelInfo += $"[Pulser Width]: { channel.PulseWidth }" + Environment.NewLine;
            ChannelInfo += $"[Interface Sound Velocity]: { channel.PartParameters.InterfaceSoundVelocity }" + Environment.NewLine;
            ChannelInfo += $"[Material Sound Velocity]: { channel.PartParameters.MaterialSoundVelocity }" + Environment.NewLine;
            ChannelInfo += $"[Probe Delay]: { channel.PartParameters.ProbeDelay }" + Environment.NewLine;
            ChannelInfo += $"[InspectionType]: { channel.PartParameters.InspectionType }" + Environment.NewLine;
        }

        private int _selectedChannelIndex;

        public int SelectedChannelIndex
        {
            get { return _selectedChannelIndex; }
            set
            { 
                _selectedChannelIndex = value;
                NotifyOfPropertyChange(() => SelectedChannelIndex);
                if(dataFile != null)
                {
                    GetChannelInfo(_selectedChannelIndex + 1); // channel index starts from 1
                    if (plotModel != null)
                    {
                        PlotAscan();
                    }
                }
            }
        }

        private BindableCollection<string> _channelList = new BindableCollection<string>();

        public BindableCollection<string> ChannelList
        {
            get { return _channelList; }
            set { _channelList = value; }
        }


        private string _channelInfo;

        public string ChannelInfo
        {
            get { return _channelInfo; }
            set
            { 
                _channelInfo = value;
                NotifyOfPropertyChange(() => ChannelInfo);
            }
        }

        #endregion

        #region Beam Info

        public void GetBeams()
        {
            var beams = dataFile.Channels[SelectedChannelIndex + 1].Beams;
            for (int i = 1; i <= beams.Count; i++)
            {
                var beam = beams[i];
                BeamList.Add(beam.Name);
            }

            // set first beam by default
            SelectedBeamIndex = 0;

            // Beam index starts from 1
            GetBeamInfo(SelectedBeamIndex + 1);
        }

        public void GetBeamInfo(int index)
        {
            var beam = dataFile.Channels[SelectedChannelIndex + 1].Beams[index];
            BeamInfo = "";

            BeamInfo += $"[Name]: { beam.Name }" + Environment.NewLine;
            BeamInfo += $"[Angle]: { beam.Angle }" + Environment.NewLine;
            BeamInfo += $"[Delay]: { beam.Delay }" + Environment.NewLine;
            BeamInfo += $"[Gain]: { beam.Gain }" + Environment.NewLine;
            BeamInfo += $"[ReferenceIndexOffset]: { beam.ReferenceIndexOffset }" + Environment.NewLine;
            BeamInfo += $"[ReferenceScanOffset]: { beam.ReferenceScanOffset }" + Environment.NewLine;
            BeamInfo += $"[Skew]: { beam.Skew }" + Environment.NewLine;
            BeamInfo += $"[TotalReferenceIndexOffset]: { beam.TotalReferenceIndexOffset }" + Environment.NewLine;
            BeamInfo += $"[TotalReferenceScanOffset]: { beam.TotalReferenceScanOffset }" + Environment.NewLine;
            BeamInfo += $"[TVReferenceIndexOffset]: { beam.TVReferenceIndexOffset }" + Environment.NewLine;
            BeamInfo += $"[TVReferenceScanOffset]: { beam.TVReferenceScanOffset }" + Environment.NewLine;
        }

        private int _selectedBeamIndex;

        public int SelectedBeamIndex
        {
            get { return _selectedBeamIndex; }
            set
            {
                _selectedBeamIndex = value;
                NotifyOfPropertyChange(() => SelectedBeamIndex);
                if (dataFile != null)
                {
                    GetBeamInfo(_selectedBeamIndex + 1); // beam index starts from 1
                    if (plotModel != null)
                    {
                        PlotAscan();
                    }
                }
            }
        }

        private BindableCollection<string> _beamList = new BindableCollection<string>();

        public BindableCollection<string> BeamList
        {
            get { return _beamList; }
            set { _beamList = value; }
        }


        private string _beamInfo;

        public string BeamInfo
        {
            get { return _beamInfo; }
            set
            {
                _beamInfo = value;
                NotifyOfPropertyChange(() => BeamInfo);
            }
        }

        #endregion

        #region Gate Info

        public void GetGates()
        {
            var gates = dataFile
                .Channels[SelectedChannelIndex + 1]
                .Beams[SelectedBeamIndex + 1]
                .Gates;

            for (int i = 1; i <= gates.Count; i++)
            {
                var gate = gates[i];
                GateList.Add(gate.Name);
            }

            // set first gate by default
            SelectedGateIndex = 0;

            // Beam index starts from 1
            GetGateInfo(SelectedGateIndex + 1);
        }

        public void GetGateInfo(int index)
        {
            var gate = dataFile
                .Channels[SelectedChannelIndex + 1]
                .Beams[SelectedBeamIndex + 1]
                .Gates[index];

            GateInfo = "";

            GateInfo += $"[Name]: { gate.Name }" + Environment.NewLine;
            GateInfo += $"[Type]: { gate.Type }" + Environment.NewLine;
            GateInfo += $"[Start]: { gate.Start }" + Environment.NewLine;
            GateInfo += $"[Width]: { gate.Width }" + Environment.NewLine;
            GateInfo += $"[Level]: { gate.Level }" + Environment.NewLine;
            GateInfo += $"[Peak Selection]: { gate.PeakSelection }" + Environment.NewLine;
            GateInfo += $"[Rectification]: { gate.Rectification }" + Environment.NewLine;
            GateInfo += $"[Reference Amplitude]: { gate.ReferenceAmplitude }" + Environment.NewLine;
        }

        private int _selectedGateIndex;

        public int SelectedGateIndex
        {
            get { return _selectedGateIndex; }
            set
            {
                _selectedGateIndex = value;
                NotifyOfPropertyChange(() => SelectedGateIndex);
                if (dataFile != null)
                {
                    GetGateInfo(_selectedGateIndex + 1); // gate index starts from 1
                    if (plotModel != null)
                    {
                        PlotAscan();
                    }
                }
            }
        }

        private BindableCollection<string> _gateList = new BindableCollection<string>();

        public BindableCollection<string> GateList
        {
            get { return _gateList; }
            set { _gateList = value; }
        }


        private string _gateInfo;

        public string GateInfo
        {
            get { return _gateInfo; }
            set
            {
                _gateInfo = value;
                NotifyOfPropertyChange(() => GateInfo);
            }
        }

        #endregion

        #region DataGroup Info

        public void GetDataGroups()
        {
            var dataGroups = dataFile
                .Channels[SelectedChannelIndex + 1]
                .Beams[SelectedBeamIndex + 1]
                .Gates[SelectedGateIndex + 1]
                .DataGroups;

            for (int i = 1; i <= dataGroups.Count; i++)
            {
                var dataGroup = dataGroups[i];
                DataGroupList.Add(dataGroup.Name);
            }

            // set first data group by default
            SelectedDataGroupIndex = 0;

            // Beam index starts from 1
            GetDataGroupInfo(SelectedDataGroupIndex + 1);
        }

        public void GetDataGroupInfo(int index)
        {
            var dataGroup = dataFile
                .Channels[SelectedChannelIndex + 1]
                .Beams[SelectedBeamIndex + 1]
                .Gates[SelectedGateIndex + 1]
                .DataGroups[index];

            DataGroupInfo = "";

            DataGroupInfo += $"[Name]: { dataGroup.Name }" + Environment.NewLine;
            DataGroupInfo += $"[Type]: { dataGroup.Type }" + Environment.NewLine;
            DataGroupInfo += $"[DataGroupMode]: { dataGroup.DataGroupMode }" + Environment.NewLine;
            DataGroupInfo += $"[DataOffset]: { dataGroup.DataOffset }" + Environment.NewLine;
            DataGroupInfo += $"[DataRectification]: { dataGroup.DataRectification }" + Environment.NewLine;
            DataGroupInfo += $"[DataResolution]: { dataGroup.DataResolution }" + Environment.NewLine;
            DataGroupInfo += $"[IndexQuantity]: { dataGroup.IndexQuantity }" + Environment.NewLine;
            DataGroupInfo += $"[IndexResolution]: { dataGroup.IndexResolution }" + Environment.NewLine;
            DataGroupInfo += $"[IndexUnit]: { dataGroup.IndexUnit }" + Environment.NewLine;
            DataGroupInfo += $"[ScanQuantity]: { dataGroup.ScanQuantity }" + Environment.NewLine;
            DataGroupInfo += $"[ScanResolution]: { dataGroup.ScanResolution }" + Environment.NewLine;
            DataGroupInfo += $"[ScanUnit]: { dataGroup.ScanUnit }" + Environment.NewLine;
            DataGroupInfo += $"[SampleQuantity]: { dataGroup.SampleQuantity }" + Environment.NewLine;
            DataGroupInfo += $"[SampleResolution]: { dataGroup.SampleResolution }" + Environment.NewLine;
            DataGroupInfo += $"[SampleUnit]: { dataGroup.SampleUnit }" + Environment.NewLine;

            // update Scan Quantity and Index Quantity
            ScanQuantity = dataGroup.ScanQuantity - 1; // starts from 0
            IndexQuantity = dataGroup.IndexQuantity - 1; 
        }

        private int _selectedDataGroupIndex;

        public int SelectedDataGroupIndex
        {
            get { return _selectedDataGroupIndex; }
            set
            {
                _selectedDataGroupIndex = value;
                NotifyOfPropertyChange(() => SelectedDataGroupIndex);
                if (dataFile != null)
                {
                    GetDataGroupInfo(_selectedDataGroupIndex + 1); // DataGroup index starts from 1
                    if (plotModel != null)
                    {
                        PlotAscan();
                    }
                }
            }
        }

        private BindableCollection<string> _dataGroupList = new BindableCollection<string>();

        public BindableCollection<string> DataGroupList
        {
            get { return _dataGroupList; }
            set { _dataGroupList = value; }
        }


        private string _dataGroupInfo;

        public string DataGroupInfo
        {
            get { return _dataGroupInfo; }
            set
            {
                _dataGroupInfo = value;
                NotifyOfPropertyChange(() => DataGroupInfo);
            }
        }

        #endregion

        #region Ascan Plotting

        private int _selectedScanValue;

        public int SelectedScanValue
        {
            get { return _selectedScanValue; }
            set 
            {
                _selectedScanValue = value;
                NotifyOfPropertyChange(() => SelectedScanValue);
                if ((plotModel != null) && (dataFile != null))
                {
                    PlotAscan();
                }
            }
        }

        private int _selectedIndexValue = 0;

        public int SelectedIndexValue
        {
            get { return _selectedIndexValue; }
            set 
            {
                _selectedIndexValue = value;
                NotifyOfPropertyChange(() => SelectedIndexValue);
                if ((plotModel != null) && (dataFile != null))
                {
                    PlotAscan();
                }
            }
        }

        private int _scanQuantity = 0;

        public int ScanQuantity
        {
            get { return _scanQuantity; }
            set
            { 
                _scanQuantity = value;
                NotifyOfPropertyChange(() => ScanQuantity);
            }
        }

        private int _indexQuantity;

        public int IndexQuantity
        {
            get { return _indexQuantity; }
            set 
            {
                _indexQuantity = value;
                NotifyOfPropertyChange(() => IndexQuantity);
            }
        }

        public void InitializeAscanPlotting()
        {
            plotModel = new PlotModel
            {
                Title = "Ascan Plotting",
            };

            LinearAxis xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
            };
            plotModel.Axes.Add(xAxis);

            LinearAxis yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
            };
            plotModel.Axes.Add(yAxis);

            lineSeries = new LineSeries
            {
                Title = "Ascan Data",
                Color = OxyColors.Blue,
                StrokeThickness = 1.5,
            };
            plotModel.Series.Add(lineSeries);
        }

        public void PlotAscan()
        {
            try
            {
                var ascanData = dataFile
                    .Channels[SelectedChannelIndex + 1]
                    .Beams[SelectedBeamIndex + 1]
                    .Gates[SelectedGateIndex + 1]
                    .DataGroups[SelectedDataGroupIndex + 1]
                    .DataAccess
                    .ReadAscan(SelectedScanValue, SelectedIndexValue);

                lock (plotModel.SyncRoot)
                {
                    lineSeries.Points.Clear();
                    for (int i = 0; i < ascanData.Length; i++)
                    {
                        lineSeries.Points.Add(new DataPoint((double)i, (double)ascanData[i]));
                    }
                    plotModel.InvalidatePlot(true);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion
    }
}
