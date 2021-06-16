using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace Brilliant.NumberTheory
{
    /// <summary>
    /// Modulus 1009 # 2
    /// (1009^2 - 2)!/1009^1008
    /// Find the remainder when the number above is divided by 1009.
    /// </summary>
    public class Modulus1009 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/modulus-1009-2/");
        }

        void ISolve.Solve()
        {
            BigInteger b1 = new BigInteger();
            BigInteger b2 = new BigInteger();
            b1 = 1; b2 = 1;
            //int f = 1009 * 1009 - 2;
            int f = 1009 * 1009 - 2;

            for (int k = 2; k <= 1009; k++) {
                b2 *= 1009;
            }

            for (int k = 2; k <= f; k++) {
                b1 *= k;
            }

            Console.WriteLine("Remainder is : {0}", b1 % b2);
            Console.ReadKey();
        }
    }
}