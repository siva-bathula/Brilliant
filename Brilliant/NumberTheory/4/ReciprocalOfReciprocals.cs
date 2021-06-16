using GenericDefs.Classes;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Brilliant.NumberTheory._4
{
    public class ReciprocalOfReciprocals : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/reciprocals-of-reciprocals/");
        }

        void ISolve.Solve()
        {
            ReciprocalOfReciprocal();
        }

        void Solution1()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("#");
            double expr1 = 0.0, rhs1 = 1.0 / 20;
            int N = 25;

            //GenerateArray(N * 20);
            int aMax = N, bMax = (int)(N * 10000), cMax = (int)(N * 20000), dMax = (int)20000 * N;
            Console.WriteLine("Iterating....");
            SpecialConsole console = QueuedConsole.GetSpecialConsole(1000, 1000);

            //no solution was found till 
            for (int a = 1; a <= aMax; a++)
            {
                for (int b = a + 1; b <= bMax; b++)
                {
                    console.LazySuppressWrite(string.Format("Iterating for a: {0}, b: {1}", a, b));
                    for (int c = b + 1; c <= cMax; c++)
                    {
                        long abc = a * b * c;
                        long bc = b * c;
                        int bpc = b + c;
                        expr1 = (1.0 / a) - (1.0 / b) - (1.0 / c) - (rhs1);
                        if (expr1 >= 0) { break; }
                        if (expr1 + (1.0 / (c + 1)) < 0) { break; }
                        for (int d = c + 1; d <= dMax; d++)
                        {
                            if ((a + d) > (bpc)) { break; }

                            if ((a + d) != (bpc)) { continue; }

                            //if (b * c <= a * d) { break; }

                            //if ((abc * d) % 20 != 0) { continue; }

                            expr1 += 1.0 / d;
                            if (expr1 < 0) { break; }
                            if (expr1 == 0)
                            {
                                Console.WriteLine("Eliminating for gcd is 1..... ");
                                int gcdab = MathFunctions.GCD(a, b);
                                int gcdcd = MathFunctions.GCD(c, d);
                                int gcdabcd = MathFunctions.GCD(gcdab, gcdcd);
                                if (gcdabcd == 1)
                                {
                                    ArrayList l = new ArrayList();
                                    l.Add(a); l.Add(b); l.Add(c); l.Add(d);
                                    p.AddCombination(l);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Done Iterating....");
            List<UniqueIntegralPairs.Combination> cs = p.GetCombinations();
            Console.WriteLine("Total possible combinations are :: {0}", cs.Count);
            string logDump = string.Empty;
            foreach (UniqueIntegralPairs.Combination c in cs)
            {
                string s = string.Format("Possible values of (a,b,c,d) :: ({0},{1},{2},{3}). The required expression is :: {4}",
                    (int)c.Pair[0], (int)c.Pair[1], (int)c.Pair[2], (int)c.Pair[3], (((int)c.Pair[2] / (int)c.Pair[0]) - ((int)c.Pair[1] / (int)c.Pair[3])));
                logDump += s;
                Console.WriteLine(s);
            }
            if (!string.IsNullOrEmpty(logDump)) Logger.Log(logDump);
            Console.ReadKey();
        }

        void Solution2()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("#");
            double expr1 = 0.0, rhs1 = 1.0 / 20;
            int N = 20;

            //GenerateArray(N * 20);
            int aMax = N, bMax = (int)(N * 10), cMax = (int)(N * 20), dMax = (int)20 * N;
            Console.WriteLine("Iterating....");
            SpecialConsole console = QueuedConsole.GetSpecialConsole(1000, 1000);

            //no solution was found till 
            for (long a = 1; a <= aMax; a++)
            {
                for (long b = a; b <= bMax; b++)
                {
                    console.LazySuppressWrite(string.Format("Iterating for a: {0}, b: {1}", a, b));
                    for (long c = b; c <= cMax; c++)
                    {
                        long abc = a * b * c;
                        long bc = b * c;
                        long bpc = b + c;
                        expr1 = (1.0 / a) - (1.0 / b) - (1.0 / c) - (rhs1);
                        if (expr1 >= 0) { break; }

                        long d = (b + c) - a;
                        long deno = (abc + (20 * ((a * bpc) - bc)));
                        if (deno <= 0) continue;
                        if (d < c) continue;
                        if ((20 * abc) % deno != 0) continue;
                        double expr2 = (20 * abc) / deno;

                        if ((int)expr2 == d)
                        {
                            Console.WriteLine("Eliminating for gcd is 1..... ");
                            long gcdab = MathFunctions.GCD(a, b);
                            long gcdcd = MathFunctions.GCD(c, d);
                            long gcdabcd = MathFunctions.GCD(gcdab, gcdcd);
                            if (gcdabcd == 1)
                            {
                                ArrayList l = new ArrayList();
                                l.Add(a); l.Add(b); l.Add(c); l.Add(d);
                                p.AddCombination(l);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Done Iterating....");
            List<UniqueIntegralPairs.Combination> cs = p.GetCombinations();
            Console.WriteLine("Total possible combinations are :: {0}", cs.Count);
            string logDump = string.Empty;
            foreach (UniqueIntegralPairs.Combination c in cs)
            {
                string s = string.Format("Possible values of (a,b,c,d) :: ({0},{1},{2},{3}). The required expression is :: {4}",
                    (long)c.Pair[0], (long)c.Pair[1], (long)c.Pair[2], (long)c.Pair[3], (((long)c.Pair[2] / (long)c.Pair[0]) - ((long)c.Pair[1] / (long)c.Pair[3])));
                logDump += s;
                Console.WriteLine(s);
            }
            if (!string.IsNullOrEmpty(logDump)) Logger.Log(logDump);
            Console.ReadKey();
        }

        decimal[] Reciprocals;
        void GenerateArray(int N)
        {
            if (Reciprocals == null)
            {
                Reciprocals = new decimal[N + 1];
            }

            for (int i = 1; i <= N; i++)
            {
                Reciprocals[i] = (decimal)1 / i;
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/the-meaning-of-life-2/
        /// </summary>
        void Solve3()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("#");
            int N = 1000;

            //GenerateArray(N * 20);
            int aMax = N, bMax = (int)(N * 100);
            Console.WriteLine("Iterating....");
            SpecialConsole console = QueuedConsole.GetSpecialConsole(1000, 1000);

            //no solution was found till 
            for (long a = 1; a <= aMax; a++)
            {
                for (long b = a; b <= bMax; b++)
                {
                    console.LazySuppressWrite(string.Format("Iterating for a: {0}, b: {1}", a, b));
                    long ab = a * b;
                    long deno = (ab - (42 * (a + b)));

                    if (deno <= 0) { continue; }

                    if ((42 * ab) % deno != 0) continue;

                    double expr2 = (42 * ab) / deno;
                    ArrayList l = new ArrayList();
                    l.Add(a); l.Add(b); l.Add((long)expr2); ;
                    p.AddCombination(l);
                }
            }

            Console.WriteLine("Done Iterating....");
            List<UniqueIntegralPairs.Combination> cs = p.GetCombinations();
            Console.WriteLine("Total possible combinations are :: {0}", cs.Count);
            long cmax = 0;
            foreach (UniqueIntegralPairs.Combination c in cs)
            {
                if ((long)c.Pair[2] > cmax) { cmax = (long)c.Pair[2]; }
                string s = string.Format("Possible values of (a,b,c) :: ({0},{1},{2})", (long)c.Pair[0], (long)c.Pair[1], (long)c.Pair[2]);
                Console.WriteLine(s);
            }

            Console.WriteLine("Maximum value of c is :: {0}", cmax);
            Console.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/hardest-multiple-ever/
        /// </summary>
        void SolveHardestMultipleEver()
        {
            bool breakThis = false;
            for (int a = 1; a <= 1000; a++)
            {
                for (int b = 1; b <= 1000; b++)
                {
                    for (int c = 1; c <= 1000; c++)
                    {
                        int abc = a * b * c;
                        int bc = b * c;
                        int apbpc = a + b + c;

                        int d1 = abc - apbpc + b;
                        if (d1 == 0) continue;
                        if (abc * (bc - 1) % d1 == 0 && abc * (bc - 1) / d1 == 15)
                        {
                            d1 = abc - apbpc + a;
                            if (d1 == 0) continue;
                            int ab = a * b;
                            if (abc * (ab - 1) % d1 == 0 && abc * (ab - 1) / d1 == 6)
                            {
                                d1 = abc - apbpc + c;
                                if (d1 == 0) continue;
                                int ac = a * c;
                                if (abc * (ac - 1) % d1 == 0 && abc * (ac - 1) / d1 == 4)
                                {
                                    QueuedConsole.WriteImmediate("a: {0}, b: {1}, c: {2}", a, b, c);
                                    breakThis = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (breakThis) break;
                }
                if (breakThis) break;
            }
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/unit-fraction-essay/
        /// </summary>
        void UnitFractionEssay()
        {
            int leastSum = int.MaxValue;
            int n = 42;
            double rhs = 1.0 / n;
            int nmax = 500;
            int amin = 0, bmin = 0, cmin = 0;
            for (int a = n; a <= nmax; a++)
            {
                for (int b = a + 1; b <= nmax; b++)
                {
                    for (int c = b + 1; c <= nmax; c++)
                    {
                        int nume = a * b * c;
                        int deno = (a * b + b * c + c * a);
                        if (nume % deno == 0)
                        {
                            if (nume / deno == 42)
                            {
                                leastSum = Math.Min(leastSum, a + b + c);
                                amin = a; bmin = b; cmin = c;
                            }
                        }
                    }
                }
            }
            QueuedConsole.WriteImmediate("Least sum : {0}", leastSum);
            QueuedConsole.WriteImmediate("amin : {0}, bmin : {1}, cmin : {2}", amin, bmin, cmin);
        }

        /// <summary>
        /// https://brilliant.org/problems/quadruple-reciprocal-sum/
        /// </summary>
        void QuadrupleReciprocalSum()
        {
            double rhs = 1.0, lhs = 0.0;
            double error = 0.00001;
            long count = 0;
            for (double a = 2; a <= 3; a++)
            {
                for (double b = a + 1; b <= 100; b++)
                {
                    for (double c = b + 1; c <= 200; c++)
                    {
                        for (double d = c + 1; d <= 400; d++)
                        {
                            lhs = (1 / a) + (1 / b) + (1 / c) + (1 / d);
                            if (Math.Abs(lhs - rhs) <= error)
                            {
                                QueuedConsole.WriteImmediate("a: {0}, b: {1}, c: {2}, d: {3}", a, b, c, d);
                                List<double> num = new List<double>() { a, b, c, d };
                                count += PermutationsAndCombinations.nPr(4, num.Distinct().Count());
                            }
                        }
                    }
                }
            }
            QueuedConsole.WriteImmediate("Number of ordered quadruples of distinct positive integers : {0}", count);
        }

        /// <summary>
        /// https://brilliant.org/problems/reciprocal-of-reciprocals/
        /// </summary>
        void ReciprocalOfReciprocal()
        {
            int uOcount = 0, oCount = 0;
            for (int a = 1; a <= 100; a++)
            {
                for(int b = 1; b <= 100; b++)
                {
                    if (a == b) continue;
                    if ((a * b) % (a + b) == 0) oCount++;
                }

                for (int b = a + 1; b <= 100; b++)
                {
                    if (a == b) continue;
                    if ((a * b) % (a + b) == 0) uOcount++;
                }
            }
            QueuedConsole.WriteImmediate("Ordered count:{0}, Unordered count: {1}", oCount, uOcount);
        }
    }
}