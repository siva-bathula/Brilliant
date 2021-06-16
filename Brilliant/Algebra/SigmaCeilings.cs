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
    public class SigmaCeilings : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/sigma-ceilings/");
        }

        void ISolve.Solve()
        {
            int sSum = 0;
            for (int i = 1; i <= 4096; i++) {
                var v = Math.Ceiling(11.0 / i);
                v = Math.Ceiling(7.0 / v);
                v = Math.Ceiling(5.0 / v);
                v = Math.Ceiling(3.0 / v);
                v = Math.Ceiling(2.0 / v);

                sSum += (int)v;
            }

            Console.WriteLine("The Sigma ceiling is : {0}", sSum);
            Console.ReadKey();
        }
    }
}