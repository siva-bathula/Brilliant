using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.NumberTheory._5
{
    /// <summary>
    /// Find the sum of digits of the least positive integer whose first digit is 7 and which is reduced 
    /// to a third of its original value when its first digit is transferred to the end.
    /// </summary>
    public class Problem23 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/problem-23/");
        }

        void ISolve.Solve()
        {
            BigInteger b = new BigInteger(2);

            while ((b - 1) % 29 != 0)
            {
                if (b == 2) { b = 1; }
                b *= 10;
            }
            b = 21 * (b - 1) / 29;
            Console.WriteLine("A is :: {0}", b.ToString());

            long sum = 0;
            while (b != 0)
            {
                BigInteger remainder;
                b = BigInteger.DivRem(b, 10, out remainder);
                sum += (int)remainder;
            }

            Console.WriteLine("Sum is :: {0}", sum);
            Console.ReadKey();
        }
    }
}