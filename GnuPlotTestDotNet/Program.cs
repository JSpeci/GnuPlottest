using AwokeKnowing.GnuplotCSharp;
using ENVIS.Model;
using System;
using System.Collections.Generic;
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
            ;

            List<UniArchiveBase> result = new List<UniArchiveBase>();

            for(int i = 0; i < 10000; i++)
            {
                result.Add(reader.LoadNext());
            }

            // values restricted to one day 1.4.2018
            var hours = reader.values.Where(i => i.Key.Day == 1 && i.Key.Month == 4).Select(i => double.Parse(i.Key.Hour.ToString())).ToArray(); // 8644 values for each 10 seconds
            var values = reader.values.Where(i => i.Key.Day == 1 && i.Key.Month == 4).Select(i => double.Parse(i.Value)).ToArray(); // 8644 values for each 10 seconds
            var values2 = result.Select(i => double.Parse(i.GetMemberValue("I_avg_3I").ToString())).Take(8644).ToArray();
            var values3 = result.Select(i => double.Parse(i.GetMemberValue("U_avg_U3").ToString())).Take(8644).ToArray();

            string tempfolder = System.IO.Path.GetTempPath();

            //set output to pdf
            GnuPlot.Set("terminal pdf size 10in,8in");
            GnuPlot.Set("output \"" + tempfolder + "P_avg_3P_C_fromGnuPlot.pdf\"");

            //set common properties of plot
            GnuPlot.Set("multiplot layout 3, 1 title \"1.4.2018\" font \", 14\"");
            GnuPlot.Set("tmargin 3");
            GnuPlot.Set("bmargin 3");
            GnuPlot.Set("rmargin 3");
            GnuPlot.Set("lmargin 3");

            //data
            GnuPlot.SaveData(hours, values, tempfolder + "plot1.data");
            GnuPlot.SaveData(hours, values2, tempfolder + "plot2.data");
            GnuPlot.SaveData(hours, values3, tempfolder + "plot3.data");

            //first line
            GnuPlot.Set("xrange  [0:24]");
            GnuPlot.Set("xtics 1");
            GnuPlot.Set("ytics 600");
            GnuPlot.Set("grid");
            GnuPlot.Set("title \"1.4.2018 řada P-avg-3P-C\"");
            GnuPlot.Set("ylabel  \"P(W)\"");
            GnuPlot.Set("xlabel  \"dayhours (h)\"");
            GnuPlot.Unset("key");
            GnuPlot.Plot(tempfolder + "plot1.data", "with linespoints pt " + (int)PointStyles.Dot + " lt 6 lw 4");

            //second line
            GnuPlot.Set("xrange  [0:24]");
            GnuPlot.Set("xtics 1");
            GnuPlot.Set("ytics 1");
            GnuPlot.Set("grid");
            GnuPlot.Set("title \"1.4.2018 řada I-avg-3I\"");
            GnuPlot.Set("ylabel  \"I(A)\"");
            GnuPlot.Set("xlabel  \"dayhours (h)\"");
            GnuPlot.Unset("key");
            GnuPlot.Plot(tempfolder + "plot2.data", "with linespoints pt " + (int)PointStyles.Dot + " lt 4 lw 4");

            //third line
            GnuPlot.Set("xrange  [0:24]");
            GnuPlot.Set("yrange  [220:250]");
            GnuPlot.Set("xtics 1");
            GnuPlot.Set("ytics 5");
            GnuPlot.Set("grid");
            GnuPlot.Set("title \"1.4.2018 řada U-avg-U3\"");

            GnuPlot.Set("ylabel  \"U(V)\"");
            GnuPlot.Set("xlabel  \"dayhours (h)\"");
            GnuPlot.Unset("key");
            GnuPlot.Plot(tempfolder + "plot3.data", "with linespoints pt " + (int)PointStyles.Dot + " lt 2 lw 4");


            //GnuPlot.Set("terminal png size 800,600");
            //GnuPlot.Set("output \"P_avg_3P_C_fromGnuPlot.png\"");
            //GnuPlot.Replot();
            //GnuPlot.Plot(tempfolder + "plot1.data", "with linespoints pt " + (int)PointStyles.Dot + " lt 6 lw 4");

            Console.ReadLine();
        }
    }
}

