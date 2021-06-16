using System;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// R(1022) = 2201, R(100) = 1. n is Brilliant if n + R(n) is a multiple of 13. B be the 10000th brilliant number.
    /// Compute the last three digits of B.
    /// </summary>
    public class ReversedMultipleAreBrilliant : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/reversed-multiples/");
        }

        void ISolve.Solve()
        {
            long B10000 = 0, N = 10000, bCount = 0;
            for (int i = 1; ; i++)
            {
                long bn = i + Convert.ToInt64(GenericDefs.DotNet.Strings.Reverse(i + ""));
                if (bn % 13 == 0)
                {
                    bCount++;
                }
                if (bCount == N)
                {
                    B10000 = i;
                    break;
                }
            }

            Console.WriteLine("{0}th brilliant number is :: {1}", N, B10000);
            Console.WriteLine("Last three digits is :: {0}", B10000 % 1000);
            Console.ReadKey();
        }
    }
}