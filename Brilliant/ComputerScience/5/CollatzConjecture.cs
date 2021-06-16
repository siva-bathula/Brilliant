using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class CollatzConjecture : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/collatz-conjecture-an-odd-number-of-steps/");
        }

        /// <summary>
        /// f(n) = n/2 if n%2 == 0 and f(n) = 3n + 1 if n%2 == 1
        /// </summary>
        void ISolve.Solve()
        {
            long nCount = 0;
            for (long i = 2; i < 10000000; i++) {
                BigInteger fkn = i;
                long kCounter = 0;
                do
                {
                    kCounter++;
                    fkn = Function(fkn);

                    if (fkn == 1) break;
                } while (fkn != 1);

                if (kCounter % 2 == 1)
                    nCount++;
            }

            Console.WriteLine("Number of n between 1 and 10000000 is :: {0}", nCount);
            Console.ReadKey();
        }

        BigInteger Function(BigInteger n) {
            if (n % 2 == 0) return n / 2;
            else return 3 * n + 1;
        }
    }
}