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
            var hours = reader.values.Where(i => i.Key.Day == 1 && i.Key.Month == 4).Select(i => double.Parse(i.Key.Hour.ToString())).ToArray(); // 8644 values for each 10 seconds
            var values = reader.values.Where(i => i.Key.Day == 1 && i.Key.Month == 4).Select(i => double.Parse(i.Value)).ToArray(); // 8644 values for each 10 seconds

            string tempfolder = System.IO.Path.GetTempPath();

            GnuPlot.Set("xrange  [0:24]");
            GnuPlot.Set("xtics 1");
            GnuPlot.Set("ytics 500");
            GnuPlot.Set("grid");
            GnuPlot.Set("title \"1.4.2018 řada P-avg-3P-C\"");
            GnuPlot.Set("ylabel  \"P(W)\"");
            GnuPlot.Set("xlabel  \"dayhours (h)\"");
            GnuPlot.Set("terminal pdf size 10in,8in");
            GnuPlot.Set("output \"P_avg_3P_C_fromGnuPlot.pdf\"");

            GnuPlot.SaveData(hours, values, tempfolder + "plot1.data");
            GnuPlot.Plot(tempfolder + "plot1.data", "with linespoints pt " + (int)PointStyles.SolidDiamond + " lt 2 lw 2");

            //Console.ReadLine();
        }
    }
}

