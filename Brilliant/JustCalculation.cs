using GenericDefs.Classes;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.DotNet;
using GenericDefs.Functions.NumberTheory;
using GenericDefs.Functions;

using Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Brilliant
{
    public class JustCalculation : ISolve
    {
        void ISolve.Init()
        {

        }

        void ISolve.Solve()
        {
            A();
            //RemainderTest();
        }

        void A()
        {
            double b = 1;
            int n = 0;
            double a = Math.PI / 11, t = Math.PI / 22;
            while(++n <= 5)
            {
                b *= Math.Sin(a * 2 * n);
                b /= Math.Sin(t * ((2 * n) - 1));
            }
            QueuedConsole.WriteImmediate("{0}", b * b);
        }

        void RemainderTest()
        {
            List<long> powers = new List<long>();
            powers.Add(547); powers.Add(547);
            QueuedConsole.WriteImmediate("{0}", RemainderCalculator.RemainderWithNestedPowers(powers.ToArray(), 1000000000));

            //https://brilliant.org/problems/1234567890-2/
            //powers.Add(12); powers.Add(34); powers.Add(56); powers.Add(78);
            //QueuedConsole.WriteImmediate("{0}", GenericDefs.Functions.RemainderCalculator.RemainderWithNestedPowers1(powers.ToArray(), 90));

            //Enumerable.Range(0, 2004).ForEach(n =>
            //{
            //    powers.Add(2004 - n);
            //});
            //QueuedConsole.WriteImmediate("{0}", GenericDefs.Functions.RemainderCalculator.RemainderWithNestedPowers1(powers.ToArray(), 1000));
        }

        void _rightmost()
        {
            FibonacciGenerator fg = new FibonacciGenerator();
            GenericDefs.Classes.Quirky.ConsecutiveStreakCounter csc = new GenericDefs.Classes.Quirky.ConsecutiveStreakCounter();
            long prevHigh = 0;
            int _10pow6 = (int)Math.Pow(10, 6);
            int _10pow7 = (int)Math.Pow(10, 7);
            while (csc.CurrentStreak < 32)
            {
                BigInteger fn = fg.Next();
                if (fn < 1000000) continue;
                
                if ((fn - fn%_10pow6) % _10pow7 == 0) {
                    csc.Add(1);
                    if(csc.CurrentStreak > prevHigh)
                    {
                        prevHigh = csc.CurrentStreak;
                        QueuedConsole.WriteImmediate("n: {0}, streak length: {1}", fg.CurrentN(), prevHigh);
                    }
                }
                else { csc.Reset(); }
            }
            QueuedConsole.WriteImmediate("{0}", fg.CurrentN());
        }

        void TangentSum()
        {
            double sum = 0.0;
            Enumerable.Range(1, 89).ForEach(x =>
            {
                double d = x * Math.PI / 180.0;
                sum += Math.Pow(Math.Tan(d), 2);
            });
            QueuedConsole.WriteImmediate("sum: {0}", sum);
        }

        void Subsets2015SumModulo31()
        {
            Dictionary<int, long> mod31 = new Dictionary<int, long>();
            for (int i = 1; i <= 2015; i++)
            {
                for (int j = 1; j <= 2015; j++)
                {
                    if (i == j) continue;
                    for (int k = 1; k <= 2015; k++)
                    {
                        if (j == k || i == k) continue;

                        int t = (i + j + k) % 31;
                        if (mod31.ContainsKey(t))
                        {
                            (mod31[t])++;
                        }
                        else mod31.Add(t, 1);
                    }
                }
            }

            var maxMod = mod31.Max(x => x.Value);
            var maxModPairs = mod31.Where(x => x.Value == maxMod);
            foreach (KeyValuePair<int, long> kvp in maxModPairs)
            {
                QueuedConsole.WriteImmediate(string.Format("{0}, Count : {1}", kvp.Key, kvp.Value));
            }
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/can-you-find-the-solution/
        /// </summary>
        void xyzintegralsolutions()
        {
            for (int x = 2; x <= 4; x++)
            {
                QuadraticEquation.Root r = new QuadraticEquation.Root();
                if (QuadraticEquation.Solve(4 * x, (x * x - 9 * x), 6, ref r))
                {
                    QueuedConsole.WriteImmediate(string.Format("x : {0}, y: {1}, z: {2}", x, 3.0 / (x * r.RMinus), r.RMinus));
                    QueuedConsole.WriteImmediate(string.Format("x : {0}, y: {1}, z: {2}", x, 3.0 / (x * r.RPlus), r.RPlus));
                }
            }

            QueuedConsole.ReadKey();
        }

        void InverseMod()
        {
            GenericDefs.Functions.RemainderCalculator.ModularInverse(5, 17);
        }

        /// <summary>
        /// an = 3*an-1 , a0=2;
        /// </summary>
        void Series()
        {
            int ai = 2;
            int n = 0;
            while (true)
            {
                ai *= 3;
                n++;
                if (n == 7) break;
            }
            QueuedConsole.WriteAndWaitOnce("a7 : {0}", ai);
        }

        /// <summary>
        /// https://brilliant.org/problems/are-you-ready-for-jee-5/
        /// </summary>
        void Orderedxy()
        {
            int nMax = 1000;
            int count = 0;
            int leastxipyi = 100;
            int maxyimxi = 0;
            for (int x = 1; x <= nMax; x++)
            {
                for (int y = 1; y <= nMax; y++)
                {
                    if (x >= y) continue;
                    if (x * y + 1 == 3 * (y - x))
                    {
                        count++;
                        if (x + y < leastxipyi) leastxipyi = x + y;
                        if (y - x > maxyimxi) maxyimxi = y - x;
                        break;
                    }
                }
            }
            int tauVal = count;
            int betaVal = leastxipyi * maxyimxi;
            QueuedConsole.WriteImmediate("ordered pairs count : {0}, beta value : {1}", tauVal, betaVal);
            QueuedConsole.WriteImmediate("tau + beta : {0}", tauVal + betaVal);
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/exploring-divisor-function-1/
        /// </summary>
        void ExploringDivisionFunction1()
        {
            Fraction<long> fr = new Fraction<long>(93261, 170104, false);
            double abyc = fr.N * 1.0 / (Math.Pow(Math.PI, 4) * fr.D);
            Fraction<long> ac = FractionConverter<long>.RealToFraction(abyc, 0.0000000001);
            QueuedConsole.WriteImmediate("a : {0}, b: {1}, c: {2}, a+b+c: {3}", ac.N, 4, ac.D, ac.N + ac.D + 4);
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/practice/quadratic-diophantine-equations-level-4-5/
        /// </summary>
        void PerfectSquare()
        {
            int n = 5;
            while (true)
            {
                if (GenericDefs.Functions.MathFunctions.IsSquare(n * (2 * n + 1))) break;
                n++;
            }
            QueuedConsole.WriteImmediate("n : {0}", n);
        }

        /// <summary>
        /// https://brilliant.org/problems/probability-45/
        /// </summary>
        void FiveCoupons()
        {
            long denominator = GenericDefs.Classes.PermutationsAndCombinations.nPr(100, 5);
            int n = 98;
            long numerator = (long)(6 * Math.Pow(n, 5) + 15 * Math.Pow(n, 4) + 10 * Math.Pow(n, 3) - n) / 120 + (long)(Math.Pow(n, 4) + 2 * Math.Pow(n, 3) + Math.Pow(n, 2)) / 8
                + (long)(2 * Math.Pow(n, 3) + 3 * Math.Pow(n, 2) + n) / 24;

            Fraction<long> fr = new Fraction<long>(numerator, denominator, false);
            QueuedConsole.WriteImmediate("a: {0}, b: {1}, a+b : {2}", fr.N, fr.D, fr.N + fr.D);
        }

        void ArraySum()
        {
            string arr = "[7, 10, 10, 2, 4, 8, 2, 7, 10, 2, 10, 2, 5, 8, 6, 4, 5, 3, 6, 4, 10, 7, 3, 2, 1, 6, 10, 9, 8, 1, 3, 6, 6, 3, 9, 5, 7, 8, 3, 7, 5]";
            arr = arr.Substring(1, arr.Length - 2).Replace(" ", "");

            string[] spArray = arr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            QueuedConsole.WriteImmediate("Array elements : {0}", spArray.Count());
            int sum = 0;
            spArray.ForEach(x => sum += int.Parse(x));
            QueuedConsole.WriteImmediate("Sum : {0}", sum);
        }

        /// <summary>
        /// https://brilliant.org/problems/a-perfect-welcome-to-2016-part-2/
        /// </summary>
        void PrimitiveRootsOfUnity2016()
        {
            SortedDictionary<int, int> Wpowers = new SortedDictionary<int, int>();
            int n = 0;
            while (++n <= 2015)
            {
                int npow = 480 * n;
                npow = npow % 2016;

                if (Wpowers.ContainsKey(npow))
                {
                    Wpowers[npow] += 1;
                }
                else { Wpowers.Add(npow, 1); }
            }

            Wpowers.OrderBy(x => x.Key);

            foreach (KeyValuePair<int, int> entry in Wpowers)
                QueuedConsole.WriteImmediate("w ^ {0}, Count: {1}", entry.Key, entry.Value);
        }

        /// <summary>
        /// https://brilliant.org/problems/roll-a-cube-with-a-tetradron/
        /// </summary>
        void RollACubeWithTetrahedron()
        {
            int n = 0, ncube = 0;
            while (ncube <= 1000000)
            {
                ++n;
                ncube = n * n * n;
                string ncubestr = "" + ncube;
                bool isValid = true;
                ncubestr.ForEach(x =>
                {
                    int nValue = (int)char.GetNumericValue(x);
                    if (nValue > 4 || nValue == 0) isValid = false;
                });
                if (isValid) QueuedConsole.WriteImmediate("n: {0}, ncube: {1}", n, ncube);
            }
        }

        void CosineSeriesProductValue()
        {
            BigInteger d = 1;
            Enumerable.Range(1, 100).ForEach(x => { d *= 3; });
            d += 1;
            
            double p = 1.0;
            BigInteger n = 1;
            Enumerable.Range(1, 100).ForEach(x =>
            {
                n *= 3;

                double val = 1 + (2 * (Math.Cos(NumberConverter.BigRationalToDouble((new BigRational(2 * n, d))) * Math.PI)));
                p *= val;
            });
            QueuedConsole.WriteImmediate("Value: {0}", p);
        }

        void LeastInt()
        {
            int n = 10000;
            while (1 > 0)
            {
                n++;
                if (n * n < 0)
                {
                    QueuedConsole.WriteImmediate("overflows at n: {0}", n);
                    break;
                }
            }
        }

        void Remainderss()
        {
            QueuedConsole.WriteImmediate("Remainder: {0}", (BigInteger.Pow(6, 98) + BigInteger.Pow(8, 98)) % 98);
        }

        void NumberOfPlayers()
        {
            int y = 0;
            while (1 > 0)
            {
                y++;
                QuadraticEquation.Root r = new QuadraticEquation.Root();
                QuadraticEquation.Solve(4, (16 - (4 * y)), -((2 * y) + 16), ref r);
                if(r.RMinus > 0 || r.RPlus > 0)
                {
                    bool found = false;
                    if(r.RMinus > 0 && r.RMinus % 1 == 0)
                    {
                        QueuedConsole.WriteImmediate("Possible value for n: {0}, y: {1}", (2 * r.RMinus) + 1, y);
                        found = true;
                    }
                    if (r.RPlus > 0 && r.RPlus % 1 == 0)
                    {
                        QueuedConsole.WriteImmediate("Possible value for n: {0}, y: {1}", (2 * r.RPlus) + 1, y);
                        found = true;
                    }
                    if (found) break;
                }
            }
            
        }

        void Series1()
        {
            double sum = 0;
            Enumerable.Range(1, 1000).ForEach(n =>
            {
                sum += 1.0 / n;
            });
            QueuedConsole.WriteImmediate("sum: {0}", sum);
        }

        void Waterproblem()
        {
            int min = int.MaxValue;
            for(int a = 1; a <= 1000; a++)
            {
                for (int b = 1; b <= 1000; b++)
                {
                    int val = a * 173 - (b * 271);
                    if (val <= 0) break;
                    if (val == 1) {
                        min = Math.Min(min, a + b);
                    }
                }
            }
            QueuedConsole.WriteImmediate("min: {0}", min);
        }

        void Divisibility()
        {
            int n = 0;
            int nMax = 0;
            while (++n <= 100000)
            {
                if ((BigInteger.Pow(n, 3) + 100) % (n + 10) == 0) nMax = n;
            }
            QueuedConsole.WriteImmediate("Max n: {0}", nMax);
        }

        void NumberOfSolutions()
        {
            int A = 1000;
            for (int i = -A; i <= A; i++)
            {
                for (int j = -A; j <= A; j++)
                {
                    if (j == 0) continue;

                    BigRational c = new BigRational(i, j);

                    BigRational b = BigRational.Pow(c, 6) + 2728;
                    b -= (3252 * c) + (2830 * c * c);
                    b -= 23 * BigRational.Pow(c, 5);
                    b += 212 * BigRational.Pow(c, 4);
                    b += 536 * BigRational.Pow(c, 3);

                    if (b.Numerator == 0)
                    {
                        QueuedConsole.WriteImmediate("Possible root for c : {0}", i);
                    }
                }
            }
        }

        void Sine()
        {
            double sum = 0.0;
            Enumerable.Range(1, 45).ForEach(x =>
            {
                sum += 1.0 / (Math.Pow(Math.Sin(2 * x - 1), 2));
            });

            QueuedConsole.WriteImmediate("Sum: {0}", sum);
        }

        /// <summary>
        /// https://brilliant.org/problems/what-series-is-this/
        /// </summary>
        void WhatSeriesIsThis()
        {
            Dictionary<string, int> FnValues = new Dictionary<string, int>();
            Func<int, int, int> Fn = null;
            Fn = delegate (int m, int n)
            {
                string key = m + ":" + n;
                if (FnValues.ContainsKey(key)) return FnValues[key];

                int retVal = 0;
                if( n == 0 && m >= 0) { retVal = 1; }
                else if (m == 0 && n > 0) { retVal = 0; }
                else if(m>0 && n > 0) {
                    retVal = Fn(m - 1, n) + Fn(m - 1, n - 1);
                }

                FnValues[key] = retVal;

                return retVal;
            };

            QueuedConsole.WriteImmediate("A(15,7): {0}", Fn(15, 7));
        }
    }
}