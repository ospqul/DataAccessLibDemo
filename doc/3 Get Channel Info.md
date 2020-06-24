## 3 Get Channel Info

#### 3.1 Add Channel Selection ComboBox

Add a ComboBox in GUI for channel selection if there are multiple channels in data file.

```xaml
# ShellView.xaml

<Label Content="Channel"/>
<ComboBox x:Name="ChannelList" Width="200" 
          SelectedIndex="{Binding SelectedChannelIndex}"/>
```

Bind this ComboBox in `ShellViewModel.cs`

```c#
// ShellViewModel.cs

private int _selectedChannelIndex;
public int SelectedChannelIndex
{
    get { return _selectedChannelIndex; }
    set
    { 
        _selectedChannelIndex = value;
        NotifyOfPropertyChange(() => SelectedChannelIndex);
        // Update Info when channel is changed
        if(dataFile != null)
        {
            // channel index starts from 1
            GetChannelInfo(_selectedChannelIndex + 1); 
        }
    }
}

private BindableCollection<string> _channelList 
    = new BindableCollection<string>();

public BindableCollection<string> ChannelList
{
    get { return _channelList; }
    set { _channelList = value; }
}
```

#### 3.2 Add method to find all channels

```c#
// ShellViewModel.cs

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
}
```

#### 3.3 Add TextBlock To Display Selected Channel's details

```xaml
# ShellView.xaml

<Border BorderThickness="1" BorderBrush="Black" Margin="5" Height="200">
    <ScrollViewer>
        <TextBlock x:Name="ChannelInfo"/>
    </ScrollViewer>
</Border>
```

Bind `ChannelInfo` in`ShellViewModel.cs`

```c#
// ShellViewModel.cs        

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
```

#### 3.4 Add Method to update ChannelInfo

```c#
// ShellViewModel.cs        

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
```

#### 3.5 Find all channels when file is opened

Add `GetChannels()` in `OpenFile()`.

```c#
// ShellViewModel.cs

public void OpenFile()
{
    RDTiffData rdtData = new RDTiffData();
    dataFile = rdtData.RDTiffDataFile;
    dataFile.OpenFile(FilePath);
    
    // Find channels
    GetChannels();
}
```

![GetChannelInfo](https://raw.githubusercontent.com/ospqul/DataAccessLibDemo/master/resources/GetChannelInfo.PNG?_sm_au_=iVV1JZ5qH4qPWFf6Cq0RGKs1CcqWp)

#### 3.6 Source Code

Run `git checkout 3_Get_channel_info` .

