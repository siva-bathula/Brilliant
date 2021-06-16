using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics
{
    public class AndrewsRecursiveDream : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/andrews-recursive-dream/");
        }

        void ISolve.Solve()
        {
            int a1 = 2, a2 = 5, N = 10;
            List<BigInteger> allA = new List<BigInteger>();
            allA.Add(a1); allA.Add(a2);
            BigInteger sum3toNm1 = 0;
            for (int i = 3; i < N; i++)
            {
                BigInteger ai = (2 * allA[i - 3]) + (3 * allA[i - 2]);
                allA.Add(ai);
                sum3toNm1 += ai;
            }

            BigInteger aN = 2 * allA[N - 3] + 3 * allA[N - 2];
            BigInteger aNp1 = 2 * allA[N - 2] + 3 * aN;

            BigInteger numerator = 2 * (a1 + aN) + 5 * a2 - aNp1;
            Console.WriteLine("Numerator :: {0}", numerator);
            Console.WriteLine("Denominator :: {0}", sum3toNm1);
            Console.WriteLine("Expression :: {0}", numerator / sum3toNm1);
            Console.ReadKey();
        }
    }
}
