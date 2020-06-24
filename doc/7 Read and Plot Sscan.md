## 7 Read and Plot Sscan

In order to plot S-scan, the first step is to get NDT raw data information, including each beam's Angle and Ascan data. Then, we define a S-scan plotting area and convert the raw data from polar coordinates (angle-radius) into Cartesian coordinates (XY) .

This tutorial uses [Oxyplot Library](https://github.com/oxyplot/oxyplot) to demonstrate a simple way to plot S-scan in Cartesian coordinates, but you could always use a different plot library and improve the conversion algorithm.

#### 7.1 Get Beam Angles

Here is the `IBeam` class interface from the SDK manual. We are able to get Beam Angle information directly from its `Angle` property.

![IBeamInterface](D:\DataAccessLibDemo\doc\IBeamInterface-1592975204137.png)

Create a `GetBeamAngles()` method to read beam angles.

```c#
// ShellViewModel.cs

public double[] GetBeamAngles()
{
    var beams = dataFile.Channels[SelectedChannelIndex + 1].Beams;
    double[] angles = new double[beams.Count];
    for (int i=0; i< beams.Count; i++)
    {
        angles[i] = beams[i + 1].Angle;
    }
    return angles;
}
```

#### 7.2 Get Sscan Data

Stack Ascan Data from all beams into a 2-d array Sscan data.

```c#
// ShellViewModel.cs

public float[][] GetSscanData()
{
    var beams = dataFile.Channels[SelectedChannelIndex + 1].Beams;
    float[][] data = new float[beams.Count][];
    for (int i = 0; i < beams.Count; i++)
    {
        data[i] = beams[i + 1]
            .Gates[SelectedGateIndex + 1]
            .DataGroups[SelectedDataGroupIndex + 1]
            .DataAccess
            .ReadAscan(SelectedScanValue, SelectedIndexValue);
    }
    return data;
}
```

#### 7.3 Define S-scan Plotting Area

Here is S-scan diagram.

![Sscan Plot Diagram](https://raw.githubusercontent.com/ospqul/DataAccessLibDemo/master/resources/Sscan%20Plot%20Area.PNG?_sm_au_=iVV1JZ5qH4qPWFf6Cq0RGKs1CcqWp)

First, create a new `AxisModel` class to represent S-scan Axis.

```c#
// AxisModel.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibDemo.Models
{
    public class AxisModel
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public int Number { get; set; }
        public double Resolution { get; set; }

        public AxisModel(double min, double max, int number)
        {
            Min = min;
            Max = max;
            Number = number;
            Resolution = (max - min) / (number - 1);
        }

        public double[] GetPoints()
        {
            double[] points = new double[Number];

            for (int i=0; i<Number; i++)
            {
                points[i] = Min + i * Resolution;
            }

            return points;
        }
    }
}
```

Second, create a new `SscanModel` class to represent S-scan Plotting Points.

```c#
// SscanModel.cs

using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibDemo.Models
{
    public class SscanModel
    {
        public AxisModel XAxis { get; set; }
        public AxisModel YAxis { get; set; }

        public SscanModel(AxisModel xAxis, AxisModel yAxis)
        {
            XAxis = xAxis;
            YAxis = yAxis;
        }
		
        // Get the XY positions of all plotting points
        public DataPoint[,] GetDataPoints()
        {
            var xPoints = XAxis.GetPoints();
            var yPoints = YAxis.GetPoints();

            DataPoint[,] dataPoints = new DataPoint[xPoints.Length, yPoints.Length];

            for (int i=0; i<xPoints.Length; i++)
            {
                for (int j=0; j<yPoints.Length; j++)
                {
                    dataPoints[i, j] = new DataPoint(xPoints[i], yPoints[j]);
                }
            }
            return dataPoints;
        }
    }
}
```

Third, define s-scan area in `ShellViewModel.cs`.

```c#
// ShellViewModel.cs

public void InitSscanModel()
{
    AxisModel xAxis = new AxisModel(-10, 100, 1000); // X ranges from -10mm to 100 mm
    AxisModel yAxis = new AxisModel(1e-3, 100, 1000);// Y ranges from -0.001mm to 100 mm
    sscanModel = new SscanModel(xAxis, yAxis); 
    plottingData = new double[xAxis.Number, yAxis.Number];// plotting points: 1000 x 1000
}
```

Fourth, Add S-scan control on GUI.

```xaml
# ShellView.xaml
<!-- Sscan -->
<oxy:PlotView Model="{Binding plotSscanModel}" Margin="10" Height="400"/>
```

Finally, bind and initiate `plotSscanModel` to GUI.

```c#
//Sscan plot
public SscanModel sscanModel { get; set; }
public PlotModel plotSscanModel { get; set; }
public double[,] plottingData { get; set; }
public HeatMapSeries heatmapSeries { get; set; }

public void InitializeSscan()
{
    InitSscanModel();

    plotSscanModel = new PlotModel
    {
        Title = "Sscan plotting",
    };

    var axis = new LinearColorAxis
    {
        Position = AxisPosition.Right,
        StartPosition = 0,
        EndPosition = 1,
    };
    plotSscanModel.Axes.Add(axis);

    heatmapSeries = new HeatMapSeries
    {
        X0 = sscanModel.XAxis.Min,
        X1 = sscanModel.XAxis.Max,
        Y1 = sscanModel.YAxis.Min,
        Y0 = sscanModel.YAxis.Max,
        Interpolate = true,
        RenderMethod = HeatMapRenderMethod.Bitmap,
        Data = plottingData,
    };
    plotSscanModel.Series.Add(heatmapSeries);
}
```

#### 7.4 Get Plotting Data

Loop through each plotting point(1000 x 1000 in this example), and find the closest value in NDT raw data.

```c#
public double GetMaterialVelocity()// velocity of materail, in m/s
{
    return dataFile
        .Channels[SelectedChannelIndex + 1]
        .PartParameters
        .MaterialSoundVelocity;
}

public double GetSampleResolution()// time resolution of samples(ascan data), in s
{
    return dataFile
        .Channels[SelectedChannelIndex + 1]
        .Beams[SelectedBeamIndex + 1]
        .Gates[SelectedGateIndex + 1]
        .DataGroups[SelectedDataGroupIndex + 1]
        .SampleResolution;
}

public double[,] GetPlottingData()
{
    var points = sscanModel.GetDataPoints();
    var data = GetSscanData();
    var angles = GetBeamAngles();
    var velocity = GetMaterialVelocity();
    var sampleRes = GetSampleResolution();

    // loop through each plotting point
    for (int xIndex = 0; xIndex < points.GetLength(0); xIndex++)
    {
        for (int yIndex = 0; yIndex < points.GetLength(1); yIndex++)
        {
            var point = points[xIndex, yIndex];
			// calculate the angle of this plotting point
            double angle = Math.Atan2(point.X, point.Y) * 180 / Math.PI;
			// if plotting point locates outside scan area, assign its value to 0
            if ((angle < angles.Min()) || (angle > angles.Max()))
            {
                plottingData[xIndex, yIndex] = 0;
            }
            else
            {
                double radius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                // find the closest beam to this plotting point
                int rawXIndex = (int)Math.Round(angle - angles.Min());
                // convert from real distance into the index of samples
                int rawYIndex = (int)Math.Round((radius * 1e-3)/ (velocity * sampleRes));
				// if plotting point is further than scan area, assign its value to 0
                if (rawYIndex >= data[rawXIndex].Length)
                {
                    plottingData[xIndex, yIndex] = 0;
                }
                // assign its value to the closet sample value
                else
                {
                    plottingData[xIndex, yIndex] = data[rawXIndex][rawYIndex];
                }
            }
        }
    }
    return plottingData;            
}
```

#### 7.5 Update S-scan with latest data on GUI

```c#
// ShellViewModel.cs

public void PlotSscan()
{
    // Get latest plotting data
    var data = GetPlottingData();
    // refresh plotting
    heatmapSeries.Data = data;
    plotSscanModel.InvalidatePlot(true);
}
```

#### 7.6 Run the code

We can test the code with sample data file in `NDT Data Accessl Library` installation folder. The default path is `C:\Program Files\OlympusNDT\NDT Data Access Library 1.12\Samples\x64\DATA FILE - WELD.opd`.

![ExampleOfSscanPlotting](D:\DataAccessLibDemo\doc\ExampleOfSscanPlotting.png)

#### 7.7 Source Code

Run `git checkout 7_Read_and_Plot_Sscan` to get source code for this section.

