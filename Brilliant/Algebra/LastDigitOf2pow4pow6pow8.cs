using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Algebra
{
    /// <summary>
    /// Find last digit of 2 pow 4 pow 6 pow 8
    /// </summary>
    public class LastDigitOf2pow4pow6pow8 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/problem-20/");
        }

        void ISolve.Solve()
        {
            BigInteger sixpow8 = 1;
            for (int i = 1; i <= 6; i++) {
                sixpow8 *= 6;
            }
            BigInteger fourpow6pow8 = 1;
            for (BigInteger i = 1; i <= sixpow8; i++)
            {
                fourpow6pow8 *= 4;
            }
            Console.WriteLine("Last two digits of 4pow6pow8 is :: {0}", fourpow6pow8 % 100);
            BigInteger twopow4pow6pow8 = 1;
            for (BigInteger i = 1; i <= fourpow6pow8; i++)
            {
                twopow4pow6pow8 *= 2;
            }

            Console.WriteLine("Last digit is :: {0}", twopow4pow6pow8 % 10);
            Console.ReadKey();
        }
    }
}