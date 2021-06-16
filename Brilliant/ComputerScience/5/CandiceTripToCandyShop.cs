using GenericDefs.Classes;
using GenericDefs.DotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class CandiceTripToCandyShop : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/candices-trip-to-the-candy-shop/");
        }

        /// <summary>
        /// total 40. 1, 2, 5 , 10.
        /// </summary>
        void ISolve.Solve()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs();

            int moneyLeft = 0, money = 40, candyCount = 10;

            int ai = 0, bi = 0, ci = 0, di = 0;
            for (int a = 0; a <= 40; a++)
            {
                ai = a; bi = 0; ci = 0; di = 0;
                moneyLeft = money - (ai * 1);

                candyCount = ai + bi + ci + di;
                if (candyCount > 10) continue;
                if (moneyLeft == 0)
                {
                    p.AddCombination(new ArrayList() { ai, bi, ci, di });
                    continue;
                }
                for (int b = 0; b <= 20; b++)
                {
                    bi = b; ci = 0; di = 0;
                    moneyLeft = money - (ai * 1) - (bi * 2);

                    candyCount = ai + bi + ci + di;
                    if (candyCount > 10) continue;

                    if (moneyLeft < 0) break;
                    if (moneyLeft == 0)
                    {
                        p.AddCombination(new ArrayList() { ai, bi, ci, di });
                        continue;
                    }
                    for (int c = 0; c <= 8; c++)
                    {
                        ci = c; di = 0;
                        moneyLeft = money - (ai * 1) - (bi * 2) - (ci * 5);

                        candyCount = ai + bi + ci + di;
                        if (candyCount > 10) continue;

                        if (moneyLeft < 0) break;
                        if (moneyLeft == 0)
                        {
                            p.AddCombination(new ArrayList() { ai, bi, ci, di });
                            continue;
                        }
                        for (int d = 0; d <= 4; d++)
                        {
                            di = d;
                            moneyLeft = money - (ai * 1) - (bi * 2) - (ci * 5) - (di * 10);

                            candyCount = ai + bi + ci + di;
                            if (candyCount > 10) continue;

                            if (moneyLeft < 0) break;
                            if (moneyLeft == 0)
                            {
                                p.AddCombination(new ArrayList() { ai, bi, ci, di });
                                continue;
                            }
                        }
                    }
                }
            }

            List<UniqueIntegralPairs.Combination> cList = p.GetCombinations();
            string logDump = string.Empty;
            BigInteger t = 0;
            foreach (UniqueIntegralPairs.Combination c in cList)
            {
                string s = string.Format("({0},{1},{2},{3})", c.Pair[0], c.Pair[1], c.Pair[2], c.Pair[3]);
                Console.WriteLine(s);
                s += Environment.NewLine;
                logDump += s;

                t += Combinations.GetAllCombinations(c.Pair);
            }

            Logger.Log(logDump);

            Console.WriteLine("All possible combinations are :: {0}", t.ToString());
            Console.ReadKey();
        }
    }
}