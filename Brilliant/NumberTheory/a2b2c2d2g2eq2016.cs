using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Classes;
using GenericDefs.Functions;
using System.Collections;
using GenericDefs.DotNet;

namespace Brilliant.NumberTheory
{
    public class a2b2c2d2g2eq2016 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/my-first-problem-in-number-theory/?group=y2XdY7thEboO");
        }

        void ISolve.Solve()
        {
            int maxValue = Convert.ToInt32(Math.Floor(Math.Sqrt(2016)));
            int m0 = maxValue, m1 = maxValue, m2 = maxValue, m3 = maxValue, m4 = maxValue;
            HashSet<string> slns = new HashSet<string>();
            UniquePairedIntegralSolutions<int> uniqueSo = new UniquePairedIntegralSolutions<int>("%");

            for (int a = 0; a <= m0; a++)
            {
                int[] arr = new int[5];
                int diff1 = 2016 - (a * a);
                m1 = Convert.ToInt32(Math.Floor(Math.Sqrt(diff1)));
                for (int b = 0; b <= maxValue; b++)
                {
                    int diff2 = 2016 - (a * a) - (b * b);
                    if (diff2 < 0) break;
                    m2 = Convert.ToInt32(Math.Floor(Math.Sqrt(diff2)));
                    for (int c = 0; c <= maxValue; c++)
                    {
                        int diff3 = 2016 - (a * a) - (b * b) - (c * c);
                        if (diff3 < 0) break;
                        m3 = Convert.ToInt32(Math.Floor(Math.Sqrt(diff3)));
                        for (int d = 0; d <= maxValue; d++)
                        {
                            int diff4 = 2016 - (a * a) - (b * b) - (c * c) - (d * d);
                            if (diff4 < 0) break;
                            m4 = Convert.ToInt32(Math.Floor(Math.Sqrt(diff4)));
                            for (int e = 0; e <= maxValue; e++)
                            {
                                int diff5 = 2016 - (a * a) - (b * b) - (c * c) - (d * d) - (e * e);
                                if (diff5 < 0) break;
                                if (diff5 == 0)
                                {
                                    arr[0] = a;
                                    arr[1] = b;
                                    arr[2] = c;
                                    arr[3] = d;
                                    arr[4] = e;
                                    uniqueSo.AddIfUnique(arr);

                                    arr = new int[5];
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            List<int[]> solutions = uniqueSo.GetUniqueSolutions();
            StringBuilder sLog = new StringBuilder();
            Console.WriteLine(Environment.NewLine + Environment.NewLine + " The possible solutions are :: " + solutions.Count);
            sLog.Append(Environment.NewLine + Environment.NewLine + "The possible solutions are :: " + solutions.Count);
            foreach (int[] x in solutions)
            {
                string slnkey = "+/- " + x[0] + ", " + x[1] + ", " + x[2] + ", " + x[3] + ", " + x[4];
                Console.WriteLine("    {0}", slnkey);
                sLog.Append(Environment.NewLine + "    " + slnkey);
            }

            Logger.Log(sLog.ToString());
            Console.ReadKey();
        }

        void Solve()
        {
            int maxValue = Convert.ToInt32(Math.Floor(Math.Sqrt(2016)));
            PythogoreanTriples t = new PythogoreanTriples(maxValue);
            List<PythogoreanTriples.Triple> aT = t.GetAllTriples();
            HashSet<string> slns = new HashSet<string>();

            for (int i = 0; i < aT.Count; i++) {
                int a1 = aT[i].a, b1 = aT[i].b, c1 = aT[i].c;
                for (int j = 0; j < aT.Count; j++) {
                    if (i == j) continue;
                    int a2 = aT[j].a, b2 = aT[j].b, c2 = aT[j].c;

                    int d = 2016 - (c1 * c1) - (c2 * c2);
                    if (d >= 0)
                    {
                        double dRoot = Math.Sqrt(d);
                        bool isSquare = (dRoot % 1 == 0);
                        if (d == 0 || isSquare) {
                            string slnkey = "+/- " + a1 + ", " + b1 + ", " + Convert.ToInt32(dRoot) + ", " + a2 + ", " + b2;
                            if (!slns.Contains(slnkey)) slns.Add(slnkey);
                        }
                    }
                }
            }

            Console.WriteLine("The possible solutions are :: ");
            foreach (string s in slns) {
                Console.WriteLine("       {0}", s);
            }
            Console.ReadKey();
        }
        
    }
}