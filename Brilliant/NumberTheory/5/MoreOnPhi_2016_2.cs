using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.NumberTheory._5
{
    /// <summary>
    /// https://brilliant.org/problems/more-on-phi-in-2016-2/
    /// </summary>
    public class MoreOnPhi_2016_2 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/more-on-phi-in-2016-2/");
        }

        void ISolve.Solve() {
            Solve1();
        }

        void Solve()
        {
            BigInteger bPi = new BigInteger(1);
            int N = 27000;

            long phiN = GenericDefs.Functions.EulerTotient.CalculateTotient(N);
            HashSet<long> factors = GenericDefs.Functions.Factors.GetAllFactors(N);
            foreach (long l in factors) {
                bPi *= GenericDefs.Functions.EulerTotient.CalculateTotient(l);
            }

            Console.WriteLine("The product pi can be given as :: {0}", bPi.ToString());

            BigInteger blog = bPi;

            while (blog % 7200 == 0) {
                blog /= 7200;
            }

            Console.WriteLine("Remainder after dividing with 7200 :: {0}", blog);
            int d8Count = 0;
            while (blog % 8 == 0)
            {
                d8Count++;
                blog /= 8;
            }
            Console.WriteLine("Number of times divisible by 8 :: {0}", d8Count);
            Console.ReadKey();
        }

        void Solve1() {
            long i = 1, phi = 0;
            while (phi != 2016) {
                i++;
                phi = GenericDefs.Functions.EulerTotient.CalculateTotient(i);
                if (phi == 2016) break;
            }

            Console.WriteLine("phi is 2016 for n: {0}", i);
            Console.ReadKey();
        }
    }
}