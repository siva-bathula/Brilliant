using GenericDefs.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    public class Cracking1980sEncryption : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/cracking-1980s-encryption/");
        }

        void ISolve.Solve()
        {
            BigInteger b = new BigInteger();
            BigInteger.TryParse("2639085015233392202740949386309743259521517793886143240989", out b);

            BigInteger f = new BigInteger(10057211679709);

            //10057211679709
            //341550071728321
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool swap = false;
            while (b % f != 0)
            {
                f = f + 1;
                if (f % 2 == 0 || f % 3 == 0) continue;
                if (sw.Elapsed.Seconds % 10 != 0) swap = false;
                if (!swap && sw.Elapsed.Seconds % 10 == 0)
                {
                    Console.WriteLine("f = {0}", f);
                    swap = true;
                }
                if (b % f == 0)
                {
                    break;
                }
            }

            sw.Stop();
            Console.WriteLine("{0}", f.ToString());
            Console.WriteLine("Digit sum is :: {0}", MathFunctions.DigitSum(f).ToString());
            Console.ReadKey();
        }
    }
}