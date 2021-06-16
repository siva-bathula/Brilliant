using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions;
using GenericDefs.Classes;

namespace Brilliant.ComputerScience._4
{
    public class kperfectnumbers : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/positively-described-numbers-part-2/");
        }

        void ISolve.Solve()
        {
            int[] foundAll = new int[6] { 0, 0, 0, 0, 0, 0 };
            long aisum = 0;
            long num = 0, i = 0; int faSum = 0;
            while (faSum < 5)
            {
                i++;
                num = i;

                List<long> pFactors = Factors.GetPrimeFactors(num);
                List<long> divisors = new List<long>();
                divisors.Add(1);
                foreach (long f in pFactors)
                {
                    while (num % f == 0)
                    {
                        num /= f;
                        divisors.Add(f);
                        if (num == 1) break;
                    }
                }

                var pSet = PowerSet.GetPowerSet<long>(divisors);
                HashSet<long> factors = new HashSet<long>();
                foreach (var p in pSet)
                {
                    var factor = p.Select(x => x).Aggregate((long)1, (x, y) => x * y);
                    factors.Add(factor);
                }

                long fSum = factors.Sum(y => y);

                if (fSum % i == 0)
                {
                    long k = fSum / i;

                    if (k <= 5)
                    {
                        if (foundAll[k] == 0)
                        {
                            foundAll[k] = 1;
                            faSum++;
                            aisum += i;
                        }
                    }
                }

                if (faSum == 5) { break; }
            }

            Console.WriteLine("Aisum is :: {0}", aisum);
            Console.WriteLine("Digit sum is :: {0}", MathFunctions.DigitSum(aisum));
            Console.ReadKey();
        }
    }
}