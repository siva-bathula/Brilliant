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
    ///Cody has 4 types of onions. 
    ///The number of purple onions can be any non-negative integer.
    ///The number of green onions is a multiple of 2.
    ///The number of red onions is a multiple of 3.
    ///The number of blue onions is a multiple of 5.
    ///If Cody has 23 onions, how many different distributions of colors can there be?
    ///Details and Assumptions: Cody can't have a negative number of onions, but he may have 0 onions of a certain type(s).
    /// </summary>
    public class IamOnion : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/i-am-onion-_/");
        }

        void ISolve.Solve()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("@#$");
            int N = 23;
            int aMax = (int)Math.Floor(N / 1.0);
            int bMax = (int)Math.Floor(N / 2.0);
            int cMax = (int)Math.Floor(N / 3.0);
            int dMax = (int)Math.Floor(N / 5.0);

            int ai = 0, bi = 0, ci = 0, di = 0;
            for (int a = 0; a <= aMax; a++)
            {
                ai = a; bi = 0; ci = 0; di = 0;
                int expr = ai + (2 * bi) + (3 * ci) + (5 * di);
                if (expr > N) { break; }
                if (expr == N)
                {
                    ArrayList l = new ArrayList();
                    l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                    p.AddCombination(l);
                }
                for (int b = 0; b <= bMax; b++)
                {
                    bi = b; ci = 0; di = 0;
                    expr = ai + (2 * bi) + (3 * ci) + (5 * di);
                    if (expr> N) { break; }
                    if (expr == N)
                    {
                        ArrayList l = new ArrayList();
                        l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                        p.AddCombination(l);
                    }
                    for (int c = 0; c <= cMax; c++)
                    {
                        ci = c; di = 0;
                        expr = ai + (2 * bi) + (3 * ci) + (5 * di);
                        if (expr > N) { break; }
                        if (expr == N)
                        {
                            ArrayList l = new ArrayList();
                            l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                            p.AddCombination(l);
                        }
                        for (int d = 0; d <= dMax; d++)
                        {
                            di = d;
                            expr = ai + (2 * bi) + (3 * ci) + (5 * di);
                            if (expr > N) { break; }
                            if (expr == N) {
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
