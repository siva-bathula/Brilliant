using GenericDefs.Classes;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using GenericDefs.Functions.NumberTheory;
using Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Brilliant.NumberTheory
{
    /// <summary>
    /// 
    /// </summary>
    public class NumberTheoryCollection : ISolve, IBrilliant, IProblemName
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("");
        }

        void ISolve.Solve()
        {
            (new Collection2()).Solve();
            //(new Remainder()).Solve();
            //(new ModularArithmetic()).Solve();
        }

        internal class Collection1
        {
            /// <summary>
            /// (phi(2017))^2 - phi(2017^2).
            /// </summary>
            internal void phi2017squared()
            {
                int num = 2017, numsq = 2017 * 2017;
                long phi2017 = EulerTotient.CalculateTotient(num);
                long phi2017sq = EulerTotient.CalculateTotient(numsq);

                QueuedConsole.WriteFinalAnswer("Diff : " + (phi2017 * phi2017 - phi2017sq));
            }

            /// <summary>
            /// https://brilliant.org/problems/more-fun-with-the-prince-of-maths/
            /// 203 + 259i
            /// </summary>
            internal void GaussianIntegers()
            {
                int n = 203 * 203 + 259 * 259;
                //Dictionary<long, int> df = Factors.GetPrimeFactorsWithMultiplicity(n);
                HashSet<long> factors = Factors.GetAllFactors(n);

                List<Tuple<int, int, int>> tu = new List<Tuple<int, int, int>>();
                foreach (long t in factors)
                {
                    UniqueIntegralPairs uip = Numbers.UnusualFunctions.NumberAsSumOfTwoSquares(t);
                    List<UniqueIntegralPairs.Combination> u = uip.GetCombinations();
                    if (u.Count == 0) continue;
                    foreach (UniqueIntegralPairs.Combination c in u)
                    {
                        tu.Add(new Tuple<int, int, int>((int)t, c.Pair[0], c.Pair[1]));
                    }
                }

                List<Tuple<int, int, int>> tu1 = tu.Select(item => item).ToList();
                List<Tuple<int, int, int>> discards = new List<Tuple<int, int, int>>();
                foreach (Tuple<int, int, int> tup in tu)
                {
                    //Dictionary<long, int> df = Factors.GetPrimeFactorsWithMultiplicity(n);
                    int num = tup.Item2 * tup.Item2 + tup.Item3 * tup.Item3;
                    HashSet<long> factors1 = Factors.GetAllFactors(num);
                    foreach (long t in factors1)
                    {
                        UniqueIntegralPairs uip = Numbers.UnusualFunctions.NumberAsSumOfTwoSquares(t);
                        List<UniqueIntegralPairs.Combination> u = uip.GetCombinations();
                        if (u.Count == 1) continue;
                        int nIndex = -1;
                        for (int i = 0; i < tu1.Count; i++)
                        {
                            Tuple<int, int, int> tup1 = tu1[i];
                            if (tup1.Item1 == tup.Item1 && tup.Item2 == tup1.Item2) { nIndex = i; break; }
                        }
                        if (nIndex >= 0)
                        {
                            discards.Add(new Tuple<int, int, int>(tu1[nIndex].Item1, tu1[nIndex].Item2, tu1[nIndex].Item3));
                            tu1.RemoveAt(nIndex);
                        }
                    }
                }
                tu.Clear();
                foreach (Tuple<int, int, int> tup in tu1)
                {
                    int f2 = n / tup.Item1;
                    //bool notGaussianPrime = false;
                    foreach (Tuple<int, int, int> dtup in discards)
                    {
                        if (dtup.Item1 == f2)
                        {
                            //notGaussianPrime = true;
                            break;
                        }
                    }
                    //if (notGaussianPrime) continue;
                    tu.Add(tup);
                    QueuedConsole.WriteFormat("factor : {0}, other factor : {1} a: {2}, b: {3}", new object[] { tup.Item1, f2, tup.Item2, tup.Item3 });
                }

                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/hexadecimal-magic-fraction/
            /// </summary>
            internal void HexadecimalFraction()
            {
                BigInteger b = new BigInteger(1);
                b *= 16; b *= 16;
                BigRational r = new BigRational(1, b);
                int n = 1;
                int cy = 0;
                while (true)
                {
                    n++;
                    b *= 16;
                    if (n == 14) n = 15;
                    r += new BigRational(n, b);
                    if (n == 15) cy++;
                    if (cy == 1) break;
                }

                Fraction<int> fr = FractionConverter<int>.BigRationalToFraction(r);
                QueuedConsole.WriteFinalAnswer("numerator : " + fr.N + " denominator : " + fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/big-sum-of-squares/
            /// </summary>
            internal void BigSumSquares()
            {
                long n1 = 2016, n2 = 641;
                UniqueIntegralPairs uipn1 = Numbers.UnusualFunctions.NumberAsSumOfTwoSquares(n1);
                List<UniqueIntegralPairs.Combination> uc = uipn1.GetCombinations();
                foreach (UniqueIntegralPairs.Combination c in uc)
                {
                    Array.Sort(c.Pair);
                    QueuedConsole.WriteImmediate(string.Format("a1 : {0}, b1: {1}", c.Pair[1], c.Pair[0]));
                }

                UniqueIntegralPairs uipn2 = Numbers.UnusualFunctions.NumberAsSumOfTwoSquares(n2);
                List<UniqueIntegralPairs.Combination> uc2 = uipn2.GetCombinations();
                foreach (UniqueIntegralPairs.Combination c in uc2)
                {
                    Array.Sort(c.Pair);
                    QueuedConsole.WriteImmediate(string.Format("a2 : {0}, b2: {1}", c.Pair[1], c.Pair[0]));
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/i-like-315-so-much-2/
            /// </summary>
            internal void Ilike315SoMuch()
            {
                List<Tuple<double, int, int>> set = new List<Tuple<double, int, int>>();
                for (int y = 1; y <= 1000; y++)
                {
                    for (int z = 1; z <= 1000; z++)
                    {
                        double a = Math.Pow(y, 3) + Math.Pow(z, 3);
                        double b = -315 * y * z;
                        double c = -45 * 49;
                        GenericDefs.Functions.NumberTheory.QuadraticEquation.Root r = new GenericDefs.Functions.NumberTheory.QuadraticEquation.Root();
                        if (GenericDefs.Functions.NumberTheory.QuadraticEquation.Solve(a, b, c, ref r))
                        {
                            if (r.RPlus > 3 && r.RPlus % 1 == 0)
                            {
                                set.Add(new Tuple<double, int, int>(r.RPlus, y, z));
                            }
                        }
                    }
                }

                foreach (Tuple<double, int, int> tup in set)
                {
                    QueuedConsole.WriteImmediate(string.Format("x : {0}, y : {1}, z : {2}", tup.Item1, tup.Item2, tup.Item3));
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/a-summoning-of-sums/
            /// </summary>
            internal void SummoningOfSums()
            {
                int n = 1;
                int[] kChk = new int[5] { 0, 0, 0, 0, 0 };
                int[] kArr = new int[5] { 0, 0, 0, 0, 0 };
                while (true)
                {
                    int count = 0;
                    bool skipN = false;
                    for (int a = 1; a <= 100; a++)
                    {
                        int lhs = a * a + a + n;
                        for (int b = 1; b <= 100; b++)
                        {
                            int rhs = b * b;
                            if (lhs == rhs)
                            {
                                count++;
                            }
                            else if (lhs - rhs < 0) break;

                            if (count > 4)
                            {
                                skipN = true;
                                break;
                            }
                        }
                        if (skipN) break;
                    }
                    if (!skipN && kArr[count] == 0)
                    {
                        kChk[count] = 1;
                        kArr[count] = n;
                    }
                    if (kChk.Sum() == 5)
                    {
                        break;
                    }
                    if (n == 100) { break; }
                    n++;
                }

                QueuedConsole.WriteImmediate("Sum : " + kArr.Sum());
                QueuedConsole.ReadKey();
            }

            internal void PlayingWithPowersOf2()
            {
                /////////////////////////////////////
                ////////////////      22123197177
                /////////////////////////////////////
                BigInteger tx = new BigInteger(1);
                int n = 0;
                while (true)
                {
                    n++;
                    if (n == 2017) break;
                    tx *= 2;
                }

                while (tx.ToString().Length > 1)
                {
                    tx = (long)(tx.ToString().ToCharArray()).Sum(a => char.GetNumericValue(a));
                }

                int N = (int)tx;
                QueuedConsole.WriteImmediate("N : " + N);

                List<long> posX = new List<long>();
                long x = 1;
                while (true)
                {
                    x++;
                    if (x == int.MaxValue) break;
                    int px = (int)(x.ToString().ToCharArray()).Sum(a => char.GetNumericValue(a));
                    BigInteger twopowpx = new BigInteger(1);
                    int c = 0;
                    while (true)
                    {
                        c++;
                        if (c > px) break;
                        twopowpx *= 2;
                    }
                    if (twopowpx % x == N) posX.Add(x);
                }

                QueuedConsole.WriteImmediate("Sum of all x > 1, : " + posX.Sum());
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// Factors of N^3
            /// https://brilliant.org/problems/factors-of-n3/
            /// </summary>
            internal void FactorsOfNpow3()
            {
                SimpleCounter counter = new SimpleCounter();
                BigInteger n1 = ((long)Math.Pow(71, 3));
                BigInteger n2 = ((long)Math.Pow(73, 2));
                BigInteger n3 = ((long)Math.Pow(79, 4));
                BigInteger N = n1 * n2 * n3;
                BigInteger Npow3 = N * N * N;
                UniqueArrangements<int> ua = new UniqueArrangements<int>();
                for (int a = 0; a <= 15; a++)
                {
                    BigInteger val = new BigInteger(1);
                    BigInteger val1 = ((long)Math.Pow(71, a));
                    for (int b = 0; b <= 15; b++)
                    {
                        BigInteger val2 = ((long)Math.Pow(73, b));
                        for (int c = 0; c <= 15; c++)
                        {
                            BigInteger val3 = ((long)Math.Pow(79, c));
                            val = val1 * val2 * val3;
                            if (val >= N) break;
                            if (val < N && N % val != 0 && Npow3 % val == 0)
                            {
                                if (ua.Add(new List<int>() { a, b, c })) counter.Increment();
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Divisor count : " + counter.GetCount().ToString());
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// Factors of N^3
            /// https://brilliant.org/problems/factors-on-n3/
            /// </summary>
            internal void FactorsOfNpow3_2()
            {
                SimpleCounter counter = new SimpleCounter();
                BigInteger n1 = ((BigInteger)Math.Pow(5791, 3));
                BigInteger n2 = ((BigInteger)Math.Pow(5801, 2));
                BigInteger n3 = ((BigInteger)Math.Pow(5807, 4));
                BigInteger N = n1 * n2 * n3;
                BigInteger Npow3 = N * N * N;
                UniqueArrangements<int> ua = new UniqueArrangements<int>();
                for (int a = 0; a <= 20; a++)
                {
                    BigInteger val = new BigInteger(1);
                    BigInteger val1 = new BigInteger(1);
                    int aC = 0;
                    while (true)
                    {
                        if (aC == a) break;
                        val1 *= 5791;
                        aC++;
                    }
                    for (int b = 0; b <= 20; b++)
                    {
                        BigInteger val2 = new BigInteger(1);
                        int bC = 0;
                        while (true)
                        {
                            if (bC == b) break;
                            val2 *= 5801;
                            bC++;
                        }
                        for (int c = 0; c <= 20; c++)
                        {
                            BigInteger val3 = new BigInteger(1);
                            int cC = 0;
                            while (true)
                            {
                                if (cC == c) break;
                                val3 *= 5807;
                                cC++;
                            }
                            val = val1 * val2 * val3;
                            if (val >= N) break;
                            if (val < N && N % val != 0 && Npow3 % val == 0)
                            {
                                if (ua.Add(new List<int>() { a, b, c })) counter.Increment();
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Divisor count : " + counter.GetCount().ToString());
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// Find three times the sum of the absolute values of the rational numbers a, for which x^3 - x - a^3
            /// is reducible over the rationals.
            /// https://brilliant.org/problems/it-is-deducible-that-this-cant-be-reducible/
            /// </summary>
            internal void PolynomialReducibleOverRationals()
            {
                HashSet<double> aPoss = new HashSet<double>();
                for (int q = 1; q <= 1000; q++)
                {
                    long q2 = q * q;
                    for (int p = 1; p <= 1000; p++)
                    {
                        long p2 = p * p;
                        long nume = 4 * q2 - p2;
                        if (nume < 0) break;

                        if (nume % 3 == 0 && nume % q2 == 0)
                        {
                            Fraction<long> fr = new Fraction<long>(nume, 3 * q2, false);
                            if (GenericDefs.OtherProjects.Numbers.IsSquare(fr.N) && GenericDefs.OtherProjects.Numbers.IsSquare(fr.D))
                            {
                                double a3 = Math.Sqrt(fr.N / fr.D) * (fr.N / fr.D - 1);
                                aPoss.Add(a3);
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Average a : " + aPoss.Sum() / aPoss.Count);
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// Does there exist a right triangle with three rational sides whose area is 5? 
            /// If so, find the least common denominator of the sides; if not, enter 666.
            /// https://brilliant.org/problems/inspired-by-fibonacci/
            /// </summary>
            internal void InspiredByFibonacci()
            {
                for (int i = 1; i <= 4; i++)
                {
                    int ca = 1, cb = 1;
                    if (i == 1) { ca = 2; cb = 5; }
                    else if (i == 2) { ca = 5; cb = 2; }
                    else if (i == 3) { ca = 10; cb = 1; }
                    else if (i == 4) { ca = 1; cb = 10; }

                    for (int a = 1; a <= 100; a++)
                    {
                        for (int b = 1; b <= 100; b++)
                        {
                            if (a == b) continue;
                            Fraction<int> adivb = FractionConverter<int>.RealToFraction((a * 1.0 / b), 0.000000001);
                            int numcsq = (ca * (int)Math.Pow(adivb.N, 4)) + (cb * (int)Math.Pow(adivb.D, 4));

                            if (MathFunctions.IsSquare(numcsq))
                            {
                                QueuedConsole.WriteImmediate(string.Format("side a : {0}x({1}/{2})^2, side b : {3}x({2}/{1})^2, side c : {4}/({1}x{2}): "
                                    , ca, a, b, cb, ((int)Math.Sqrt(numcsq))));
                                continue;
                            }
                        }
                    }
                }

                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// f(x) for x > 0, is number of k for which k mod (2k-1) = x mod (2k-1);
            /// https://brilliant.org/problems/peculiar-mod/
            /// </summary>
            internal void PeculiarMod()
            {
                int oddfx = 0;
                for (int i = 1; i <= 2014; i++)
                {
                    int count = 0;
                    for (int j = 1, k = 2 * j - 1; j <= 2014; j++, k = 2 * j - 1)
                    {
                        if (i % k == j % k)
                        {
                            count++;
                        }
                    }
                    if (count % 2 != 0)
                    {
                        oddfx++;
                    }
                }
                QueuedConsole.WriteImmediate("f(x) is odd for : " + oddfx + " integers.");
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/challenge-for-champions-2/
            /// </summary>
            internal void ChallengeForChampions2()
            {
                int x = 1;
                HashSet<int> aSet = new HashSet<int>();
                while (true)
                {
                    int bxpow100Mod1000 = 1;
                    int counter = 0;
                    while (true)
                    {
                        counter++;
                        bxpow100Mod1000 *= x;
                        bxpow100Mod1000 %= 1000;
                        if (counter == 100) break;
                    }
                    bxpow100Mod1000 %= 1000;
                    aSet.Add(bxpow100Mod1000);
                    x++;
                    if (x == 98) break;
                }

                QueuedConsole.WriteImmediate(string.Format("Sum of all possible values of a : {0}", aSet.Sum()));
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/all-about-factorials/
            /// </summary>
            internal void AllAboutFactorials()
            {
                int a = 1;
                List<BigInteger> bList = new List<BigInteger>();
                while (true)
                {
                    bList.Add(Numbers.Factorial(a));
                    a++;
                    if (a == 15) break;
                }

                a = 1;
                while (true)
                {
                    int x = 1;
                    BigInteger b1 = 0, b2 = 0;
                    while (true)
                    {
                        BigInteger factorial = Numbers.Factorial(x);
                        if (x <= a) { b2 += factorial; }
                        if (x <= 2 * a) { b1 += factorial; }
                        x++;
                        if (x > 2 * a) break;
                    }
                    BigInteger n = b1 - b2;
                    foreach (BigInteger bi in bList)
                    {
                        if (n % bi == 0)
                        {
                            QueuedConsole.WriteImmediate(string.Format("n : {0}, b! : {1}, n/ b! : {2}", n.ToString(), bi.ToString(), (n / bi).ToString()));
                            if (GenericDefs.OtherProjects.Numbers.IsSquare(Convert.ToInt64((n / bi).ToString())))
                            {
                                QueuedConsole.WriteImmediate(string.Format(" n/b! = (a!)^2 : {0}", (n / bi).ToString()));
                            }
                        }
                    }
                    a++;
                    if (a == 10) break;
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/oldest-problem-on-brilliant/
            /// </summary>
            void OldestProblemOnBrilliant()
            {
                long n = 0, k = 0;
                long sign = -1;
                double A = 0.0;
                while (true)
                {
                    sign *= -1;
                    k = 2 * n + 1;

                    long fCount = Factors.GetAllFactors(k).Count;
                    A += 1.0 * sign * fCount / k;
                    n++;
                    if (n == 10000000) break;
                }

                QueuedConsole.WriteImmediate(string.Format("A : {0}", A));
                QueuedConsole.WriteImmediate(string.Format("Math.Floor(100000*A) : {0}", Math.Floor(100000 * A)));

                QueuedConsole.WriteImmediate(string.Format("phi - 1 : {0}", (Math.Sqrt(5) - 1) / 2.0));
                QueuedConsole.WriteImmediate(string.Format("Math.Floor(100000*(phi-1)) : {0}", Math.Floor(100000 * (Math.Sqrt(5) - 1) / 2.0)));
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/a-golden-fraction/
            /// </summary>
            void AGoldenFraction()
            {
                BigInteger denominator = BigInteger.Parse("99999999989999999999");
                BigInteger numerator = BigInteger.Parse("100000000000000000000");

                string val = Numbers.BigIntegerArithmetic.DecimalPartOfDivision(numerator, denominator, 1000);
                QueuedConsole.WriteImmediate("Number is : " + val);
                QueuedConsole.WriteImmediate("substring 240-250 : " + val.Substring(239, 11));
                QueuedConsole.WriteFinalAnswer("250th digit is : " + val[249]);
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-abhay-tiwari/
            /// </summary>
            void Nsquare987654321()
            {
                ////////////////////////////
                //wrong approach. correct would be to take approx. square root,
                //of 987654321*10^n, with n odd and n even, and examining the values with increasing order of n.
                ////////////////////////////
                BigInteger nStart = 3000;
                BigInteger nMax = nStart * new BigInteger(900000000000);
                BigInteger n = nStart;
                string nStr = string.Empty;
                string nsq = string.Empty;
                string startsWith = "987654321";
                bool found = false;
                List<BigInteger> solutions = new List<BigInteger>();
                while (true)
                {
                    nStr = n.ToString();
                    if (nStr.StartsWith("4"))
                    {
                        nStart = BigInteger.Parse("" + 9 + nStart.ToString().Substring(1, nStr.Length - 1));
                        n = nStart;
                    }
                    else if (nStr.StartsWith("9"))
                    {
                        nStart = BigInteger.Parse("" + 3 + nStart.ToString().Substring(1, nStr.Length - 1) + 0);
                        n = nStart;
                    }
                    if (n >= nMax) break;
                    nsq = (n * n).ToString();
                    if (nsq.StartsWith(startsWith))
                    {
                        solutions.Add(n);
                        found = true;
                        if (solutions.Count == 5) break;
                    }
                    n++;
                }
                if (!found)
                {
                    QueuedConsole.WriteImmediate("Not found. Change range.");
                    QueuedConsole.ReadKey();
                }
                else
                {
                    solutions.Sort();
                    QueuedConsole.WriteFinalAnswer("N is {0}, Nsq is : {1}", (solutions[0]).ToString(), (solutions[0] * solutions[0]).ToString());
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-helpmr-wilson/
            /// </summary>
            void HelpMrWilson()
            {
                BigInteger nume = 1;
                BigInteger deno = 1;
                int n = 1;
                while (true)
                {
                    nume *= (n + 997);
                    deno *= n;
                    n++;

                    if (nume.ToString().Length > 15)
                    {
                        BigIntFraction t = new BigIntFraction(nume, deno, false);
                        nume = t.N;
                        deno = t.D;
                    }

                    if (n == 998) break;
                }
                nume /= deno;
                BigInteger divisor = 997 * 997 * 997;
                QueuedConsole.WriteFinalAnswer("Remainder is : {0}", nume % divisor);
            }

            /// <summary>
            /// https://brilliant.org/problems/varsigma/
            /// </summary>
            void SquareFreeVarSigma()
            {
                long n = 2;
                int nMax = 1000000;
                double S = 0.0;
                while (true)
                {
                    Dictionary<long, int> factors = Factors.GetPrimeFactorsWithMultiplicity(n);

                    long sqFreeFactor = 1;
                    foreach (KeyValuePair<long, int> kvp in factors)
                    {
                        sqFreeFactor *= kvp.Key;
                    }

                    S += (Math.Log(sqFreeFactor) / (1.0 * n * n));
                    n++;
                    if (n % 10000 == 0)
                    {
                        QueuedConsole.WriteImmediate("current n : {0}, S: {1}", n, S);
                    }
                    if (n > nMax) break;
                }

                QueuedConsole.WriteImmediate("The summation is : {0}, ", S);
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/maximizing-a-base/
            /// A five digit number in base n has digits n-1, n-2, n-3, n-4, and n-5, arranged in any order. What is the maximum value of n such that the five digit number is divisible by n-1?
            /// </summary>
            void BaseNDivisibility()
            {
                int n = 5;
                int maxN = 0;
                while (true)
                {
                    List<int> digits = new List<int>();
                    for (int i = 1; i <= 5; i++) digits.Add(n - i);

                    IEnumerable<IEnumerable<int>> permutations = Permutations.GetPermutations(digits, 5);

                    foreach (IEnumerable<int> p in permutations)
                    {
                        List<int> per = p.ToList();
                        int k = 0;
                        long sum = (long)per.Sum(x => x * (Math.Pow(n, k++)));
                        if (sum % (n - 1) == 0) { maxN = Math.Max(n, maxN); break; }
                    }

                    n++;
                    if (n == 100) break;
                }
                QueuedConsole.WriteImmediate("Maximum value of n : {0}", maxN);
            }

            /// <summary>
            /// https://brilliant.org/problems/185-followers-special/
            /// </summary>
            void RationalRoots()
            {
                ///////////////////////////////////////////////
                ////////// estimation only. not a solution.
                ///////////////////////////////////////////////
                //x1+x2+x3 = 5, y1+y2+y3 = 4; product of roots = 32/81.
                UniqueArrangements<int> uaN = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(5, 3, -3, 10);
                UniqueArrangements<int> uaD = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(4, 3, -3, 10);

                List<List<int>> nSets = uaN.ExtractAsSets();
                List<List<int>> dSets = uaD.ExtractAsSets();
                int leastValue = int.MaxValue;
                foreach (List<int> n in nSets)
                {
                    foreach (List<int> d in dSets)
                    {
                        IEnumerable<IEnumerable<int>> permutations = Permutations.GetPermutations(d, 3);
                        foreach (IEnumerable<int> p in permutations)
                        {
                            List<int> per = p.ToList();
                            double x1 = Math.Pow(2, n[0]) / Math.Pow(3, per[0]);
                            double x2 = Math.Pow(2, n[1]) / Math.Pow(3, per[1]);
                            double x3 = Math.Pow(2, n[2]) / Math.Pow(3, per[2]);
                            double lhs = x1 + 2 * x2 + 3 * x3;
                            if (lhs > (4.0 - 0.01) && lhs < (4.0 + 0.01))
                            {
                                Fraction<int> fr = FractionConverter<int>.RealToFraction((x1 * x2) + (x1 * x3) + (x2 * x3), 0.0000000001);
                                leastValue = Math.Min(leastValue, (5 * fr.D) - (2 * fr.N));
                            }
                        }
                    }
                }

                QueuedConsole.WriteImmediate("Least value : {0}", leastValue);
            }

            /// <summary>
            /// https://brilliant.org/problems/coprime-game-i-2/
            /// </summary>
            void CoprimeGame()
            {
                int n = 200;
                while (true)
                {
                    List<int> Coprimes = new List<int>();
                    for (int i = 1; i < n; i++)
                    {
                        if (MathFunctions.GCD(n, i) == 1) Coprimes.Add(i);
                    }
                    QueuedConsole.WriteImmediate("n: {0}, Coprime-count : {1}, Coprimes : {2}", n, Coprimes.Count, string.Join(",", Coprimes));
                    if ((--n) == 1) break;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/divisor-game/
            /// </summary>
            void DivisorGame()
            {
                ////////////////////////////////////////////////////
                /////////////// ignore
                ////////////////////////////////////////////////////
                int n = 200;
                ClonedPrimes cPrimes = KnownPrimes.CloneKnownPrimes(1, 200);

                while (true)
                {
                    HashSet<long> divisors = Factors.GetAllFactors(n, cPrimes);
                    //foreach(long t in divisors)
                    //{
                    //    HashSet<long> d1 = Factors.GetAllFactors(t, cPrimes);
                    //    QueuedConsole.WriteImmediate("d : {0}, divisors : {1}, count : {2} ", t, string.Join(",", d1), d1.Count);
                    //}
                    QueuedConsole.WriteImmediate("n : {0}, count : {1} ", n, divisors.Count);
                    if ((--n) == 1) break;
                    //break;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/fractions-sum-to-1/
            /// </summary>
            void FractionSumTo1()
            {
                double sum = 0.0;
                int count = 0, nMax = 100;
                for (int a = 2; a <= nMax; a++)
                {
                    sum = (1.0 / a);
                    for (int b = a; b <= nMax; b++)
                    {
                        double sum1 = sum + (1.0 / b);
                        if (sum1 >= 1.0) continue;
                        for (int c = b; c <= nMax; c++)
                        {
                            double sum2 = sum1 + (1.0 / c);
                            if (sum2 >= 1.0) continue;
                            for (int d = c; d <= nMax; d++)
                            {
                                double sum3 = sum2 + (1.0 / d);
                                if (sum3 > 1.0) continue;
                                if (sum3 == 1.0)
                                {
                                    QueuedConsole.WriteImmediate("Possible solution, a: {0}, b: {1}, c: {2}, d: {3}", a, b, c, d);
                                    count++;
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of solutions : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/such-years-such-summations-much-wow/
            /// </summary>
            void SuchYearsSuchSummations()
            {
                BigInteger sum = 0, _2pown = 1;
                int n = 2, t = 0;
                BigInteger _2pow2014 = 1;
                while (true)
                {
                    sum += _2pown * n;
                    n++;
                    if (n > 2016)
                    {
                        _2pow2014 = _2pown;
                        break;
                    }
                    _2pown *= 2;
                    t++;
                }

                sum *= ((2 * _2pow2014) - 1);
                sum += (2015 * _2pow2014);

                sum /= 2015;
                sum /= _2pow2014;
                BigInteger _number = (4 * _2pow2014) - 1;
                string _numberStr = "(2^2016 - 1)";
                QueuedConsole.WriteImmediate("Sum is {0} ? {1}", _numberStr, sum == _number);
                QueuedConsole.WriteImmediate("Sum is divisible by {0} ? {1}", _numberStr, ((sum + _number) % _number) == BigInteger.Zero);

                int b = 0;
                BigInteger a = _number;
                BigInteger c = 0;
                while (true)
                {
                    BigInteger sumCopy = sum + a;
                    b = 0;
                    if (sumCopy % a == 0)
                    {
                        while (true)
                        {
                            if (sumCopy % a == 0)
                            {
                                sumCopy /= a;
                                b++;
                            }
                            else
                            {
                                QueuedConsole.WriteImmediate("{0} is a factor. Divides sum {1} times", _numberStr, b);
                                break;
                            }
                        }
                        if (b > 2)
                        {
                            c = sumCopy;
                            break;
                        }
                    }
                    a++;
                    if (a >= 100000000) break;
                }

                QueuedConsole.WriteImmediate("a: {0}, b: {1}, c: {2}", _numberStr, b, c.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/3-factors-2-factors/
            /// </summary>
            void _3factors2factors()
            {
                List<long> primes = KnownPrimes.GetClonedPrimes(1, 10000);
                int sum = 0;
                foreach (long p in primes)
                {
                    long t = (p - 4) * (p + 1) * (p + 3) + 16;
                    if (MathFunctions.IsSquare(t)) sum += (int)p;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/why-three-digit-integers-only/
            /// </summary>
            void WhyThreeDigitIntegersOnly()
            {
                int n1 = 100, n2 = 100;
                int leastN = int.MaxValue;
                while (true)
                {
                    n2 = 100;
                    while (true)
                    {
                        int num = int.Parse(n1 + "" + n2);
                        if (num % n2 == 0)
                        {
                            leastN = Math.Min(leastN, num / n2);
                        }
                        n2++;
                        if (n2 == 1000) break;
                    }
                    n1++;
                    if (n1 == 1000) break;
                }
                QueuedConsole.WriteImmediate("Smallest integer N : {0}", leastN);
            }

            /// <summary>
            /// https://brilliant.org/problems/why-three-digit-integers-only-part-2/
            /// </summary>
            void WhyThreeDigitIntegersOnly_Part2()
            {
                int n1 = 100, n2 = 100;
                int sum = 0;
                while (true)
                {
                    if (MathFunctions.IsSquare(n1))
                    {
                        n2 = n1;
                        while (true)
                        {
                            if (MathFunctions.IsSquare(n2))
                            {
                                int diff = n2 - n1;
                                if (diff >= 0 && MathFunctions.IsSquare(diff))
                                {
                                    int num = int.Parse(n1 + "" + n2);
                                    if (MathFunctions.IsSquare(num))
                                    {
                                        if (num % n2 == 0)
                                        {
                                            int num2 = num / n2;
                                            if (MathFunctions.IsSquare(num2)) { sum += n1 + n2; }
                                        }
                                    }
                                }
                            }
                            n2++;
                            if (n2 == 1000) break;
                        }
                    }
                    n1++;
                    if (n1 == 1000) break;
                }
                QueuedConsole.WriteImmediate("Sum of all possible values of A and B : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/we-all-love-primes/
            /// </summary>
            void WeAllLovePrimes()
            {
                List<long> primes = KnownPrimes.GetClonedPrimes(2, 1000);
                long sum = 0;
                foreach (long p in primes)
                {
                    bool found = false;
                    long a = p + 1;
                    long psq = p * p;
                    long _1000p = 1000 * p;
                    while (true)
                    {
                        long asq = a * a;

                        if (a * (asq + psq) % (asq - psq) == 0)
                        {
                            sum = (a * (asq + psq) / (asq - psq)) + a + p;
                            QueuedConsole.WriteImmediate("a: {0}, b: {1}, p: {2}", a, (a * (asq + psq) / (asq - psq)), p);
                            found = true;
                            break;
                        }
                        if (a > _1000p) break;

                        a++;
                    }
                    if (found) break;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/not-a-taxicab-number/
            /// </summary>
            void NotATaxiCabNumber()
            {
                int n = 2000, nMax = 2099, nCount = 0;
                while (true)
                {
                    UniqueIntegralPairs uip = Numbers.UnusualFunctions.NumberAsSumOfTwoSquares(n);
                    if (uip.Count() >= 1)
                    {
                        nCount++;
                        QueuedConsole.WriteImmediate("n : {0}, count : {1}", n, uip.Count());
                        if (uip.Count() >= 2)
                        {
                            foreach (UniqueIntegralPairs.Combination c in uip.GetCombinations())
                            {
                                QueuedConsole.WriteImmediate("Pair : {0}, {1}", c.Pair[0], c.Pair[1]);
                            }
                        }
                    }
                    if (++n > nMax) break;
                }
                QueuedConsole.WriteImmediate("Total number of integers : {0}", nCount);
            }

            /// <summary>
            /// https://brilliant.org/problems/sum-of-squares-17/
            /// </summary>
            void SumofSquares()
            {
                long sum = 1;
                int n = 2;
                while (true)
                {
                    sum += n * n;
                    if (MathFunctions.IsSquare(sum)) { break; }
                    n++;
                }
                QueuedConsole.WriteImmediate("Smallest a : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/interesting-equation/
            /// </summary>
            void InterestingEquation()
            {
                int xyz = 0;
                HashSet<int> xyzSet = new HashSet<int>();
                for (int x = -100; x <= 100; x++)
                {
                    for (int y = -100; y <= 100; y++)
                    {
                        int z = 3 - x - y;
                        int sum = (int)(Math.Pow(x, 3) + Math.Pow(y, 3) + Math.Pow(z, 3) - 3);
                        if (sum == 0)
                        {
                            int p = x * y * z;
                            QueuedConsole.WriteImmediate("Possible values. x: {0}, y: {1}, z: {2}, xyz: {3}", x, y, z, p);
                            if (!xyzSet.Contains(p)) { xyz += p; xyzSet.Add(p); }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Sum of distinct values of xyz : {0}", xyz);
            }

            /// <summary>
            /// https://brilliant.org/problems/is-it-enough-information-2/
            /// </summary>
            void IsItEnoughInformation()
            {
                int nMax = 997 / 4;
                int answer = 0;
                for (int a = 1; a <= nMax; a++)
                {
                    bool found = false;
                    for (int b = 1; b <= nMax; b++)
                    {
                        for (int c = 1; c <= nMax; c++)
                        {
                            int sum = a * b * c + (a * b + b * c + c * a) + a + b + c;
                            if (sum > 1000) break;
                            if (sum == 1000) { answer = a + b + c; found = true; break; }
                        }
                        if (found) break;
                    }
                    if (found) break;
                }
                QueuedConsole.WriteImmediate("a+b+c : {0}", answer);
            }

            void PolynomialEquation()
            {
                int nMax = 2000;
                int answer = 0;
                bool found = false;
                for (int a = 0; a <= nMax; a++)
                {
                    for (int b = -nMax; b <= nMax; b++)
                    {
                        for (int c = -nMax; c <= nMax; c++)
                        {
                            int sum = (a * a * a) + (b * b * b) + (c * c * c);
                            if (sum == 30)
                            {
                                answer = a + b + c; found = true; break;
                            }
                        }
                        if (found) break;
                    }
                    if (found) break;
                }
                QueuedConsole.WriteImmediate("a+b+c : {0}", answer);

            }

            /// <summary>
            /// https://brilliant.org/problems/find-and-find-again/
            /// </summary>
            void FindAndFindAgain()
            {
                int sum = 0;
                int n = 0;
                int d = 100;
                while (n < 101)
                {
                    int t = 1;
                    int tFact = 1;
                    while (t < n + 1)
                    {
                        tFact *= t;
                        tFact %= d;
                        t++;
                    }
                    sum += tFact;
                    n++;
                }
                int x = sum % d;

                sum = 0;
                n = 0;
                while (n < 101)
                {
                    int t = 1;
                    int tPow = 1;
                    while (t < x + 1)
                    {
                        tPow *= n;
                        tPow %= d;
                        t++;
                    }
                    sum += tPow;
                    n++;
                }
                QueuedConsole.WriteImmediate("Last 2 digits : {0}", sum % d);
            }
        }

        internal class Collection2
        {
            internal void Solve()
            {
                Divisibility1To9();
            }

            /// <summary>
            /// https://brilliant.org/problems/divisibility-from-1-to-9/
            /// </summary>
            void Divisibility1To9()
            {
                Dictionary<int, string> d = new Dictionary<int, string>();
                string s = "0";

                Enumerable.Range(2, 8).ForEach(n =>
                {
                    s += "," + (n - 1);
                    d.Add(n, s);
                });

                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    bool found = true;
                    foreach (KeyValuePair<int, string> kvp in d)
                    {
                        int num = cr.ExtractValue(kvp.Value);
                        if (!(num % kvp.Key == 0)) { found = false; break; }
                    }
                    return found;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < 9; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 1 });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(9, iterators);
                ua.Extract().ForEach(x =>
                {
                    QueuedConsole.WriteImmediate("{0}", x);
                });
            }

            /// <summary>
            /// https://brilliant.org/problems/remainders-of-powers-of-2/
            /// </summary>
            void PowersOf2()
            {
                int n = 0, count = 0;
                while (++n <= 1000)
                {
                    BigInteger num = 1;
                    List<int> remainders = new List<int>();
                    while (true)
                    {
                        int rem = int.Parse((num % n).ToString());
                        if (!remainders.Contains(rem))
                        {
                            remainders.Add(rem);
                        } else break;
                        num *= 2;
                    }
                    if (remainders.Sum() % n == 0)
                    {
                        count++;
                    }
                }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/squarish-numbers/
            /// </summary>
            void SquarishNumbers()
            {
                int n = 0;
                int sn = 0;
                double[] a = new double[1] { 0.75 };
                while (++n <= 100000000)
                {
                    if (MathFunctions.IsPerfectSquare(n))
                    {
                        sn += 1;
                    }
                    else
                    {
                        int sqrt = (int)Math.Sqrt(n);
                        if (MathFunctions.IsPerfectSquare((long)Math.Pow((sqrt + 1), 2) - n))
                        {
                            sn += 1;
                        }
                    }
                    if (n % 10000000 == 0)
                    {
                        QueuedConsole.WriteImmediate("n: {0}", n);
                        a.ForEach(ai =>
                        {
                            QueuedConsole.WriteImmediate("a: {0}, limit: {1}", ai, (sn * 1.0 / Math.Pow(n, ai)));
                        });
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/these-are-some-odd-digits/
            /// </summary>
            void TheseAreSomeOddDigits()
            {
                List<long> n1to10 = new List<long>();
                long N = long.Parse("999999991");
                long x = 1;
                long Nx = N;
                while (++x > 0)
                {
                    Nx += N;

                    bool found = true;
                    long NxCopy = Nx;
                    while (NxCopy > 0)
                    {
                        int digit = (int)(NxCopy % 10);
                        NxCopy /= 10;
                        if (digit % 2 == 0)
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        n1to10.Add(x);
                        if (n1to10.Count == 10) break;
                    }
                }
                long sum = 0;
                n1to10.ForEach(n =>{ sum += n; });
                QueuedConsole.WriteImmediate("{0}", Strings.ToDisplayNumber(sum));
            }

            /// <summary>
            /// https://brilliant.org/problems/an-arithmetic-differential-equation-2/
            /// </summary>
            void ArithmeticDifferentialEquation()
            {
                BigRational sum = new BigRational(0, 1);
                HashSet<string> set = new HashSet<string>();

                Dictionary<int, Dictionary<long, int>> PrimeFactors = new Dictionary<int, Dictionary<long, int>>();
                ClonedPrimes c = new ClonedPrimes(1, 1000);
                for (int n = 1; n <= 999; n++)
                {
                    PrimeFactors.Add(n, Factors.GetPrimeFactorsWithMultiplicity(n, c));
                }
                for (int b = 1; b <= 998; b++)
                {
                    for (int a = b + 1; a < 1000; a++)
                    {
                        Fraction<int> ab = new Fraction<int>(a, b, false);
                        BigRational abyb = new BigRational(ab.N, ab.D);
                        Dictionary<long, int> apf = PrimeFactors[ab.N];
                        Dictionary<long, int> bpf = PrimeFactors[ab.D];

                        BigRational adofa = new BigRational(0, 1);
                        apf.ForEach(n =>
                        {
                            adofa += new BigRational(n.Value, n.Key);
                        });
                        adofa *= a;
                        BigRational adofb = new BigRational(0, 1);
                        bpf.ForEach(n =>
                        {
                            adofb += new BigRational(n.Value, n.Key);
                        });
                        adofb *= b;

                        BigRational adofabyb = ((adofa * b) - (adofb * a)) / (b * b);
                        if (3 * abyb == adofabyb)
                        {
                            string key = abyb.Numerator + "@" + abyb.Denominator;
                            if (!set.Contains(key))
                            {
                                sum += abyb;
                                set.Add(key);
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("{0}", sum.Numerator + sum.Denominator);
            }

            /// <summary>
            /// https://brilliant.org/problems/divisible-by-all/
            /// </summary>
            void DivisibleByAll()
            {
                int n = 0, sum = 0;
                while(++n <= 100000)
                {
                    int a = -1;
                    bool conditionMet = true;
                    while (true)
                    {
                        a += 2;

                        if (a * a <= n)
                        {
                            if (n % a == 0) continue;
                            else { conditionMet = false; break; }
                        }
                        else break;
                    }
                    if (conditionMet)
                    {
                        sum += n;
                        QueuedConsole.WriteImmediate("n : {0}", n);
                    }
                }
                QueuedConsole.WriteImmediate("sum of all n : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/pairs-of-perfect-squares/
            /// </summary>
            void PairsOfPerfectSquares()
            {
                int sum = 0;
                for (int N = 1; N <= 100; N++)
                {
                    string Nsq = ((N * N).ToString());
                    if (!(Nsq.Contains("9") || Nsq.Contains("8") || Nsq.Contains("7")))
                    {
                        double incNum = 0;
                        Nsq.ForEach(n =>
                        {
                            incNum += (char.GetNumericValue(n) + 3);
                            incNum *= 10;
                        });
                        incNum /= 10;

                        if (MathFunctions.IsPerfectSquare((long)incNum))
                        {
                            sum++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("{0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/are-you-sure-finding-3-digit-numbers-can-be-this/
            /// </summary>
            void _3digitNumbers()
            {
                int sum = 0;
                Dictionary<int, int> Factorial = new Dictionary<int, int>();
                Enumerable.Range(0, 7).ForEach(n =>
                {
                    Factorial.Add(n, (int)PermutationsAndCombinations.Factorial(n));
                });

                for(int a = 1; a <= 6; a++)
                {
                    for (int b = 0; b <= 6; b++)
                    {
                        for (int c = 0; c <= 6; c++)
                        {
                            int n = (100 * a) + (10 * b) + c;
                            int rhs = 3 * Factorial[a] * Factorial[b] * Factorial[c];
                            if (2 * n == rhs) sum += n;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("{0}", sum);
            }

            void MordellEquation()
            {
                long sum = 0;
                List<long> primes = KnownPrimes.GetPrimes(6131, 10000);
                primes.ForEach(p =>
                {
                    for(long x = p + 1; x <= p + 2000; x++)
                    {
                        long ysq = (x * x * x) - (p * p * p);
                        if(ysq % p != 0 && ysq % 3 != 0)
                        {
                            if (MathFunctions.IsSquare(ysq))
                            {
                                sum += (x + p);
                                QueuedConsole.WriteImmediate("x: {0}, p: {1}", x, p);
                            }
                        }
                    }
                });
                QueuedConsole.WriteImmediate("{0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/mode-of-remainders/
            /// </summary>
            void ModeOfRemainders()
            {
                Dictionary<int, int> d = new Dictionary<int, int>();
                Enumerable.Range(1, 1000).ForEach(n =>
                {
                    int r = 1000 % n;
                    if (d.ContainsKey(r)) d[r]++;
                    else d.Add(r, 1);
                });
                Dictionary<int, int> d1 = d.OrderByValue(OrderByDirection.Descending);
                QueuedConsole.WriteImmediate("mode : {0}", d1[0]);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-decade-of-divisibility/
            /// </summary>
            void ADecadeOfDivisibility()
            {
                int N = 2017 * 2027;
                long rem = 0, rem1 = 1, rem2 = 1;
                Enumerable.Range(1, 2026).ForEach(n =>
                {
                    rem1 *= 2017;
                    rem1 %= N;
                    if(n <= 2016)
                    {
                        rem2 *= 2027;
                        rem2 %= N;
                    }
                });
                rem = rem1 + rem2 - 1;
                rem %= N;
                QueuedConsole.WriteImmediate("{0}", rem);
            }

            /// <summary>
            /// https://brilliant.org/problems/fractional-diophantine-equation-2/
            /// </summary>
            void FractionalDiophantineEquation()
            {
                int Nmax = 500;
                HashSet<int> values = new HashSet<int>();
                for (int a = 1; a <= Nmax; a++)
                {
                    for (int b = 1; b <= Nmax; b++)
                    {
                        for (int c = 1; c <= Nmax; c++)
                        {
                            int rhs = a * b * c;
                            int lhs = (a * (b + c)) + (b * c) + 24;

                            if (lhs == rhs) { values.Add(rhs); }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("{0}, sum : {1}", values.Count, values.Sum());

            }

            /// <summary>
            /// https://brilliant.org/problems/non-homogenous-diophantine-equation/
            /// </summary>
            void NonHomogenousDiophantineEquation()
            {
                int count = 0;
                for (int a = 1; a <= 100; a++)
                {
                    int asq = a * a;
                    for(int b = 1; b <= 1000; b++)
                    {
                        for (int c = 1; c <= 1000; c++)
                        {
                            int rhs = a * b * c;
                            rhs -= (b + c);

                            if (asq == rhs) count++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/i-3-squares/
            /// </summary>
            void Squares()
            {
                int count = 0;
                int X = 0;
                int n2 = 0;
                while (1>0)
                {
                    n2++;
                    X = (4 * n2 * (n2 + 1)) + 6;
                    if (X > 1000000) break;
                    int n1sq = X - 2;
                    if(n1sq % 3 == 0)
                    {
                        n1sq /= 3;
                        if (MathFunctions.IsPerfectSquare(n1sq)) count++;
                    }
                }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/twin-prime-problem/
            /// </summary>
            void TwinPrimeProblem()
            {
                List<long> primes = KnownPrimes.GetAllKnownPrimes();
                List<Tuple<long, long>> twinPrimes = new List<Tuple<long, long>>();
                long prev = 0;
                primes.ForEach(n =>
                {
                    if (prev > 3)
                    {
                        if(prev == n - 2)
                        {
                            twinPrimes.Add(new Tuple<long, long>(prev,n));
                        }
                    }
                    prev = n;
                });

                int N = 5;
                BigInteger b = (BigInteger)(Math.Pow(2, N) - 1);
                bool found = false;
                while (1 > 0)
                {
                    foreach(Tuple<long, long> tp in twinPrimes)
                    {
                        if(b % tp.Item1 == 0 && b % tp.Item2 == 0)
                        {
                            QueuedConsole.WriteImmediate("n:{0}, p:{1}, q:{2}", N, tp.Item1, tp.Item2);
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                    else {
                        N += 2;
                        b *= 4;
                        b += 3;
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/divides-by-143/
            /// </summary>
            void DividesBy143()
            {
                BigInteger num = BigInteger.Parse(BaseConversion.ConvertFromBaseAToBaseB("11221133112211", 12, 10));
                QueuedConsole.WriteImmediate("{0}", num % 143 == 0 ? "true" : "false");
            }

            /// <summary>
            /// https://brilliant.org/problems/more-fun-in-2015-part-13/
            /// </summary>
            void Base5WithLessThan4()
            {
                int count = 0;
                for (int n = 1; n <= 2015; n++)
                {
                    string base5 = BaseConversion.ConvertFromBaseAToBaseB(n + "", 10, 5);
                    if (base5.IndexOf("4") < 0) {
                        count++;
                    } else {
                        QueuedConsole.WriteImmediate("n:{0}, in b : {1}", n, base5);
                    }
                }
                QueuedConsole.WriteImmediate("Numbers which can be written in the specified format : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/sum-of-reciprocals-of-products/
            /// </summary>
            void SumOfReciprocalsOfProducts()
            {
                BigRational sum = new BigRational(0, 1);
                for (int a = 1; a <= 2016; a++)
                {
                    int minb = a + 1;
                    if (2018 - a > a + 1) minb = 2018 - a;
                    BigRational sumab = new BigRational(0, 1);
                    for (int b = minb; b <= 2017; b++)
                    {
                        if(MathFunctions.GCD(a,b) == 1) sumab += new BigRational(1, b);
                    }
                    sumab /= a;

                    sum += sumab;
                }
                QueuedConsole.WriteImmediate("sum: {0}, a/b : {1}/{2}", NumberConverter.BigRationalToDouble(sum), sum.Numerator, sum.Denominator);
            }

            /// <summary>
            /// https://brilliant.org/problems/small-smaller-smallest/
            /// </summary>
            void FractionLessThan()
            {
                BigRational r = new BigRational(13, 15);
                BigRational smallest = new BigRational(14, 15);
                for(int a = 1; a < 500; a++)
                {
                    for (int b = 1; b < 500; b++)
                    {
                        BigRational d = new BigRational(a, b);

                        if (d > r && d < smallest)
                        {
                            smallest = d;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("a/b : {0}/{1}", smallest.Numerator, smallest.Denominator);
            }

            /// <summary>
            /// https://brilliant.org/problems/aime-i-problem-3/
            /// </summary>
            void RationalSumTo1000()
            {
                int count = 0;
                Enumerable.Range(1, 250).ForEach(n =>
                {
                    int num = (2 * n) - 1;
                    BigRational b = new BigRational(num, 1000 - num);
                    if (b.Numerator + b.Denominator == 1000) count++;
                });
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/finding-the-solutions/
            /// </summary>
            void FindingTheSolutionsIMOProblem()
            {
                int count = 0;
                for (int x = 1; x < 122; x++)
                {
                    for (int y = 1; y < 122; y++)
                    {
                        int num = (x * ((x * y) + 1)) + y;
                        int den = (y * ((x * y) + 1)) + 11;
                        if (num % den == 0)
                        {
                            QueuedConsole.WriteImmediate("x : {0}, y : {1}", x, y);
                            count++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("count : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/almost-automorphic-number/
            /// </summary>
            void AutoMorphicNumber()
            {
                int count = 0;
                Enumerable.Range(1, 999).ForEach(n =>
                {
                    BigInteger b = n;
                    BigInteger a = 1;
                    int nsq = n * n;
                    int mod = n < 10 ? 10 : n < 100 ? 100 : 1000;
                    int t = 1;
                    string nstr = n + "";
                    bool isBTrue = false;
                    bool isATrue = false;
                    while (1 > 0)
                    {
                        if (!isBTrue)
                        {
                            b *= nsq;
                            string s = b.ToString();
                            if (s.EndsWith(nstr))
                            {
                                isBTrue = true;
                            }
                        }
                        if (!isATrue)
                        {
                            a *= nsq;
                            string s = a.ToString();
                            if (s.EndsWith(nstr))
                            {
                                isATrue = true;
                            }
                        }
                        if (isATrue) break;
                        if (++t > 50) break;
                    }

                    if (isBTrue && !isATrue) count++;
                });
                QueuedConsole.WriteImmediate("Number of almost automorphic numbers : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-unique-10-digit-number/
            /// </summary>
            void AUniqueTenDigitNumber()
            {
                Action<int, List<int>> Add = null;
                Add = delegate (int depth, List<int> used)
                {
                    if(depth == 5) {
                        used.Add(5);
                        Add(depth + 1, used);
                    } else if(depth == 10) {
                        QueuedConsole.WriteImmediate("abc : {0}", string.Join("", used.GetRange(0, 3)));
                    }

                    for (int i = 1; i <= 9; i++)
                    {
                        if (i == 5) continue;
                        if (used.Contains(i)) continue;

                        List<int> next = new List<int>(used);
                        next.Add(i);

                        int num = int.Parse(string.Join("", next));
                        if (num % depth != 0) continue;
                        else Add(depth + 1, next);
                    }
                };
                Add(1, new List<int>());
            }

            /// <summary>
            /// https://brilliant.org/problems/is-it-divisible/
            /// </summary>
            void IsItDivisible()
            {
                int maxdepth = 3;
                int sum = 0;
                Action<int, List<int>> Add = null;
                Add = delegate (int depth, List<int> used)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        List<int> next = new List<int>(used);
                        next.Add(i);

                        if (depth == maxdepth - 1)
                        {
                            int num = int.Parse("739" + string.Join("", next));
                            if(num%7 == 0)
                            {
                                if (num % 8 == 0)
                                {
                                    if (num % 9 == 0)
                                    {
                                        sum += next.Sum();
                                    }
                                }
                            }
                        } else Add(depth + 1, next);
                    }
                };
                Add(0, new List<int>());
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/an-algebra-problem-by-ilham-saiful-fauzi-6/
            /// </summary>
            void EvenOddFunction()
            {
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 10000000);
                Dictionary<long, long> Known = new Dictionary<long, long>();

                Func<long, long> Fn = delegate (long n)
                {
                    if (Known.ContainsKey(n)) return Known[n];

                    bool isnOdd = n % 2 == 1;
                    long m = isnOdd ? (n + 1) / 2 : n / 2;

                    long retVal = 0;

                    if (isnOdd) retVal = (long)Math.Pow(2, m);
                    else retVal = m + (2 * m / (Factors.GetAllOddFactors(m, p).Max()));

                    Known.Add(n, retVal);

                    return retVal;
                };

                int k = 0;
                int fk1 = 2016;
                long fki = 1;
                while (1 > 0)
                {
                    k++;
                    fki = Fn(fki);
                    QueuedConsole.WriteImmediate("k : {0}, fki : {1}", k, fki);
                    if (k % 100 == 0)
                    {
                        
                    }
                    if (fki == fk1) break;
                }
                QueuedConsole.WriteImmediate("Number of composition k : {0}, fki : {1}", k, fki);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-number-theory-problem-by-aareyan-manzoor/
            /// </summary>
            void DirichletSumGCD2016()
            {
                BigRational sum = new BigRational(0, 1);
                long n = 0;
                while (++n <= 20000)
                {
                    sum += new BigRational(MathFunctions.GCD(2016, n), n * n);
                }
                QueuedConsole.WriteImmediate("a/b : {0}", NumberConverter.BigRationalToDouble(sum) / Math.Pow(Math.PI, 2));
            }

            /// <summary>
            /// https://brilliant.org/problems/open-lockers/
            /// </summary>
            void OpenLockers()
            {
                int N = 1000;
                bool[] Lockers = new bool[N];
                Enumerable.Range(0, N).ForEach(x => { Lockers[x] = false; });
                Enumerable.Range(1, N).ForEach(x =>
                {
                    Enumerable.Range(1, N).ForEach(y =>
                    {
                        if (y % x == 0) { Lockers[y - 1] = !Lockers[y - 1]; }
                    });
                });
                QueuedConsole.WriteImmediate("Open lockers : {0}", Lockers.Count(x => { return x == true; }));
            }

            /// <summary>
            /// https://brilliant.org/problems/3rd-and-last-700-followers-problem/
            /// </summary>
            void _700FollowersProblem()
            {
                int sum = 0;
                for (int A = 0; A <= 10000; A++)
                {
                    int n = 8382 - (3 * A);
                    if (n < 0) break;
                    BigRational b = new BigRational(n, A + 2);
                    if (MathFunctions.IsPerfectSquare((int)b.Numerator) && MathFunctions.IsPerfectSquare((int)b.Denominator))
                    {
                        sum += A;
                    }
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/second-7-hundred-followers-problem/
            /// </summary>
            void Second700FollowersProblem()
            {
                int sum = 0;
                for (int A = -1000; A <= 1000; A++)
                {
                    if (A == -4) continue;

                    BigRational b = new BigRational(3 * (16 - A), A + 4);
                    if (MathFunctions.IsPerfectSquare((int)b.Numerator) && MathFunctions.IsPerfectSquare((int)b.Denominator))
                    {
                        sum += A;
                    }
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-number-theory-problem-by-ilham-saiful-fauzi-4/
            /// </summary>
            void SumDivisors()
            {
                long n = -5000000, nmax = 5000000;
                long sum = 0;
                while (true)
                {
                    long num = (n * n * n) - 3;
                    long den = n - 3;
                    if (den != 0 && num % den == 0) sum += n;
                    if (++n > nmax) break;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/phat-2/
            /// </summary>
            void Phat2()
            {
                int k = 1;
                while (true)
                {
                    int n = 49 * k;
                    if (n % 17 == 16 && n % 16 == 15)
                    {
                        QueuedConsole.WriteImmediate("n: {0}", n);
                        break;
                    }
                    k++;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-sophie-germain/
            /// </summary>
            void SumOfSolutions()
            {
                int x = 0;
                int xMax = 100;
                BigInteger sum = 0;
                while (++x < xMax)
                {
                    long y = 27 + (long)Math.Pow(x, 6);
                    ClonedPrimes c = KnownPrimes.CloneKnownPrimes(1, long.MaxValue);
                    Dictionary<long, int> d = Factors.GetPrimeFactorsWithMultiplicity(y, c);
                    if (d.Count == 2)
                    {
                        if (d.All(kvp => { return kvp.Value == 1; }))
                        {
                            QueuedConsole.WriteImmediate("Possible x, y : {0},{1}", x, y);
                            sum += x + y;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/times-3-or-plus-1/
            /// </summary>
            void TimesThreeOrPlus1()
            {
                int maxSteps = 15;

                Dictionary<int, int> NumberSteps = new Dictionary<int, int>();
                Action<int, int> AddToDictionary = delegate (int n, int steps)
                {
                    if (NumberSteps.ContainsKey(n))
                    {
                        NumberSteps[n] = Math.Min(NumberSteps[n], steps);
                    }
                    else { NumberSteps.Add(n, steps); }
                };

                Action<List<int>, int> next = null;
                next = delegate (List<int> o, int steps)
                {
                    if (steps == maxSteps) return;

                    o.ForEach(x =>
                    {
                        int xp1 = x + 1;
                        AddToDictionary(xp1, steps + 1);
                        int xm3 = x * 3;
                        AddToDictionary(xm3, steps + 1);

                        next(new List<int>() { xp1, xm3 }, steps + 1);
                    });

                };

                next(new List<int>() { 1 }, 1);

                int leastN = int.MaxValue;
                NumberSteps.ForEach(x =>
                {
                    if (x.Value == maxSteps) { leastN = Math.Min(leastN, x.Key); }
                });
                QueuedConsole.WriteImmediate("Least number obtained with 15 steps : {0}", leastN);
            }

            /// <summary>
            /// https://brilliant.org/problems/double-square-summation/
            /// </summary>
            void DoubleSquareSummation()
            {
                BigRational sum = 0.0;
                int n = 0;
                while (n < 1000000)
                {
                    BigInteger n4 = n;
                    n4 *= n4 * n4 * n4;
                    sum += new BigRational(n, n4 + 4);
                    n++;
                }
                double sumDbl = NumberConverter.BigRationalToDouble(sum);
                QueuedConsole.WriteImmediate("Sum : {0}", sumDbl.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/numbers-having-12-factors/
            /// </summary>
            void NumbersHaving12Factors()
            {
                int n = 1;
                int sum = 0, fcount = 12;
                ClonedPrimes c = KnownPrimes.CloneKnownPrimes(0, 100);
                while (++n < 100)
                {
                    if (Factors.GetAllFactorsCount(n, c) == fcount) sum += n;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/equally-divisible/
            /// </summary>
            void EquallyDivisible()
            {
                int n = 3;
                int nmax = 0;
                while (++n <= 1000000)
                {
                    int tmax = (int)Math.Sqrt(n);
                    int t = 1;
                    bool isDivisible = true;
                    while (++t <= tmax)
                    {
                        if (!(n % t == 0)) { isDivisible = false; break; }
                    }
                    if (isDivisible) nmax = n;
                }
                QueuedConsole.WriteImmediate("Largest x : {0}", nmax);
            }

            /// <summary>
            /// https://brilliant.org/problems/2016-and-factorial/
            /// </summary>
            void FactorialAnd2016()
            {
                long t = 1;
                int n = 0;
                long sum = 0;
                while (++n <= 20)
                {
                    t *= n;
                    if (MathFunctions.IsSquare(t + 2016))
                    {
                        sum += (long)Math.Sqrt(t + 2016);
                    }
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/what-is-the-name-of-this-question/
            /// </summary>
            void FloorLogarithm()
            {
                int sum = 0;
                int n = 1;
                while (true)
                {
                    sum += (int)Math.Floor(Math.Log(n, 2));
                    if (sum == 1994) break;
                    n++;
                }
                QueuedConsole.WriteImmediate("n : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/practice/floor-and-ceiling-functions-level-5-challenges/?problem=birthday-special&subtopic=advanced-equations&chapter=floor-and-ceiling-functions
            /// https://brilliant.org/problems/birthday-special/
            /// </summary>
            void FloorAndCeilingFunction()
            {
                int n = 0;
                double sum = 0.0;
                while (++n <= 1995)
                {
                    double val = Math.Sqrt(Math.Sqrt(n));
                    double floor = Math.Floor(val);
                    if (val - floor >= 0.5) { sum += 1.0 / Math.Ceiling(val); }
                    else { sum += 1.0 / floor; }
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/floor-and-ceiling-functions-level-5-challenges/?problem=number-theory-question-2&subtopic=advanced-equations&chapter=floor-and-ceiling-functions
            /// https://brilliant.org/problems/number-theory-question-2/
            /// </summary>
            void FloorAndCeilingSummation1()
            {
                decimal sum = 0;
                decimal value = 1;
                decimal incrementalVal = 7;
                incrementalVal /= 10;
                while (value < 1000)
                {
                    decimal ceil = Math.Ceiling(value);
                    sum += ceil;
                    QueuedConsole.WriteImmediate("Value : {0}, Ceil value : {1}, Sum so far : {2}", value, ceil, sum);
                    value += incrementalVal;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/functional-equations-problem-solving/?problem=algebra-problem-57926&subtopic=advanced-equations&chapter=functional-equations
            /// https://brilliant.org/problems/algebra-problem-57926/
            /// </summary>
            void FunctionValueMultipleOf7()
            {
                BigInteger n1 = 1, n2 = 2;
                int n = 2;
                int multiplesCount = 0;
                while (++n <= 3581)
                {
                    BigInteger fn = n2 + (n1 * n1) + 3581;
                    if (fn % 7 == 0) multiplesCount++;
                    n1 = n2;
                    n2 = fn;
                }
                QueuedConsole.WriteImmediate("Multiples Count : {0}", multiplesCount);
            }

            /// <summary>
            /// https://brilliant.org/problems/11-and-13/
            /// </summary>
            void _11And13()
            {
                int leastmpn = int.MaxValue;
                int mVal = 0, nVal = 0;
                for (int m = 1; m <= 1000; m++)
                {
                    for (int n = 1; n <= 1000; n++)
                    {
                        int t1 = 13 * n + m;
                        if (t1 % 11 == 0)
                        {
                            int t2 = 11 * n + m;
                            if (t2 % 13 == 0)
                            {
                                if (m + n < leastmpn)
                                {
                                    leastmpn = m + n;
                                    mVal = m;
                                    nVal = n;
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Least m + n : {0}, m : {1}, n: {2}", leastmpn, mVal, nVal);
            }

            /// <summary>
            /// https://brilliant.org/problems/rationals-from-25/
            /// </summary>
            void RationalsFrom25()
            {
                List<int> Numbers = new List<int>() { 13, 17, 19, 23, 343, 121, 4194304, 59049, 15625 };
                BigInteger qMax = new BigInteger(1);
                Numbers.ForEach(x => { qMax *= x; });

                int count = 1;
                Enumerable.Range(1, 9).ForEach(x =>
                {
                    List<IList<int>> combinations = Combinations.GetAllCombinations(Numbers, x);

                    foreach (IList<int> c in combinations)
                    {
                        BigInteger qCopy = (new BigInteger(1)) * qMax;
                        BigInteger p = 1;
                        foreach (int t in c)
                        {
                            p *= t;
                            qCopy /= t;
                        }
                        if (p <= qCopy) count++;
                    }

                });

                QueuedConsole.WriteImmediate("Number of rational numbers : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/arithmetic-progression-with-primes/
            /// </summary>
            void ArithmeticProgressionWithPrimes()
            {
                List<long> primes = KnownPrimes.GetClonedPrimes(1, 430);
                HashSet<long> set = new HashSet<long>(primes);
                int SeqLength = 10;
                SimpleCounter loopcounter = new SimpleCounter();
                int MaxSequenceFound = int.MinValue;
                for (int diff = 1; diff <= 400; diff++)
                {
                    bool found = true;
                    for (int j = 0; j < primes.Count; j++)
                    {
                        found = true;
                        long sum = primes[j];
                        int curSeq = 1;
                        for (int i = 1; i < SeqLength; i++)
                        {
                            loopcounter.Increment();
                            long num = primes[j] + (diff * i);
                            if (set.Contains(num))
                            {
                                curSeq++;
                                sum += num;
                            }
                            else { found = false; break; }
                        }

                        MaxSequenceFound = Math.Max(MaxSequenceFound, curSeq);

                        if (found)
                        {
                            QueuedConsole.WriteImmediate("Sum : {0}", sum);
                            break;
                        }
                    }
                    if (found) break;
                }
                QueuedConsole.WriteImmediate("MaxSequenceFound : {0}", MaxSequenceFound);
                QueuedConsole.WriteImmediate("Loop Count : {0}", loopcounter.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/partial-fraction-maybeor-maybe-not/
            /// </summary>
            void PartialFraction()
            {
                int nMax = 100;
                BigRational tau = new BigRational(0, 1);
                Enumerable.Range(0, nMax).ForEach(x =>
                {
                    BigInteger numerator = PermutationsAndCombinations.Factorial(2 * x);
                    BigInteger denominator = PermutationsAndCombinations.Factorial(x);
                    denominator *= denominator;
                    BigInteger _6pow = new BigInteger(1);
                    Enumerable.Range(0, x).ForEach(p => { _6pow *= 6; });
                    denominator *= _6pow;

                    tau += new BigRational(numerator, denominator);
                });

                double value = NumberConverter.BigRationalToDouble(tau);
                QueuedConsole.WriteImmediate("Tau : {0}, Tau^6 : {1}", value, Math.Pow(value, 6));
            }

            /// <summary>
            /// https://brilliant.org/problems/an-algebra-problem-by-stanislava-sojakova/
            /// To the nearest percent, 16% of students got an A in their exam. What is the minimum number of students in the group?
            /// </summary>
            void MinimumNumberOfStudents()
            {
                int n = 1;
                double n1 = 31.0 / 200;
                double n2 = 32.0 / 200;
                while (++n <= 200)
                {
                    if (Math.Ceiling(n1 * n) == Math.Floor(n2 * n)) break;
                }
                QueuedConsole.WriteImmediate("minimum number of students : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/maaaaaaaagic/
            /// </summary>
            void SmallestPositiveIntegerForPerfectSquare()
            {
                int n = 0;
                while (++n > 0)
                {
                    int t = 3 + (2 * n * n) + (5 * n);
                    if (t % 3 == 0)
                    {
                        t /= 3;
                        if (MathFunctions.IsSquare(t)) break;
                    }
                }
                QueuedConsole.WriteImmediate("Smallest positive integer n : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/from-imo-1981-problem-3/
            /// </summary>
            void IMO1981Problem()
            {
                int maxVal = int.MinValue;
                for (int m = 1; m <= 1981; m++)
                {
                    for (int n = 1; n <= 1981; n++)
                    {
                        int val = (n * n) - (m * n) - (m * m);
                        if (val > 1) break;
                        if (Math.Abs(val) == 1)
                        {
                            maxVal = Math.Max((m * m) + (n * n), maxVal);
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Maximum value of m^2 + n^2 : {0}", maxVal);
            }

            /// <summary>
            /// https://brilliant.org/problems/finding-counterexample/
            /// </summary>
            void FindingCounterExample()
            {
                BigInteger ai = 1;
                BigInteger sum = ai;
                int n = 0;
                while (++n > 0)
                {
                    sum += ai * ai;
                    if (sum % n != 0) break;
                    else ai = sum / n;
                }
                QueuedConsole.WriteImmediate("Smallest integer : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/180-follower-problem/
            /// </summary>
            void _180Divisors()
            {
                ClonedPrimes c = KnownPrimes.CloneKnownPrimes(1, 1000000);
                int maxDivisors = int.MinValue;
                int n = 100, nmax = 1000000, maxDivisorsAt = int.MinValue;
                while (++n <= nmax)
                {
                    int dCount = Factors.GetAllFactorsCount(n, c);
                    if (dCount == 180)
                    {
                        QueuedConsole.WriteImmediate("{0} has 180 divisors.", n);
                    }
                    if (dCount > maxDivisors)
                    {
                        maxDivisors = dCount;
                        maxDivisorsAt = n;
                    }
                }

                QueuedConsole.WriteImmediate("Max divisors for n : {0} is {1} for nmax : {2}", maxDivisorsAt, maxDivisors, nmax);
            }

            /// <summary>
            /// https://brilliant.org/problems/think-2-2/
            /// </summary>
            void _2020()
            {
                int nSum = 0;
                int n = 3125, nZeroes = 2020;
                while (++n > 0)
                {
                    int k = 1;
                    int zeroCount = 0;
                    while (n / k > 1)
                    {
                        k *= 5;
                        zeroCount += n / k;
                    }
                    if (zeroCount == nZeroes) { nSum += n; }

                    if (zeroCount > nZeroes) break;
                }

                QueuedConsole.WriteImmediate("Sum of all values of n : {0}", nSum);
            }

            /// <summary>
            /// https://brilliant.org/problems/interchanging-digits/
            /// </summary>
            void InterchangingDigits()
            {
                int n = 999;
                int count = 0;
                while (++n <= 10000)
                {
                    if (n % 7 == 0)
                    {
                        string numStr = n + "";
                        string interchanged = numStr.Substring(3, 1) + numStr.Substring(1, 2) + numStr.Substring(0, 1);
                        int n1 = int.Parse(interchanged);
                        if (n1 % 7 == 0) { count++; }
                    }
                }
                QueuedConsole.WriteImmediate("Numbers : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/an-algebra-problem-by-dragan-markovic/
            /// </summary>
            void FractionNoMore()
            {
                long y = -100000;
                int count = 0;
                while (++y <= 100000)
                {
                    long n1 = -9 + (5 * y * y);
                    long d = (2 * y) + 6;
                    if (d != 0 && n1 % d == 0)
                    {
                        count++;
                    }
                }
                QueuedConsole.WriteImmediate("Integer count: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/rmo-400-points/
            /// </summary>
            void RMO400Points()
            {
                long sumXyz = 0;
                for (int z = 1000; z > 0; z--)
                {
                    int z3 = z * z * z;
                    for (int y = z; y > 0; y--)
                    {
                        long y3z3 = (y * y * y) + z3;
                        long yz = y * z;
                        for (int x = y; x > 0; x--)
                        {
                            long lhs = y3z3 * x * x * x;
                            long rhs = 2012 * (x * yz + 2);

                            if (lhs.Equals(rhs)) sumXyz += x + y + z;
                        }
                    }
                }

                QueuedConsole.WriteImmediate("sum : {0}", sumXyz);
            }

            /// <summary>
            /// https://brilliant.org/problems/how-tough-are-you/
            /// </summary>
            void HowToughAreYou()
            {
                int rSum = 0;
                List<long> primes = KnownPrimes.GetPrimes(500, 50000);
                HashSet<int> SetR = new HashSet<int>();
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 50000);
                foreach (long prime in primes)
                {
                    for (int r = 1; r <= 500; r++)
                    {
                        long pr = prime * r;
                        long rhs = 2 * pr + 992;

                        if (Factors.GetAllFactorsSum(pr, p) == rhs)
                        {
                            SetR.Add(r);
                            rSum += r;
                        }
                    }
                }

                QueuedConsole.WriteImmediate("Sum of SetR : {0}", SetR.Sum());
                QueuedConsole.WriteImmediate("Sum of all r : {0}", rSum);
            }

            /// <summary>
            /// https://brilliant.org/problems/factorial-factors-2/
            /// </summary>
            void FactorialFactors()
            {
                BigInteger _20f = PermutationsAndCombinations.Factorial(20);
                BigInteger _21f = PermutationsAndCombinations.Factorial(21);
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 20);
                int diff = Factors.GetAllFactorsCount(_21f, p) - Factors.GetAllFactorsCount(_20f, p);
                QueuedConsole.WriteImmediate("Answer: {0}", diff);
            }

            /// <summary>
            /// https://brilliant.org/problems/isnt-100000-such-a-huge-number-part-2/
            /// </summary>
            void ReciprocalSumOrderedPairs1()
            {
                long n = 100000, a = n;
                int count = 0;
                while (++a <= 2 * n)
                {
                    if (n * a % (a - n) == 0) count++;
                }
                QueuedConsole.WriteImmediate("Number of ordered pairs : {0}", 2 * count - 1);
            }

            /// <summary>
            /// https://brilliant.org/problems/imo-training-problem/
            /// </summary>
            void LargestPrimeFactor()
            {
                int n = 0;
                int S = 0;
                while (++n < 1000)
                {
                    int tp = 1;
                    string nstr = "" + n;
                    nstr.ForEach(x =>
                    {
                        if (!x.Equals('0')) { tp *= (int)char.GetNumericValue(x); }
                    });
                    S += tp;
                }

                List<long> factors = Factors.GetPrimeFactors(S);
                factors.Sort();
                QueuedConsole.WriteImmediate("S : {0}", S);
                QueuedConsole.WriteImmediate("Largest prime factor of S : {0}", factors.Last());
            }

            /// <summary>
            /// https://brilliant.org/problems/finding-a-product/
            /// </summary>
            void FindingProduct()
            {
                bool found = false;
                for (int a = 2; a <= 100; a++)
                {
                    for (int b = a + 1; b <= 100; b++)
                    {
                        for (int c = b + 1; c <= 100; c++)
                        {
                            int abc = a * b * c;
                            int num = (a * b - 1) * (c * b - 1) * (a * c - 1);

                            if (num % abc == 0)
                            {
                                QueuedConsole.WriteImmediate("abc: {0}", abc);
                                found = true;
                            }
                            if (found) break;
                        }
                        if (found) break;
                    }
                    if (found) break;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/an-interesting-problem-120/
            /// </summary>
            void AnInterestingDiophantine()
            {
                int m = 11;
                while (true)
                {
                    m += 2;

                    int n = (3 * m * m) + 2;

                    int lhs = (n - 1) / 2;
                    int sqrt = (int)Math.Sqrt(lhs);

                    int t = 13;
                    bool found = false;
                    while (++t <= sqrt + 10)
                    {
                        if (t * (t + 1) == lhs) { found = true; break; }
                    }
                    if (found)
                    {
                        QueuedConsole.WriteImmediate("n: {0}", n);
                        break;
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/a-perfect-square-no-way/
            /// </summary>
            void APerfectSquareNoWay()
            {
                long y = 1;
                int counter = 0;
                while (++y <= 1000000)
                {
                    long r = (2 * y * y) + 1;
                    if (MathFunctions.IsPerfectSquare(r))
                    {
                        int n = ((int)Math.Sqrt(r) - 3) / 2;
                        counter++;
                        QueuedConsole.WriteImmediate("n: {0}", n);
                    }
                }
                QueuedConsole.WriteImmediate("total n: {0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-solve-it-4/
            /// </summary>
            void CanYouSolveIt()
            {
                int n = 0;
                BigInteger b = 1;
                while (++n <= 4444)
                {
                    b *= 4444;
                }

                string bStr = b.ToString();
                int ds = 0, ds1 = 0;
                bStr.ForEach(x =>
                {
                    ds += (int)char.GetNumericValue(x);
                });
                string dStr = "" + ds;
                dStr.ForEach(x =>
                {
                    ds1 += (int)char.GetNumericValue(x);
                });

                QueuedConsole.WriteImmediate("A: {0}, B : {1}", ds, ds1);
            }

            /// <summary>
            /// https://brilliant.org/problems/its-like-youre-my-mirror-my-mirror-staring-back-at/
            /// </summary>
            void MirrorMirror()
            {
                long n = 99999;
                long sum = 0;
                while (++n < 1000000)
                {
                    long nsq = n * n;
                    if (("" + nsq).EndsWith("" + n)) sum += n;
                }
                QueuedConsole.WriteImmediate("Answer: {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/fractional-numbers/
            /// </summary>
            void FractionalNumbers()
            {
                int n = 1, d = 2003;
                int count = 1;
                ClonedPrimes primes = KnownPrimes.CloneKnownPrimes(0, 1000);
                while (true)
                {
                    n++;
                    d--;
                    if (n >= d) break;

                    List<long> nFac = Factors.GetPrimeFactors(n, primes);
                    HashSet<long> dFac = new HashSet<long>(Factors.GetPrimeFactors(d, primes));

                    bool isirreducible = true;
                    foreach (long np in nFac)
                    {
                        if (dFac.Contains(np)) { isirreducible = false; break; }
                    }
                    if (isirreducible) count++;
                }
                QueuedConsole.WriteImmediate("Number of fractions : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/problem-of-the-day-a/
            /// </summary>
            void ProblemOfTheDayA()
            {
                long c = 3;
                int count = 0;
                while (++c <= 100000000)
                {
                    long num = (c + 3) * (c - 3);
                    if (num % 2 == 0 || num % 3 == 0)
                    {
                        while (true)
                        {
                            if (num % 2 == 0) num /= 2;
                            else break;
                        }
                        while (true)
                        {
                            if (num % 3 == 0) num /= 3;
                            else break;
                        }

                        if (num == 1) count++;
                    }
                }

                QueuedConsole.WriteImmediate("Number of solutions: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/must-have-a-lot-of-factors/
            /// </summary>
            void LotsOfFactors()
            {
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 1000);
                int n = 1, nCool = 0, nCoolNotPrimes = 0;
                while (++n < 1000)
                {
                    HashSet<long> factors = Factors.GetAllFactors(n, p);
                    List<IList<long>> combinations = Combinations.GetAllCombinations(factors.ToList(), 2);
                    bool isCool = true;
                    foreach (IList<long> c in combinations)
                    {
                        if (c.Count >= 2)
                        {
                            if (MathFunctions.GCD(c[0], c[1]) == 1)
                            {
                                if (n % (c[0] + c[1] - 1) != 0)
                                {
                                    isCool = false; break;
                                }
                            }
                        }
                    }
                    if (isCool)
                    {
                        nCool++;
                        if (!p.Contains(n)) nCoolNotPrimes++;
                    }
                }
                QueuedConsole.WriteImmediate("Number of cool numbers : {0}", nCool);
                QueuedConsole.WriteImmediate("Number of cool numbers which are not primes : {0}", nCoolNotPrimes);
            }

            /// <summary>
            /// https://brilliant.org/problems/odd-man-out-2/
            /// </summary>
            void OddManOut()
            {
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 10000);
                int n = 1;
                List<long> firstTwo = new List<long>();
                Dictionary<string, long> Derivatives = new Dictionary<string, long>();

                Func<int, long, long> NthArithmeticDerivative = null;
                NthArithmeticDerivative = delegate (int N, long num)
                {
                    string key = num + "-" + N;
                    if (Derivatives.ContainsKey(key)) return Derivatives[key];
                    if (p.Contains(num))
                    {
                        if (N == 1) return 1;
                        if (N > 1) return 0;
                    }

                    long a = Factors.GetLeastPrimeFactor(num, p);
                    long b = num / a;

                    long derivative = b + a * NthArithmeticDerivative(1, b);

                    if (N == 1)
                    {
                        Derivatives.AddOnce(key, derivative);
                        return derivative;
                    }
                    else
                    {
                        long retVal = NthArithmeticDerivative(N - 1, derivative);
                        Derivatives.AddOnce(key, retVal);
                        return retVal;
                    }
                };

                while (n <= 10000)
                {
                    n += 2;

                    if (NthArithmeticDerivative(n, n) != 0)
                    {
                        firstTwo.Add(n);
                        if (firstTwo.Count == 2) break;
                    }

                }
                QueuedConsole.WriteImmediate("Required sum : {0}", firstTwo.Sum());
            }

            /// <summary>
            /// https://brilliant.org/problems/conceptual-equation/
            /// </summary>
            void IntegralSolution1()
            {
                int nSolutions = 0;
                for (int x = -10; x <= 10; x++)
                {
                    for (int y = -10; y <= 10; y++)
                    {
                        if (((x * x * (y - 1)) + (y * y * (x - 1))) == 1)
                        {
                            nSolutions++;
                            QueuedConsole.WriteImmediate("x: {0}, y: {1}", x, y);
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of solutions : {0}", nSolutions);
            }

            /// <summary>
            /// https://brilliant.org/problems/zi-songs-bound/
            /// </summary>
            void BoundFunctionValue()
            {
                double maxVal = double.MinValue, minVal = double.MaxValue;
                int n = 1;
                while (++n <= 100000)
                {
                    double s = MathFunctions.DigitSum(n);
                    s /= MathFunctions.DigitSum(5 * n);

                    minVal = Math.Min(s, minVal);
                    maxVal = Math.Max(s, maxVal);
                }

                double sum = minVal + maxVal;
                Fraction<int> fr = FractionConverter<int>.RealToFraction(sum);
                QueuedConsole.WriteImmediate("a+b: {0}", fr.N + fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/solving-a-diophantine-fraction/
            /// </summary>
            void SolvingDiophantineFraction()
            {
                int N = 0;
                while (++N < 1000)
                {
                    int num = (N * N) - 75;
                    int d = 5 * N + 56;

                    if (num % d == 0)
                    {
                        QueuedConsole.WriteImmediate("N : {0}", N);
                        break;
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/fibonacci-6/
            /// </summary>
            void FibonacciF100()
            {
                Dictionary<int, BigInteger> FN = new Dictionary<int, BigInteger>();
                FN.Add(0, 0);
                FN.Add(1, 1);

                FibonacciGenerator fg = new FibonacciGenerator(0, 1);
                int N = fg.CurrentN();
                while (++N <= 100)
                {
                    FN.Add(N, fg.Next());
                }

                int count = 0;
                foreach (KeyValuePair<int, BigInteger> kvp in FN)
                {
                    if (kvp.Key > 0 && kvp.Key < 100)
                    {
                        if (FN[100] % kvp.Value == 0) count++;
                    }
                }
                QueuedConsole.WriteImmediate("Number of fibonacci divisors : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/dereks-prime-tuple/
            /// </summary>
            void DerekPrimeTuple()
            {
                HashSet<long> primes = new HashSet<long>(KnownPrimes.GetPrimes(1, 50));
                int count = 0, N = 2223;
                for (int x = 1; x <= 50; x++)
                {
                    if (!primes.Contains(x)) continue;
                    int x2 = x * x;
                    for (int y = x + 1; y <= 50; y++)
                    {
                        int y2 = y * y;
                        if (!primes.Contains(y)) continue;
                        for (int z = y + 1; z <= 50; z++)
                        {
                            int z2 = z * z;
                            if (!primes.Contains(z)) continue;
                            for (int a = z + 1; a <= 50; a++)
                            {
                                int a2 = a * a;
                                if (!primes.Contains(a)) continue;

                                if (x2 + y2 + z2 + a2 == N) count++;
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("distinct 4 tuples: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/minimum-totient-quotient/
            /// </summary>
            void MinimumTotientQuotient()
            {
                double value = double.MaxValue;
                int nleast = 0;
                int n = 0;
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 1000);
                while (++n <= 1000)
                {
                    double phinbyn = EulerTotient.CalculateTotient(n, p);
                    phinbyn /= n;

                    if(phinbyn< value ) { value = phinbyn; nleast = n; }
                }
                QueuedConsole.WriteImmediate("n: {0}", nleast);
            }

            /// <summary>
            /// https://brilliant.org/problems/theyre-not-all-the-same-degree/
            /// </summary>
            void TheyAreNotAllTheSame()
            {
                int sum = 0;
                for (int x = 1; x <= 20; x++)
                {
                    for (int y = x; y <= 20; y++)
                    {
                        int lhs = (int)Math.Pow(x, 3) + (int)Math.Pow(y, 3) + (3 * x * y);
                        if (lhs % 3 == 0)
                        {
                            lhs /= 3;
                            if (lhs == 2007)
                            {
                                sum += x * y;
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Sum: {0}", sum);
            }

            void _4000FollowersProblem()
            {
                Dictionary<int, BigInteger> Factorials = new Dictionary<int, BigInteger>();
                Enumerable.Range(0, 201).ForEach(x =>
                {
                    Factorials.Add(x, PermutationsAndCombinations.Factorial(x));
                });

                int n = 0, sum = 0;
                for (int a = 0; a <= 200; a++)
                {
                    BigInteger ai = Factorials[a] - 1;
                    for (int b = 0; b <= 200; b++)
                    {
                        BigInteger bi = Factorials[b] - 1;
                        for (int c = 0; c <= 200; c++)
                        {
                            BigInteger ci = Factorials[c] + 1;

                            if(ai*bi == ci)
                            {
                                n++;
                                sum += a + b + c;
                            }
                        }
                    }
                }

                QueuedConsole.WriteImmediate("n: {0}, sum: {1}, n + sum : {2}", n, sum, n + sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-make-it-the-best/
            /// </summary>
            void CanYouMakeItTheBest()
            {
                int maxVal = 0;
                for(int a = 1; a <= 100; a++)
                {
                    for (int b = a; b <= 100; b++)
                    {
                        BigRational lhs = new BigRational(a + b, a * b);

                        int n = 0;
                        while (++n > 0)
                        {
                            BigRational rhs = new BigRational(BigInteger.Pow(2, n), BigInteger.Pow(3, n));
                            if (rhs <= lhs)
                            {
                                if (lhs == rhs)
                                {
                                    maxVal = Math.Max(maxVal, a + b + n);
                                }
                                break;
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Maxval of x + y + z: {0}", maxVal);
            }

            /// <summary>
            /// https://brilliant.org/problems/around-farey-sequences/
            /// </summary>
            void FareySequences()
            {
                HashSet<string> set = new HashSet<string>();
                int n = 1;
                while (1 > 0)
                {
                    int p = 0;
                    while (p <= n)
                    {
                        BigRational r = new BigRational(p, n);
                        string key = r.Numerator + ":" + r.Denominator;
                        set.Add(key);

                        p++;
                    }
                    if(set.Count >= 500) { n--; break; }
                    n++;
                }
                QueuedConsole.WriteImmediate("n: {0}", n);
            }
        }

        internal class Remainder
        {
            internal void Solve()
            {
                Remainder2555759832();
            }

            /// <summary>
            /// https://brilliant.org/problems/finding-the-last-two-digit/
            /// </summary>
            void Remainder2555759832()
            {
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder :: {0}", RemainderCalculator.RemainderWithNestedPowers(new long[] { 2555759, 832 }, 100)));
            }

            /// <summary>
            /// https://brilliant.org/problems/last-3-digits-3/
            /// </summary>
            void Remainder123456()
            {
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder :: {0}", RemainderCalculator.RemainderWithNestedPowers(new long[] { 123, 456 }, 1000)));
            }

            /// <summary>
            /// https://brilliant.org/problems/1234567890-2/
            /// </summary>
            void Remainder1234567890()
            {
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder :: {0}", RemainderCalculator.RemainderWithNestedPowers(new long[] { 12, 34, 56, 78 }, 90)));
            }

            /// <summary>
            /// https://brilliant.org/problems/serieous-sum/
            /// </summary>
            void SerieousSum()
            {
                QueuedConsole.WriteFinalAnswer(string.Format("Last three digits is :: {0}", RemainderCalculator.RemainderWithNestedPowers(new int[] { 2, 3, 4, 5, 6 }, 1000)));
            }

            void RecursiveRemainderCalculator()
            {
                int n = 1729;
                List<int> nArr = new List<int>();
                while (true)
                {
                    nArr.Add(n);
                    n -= 2;
                    if (n < 0) break;
                }

                long mod = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 100);
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod));
            }

            void RecursiveRemainderCalculator1()
            {
                int[] nArr = new int[] { 2, 99 };

                long mod = RemainderCalculator.RemainderWithNestedPowers(nArr, 99);
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod));
            }

            void ThePowerOfAllPowers()
            {
                Dictionary<long, long> n5d7 = RemainderCalculator.GetCycles(5, 7);
                Dictionary<long, long> n3d5 = RemainderCalculator.GetCycles(3, 5);
                Dictionary<long, long> n2d3 = RemainderCalculator.GetCycles(2, 3);
                Dictionary<long, long> n7d11 = RemainderCalculator.GetCycles(7, 11);

                foreach (KeyValuePair<long, long> kvp in n5d7)
                {
                    QueuedConsole.WriteImmediate(string.Format("{0} ^ {1} = {2} (mod {3})", 5, kvp.Key, kvp.Value, 7));
                }

                QueuedConsole.WriteImmediate("5^x (mod 7) . The cycle length is : " + n5d7.Count);

                foreach (KeyValuePair<long, long> kvp in n3d5)
                {
                    QueuedConsole.WriteImmediate(string.Format("{0} ^ {1} = {2} (mod {3})", 3, kvp.Key, kvp.Value, 5));
                }

                QueuedConsole.WriteImmediate("3^x (mod 5) . The cycle length is : " + n3d5.Count);

                foreach (KeyValuePair<long, long> kvp in n2d3)
                {
                    QueuedConsole.WriteImmediate(string.Format("{0} ^ {1} = {2} (mod 7)", 2, kvp.Key, kvp.Value, 3));
                }

                QueuedConsole.WriteImmediate("2^x (mod 3) . The cycle length is : " + n2d3.Count);

                foreach (KeyValuePair<long, long> kvp in n7d11)
                {
                    QueuedConsole.WriteImmediate(string.Format("{0} ^ {1} = {2} (mod {3})", 7, kvp.Key, kvp.Value, 11));
                }

                QueuedConsole.WriteImmediate("7^x (mod 11) . The cycle length is : " + n7d11.Count);

                Dictionary<int, int> axbcd = new Dictionary<int, int>();
                axbcd.Add(1, 30); axbcd.Add(2, 8); axbcd.Add(3, 24); axbcd.Add(4, 8); axbcd.Add(5, 6); axbcd.Add(6, 6);
                int k1 = 1;
                int axbxcxd = 0;
                while (true)
                {
                    int calVal = ((6 * k1 + 1) * axbcd[1]);
                    if ((calVal - 1) % 10 == 0)
                    {
                        axbxcxd = calVal;
                        break;
                    }
                    k1++;
                    if (k1 == 10) break;
                }

                QueuedConsole.WriteImmediate("Least possible value of abcd is : " + axbxcxd);
                QueuedConsole.ReadKey();
            }

            void RecursiveRemainderCalculator2()
            {
                int[] nArr = new int[] { 6, 6, 6, 6, 6, 6, 6 };

                long mod = RemainderCalculator.RemainderWithNestedPowers(nArr, 7);
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod));
            }

            void Mod271Of124_1s()
            {
                string numStr = string.Empty;
                while (true)
                {
                    numStr += "1";
                    if (numStr.Length == 124) break;
                }

                BigInteger num = BigInteger.Parse(numStr);

                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", (num % 271).ToString()));
            }

            void _2pow1990div1990()
            {
                Dictionary<long, long> cycles = RemainderCalculator.GetCycles(2, 1990);
                int cycleLength = cycles.Count;
                int mod = 1990 % cycleLength;
                QueuedConsole.WriteFinalAnswer("Remainder is : {0}", cycles[mod]);
            }

            void _5603pow1243mod100()
            {
                Dictionary<long, long> cycles = RemainderCalculator.GetCycles(5603, 100);
                int cycleLength = cycles.Count;
                int mod = 1243 % cycleLength;
                QueuedConsole.WriteFinalAnswer("Remainder is : {0}", cycles[mod]);
            }

            void DivisibilityBy7()
            {
                Dictionary<long, long> cyclesOf2 = RemainderCalculator.GetCycles(2, 7);
                Dictionary<long, long> cyclesOf4 = RemainderCalculator.GetCycles(4, 7);

                int n = 1;
                int c = 0;
                while (true)
                {
                    long rem = 1 + cyclesOf2[n % cyclesOf2.Count != 0 ? n % cyclesOf2.Count : cyclesOf2.Count] + cyclesOf4[n % cyclesOf4.Count != 0 ? n % cyclesOf4.Count : cyclesOf4.Count];
                    if (rem % 7 == 0) c++;
                    n++;
                    if (n == 1000) break;
                }
                QueuedConsole.WriteImmediate("Number of n, for which expression is divisible by 7 : {0}", c);
            }

            /// <summary>
            /// https://brilliant.org/problems/in-the-end-remainder-matters/
            /// </summary>
            void InTheEndRemainderMatters()
            {
                string s = string.Empty;
                int n = 0;
                BigInteger sum = 0;
                while (++n < 49)
                {
                    s += "2";
                    BigInteger nThTerm = BigInteger.Parse(s);
                    nThTerm *= nThTerm;
                    sum += nThTerm;
                }
                QueuedConsole.WriteImmediate("Remainder : {0}", sum % 9);
            }

            /// <summary>
            /// https://brilliant.org/problems/exponential-ap/
            /// </summary>
            void ExponentialAP()
            {
                int n = 9;
                List<int> nArr = new List<int>();
                while (true)
                {
                    nArr.Add(n);
                    n--;
                    if (n <= 1) break;
                }
                //nArr.Reverse();
                long mod = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 100000);
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod));
            }

            /// <summary>
            /// https://brilliant.org/problems/now-we-are-evolving/
            /// </summary>
            void NowWeAreEvolving()
            {
                List<int> nArr = new List<int>() { 97, 93, 91, 89, 87, 83, 81 };
                long mod = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 100000);
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod));
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-ayush-rai/
            /// </summary>
            void FindRemainder95pow199996pow1999()
            {
                List<int> nArr = new List<int>() { 95, 1999 };
                long mod1 = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 1000);

                nArr = new List<int>() { 96, 1999 };
                long mod2 = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 1000);

                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", (mod1 + mod2) % 1000));
            }

            /// <summary>
            /// https://brilliant.org/problems/tower-of-31-part-4-tedious-one/
            /// </summary>
            void TowerOf31Part4()
            {
                List<int> nArr = new List<int>() { 31, 123, 1234, 12345 };
                long mod1 = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 1000);

                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}, sum of last three digits : {1}", mod1, MathFunctions.DigitSum(mod1)));
            }

            /// <summary>
            /// https://brilliant.org/problems/tower-of-31-part-5/
            /// </summary>
            void TowerOf31Part5()
            {
                long sum = 0;
                int n = 0;
                while (++n <= 5)
                {
                    List<int> nArr = new List<int>() { 31, 31 * n };
                    sum += RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 1000);
                }

                QueuedConsole.WriteFinalAnswer(string.Format("Last three digits of sum is :: {0}", sum % 1000));
            }

            /// <summary>
            /// https://brilliant.org/problems/tower-of-31-part-3/
            /// </summary>
            void TowerOf31Part3()
            {
                List<int> nArr = new List<int>() { 1111, 31, 1111 };
                long mod1 = RemainderCalculator.RemainderWithNestedPowers(nArr.ToArray(), 1000);

                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod1));
            }

            /// <summary>
            /// https://brilliant.org/problems/finding-the-remainder-2/
            /// </summary>
            void Remainder2pow1990()
            {
                int[] arr = new int[2] { 2, 1990 };
                long mod1 = RemainderCalculator.RemainderWithNestedPowers(arr, 1990);

                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod1));
            }

            /// <summary>
            /// https://brilliant.org/problems/sanchayapols-number-theory-remixed/
            /// </summary>
            void _1007pow2013mod2014()
            {
                BigInteger b = 1;
                int n = 0;
                while (++n <= 2013)
                {
                    b *= 1007;
                    b %= 2014;
                }

                QueuedConsole.WriteImmediate("Remainder : {0}", b % 2014);
            }

            /// <summary>
            /// https://brilliant.org/problems/damnnnnremainder/
            /// </summary>
            void DamnRemainder()
            {
                string n = "333";
                while (n.Length <= 2000)
                {
                    n += "3";

                    BigInteger b = BigInteger.Parse(n);
                    if (b%383 == 0)
                    {
                        b /= 383;
                        QueuedConsole.WriteImmediate("Remainder: {0}", b % 1000);
                        break;
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/digits-modulo-100/
            /// </summary>
            void DigitsModulo100()
            {
                long mod1 = RemainderCalculator.RemainderWithNestedPowers(new int[2] { 79, 79 }, 100);

                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod1));
            }
        }

        internal class ModularArithmetic
        {
            internal void Solve()
            {
                WhereIsThePattern();
            }

            /// <summary>
            /// https://brilliant.org/problems/where-is-the-pattern/
            /// </summary>
            void WhereIsThePattern()
            {
                int p = 1;
                Enumerable.Range(1, 719).ForEach(n =>
                {
                    if (MathFunctions.GCD(n, 720) == 1)
                    {
                        p *= n * n;
                        p %= 720;
                    }
                });

                p %= 720;
                QueuedConsole.WriteImmediate("Remainder: {0}", p);
            }

            /// <summary>
            /// https://brilliant.org/problems/long-modulus/
            /// </summary>
            void LongModulus()
            {
                int n = 1;
                int mod = 1;
                while (true)
                {
                    int counter = n;
                    while (true)
                    {
                        mod *= n;
                        mod %= 101;
                        counter--;
                        if (counter == 0) break;
                    }
                    n++;
                    if (n > 100) break;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is : {0}", mod));
            }

            /// <summary>
            /// https://brilliant.org/problems/ukmt-intermediate-challenge-iv/
            /// </summary>
            void LargestRemainderOf2015()
            {
                int rMax = 0;
                int n = 0;
                while (true)
                {
                    n++;
                    if (n == 1001) break;
                    rMax = Math.Max(2015 % n, rMax);
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Largest Remainder is : {0}", rMax));
            }

            /// <summary>
            /// https://brilliant.org/problems/my-calculator-isnt-good-enough/
            /// </summary>
            void RemainderWhenDividedBy11_1()
            {
                int rem = 0;
                int n = 0;
                while (true)
                {
                    n++;
                    if (n == 2016) break;
                    int rem1 = 1;
                    int count = 0;
                    while (true)
                    {
                        rem1 *= 10;
                        rem1 %= 11;
                        count++;
                        if (count == n) break;
                    }
                    rem1 *= n;
                    rem1 %= 11;

                    rem += rem1;
                    rem %= 11;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is : {0}", rem));
            }

            /// <summary>
            /// https://brilliant.org/problems/remainder-of-a-sum-modulo-211/
            /// </summary>
            void RemainderOfASumModulo211()
            {
                Func<long, BigInteger> fx = delegate (long n)
                {
                    BigInteger fn = n + 2 + (n * n * n);

                    BigInteger retVal = 1;
                    int t = 0;
                    while (++t <= 71) retVal *= fn;

                    return retVal;
                };

                int N = 211;
                int remainder = 0;
                Enumerable.Range(0, N).ForEach(x =>
                {
                    remainder += (int)(fx(x) % N);
                    remainder %= N;
                });
                QueuedConsole.WriteImmediate("Remainder : {0}", remainder);
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-find-it/
            /// </summary>
            void CanYouFindIt()
            {
                Dictionary<long, long> c = RemainderCalculator.GetCycles(13, 2013);
                QueuedConsole.WriteImmediate("Least n :{0}", c.Where(x => x.Value == 1).Select(x => x.Key).First());
            }
        }

        //Keep this lowest in view.
        string IProblemName.GetName()
        {
            return "Brilliant.NumberTheory.Collection";
        }
    }
}