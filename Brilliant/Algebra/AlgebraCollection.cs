using GenericDefs.Classes;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using GenericDefs.Functions.NumberTheory;
using Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Brilliant.Algebra
{
    /// <summary>
    /// 
    /// </summary>
    public class AlgebraCollection : ISolve, IBrilliant
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
            //(new VietaAndNewtonIdentities()).Solve();
            (new LinearEquations()).Solve();
            //(new Collection2()).Solve();
            //(new TelescopingSeries()).Solve();
        }

        internal class Collection1
        {
            internal void Solve()
            {
                //SolveWithBiggerStack();
                BodyBuilding();
            }

            void BodyBuilding()
            {
                int LeastCost = int.MaxValue;
                for (int a = 1; a <= 100; a++)
                {
                    for (int b = 1; b <= 150; b++)
                    {
                        if ((2 * b) + a >= 120)
                        {
                            if ((2 * b) + (3 * a) >= 220)
                            {
                                if ((6 * b) + (5 * a) >= 500)
                                {
                                    LeastCost = Math.Min(LeastCost, (3 * a) + (4 * b));
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Least cost: {0}", LeastCost);
            }

            /// <summary>
            /// https://brilliant.org/problems/if-you-can-solve-it/
            /// </summary>
            void IfYouCanSolveIt()
            {
                double sum = 0.0;
                Enumerable.Range(100, 900).ForEach(x =>
                {
                    sum += x * 1.0 / MathFunctions.DigitSum(x);
                });
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/summation-problem-2015/
            /// </summary>
            void SummationProblem2015()
            {
                BigInteger b = 2015 * 20;
                Enumerable.Range(1, 20).ForEach(x => { b += BigInteger.Pow(x, 6); });

                int max = 0;
                List<long> primes = KnownPrimes.GetAllKnownPrimes();
                primes.ForEach(x =>
                {
                    while (b % x == 0) { b /= x; max = (int)x; }
                });
                QueuedConsole.WriteImmediate("greatest prime factor : {0}", max);
            }

            void SolveWithBiggerStack()
            {
                System.Threading.Thread t = new System.Threading.Thread(OddCoefficients, 1000000);
                t.Start();
                t.Join();
            }

            /// <summary>
            /// https://brilliant.org/practice/polynomial-arithmetic-level-4-5-challenges/?problem=my-100-followers-problem
            /// https://brilliant.org/problems/my-100-followers-problem/
            /// </summary>
            void CoefficientOfExpansion()
            {
                List<int> numbers = new List<int>();
                Enumerable.Range(1, 50).ForEach(x => { numbers.Add(x); });

                for (int i = 2; i <= 4; i++)
                {
                    List<IList<int>> combinations = Combinations.GetAllCombinations(numbers, i);
                    long sum = 0;
                    foreach (IList<int> c in combinations)
                    {
                        int t = 1;
                        c.ForEach(x => { t *= x; });
                        sum += t;
                    }
                    QueuedConsole.WriteImmediate("Coefficient of x^{0} is : {1}", 50 - i, sum);
                }

                long reqValue = 1275 * 1275 + 1;
                reqValue -= 791350 * 2;
                QueuedConsole.WriteImmediate("Required sum : {0}", reqValue);
            }

            /// <summary>
            /// a^2 = bc + 1; b^2 = ac + 1; a,b,c are integers
            /// Find all ordered triplets a,b,c
            /// </summary>
            void a2isbcp1b2isacp1()
            {
                UniqueIntegralPairs p = new UniqueIntegralPairs("@#$");
                int aMax = 100, aMin = -100;
                int bMax = 100, bMin = -100;
                int cMax = 100, cMin = -100;

                for (int a = aMin; a <= aMax; a++)
                {
                    for (int b = bMin; b <= bMax; b++)
                    {
                        for (int c = cMin; c <= cMax; c++)
                        {
                            int expr1 = (a * a) - ((b * c) + 1);
                            int expr2 = (b * b) - ((a * c) + 1);
                            if (expr1 == 0 && expr2 == 0)
                            {
                                ArrayList l = new ArrayList();
                                l.Add(a); l.Add(b); l.Add(c);
                                p.AddCombination(l);
                            }
                        }
                    }
                }
                Console.WriteLine("Number of ways :: {0}", p.GetCombinations().Count);

                foreach (UniqueIntegralPairs.Combination c in p.GetCombinations())
                {
                    Console.WriteLine("a :: {0}, b :: {1}, c :: {2}", c.Pair[0], c.Pair[1], c.Pair[2]);
                }
                Console.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/squares-in-between/
            /// a + b + c = 11
            /// a^3 + b^3 + c^3 = 785.  Sum of any two variables is larger than 1.
            /// Find a^2 + b^2 + c^2.
            /// </summary>
            void SquaresInBetween()
            {
                int sum = 0;
                for (int a = 0; a <= 100; a++)
                {
                    long a3 = a * a * a;
                    for (int b = 0; b >= -100; b--)
                    {
                        long b3 = b * b * b;
                        if (a + b <= 1) continue;
                        for (int c = 0; c <= 100; c++)
                        {
                            long c3 = c * c * c;
                            if ((c + b <= 1) || (a + c <= 1)) continue;
                            if ((a + b + c == 11) && (a3 + b3 + c3 == 785))
                            {
                                sum = (a * a) + (b * b) + (c * c);
                                QueuedConsole.WriteImmediate(string.Format("All conditions met for, a: {0}, b: {1}, c: {2}", a, b, c));
                                break;
                            }
                        }
                    }
                }

                QueuedConsole.WriteFinalAnswer("Sum : " + sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/do-it-like-gauss-part-2/
            /// A Gaussian Integer is a complex number of the form z = a + ib, where a and b are integers. 
            /// How many Gaussian Integers z divide 1000, in the sense that 1000 = z x w. for some Gaussian Integer w?
            /// </summary>
            void DoItLikeGauss()
            {
                HashSet<long> factors = Factors.GetAllFactors(1000);
                int totalWays = 0;
                foreach (long f in factors)
                {
                    UniqueIntegralPairs p = Numbers.UnusualFunctions.NumberAsSumOfTwoSquares(f);
                    List<UniqueIntegralPairs.Combination> cList = p.GetCombinations();
                    if (cList.Count > 0)
                    {
                        foreach (UniqueIntegralPairs.Combination c in cList)
                        {
                            int k = 1000 / ((c.Pair[0] * c.Pair[0]) + (c.Pair[1] * c.Pair[1]));
                            Dictionary<long, int> kFactors = Factors.GetPrimeFactorsWithMultiplicity(k);
                            int ways = 1;
                            foreach (KeyValuePair<long, int> kvp in kFactors)
                            {
                                ways *= GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(kvp.Value, 2, 0).Count();
                            }
                            if (c.Pair[0] == 0 || c.Pair[1] == 0)
                                totalWays += 2 * ways;
                            else totalWays += 4 * ways;
                            QueuedConsole.WriteImmediate(string.Format("k: {0}, z: {1} + {2}i, w: {1} - {2}i, ways: {3}", k, c.Pair[0], c.Pair[1], ways));
                        }
                    }
                }

                QueuedConsole.WriteFinalAnswer(string.Format("Totalways : {0}", totalWays));
            }

            /// <summary>
            /// https://brilliant.org/problems/is-it-recurring/
            /// tau(x) = 1/2-x. if x = m/n, tau(x) = n/(2n-m).
            /// </summary>
            void IsItRecurring()
            {
                int n = 0;

                Func<long[], long[]> constructor = delegate (long[] a)
                {
                    long[] result = new long[2];
                    result[0] = a[1];
                    result[1] = 2 * a[1] - a[0];
                    return result;
                };

                Fraction<long> t1 = new Fraction<long>(constructor, new long[] { 6, 29 }), tn = t1;
                QueuedConsole.WriteImmediate(string.Format("Tau - {0} is : {1}", n, tn.N + " / " + tn.D));
                while (true)
                {
                    n++;
                    if (n == 1)
                    {
                        continue;
                    }
                    else
                    {
                        tn = new Fraction<long>(constructor, new long[] { tn.N, tn.D });
                    }

                    QueuedConsole.WriteImmediate(string.Format("Tau - {0} is : {1}", n, tn.N + " / " + tn.D));
                    if (tn.N == t1.N && tn.D == t1.D)
                    {
                        QueuedConsole.WriteImmediate(string.Format("Its a recurring sequence. Cycle : {0}", n - 1));
                        break;
                    }
                    if (n == 10000) break;
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/till-the-last-breath/
            /// </summary>
            void TillTheLastBreath()
            {
                BigRational bSum = new BigRational(0.0);
                BigRational bi;
                long denominator = 0;
                int n = 0;
                while (true)
                {
                    n++;
                    denominator += n * (n + 1) * (n + 2);
                    bi = new BigRational(n * (n + 2), denominator);
                    bSum += bi;

                    if (n == 10000) break;
                }

                Fraction<long> frac = FractionConverter<long>.BigRationalToFraction(bSum, 0.000000000001);
                QueuedConsole.WriteImmediate(" phi is : A/B = " + frac.N + " / " + frac.D);
                QueuedConsole.WriteImmediate(" phi is : = " + (1.0 * frac.N / frac.D));
                QueuedConsole.ReadKey();
            }

            void RemovingFloors()
            {
                int n = 1;
                int counter = 0;
                while (true)
                {
                    int lhs = (int)Math.Floor(1.0 * n / 2) + (int)Math.Floor(1.0 * n / 3) + (int)Math.Floor(1.0 * n / 6);
                    int rhs = n - 1;
                    if (lhs == rhs) counter++;
                    n++;
                    if (n == 101) break;
                }

                QueuedConsole.WriteFinalAnswer("Count : " + counter);
            }

            void Pow2First9DigitsAs9()
            {
                BigInteger b = new BigInteger(1);
                int n = 0;
                while (true)
                {
                    n++;
                    b *= 2;
                    if (b.ToString().StartsWith("99999"))
                    {
                        break;
                    }
                }
                QueuedConsole.WriteFinalAnswer("Count : " + n);
            }

            /// <summary>
            /// https://brilliant.org/problems/sigma-5/
            /// </summary>
            void SigmaSum()
            {
                double sum = 0.0;
                int n = 0;
                while (true)
                {
                    n++;
                    double fourpowx = Math.Pow(4, n * 1.0 / 1000.0);
                    sum += fourpowx / (fourpowx + 2);
                    if (n == 1000) break;
                }

                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/sequenceaits/
            /// </summary>
            void SequenceAits()
            {
                long n = 2;
                BigRational b = new BigRational(0, 1);
                while (true)
                {
                    long _2n = 2 * n;
                    b += new BigRational(_2n + 1, (_2n - 1) * (_2n - 3) * (_2n + 3));
                    n++;
                    if (n == 1000000) break;
                }

                Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(b);
                QueuedConsole.WriteImmediate("45*S-1 : {0}, n : {1}", (45 * fr.N / fr.D) - 1, Math.Ceiling(Math.Sqrt((1.0 * 45 * fr.N / fr.D) - 1)));
            }

            /// <summary>
            /// https://brilliant.org/problems/an-algebra-problem-by-ilham-saiful-fauzi/
            /// </summary>
            void FloorFunctionSolutions()
            {
                int floorX = -2017;
                int count = 0;
                while(++floorX <= 2016)
                {
                    double x = 2016.0;
                    x/= 2015;
                    x *= floorX;
                    if (Math.Floor(x) == floorX) {
                        QueuedConsole.WriteImmediate("x : {0}", x);
                        count++;
                    }
                }
                QueuedConsole.WriteImmediate("Number of real solutions : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/brilliant-math-contest-2/
            /// </summary>
            void BrilliantMathContest2()
            {
                int m = 1;
                BigRational _6by7 = new BigRational(6, 7);
                BigRational _1by7 = new BigRational(1, 7);
                BigRational nLeft = new BigRational(1, 7);
                BigRational sLeft = new BigRational(-1, 7);
                BigRational sumnLeft = nLeft;
                BigRational sumsLeft = sLeft;
                int ksum = 1;
                while (m <= 10)
                {
                    BigRational lhs = 1 - sumnLeft;
                    BigRational rhs = ksum + m + 1 - sumsLeft;
                    BigRational nVal = rhs / lhs;

                    double dnVal = NumberConverter.BigRationalToDouble(nVal);
                    //QueuedConsole.WriteImmediate("S left numerator : {0}, denominator : {1}", sLeft.Numerator, sLeft.Denominator);
                    //QueuedConsole.WriteImmediate("N left numerator : {0}, denominator : {1}", nLeft.Numerator, nLeft.Denominator);
                    QueuedConsole.WriteImmediate("k : {0}, n : {1}", m + 1, dnVal.ToString());

                    if (nVal.GetFractionPart().Numerator.Equals(BigInteger.Zero)) { break; }

                    ++m;
                    ksum += m;
                    nLeft *= _6by7;
                    sumnLeft += nLeft;
                    sLeft *= 6;
                    sLeft -= m;
                    sLeft *= _1by7;
                    sumsLeft += sLeft;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/can-we-assume-that-xyz/
            /// </summary>
            void CanWeAssumeThatxyz()
            {
                int xmax = 100, zmax = 100, ymax = 100;
                long leastr = long.MaxValue;
                for(int x = 1; x <= xmax; x++)
                {
                    for (int y = 1; y <= ymax; y++)
                    {
                        for (int z = 1; z <= zmax; z++)
                        {
                            long number = (5 * z + 1) * (4 * z + 3 * x);
                            number *= (y + 18) * (5 * x + 6 * y);
                            if(number % x == 0) {
                                number /= x;
                                if (number % y == 0) {
                                    number /= y;
                                    if (number % z == 0) {
                                        number /= z;
                                        leastr = Math.Min(leastr, number);
                                    }
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Least r : {0}", leastr);
            }

            /// <summary>
            /// https://brilliant.org/problems/perfect-square-progression/
            /// </summary>
            void PerfectSquareProgression()
            {
                List<int> numbers = new List<int>();
                int n = 1;
                while (n * n <= 1000)
                {
                    numbers.Add(n * n);
                    ++n;
                }
                List<IList<int>> combinations = Combinations.GetAllCombinations(numbers, 3);
                int count = 0;
                foreach (IList<int> c in combinations)
                {
                    List<int> set = c.ToList();
                    set.Sort();
                    if ((set[1] - set[0]) == (set[2] - set[1])) count++;
                }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/number-of-solutions-5/
            /// </summary>
            [SearchKeyword("Incorrect")]
            void NumberOfSolutionsSineEquation()
            {
                int n = 1;
                int totalSolutions = 0;
                int nMax = 2007;
                while (++n <= nMax)
                {
                    int np1 = n + 1;
                    int k = -1;
                    int counter = 2;
                    if ((n - 1) % 4 == 0) counter++;
                    while (++k >= 0)
                    {
                        int _4kp1 = (4 * k) + 1;
                        if (_4kp1 > np1) break;
                        double d1 = _4kp1 * 1.0 / np1;
                        if (d1 < 1.0 && 2 * _4kp1 != np1)
                        {
                            counter++;
                        }
                    }
                    QueuedConsole.WriteImmediate("F({0}) : {1}", n, counter);
                    totalSolutions += counter;
                }
                QueuedConsole.WriteImmediate("Total solutions : {0}", totalSolutions);
            }

            /// <summary>
            /// https://brilliant.org/problems/factorization-is-an-art-5/
            /// </summary>
            void FactorizationIsAnArt()
            {
                Func<int, long> FOfX = delegate (int x) {
                    long retVal = (4 * x * x) - (6 * x) + 4;
                    retVal *= x;
                    retVal -= 1;
                    return retVal;
                };

                long summation = FOfX(-2016);
                int n = 1000;
                while(++n > 0)
                {
                    if (-FOfX(n) == summation) break;
                }
                QueuedConsole.WriteFinalAnswer("y : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/help-im-trapped-in-a-math-factory/
            /// </summary>
            void TrappedInMathFactory()
            {
                int n = 0;
                BigInteger b = 1;
                while (++n <= 2015)
                {
                    b *= 2;
                }
                b -= 1;
                QueuedConsole.WriteImmediate("Unit's digit of N : {0}", b % 10);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-pirate-code/
            /// </summary>
            void PirateCode()
            {
                int n = 1;
                int sum = 1, prev = 1, curr = 0, add = 2;
                while (++n <= 10)
                {
                    curr = (prev * 3) + add;
                    sum += curr;
                    prev = curr;
                    add *= 3;
                }
                int a = 2;
                int sum1 = sum;
                while (sum1 % 3 == 0)
                {
                    sum1 /= 3;
                    a++;
                }
                QueuedConsole.WriteImmediate("a : {0}, b:{1}", a, sum1);
            }

            /// <summary>
            /// https://brilliant.org/problems/black-book-1/
            /// </summary>
            void AddingFloors()
            {
                int n = -1001;
                int count = 0;
                while (++n <= 1000)
                {
                    double x = (2 * n + 1) *1.0/ 3;

                    double rhs = ((3 * x) - 1) / 2;
                    rhs += 0.01;

                    double lhs = Math.Floor(((2 * x) + 1) / 3) + Math.Floor(((4 * x) + 5) / 6);

                    if (Math.Floor(lhs) == Math.Floor(rhs)) { count++; }
                }
                QueuedConsole.WriteImmediate("Number of solutions : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/left/
            /// </summary>
            void FindTwentyFirstDigit()
            {
                int n = 0;
                BigInteger sum = 0;
                while (++n <= 672)
                {
                    sum += n * PermutationsAndCombinations.nCrBig(2015, (3 * n) - 1);
                }

                QueuedConsole.WriteImmediate("First 21 digits : {0}", sum.ToString().Substring(0, 21));
                QueuedConsole.WriteImmediate("21st digit from left : {0}", (sum.ToString())[20]);
            }

            /// <summary>
            /// https://brilliant.org/problems/i-love-floor-function-2/
            /// </summary>
            void FloorFunction()
            {
                int n = -100000;
                int count = 0;
                double r = 1003 * 1.0 / 1001;
                while (++n <= 100000)
                {
                    double val = 1.0 * n * r;
                    if(Math.Floor(val) == n) {
                        QueuedConsole.WriteImmediate("n: {0}", n);
                        count++;
                    }
                }

                QueuedConsole.WriteImmediate("S: {0}, [S/67] : {1}", count, Math.Floor(count * 1.0 / 67));
            }

            /// <summary>
            /// https://brilliant.org/problems/floor-roof/
            /// </summary>
            void FloorRoof()
            {
                int n = -100000;
                int count = 0;
                while (++n <= 100000)
                {
                    double lhs = 100.0 * n / 101;
                    double rhs = 99.0 * n / 100;
                    if ((1+Math.Floor(lhs)) == Math.Floor(rhs))
                    {
                        count++;
                    }
                }

                QueuedConsole.WriteImmediate("S: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/dont-crash/
            /// </summary>
            void DontCrash()
            {
                double sum = 0.0;
                int n = 0;
                while (++n <= 2016)
                {
                    sum += 1.0 / n;
                }

                sum *= 1008;
                sum *= 2016;
                sum *= 6051;
                sum -= 4033;

                QueuedConsole.WriteImmediate("Sum : {0}, first 5 digits: {1}", sum, ((long)Math.Floor(sum) + "").Substring(0, 5));
            }

            /// <summary>
            /// https://brilliant.org/problems/weird-minimax/
            /// </summary>
            void WeirdMinimax()
            {
                int maxDepth = 10;
                Action<int, HashSet<int>> Add = null;
                long sum = 0;
                Add = delegate (int depth, HashSet<int> used)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        HashSet<int> next = new HashSet<int>(used);

                        if (depth == maxDepth) {
                            if (!next.Contains(10)) next.Add(10);
                            else next.Add(i);

                            sum += next.Min();
                        } else {
                            next.Add(i);
                            Add(depth + 1, next);
                        }
                    }
                };

                Add(1, new HashSet<int>());

                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/recurring-theme-part-n1/
            /// </summary>
            void RecurringTheme()
            {
                double ai = 10.0, aip1 = 6.0, aip2 = 0.0;
                int n = 0;
                double anMax = ai;
                while (++n <= 10000000)
                {
                    aip2 = ((6 * aip1) - (5 * ai)) / 5.0;

                    anMax = Math.Max(aip2, anMax);
                    ai = aip1;
                    aip1 = aip2;
                }
                QueuedConsole.WriteImmediate("Max. an : {0}", anMax);
            }

            /// <summary>
            /// https://brilliant.org/problems/four-lets-and-three-sets-are-perfect-with-three/
            /// </summary>
            void FourLetsAndThreeSets()
            {
                int x0 = 192;
                int nmax = 1000;
                int zLeast = int.MaxValue;
                for (int a = 1; a <= nmax; a++)
                {
                    int val1 = 3 * a * a + 237;
                    for (int b = 1; b <= nmax; b++)
                    {
                        int val2 = val1 + (27 * b * b) - (18 * a * b);
                        for (int c = 1; c <= nmax; c++)
                        {
                            int val3 = (5 * c * (c - 6)) + val2;
                            if (val3 == x0)
                            {
                                zLeast = Math.Min((a + b + c), zLeast);
                            }
                        }
                    }
                }

                QueuedConsole.WriteImmediate("z least: {0}", zLeast);
            }

            /// <summary>
            /// https://brilliant.org/problems/every-third-binomial-671-times/
            /// </summary>
            void EveryThirdBinomial()
            {
                int n = 1;
                BigInteger sum = 0;
                while(n <= 2014)
                {
                    sum += PermutationsAndCombinations.nCrBig(2014, n);
                    n += 3;
                }

                QueuedConsole.WriteImmediate("Remainder : {0}", sum % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/parity-does-matter/
            /// </summary>
            void OddCoefficients()
            {
                int n = -1;
                int count = 0;
                while (++n <= 2015)
                {
                    if (PermutationsAndCombinations.nCrBig(2015, n) % 2 != 0) { count++; }
                }

                QueuedConsole.WriteImmediate("Number of odd coeff : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/series-jee-2/
            /// </summary>
            void SumSeries()
            {
                int n = 2, next = 3;
                int count = 0;
                BigRational sum = 0;
                while (++count <= 50000)
                {
                    sum += new BigRational(1, n);
                    n += next;

                    next++;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", NumberConverter.BigRationalToDouble(sum));
            }

            /// <summary>
            /// https://brilliant.org/problems/numbers-and-numbers-4/
            /// </summary>
            void NumbersAndNumbers()
            {
                int nSequences = 0;
                for (int a1 = 1; a1 <= 10; a1++)
                {
                    for (int a2 = 1; a2 <= 10; a2++)
                    {
                        for (int a3 = 1; a3 <= 10; a3++)
                        {
                            int snCount = 0;
                            int an0count = 0;

                            BigInteger anm1 = (long)a3, anm2 = (long)a2, anm3 = (long)a1;
                            BigInteger an = 0;
                            while (++snCount <= 20)
                            {
                                BigInteger anm2manm3 = anm2 - anm3;

                                if (anm2manm3.Sign < 0) anm2manm3 *= -1;
                                an = anm1 * anm2manm3;

                                if (an == 0)
                                {
                                    an0count++;
                                    if (an0count == 1)
                                    {
                                        nSequences++;
                                        break;
                                    }
                                }
                                anm3 = anm2;
                                anm2 = anm1;
                                anm1 = an;
                            }
                        }
                    }
                }

                QueuedConsole.WriteImmediate("Number of sequences having atleast one an = 0 is :: {0}", nSequences);
            }

            /// <summary>
            /// https://brilliant.org/problems/functions-can-you-solve-this/
            /// </summary>
            void CanYouSolveThis()
            {
                int n = 0;
                int sign = -1;
                BigInteger sum = 0;
                BigInteger fl = 0, flprev = 0;
                int fr = 0;
                while (++n <= 2008)
                {
                    flprev = fl;
                    fr = Math.Abs(fr) + 1;
                    fr *= sign;

                    sign *= -1;
                    fl = sign * n;
                    fl += (-2) * flprev;

                    if (n <= 2007) { sum += fl; }
                }
                QueuedConsole.WriteImmediate("f2009: left term: {0}, right term: {1} ", fl, fr);
                QueuedConsole.WriteImmediate("sum till f2008 of left term: {0}", sum);
                QueuedConsole.WriteImmediate("value: {0}", sum * 3 + fl);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-very-average-bunch-of-numbers/
            /// </summary>
            void AVeryAverageBunchOfNumbers()
            {
                int xnleft = 0, xnright = 1;
                int xnm1left = 1, xnm1right = 0;

                int n = 1;
                int xn343434left = 0, xn343434right = 0;
                int xn515151left = 0, xn515151right = 0;
                while (++n <= 515151)
                {
                    int xnp1left = 2 * xnleft - xnm1left;
                    int xnp1right = 2 * xnright - xnm1right;

                    if (n == 343434)
                    {
                        xn343434left = xnp1left;
                        xn343434right = xnp1right;
                    }
                    if (n == 515151)
                    {
                        xn515151left = xnp1left;
                        xn515151right = xnp1right;
                    }

                    xnm1left = xnleft; xnm1right = xnright;
                    xnleft = xnp1left; xnright = xnp1right;
                }

                QueuedConsole.WriteImmediate("x at 343434 = ({0}).a + ({1}).b", xn343434left, xn343434right);
                QueuedConsole.WriteImmediate("x at 515151 = ({0}).a + ({1}).b", xn515151left, xn515151right);
            }

            /// <summary>
            /// https://brilliant.org/problems/sum-of-gcds-2/
            /// </summary>
            void SumOfGCDs()
            {
                HashSet<long> set = new HashSet<long>();
                long n = 1;
                long prev = 51;
                while (++n <= 100000)
                {
                    long cur = (n * n) + 50;

                    set.Add(MathFunctions.GCD(cur, prev));

                    prev = cur;
                }
                QueuedConsole.WriteImmediate("Sum of all distinct elements : {0}", set.Sum());
            }

            /// <summary>
            /// https://brilliant.org/problems/youre-floored/
            /// </summary>
            void YoureFloored()
            {
                BigInteger b31 = 1, b93 = 1;
                Enumerable.Range(1, 93).ForEach(x =>
                {
                    if (x <= 31) b31 *= 10;
                    b93 *= 10;
                });

                BigRational r = new BigRational(b93, b31 + 3);
                BigInteger gI = r.GetWholePart();
                string lastTwo = gI.ToString();
                lastTwo = lastTwo.Substring(lastTwo.Length - 2, 2);

                int sumDigits = (int)(char.GetNumericValue(lastTwo[0]) + char.GetNumericValue(lastTwo[1]));
                QueuedConsole.WriteImmediate("sum of digits : {0}", sumDigits);
            }

            /// <summary>
            /// https://brilliant.org/problems/youre-floored-again/
            /// </summary>
            void YoureFlooredAgain()
            {
                BigInteger b100 = 1, b20000 = 1;
                Enumerable.Range(1, 20000).ForEach(x =>
                {
                    if (x <= 100) b100 *= 10;
                    b20000 *= 10;
                });

                BigRational r = new BigRational(b20000, b100 + 3);
                BigInteger gI = r.GetWholePart();
                string lastTwo = gI.ToString();

                QueuedConsole.WriteImmediate("units digit : {0}", lastTwo[lastTwo.Length - 1]);
            }

            /// <summary>
            /// https://brilliant.org/problems/p-and-q-are-integers/
            /// </summary>
            void PQAreIntegers()
            {
                int n = 11, r = 0;
                double sum = 0.0, p = 0.34;
                while(r <= n)
                {
                    sum += (r * r) * (PermutationsAndCombinations.nCr(n, r)) * Math.Pow(p, r) * Math.Pow(1 - p, n - r);
                    ++r;
                }
                QueuedConsole.WriteImmediate("Sum: {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-problem-by-syed-baqir-7/
            /// </summary>
            void ExactCheque()
            {
                int b = 0;
                while (++b <= 1000)
                {
                    int n = 9999 * b + 5;
                    int a = 199 * b + 5;
                    if (n % 98 == 0 && a % 98 == 0)
                    {
                        a /= 98;
                        n /= 98;
                        if (a < 100) QueuedConsole.WriteImmediate("Cheque amount: {0} cents, b :{1}", n, b);
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/back-bigger-better-stronger/
            /// </summary>
            void BackBiggerBetterStronger()
            {
                Dictionary<int, int> FValues = new Dictionary<int, int>();
                FValues[1] = 2; FValues[2] = 3; FValues[3] = 4;
                Func<int, int?> Fx = delegate (int x)
                {
                    if (!FValues.ContainsKey(x)) return null;
                    return FValues[x];
                };

                Func<int, int?> InverseFx = delegate (int x)
                {
                    if (!FValues.ContainsValue(x)) return null;
                    return FValues.Where(p => p.Value == x).Select(p => p.Key).First();
                };

                Action<int> AddMissing = delegate (int y)
                {
                    if (y > 3171) return;

                    int t = y, tmax = FValues.Keys.Max();

                    while (t <= tmax)
                    {
                        int? fx = InverseFx(t);
                        int? ffx = null;
                        if (fx.HasValue) ffx = InverseFx(fx.Value);
                        if (ffx.HasValue)
                        {
                            if (!FValues.ContainsKey(t)) FValues.AddOnce(t, 4 * ffx.Value);
                        }
                        t++;
                    }

                    t = y;
                    while (t < tmax)
                    {
                        if (FValues.ContainsKey(t))
                        {
                            int t1 = FValues[t];
                            if (!FValues.ContainsKey(t + 1)) FValues.Add(t + 1, t1 + 1);
                        }
                        t++;
                    }
                };

                int n = 2;
                while (!FValues.ContainsKey(3171))
                {
                    int? fx = Fx(n);
                    int? ffx = null, fffx = null;
                    if (fx.HasValue) ffx = Fx(fx.Value);
                    if (ffx.HasValue) fffx = Fx(ffx.Value);
                    int fffn = 4 * n;
                    
                    AddMissing(n);

                    if (!fx.HasValue) fx = Fx(n);
                    if (fx.HasValue && !ffx.HasValue) ffx = Fx(fx.Value);
                    if (ffx.HasValue)
                    {
                        FValues.AddOnce(ffx.Value, fffn);
                    }

                    ++n;
                }

                QueuedConsole.WriteImmediate("f(3171): {0}", FValues[3171]);
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-read-my-mind/
            /// </summary>
            void FibonacciSums()
            {
                for (int a = 1; a <= 50; a++)
                {
                    int _89a = 89 * a;
                    for (int b = 1; b <= 30; b++)
                    {
                        int _144b = 144 * b;
                        if(_89a + _144b == 3935)
                        {
                            QueuedConsole.WriteImmediate("a:{0}, b:{1}, a*b: {2}", a, b, a * b);
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/nmtc-triiicky-problems-2/
            /// </summary>
            void Nmtc2()
            {
                Func<int, int> Fn = delegate (int a)
                {
                    double sqrt = Math.Sqrt(a);
                    int asqVal = (int)sqrt;
                    if (sqrt - asqVal > 0.5) asqVal += 1;

                    return asqVal;
                };

                int n = 1;
                double sum = 0;
                while (++n <= 2008)
                {
                    sum += 1.0 / Fn(n);
                }
                QueuedConsole.WriteImmediate("Sum: {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/nmtc-triiicky-problems-1/
            /// </summary>
            void Nmtc1()
            {
                BigInteger P = 1, Q = 2009 + (2008 * 2008);
                Enumerable.Range(1, 2007).ForEach(x =>
                {
                    P *= 2008;
                });
                P -= 2008;
                QueuedConsole.WriteImmediate("Remainder: {0}", (P % Q).ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/wee-hours/
            /// </summary>
            void WeeHours()
            {
                List<int> XValues = new List<int>();
                double error = 0.000001, leastVal = double.MaxValue;
                Enumerable.Range(1, 36000000).ForEach(x =>
                {
                    double t = x / 10000.0;

                    double A = (t / 120.0) + 90;
                    double B = t / 10.0;
                    double C = 6 * (t % 60);

                    double lhs = Math.Abs(C-B);
                    double rhs = Math.Abs(A-B);

                    if(Math.Abs(lhs - rhs) < error)
                    {
                        XValues.Add((int)Math.Round(t));
                    }

                    leastVal = Math.Min(leastVal, Math.Abs(lhs - rhs));
                });

                QueuedConsole.WriteImmediate("Least difference: {0}", leastVal);

                XValues.ForEach(y =>
                {
                    QueuedConsole.WriteImmediate("x: {0}, in minutes: {1}", y, y / 60.0);
                });
            }

            /// <summary>
            /// https://brilliant.org/problems/compare-a-and-b/
            /// </summary>
            void Compareaandb()
            {
                BigRational a = new BigRational(0, 1);
                BigRational b = new BigRational(0, 1);

                int sign = -1;
                Enumerable.Range(1, 2000).ForEach(x =>
                {
                    sign *= -1;
                    BigRational add = new BigRational(1, x);
                    a += sign * add;
                    if (x >= 1000) b += add;
                });

                if (a > b) { QueuedConsole.WriteImmediate("a is > b"); }
                else if (a == b) { QueuedConsole.WriteImmediate("a = b"); }
                else if (a < b) { QueuedConsole.WriteImmediate("a is < b"); }

                double aVal = NumberConverter.BigRationalToDouble(a);
                double bVal = NumberConverter.BigRationalToDouble(b);

                QueuedConsole.WriteImmediate("a:{0}, b:{1}", aVal, bVal);
            }
        }

        internal class Collection2
        {
            internal void Solve()
            {
                FamiliarRecurrence();
            }

            /// <summary>
            /// https://brilliant.org/problems/something-looks-familiar-about-this-recurrence/
            /// </summary>
            void FamiliarRecurrence()
            {
                Func<Tuple<BigInteger, bool>, Tuple<BigInteger, bool>, List<Tuple<BigInteger, bool>>> Fx1 = delegate (Tuple<BigInteger, bool> p1, Tuple<BigInteger, bool> p0)
                {
                    List<Tuple<BigInteger, bool>> retVal = new List<Tuple<BigInteger, bool>>();
                    BigInteger t1i1 = p1.Item1;
                    bool t1i2 = p1.Item2;
                    if (p1.Item2)
                    {
                        t1i1 *= -1;
                        t1i2 = false;
                    }
                    else
                    {
                        t1i2 = true;
                    }
                    Tuple<BigInteger, bool> t1 = new Tuple<BigInteger, bool>(t1i1, t1i2);
                    retVal.Add(t1);

                    BigInteger t2i1 = p0.Item1;
                    bool t2i2 = p0.Item2;
                    if (p0.Item2)
                    {
                        t2i1 *= -1;
                        t2i2 = false;
                    }
                    else
                    {
                        t2i2 = true;
                    }
                    Tuple<BigInteger, bool> t2 = new Tuple<BigInteger, bool>(t2i1, t2i2);
                    retVal.Add(t2);

                    return retVal;
                };

                Func<Tuple<BigInteger, bool>, Tuple<BigInteger, bool>, List<Tuple<BigInteger, bool>>> Fx2 = delegate (Tuple<BigInteger, bool> p1, Tuple<BigInteger, bool> p0)
                {
                    List<Tuple<BigInteger, bool>> retVal = new List<Tuple<BigInteger, bool>>();
                    Tuple<BigInteger, bool> t1 = new Tuple<BigInteger, bool>((-1) * p1.Item1 , p1.Item2);
                    retVal.Add(t1);
                    Tuple<BigInteger, bool> t2 = new Tuple<BigInteger, bool>((-1) * p0.Item1, p0.Item2);
                    retVal.Add(t2);

                    return retVal;
                };

                int n = 1;
                List<Tuple<BigInteger, bool>> Pn = new List<Tuple<BigInteger, bool>> { new Tuple<BigInteger, bool>(1, false), new Tuple<BigInteger, bool>(0, false) };
                List<Tuple<BigInteger, bool>> Pnm1 = new List<Tuple<BigInteger, bool>> { new Tuple<BigInteger, bool>(0, false), new Tuple<BigInteger, bool>(1, false) };
                BigInteger P17 = 0, P2017 = 0;
                while (++n <= 2017)
                {
                    List<Tuple<BigInteger, bool>> Pnp11 = Fx1(Pn[0], Pn[1]);
                    List<Tuple<BigInteger, bool>> Pnp10 = Fx2(Pnm1[0], Pnm1[1]);

                    List<Tuple<BigInteger, bool>> Pnp1 = new List<Tuple<BigInteger, bool>>
                    {
                        new Tuple<BigInteger, bool>(Pnp11[0].Item1 + Pnp10[0].Item1, Pnp11[0].Item2),
                        new Tuple<BigInteger, bool>(Pnp11[1].Item1 + Pnp10[1].Item1, Pnp10[1].Item1 != 0 ? Pnp10[1].Item2 : Pnp11[1].Item2)
                    };

                    if (n == 17 || n == 2017)
                    {
                        BigInteger p = Pnp1[0].Item1 + 2 * Pnp1[1].Item1;
                        if (n == 17) P17 = p;
                        else P2017 = p;
                        QueuedConsole.WriteImmediate("n: {0}", n);
                        QueuedConsole.WriteImmediate("{0}{1}", p, "/2");
                    }
                    Pnm1 = new List<Tuple<BigInteger, bool>> { new Tuple<BigInteger, bool>(Pn[0].Item1, Pn[0].Item2), new Tuple<BigInteger, bool>(Pn[1].Item1, Pn[1].Item2) };
                    Pn = new List<Tuple<BigInteger, bool>> { new Tuple<BigInteger, bool>(Pnp1[0].Item1, Pnp1[0].Item2), new Tuple<BigInteger, bool>(Pnp1[1].Item1, Pnp1[1].Item2) };
                }
                QueuedConsole.WriteImmediate("m: {0}, n: {1}", P2017 % P17, P17);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-1-to-be-proper-this-year/
            /// </summary>
            void SumOfAllProperFractions()
            {
                double sum = 0.0;
                HashSet<string> s = new HashSet<string>();
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                Enumerable.Range(2, 2014).ForEach(d =>
                {
                    for (int n = 1; n < d; n++)
                    {
                        Fraction<int> fr = new Fraction<int>(n, d, false);
                        string key = fr.N + ":" + fr.D;
                        if (!s.Contains(key))
                        {
                            s.Add(key);
                            sum += fr.GetValue();
                        }
                    }
                });
                sw.Stop();
                QueuedConsole.WriteImmediate("{0}, time taken : {1} seconds", sum, sw.ElapsedMilliseconds * 1.0 / 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/quite-large/
            /// </summary>
            void QuiteLarge()
            {
                Func<Tuple<long, long>, Tuple<long, long>> f = delegate (Tuple<long, long> input)
                {
                    return new Tuple<long, long>(input.Item1 * 9, 2 + (input.Item2 * 9));
                };
                Func<Tuple<long, long>, Tuple<long, long>> g = delegate (Tuple<long, long> input)
                {
                    return new Tuple<long, long>(input.Item1 * 25, 6 + (input.Item2 * 25));
                };

                List<Tuple<long, long>> Functions = new List<Tuple<long, long>>();
                Action<int, Tuple<long, long>, string, int, int> r = null;
                r = delegate (int depth, Tuple<long, long> input, string fCom, int fCount, int gCount) {
                    for (int a = 0; a <= 1; a++)
                    {
                        Tuple<long, long> next = null;
                        string fcn = "" + fCom;
                        int fCN = fCount, gCN = gCount;
                        if (a == 0 )
                        {
                            if (fCN < 4) {
                                fcn += "f";
                                next = f(input);
                                fCN++;
                            } else {
                                fcn += "g";
                                next = g(input);
                                gCN++;
                            }
                        } 
                        if (a == 1)
                        {
                            if (gCN < 4) {
                                fcn += "g";
                                next = g(input);
                                fCN++;
                            } else {
                                fcn += "f";
                                next = f(input);
                                gCN++;
                            }
                        }
                        if (depth < 7)
                        {
                            r(depth + 1, next, fcn, fCN, gCN);
                        }
                        if (depth == 7)
                        {
                            if (!Functions.Any(x => x.Item1 == next.Item1 && x.Item2 == next.Item2)) Functions.Add(next);
                            QueuedConsole.WriteImmediate("{0}", string.Join("", fcn.Reverse()));
                        }
                    }
                };
                r(0, new Tuple<long, long>(1, 0), "", 0, 0);
                QueuedConsole.WriteImmediate("{0}", Functions.Select(x => x.Item2).Sum());
            }

            /// <summary>
            /// https://brilliant.org/problems/lets-go-into-the-future/
            /// </summary>
            void LetsGoIntoTheFuture()
            {
                Dictionary<int, BigInteger> Coefficients = new Dictionary<int, BigInteger>();
                int xpow = -1;
                Enumerable.Range(0, 2019).ForEach(n =>
                {
                    xpow += 1;
                    Coefficients.GenericAddOrIncrement(xpow, PermutationsAndCombinations.nCrBig(2018, n));
                });
                xpow = -2;
                Enumerable.Range(0, 2018).ForEach(n =>
                {
                    xpow += 2;
                    Coefficients.GenericAddOrIncrement(xpow, PermutationsAndCombinations.nCrBig(2017, n));
                });
                xpow = -3;
                Enumerable.Range(0, 2017).ForEach(n =>
                {
                    xpow += 3;
                    Coefficients.GenericAddOrIncrement(xpow, PermutationsAndCombinations.nCrBig(2016, n));
                });

                QueuedConsole.WriteImmediate("{0}", Coefficients.Values.Distinct().Count());
            }

            /// <summary>
            /// https://brilliant.org/problems/yet-another-easy-question-4-corrected/
            /// </summary>
            void YetAnotherEasyQuestion()
            {
                BigInteger factorial = 1;
                BigInteger b100 = 0, a100 = 0;
                Enumerable.Range(1, 100).ForEach(n =>
                {
                    factorial *= n;
                    BigInteger ai = ((n * n) + 1) * factorial;
                    b100 += ai;
                    if (n == 100) a100 = ai;
                });
                BigRational AbyB = new BigRational(a100, b100);
                QueuedConsole.WriteImmediate("B - A : {0}", AbyB.Denominator - AbyB.Numerator);
            }

            /// <summary>
            /// https://brilliant.org/problems/binomial-coefficients-modulo-2017/
            /// </summary>
            void BinomialCoefficientsModulo2017()
            {
                int mod = 0;
                Enumerable.Range(0, 63).ForEach(n =>
                {
                    mod += (int)(PermutationsAndCombinations.nCrBig(2014, n) % 2017);
                });
                mod %= 2017;
                QueuedConsole.WriteImmediate("a^2 : {0}, a : {1}", mod, Math.Pow(mod, 0.5));
            }

            /// <summary>
            /// https://brilliant.org/problems/harmonic-overdose/
            /// </summary>
            void HarmonicOverdose()
            {
                Dictionary<int, BigRational> H = new Dictionary<int, BigRational>();
                BigRational Hi = new BigRational(0, 1);
                Enumerable.Range(1, 1728).ForEach(n =>
                {
                    Hi += new BigRational(1 , n);
                    H.Add(n, new BigRational(Hi.Numerator, Hi.Denominator));
                });

                BigRational M = new BigRational(0, 1);
                BigRational E = new BigRational(0, 1);
                Enumerable.Range(1, 1728).ForEach(n =>
                {
                    M += H[n] * H[1729 - n];
                    E += 2 * (1729 - n) * H[n] / (n + 1);
                });

                BigRational Sum = 1729 * ((M / E) + (E / M));
                QueuedConsole.WriteImmediate("{0}", Sum.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/number-theory-18/
            /// </summary>
            void DoubtfulStart()
            {
                Func<double, double> Fn = delegate (double a)
                {
                    double fn = 0.0;

                    double arootm1 = Math.Sqrt(a) - 1;
                    fn = Math.Pow(((arootm1 * arootm1) + 1), 2);
                    fn -= (a / arootm1);

                    return fn;
                };

                for(int a = 0; a<=10; a++)
                {
                    if (a != 1) { QueuedConsole.WriteImmediate("a: {0}, f(a): {1}", a, Fn(a)); }
                    else {
                        double adel = a - 0.001;
                        QueuedConsole.WriteImmediate("a: {0}, f(a): {1}", adel, Fn(adel));
                        adel = a + 0.001;
                        QueuedConsole.WriteImmediate("a: {0}, f(a): {1}", adel, Fn(adel));
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/problem-on-jee-and-not-for-jee/
            /// </summary>
            void JEEPossibleScores()
            {
                int nQ = 90;
                HashSet<int> marks = new HashSet<int>();
                for(int a = 0; a <= 90; a++)
                {
                    for(int b = 0; b <= nQ - a; b++)
                    {
                        marks.Add((4 * a) - (2 * b));
                    }
                }
                QueuedConsole.WriteImmediate("{0}", marks.Sum());
            }

            /// <summary>
            /// https://brilliant.org/problems/an-elegant-function-with-elegant-conditions/
            /// </summary>
            void ElegantFunctionWithElegantConditions()
            {
                Dictionary<int, int> FnValues = new Dictionary<int, int>();
                int count = 0;
                Action<int> Setter = delegate (int n)
                {
                    if (FnValues.ContainsKey(n)) return;

                    int fnValue = 0;
                    if (n == 1) fnValue = 1;
                    else if (n == 3) fnValue = 3;
                    else if ((n - 1) % 4 == 0) {
                        int t = (n - 1) / 4;
                        fnValue = 2 * FnValues[(2 * t) + 1] - FnValues[t];
                    } else if ((n - 3) % 4 == 0) {
                        int t = (n - 3) / 4;
                        fnValue = 3 * FnValues[(2 * t) + 1] - 2 * FnValues[t];
                    }

                    if (fnValue == n) count++;
                    while (n <= 1988)
                    {
                        FnValues.AddOnce(n, fnValue);
                        n *= 2;
                    }
                };

                int a = 0;
                while (++a <= 1988) {  Setter(a); }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/do-you-dare/
            /// </summary>
            void DoYouDare()
            {
                BigInteger[,] A = new BigInteger[2, 2];
                A[0, 0] = 2; A[0, 1] = 1;
                A[1, 0] = -1; A[1, 1] = 4;

                BigInteger[,] B = Matrix.Pow(A, 2016);

                QueuedConsole.WriteImmediate("b22 % 7 = {0}", B[1, 1] % 7);
            }

            /// <summary>
            /// https://brilliant.org/problems/function-of-this-year/
            /// </summary>
            void FunctionOfThisYear()
            {
                BigRational bi = 2016;
                BigRational sum = bi;

                int N = 1;
                Enumerable.Range(2, 2015).ForEach(n =>
                {
                    bi = sum / ((n - 1) * (n + 1));
                    N++;
                    sum += bi;
                });

                QueuedConsole.WriteImmediate("f({0}) : a/b = {1}/{2}, a + b : {3}", N, bi.Numerator, bi.Denominator, bi.Numerator + bi.Denominator);
            }

            /// <summary>
            /// https://brilliant.org/problems/dominated-2-its-algebra-cum-geometry/
            /// </summary>
            void AlgebraAndGeometry()
            {
                double ai = Math.Sqrt(0.5);
                double p = ai;
                Enumerable.Range(1, 10000).ForEach(x =>
                {
                    ai = Math.Sqrt((1 + ai) / 2);
                    p *= ai;
                });
                QueuedConsole.WriteImmediate("a/b: {0}", p * Math.PI);
            }

            /// <summary>
            /// https://brilliant.org/problems/separate-the-wheat/
            /// </summary>
            void SeparateTheWheat()
            {
                Func<int, int, int, BigRational> Fn1 = delegate (int a, int b, int c)
                {
                    return new BigRational((c * (a + b) - a * b) , (a * b * c));
                };

                Func<int, int, int, BigRational> Fn2 = delegate (int a, int b, int c)
                {
                    return new BigRational((c * a + b * c + a * b), (a * b * c));
                };

                Func<int, int, int, BigRational> Fn = delegate (int a, int b, int c)
                {
                    return Fn2(a, b, c) * Fn1(a, b, c) * Fn1(c, b, a) * Fn1(a, c, b);
                };

                BigRational A = Fn(168, 175, 600);
                BigRational B = Fn(240, 260, 624);
                BigRational C = Fn(432, 540, 720);

                double sum = 0;
                sum += 1.0 / Math.Sqrt(NumberConverter.BigRationalToDouble(A));
                sum += 1.0 / Math.Sqrt(NumberConverter.BigRationalToDouble(B));
                sum += 1.0 / Math.Sqrt(NumberConverter.BigRationalToDouble(C));

                QueuedConsole.WriteImmediate("Sum: {0}, Digit sum: {1}", sum, MathFunctions.DigitSum((long)sum));
            }

            void _2016IsAwesome()
            {
                int counter = 0;
                for (int k = 1; k <= 2016; k++) {
                    double k1 = Math.Floor(2016.0 / k);
                    if(Math.Floor(2016.0/k1) == k * 1.0)
                    {
                        double k2 = Math.Ceiling(2016.0 / k);
                        if (Math.Ceiling(2016.0 / k2) == k * 1.0)
                        {
                            counter++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("{0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/functional-analysis/
            /// </summary>
            void FunctionalAnalysis()
            {
                long sum = 0;
                Dictionary<int, int> FunctionValues = new Dictionary<int, int>();
                FunctionValues.Add(0, 0); FunctionValues.Add(1, 0); FunctionValues.Add(2, 1);
                Func<int, int> Fn = delegate (int n)
                {
                    if (!FunctionValues.ContainsKey(n))
                    {
                        int ni = 1;
                        while(++ni > 0)
                        {
                            if(n % ni != 0)
                            {
                                FunctionValues.Add(n, ni);
                                break;
                            }
                        }
                    }
                    return FunctionValues[n];
                };

                int k = 1;
                while(++k <= 2017)
                {
                    sum += Fn(Fn(Fn(k)));
                }

                QueuedConsole.WriteImmediate("{0}", sum);
            }
        }

        internal class TelescopingSeries
        {
            internal void Solve()
            {
                WheresMyTelescope2();
            }

            /// <summary>
            /// https://brilliant.org/problems/wheres-my-telescope-2/
            /// </summary>
            void WheresMyTelescope2()
            {
                BigRational sumS = new BigRational(0, 1);
                BigRational sumT = new BigRational(0, 1);
                Enumerable.Range(1, 50).ForEach(n =>
                {
                    int sn = 2 * (n - 1) + 1;
                    sumS += new BigRational(1, sn * (sn + 1));
                    sumT += new BigRational(1, (50 + n) * (101 - n));
                });
                double s = NumberConverter.BigRationalToDouble(sumS);
                double t = NumberConverter.BigRationalToDouble(sumT);
                Fraction<int> fr = FractionConverter<int>.RealToFraction(s / t);
                QueuedConsole.WriteImmediate("p/q : {0}/{1}", fr.N, fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/an-algebra-problem-by-darel-gunawan/
            /// </summary>
            void _1()
            {
                BigRational sum = new BigRational(0, 1);
                Enumerable.Range(2, 99).ForEach(x =>
                {
                    sum += new BigRational(1, (x * (x - 1) * (x + 1)));
                });

                Fraction<int> fr = FractionConverter<int>.BigRationalToFraction(sum);
                QueuedConsole.WriteImmediate("Sum = a/b = {0}/{1}", fr.N, fr.D);
            }
        }

        internal class LinearEquations
        {
            internal void Solve()
            {
                DoesPatternAlwaysTrue();
            }

            /// <summary>
            /// https://brilliant.org/problems/does-pattern-always-true/
            /// </summary>
            void DoesPatternAlwaysTrue()
            {
                double[] Y = new double[5];
                Enumerable.Range(0, 5).ForEach(p => { Y[p] = p * 1.0 / (p + 1); });
                double[,] X = new double[5, 5];
                int n = -1;
                while (++n <= 4)
                {
                    int p = 1;
                    Enumerable.Range(0, 5).ForEach(m =>
                    {
                        X[n, m] = p;
                        p *= n;
                    });
                }

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                n = 1;
                double p5 = 0.0;
                Enumerable.Range(0, 5).ForEach(m =>
                {
                    p5 += n * Y[m];
                    n *= 5;
                });
                QueuedConsole.WriteImmediate("p(5) : {0}", p5);
            }

            /// <summary>
            /// https://brilliant.org/problems/application-of-equations-2/
            /// </summary>
            void ApplicationOfEquations2()
            {
                double[] Y = new double[] { 1, 3, 4, 9, 254 };
                double[,] X = new double[5, 5];
                int n = 0;
                while (++n <= 6)
                {
                    if (n == 5) ++n;

                    int p = n;
                    int xIndex = n - 1;
                    if (n == 6) xIndex = n - 2;
                    Enumerable.Range(0, 5).ForEach(m =>
                    {
                        X[xIndex, m] = p;
                        p *= n;
                    });
                }

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                double x = 0.0;
                n = 5;
                Enumerable.Range(0, 5).ForEach(m =>
                {
                    x += n * Y[m];
                    n *= 5;
                });
                QueuedConsole.WriteImmediate("x : {0}", x);
            }

            /// <summary>
            /// https://brilliant.org/problems/what-is-anwesha-biswas-tithis-student-id/
            /// </summary>
            void FindStudentId()
            {
                double[] Y = new double[] { 6, 6, 8 };
                double[,] X = new double[3, 3];
                X[0, 0] = -3; X[0, 1] = 2; X[0, 2] = -6;
                X[1, 0] = 5; X[1, 1] = 7; X[1, 2] = -5;
                X[2, 0] = 1; X[2, 1] = 4; X[2, 2] = -2;

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                QueuedConsole.WriteImmediate("(x,y,z) : {0}", string.Join(",", Y));
                QueuedConsole.WriteImmediate("x + y : {0} , z : {1}", Y[0] + Y[1], Y[2]);
            }

            /// <summary>
            /// https://brilliant.org/problems/think-it-the-other-way/
            /// </summary>
            void ThinkOfItTheOtherWay()
            {
                double[] Y = new double[] { 0, -30, -240, -1020, -3120 };
                double[,] X = new double[5, 5];
                X[0, 0] = 1; X[0, 1] = 1; X[0, 2] = 1; X[0, 3] = 1; X[0, 4] = 1;
                X[1, 0] = 16; X[1, 1] = 8; X[1, 2] = 4; X[1, 3] = 2; X[1, 4] = 1;
                X[2, 0] = 81; X[2, 1] = 27; X[2, 2] = 9; X[2, 3] = 3; X[2, 4] = 1;
                X[3, 0] = 256; X[3, 1] = 64; X[3, 2] = 16; X[3, 3] = 4; X[3, 4] = 1;
                X[4, 0] = 625; X[4, 1] = 125; X[4, 2] = 25; X[4, 3] = 5; X[4, 4] = 1;

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                QueuedConsole.WriteImmediate("8bcd/125ae : {0}", Math.Abs(8 * Y[1] * Y[2] * Y[3] / (125 * Y[0] * Y[4])));
            }

            /// <summary>
            /// https://brilliant.org/practice/remainder-factor-theorem-evaluate-roots/?problem=polynomial-reciprocal-on-integers/
            /// https://brilliant.org/problems/polynomial-reciprocal-on-integers/
            /// </summary>
            void PolynomialReciprocal()
            {
                double[] Y = new double[] { 1.0, 1.0 / 2, 1.0 / 3, 1.0 / 4, 1.0 / 5 };
                double[,] X = new double[5, 5];
                X[0, 0] = 1; X[0, 1] = 1; X[0, 2] = 1; X[0, 3] = 1; X[0, 4] = 1;
                X[1, 0] = 16; X[1, 1] = 8; X[1, 2] = 4; X[1, 3] = 2; X[1, 4] = 1;
                X[2, 0] = 81; X[2, 1] = 27; X[2, 2] = 9; X[2, 3] = 3; X[2, 4] = 1;
                X[3, 0] = 256; X[3, 1] = 64; X[3, 2] = 16; X[3, 3] = 4; X[3, 4] = 1;
                X[4, 0] = 625; X[4, 1] = 125; X[4, 2] = 25; X[4, 3] = 5; X[4, 4] = 1;

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                Fraction<int> fr = FractionConverter<int>.RealToFraction(Y[4]);
                QueuedConsole.WriteImmediate("a/b : {0}, fraction : {1}/{2}, a + b : {3}", Y[4], fr.N, fr.D, fr.N + fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-polynomial-problem/
            /// </summary>
            void APolynomialProblem()
            {
                double[] Y = new double[9];
                Enumerable.Range(1, 9).ForEach(x => { Y[x - 1] = 1.0 / (1.0 * x); });

                double[,] X = new double[9, 9];
                Enumerable.Range(1, 9).ForEach(x => {
                    int k = 1;
                    Enumerable.Range(1, 9).ForEach(y =>
                    {
                        X[x - 1, y - 1] = k;
                        k *= x;
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                double P10 = 0.0;
                int t = 1;
                Enumerable.Range(1, 9).ForEach(x =>
                {
                    P10 += t * Y[x - 1];
                    t *= 10;
                });

                QueuedConsole.WriteImmediate("P10 : {0}", P10);
            }

            /// <summary>
            /// https://brilliant.org/problems/do-you-know-the-reverse-process-2/
            /// </summary>
            void PolynomialCoefficients()
            {
                double[] Y = new double[] { 51, 111, 213, 231 };
                double[,] X = new double[4, 4];
                X[0, 0] = 9; X[0, 1] = 5; X[0, 2] = 3; X[0, 3] = 2;
                X[1, 0] = 35; X[1, 1] = 13; X[1, 2] = 5; X[1, 3] = 2;
                X[2, 0] = 91; X[2, 1] = 25; X[2, 2] = 7; X[2, 3] = 2;
                X[3, 0] = 125; X[3, 1] = 25; X[3, 2] = 5; X[3, 3] = 1;

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                double sum = 0.0, product = 1.0;
                Enumerable.Range(0, 4).ForEach(y =>
                {
                    sum += Y[y];
                    product *= Y[y];
                });
                QueuedConsole.WriteImmediate("value : {0}", sum * product);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-quintic-polynomial/
            /// </summary>
            void AQuinticPolynomial()
            {
                int n = 6;
                BigRational[] Y = new BigRational[n];
                BigRational[,] X = new BigRational[n, n];

                Enumerable.Range(1, n).ForEach(x =>
                {
                    int t = 5 * x + 3;
                    string y = string.Empty;
                    string xp = string.Empty;
                    Enumerable.Range(0, t).ForEach(s => { y += "7"; });
                    Enumerable.Range(0, x).ForEach(s => { xp += "7"; });

                    BigInteger yVal = BigInteger.Parse(y);
                    Y[x - 1] = yVal;

                    BigInteger xVal = BigInteger.Parse(xp);
                    BigInteger xpow = 1;
                    Enumerable.Range(0, n).ForEach(s =>
                    {
                        xpow *= xVal;
                        X[x - 1, s] = xpow;
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                BigRational sum = 0.0;

                Enumerable.Range(0, n).ForEach(y =>
                {
                    sum += Y[y];
                });

                double f1 = NumberConverter.BigRationalToDouble(sum);
                QueuedConsole.WriteImmediate("value : {0}", f1);
            }

            /// <summary>
            /// https://brilliant.org/problems/cazzy-function/
            /// </summary>
            void CrazyFunction()
            {
                int n = 6;
                BigRational[] Y = new BigRational[n];
                BigRational[,] X = new BigRational[n, n];

                Enumerable.Range(0, n).ForEach(x =>
                {
                    Y[x] = x;
                    int xpow = (int)Math.Pow(2, x);
                    long coeff = 1;
                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x, y] = coeff;
                        coeff *= xpow;
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                double cx = NumberConverter.BigRationalToDouble(Y[1]);
                QueuedConsole.WriteImmediate("Coefficient of x : {0}", cx);

                Fraction<int> fr = FractionConverter<int>.BigRationalToFraction(Y[1]);
                QueuedConsole.WriteImmediate("p/q: {0}/{1}", fr.N, fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/oh-how-long-it-takes/
            /// </summary>
            void HowLongItTakes()
            {
                int n = 4;
                BigRational[] Y = new BigRational[n];
                BigRational[,] X = new BigRational[n, n];

                Enumerable.Range(0, n).ForEach(x =>
                {
                    Y[x] = 1;
                    int a = 2 * (x + 1);
                    a *= a;
                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        int b = (2 * y) + 1;
                        b *= b;

                        X[x, y] = new BigRational(1, a - b);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                BigRational sum = new BigRational(0, 1);
                Y.ForEach(y => { sum += y; });

                double sVal = NumberConverter.BigRationalToDouble(sum);
                QueuedConsole.WriteImmediate("Sum: {0}", sVal);
            }

            /// <summary>
            /// https://brilliant.org/problems/same-old-same-old/
            /// </summary>
            void SameOldSameOld()
            {
                int n = 5;
                double[] Y = new double[] { 4, 9, 20, 44, 88 };
                double[,] X = new double[n, n];

                Enumerable.Range(1, n).ForEach(x =>
                {
                    Y[x - 1] -= Math.Pow(x, 5);

                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x - 1, y] = Math.Pow(x, n - 1 - y);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                double f6 = Math.Pow(6, n);
                Enumerable.Range(0, n).ForEach(x =>
                {
                    f6 += Y[x] * Math.Pow(6, n - 1 - x);
                });

                QueuedConsole.WriteImmediate("value : {0}", f6);
            }

            /// <summary>
            /// https://brilliant.org/problems/polynomial-powered-by-2/
            /// </summary>
            void PoweredByTwo()
            {
                int n = 9;
                double[] Y = new double[n];
                double[,] X = new double[n, n];

                Enumerable.Range(0, n).ForEach(x =>
                {
                    Y[x] = Math.Pow(2, x);

                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x, y] = Math.Pow(x, n - 1 - y);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                double f9 = 0;
                Enumerable.Range(0, n).ForEach(x =>
                {
                    f9 += Y[x] * Math.Pow(n, n - 1 - x);
                });

                QueuedConsole.WriteImmediate("P(9) : {0}", f9);
            }

            /// <summary>
            /// https://brilliant.org/problems/an-algebra-problem-by-sompong-chuisurichy/
            /// </summary>
            void CubicEquationWithComplexCoefficients()
            {
                int n = 3;
                double[] Y = new double[n];
                double[,] X = new double[n, n];

                Enumerable.Range(0, n).ForEach(x =>
                {
                    Y[x] = (x + 1) - Math.Pow(x + 1, 3);

                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x, y] = Math.Pow(x + 1, n - 1 - y);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                double f4 = Math.Pow(4, n);
                Enumerable.Range(0, n).ForEach(x =>
                {
                    f4 += Y[x] * Math.Pow(4, n - 1 - x);
                });

                QueuedConsole.WriteImmediate("P(4) : {0}", f4);
            }

            /// <summary>
            /// https://brilliant.org/problems/double-polynomials/
            /// </summary>
            void DoublePolynomials()
            {
                int n = 3;
                double[] Y = new double[] { 13, 97, 349 };
                double[,] X = new double[n, n];

                Enumerable.Range(1, n).ForEach(x =>
                {
                    int r = 2 * x - 1;
                    Y[x - 1] -= 2 * Math.Pow(r, 3);

                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x - 1, y] = Math.Pow(r, n - 1 - y);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                string px = string.Empty;
                Enumerable.Range(0, n).ForEach(x =>
                {
                    px += " + " + Y[x] + "x^" + x;
                });
                px += " + 2x^3";
                QueuedConsole.WriteImmediate("p(x) : {0}", px);
            }

            /// <summary>
            /// https://brilliant.org/problems/fibonacci-polynomial/
            /// </summary>
            void FibonacciPolynomial()
            {
                int n = 14;
                BigRational[] Y = new BigRational[n];
                BigRational[,] X = new BigRational[n, n];

                FibonacciGenerator fg = new FibonacciGenerator(0, 1);
                while (fg.CurrentN() < n) fg.Next();

                Enumerable.Range(15, n).ForEach(x =>
                {
                    Y[x - 15] = new BigRational(fg.Next(), 1);
                    BigInteger coeff = 1;
                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x - 15, y] = new BigRational(coeff, 1);
                        coeff *= x;
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                BigRational P29 = new BigRational(0,1);
                BigInteger xpow29 = 1;
                Enumerable.Range(0, n).ForEach(y =>
                {
                    P29 += Y[y] * xpow29;
                    xpow29 *= 29;
                });

                double P29val = NumberConverter.BigRationalToDouble(P29);
                QueuedConsole.WriteImmediate("P29: {0}", P29val);
            }

            /// <summary>
            /// https://brilliant.org/problems/you-dont-have-to-find-the-as/
            /// </summary>
            void YouDontHaveToFindA()
            {
                int n = 10;
                double[] Y = new double[n];
                double[,] X = new double[n, n];

                Enumerable.Range(0, n).ForEach(x =>
                {
                    Y[x] = Math.Pow(Math.E, x + 1);
                    long coeff = 1;
                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x, y] = coeff;
                        coeff *= (x + 1);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                double x11 = 0.0;
                long _11pow = 1;
                Enumerable.Range(0, n).ForEach(x =>
                {
                    x11 += Y[x] * _11pow;
                    _11pow *= 11;
                });

                QueuedConsole.WriteImmediate("floor(x11): {0}", Math.Floor(x11));
            }

            /// <summary>
            /// https://brilliant.org/problems/a-question-on-polynomials-part-3/
            /// </summary>
            void AQuestionOnPolynomials()
            {
                int n = 101;
                BigRational[] Y = new BigRational[n];
                BigRational[,] X = new BigRational[n, n];

                BigInteger _2pow = 1;
                BigInteger xpow = 1;
                Enumerable.Range(1, n).ForEach(x =>
                {
                    _2pow *= 2;
                    Y[x - 1] = x * _2pow;

                    BigInteger xVal = x;
                    xpow = 1;
                    Enumerable.Range(0, n).ForEach(s =>
                    {
                        X[x - 1, s] = xpow;
                        xpow *= xVal;
                    });
                });

                BigInteger _1022pow102 = 204 * _2pow;

                GenericDefs.Functions.LinearEquations.Solve(X, Y);
                BigRational _f102 = 0.0;

                xpow = 1;
                Enumerable.Range(0, n).ForEach(y =>
                {
                    _f102 += xpow * Y[y];
                    xpow *= 102;
                });

                BigRational A = _1022pow102 - _f102;

                double f1 = NumberConverter.BigRationalToDouble(A);
                QueuedConsole.WriteImmediate("value : {0}", f1);
            }
        }

        internal class VietaAndNewtonIdentities
        {
            internal void Solve()
            {
                //System.Threading.Thread t = new System.Threading.Thread(WannaBashIt_2, 10000000);
                //t.Start();
                //t.Join();
                ApplicationOfEquations1();
            }

            /// <summary>
            /// https://brilliant.org/problems/application-of-equations-1/
            /// </summary>
            void ApplicationOfEquations1()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 1, -1, -1, -1 });
                QueuedConsole.WriteImmediate("P10 - P9 - P8 : {0}", neq.NewtonsIdentities(10) - neq.NewtonsIdentities(9) - neq.NewtonsIdentities(8));
            }

            /// <summary>
            /// https://brilliant.org/problems/have-you-learnt-newton-sum/
            /// </summary>
            void HaveYouLearntNewtonSum()
            {
                QuadraticEquation.NewtonsIdentities id = new QuadraticEquation.NewtonsIdentities();
                double P10 = QuadraticEquation.NthNewtonsIdentities(5, -6, -2, 10, id);
                double P5 = QuadraticEquation.NthNewtonsIdentities(5, -6, -2, 5, id);
                QueuedConsole.WriteImmediate("P5 + P10 : {0}", P5 + P10);
            }

            /// <summary>
            /// x^2 + x - 1 = 0; Find P7;
            /// https://brilliant.org/problems/perhaps-you-call-it-newtons-sum/
            /// </summary>
            void PerhapsYouCallItNewtonsSum()
            {
                QuadraticEquation.NewtonsIdentities id = new QuadraticEquation.NewtonsIdentities();
                double P7 = QuadraticEquation.NthNewtonsIdentities(1, 1, -1, 7, id);
                QueuedConsole.WriteImmediate("P7 : {0}", P7);
            }

            /// <summary>
            /// https://brilliant.org/problems/newtons-sums/
            /// </summary>
            void NewtonsSums()
            {
                CubicEquation.NewtonsIdentities id = new CubicEquation.NewtonsIdentities();
                double P4 = CubicEquation.NthNewtonsIdentities(1, 3, 4, -8, 4, id);
                double P2 = CubicEquation.NthNewtonsIdentities(1, 3, 4, -8, 2, id);
                QueuedConsole.WriteImmediate("P2 + P4 : {0}", P2 + P4);
            }

            /// <summary>
            /// https://brilliant.org/problems/shall-we-use-newtons-sum-2-2/
            /// </summary>
            void ShallWeUseNewtonsSums2()
            {
                int n = 0;
                double sum = 0;
                int nMax = 1000000;
                CubicEquation.NewtonsIdentities id = new CubicEquation.NewtonsIdentities();
                while (true)
                {
                    sum += CubicEquation.NthNewtonsIdentities(245, -287, 99, -9, n, id);
                    if (++n > nMax) break;
                }
                QueuedConsole.WriteImmediate("m/n : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/earthquake-in-my-mind/
            /// </summary>
            void EarthquakeInMyMind()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(5, new double[] { 1, 0, 0, 0, 7, -1 });
                QueuedConsole.WriteImmediate("Value of expression : {0} ", neq.NewtonsIdentities(8) * neq.NewtonsIdentities(11));
            }

            /// <summary>
            /// https://brilliant.org/problems/system-of-sums-of-powers/
            /// </summary>
            void SystemOfSumsPowers()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 6, -6, -3, -1 });
                CubicEquation.NewtonsIdentities id = new CubicEquation.NewtonsIdentities();
                int n = 4;
                while (true)
                {
                    if (neq.NewtonsIdentities(n) % 1 == 0) break;
                    n++;
                }
                QueuedConsole.WriteImmediate("Least n > 3 : {0} ", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/not-just-vietas-derivatives/
            /// </summary>
            void NotJustVietaDerivatives()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(4, new double[] { 1, -1, -1, 0, -1 });
                double sumzisq = neq.NewtonsIdentities(2);
                double sumzi = neq.NewtonsIdentities(1);
                QueuedConsole.WriteImmediate("Sum zi^2 : {0}, sum zi : {1} ", sumzisq, sumzi);
                QueuedConsole.WriteImmediate("Value of expression : {0} ", sumzisq - sumzi + 1.0);
            }

            /// <summary>
            /// https://brilliant.org/practice/polynomial-arithmetic-level-4-5-challenges/?p=3
            /// https://brilliant.org/problems/isnt-100000-such-a-huge-number-part-11/
            /// </summary>
            void RootsofUnity()
            {
                int N = 124;
                double[] coeff = new double[125];
                Enumerable.Range(0, 125).ForEach(x => { coeff[x] = 1; });
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(N, coeff);

                double sum = 0.0;
                int t = -1;
                for(int i = 0; i <= 100000; i++)
                {
                    t *= -1;
                    sum += t * neq.NewtonsIdentities(i);
                }
                QueuedConsole.WriteImmediate("Value of expression : {0} ", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/but-i-only-know-a2b2/
            /// 
            /// </summary>
            void P7()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(2, new double[] { 2, -2, -1 });
                double P7 = neq.NewtonsIdentities(7);
                Fraction<int> fr = FractionConverter<int>.RealToFraction(P7, 0.00000001);
                QueuedConsole.WriteImmediate("P7 : {0}, In fraction : {1}/{2}", P7, fr.N, fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/newtons-sums/
            /// https://brilliant.org/practice/vietas-formula-level-4-5-challenges/?problem=newtons-sums&subtopic=advanced-polynomials&chapter=vietas-formula-2
            /// </summary>
            void P2PlusP4()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 1, 3, 4, -8 });
                QueuedConsole.WriteImmediate("P2 + P4 : {0}", neq.NewtonsIdentities(2) + neq.NewtonsIdentities(4));
            }

            /// <summary>
            /// https://brilliant.org/problems/raised-to-power-9/
            /// https://brilliant.org/practice/vietas-formula-level-4-5-challenges/?problem=raised-to-power-9&subtopic=advanced-polynomials&chapter=vietas-formula-2
            /// </summary>
            void RaisedToPower9()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 1, 0, 3, 9 });
                QueuedConsole.WriteImmediate("P9 : {0}", neq.NewtonsIdentities(9));
            }

            /// <summary>
            /// https://brilliant.org/problems/what-about-this-2-2/
            /// https://brilliant.org/problems/vietas-is-fun-so-ill-post-another-problem/
            /// </summary>
            void WhatAboutThis()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 1, -7, 5, 2 });
                double P3 = neq.NewtonsIdentities(3);
                double P4 = neq.NewtonsIdentities(4);
                double P5 = neq.NewtonsIdentities(5);
                double P6 = neq.NewtonsIdentities(6);
                QueuedConsole.WriteImmediate("P3 : {0}", P3);
                QueuedConsole.WriteImmediate("P4 : {0}", P4);
                QueuedConsole.WriteImmediate("P5 : {0}", P5);
                QueuedConsole.WriteImmediate("P6 : {0}", P6);
                double value = (2 * P5) - (2 * P3) - (10 * P4);
                value /= 8.0;
                QueuedConsole.WriteImmediate("Problem1 Answer: {0}", value);
                value = 2 * P6 - 2 * P4 - 10 * P5;
                value /= 8.0;
                QueuedConsole.WriteImmediate("Problem2 Answer: {0}", value);
            }

            /// <summary>
            /// https://brilliant.org/problems/such-power-much-scare-many-wow/
            /// </summary>
            void SuchPowerMuchScare()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 1, -4, 6, -5 });
                double P0 = neq.NewtonsIdentities(0);
                double P1 = neq.NewtonsIdentities(1);
                double P2 = neq.NewtonsIdentities(2);
                double P3 = neq.NewtonsIdentities(3);
                double P4 = neq.NewtonsIdentities(4);
                double P5 = neq.NewtonsIdentities(5);
                double P6 = neq.NewtonsIdentities(6);
                double P7 = neq.NewtonsIdentities(7);
                double P = neq.NewtonsIdentities(8);
                QueuedConsole.WriteImmediate("P0 : {0}", P0);
                QueuedConsole.WriteImmediate("P1 : {0}", P1);
                QueuedConsole.WriteImmediate("P2 : {0}", P2);
                QueuedConsole.WriteImmediate("P3 : {0}", P3);
                QueuedConsole.WriteImmediate("P4 : {0}", P4);
                QueuedConsole.WriteImmediate("P5 : {0}", P5);
                QueuedConsole.WriteImmediate("P6 : {0}", P6);
                QueuedConsole.WriteImmediate("P : {0}", P);
            }

            /// <summary>
            /// https://brilliant.org/problems/excel-crashed-while-computing-these/
            /// </summary>
            void ExcelCrashed_P1024()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(4, new double[] { 1, 3, 4, 3, 1 });
                double val = neq.NewtonsIdentities(1024);
                QueuedConsole.WriteImmediate("Answer : {0}", val * 2);
            }

            /// <summary>
            /// https://brilliant.org/problems/wanna-bash-it-2/
            /// </summary>
            void WannaBashIt_2()
            {
                BigRational[] Coeff = new BigRational[5] { new BigRational(1,1), new BigRational(-3, 1),
                    new BigRational(2, 1), new BigRational(2,1), new BigRational(-4,1) };
                NthDegreePolynomialBigRational neq = new NthDegreePolynomialBigRational(4, Coeff);
                
                BigRational val = neq.NewtonsIdentities(2015);
                
                int r = -3;
                int maxDivp = int.MinValue, maxDivq = int.MinValue, rbest = 0;
                while (++r <= 10000)
                {
                    BigRational copy = new BigRational(val.Numerator, val.Denominator);
                    copy -= r;
                    if (copy % 2 == 0)
                    {
                        int count = 0;
                        while (true)
                        {
                            if (copy % 2 == 0) { copy /= 2; count++; }
                            else
                            {
                                if (count > maxDivp)
                                {
                                    maxDivp = count; rbest = r;
                                    QueuedConsole.WriteImmediate("For r : {0}, p : {1}", r, count);
                                }
                                BigRational copy2 = copy - 1;
                                count = 0;
                                while (true)
                                {
                                    if (copy2 % 2 == 0) { copy2 /= 2; count++; }
                                    else
                                    {
                                        if (count > maxDivq)
                                        {
                                            maxDivq = count; rbest = r;
                                            QueuedConsole.WriteImmediate("For r : {0}, q : {1}", r, count);
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
                QueuedConsole.WriteImmediate("For r : {0}, p :{1}, q: {2}", rbest, maxDivp);
            }

            /// <summary>
            /// https://brilliant.org/problems/im-not-doing-newtons-sum-all-the-way/
            /// </summary>
            void ReverseNewtonIdentities()
            {
                NthDegreePolynomialReverseNewtonIdentities ndp = new NthDegreePolynomialReverseNewtonIdentities(9, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                BigRational val = ndp.NewtonsIdentities(10);
                QueuedConsole.WriteImmediate("V10 : {0}", val);
                Fraction<int> fr = FractionConverter<int>.BigRationalToFraction(val);
                QueuedConsole.WriteImmediate("N: {0}, D: {1}", fr.N, fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/just-a-century/
            /// </summary>
            void JustACentury()
            {
                double[] Coefficients = new double[101];

                Enumerable.Range(0, 101).ForEach(x =>
                {
                    if (x == 0 || x >= 99) Coefficients[x] = 1;
                    else Coefficients[x] = 0;
                });

                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(100, Coefficients);
                double sum = 0.0;
                Enumerable.Range(0, 101).ForEach(x =>
                {
                    sum += neq.NewtonsIdentities(x);
                });
                QueuedConsole.WriteImmediate("Answer : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/it-appears-more-than-you-think/
            /// </summary>
            void ItAppearsMoreThanYouThink()
            {
                NthDegreePolynomialEquation neq = new NthDegreePolynomialEquation(3, new double[] { 1, -2, 3, -4 });
                int n = 0, nSum = 0;
                int nSumPrecisionCheck = 0;
                double eq = 0.0;
                eq += 2;
                double precision = 0.001;
                while (++n <= 1000)
                {
                    double val = neq.NewtonsIdentities(n);
                    double diff = Math.Abs(Math.Abs(val) - eq);
                    if (Math.Abs(val).Equals(eq)) {
                        nSum += n;
                    } else if(diff <= precision) {
                        nSumPrecisionCheck += n;
                    }
                }
                QueuedConsole.WriteImmediate("Sum of all n : {0}", nSum);
                QueuedConsole.WriteImmediate("Sum of all n for |Sm| ~ 2 : {0}", nSumPrecisionCheck);
            }

            /// <summary>
            /// https://brilliant.org/problems/easy-to-think-harder-to-solve-2/
            /// </summary>
            void EasyToThinkHarderToSolve()
            {
                int n = 4;
                BigRational[] Y = new BigRational[] { new BigRational(0,1), new BigRational(240, 1), new BigRational(1344, 1), new BigRational(4663, 1) };
                BigRational[,] X = new BigRational[n, n];

                Enumerable.Range(1, n).ForEach(x =>
                {
                    int r = (2 * x) - 1;
                    Y[x - 1] -= new BigRational((long)Math.Pow(r, 4), 1);

                    Enumerable.Range(0, n).ForEach(y =>
                    {
                        X[x - 1, y] = new BigRational((long)Math.Pow(r, n - 1 - y), 1);
                    });
                });

                GenericDefs.Functions.LinearEquations.Solve(X, Y);

                BigRational[] Coeff = new BigRational[5] { new BigRational(1,1), Y[0], Y[1], Y[2], Y[3] };
                NthDegreePolynomialBigRational neq = new NthDegreePolynomialBigRational(4, Coeff);

                NthDegreePolynomialEquation neq1 = new NthDegreePolynomialEquation(4, new double[] { 48, 583, -2847, 7649, -5433 });

                double v1 = neq1.NewtonsIdentities(3) + neq1.NewtonsIdentities(6);

                QueuedConsole.WriteImmediate("P1: {0}", neq1.NewtonsIdentities(1));
                QueuedConsole.WriteImmediate("P2: {0}", neq1.NewtonsIdentities(2));
                QueuedConsole.WriteImmediate("Answer : {0}", v1);
            }
        }

        internal class Induction
        {
            public void Solve()
            {
                InductionOrFunctionCorrected();
            }

            /// <summary>
            /// https://brilliant.org/problems/induction-or-function-corrected/
            /// </summary>
            void InductionOrFunctionCorrected()
            {
                Dictionary<int, int> d = new Dictionary<int, int>();
                int n = 1;
                d.Add(1, 0);
                while (true)
                {
                    n++;
                    int fnVal = 0;
                    if (n % 2 == 0) {
                        fnVal = 1 + (2 * d[n / 2]);
                    } else {
                        fnVal = 2 * d[(n - 1) / 2];
                    }
                    d.Add(n, fnVal);
                    if (fnVal == 1994) break;
                }
                QueuedConsole.WriteImmediate("Smallest value of n : {0}", n);
            }
        }
    }
}