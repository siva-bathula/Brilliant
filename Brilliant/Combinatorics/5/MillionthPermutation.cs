using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics._5
{
    public class MillionthPermutation : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/is-it-really-a-symmetry/");
        }

        void ISolve.Solve()
        {
            long n = 123456789;
            int counter = 0;
            long nMax = 9876543210;
            while (true)
            {
                if (GenericDefs.Functions.Numbers.GenericDigitChecker(n, false, true)) counter++;
                if (counter == 1000000) break;
                if (n > nMax) break;
                n++;
            }

            QueuedConsole.WriteFinalAnswer("Counter is :: " + n);
            QueuedConsole.WriteFinalAnswer(string.Format("{0}th permutation is :: {1}", counter, n));
        }
    }
}