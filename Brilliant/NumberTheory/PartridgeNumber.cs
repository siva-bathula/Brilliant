using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.NumberTheory
{
    public class PartridgeNumber : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get
            {
                return thisProblem;
            }
        }
        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/profile/michael-j990dq/sets/advent-calendar-2014/193562/problem/day-1-a-partridge-in-a-number-tree/?group=XQG6KGF91dRa");
        }

        void ISolve.Solve()
        {
            int n = 1;
            int i = 2;
            int k = 0;
            while (n < 2014) {
                n = i * i;
                i++;
                bool isNPartridge = false;
                int gx = 0, gy = 0;
                for (int x = 1; x <= 100; x++)
                {
                    bool breakThis = false;
                    for (int y = 1; y <= 100; y++)
                    {
                        int expr = (((x * x) - 1) * ((y * y) - 1)) + 1;
                        if (n == expr && (x != (y - 1) || y != (x - 1))) {
                            gx = x; gy = y;
                            isNPartridge = true;
                        }
                        if (n == expr && (x == (y - 1) || y == (x - 1)))
                        {
                            gx = 0; gy = 0;
                            isNPartridge = false;
                            breakThis = true;
                            break;
                        }
                    }
                    if (breakThis) { break; }
                }
                if (isNPartridge)
                {
                    k++;
                    Console.WriteLine("Partridge number {0} : {1} for x = {2} and y = {3}", k, n, gx, gy);
                }
            }
            Console.ReadKey();
        }
    }
}