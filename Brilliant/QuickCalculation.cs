using System;
using System.Collections.Generic;
using System.Linq;

using GDF = GenericDefs.Functions;
using GenericDefs.DotNet;
using GenericDefs.Classes;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Numerics;
using Numerics;
using GenericDefs.Classes.NumberTypes;

namespace Brilliant
{
    public class QuickCalculation : ISolve, IRunSaved, ISaveProgress
    {
        bool ISaveProgress.CanSave
        {
            get; set;
        }

        bool IRunSaved.IsResume
        {
            get; set;
        }

        void ISolve.Init()
        {
            ((IRunSaved)this).IsResume = false;
            ((ISaveProgress)this).CanSave = true;
        }

        void IRunSaved.Resume()
        {

        }

        Task<bool> ISaveProgress.Save()
        {
            return Task.FromResult(true);
        }

        void ISolve.Solve()
        {
            (new ProblemCollection1()).Solve();
        }

        internal class ProblemCollection1
        {
            internal void Solve()
            {
                Remainder1();
            }

            void Remainder1()
            {
                BigInteger b = BigInteger.Pow(2, 2014);
                b -= 1;
                b /= 3;
                b += 2;
                QueuedConsole.WriteImmediate("Modulus : {0}", b % 100);
            }

            /// <summary>
            /// https://brilliant.org/problems/prime-sum-2-2/
            /// </summary>
            void PrimeSum()
            {
                int n = 0, nsqMax = 1000 * 1000;
                int sum = 0;
                GDF.ClonedPrimes p = GDF.KnownPrimes.CloneKnownPrimes(0, nsqMax);
                while (1 > 0)
                {
                    n++;

                    int nsq = n * n;
                    if (nsq >= nsqMax) break;
                    int num = nsq + 1;
                    if (GDF.Prime.IsPrime(num, p))
                    {
                        sum += num;
                    }
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            void SumPerfectNumbers()
            {
                GDF.ClonedPrimes p = GDF.KnownPrimes.CloneKnownPrimes(0, 10000);
                int sum = 0;
                Enumerable.Range(1, 9999).ForEach(n =>
                {
                    if (GDF.Factors.GetAllFactorsSum(n, p) == 2 * n) {
                        QueuedConsole.WriteImmediate("{0} is perfect number", n);
                        sum += n;
                    }
                });
                QueuedConsole.WriteImmediate("Sum of perfect numbers : {0}", sum);
            }

            void DivisibleBy120()
            {
                int n = 0, d = 2;
                int count = 0;
                while (n <= 14520)
                {
                    n += d;
                    if (n % 120 == 0) count++;
                    d += 2;
                }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            void NumberStartsWith()
            {
                BigInteger _2pow = 1, _5pow = 1;
                int count = 0;
                Enumerable.Range(1, 200).ForEach(x =>
                {
                    _2pow *= 2;
                    _5pow *= 5;

                    if (_2pow.ToString()[0] == _5pow.ToString()[0]) count++;
                });
                QueuedConsole.WriteImmediate("{0}", count);
            }

            void _50Termsums()
            {
                BigRational s = new BigRational(0, 1);
                BigRational t = new BigRational(0, 1);

                Enumerable.Range(1, 50).ForEach(n =>
                {
                    s += new BigRational(1, 2 * n * ((2 * n) - 1));
                    t += new BigRational(1, (50 + n) * (101 - n));
                });

                double S = NumberConverter.BigRationalToDouble(s);
                double T = NumberConverter.BigRationalToDouble(t);

                QueuedConsole.WriteImmediate("S/T : {0}", S / T);
            }

            void AllCompositionof15()
            {
                UniqueIntegralPairs p = new UniqueIntegralPairs(".");
                int[] xi = new int[7];
                for (xi[0] = 0; xi[0] <= 15; ++xi[0])
                {
                    if (xi[0] == 2) continue;
                    for (xi[1] = 0; xi[1] <= 15; ++xi[1])
                    {
                        if (xi[1] == 2) continue;
                        for (xi[2] = 0; xi[2] <= 15; ++xi[2])
                        {
                            if (xi[2] == 2) continue;
                            for (xi[3] = 0; xi[3] <= 15; ++xi[3])
                            {
                                if (xi[3] == 2) continue;
                                for (xi[4] = 0; xi[4] <= 15; ++xi[4])
                                {
                                    if (xi[4] == 2) continue;
                                    for (xi[5] = 0; xi[5] <= 15; ++xi[5])
                                    {
                                        if (xi[5] == 2) continue;
                                        for (xi[6] = 0; xi[6] <= 15; ++xi[6])
                                        {
                                            if (xi[6] == 2) continue;
                                            if (xi.Sum(x => x) == 15)
                                            {
                                                ArrayList l = new ArrayList();
                                                foreach (int x in xi)
                                                {
                                                    if (x != 2) l.Add(x);
                                                }

                                                p.AddCombination(l);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Total number of compositions :: {0}", p.GetCombinations().Count);
                Console.ReadKey();
            }

            void threeconsecutivenumbers()
            {
                long n = 1;
                while (n <= 1000000)
                {
                    if (n * (n + 1) * (n + 2) % 247 == 0)
                    {
                        Console.WriteLine("n is :: {0}", n * (n + 1) * (n + 2) / 247);
                        break;
                    }
                    n++;
                }

                Console.ReadKey();
            }

            void CalculateLimits()
            {
                double xn = 1, numerator = 0.0, limit = 0.0;
                long n = 1;
                while (n <= 1000000)
                {
                    numerator = Math.Abs((2 * n / xn) - xn);
                    limit = numerator / Math.Sqrt(n);
                    Console.WriteLine("n = {0}, Limit ={1}", n, limit);
                    xn = (2 * n / xn);
                    n++;
                }

                Console.ReadKey();
            }

            void Remainder585()
            {
                long N = 0;
                for (long i = 0; ; i++)
                {
                    if (i % 5 == 4 && i % 9 == 8 && i % 13 == 12)
                    {
                        N = i;
                        break;
                    }
                }

                Console.WriteLine("The number is multiple of :: {0}", N);
                Console.WriteLine("Remainder when divided by 585 is :: {0}", N % 585);
                Console.ReadKey();
            }

            void SumPrimeFactors201520162017()
            {
                long prime = 201520162017;
                int factor = 2;
                long sumFactors = 0;
                while (prime != 1)
                {
                    bool isAdded = false;
                    while (prime % factor == 0)
                    {
                        if (!isAdded)
                        {
                            sumFactors += factor;
                            isAdded = true;
                        }
                        prime /= factor;
                        if (prime == 1 || factor == 1) break;
                    }
                    factor++;
                }

                Console.WriteLine("Sum of factors of 201520162017 is :: {0}", sumFactors);
                Console.ReadKey();
            }

            /// <summary>
            /// How many numbers are there in the form of AABB which is a perfect square? Example: 7744 is in the form of AABB and it is square of 88.
            /// </summary>
            void PerfectSquaresCalculation()
            {
                int nSquares = 0;
                for (int a = 1; a < 10; a++)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        int num = int.Parse(a + "" + a + "" + b + "" + b);
                        if (GDF.MathFunctions.IsSquare(num)) nSquares++;
                    }
                }

                Console.WriteLine("No. of squares :: {0}", nSquares);
                Console.ReadKey();
            }

            void LeastmbynForpt33and1by3()
            {
                int m = 0, n = 0;
                for (n = 1; ; n++)
                {
                    bool found = false;
                    long n99 = 99 * n;
                    long i = n99 + 1;
                    while (i < 100 * n)
                    {
                        if (i % 300 == 0)
                        {
                            m = (int)i / 300;
                            found = true;
                        }
                        if (found) break;
                        i++;
                    }
                    if (found) break;
                }

                Console.WriteLine("m :: {0}, n :: {1}", m, n);
                Console.ReadKey();
            }

            void apowbeqbpowa()
            {
                System.Numerics.BigInteger a = 0;
                System.Numerics.BigInteger b = 0;

                int counter = 100;
                for (int i = 2; i <= 100; i++)
                {
                    for (int j = 2; j <= 100; j++)
                    {
                        if (i == j) continue;

                        bool breakthis = false;
                        a = 1; b = 1;
                        int iCounter = i;
                        while (iCounter > 0)
                        {
                            a *= j;
                            iCounter--;
                        }
                        int jCounter = j;
                        while (jCounter > 0)
                        {
                            b *= i;
                            jCounter--;

                            if (b > a)
                            {
                                breakthis = true;
                                break;
                            }
                        }

                        if (breakthis) break;

                        if (a == b)
                        {
                            counter++;
                        }
                    }
                }

                Console.WriteLine("Count :: {0}", counter);
                Console.ReadKey();
            }

            void Reciprocalab()
            {
                UniqueIntegralPairs p = new UniqueIntegralPairs("#");
                for (long i = 1; i <= 250000; i++)
                {
                    if (i != 156 && i * 156 % (i - 156) == 0)
                    {
                        if (i * 156 / (i - 156) > 0)
                        {
                            ArrayList l = new ArrayList();
                            l.Add(i); l.Add(i * 156 / (i - 156));
                            p.AddCombination(l);
                        }
                    }
                }

                Console.WriteLine("Done Iterating....");
                List<UniqueIntegralPairs.Combination> cs = p.GetCombinations();
                Console.WriteLine("Total possible combinations are :: {0}", cs.Count);
                string logDump = string.Empty;
                foreach (UniqueIntegralPairs.Combination c in cs)
                {
                    string s = string.Format("Possible values of (a,b) :: ({0},{1})", (long)c.Pair[0], (long)c.Pair[1]);
                    Console.WriteLine(s);
                }

                Console.ReadKey();
            }

            void LovableSeries()
            {
                long n = 1;
                long an = 0, anp1 = 0;
                long dn = 0, dmax = 0;

                while (n <= 100000000)
                {
                    an = 100 + (n * n);
                    anp1 = 100 + ((n + 1) * (n + 1));

                    dn = GDF.MathFunctions.GCD(an, anp1);

                    if (dn > dmax) dmax = dn;
                    ++n;
                }

                Console.WriteLine("Maximum of dn is :: {0}", dmax);
                Console.ReadKey();
            }

            /// <summary>
            /// n^3 + 100 % n + 10 = 0; Largest n.
            /// </summary>
            void ncubep100modnp10()
            {
                int n = 1;

                int nMax = 0;
                while (n <= 1000000)
                {
                    if ((Math.Pow(n, 3) + 100) % (n + 10) == 0)
                    {
                        if (n > nMax) nMax = n;
                    }
                    n++;
                }

                Console.WriteLine("Maximum n is :: {0}", nMax);
                Console.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/infinite-or-infinite-not-2/
            /// </summary>
            void InfiniteOrNot()
            {
                int k = 1; for (; k > 0; k++)
                {

                }
                Console.WriteLine(k);
                Console.ReadKey();
            }

            void Solve7()
            {
                int i = 1, count = 0; ;
                while (i > 0)
                {
                    List<long> residues = GDF.QuadraticResidue.GetAllQuadraticResidues(i);
                    if (residues.Sum(x => Convert.ToInt32(x)) * 9 == i * i)
                    {
                        ++count;
                        Console.WriteLine("Number found!! p = {0}", i);
                        if (count == 3) break;
                    }
                    i++;
                }
                Console.ReadKey();
            }

            void Solve6()
            {
                List<long> primes = GDF.KnownPrimes.GetAllKnownPrimes();

                foreach (long p in primes)
                {
                    List<long> residues = GDF.QuadraticResidue.GetAllQuadraticResidues(p);

                    Console.WriteLine("P :: {0}, residue count :: {1}", p, residues.Count);
                    if (residues.Count == 27)
                    {
                        Console.WriteLine("Number found!! p = {0}", p);
                        break;
                    }
                }
                Console.ReadKey();
            }

            void Solve5()
            {
                int count = 0;
                for (int i = 0; i <= 10000; i++)
                {
                    if (i % 24 == 0) { count++; }
                }

                Console.WriteLine("Total count is :: {0}", count);
                Console.ReadKey();
            }

            void Solve4()
            {
                System.Numerics.BigInteger b = new System.Numerics.BigInteger(0);
                for (int i = 1; i <= 888; i++)
                {
                    b = (b * 10) + 8;
                }

                Console.WriteLine("Remainder is :: {0}", b % 887);
                Console.ReadKey();
            }

            /// <summary>
            /// Number of squares in m x n grid with top right m1 x n1 portion removed. of size 1x1, 2x2, 3x3 so on ...
            /// </summary>
            void Solve3()
            {
                long sum = 0;
                int xMax = 25, yMax = 21;
                int x1Max = 12, y1Max = 11;
                int i, j;

                //region 1. rectangle 13 x 21.
                i = x1Max; j = yMax;
                if (x1Max.CompareTo(yMax) < 0)
                {
                    i = yMax;
                    j = x1Max;
                }

                int diff = Math.Abs((xMax - x1Max) - yMax);
                for (; i >= diff; i--, j--)
                {
                    sum += i * j;
                }

                //region 2. rectangle 12 x 10.
                i = xMax - x1Max; j = yMax - y1Max;
                if ((xMax - x1Max).CompareTo(yMax - y1Max) < 0)
                {
                    i = yMax - y1Max;
                    j = xMax - x1Max;
                }

                diff = Math.Abs((xMax - x1Max) - (yMax - y1Max));
                for (; i >= diff; i--, j--)
                {
                    sum += i * j;
                }

                //region 1 intersection region 2 near the border.
                i = 1; j = yMax - y1Max - 1;
                for (; i < yMax - y1Max; i++, j--)
                {
                    sum += i * j;
                }

                Console.WriteLine("Possible squares of the given grid is :: {2}", xMax, yMax, sum);
                Console.ReadKey();
            }

            /// <summary>
            /// Number of squares in m x n grid. of size 1x1, 2x2, 3x3 so on ...
            /// </summary>
            void Solve2()
            {
                long sum = 0;
                int xMax = 6, yMax = 6;
                int i = xMax, j = yMax;

                if (xMax.CompareTo(yMax) < 0)
                {
                    i = yMax;
                    j = xMax;
                }

                int diff = Math.Abs(xMax - yMax);
                for (; i >= diff; i--, j--)
                {
                    sum += i * j;
                }
                Console.WriteLine("Possible squares of {0} x {1} grid is :: {2}", xMax, yMax, sum);
                Console.ReadKey();
            }

            void Solve1()
            {
                double v = 450, c = 299792458;
                double onehourinms = 3600 * 1000;
                double nu = v * onehourinms / c;
                double denominator = Math.Sqrt((onehourinms - nu) * (onehourinms + nu));

                double timeDilation = Math.Abs(onehourinms - denominator);

                Console.WriteLine("Time Dilation in nano seconds is :: {0}", timeDilation * 1000000);
                Console.ReadKey();
            }
        }

        internal class ProblemCollection2
        {
            internal void Solve()
            {
                FindRemainder();
            }

            void FindRemainder()
            {
                QueuedConsole.WriteImmediate("Remainder: {0}", PermutationsAndCombinations.nCrBig(169, 13) % BigInteger.Pow(13, 5));
            }

            void PenAndPencils()
            {
                UniqueIntegralPairs p = new UniqueIntegralPairs();
                for (int i = 0; i <= 50; i++)
                {
                    for (int j = 0; j <= 168; j++)
                    {
                        if (50 * i + 15 * j == 2500)
                        {
                            p.AddCombination(new ArrayList() { i, j });
                        }
                    }
                }

                Console.WriteLine("Total numbers of ways :: {0}", p.GetCombinations().Count);
                Console.ReadKey();
            }

            /// <summary>
            /// All digits appears atleast twice.
            /// </summary>
            void NumberOf6DigitNumbers()
            {
                int count = 0;
                for (int i = (int)Math.Pow(10, 5); i < (int)Math.Pow(10, 6); i++)
                {
                    string istr = i.ToString();
                    IEnumerable<char> distinctCh = istr.ToCharArray().Distinct();
                    bool valid = true;
                    foreach (char ch in distinctCh)
                    {
                        if (istr.Count(x => x == ch) < 2)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                    {
                        count++;
                    }
                }

                QueuedConsole.WriteFinalAnswer(string.Format("Number of 6 digit numbers with digit appearing atleast twice is :: {0}", count));
            }

            /// <summary>
            /// https://brilliant.org/problems/joke-number/
            /// Smith Numbers are composite numbers n such that sum of digits of n = sum of digits of prime factors of n (counted with multiplicity).
            /// </summary>
            void SmithNumbers()
            {
                int n = 0;
                int consecutiveCounter = 0;
                int nStart = 0;
                while (true)
                {
                    n++;
                    if (GDF.Prime.IsPrime(n))
                    {
                        consecutiveCounter = 0;
                        nStart = 0;
                        continue;
                    }

                    if (GDF.Numbers.IsSmithNumber(n))
                    {
                        if (consecutiveCounter == 0)
                        {
                            nStart = n;
                        }
                        consecutiveCounter++;
                    }
                    else
                    {
                        consecutiveCounter = 0;
                        nStart = 0;
                    }

                    if (consecutiveCounter == 4)
                    {
                        break;
                    }
                }

                QueuedConsole.WriteFinalAnswer(string.Format("{0}, {1}, {2}, {3} are smith numbers", nStart, nStart + 1, nStart + 2, nStart + 3));
            }

            /// <summary>
            /// https://brilliant.org/problems/joke-number/
            /// What is the greatest integer n less than 10^5 such that both n and the sum of digits of n are prime ?
            /// </summary>
            void PrimeandDigitPrime()
            {
                int n = (int)Math.Pow(10, 5);
                while (true)
                {
                    if (GDF.Prime.IsPrime(n) && GDF.Prime.IsPrime((int)GDF.MathFunctions.DigitSum(n)))
                    {
                        break;
                    }
                    n--;
                }

                QueuedConsole.WriteFinalAnswer(string.Format("n is {0}", n));
            }

            /// <summary>
            /// Find the least n such that 2^n contains 3 consecutive 6's.
            /// </summary>
            void LeastnWith3consecutive6()
            {
                int n = 0;
                System.Numerics.BigInteger b = new System.Numerics.BigInteger(1);
                while (true)
                {
                    n++;
                    b *= 2;
                    if (b.ToString().IndexOf("666") > 0) break;
                }

                QueuedConsole.WriteFinalAnswer(string.Format("n is {0}", n));
            }

            void SumProductBrianBrina()
            {
                HashSet<int> set = new HashSet<int>();
                for (int i = 2; i < 100; i++)
                {
                    for (int j = i + 1; j <= 100; j++)
                    {
                        if (i + j >= 100) break;
                        else if (GDF.MathFunctions.IsSquare(i * j))
                        {
                            set.Add(i + j);
                        }
                        else if (GDF.Prime.IsPrime(i) && GDF.Prime.IsPrime(j))
                        {
                            set.Add(i + j);
                        }
                    }
                }
                Dictionary<int, int> iplusj = new Dictionary<int, int>();
                Dictionary<int, int> iproductj = new Dictionary<int, int>();
                List<Tuple<int, int, int>> tupleSums = new List<Tuple<int, int, int>>();
                tupleSums.Sort((x, y) =>
                {
                    int result = y.Item1.CompareTo(x.Item1);
                    return result == 0 ? y.Item2.CompareTo(x.Item2) : result;
                });
                List<Tuple<int, int, int>> tupleProds = new List<Tuple<int, int, int>>();
                tupleProds.Sort((x, y) =>
                {
                    int result = y.Item1.CompareTo(x.Item1);
                    return result == 0 ? y.Item2.CompareTo(x.Item2) : result;
                });
                for (int i = 2; i < 100; i++)
                {
                    for (int j = i + 1; j <= 100; j++)
                    {
                        if (i + j >= 100) break;
                        else if (GDF.Prime.IsPrime(i) && GDF.Prime.IsPrime(j))
                        {
                            continue;
                        }
                        else if (set.Contains(i + j)) continue;
                        else {
                            tupleSums.Add(new Tuple<int, int, int>(i + j, i, j));
                            tupleProds.Add(new Tuple<int, int, int>(i * j, i, j));

                            if (!iplusj.ContainsKey(i + j)) iplusj.Add(i + j, 1);
                            else iplusj[i + j] += 1;
                            if (!iproductj.ContainsKey(i * j)) iproductj.Add(i * j, 1);
                            else iproductj[i * j] += 1;

                            //Console.WriteLine("Possible x, y : {0}, {1}, product : {2}, sum :: {3}", i, j, i * j, i + j);
                        }
                    }
                }

                HashSet<int> iplusjSet = new HashSet<int>();
                foreach (KeyValuePair<int, int> kvp in iplusj)
                {
                    if (kvp.Value > 1)
                    {
                        iplusjSet.Add(kvp.Key);
                    }
                }

                HashSet<int> iproductjSet = new HashSet<int>();
                foreach (KeyValuePair<int, int> kvp in iproductj)
                {
                    if (kvp.Value > 1)
                    {
                        iproductjSet.Add(kvp.Key);
                    }
                }

                List<Tuple<int, int, int, int>> tuples = new List<Tuple<int, int, int, int>>();
                foreach (Tuple<int, int, int> prod in tupleProds)
                {
                    if (!iproductjSet.Contains(prod.Item1)) continue;
                    foreach (Tuple<int, int, int> sum in tupleSums)
                    {
                        if (!iplusjSet.Contains(sum.Item1)) continue;
                        if (prod.Item2 == sum.Item2 && prod.Item3 == sum.Item3)
                        {
                            tuples.Add(new Tuple<int, int, int, int>(sum.Item1, prod.Item1, sum.Item2, sum.Item3));
                        }
                    }
                }
                tuples.Sort((x, y) =>
                {
                    int result = x.Item1.CompareTo(y.Item1);
                    return result == 0 ? x.Item2.CompareTo(y.Item2) : result;
                });

                TEqualityComparer<Tuple<int, int, int, int>> tComparer = new TEqualityComparer<Tuple<int, int, int, int>>(
                    (t1, t2) => (t1.Item2 == t2.Item2 && t1.Item1 != t2.Item1),
                    t => ((t.Item1 + t.Item2) * (t.Item3 + t.Item4)).GetHashCode());

                List<Tuple<int, int, int, int>> clonedTuples = new List<Tuple<int, int, int, int>>();
                foreach (Tuple<int, int, int, int> t in tuples)
                {
                    clonedTuples.Add(new Tuple<int, int, int, int>(t.Item1, t.Item2, t.Item3, t.Item4));
                }

                List<Tuple<int, int, int, int>> cTuples = Enumerable.Intersect(tuples, clonedTuples, tComparer).ToList();

                foreach (Tuple<int, int, int, int> t in cTuples)
                {
                    Console.WriteLine("sum: {0}, product: {1}, x: {2}, y: {3}", t.Item1, t.Item2, t.Item3, t.Item4);
                }

                foreach (int l in iproductjSet)
                {
                    Dictionary<long, int> factors = GDF.Factors.GetPrimeFactorsWithMultiplicity(l);
                    string fStr = string.Empty;
                    foreach (KeyValuePair<long, int> kvp in factors)
                    {
                        int t = kvp.Value;
                        while (true)
                        {
                            fStr += kvp.Key + " ,";
                            t--;
                            if (t == 0) break;
                        }
                    }
                    //Console.WriteLine("product: {0}, factors: {1}", l, fStr);
                }

                Console.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/3-2-go/
            /// </summary>
            void _532go()
            {
                int n = 2017;
                string concatNum = string.Empty;
                while (true)
                {
                    if (GDF.Prime.IsPrime(n))
                    {
                        concatNum = concatNum + "" + n;
                    }
                    n--;
                    if (n == 1) break;
                }
                int L = concatNum.Length, S = 0;
                char[] concatNumArr = concatNum.ToCharArray();
                foreach (char ch in concatNumArr)
                {
                    S += (int)char.GetNumericValue(ch);
                }
                QueuedConsole.WriteImmediate(string.Format("S: {0}, L: {1}, (S-L)(mod2016): {2}", S, L, (S - L) % 2016));
                QueuedConsole.WriteFinalAnswer(string.Format("[(S-L)(mod2016)]/10 = {0}", ((double)(S - L) % 2016) / 10.0));
            }

            /// <summary>
            /// https://brilliant.org/problems/lets-wait-for-2016/
            /// </summary>
            void WaitFor2016()
            {
                int n = 2014;
                string concatNum = string.Empty;
                int totMod = 0;
                while (true)
                {
                    int n1 = n % 2016;
                    int p = 1, counter = 2014;
                    while (true)
                    {
                        p *= n1;
                        p = p % 2016;
                        counter--;
                        if (counter == 0) break;
                    }
                    totMod += p;
                    n--;
                    if (n == 0) break;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Final answer : {0}", totMod % 2016));
            }

            /// <summary>
            /// https://brilliant.org/problems/matrix-of-primes/
            /// </summary>
            void MatrixOfPrimes()
            {
                UniqueIntegralPairs p = new UniqueIntegralPairs();
                HashSet<int> set = new HashSet<int>() { 2, 3, 5, 7 };
                for (int w = 2; w < 8; w++)
                {
                    if (!set.Contains(w)) continue;
                    for (int x = 2; x < 8; x++)
                    {
                        if (!set.Contains(x)) continue;
                        for (int y = 2; y < 8; y++)
                        {
                            if (!set.Contains(y)) continue;
                            for (int z = 2; z < 8; z++)
                            {
                                if (!set.Contains(z)) continue;

                                if (w * z - y * x > 0 && GDF.Prime.IsPrime(w * z - y * x))
                                {
                                    p.AddCombination(new ArrayList() { w, x, y, z });
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Final answer : {0}", p.GetCombinations().Count));
            }

            /// <summary>
            /// Huge_Even+Huge_Odd=Headache
            /// https://brilliant.org/problems/huge-even-huge-odd-headache/
            /// </summary>
            void HugeOddEvenHeadache()
            {
                int i = 0;
                string sOdd = string.Empty, sEven = string.Empty;
                while (true)
                {
                    if (i % 2 == 0) sEven += i;
                    else sOdd += i;

                    i++;
                    if (i > 2017) break;
                }
                System.Numerics.BigInteger b = System.Numerics.BigInteger.Parse(sOdd);
                b += System.Numerics.BigInteger.Parse(sEven);

                QueuedConsole.WriteFinalAnswer(string.Format("Sum of digits of number :: {0}", (GDF.MathFunctions.DigitSum(b)).ToString()));
            }

            /// <summary>
            /// Given is a number with 10000 digits. Find the fourteen adjacent digits in this number that have the greatest product. What is the value of this product?
            /// https://brilliant.org/problems/multiply-me-out/
            /// </summary>
            void MultiplyMeOut()
            {
                //14-adjacent-digits-max-product
                string tenthoudigitNum = string.Empty;
                var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.14-adjacent-digits-max-product.txt", true);
                using (StreamReader sr = new StreamReader(stream))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        tenthoudigitNum += s.Replace(" ", "");
                    }
                }
                char[] array = tenthoudigitNum.ToCharArray();
                long maxProduct = 0;
                for (int i = 0; i + 14 <= tenthoudigitNum.Length; i++)
                {
                    char[] ranged = (tenthoudigitNum.Substring(i, 14)).ToCharArray();
                    long product = 1;
                    foreach (char ch in ranged)
                    {
                        int c = (int)char.GetNumericValue(ch);
                        if (c == 0)
                        {
                            break;
                        }
                        product *= c;
                    }
                    if (product > maxProduct) maxProduct = product;
                }

                QueuedConsole.WriteFinalAnswer(string.Format("Greatest product is :: {0}", maxProduct));
            }

            /// <summary>
            /// https://brilliant.org/problems/numerical-palindromes/
            /// Consider all substrings of N with length strictly greater than 1. 
            /// Let P be the number of these substrings that are palindromes (read the same forwards and backwards), and let L 
            /// be the largest palindromic substring (converted to an integer). Compute the remainder when P + L is divided by 1000.
            /// </summary>
            void NumberPalindrome()
            {
                string fifhunddigitNum = string.Empty;
                var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.1500-digit-number-palindrome.txt", true);
                using (StreamReader sr = new StreamReader(stream))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        fifhunddigitNum += s.Replace(" ", "");
                    }
                }

                int P = 0;
                System.Numerics.BigInteger L = new System.Numerics.BigInteger(0);
                for (int len = 2; len < fifhunddigitNum.Length; len++)
                {
                    for (int i = 0; i + len <= fifhunddigitNum.Length; i++)
                    {
                        string s = fifhunddigitNum.Substring(i, len);
                        if (GenericDefs.Functions.Algorithms.DP.Palindrome.IsPalindrome(s))
                        {
                            P++;
                            System.Numerics.BigInteger b = System.Numerics.BigInteger.Parse(s);
                            if (b > L) L = b;
                        }
                    }
                }

                QueuedConsole.WriteImmediate(string.Format("P: {0}, L: {1}", P, L.ToString()));
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", (L + P) % 1000));
            }

            void DividesAllNumbersToAMillion()
            {
                List<int> primes = GDF.Prime.GeneratePrimesNaiveNMax(1000000);

                Dictionary<int, int> allFactors = new Dictionary<int, int>();
                foreach (int p in primes)
                {
                    allFactors.Add(p, 1);
                }

                for (int i = 2; i <= 1000000; i++)
                {
                    List<GDF.PrimeFactor> l = GDF.Prime.GeneratePrimeFactors(i, primes);

                    foreach (GDF.PrimeFactor pf in l)
                    {
                        if (pf.Mul > allFactors[pf.Prime]) allFactors[pf.Prime] = pf.Mul;
                    }
                }

                int m = allFactors[2];
                if (m > allFactors[5]) m = allFactors[5];

                allFactors[2] = allFactors[2] - m;
                allFactors[5] = allFactors[5] - m;

                int pFinal = 1;
                foreach (KeyValuePair<int, int> kvp in allFactors)
                {
                    int ithMod = pFinal;
                    int ni = kvp.Key % 1000;
                    int counter = kvp.Value;
                    while (true)
                    {
                        if (counter == 0) break;
                        ithMod *= ni;
                        ithMod = ithMod % 1000;
                        counter--;
                    }
                    pFinal = ithMod;
                }

                string answer = string.Format("Last 3 digits of the result is : {0}", pFinal);
                Logger.Log("https://brilliant.org/problems/divides-all-numbers-to-a-million/");
                Logger.Log("DividesAllNumbersToAMillion.");
                Logger.Log(answer);
                QueuedConsole.WriteFinalAnswer(answer);
            }

            void RemainderTest()
            {
                int[] arr = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
                int divisor = 1000;

                long mod = GDF.RemainderCalculator.RemainderWithNestedPowers(arr, divisor);
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", mod));
            }

            void LcmLcmSum()
            {
                System.Numerics.BigInteger b = new System.Numerics.BigInteger();
                for (int j = 1; j <= 1000; j++)
                {
                    for (int i = 1; i <= 10000; i++)
                    {
                        b += GDF.MathFunctions.LCM(i, j);
                    }
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is :: {0}", b.ToString()));
            }

            void FrobeniusTest()
            {
                int? frobenius = null;
                if (GDF.Algorithms.DP.FrobeniusSolver.NthFrobeniusNumber(new int[] { 13, 31 }, ref frobenius, 2))
                {
                    QueuedConsole.WriteFinalAnswer(string.Format("Frobenius is : {0}", frobenius.Value));
                }
                else {
                    QueuedConsole.WriteFinalAnswer("Frobenius not found.");
                }
            }

            void FrobeniusBrute()
            {
                int n = 1000;
                int counter = 0;
                int frobenius = 0;
                while (true)
                {
                    int xMax = n / 13;
                    bool found = false;
                    for (int x = 0; x <= xMax; x++)
                    {
                        int yMax = (n - (x * 13)) / 31;
                        for (int y = 0; y <= yMax; y++)
                        {
                            if (n == (x * 13) + (y * 31))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found) break;
                    }
                    if (!found)
                    {
                        counter++;
                    }
                    if (counter == 2)
                    {
                        frobenius = n;
                        break;
                    }
                    if (n == 0) break;
                    n--;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Frobenius is : {0}", frobenius));
            }

            void TestConsoleExecutionWhileSleep()
            {
                Func<StatusUpdater, StatusUpdater> initializer = delegate (StatusUpdater su)
                {
                    su.SetMessage("test");
                    return su;
                };

                StatusUpdater s = new StatusUpdater(1000, initializer);
                s.Start();

                Stopwatch sw = new Stopwatch();
                while (true)
                {
                    if (!sw.IsRunning)
                    {
                        sw.Start();
                    }
                    long counter = 0;
                    while (true)
                    {
                        counter++;

                        if (counter == 10000000) break;
                    }
                    if (sw.ElapsedMilliseconds % 100000000000 == 0) break;
                }
                sw.Stop();
                s.StopAndDispose();
            }

            /// <summary>
            /// https://brilliant.org/problems/dare-to-solve-2/
            /// </summary>
            void HowMany6DigitUsing4Digits()
            {
                int counter = 0;
                for (int i = 100000; i < 1000000; i++)
                {
                    if (GDF.Numbers.GetDigitArray(i).Distinct().Count() == 4) counter++;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Number of 6 digit numbers : {0}", counter));
            }

            void SumOddDigitsFibonacci()
            {
                int N = 1000;
                System.Numerics.BigInteger b = GDF.NumberTheory.Fibonacci.NthFibonacciNumber(N);
                string bS = b.ToString();
                int sum = 0;
                for (int i = 0; i < bS.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        sum += (int)char.GetNumericValue(bS[i]);
                    }
                }

                SpecialConsole scon = new SpecialConsole(10, 100);

                scon.WriteHighlightedImmediate("Sum of odd digits of {0}th fibonacci number is : {1}", new object[] { N, sum });
                scon.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/are-you-smarter-than-me-42/
            /// </summary>
            void AreYouSmarterThanMe42()
            {
                int N = 42;
                System.Numerics.BigInteger b43 = GDF.NumberTheory.Fibonacci.NthFibonacciNumber(N);
                System.Numerics.BigInteger b44 = GDF.NumberTheory.Fibonacci.NthFibonacciNumber(N + 1);

                QueuedConsole.WriteImmediate(string.Format("|z| is : {0}", (b43 + b44).ToString()));
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/crazy-minimum/
            /// </summary>
            void CrazyMinimum()
            {
                int N = 0;
                while (true)
                {
                    N++;
                    int n = 0;
                    System.Numerics.BigInteger num = new System.Numerics.BigInteger(0);
                    System.Numerics.BigInteger den = new System.Numerics.BigInteger(0);
                    while (true)
                    {
                        n++;
                        num += 2 * n * 2 * n;
                        den += ((2 * n) - 1) * ((2 * n) - 1);
                        if (n == N) break;
                    }
                    if (100 * num < 101 * den) break;
                }

                QueuedConsole.WriteImmediate(string.Format("n is : {0}", N));
                QueuedConsole.ReadKey();
            }

            void PersistentDigit()
            {
                int n = 0;
                string nStr = string.Empty;
                while (true)
                {
                    n++;
                    nStr += n + "";
                    if (n == 2016) break;
                }
                
                Dictionary<int, int> d = new Dictionary<int, int>();
                Enumerable.Range(0, 10).ForEach(x => d.Add(x, 0));
                nStr.ToCharArray().ForEach(c => d[(int)char.GetNumericValue(c)]++);

                int nMax = -1, cMax = int.MinValue;
                d.ForEach(x => { if (x.Value > cMax) { cMax = x.Value; nMax = x.Key; } });
                QueuedConsole.WriteImmediate("Most repeated digit : {0}",nMax);
            }

            /// <summary>
            /// https://brilliant.org/problems/arithmetic-mean-2/
            /// </summary>
            void ArithmeticMean2()
            {
                HashSet<double> set = new HashSet<double>();
                Enumerable.Range(1, 2016).ForEach(x => set.Add(x * 1.0));

                while (true)
                {
                    double d1 = set.ElementAt(set.Count - 1); set.Remove(d1);
                    double d2 = set.ElementAt(set.Count - 1); set.Remove(d2);
                    set.Add((d1 + d2) / 2.0);
                    if (set.Count == 1) break;
                }

                QueuedConsole.WriteImmediate("Remaining value : {0}", set.ElementAt(0));
            }

            /// <summary>
            /// https://brilliant.org/problems/consecutive-integers-7/
            /// </summary>
            void ConsecutiveIntegers7()
            {
                int n = 5;
                while (true)
                {
                    if((n+1) % 7 == 0)
                    {
                        if ((n + 2) % 9 == 0)
                        {
                            if ((n + 3) % 11 == 0)
                            {
                                break;
                            }
                        }
                    }
                    n += 5;
                }
                QueuedConsole.WriteImmediate("a+b+c+d : {0}", (4 * n) + 6);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-computer-science-problem-by-paola-ramirez-3/
            /// </summary>
            void _4DigitPerfectSquares()
            {
                int n = 1000;
                int count = 0;
                while (true)
                {
                    if (GDF.MathFunctions.IsSquare(n))
                    {
                        int ab = n / 100;
                        int cd = n % 100;
                        if (GDF.MathFunctions.IsSquare(ab - cd)) { count++; }
                    }
                    n++;
                    if (n >= 10000) break;
                }
                QueuedConsole.WriteImmediate("Number of integers satisfying the conditon : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-100-sided-die/
            /// </summary>
            void _100SidedDieExpectedValue()
            {
                QueuedConsole.WriteImmediate("Expected value for number of rolls : {0}", Math.Ceiling(100 * GDF.NumberTheory.SeriesSummation._1ByN(100)));
            }

            /// <summary>
            /// https://brilliant.org/problems/restricted-sum/
            /// </summary>
            void RestrictedSum()
            {
                UniqueArrangements<int> ua = GDF.Algorithms.DP.Knapsack.Variation1.Solve(200, 3, 0, 100);
                QueuedConsole.WriteImmediate("Number of possible non-distinct triples : {0}", ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/sum-of/
            /// </summary>
            void SumOfAll()
            {
                long sum = 0;
                Enumerable.Range(1, 2015).ForEach(x => sum += GDF.MathFunctions.GCD(x, 2015));
                QueuedConsole.WriteImmediate("Approach 1. Sum : {0}", sum);

                sum = 0;
                HashSet<long> divisors = GDF.Factors.GetAllFactors(2015);
                divisors.ForEach(x => sum += 2015 * GDF.EulerTotient.CalculateTotient(x) / x);
                QueuedConsole.WriteImmediate("Approach 2. Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/largest-greatest-common-divisor/
            /// </summary>
            void LargestGreatestCommonDivisor()
            {
                int maxGCD = int.MinValue;
                int n = 1;
                while (true)
                {
                    maxGCD = Math.Max(maxGCD, GDF.MathFunctions.GCD(5 * n + 6, 8 * n + 7));
                    n++;
                    if (n > 100000) break;
                }
                QueuedConsole.WriteImmediate("Largest possible value of the greatest common divisor : {0}", maxGCD);
            }

            /// <summary>
            /// https://brilliant.org/problems/2015-is-great/
            /// </summary>
            void _2015IsGreat()
            {
                BigInteger b = PermutationsAndCombinations.Factorial(2015);
                while (b % 10 == 0) { b /= 10; }
                QueuedConsole.WriteImmediate("Last three digits of 2015! : {0}", b % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/another-pascal-problem/
            /// </summary>
            void AnotherPascalProblem()
            {
                BigInteger ncr = PermutationsAndCombinations.nCrBig(2014, 2);
                ncr *= ncr;
                BigInteger sumicube = GDF.NumberTheory.SeriesSummation.SumNCube(2013);

                QueuedConsole.WriteImmediate("Value : {0}", (ncr - sumicube).ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/calculator-will-blow-up/
            /// </summary>
            void TrailingZeroesOneLakhFactorial()
            {
                int nVal = 100000;
                int nZeroes = 0;
                int d = 5;
                while (true)
                {
                    nZeroes += nVal / d;
                    d *= 5;
                    if (d > nVal) break;
                }
                QueuedConsole.WriteImmediate("Number of trailing zeroes in {0}! : {1}", nVal, nZeroes);
            }

            void LastFourNonZeroDigits()
            {
                BigInteger bi = PermutationsAndCombinations.Factorial(2016);
                while (bi % 10 == 0) { bi /= 10; }
                QueuedConsole.WriteImmediate("Last four non-zero digits of 2016! : {0}", bi % 10000);
            }

            void SumPrimesLessThan1000()
            {
                int i = 0;
                int sum = 0;
                while(i*i < 1000)
                {
                    i++;
                    int isq = i * i;
                    if (isq > 1000) break;

                    if (GDF.Prime.IsPrime(isq + 1)) sum += isq + 1;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/floor-function-5/
            /// </summary>
            void FloorFunction5()
            {
                int t = 0;
                int nMax = 1;
                while (++t <= 10)
                {
                    BigInteger b = 1;
                    nMax += 10;
                    int n = 0;
                    while (++n <= nMax)
                    {
                        b *= 10;
                    }
                    BigInteger bCube = b * b * b;

                    BigRational r = new BigRational(bCube, b + 2016);
                    QueuedConsole.WriteImmediate("Last seven digits for n : {0} is : {1} ", nMax, r.GetWholePart() % 10000000);
                }

            }

            /// <summary>
            /// https://brilliant.org/problems/solve-it-40/?ref_id=1238108
            /// </summary>
            void SolveIt40()
            {
                double d1 = 89.9999999999;
                double d2 = 89.9999999999999;
                double tan1 = Math.Tan(d1 * Math.PI / 180);
                double tan2 = Math.Tan(d2 * Math.PI / 180);

                QueuedConsole.WriteImmediate("tanA/tanB : {0}", tan2 / tan1);
            }

            void NumbersStartingWith1()
            {
                long n = 1,nMax = 100000;
                nMax *= nMax;
                long count = 1;
                double log2 = Math.Log10(2);
                while (n <= nMax)
                {
                    double val = n * log2;
                    if((val - Math.Floor(val)) < log2) { count++; n += 3; }
                    else { n++; }
                }
                QueuedConsole.WriteImmediate("Last three digits of N : {0}", count % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/finding-that-missing-number/
            /// </summary>
            void FindingMissingNumbers()
            {
                List<List<int>> Numbers = new List<List<int>>() {
                    new List<int>(){ 8, 4, 4 },new List<int>(){ 8, 7, 1 },new List<int>(){ 7, 1, 3 },new List<int>(){ 4, 1, 3 },
                    new List<int>(){ 6, 3, 5 },new List<int>(){ 5, 2, 3 },new List<int>(){ 6, 1, 5 }
                };

                Numbers.ForEach(x =>
                {
                    int xpy = (x[1] + x[2]);
                    int xor1 = 1;
                    Enumerable.Range(2, x[0]).ForEach(y => { xor1 = xor1 ^ y; });
                    int? xor2 = null;
                    Enumerable.Range(1, x[0]).ForEach(y => {
                        if (y != x[1] && y != x[2]) {
                            if (xor2 == null) xor2 = y;
                            else xor2 = xor2.Value ^ y;
                        }
                    });

                    xor1 = xor1 ^ xor2.Value;
                    QueuedConsole.WriteImmediate("1:: x + y = {0}, 2:: x @ y = {1}", xpy, xor1);
                    QueuedConsole.WriteImmediate("-------------------");
                });
            }

            /// <summary>
            /// https://brilliant.org/problems/150-day-streak-completed-but-whats-this/
            /// </summary>
            void Mod2289()
            {
                int n = 11;
                int mod = 0;
                while(--n > 0)
                {
                    int t = 0;
                    BigInteger bi = 1;
                    while (++t<=n) {
                        bi *= n;
                    }
                    mod += (int)(bi % 2289);
                }
                mod -= 634 % 2289;
                mod = mod % 2289;
                QueuedConsole.WriteImmediate("Value : {0}", mod);
            }

            /// <summary>
            /// https://brilliant.org/problems/evaluate-the-sum/
            /// </summary>
            void EvaluateTheSum()
            {
                BigRational r = new BigRational(1, 1);
                long n = 0;
                while (++n <= 200000)
                {
                    long c = 4 * n * n;
                    r *= c;
                    r /= (c - 1);
                }
                double val = NumberConverter.BigRationalToDouble(r);
                double ln = Math.Log(val);
                QueuedConsole.WriteImmediate("val : {0}, S : {1}", val, ln);
                QueuedConsole.WriteImmediate("Floor(10^6*val) : {0}", Math.Floor(1000000 * ln));

            }

            void FractionConvert()
            {
                double val = 0.0439798350698182;
                Fraction<int> fr = FractionConverter<int>.RealToFraction(val, 0.00000000001);
                QueuedConsole.WriteImmediate("a:{0}, b:{1}", fr.N, fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/ratio-of-diverging-series/
            /// </summary>
            void DivergingSeries()
            {
                int n = 0;
                BigRational sum = new BigRational(0, 1);
                while (++n <= 10000)
                {
                    long numerator = n * (n + 5);
                    long denominator = 2 * (n + 2);
                    denominator *= ((2 * n * n) + (4 * n) - 1);
                    sum += new BigRational(numerator, denominator);
                }

                Fraction<int> fr = FractionConverter<int>.BigRationalToFraction(sum);
                double val = NumberConverter.BigRationalToDouble(sum);
                QueuedConsole.WriteImmediate("Sum of series : {0}", val);
                QueuedConsole.WriteImmediate("A: {0}, B: {1}", fr.N, fr.D);
            }

            void _9DigitNumbers()
            {
                int d = 360;
                int counter = 0;
                long n = 10000, nMax = 100000;
                n *= 10000;
                nMax *= 10000;
                while (++n > 0) { if (n % d == 0) break; }
                while (n <= nMax)
                {
                    HashSet<char> set = new HashSet<char>();
                    string nstr = n + "";
                    bool inValid = false;
                    foreach (char c in nstr)
                    {
                        if (!set.Add(c)) { inValid = true; break; }
                    }
                    if (!inValid) counter++;

                    n += d;
                }
                QueuedConsole.WriteImmediate("N: {0}", counter);
            }
        }
    }
}