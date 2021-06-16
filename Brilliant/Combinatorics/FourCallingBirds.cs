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
    /// The four calling birds are singing a melody with 7 notes. Each of the four calling birds can sing one unique note; A, B, C and D respectively. 
    /// In the melody every bird must sing at least one note. One possible melody for example is ABCDABC.
    ///Find the total number of different melodies that they can sing.
    /// </summary>
    public class FourCallingBirds : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/day-4-four-calling-birds/");
        }

        void ISolve.Solve()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("@#$");

            int ai = 1, bi = 1, ci = 1, di = 1;
            for (int a = 1; a <= 4; a++)
            {
                ai = a; bi = 1; ci = 1; di = 1;
                int expr = ai + bi + ci + di;
                if (expr > 7) { break; }
                if (expr == 7)
                {
                    ArrayList l = new ArrayList();
                    l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                    p.AddCombination(l);
                }
                for (int b = 1; b <= 4; b++)
                {
                    bi = b; ci = 1; di = 1;
                    expr = ai + bi + ci + di;
                    if (expr > 7) { break; }
                    if (expr == 7)
                    {
                        ArrayList l = new ArrayList();
                        l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                        p.AddCombination(l);
                    }
                    for (int c = 1; c <= 4; c++)
                    {
                        ci = c; di = 1;
                        expr = ai + bi + ci + di;
                        if (expr > 7) { break; }
                        if (expr == 7)
                        {
                            ArrayList l = new ArrayList();
                            l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                            p.AddCombination(l);
                        }
                        for (int d = 1; d <= 4; d++)
                        {
                            di = d;
                            expr = ai + bi + ci + di;
                            if (expr > 7) { break; }
                            if (expr == 7)
                            {
                                ArrayList l = new ArrayList();
                                l.Add(ai); l.Add(bi); l.Add(ci); l.Add(di);
                                p.AddCombination(l);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Number of unique solutions :: {0}", p.GetCombinations().Count);

            int[] oi = new int[4] { 1, 2, 3, 4 };
            long count = 0;
            List<UniqueIntegralPairs.Combination> pairs = p.GetCombinations();
            foreach (UniqueIntegralPairs.Combination c in pairs)
            {
                List<int> nP = new List<int>();
                string soln = string.Empty;
                for (int i = 0; i < 4; i++)
                {
                    int iter = (int)c.Pair[i];
                    while (iter > 0)
                    {
                        iter--;
                        nP.Add(oi[i]);
                        if (i == 0) soln += "A";
                        if (i == 1) soln += "B";
                        if (i == 2) soln += "C";
                        if (i == 3) soln += "D";
                    }
                }
                Console.WriteLine("Melody :: {0}", soln);
                count += Permutations.GetPermutationsCount(nP, nP.Count);
            }
            Console.WriteLine("Number of ways :: {0}", count);
            Console.ReadKey();
        }
    }
}
