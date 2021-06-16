using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions;

namespace Brilliant.NumberTheory
{
    public class TwoLastDigitsBeautiful : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/2-last-digits/?group=NFPkEi4AI2CQ");
        }

        void ISolve.Solve()
        {
            List<int> primes = Prime.GeneratePrimesSieveOfSundaram(100);
            List<int> wantedNums = new List<int>();
            for (int j = 1; j <= 1000; j++) {
                int k1 = j % 10, k2 = 0;
                if (k1 == 1 || k1 == 3 || k1 == 7 || k1 == 9) {
                    k2 = (j * j) % 100;
                    if (primes.Contains(k2)) {
                        if (!wantedNums.Contains(k2)) { wantedNums.Add(k2); }
                    }
                }
            }

            Console.WriteLine("Number of beautiful numbers : {0}", wantedNums.Count);
            int tSum = 0;
            foreach (int i in wantedNums) {
                tSum += i;
                Console.Write(" {0}, ", i);
            }
            Console.WriteLine("Total sum of beautiful numbers : {0} ", tSum);
            Console.ReadKey();
        }
    }
}