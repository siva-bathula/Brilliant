using GenericDefs.DotNet;
using System.Linq;

namespace Brilliant.ComputerScience._5
{
    public class SelfDescribingSequence : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/self-describing-sequence/");
        }
        
        void ISolve.Solve()
        {
            long n2Count = 0;

            System.Text.StringBuilder sequence = new System.Text.StringBuilder();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            sequence.Append("122");
            int n = 2;
            int seqNext = 2;
            while (true)
            {
                if (seqNext == 2) seqNext = 1;
                else if (seqNext == 1) seqNext = 2;
                sequence.Append(seqNext + "");
                if (char.GetNumericValue(sequence[n]) == 2) sequence.Append("" + seqNext);

                if (sequence.Length >= 1000000) break;
                n++;
            }
            n2Count = (sequence.ToString().Substring(0, 1000000)).Count(x => char.GetNumericValue(x) == 2);
            sw.Stop();
            QueuedConsole.WriteImmediate("Total time taken : " + sw.ElapsedMilliseconds * 1.0 / 1000 + " seconds.");
            QueuedConsole.WriteFinalAnswer("Number of 2's in the first 1 million of the sequence is :: {0}", n2Count);
        }
    }
}