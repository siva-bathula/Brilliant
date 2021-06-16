using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.NumberTheory._5
{
    /// <summary>
    /// A positive integer n is funny if for each of its positive divisors d, d+2 is a prime number. Find the sum of all funny numbers that have the most quantity of divisors.
    /// </summary>
    public class FunnyNumbers : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/funny-numbers/");
        }

        void ISolve.Solve()
        {
            long nMax = 10000000;
            List<long> primes = GenericDefs.Functions.Prime.GeneratePrimesNaiveNMax(nMax);
            long n = 1;
            List<long> list = new List<long>();
            int dCount = 0;
            while (true)
            {
                HashSet<long> pSet = GenericDefs.Functions.Factors.GetAllFactors(n);
                if (pSet.Count > dCount)
                {
                    bool isFunny = true;
                    foreach (long f in pSet)
                    {
                        if (f % 2 == 0) { isFunny = false; break; }
                        if (!primes.Contains(f + 2)) { isFunny = false; break; }
                    }
                    if (isFunny)
                    {
                        if (pSet.Count > dCount)
                        {
                            dCount = pSet.Count;
                            list.Clear();
                            list.Add(n);
                        }
                        else if (pSet.Count == dCount)
                        {
                            list.Add(n);
                        }
                    }
                }
                n += 2;
                if (n == 10000000000) break;
            }
            QueuedConsole.WriteFinalAnswer("Sum : " + list.Sum());
        }
    }
}