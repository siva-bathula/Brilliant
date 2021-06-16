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
    ///There are 30 balls as follows: 
    ///5 identical white balls, 
    ///10 identical black balls, 
    ///15 identical red balls.
    ///How many ways are there to divide these balls into 2 boxes A and B, each of which contains 15 balls?
    /// </summary>
    public class Dividingthirtyballs : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/dividing-30-balls/");
        }

        void ISolve.Solve()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("@#$");
            int N = 15;
            int aMax = 5;
            int bMax = 10;
            int cMax = 15;

            int ai = 0, bi = 0, ci = 0;
            for (int a = 0; a <= aMax; a++)
            {
                ai = a; bi = 0; ci = 0;
                int expr = ai + bi + ci;
                if (expr > N) { break; }
                if (expr == N)
                {
                    ArrayList l = new ArrayList();
                    l.Add(ai); l.Add(bi); l.Add(ci);
                    p.AddCombination(l);
                }
                for (int b = 0; b <= bMax; b++)
                {
                    bi = b; ci = 0;
                    expr = ai + bi + ci;
                    if (expr> N) { break; }
                    if (expr == N)
                    {
                        ArrayList l = new ArrayList();
                        l.Add(ai); l.Add(bi); l.Add(ci);
                        p.AddCombination(l);
                    }
                    for (int c = 0; c <= cMax; c++)
                    {
                        ci = c;
                        expr = ai + bi + ci;
                        if (expr > N) { break; }
                        if (expr == N)
                        {
                            ArrayList l = new ArrayList();
                            l.Add(ai); l.Add(bi); l.Add(ci);
                            p.AddCombination(l);
                        }
                    }
                }
            }
            Console.WriteLine("Number of ways :: {0}", p.GetCombinations().Count);
            Console.ReadKey();
        }
    }
}
