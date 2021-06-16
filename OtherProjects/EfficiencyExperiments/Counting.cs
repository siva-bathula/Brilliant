using GenericDefs.DotNet;
using System;
using System.Diagnostics;

namespace OtherProjects.EfficiencyExperiments
{
    public class Counting
    {
        public void Run() { CountTill10Pow10(); }
        void CountTill10Pow10()
        {
            long n = 0;
            long nMax = (long)Math.Pow(10, 10);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (n < nMax) { n++; }
            sw.Stop();
            QueuedConsole.WriteImmediate("Time taken to count till {0} is : {1} milli sec. or {2} sec.", n, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds * 1.0 / 1000);
        }
        void CountTill10Pow11()
        {
            long n = 0;
            long nMax = (long)Math.Pow(10, 11);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (n < nMax) { n++; }
            sw.Stop();
            QueuedConsole.WriteImmediate("Time taken to count till {0} is : {1} milli sec. or {2} sec.", n, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds * 1.0 / 1000);
        }
    }
}
