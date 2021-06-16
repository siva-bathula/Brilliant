using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Algebra
{
    /// <summary>
    /// What is the greatest common factor of all integers of the form p^4 - 1, where p is a prime number greater than 5?
    /// </summary>
    public class Primetastic : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/primetastic/");
        }

        void ISolve.Solve()
        {
            List<int> p = GenericDefs.Functions.Prime.GeneratePrimesSieveOfSundaram(1000);
            List<long> n = new List<long>();
            foreach(int pr in p)
            {
                if (pr > 5)
                {
                    long t = pr * pr;
                    n.Add((t * t) - 1);
                }
            }

            long gcd = GenericDefs.Functions.MathFunctions.GCD(n);

            Console.WriteLine("The GCD is : {0}", gcd);
            Console.ReadKey();
        }
    }
}