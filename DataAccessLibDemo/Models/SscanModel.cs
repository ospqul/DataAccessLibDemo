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
