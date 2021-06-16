using GenericDefs.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics
{
    /// <summary>
    /// In how many ways can you pay for a pair of $100 shoes using only $1, $5, $10, and $25?
    /// a+5b+10c+25d=100;
    /// </summary>
    public class AreYouGoodWithMoney : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/are-you-good-with-money/");
        }

        void ISolve.Solve()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("@#$");
            int aMax = (int)Math.Floor(100.0 / 1);
            int bMax = (int)Math.Floor(100.0 / 5);
            int cMax = (int)Math.Floor(100.0 / 10);
            int dMax = (int)Math.Floor(100.0 / 25);

            int ai = 0, bi = 0, ci = 0, di = 0;
            for (int a = 0; a <= aMax; a++)
            {
                ai = a; bi = 0; ci = 0; di = 0;
                int expr = ai + (5 * bi) + (10 * ci) + (25 * di);
                if (expr > 100) { break; }
                if (expr == 100)
                {
                    ArrayList l = new ArrayList();
                    l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                    p.AddCombination(l);
                }
                for (int b = 0; b <= bMax; b++)
                {
                    bi = b; ci = 0; di = 0;
                    expr = ai + (5 * bi) + (10 * ci) + (25 * di);
                    if (expr> 100) { break; }
                    if (expr == 100)
                    {
                        ArrayList l = new ArrayList();
                        l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                        p.AddCombination(l);
                    }
                    for (int c = 0; c <= cMax; c++)
                    {
                        ci = c; di = 0;
                        expr = ai + (5 * bi) + (10 * ci) + (25 * di);
                        if (expr > 100) { break; }
                        if (expr == 100)
                        {
                            ArrayList l = new ArrayList();
                            l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                            p.AddCombination(l);
                        }
                        for (int d = 0; d <= dMax; d++)
                        {
                            di = d;
                            expr = ai + (5 * bi) + (10 * ci) + (25 * di);
                            if (expr > 100) { break; }
                            if (expr == 100) {
                                ArrayList l = new ArrayList();
                                l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                                p.AddCombination(l);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Number of ways :: {0}", p.GetCombinations().Count);
            Console.ReadKey();
        }
    }
}
