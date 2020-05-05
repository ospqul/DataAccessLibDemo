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

            GetChannels();
            GetBeams();
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
    }
}
