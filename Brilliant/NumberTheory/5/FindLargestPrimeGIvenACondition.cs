using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.NumberTheory._5
{
    /// <summary>
    /// It is given that p is a given prime number such that : For some positive integers x and y, 
    /// x^3 + y^3 - 3xy + 1 = p;
    /// find the largest value of.
    /// </summary>
    public class FindLargestPrimeGIvenACondition : ISolve, IBrilliant
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
            long p = 2;
            QueuedConsole.WriteImmediate("Calculating primes ... ");
            List<int> primes = GenericDefs.Functions.Prime.GeneratePrimesNaiveNMax(100000000);
            primes.Sort();
            QueuedConsole.WriteImmediate("Calculated primes ... Count = " + primes.Count);
            long pMax = primes[primes.Count - 1];
            GenericDefs.OtherProjects.Primality prim = new GenericDefs.OtherProjects.Primality();
            for (long x = 2; x <= 10000; x++)
            {
                for (long y = 2; y <= 10000; y++)
                {
                    long calValue = (x * x * x) + (y * y * y) - (3 * x * y) + 1;
                    if (calValue <= pMax && primes.Contains((int)calValue))
                    {
                        if (calValue > p) p = calValue;
                    }
                    else if (prim.IsPrimeMillerRabin(calValue)) {
                        if (calValue > p) p = calValue;
                    }
                }
            }

            QueuedConsole.WriteFinalAnswer("Largest value of p is :: " + p);
        }
    }
}