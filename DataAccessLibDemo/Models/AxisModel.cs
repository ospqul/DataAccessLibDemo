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
