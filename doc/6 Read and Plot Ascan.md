## 6 Read and Plot Ascan

There are a lot C# plotting libraries available in the market, for example, [Oxyplot](https://github.com/oxyplot/oxyplot), [Live Charts](https://lvcharts.net/), and [InteractiveDataDisplay](https://github.com/microsoft/InteractiveDataDisplay.WPF) from Microsoft. You could pick whichever plotting library you are familiar with.

This course takes Oxyplot as an example to show you how to plot Ascan, and future courses will show how to plot other scan views with Oxyplot.

#### 6.1 Install Oxyplot package

Go to `Manage NuGet Packages`, search "Oxyplot", and install latest version of  `Oxyplot.Wpf`.

#### 6.2 Add oxyplot control in GUI

Add `xmlns:oxy="http://oxyplot.org/wpf"` to `Window` 's property in `ShellView.xaml`.

1. Add a `Scan` slider and `Index` slider to select an A-scan data to plot;
2. Add oxyplot view;

```xaml
# ShellView.xaml
<!-- Plotting -->
<Grid Grid.Row="2" Grid.Column="2" Margin="10">
    <StackPanel>
        <StackPanel Orientation="Horizontal">
            <Label Content="Scan"/>
            <Slider Minimum="0" Maximum="{Binding ScanQuantity}"
                    Value="{Binding SelectedScanValue}" Width="200"/>
            <Label Content="Index"/>
            <Slider Minimum="0" Maximum="{Binding IndexQuantity}"
                    Value="{Binding SelectedIndexValue}" Width="200"/>
        </StackPanel>
        <oxy:PlotView Model="{Binding plotModel}" Margin="10" Height="300"/>
    </StackPanel>
</Grid>
```

3. Bind `SelectedScanValue` and `SelectedIndexValue` in `ShellViewModel.cs`;

   ```c#
   // ShellViewModel.cs
   
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
   ```

4. Bind PlotView Model in `ShellViewModel.cs`.

```c#
// ShellViewModel.cs

public PlotModel plotModel { get; set; }
public LineSeries lineSeries { get; set; }
```

#### 6.3 Initialize PlotView Model

Initialize Oxyplot PlotView Model in `ShellViewModel` class constructor.

Initiate an instance for Oxyplot PlotModel, its name, **plotModel**, should match `Model="{Binding plotModel}"`.

Add the following using statements in header.

```c#
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
```

Create `InitializeAscanPlotting()` method to initiate Ascan plotting.

```c#
// ShellViewModel.cs    
    
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
```

#### 6.4 Read and Plot Ascan data

Use the selected values on GUI to decide which Ascan data is to plot.

```c#
var ascanData = dataFile
    .Channels[SelectedChannelIndex + 1]
    .Beams[SelectedBeamIndex + 1]
    .Gates[SelectedGateIndex + 1]
    .DataGroups[SelectedDataGroupIndex + 1]
    .DataAccess
    .ReadAscan(SelectedScanValue, SelectedIndexValue);
```

Create `PlotAscan()` method to get selected Ascan data and plot on GUI.

```c#
// ShellViewModel.cs

public void PlotAscan()
{
    try
    {
        // Read selected Ascan Data
        var ascanData = dataFile
            .Channels[SelectedChannelIndex + 1]
            .Beams[SelectedBeamIndex + 1]
            .Gates[SelectedGateIndex + 1]
            .DataGroups[SelectedDataGroupIndex + 1]
            .DataAccess
            .ReadAscan(SelectedScanValue, SelectedIndexValue);

        // prevent modification when updating Ascan plot
        lock (plotModel.SyncRoot)
        {
            // clear old points
            lineSeries.Points.Clear();
            // add new points
            for (int i = 0; i < ascanData.Length; i++)
            {
                lineSeries.Points.Add(new DataPoint((double)i, (double)ascanData[i]));
            }
            // update Ascan plotting
            plotModel.InvalidatePlot(true);
        }
    }
    catch(Exception e)
    {
        MessageBox.Show(e.Message);
    }
}
```

#### 6.5 Source Code

Run `git checkout 6_Read_and_Plot_Ascan` to get source code for this section.

