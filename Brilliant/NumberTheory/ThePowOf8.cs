using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.NumberTheory
{
    public class ThePowOf8 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/day-8-the-power-of-eight/");
        }

        void ISolve.Solve()
        {
            int sum = 0;
            Console.WriteLine("Possible values are : ");
            for (int n = 1; n <= 100; n++)
            {
                long n2m4 = n * n - 4;
                if (n2m4 <= 0) continue;
                int aMax = Convert.ToInt32(Math.Ceiling(Math.Log(n2m4, 2)));
                for (int a = 0; a <= aMax; a++)
                {
                    long n2m4m2a = n2m4 - Convert.ToInt32(Math.Pow(2.0, a));
                    if (n2m4m2a <= 0) break;
                    int bMax = Convert.ToInt32(Math.Ceiling(Math.Log(n2m4m2a, 7)));
                    for (int b = 0; b <= bMax; b++)
                    {
                        long diff = n2m4m2a - Convert.ToInt32(Math.Pow(7.0, b));
                        if (diff < 0) break;
                        if (diff == 0)
                        {
                            sum += a + b + n;
                            Console.WriteLine(" (a,b,n) : ( {0}, {1}, {2} )", a, b, n);
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("        Total Sum : {0}", sum);
            Console.ReadKey();
        }
    }
}