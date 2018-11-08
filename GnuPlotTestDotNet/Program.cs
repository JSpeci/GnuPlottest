using AwokeKnowing.GnuplotCSharp;
using System;
using System.Linq;
using System.Threading;

namespace GnuPlotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\King\\Desktop\\EnvisBackUps\\2018-04.cea";
            MyDataReader reader = new MyDataReader();
            reader.LoadCea(path);

            for (int i = 0; i < 10000; i++)
            {
                var loadedRow = reader.LoadNext();
            }

            // values restricted to one day 1.4.2018
            var times = reader.values.Where(i => i.Key.Day == 1 && i.Key.Month == 4).Select(i => double.Parse(i.Key.Hour.ToString())).ToArray(); // 8644 values for each 10 seconds
            var values = reader.values.Where(i => i.Key.Day == 1 && i.Key.Month == 4).Select(i => double.Parse(i.Value)).ToArray(); // 8644 values for each 10 seconds

            double[] X = new double[] { -10, -8.5, -2, 1, 6, 9, 10, 14, 15, 19 };
            double[] Y = new double[] { -4, 6.5, -2, 3, -8, -5, 11, 4, -5, 10 };
            string tempfolder = System.IO.Path.GetTempPath();

            GnuPlot.Set("xrange  [0:24]");
            GnuPlot.Set("xtics 1");
            GnuPlot.Set("ytics 500");
            GnuPlot.Set("grid");
            GnuPlot.SaveData(times, values, tempfolder + "plot1.data");
            GnuPlot.Plot(tempfolder + "plot1.data", "with linespoints pt " + (int)PointStyles.SolidDiamond);
        }
    }
}

