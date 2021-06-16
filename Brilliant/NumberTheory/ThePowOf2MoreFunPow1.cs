using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Brilliant.NumberTheory
{
    /// <summary>
    /// The power of two is more fun than the power of one.
    ///  2^1 - 2^2 + 2^3 - 2^4 + ..... + 2^2013
    /// Let a be the value of the expression above. Find the last two digits of a.
    /// </summary>
    public class ThePowOf2MoreFunPow1 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/the-power-of-two-is-more-fun-than-the-power-of-one/");
        }

        void ISolve.Solve()
        {
            BigInteger b1 = new BigInteger();
            BigInteger bSum = new BigInteger();
            bSum = 0;
            for (int i = 1; i <= 2013; i++) {
                b1 = 1;
                int k = Convert.ToInt32(Math.Pow(-1, (i + 1)));
                for(int j = 1; j<= i; j++)
                {
                    b1 *= 2;
                }
                bSum += k * b1;
            }

            Console.WriteLine("Last two digits of the expression is :: {0}", bSum % 100);
            Console.ReadKey();
        }
    }
}