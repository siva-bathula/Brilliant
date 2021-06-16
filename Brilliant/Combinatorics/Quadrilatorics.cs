using GenericDefs.DotNet;
using System;

namespace Brilliant.Combinatorics
{
    /// <summary>
    /// </summary>
    public class Quadrilatorics : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/i-prefer-triangles-count-em-all-17/");
        }

        void ISolve.Solve()
        {
            CountingSquares();
        }

        /// <summary>
        /// https://brilliant.org/problems/i-prefer-triangles-count-em-all-17/
        /// </summary>
        internal static void CountEmAll()
        {
            int n = 0; int a = 0;
            while (true)
            {
                n++;
                //a += ((n - 1) * (n - 1) + 1) + (n * (n - 1) / 2) + (n * (n - 1));
                a += (n - 1) * (n + 4) * (30 + 1 - n) / 2;
                if (n == 30) break;
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of quadrilaterals : {0}", a));
        }

        /// <summary>
        /// https://brilliant.org/problems/count-em-all-16/
        /// </summary>
        internal static void CountEmAll16()
        {
            int n = 0;
            int ai = 0;
            int nMax = 30;
            int f23f24f25 = 0;
            while (true)
            {
                n++;
                if (n > nMax) break;
                int counter = 0;
                int diff = 0;
                while (true)
                {
                    diff += counter * (counter + 1) / 2;
                    counter++;
                    if (counter > n) break;
                }
                ai += diff;
                if (n > 23 && n < 27)
                {
                    f23f24f25 += ai;
                }
                QueuedConsole.WriteImmediate("n: {0}, quadrilaterals : {1}", n, ai);
            }
            QueuedConsole.WriteFinalAnswer(string.Format("f(23) + f(24) + f(25) : {0}", f23f24f25));
        }

        /// <summary>
        /// Number of squares in m x n grid. of size 1x1, 2x2, 3x3 so on ...
        /// </summary>
        internal static void CountingSquares()
        {
            long sum = 0;
            int xMax = 8, yMax = 8;
            int i = xMax, j = yMax;

            if (xMax.CompareTo(yMax) < 0)
            {
                i = yMax;
                j = xMax;
            }

            int diff = Math.Abs(xMax - yMax);
            for (; i >= diff; i--, j--)
            {
                sum += i * j;
            }
            QueuedConsole.WriteFinalAnswer("Possible squares of {0} x {1} grid is :: {2}", xMax, yMax, sum);
        }
    }
}