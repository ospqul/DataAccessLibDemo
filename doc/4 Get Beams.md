## 4 Get Beam Info

#### 4.1 Add Beam Selection ComboBox

Add a ComboBox in GUI for beam selection if there are multiple beams in a channel.

```xaml
# ShellView.xaml

<Label Content="Beam"/>
<ComboBox x:Name="BeamList" Width="200"
          SelectedIndex="{Binding SelectedBeamIndex}"/>
```

Bind this ComboBox in `ShellViewModel.cs`

```c#
// ShellViewModel.cs

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
```

#### 4.2 Add method to find all beams

```c#
// ShellViewModel.cs

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
```

#### 4.3 Add TextBlock To Display Selected Beam's details

```xaml
# ShellView.xaml

<Border BorderThickness="1" BorderBrush="Black"
        Margin="5" Height="200">
    <ScrollViewer>
        <TextBlock x:Name="BeamInfo"/>
    </ScrollViewer>
</Border>
```

Bind `BeamInfo` in`ShellViewModel.cs`

```c#
// ShellViewModel.cs        

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
```

#### 4.4 Add Method to update BeamInfo

```c#
// ShellViewModel.cs        

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
```

#### 4.5 Find all beams when file is opened

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
    // Find beams
    GetBeams();
}
```

![](https://raw.githubusercontent.com/ospqul/DataAccessLibDemo/master/resources/GetBeamInfo.PNG?_sm_au_=iVV1JZ5qH4qPWFf6Cq0RGKs1CcqWp)

#### 4.6 Source Code

Run `git checkout 4_Get_Beams` .

