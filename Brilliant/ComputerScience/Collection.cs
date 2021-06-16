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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// 
    /// </summary>
    public class Collection : ISolve, IBrilliant, IProblemName
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
            (new Collection1()).AckermannFunction();
            //(new Collection2()).Solve();
            //(new Quizzes()).Solve();
        }

        internal class Collection1
        {
            /// <summary>
            /// https://brilliant.org/problems/please-dont-hate-me-for-this/
            /// </summary>
            internal void Base36toBase10Conversion()
            {
                string wholePart = "nevergonnagiveyouup";
                string decimalPart = "nevergonnaletyoudown";

                BigRational br = BaseConversion.ConvertToBase10(wholePart, decimalPart, 36);
                QueuedConsole.WriteFormat("A : {0}, B : {1}", br.Numerator, br.Denominator);
                BigIntFraction frac = new BigIntFraction(br.Numerator, br.Denominator, false);

                BigInteger digitSum = MathFunctions.DigitSum(frac.N + frac.D);
                QueuedConsole.WriteFinalAnswer("A + B : " + digitSum);
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// 160001001200030002150101 is in base 7.
            /// Subtract a number n, 0 l.t. n l.t. 5 to make it divisible by 6.
            /// </summary>
            internal void Base7()
            {
                BigRational br = BaseConversion.ConvertToBase10("160001001200030002150101", "", 7);
                QueuedConsole.WriteFinalAnswer("Number to subtract : " + (br.GetWholePart() % 6).ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/hexy-prime-sum/
            /// </summary>
            internal void HexyPrimeSum()
            {
                int hnMax = 2500 * (2500 * 2 - 1);
                List<int> primes = Prime.GeneratePrimesNaiveNMax(hnMax);
                primes.Sort();
                int n = 2;
                long tauSum = 0;
                while (true)
                {
                    tauSum += primes.Where(x => x < n * (2 * n - 1)).Max();
                    n++;
                    if (n == 2501) break;
                }

                QueuedConsole.WriteFinalAnswer("The required sum is {0}", tauSum);
            }

            /// <summary>
            /// https://brilliant.org/problems/aperiodic-binary-strings/
            /// </summary>
            internal void AperiodicBinaryStrings()
            {
                BigInteger val = 1;
                int count = 0;
                while (true)
                {
                    val *= 2;
                    count++;
                    if (count == 23) break;
                }
                val -= 2;
                QueuedConsole.WriteFinalAnswer("Number of aperiodic binary strings of length 23 is {0}", val.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/aperiodic-binary-strings-redux/
            /// </summary>
            internal void AperiodicBinaryStringsRedux()
            {
                BigInteger val = 1;
                int n = 1;
                Dictionary<long, int> allFactors = new Dictionary<long, int>();
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 100);
                while (true)
                {
                    Dictionary<long, int> factors = Factors.GetPrimeFactorsWithMultiplicity(n, p);
                    foreach (KeyValuePair<long, int> kvp in factors)
                    {
                        if (allFactors.ContainsKey(kvp.Key)) { allFactors[kvp.Key] += kvp.Value * 23; }
                        else { allFactors.Add(kvp.Key, kvp.Value * 23); }
                    }
                    if (n == 23) break;
                    n++;
                }

                List<BigInteger> fList = new List<BigInteger>();
                foreach (KeyValuePair<long, int> kvp in allFactors)
                {
                    int count = 0;
                    while (true)
                    {
                        fList.Add(kvp.Key);
                        count++;
                        if (count == kvp.Value) break;
                    }
                }

                List<BigInteger> factorsPowerSet = PowerSet.GetAllFactors(fList).ToList();

                BigInteger nStrings = 1;
                n = 1;
                while (true)
                {
                    nStrings *= n;
                    if (n == 23) break;
                    n++;
                }

                nStrings = BigInteger.Pow(nStrings, 23);

                //QueuedConsole.WriteImmediate("Number of strings of length 23! ^ 23 is 2 ^ {0}", nStrings.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/highly-divisible-number/
            /// </summary>
            internal void HighlyDivisibleNumber()
            {
                List<int> primes = Prime.GeneratePrimesNaiveNMax(10000);
                primes.Sort();
                HashSet<int> primesN = new HashSet<int>();

                int pN = 10000;
                int pi = 0;
                int totalPrimes = 0;
                while (true)
                {
                    pi = pN - totalPrimes;
                    if (pi < 2) break;
                    Dictionary<long, int> pFactors = Factors.GetPrimeFactorsWithMultiplicity(pi);
                    foreach (KeyValuePair<long, int> kvp in pFactors)
                    {
                        totalPrimes += kvp.Value;
                        primesN.Add((int)kvp.Key);
                    }
                }

                int smallestPrime = 10000;
                foreach (int p in primes)
                {
                    if (!primesN.Contains(p))
                    {
                        if (smallestPrime > p) smallestPrime = p;
                    }
                }

                QueuedConsole.WriteFinalAnswer("Smallest prime is {0}", smallestPrime);
            }

            /// <summary>
            /// https://brilliant.org/problems/symmetry-equiv-degeneracy/
            /// </summary>
            internal void SymmetryDegeneracy()
            {
                int nMax = 939;
                int n = 300;

                int sumnxsq = 0;
                int maxDegenStates = 0;
                while (true)
                {
                    int nxMax = (int)Math.Floor(Math.Sqrt(n));
                    int count = 0;
                    for (int x = 1; x <= nxMax; x++)
                    {
                        int nxsq = x * x;
                        for (int y = 1; y <= nxMax; y++)
                        {
                            int nysq = y * y;
                            if (nysq + nxsq > n) break;
                            for (int z = 1; z <= nxMax; z++)
                            {
                                int nzsq = z * z;
                                int sum = nysq + nxsq + nzsq;
                                if (sum > n) break;
                                if (sum == n) { count++; break; }
                            }
                        }
                    }

                    if (count > maxDegenStates) { maxDegenStates = count; sumnxsq = n; }
                    n++;
                    if (n > nMax) break;
                }

                double A = 2.26536 * sumnxsq / 939.7711;
                QueuedConsole.WriteFinalAnswer("Floor(A*10^37) : " + Math.Floor(1000 * A));
            }

            /// <summary>
            /// https://brilliant.org/problems/positive-streak/
            /// </summary>
            internal void PositiveStreak()
            {
                var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.positive-streak.txt", true);
                List<int> list = new List<int>();
                string s = string.Empty;
                using (StreamReader sr = new StreamReader(stream))
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        break;
                    }
                }
                s = s.Replace(" ", "");
                string[] numbers = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string t in numbers)
                {
                    list.Add(int.Parse(t));
                }

                ConsecutiveStreakCounter csc = new ConsecutiveStreakCounter();
                foreach (int n in list)
                {
                    csc.Add(n >= 0 ? 1 : -1);
                }

                QueuedConsole.WriteFinalAnswer("Maximum streak is : " + csc.GetMaximumPositiveStreak());
            }

            internal void ThreedigitNumbersDivisibleBydigits()
            {
                int n = 100;
                int counter = 0;
                while (true)
                {
                    char[] distinct = (n + "").ToCharArray().Distinct().ToArray();
                    if (string.Join("", distinct).IndexOf("0") < 0)
                    {
                        if (!distinct.Any(x => n % char.GetNumericValue(x) != 0)) counter++;
                    }
                    if (n == 999) break;
                    n++;
                }
                QueuedConsole.WriteImmediate("Number of three digit numbers : {0}", counter);
            }

            internal void DigitNumbers3Or7()
            {
                int n = 1;
                int counter = 0;
                while (true)
                {
                    char[] distinct = (n + "").ToCharArray().Distinct().ToArray();
                    if (!distinct.Any(x => (char.GetNumericValue(x) != 3 && char.GetNumericValue(x) != 7)))
                    {
                        QueuedConsole.WriteImmediate("{0}", n);
                        counter++;
                    }
                    if (n == 10000) break;
                    n++;
                }
                QueuedConsole.WriteImmediate("Numbers containing only 3 or 7 : {0}", counter);
            }

            internal void CashPermutations()
            {
                SimpleCounter counter = new SimpleCounter();
                for (int a = 0; a <= 20; a++)
                {
                    for (int b = 0; b <= 6; b++)
                    {
                        for (int c = 0; c <= 5; c++)
                        {
                            if (((5 * a) + (10 * b) + (20 * c)) == 100)
                            {
                                QueuedConsole.WriteImmediate("5$ notes : {0}, 10$ notes : {1}, 20$ notes : {2}", a, b, c);
                                BigInteger add = PermutationsAndCombinations.Factorial(a + b + c)
                                    / (PermutationsAndCombinations.Factorial(a) * PermutationsAndCombinations.Factorial(b) * PermutationsAndCombinations.Factorial(c));
                                counter.Increment(add);
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of ways : {0}", counter.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/increasing-sub-array/
            /// </summary>
            internal void StrictlyIncreasingSubArrays()
            {
                List<int> aList = new List<int>();
                using (var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.sub-arrays.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            s = (s.Substring(1, s.Length - 2)).Replace(" ", "");
                            var sSplit = (s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                            sSplit.ForEach(x => aList.Add(int.Parse(x)));
                        }
                    }
                }

                int n = 2;
                int count = 0;
                while (true)
                {
                    for (int i = 0; i <= aList.Count - n; i++)
                    {
                        List<int> range = aList.GetRange(i, n);
                        count++;
                        for (int j = 0; j < range.Count - 1; j++)
                        {
                            if (range[j] >= range[j + 1]) { count--; break; }
                        }
                    }
                    n++;
                    if (n >= aList.Count) break;
                }

                QueuedConsole.WriteImmediate("Strictly increasing sub-arrays : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-zero-array/
            /// </summary>
            internal void TheZeroArray()
            {
                //string arr = "39, 26, 45, 20, 47, 35, 46, 37, 26, 34, 31, 29, 36, 21, 31, 47, 17, 49, 22, 28, 36, 40, 24, 24, 21, 43, 48, 31, 38, 27, 46, 19, 37, 39, 36, 19, 42, 37, 33, 24, 15, 22, 21, 34, 44, 27, 16, 30, 44, 45, 20, 21, 18, 24, 39, 44, 26, 30, 24, 33, 21, 32, 30, 29, 46, 39, 30, 25, 50, 17, 26, 33, 45, 44, 17, 29, 36, 17, 15, 50, 45, 21, 48, 45, 33, 18, 31, 18, 28, 37, 45, 16, 45, 30, 26, 21, 28, 37, 49, 36, 49, 48, 19, 46, 43, 23, 28, 19, 17, 35, 36, 30, 31, 49, 19, 30, 17, 48, 25, 19, 32, 45, 30, 25, 39, 47, 34, 38, 17, 17, 37, 17, 34, 27, 27, 44, 27, 47, 50, 49, 50, 26, 38, 38, 23, 46, 40, 15, 42";
                string arr = "39, 26, 45, 20, 47, 35, 46, 37, 26, 34, 31, 29, 36, 21, 31, 47, 17, 49, 22, 28, 36, 40, 24, 24, 21, 43, 48, 31, 38, 27, 46, 19, 37, 39, 36, 19, 42, 37, 33, 24, 15, 22, 21, 34, 44, 27, 16, 30, 44, 45, 20, 21, 18, 24, 39, 44, 26, 30, 24, 33, 21, 32, 30, 29, 46, 39, 30, 25, 50, 17, 26, 33, 45, 44, 17, 29, 36, 17, 15, 50, 45, 21, 48, 45, 33, 18, 31, 18, 28, 37, 45, 16, 45, 30, 26, 21, 28, 37, 49, 36, 49, 48, 19, 46, 43, 23, 28, 19, 17, 35, 36, 30, 31, 49, 19, 30, 17, 48, 25, 19, 32, 45, 30, 25, 39, 47, 34, 38, 17, 17, 37, 17, 34, 27, 27, 44, 27, 47, 50, 49, 50, 26, 38, 38, 23, 46, 40, 15, 42";
                //string arr = "2,2,3,3";
                arr = arr.Replace(" ", "");
                List<int> list = new List<int>();
                (arr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ForEach(x => list.Add(int.Parse(x)));

                QueuedConsole.WriteImmediate("Least value in list: {0}, count : {1}", list.Min(), list.Count);
                QueuedConsole.WriteImmediate("Maximum no. of operations : {0}", list.Sum());
                int n = 0;
                int _2pown = (int)Math.Pow(2, n);

                QueuedConsole.WriteImmediate("------------Approach 1 ----------------");
                int listMin = list.Min();
                int listMax = list.Max();
                int nMin = 0, nMax = 0;
                while (true)
                {
                    n++;
                    _2pown = (int)Math.Pow(2, n);
                    if (nMin == 0 && _2pown > listMin) { nMin = n - 1; }
                    if (nMax == 0 && _2pown > listMax) { nMax = n - 1; break; }
                }

                n = nMax;
                List<int> genList = new List<int>();
                Enumerable.Range(0, list.Count).ForEach(x => genList.Add(0));
                int nOps = 0;
                while (true)
                {
                    _2pown = (int)Math.Pow(2, n);
                    bool doublingOp = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] > _2pown && genList[i] == 0) { nOps++; genList[i] = 1; }
                        else if (list[i] > _2pown && genList[i] >= 1) { doublingOp = true; genList[i] *= 2; }
                    }
                    if (doublingOp)
                    {
                        nOps++;
                    }

                    if (--n < 0) break;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (genList[i] > list[i]) { QueuedConsole.WriteImmediate("Error at index : {0}", i); }
                    if (list[i] > genList[i]) { nOps += list[i] - genList[i]; genList[i] = list[i]; }
                }
                QueuedConsole.WriteImmediate("Approach 1, Number of operations needed : {0}", nOps);
                QueuedConsole.WriteImmediate(Environment.NewLine);
                QueuedConsole.WriteImmediate("------------Approach 2 ----------------");

                int nOps_app2 = 0;
                List<int> clonedList = list.ToList();
                while (true)
                {
                    bool doublingOpNeeded = false;
                    for (int i = 0; i < clonedList.Count; i++)
                    {
                        if (clonedList[i] % 2 != 0) { clonedList[i]--; nOps_app2++; }
                        if (clonedList[i] > 0 && clonedList[i] % 2 == 0) { clonedList[i] /= 2; doublingOpNeeded = true; }
                    }
                    if (doublingOpNeeded) nOps_app2++;

                    if (clonedList.All(x => x == 0)) break;
                }
                QueuedConsole.WriteImmediate("Number of operations needed : {0}", nOps_app2);
            }

            /// <summary>
            /// https://brilliant.org/problems/shifty-sort/
            /// </summary>
            internal void ShiftySort()
            {
                List<List<int>> sequences = new List<List<int>>();
                using (var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.shifty-sort.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        char[] splC = new char[] { ',' };
                        while ((s = sr.ReadLine()) != null)
                        {
                            s = (s.Substring(1, s.Length - 2)).Replace(" ", "");
                            List<int> seq = new List<int>();
                            (s.Split(splC, StringSplitOptions.RemoveEmptyEntries)).ForEach(x => seq.Add(int.Parse(x)));
                            sequences.Add(seq);
                        }
                    }
                }
                int sumFa = 0;
                for (int i = 0; i < sequences.Count; i++)
                {
                    List<int> curSeq = sequences[i].ToList();
                    int nShifts = 0;
                    while (true)
                    {
                        if (LinqExtensions.IsOrdered(curSeq, OrderByDirection.Ascending)) break;
                        else
                        {
                            int last = curSeq[curSeq.Count - 1];
                            curSeq.RemoveAt(curSeq.Count - 1);
                            curSeq.Insert(0, last);
                            nShifts++;
                            if (nShifts == curSeq.Count) break;
                        }
                    }

                    sumFa += (nShifts == curSeq.Count) ? -1 : nShifts;
                }
                QueuedConsole.WriteImmediate("The sum of F(a) for all the sequences in the text file : {0}", sumFa);
            }

            /// <summary>
            /// https://brilliant.org/practice/graphs-and-trees-level-4-5-challenges/?problem=passing-condition-2
            /// </summary>
            internal void _4000ThFineNumber()
            {
                int n = 1;
                int nFineCount = 0, nFineMax = 4000;
                SortedSet<long> set = new SortedSet<long>() { 2, 3, 5, 7 };
                while (true)
                {
                    if (Factors.HasPrimeFactors(n, set)) ++nFineCount;
                    if (nFineCount == nFineMax) break;
                    n++;
                }
                QueuedConsole.WriteImmediate("{0} th fine number is : {1}", nFineMax, n);
            }

            /// <summary>
            /// https://brilliant.org/problems/fibonacci-sum/
            /// </summary>
            internal void FibonacciSum()
            {
                QueuedConsole.WriteImmediate("Sum of indices : {0}", (Fibonacci.ZeckendorfIndices(BigInteger.Parse("99887766554433221100"))).Sum());
            }

            /// <summary>
            /// https://brilliant.org/problems/find-the-missing-number-8/
            /// </summary>
            internal void FindMissingNumber()
            {
                string array1 = Http.Request.GetHtmlResponse("http://pastebin.com/raw/C6zyH5KM");
                string array2 = Http.Request.GetHtmlResponse("http://pastebin.com/raw/c4adp7Q3");
                array1 = array1.Replace(" ", "");
                array2 = array2.Replace(" ", "");
                string[] sparray1 = array1.Split(StringSplitter.Comma, StringSplitOptions.RemoveEmptyEntries);
                string[] sparray2 = array2.Split(StringSplitter.Comma, StringSplitOptions.RemoveEmptyEntries);

                long sum1 = 0, sum2 = 0;
                sparray1.ForEach(x => sum1 += long.Parse(x));
                sparray2.ForEach(x => sum2 += long.Parse(x));
                QueuedConsole.WriteImmediate("Missing number : {0}", sum1 - sum2);
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-3-challenges/?subtopic=types-and-data-structures&chapter=fun-computer-science-quizzes
            /// https://brilliant.org/problems/extremely-deep-recursion-part-1/
            /// </summary>
            internal void AckermannFunction()
            {
                Func<int, BigInteger, BigInteger> A = null;
                A = delegate (int m, BigInteger n)
                {
                    if (m == 0) return n + 1;
                    else if (m > 0 && n == 0) return A(m - 1, 1);
                    else return A(m - 1, A(m, n - 1));
                };
                BigInteger A33 = A(3, 3);

                QueuedConsole.WriteImmediate("A(3,3) : {0}", A33.ToString());
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-3-challenges/
            /// </summary>
            internal void MaximumKnapsackWeight()
            {
                double[] bottles = new double[] { 0.77, 1.1, 3.4, 7.0 };
                QueuedConsole.WriteImmediate("Most amount of water Tom can carry : {0}", GenericDefs.Functions.Algorithms.DP.Knapsack.Variation3.Solve(bottles, 15));
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-3-challenges/?p=3
            /// </summary>
            internal void NumberOfPaths()
            {
                int p = 35;
                BigInteger nPaths = new BigInteger(0);
                BigInteger p2 = (BigInteger)Math.Pow(p, 2);
                BigInteger p4 = p2 * p2;
                BigInteger p6 = p2 * p4;
                BigInteger p8 = p4 * p4;
                BigInteger p10 = p4 * p6;
                nPaths += p2;
                nPaths += (6 * p4);
                nPaths += (2 * p6);
                nPaths += (20 * p8);
                nPaths += (6 * p10);
                QueuedConsole.WriteImmediate("Number of possible paths : {0}", nPaths.ToString());
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-3-challenges/?p=4
            /// The squares of a 3 x 3 grid are filled with integers from 1 to 9 such that the sum of each row and the sum of each column is 15. 
            /// How many different ways can the squares be filled?
            /// Rotations and reflections are distinct arrangements.
            /// </summary>
            internal void MagicSquares()
            {
                List<int[]> rows = new List<int[]>();
                rows.Add(new int[3] { 0, 1, 2 });
                rows.Add(new int[3] { 3, 4, 5 });
                rows.Add(new int[3] { 6, 7, 8 });

                List<int[]> cols = new List<int[]>();
                cols.Add(new int[3] { 0, 3, 6 });
                cols.Add(new int[3] { 1, 4, 7 });
                cols.Add(new int[3] { 2, 5, 8 });

                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    foreach (int[] row in rows)
                    {
                        if (cr.CoefficientSum(row) != 15) return false;
                    }
                    foreach (int[] col in cols)
                    {
                        if (cr.CoefficientSum(col) != 15) return false;
                    }
                    return true;
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

                QueuedConsole.WriteFinalAnswer("The squares can be filled in : {0} ways.", ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-4-challenges/?p=3
            /// https://brilliant.org/problems/ordering-from-a-menu/
            /// </summary>
            internal void RestaurantSpendingDifferentWays()
            {
                int[] items = new int[] { 1, 5, 20, 50, 100, 200 };
                UniqueArrangements<int> c = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation3.UniqueSolutions(items, 300);
                QueuedConsole.WriteImmediate("Number of ways O : {0}", c.GetCount());
                QueuedConsole.WriteImmediate("Last three digits of O : {0}", c.GetCount() % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/sum-the-ino-numbers/
            /// </summary>
            internal void SumTheInoNumbers()
            {
                Func<double, double> In_no = delegate (double n)
                {
                    double nsq = n * n;
                    if (n < nsq) return nsq - n;
                    if (n > nsq)
                    {
                        if (n > n / 2) return n + (n / 2);
                        else if (n < n / 2) return 2 * n;
                        else if (n + 1 < n) return Math.Abs(n);
                        else return n + 2 - (2 * n) + (n / 2);
                    }
                    return n - (nsq / (n + 1));
                };

                double sum = 0;
                double loc1 = 0;
                while (loc1 < 11)
                {
                    double loc2 = 1;
                    while (loc2 < 11)
                    {
                        sum += In_no(loc1 / loc2);
                        loc2 += 1;
                    }
                    loc1 += 1;
                }

                QueuedConsole.WriteImmediate("sum : {0}, rounded sum : {1}", sum, Math.Round(sum));
            }

            /// <summary>
            /// https://brilliant.org/problems/digit-constraints/
            /// </summary>
            internal void DigitConstraints()
            {
                int dMax = 7;
                int maxDepth = 10;
                SimpleCounter c = new SimpleCounter();
                Func<int[], int, bool> AddDigits = null;
                AddDigits = delegate (int[] digits, int depth) {
                    int i = depth == 0 ? 1 : 0;
                    for (; i <= dMax; i++)
                    {
                        int[] after = digits.ToArray();
                        if (depth >= 2) { if ((digits[depth - 2] + digits[depth - 1] + i) > dMax) return false; }
                        after[depth] = i;
                        if(depth == maxDepth) {
                            c.Increment(); continue;
                        }
                        else { AddDigits(after, depth + 1); }
                    }
                    return false;
                };

                AddDigits(new int[77], 0);
                QueuedConsole.WriteImmediate("N : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/magic-squares/
            /// </summary>
            internal void MagicSquares4x4()
            {
                List<int[]> rows = new List<int[]>();
                rows.Add(new int[4] { 0, 1, 2, 3 });
                rows.Add(new int[4] { 4, 5, 6, 7 });
                rows.Add(new int[4] { 8, 9, 10, 11 });
                rows.Add(new int[4] { 12, 13, 14, 15 });

                List<int[]> cols = new List<int[]>();
                cols.Add(new int[4] { 0, 4, 8, 12 });
                cols.Add(new int[4] { 1, 5, 9, 13 });
                cols.Add(new int[4] { 2, 6, 10, 14 });
                cols.Add(new int[4] { 3, 7, 11, 15 });

                List<int[]> diagonals = new List<int[]>();
                diagonals.Add(new int[4] { 0, 5, 10, 15 });
                diagonals.Add(new int[4] { 3, 6, 9, 12 });

                int n = 4;
                int nSum = n * ((n * n) + 1) / 2;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    foreach (int[] row in rows)
                    {
                        if (cr.CoefficientSum(row) != nSum) return false;
                    }
                    foreach (int[] col in cols)
                    {
                        if (cr.CoefficientSum(col) != nSum) return false;
                    }
                    foreach (int[] diagonal in diagonals)
                    {
                        if (cr.CoefficientSum(diagonal) != nSum) return false;
                    }
                    return true;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < n * n; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = n * n, Min = 1 });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(n * n, iterators);

                List<List<int>> sets = ua.ExtractAsSets();
                sets.ForEach(x => {
                    QueuedConsole.WriteImmediate("Sum : {0}", x[0] + x[3] + x[12] + x[15]);
                });
            }
        }

        internal class Collection2
        {
            internal void Solve()
            {
                _4096MadeHard();
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-crack-the-code-in-time/
            /// </summary>
            void CrackVigenereCipher()
            {
                string cipherText = @"TINTWNPITBPOBPAHKWXBLJGDYLVUPZSQCVPHKZDBZMRDBKIGNQVLTHMZBTBGAWEBOFVXCSKQKXGPWCIVYMTBCCQCVGVCIWHVEPGRSMWPJYMHICWUPNHLVTCSTGCPJHTPRHPDXIMKMITQAMSDTMKDPGAWUGOTAVPHLAIDDTINISUAJHDLKITBLUNBOFJTBOZAXCUMWGXBCIHXCUAIDDTINTZLDTCKOIIXGFWJGDVAXIWVVHICWUPNHDMAKSLFITBZQKTSUMBNBHDPAOJBXKWAGCTOYKPAOPAHICWUPNHOQGISLVEASHATLVHBXHMVCGECZQIXCUAIDDTINUCBZITSUQCRCTQCVGAWGBAHGXCHLZGJDAKDBABVXROAQDCGMWGHSCMGPZKINHGAWEBOFMXVVAMTTBTQCDFZSXGAPAWXBJPPCBLTLXHOTDHGLADCPVBWHWKMHHHVXBPMAETCHFLXKWZQDCHOZTTOYZXKSKQCASOIKGSZBDEAHGILSUBNDBLEWPHPANDIYXDHWAQDCGAWEBOFBLTBAGILCKQKXGPWCIVYMTPGZQHIWUOLXHOXGTDHZPIWVVHUCYJTPQOPTPRKMUTBZMHICWUPNHDMCIMUQCTZHKZXBNIBBIUQIXCUNDGAHKWXBLOJCPBVZTFZXATOZMHTBKEXIVYMHJDWTNHHVXBPMAPXGHFUDGSLVTBMHKIXJPBNCSHZRPZHQHBCCQCVRPDXHWVVIWFLMIDQHTPXGZBDEXBVTDBLTTWOCZTPBKWIWSYJTPQOMHHSJCGTAHAHXBNLTUSUATPHJIAPWZAIDDQCCTHDWGTDVZIHCMMMISUAXKSAZDDDTWKTALVIHOULCPJHTPRHPDXIMVNUTBAQGTQVIHICMJGXHAIXCGAWEHIZXTRHPVKPGPWCXATQCTBAAIDDZMCIOSTEDGZQQASYMXCTVZRTALVIHHVKPAOPAHICWRJCSAPGTSHTAECZAXQZLXGTDHZPIWVVHRCTXATHLLHICWRJCSMWJGKVZGXSKIQDIAXDHGPJATSUMBNWUBTGQLXIXCUWURCTUJCQHBXDBZAIDDQCCTGPFCDFTICSMPVKPRLLHTBKPTPJFJDBPYMEGWZIAHHVUPCQOMHISYWCYIUMTXUOBHICW";
                //cipherText = @"cit yvthftnes jy mtcctvg on ctzc iqg wttn gcfrotr yjv fgt on evsbcqnqmsgog 
                //    qnr yvthftnes qnqmsgog on bqvcoefmqv rqconu wqel cj cit ovqho 
                //    kqcitkqcoeoqn qm lonro dij yjvkqmms rtxtmjbtr cit ktcijr cit eobitvg 
                //    wvtqlqwmt ws ciog cteinohft uj wqel qc mtqgc cj cit eqtgqv eobitv 
                //    onxtnctr ws pfmofg eqtgqv gj ciog ktcijr ejfmr iqxt wttn tzbmjvtr 
                //    on emqggoeqm coktg";
                //cipherText.Replace(" ", "");
                //cipherText = cipherText.ToUpper();

                int n = 1;
                while (++n <= 50)
                {
                    QueuedConsole.WriteAndWaitOnce("Key : {0}, Length: {1}", GenericDefs.Functions.Crypto.Cypher.Vigenere.FindKey(cipherText, n), n);
                }
                //string key = "HIEPO";
                string key = "HIPPO";
                string dText = GenericDefs.Functions.Crypto.Cypher.Vigenere.Decipher(cipherText, key);
                QueuedConsole.WriteImmediate("Code : {0}, Key: {1}", dText, key);
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-4-challenges/?problem=knight-paths&subtopic=types-and-data-structures&chapter=fun-computer-science-quizzes
            /// </summary>
            void KnightPath()
            {
                int LeastMoves = 6;
                int XDest = 7, YDest = 7;
                int nSequences = 0;
                Func<int, int, int, bool> Recursion = null;
                Recursion = delegate (int curX, int curY, int nMoves) {
                    if (nMoves > LeastMoves) return false;
                    if (curX < 0 || curY < 0) return false;
                    if (curX > XDest) return false;
                    if (curY > YDest) return false;
                    if (curX == XDest && curY == YDest && nMoves <= LeastMoves) { QueuedConsole.WriteImmediate("Sequence counter : {0}", ++nSequences); return true; }
                    else
                    {
                        Recursion(curX + 2, curY + 1, nMoves + 1);
                        Recursion(curX + 1, curY + 2, nMoves + 1);
                        Recursion(curX - 2, curY + 1, nMoves + 1);
                        Recursion(curX - 1, curY + 2, nMoves + 1);
                        Recursion(curX + 2, curY - 1, nMoves + 1);
                        Recursion(curX + 1, curY - 2, nMoves + 1);
                        Recursion(curX - 2, curY - 1, nMoves + 1);
                        Recursion(curX - 1, curY - 2, nMoves + 1);
                    }
                    return false;
                };

                Recursion(0, 0, 0);
                QueuedConsole.WriteImmediate("Sequence counter : {0}", nSequences);
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-4-challenges/?p=2
            /// </summary>
            void CuriousGeorge()
            {
                SimpleHashSet Points = new SimpleHashSet(maxLength: 100000);
                Queue<string> RecursionQueue = new Queue<string>();
                char[] splitter = new char[] { ':' };
                int sumMax = 2;

                Dictionary<int, long> DigitSum = new Dictionary<int, long>();
                Func<int, int, long> GetDigitSum = null;
                GetDigitSum = delegate (int a, int b)
                {
                    a = Math.Abs(a);
                    if (!DigitSum.ContainsKey(a)) DigitSum.Add(a, MathFunctions.DigitSum(a));
                    b = Math.Abs(b);
                    if (!DigitSum.ContainsKey(b)) DigitSum.Add(b, MathFunctions.DigitSum(b));
                    return DigitSum[a] + DigitSum[b];
                };

                Func<int, int, bool> Move = null;
                Move = delegate (int curX, int curY) {
                    string key = string.Empty;
                    if (GetDigitSum(curX + 1, curY) <= sumMax)
                    {
                        key = (curX + 1) + ":" + curY;
                        if (Points.Add(key)) RecursionQueue.Enqueue(key);
                    }
                    if (GetDigitSum(curX, curY + 1) <= sumMax)
                    {
                        key = curX + ":" + (curY + 1);
                        if (Points.Add(key)) RecursionQueue.Enqueue(key);
                    }
                    if (GetDigitSum(curX - 1, curY) <= sumMax)
                    {
                        key = (curX - 1) + ":" + curY;
                        if (Points.Add(key)) RecursionQueue.Enqueue(key);
                    }
                    if (GetDigitSum(curX, curY - 1) <= sumMax)
                    {
                        key = curX + ":" + (curY - 1);
                        if (Points.Add(key)) RecursionQueue.Enqueue(key);
                    }
                    return false;
                };
                
                RecursionQueue.Enqueue("0:0");
                while (RecursionQueue.Count > 0)
                {
                    string p = RecursionQueue.Dequeue();
                    string[] xy = (p).Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    Move(int.Parse(xy[0]), int.Parse(xy[1]));
                }
                QueuedConsole.WriteImmediate("Number of points : {0}", Points.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-4-challenges/?problem=game-of-stones&subtopic=types-and-data-structures&chapter=fun-computer-science-quizzes
            /// </summary>
            void MovingSquares()
            {
                SimpleCounter c = new SimpleCounter();
                int leftXMax = 50;
                Func<int, int, bool> Move = null;
                Move = delegate (int curX, int nMoves) {
                    if (curX > leftXMax) return false;
                    if (nMoves > 15) return false;
                    else if (curX == leftXMax) { c.Increment(); return false; }
                    else if (nMoves == 15) return false;
                    else {
                        if (curX + 1 <= leftXMax) Move(curX + 1, nMoves + 1);
                        if (curX + 2 <= leftXMax) Move(curX + 2, nMoves + 1);
                        if (curX + 3 <= leftXMax) Move(curX + 3, nMoves + 1);
                        if (curX + 4 <= leftXMax) Move(curX + 4, nMoves + 1);
                        if (curX + 5 <= leftXMax) Move(curX + 5, nMoves + 1);
                    }
                    return false;
                };

                Move(1, 0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-4-challenges/?problem=can-you-crack-diffie-hellman&subtopic=types-and-data-structures&chapter=fun-computer-science-quizzes
            /// </summary>
            void FindPrivateKey()
            {
                BigInteger g = BigInteger.Parse("4567316637081665907977181518889182113445441987446007457733573698022127469439135656733423482251931597580003871195410610463070846363265764446085651841441485");
                BigInteger p = BigInteger.Parse("20992321342930071437617535054294082600409305694823917901103517516849439468932230646876254462856426279068085286595868000559162159946064999915305764788212947");
                BigInteger B = BigInteger.Parse("13261683723811565199480160483723583184154281997005060032137595421236239575828461774398757507049515905188090075911486594228158952115852574793046073896571395");

                BigInteger mod = B % p;
                //long b = 0;
                long b = 1009033615;
                //BigInteger gModp = 1;
                BigInteger gModp = BigInteger.Parse("9701575045345523252461191805568702754719726033720274412469421720437973138330918021562313154004950006832368084560208989567679088831975870439952775409486413");
                do
                {
                    gModp *= g;
                    gModp %= p;
                    b++;
                    if (b % 100000 == 0) QueuedConsole.WriteImmediate("b : {0}", b);
                } while (!gModp.Equals(mod));
                QueuedConsole.WriteImmediate("Private key b : {0}", b);
            }

            /// <summary>
            /// https://brilliant.org/problems/diffie-hellman-and-the-last-bit/
            /// </summary>
            void DiffieHellmanLastBit()
            {
                int g = 5;
                int p = 10007;

                int m = 0;
                int gModp = 1;
                do
                {
                    gModp *= g;
                    gModp %= p;
                    m++;
                } while (!gModp.Equals(2));
                QueuedConsole.WriteImmediate("m : {0}", m);

                gModp = 1;
                do
                {
                    gModp *= g;
                    gModp %= p;
                    m++;
                } while (!gModp.Equals(3));
                QueuedConsole.WriteImmediate("n : {0}", m);
            }

            /// <summary>
            /// https://brilliant.org/problems/walking-at-random-computer-science/
            /// https://brilliant.org/practice/computer-science-warmups-level-5-challenges/?problem=walking-at-random-computer-science
            /// </summary>
            void WalkingAtRandom()
            {
                SimpleCounter c = new SimpleCounter();
                double totalDistance = 0;
                Func<int, int, int, bool> Move = null;
                Move = delegate (int x, int y, int nMoves) {
                    if (x < 0 || x > 4) return false;
                    if (y < 0 || y > 4) return false;
                    if (nMoves > 10) return false;
                    if (nMoves == 10) {
                        c.Increment();
                        totalDistance += Math.Sqrt((x * x) + (y * y));
                        return true;
                    } else {
                        if (x + 1 <= 4) Move(x + 1, y, nMoves + 1);
                        if (x - 1 >= 0) Move(x - 1, y, nMoves + 1);
                        if (y + 1 <= 4) Move(x, y + 1, nMoves + 1);
                        if (y - 1 >= 0) Move(x, y - 1, nMoves + 1);
                    }
                    return false;
                };

                Move(0, 0, 0);
                QueuedConsole.WriteImmediate("N : {0}, Total Distance : {1}", c.GetCount(), totalDistance);
                QueuedConsole.WriteImmediate("Average Euclidean Distance : {0}", totalDistance / (long)c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-5-challenges/?p=3
            /// https://brilliant.org/practice/computer-science-warmups-level-5-challenges/?problem=dont-stand-next-to-me
            /// </summary>
            void ProbabilityHeightDifference()
            {
                int nMax = 10000000;
                int n = 0;
                int pCount = 0;
                Random r = new Random();
                double average = 68.0;
                double standardDeviation = 4.5;
                while(++n <= nMax)
                {
                    if (Math.Abs(RandomExtensions.NextGaussian(r, average, standardDeviation) - RandomExtensions.NextGaussian(r, average, standardDeviation)) > 12)
                        pCount++;
                }
                QueuedConsole.WriteImmediate("count : {0}, sample: {1}", pCount, nMax);
                QueuedConsole.WriteImmediate("Probability : {0}", pCount * 1.0 / nMax);
            }

            /// <summary>
            /// https://brilliant.org/problems/self-obsessed-numbers/
            /// https://brilliant.org/practice/computer-science-warmups-level-5-challenges/?problem=self-obsessed-numbers&subtopic=types-and-data-structures
            /// </summary>
            void SelfObsessedNumbers()
            {
                long n = 1;
                Dictionary<int, int> digitPowers = new Dictionary<int, int>();
                Enumerable.Range(1, 9).ForEach(x => {
                    digitPowers.Add(x, (int)Math.Pow(x, x));
                });
                while (++n > 0)
                {
                    int[] digits = Numbers.GetDigitArray(n);
                    if (!digits.Any(x => x == 0))
                    {
                        long nConstructed = 0;
                        digits.ForEach(x => { nConstructed += digitPowers[x]; });
                        if (nConstructed == n) break;
                    }
                }
                QueuedConsole.WriteImmediate("Self obsessed number : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/practice/computer-science-warmups-level-5-challenges/?problem=is-this-how-auto-correct-works
            /// https://brilliant.org/problems/is-this-how-auto-correct-works/
            /// </summary>
            void IsThisHowAutoCorrectWorks()
            {
                string a = "xmzjzohczxwssymjnlvbmogxvgtttcvkuajoamcaeaxwpoxlsayginhnpskufymnomfwtnnznrkndovjjdigahhpzihnmgvznnbx";
                string b = "psakmqrhorcwpaocjnwppepnnhjuhqbyzcmvyyaoatbxgunkznwrwzkdhqfsxxlbqsbwpifsaplueyvbjcxbxddjnneioaybxcwcfbdzdlizxgzjnyervyhzsafmiiecabzxxtbqrmoqiifcxjjesx";
                Dictionary<char, List<int>> charactersInA = new Dictionary<char, List<int>>();
                Dictionary<char, List<int>> charactersInB = new Dictionary<char, List<int>>();
                Enumerable.Range('a', 'z' - 'a' + 1).ForEach(c => {
                    charactersInA.Add((char)c, new List<int>());
                    charactersInB.Add((char)c, new List<int>());
                });
                int n = -1;
                a.ForEach(c => charactersInA[c].Add(++n));
                foreach (var item in charactersInA.Where(kvp => kvp.Value.Count == 0).ToList())
                {
                    charactersInA.Remove(item.Key);
                }
                n = -1;
                b.ForEach(c => charactersInB[c].Add(++n));
                foreach (var item in charactersInB.Where(kvp => kvp.Value.Count == 0).ToList())
                {
                    charactersInB.Remove(item.Key);
                }
                Enumerable.Range('a', 'z' - 'a' + 1).ForEach(c => {
                    char cKey = (char)c;
                    int aCount = charactersInA.ContainsKey(cKey) ? charactersInA[cKey].Count : 0;
                    int bCount = charactersInB.ContainsKey(cKey) ? charactersInB[cKey].Count : 0;
                    //QueuedConsole.WriteImmediate("Character : {0}, a-count : {1}, b-count : {2}, diff : {3}", cKey, aCount, bCount, bCount - aCount);
                });
                int minDistance = GenericDefs.Functions.Algorithms.DP.Strings.LevenshteinDistance(a, b);
                QueuedConsole.WriteImmediate("Approach One. Levenshtein distance : {0}", minDistance);
                GenericDefs.Functions.Algorithms.DP.MinEditDistance<char> mEditDistAlgo = new GenericDefs.Functions.Algorithms.DP.MinEditDistance<char>(a.ToList(), b.ToList());
                QueuedConsole.WriteImmediate("Approach Two. Levenshtein distance : {0}", mEditDistAlgo.CalcMinEditDistance());
            }

            void SquareSpiralMakeYouGiddy()
            {
                int x1 = 12, y1 = -22;
                int xCal = 0, yCal = 0;
                int count = 2;
                int n = 1, Z = 0, len = 1;

                Func<string, string> NextDirection = delegate (string o)
                {
                    if (o == "+x") return "+y";
                    else if (o == "+y") return "-x";
                    else if (o == "-x") return "-y";
                    else if (o == "-y") return "+x";
                    else return "+x";
                };

                string d = NextDirection("");
                while (true)
                {
                    QueuedConsole.WriteImmediate("Z: {0}, x: {1}, y: {2}", Z, xCal, yCal);
                    if (xCal == x1 && yCal == y1) break;

                    if (d == "+x") xCal += 1;
                    else if (d == "+y") yCal += 1;
                    else if (d == "-x") xCal -= 1;
                    else if (d == "-y") yCal -= 1;
                    if (--n == 0) { count--; d = NextDirection(d); if (count == 1) { n = len; } else if (count == 0) { len++; count = 2; n = len; } }
                    Z++;
                }
                QueuedConsole.WriteImmediate("Z : {0}", Z);
            }

            /// <summary>
            /// https://brilliant.org/problems/champernownes-constantprime-numbersthe-real-fun/
            /// </summary>
            void ChampernowneConstant()
            {
                List<int> primes = ((KnownPrimes.GetAllKnownFirstNPrimes(1000))
                    .Where(x => { if (x < 1000) return true; else return false; })
                    .ToList()).ConvertAll(i => (int)i);
                string ChampernownesDecimal = ".";
                int n = 1;
                while(ChampernownesDecimal.Length < 1001)
                {
                    ChampernownesDecimal += "" + n;
                    n++;
                }
                BigInteger b = new BigInteger(1);
                primes.ForEach(x => {
                    int ai = (int)char.GetNumericValue(ChampernownesDecimal[x]);
                    if (ai != 0) b *= ai;
                });
                string bStr = b.ToString();
                int digitSum = 0;
                bStr.ForEach(x => { digitSum += (int)char.GetNumericValue(x); });
                QueuedConsole.WriteImmediate("the digital sum : {0}", digitSum);
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-project-euler/
            /// http://www.mathblog.dk/project-euler-145-how-many-reversible-numbers-are-there-below-one-billion/
            /// </summary>
            void ReversibleNumbers()
            {
                //char[] even = new char[] { '0', '2', '4', '6', '8' };
                //long n = 100000;
                //long sum = 0;
                //string sumStr = string.Empty;
                //long counter = 0;
                //long nMax = 1000000;
                //while (++n < nMax)
                //{
                //    if (n % 10 == 0) continue;
                //    sum = n + long.Parse(Strings.Reverse("" + n));
                //    sumStr = "" + sum;
                //    if (sumStr.IndexOfAny(even) < 0) {
                //        counter++;
                //    }
                //}

                long counter = 0;
                int k = 1;
                while (++k < 12)
                {
                    if ((k - 1) % 4 == 0) { continue; }
                    if ((k - 3) % 4 == 0) { int t = (k - 3) / 4; counter += 100 * (long)Math.Pow(500, t); }
                    if (k % 2 == 0) { int t = (k - 2)/ 2 ; counter += 20 * (long)Math.Pow(30, t); }
                }
                QueuedConsole.WriteImmediate("Total number of Reversible numbers : {0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/how-quickly-can-you-do-this/
            /// </summary>
            void FindTuples()
            {
                int nMax = 45;
                long countA = 0;
                Func<int, int, bool> FindTuplesA = null;
                FindTuplesA = delegate (int depth, int sum)
                {
                    for (int i = 1; i <= nMax; i++)
                    {
                        int sumNext = sum + (depth * i);
                        if(sumNext == nMax) { countA++; }
                        if (sumNext > nMax) { return false; }
                        FindTuplesA(depth + 1, sumNext);
                    }
                    return false;
                };

                Stopwatch sw = new Stopwatch();
                sw.Start();
                FindTuplesA(1, 0);
                sw.Stop();
                QueuedConsole.WriteImmediate("Time taken to find tuples a : {0} mil. sec.", sw.ElapsedMilliseconds);
                long countB = 0;
                Func<int, int, bool> FindTuplesB = null;
                FindTuplesB = delegate (int depth, int sum)
                {
                    for (int i = 1; i <= nMax; i++)
                    {
                        int sumNext = sum + ((depth + 1) * i);
                        if (sumNext <= nMax) { countB++; }
                        if (sumNext > nMax) { return false; }
                        FindTuplesB(depth + 1, sumNext);
                    }
                    return false;
                };

                sw.Restart();
                FindTuplesB(1, 0);
                sw.Stop();
                QueuedConsole.WriteImmediate("Time taken to find tuples b : {0} mil. sec.", sw.ElapsedMilliseconds);

                QueuedConsole.WriteImmediate("a: {0}, b: {1}, a concantenated b : {2}", countA, countB, countA + "" + countB);
            }

            /// <summary>
            /// https://brilliant.org/problems/longest-repeating-substring/
            /// </summary>
            void FindLongestRepeatingSubstring()
            {
                string html = Http.Request.GetHtmlResponse("https://gist.githubusercontent.com/praran26/7b744368bb68d77ca24e/raw/81cd81a4d672ac91860698ba0a04e35c1783e858/Brilliant%2520Problem");
                string[] lines = html.Splitter(StringSplitter.SplitUsing.LineBreak);
                int tScore = 0;
                lines.ForEach(s => {
                    tScore += GenericDefs.Functions.Algorithms.DP.Strings.FindLongestSubstring(s);
                });
                QueuedConsole.WriteImmediate("S : {0}", tScore);
            }

            /// <summary>
            /// https://brilliant.org/problems/fancy-function/
            /// </summary>
            void FindOutput()
            {
                Dictionary<string, int> CKeys = new Dictionary<string, int>();
                CKeys.Add(0 + ":" + 0, 0);

                Func<int, int, RecursionResult<int>> C = null;
                C = delegate (int a, int b) {
                    string key = a + ":" + b;
                    if (CKeys.ContainsKey(key)) {
                        return TailRecursion.Return(CKeys[key]);
                    }

                    RecursionResult<int> next = null;

                    if (a == 0) next = TailRecursion.Next(() => C(a, b - 1));
                    else if (b == 0) next = TailRecursion.Next(() => C(a - 1, b));
                    else if (a > b) next = TailRecursion.Next(() => C(TailRecursion.Execute(() => C(a - b, b)), b - 1));
                    else next = TailRecursion.Next(() => C(a - 1, TailRecursion.Execute(() => C(b - a, a))));

                    if (next.IsFinalResult) CKeys.Add(key, 1 + next.Result);

                    RecursionResult<int> retVal = new RecursionResult<int>(next.IsFinalResult, next.Result + 1, next.NextStep);
                    return retVal;
                };

                QueuedConsole.WriteImmediate("Final output : {0}", TailRecursion.Execute(() => C(31415, 27182)));
            }

            /// <summary>
            /// https://brilliant.org/problems/merge-prime/
            /// </summary>
            void MergePrime()
            {
                List<long> knownPrimes = KnownPrimes.GetAllKnownPrimes();
                HashSet<int> setPrimes = new HashSet<int>();
                int pow6 = (int)Math.Pow(10, 6), pow7 = (int)Math.Pow(10, 7);
                knownPrimes.ForEach(x => { if(x < pow7) { setPrimes.Add((int)x); } });
                BigInteger sum = 0;

                knownPrimes.ForEach(x =>
                {
                    if (x > pow6 && x < pow7) {
                        bool isMergePrime = false;
                        string xStr = x + "";
                        for (int i = 1; i < xStr.Length; i++)
                        {
                            string s1 = xStr.Substring(0, i);
                            string s2 = xStr.Substring(i, xStr.Length - i);

                            if (setPrimes.Contains(int.Parse(s1)) && setPrimes.Contains(int.Parse(s2))) { isMergePrime = true; break; }
                        }
                        if (isMergePrime) sum += x;
                    }
                });

                QueuedConsole.WriteImmediate("Sum of all merge primes : {0}", sum.ToString());
                QueuedConsole.WriteImmediate("Last three digits : {0}", (sum % 1000).ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/just-make-sure-your-computer-doesnt-crash/
            /// </summary>
            void TicketsSold()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/VtazT240");
                string[] lines = html.Splitter(StringSplitter.SplitUsing.LineBreak);
                List<int> players = new List<int>();
                lines.ForEach(s => {
                    players.Add(int.Parse(s));
                });
                long totalTickets = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = i + 1; j < players.Count; j++)
                    {
                        totalTickets += players[i] * players[j];
                    }
                }
                QueuedConsole.WriteImmediate("Last three digits of the total number of tickets sold in the tournament : {0}", totalTickets % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/splitting-the-plane/
            /// </summary>
            void SplittingThePlane()
            {
                HashSet<long> powersof2 = new HashSet<long>();
                int nmax = 1000000;
                long l = 1;
                long lmax = nmax;
                lmax *= nmax;
                lmax += nmax + 2;
                lmax /= 2;
                while (l <= lmax)
                {
                    l *= 2;
                    powersof2.Add(l);
                }

                int n = 0;
                int count = 0;
                while (++n <= nmax)
                {
                    long An = ((n * n) + n + 2) / 2;
                    if (powersof2.Contains(An)) count++;
                }
                QueuedConsole.WriteImmediate("Numbers : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/everyday-im-shuffling/
            /// </summary>
            void EverydayImShuffling()
            {
                string word = "GOLDENBINARY";
                string shuffledOnce = string.Empty;
                string shuffledTwice = "ANINOYGREDBL";
                char[] shufTwiceArray = shuffledTwice.ToArray();

                Func<string, int[], string> Shuffle = delegate (string input, int[] shuffleIndex)
                {
                    char[] retVal = new char[input.Length];
                    for (int i = 0; i < input.Length; i++)
                    {
                        retVal[shuffleIndex[i]] = input[i];
                    }
                    return string.Join("", retVal);
                };

                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    string s1 = Shuffle(word, coeff);
                    string s2 = Shuffle(s1, coeff);

                    if (s2.Equals(shuffledTwice)) {
                        shuffledOnce = s1;
                    }
                    
                    return s2.Equals(shuffledTwice);
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);
                BlackList<int> bl = new BlackList<int>();
                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < word.Length; i++)
                {
                    bl.Add(i, new int[1] { i });
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = word.Length - 1, Min = 0 });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                solver.Solve(word.Length, bl);
                QueuedConsole.WriteImmediate("Card reader after shuffling once : {0}", shuffledOnce);
                //Solution : shuffledOnce = "LEARNBYDOING";
                QueuedConsole.WriteImmediate("Shuffled once : {0}", Strings.WordToNumber(shuffledOnce));
            }

            /// <summary>
            /// https://brilliant.org/problems/kaboobly-dooists-visit-the-canteen/
            /// </summary>
            void Canteen()
            {
                var items = new List<Tuple<int, int>>() {
                    new Tuple<int,int>(12, 7),
                    new Tuple<int,int>(10, 9),
                    new Tuple<int,int>(7, 13),
                    new Tuple<int,int>(11, 9),
                    new Tuple<int,int>(8, 13),
                    new Tuple<int,int>(10, 10),
                    new Tuple<int,int>(8, 11),
                    new Tuple<int,int>(11, 9),
                    new Tuple<int,int>(7, 11),
                    new Tuple<int,int>(9, 12)
                };
                int MaxTuples = 5;
                List<List<int>> TwobitTupleChooser = new List<List<int>>();
                Action<List<int>, int> Add = null;
                Add = delegate (List<int> numbers, int depth)
                {                    
                    for (int i = 0; i < 2; i++)
                    {
                        List<int> after = new List<int>(numbers);
                        after.Add(i);
                        if (depth == MaxTuples - 1) { TwobitTupleChooser.Add(after); }
                        else { Add(after, depth + 1); }
                    }
                };

                Add(new List<int>(), 0);

                List<int> Tuples = new List<int>();
                Enumerable.Range(0, 10).ForEach(x => { Tuples.Add(x); });
                List<IList<int>> combinations = Combinations.GetAllCombinations(Tuples, 5);

                int MaxCost = 50;
                SimpleCounter orderingWays = new SimpleCounter();
                foreach (IList<int> c in combinations)
                {
                    foreach (List<int> choice in TwobitTupleChooser)
                    {
                        int cost = 0;
                        for (int t = 0; t < c.Count; t++)
                        {
                            cost += choice[t] == 0 ? items[c[t]].Item1 : items[c[t]].Item2;
                        }
                        if (cost <= MaxCost) orderingWays.Increment();
                    }
                }
                QueuedConsole.WriteImmediate("Number of ways to order : {0}", orderingWays.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/cities-around-amsterdam/
            /// </summary>
            void DistanceBetweenCities()
            {          
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/5yXeCA1r");
                html = html.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numStr = html.Splitter(StringSplitter.SplitUsing.Comma);
                List<long> set = new List<long>();
                numStr.ForEach(x => { set.Add(long.Parse(x)); });

                int count = 0;
                for (int i = 0; i < set.Count - 1; i++)
                {
                    for (int j = i +1 ; j < set.Count; j++)
                    {
                        if (i == j) continue;
                        if (Math.Abs(set[i] - set[j]) <= 1000) count++;
                    }
                }
                QueuedConsole.WriteImmediate("Elements in S : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/everything-is-a-palindrome/
            /// </summary>
            void EverythingIsAPalindrome()
            {
                string input = "lralhnermakjfipkseokpqlsfjhasgcnpabmpflhiebsnsljjlmlijfnmasoqjeorinsenomsagafjlgklrjnjbmgmpcgkmkrlef";
                string copyInput = input.ToString();

                int PalComp = 0;
                while(copyInput.Length > 0)
                {
                    for(int len = copyInput.Length; len >= 1; len--)
                    {
                        string s = copyInput.Substring(0, len);
                        if (GenericDefs.Functions.Algorithms.DP.Palindrome.IsPalindrome(s))
                        {
                            PalComp += 1;
                            copyInput = copyInput.Remove(0, len);
                            break;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Input length : {0}", input.Length);
                QueuedConsole.WriteImmediate("PalComp value : {0}", PalComp);
            }

            /// <summary>
            /// https://brilliant.org/problems/not-as-many-as-i-expected/
            /// </summary>
            void NotAsManyAsIExpected()
            {
                int[] numbers = new int[10] { 67, 97, 95, 99, 60, 50, 55, 37, 70, 61 };
                int nSigns = 9;

                HashSet<int> Set = new HashSet<int>();
                Action<List<int>, int> Recursion = null;
                Recursion = delegate (List<int> before, int depth)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        List<int> after = new List<int>(before);
                        after.Add(i);
                        if (depth == nSigns - 1) {
                            int n = numbers[0];
                            for (int j = 0; j < after.Count; j++)
                            {
                                if (after[j] == 0) n -= numbers[j + 1];
                                if (after[j] == 1) n += numbers[j + 1];
                            }
                            Set.Add(n);
                        } else { Recursion(after, depth + 1); }
                    }
                };
                Recursion(new List<int>(), 0);

                QueuedConsole.WriteImmediate("Number of different numbers : {0}", Set.Count);
            }

            /// <summary>
            /// https://brilliant.org/problems/maximum-perimeter
            /// </summary>
            void MaximumPerimeter()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/uGkr3GjY");
                html = html.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numStr = html.Splitter(StringSplitter.SplitUsing.Comma);
                List<long> set = new List<long>();
                numStr.ForEach(x => { set.Add(long.Parse(x)); });

                set.Sort();
                for (int i = 0; i < set.Count - 1; i++)
                {
                    QueuedConsole.WriteImmediate("i : {0}", set[i]);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/palindrome-root/
            /// </summary>
            void PalindromeRoot()
            {
                string number = "4668731596684224866951378664";
                int len = number.Length;
                //string number = "121";
                int n = 0;
                while (++n > 0)
                {
                    BigInteger num = n;
                    while (true)
                    {
                        string nStr = num.ToString();
                        if (nStr.Length > len) break;
                        string nRev = string.Join("", nStr.Reverse());
                        if (nStr.Equals(nRev)) break;

                        num += BigInteger.Parse(nRev);
                    }
                    if (num.ToString().Equals(number)) break;
                }
                QueuedConsole.WriteImmediate("Palindromic root of {0} is {1}", number, n);
            }

            /// <summary>
            /// https://brilliant.org/problems/domino-tiling-3/
            /// </summary>
            void DominoTiling()
            {
                //int m = 3, n = 4;
                int m = 5, n = 6;
                int mn = m * n;
                Dictionary<int, List<int>> HorizontalTiling = new Dictionary<int, List<int>>();
                Dictionary<int, List<int>> VerticalTiling = new Dictionary<int, List<int>>();
                Enumerable.Range(0, m * n).ForEach(x => {
                    List<int> xhMatches = new List<int>();
                    List<int> xvMatches = new List<int>();
                    if (x % n == 0) {
                        if (x != 0) { xvMatches.Add(x - n); }
                        if(x != (m - 1) * n) { xvMatches.Add(x + n); }
                        xhMatches.Add(x + 1);
                    } else if (x+1 % n == 0) {
                        if (x != n-1) { xvMatches.Add(x - n); }
                        if (x != (m * n) - 1) { xvMatches.Add(x + n); }
                        xhMatches.Add(x - 1);
                    } else if (x < n) {
                        xhMatches.Add(x - 1);
                        xhMatches.Add(x + 1);
                        xvMatches.Add(x + n);
                    } else if (x > (n * (m - 1)) - 1) {
                        xhMatches.Add(x - 1);
                        xhMatches.Add(x + 1);
                        xvMatches.Add(x - n);
                    } else {
                        xhMatches.Add(x - 1);
                        xhMatches.Add(x + 1);
                        xvMatches.Add(x + n);
                        xvMatches.Add(x - n);
                    }
                    HorizontalTiling.Add(x, xhMatches);
                    VerticalTiling.Add(x, xvMatches);
                });

                long counter = 0;
                Action<int, List<int>> Tiling = null;
                Tiling = delegate (int depth, List<int> tiles)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        List<int> after = new List<int>(tiles);
                        after.Add(i);
                        if (depth < mn - 1) {
                            Tiling(depth + 1, after);
                        } else {
                            HashSet<int> UsedTiles = new HashSet<int>();
                            bool IsPositiveArrangement = false;
                            for (int j = 0; j < after.Count; j++)
                            {
                                if (UsedTiles.Contains(j)) continue;

                                IsPositiveArrangement = false;
                                if(after[j] == 0) {
                                    foreach (int a in HorizontalTiling[j])
                                    {
                                        if (!UsedTiles.Contains(a) && after[a] == 0) { IsPositiveArrangement = true; UsedTiles.Add(j); UsedTiles.Add(a); break; }
                                    }
                                    if (!IsPositiveArrangement) break;
                                } else {
                                    foreach(int a in VerticalTiling[j]){
                                        if (!UsedTiles.Contains(a) && after[a] == 1) { IsPositiveArrangement = true; UsedTiles.Add(j); UsedTiles.Add(a);  break; }
                                    }
                                    if (!IsPositiveArrangement) break;
                                }
                                if (!IsPositiveArrangement) break;
                            }
                            //if (IsPositiveArrangement) counter++;
                            if (IsPositiveArrangement)
                            {
                                counter++;
                                QueuedConsole.WriteImmediate("Solution : {0}", counter);
                                QueuedConsole.WriteImmediate("------------------");
                                string line = string.Empty;
                                for (int j = 0; j < after.Count; j++)
                                {
                                    line += after[j] == 1 ? " | " : " = ";
                                    if ((j + 1) % n == 0) { QueuedConsole.WriteImmediate(line); line = string.Empty; }
                                }
                                QueuedConsole.WriteImmediate("------------------");
                            }
                        }
                    }
                };

                Tiling(0, new List<int>());
                QueuedConsole.WriteImmediate("Number of ways : {0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/this-is-really-an-addition-problem/
            /// </summary>
            void AnAdditionProblem()
            {
                string input = @"40996 39880 7067 5949 20530 47099 14939 34263 4852 12571 28305 4001 50754 57793 1920 15349 47698 64067 10670 38711 47814 34815 46776 53651 10463 58048 43572 951 14204 25471 11327 54096 3704 11969 53390 50631 49569 37005 54003 27852 54179 44950 28408 60604 15526 17366 62307 4639 30484 31694 5265 61381 39165 41980 39645 12285 50148 42467 34529 49793 24905 38057 10260 63377 58100 52311 32614 56288 56656 8047 45041 8947 619 9617 64585 6217 56633 46346 55014 48435 31879 60204 13174 62431 18602 18302 64238 44897 56200 14887 52644 48527 21859 29388 31169 47749 32070 62909 22325 12037";
                string[] numbers = input.Splitter(StringSplitter.SplitUsing.Space, StringSplitOptions.RemoveEmptyEntries);
                List<long> set = new List<long>();
                numbers.ForEach(x => { set.Add(long.Parse(x)); });
                long sum = 0;
                BigInteger sum1 = 0;
                while (set.Count >= 2)
                {
                    set.Sort();
                    sum = set[0] + set[1];
                    set.RemoveAt(0); set.RemoveAt(0);
                    set.Add(sum);
                    sum1 += sum;
                }
                QueuedConsole.WriteImmediate("Minimum total cost : {0}", sum1.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-beakal/
            /// https://brilliant.org/problems/specific-height/
            /// </summary>
            void PossibleBSTs()
            {
                int height = 6;
                SimpleCounter c = new SimpleCounter();
                List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
                IEnumerable<IEnumerable<int>> p = Permutations.GetPermutations(numbers, 7);
                QueuedConsole.WriteImmediate("Permutations : {0}", p.Count());
                Dictionary<int, int> Trees = new Dictionary<int, int>();
                HashSet<string> TreesWithHeightN = new HashSet<string>();
                p.ForEach(x =>
                {
                    GenericDefs.Classes.Collections.BinaryTree<int> bTree = new GenericDefs.Classes.Collections.BinaryTree<int>(true);
                    x.ForEach(y => { bTree.InsertNode(y); });
                    
                    int bTreeHeight = bTree.GetHeight();
                    Trees.GenericAddOrIncrement(bTreeHeight, 1);
                    if (bTreeHeight == height) {
                        if (TreesWithHeightN.Add(bTree.GetTreeSignature()))
                        {
                            c.Increment();
                        }
                    }
                });
                
                QueuedConsole.WriteImmediate("Number of possible bst's with height : {0} is {1}", height, c.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-beakal/
            /// https://brilliant.org/problems/specific-height/
            /// https://brilliant.org/problems/-58/
            /// </summary>
            void PossibleBSTWithBestPacking()
            {
                int height = 2;
                SimpleCounter c = new SimpleCounter();
                List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
                IEnumerable<IEnumerable<int>> p = Permutations.GetPermutations(numbers, 7);
                QueuedConsole.WriteImmediate("Permutations : {0}", p.Count());
                Dictionary<int, int> Trees = new Dictionary<int, int>();
                HashSet<string> TreesWithHeightN = new HashSet<string>();
                p.ForEach(x =>
                {
                    GenericDefs.Classes.Collections.BinaryTree<int> bTree = new GenericDefs.Classes.Collections.BinaryTree<int>(true);
                    x.ForEach(y => { bTree.InsertNode(y); });

                    int bTreeHeight = bTree.GetHeight();
                    Trees.GenericAddOrIncrement(bTreeHeight, 1);
                    if (bTreeHeight == height)
                    {
                        if (TreesWithHeightN.Add(bTree.GetTreeSignature()))
                        {
                            c.Increment();
                        }
                    }
                });

                QueuedConsole.WriteImmediate("Number of possible bst's with height : {0} is {1}", height, c.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/throwing-numbers/
            /// </summary>
            void ThrowingNumbers1()
            {
                int target = 20;
                List<int> Numbers = new List<int>() { 1, 2, 3, 4, 5, 6 };

                int LastAddedNumber = Numbers[Numbers.Count - 1];
                long S = 0;
                HashSet<int> FirstNumbersSet = new HashSet<int>();
                while (Numbers[0] != target)
                {

                    int first = Numbers[0];
                    int second = Numbers[1];
                    if (Numbers.Count < second + 1)
                    {
                        while (Numbers.Count < second + 1)
                        {
                            Numbers.Add(++LastAddedNumber);
                        }
                        QueuedConsole.WriteImmediate("Number added at Sequence : {0}, Last three digits : {1}", S, S % 1000);
                    }
                    if (!FirstNumbersSet.Contains(first))
                    {
                        FirstNumbersSet.Add(first);
                        QueuedConsole.WriteImmediate("New first number found at Sequence {0}, First number {1}, Number Count : {2}", S, Numbers[0], Numbers.Count);
                    }
                    Numbers.Insert(second + 1, first);
                    Numbers.RemoveAt(0);
                    S++;
                }
                QueuedConsole.WriteImmediate("Last 3 digits of S : {0}", S % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/throwing-numbers-ii/
            /// </summary>
            void ThrowingNumbers2()
            {
                // This is a sequence where a number n will be added to the top of the sequence after 2^(n-1) throws.
                int target = 1000;
                
                int n = 0;
                BigInteger S2 = 1;
                while(++n <= target-1)
                {
                    S2 *= 2;
                }
                QueuedConsole.WriteImmediate("Last Three digits Of S : {0}", S2%1000);
                QueuedConsole.WriteImmediate("The second sequence S where {0} is at the top of the sequence: {1}", target, (2*S2).ToString());

                List<int> Numbers = new List<int>() { 1, 2, 3, 4, 5, 6 };
                
                int LastAddedNumber = Numbers[Numbers.Count - 1];
                long S = 1;
                HashSet<int> FirstNumbersSet = new HashSet<int>();
                while (Numbers[0] != target)
                {
                    
                    int first = Numbers[0];
                    if (Numbers.Count < first + 1)
                    {
                        while (Numbers.Count < first + 1)
                        {
                            Numbers.Add(++LastAddedNumber);
                        }
                        QueuedConsole.WriteImmediate("Number added at Sequence : {0}, Last three digits : {1}", S, S % 1000);
                    }
                    if (!FirstNumbersSet.Contains(first))
                    {
                        FirstNumbersSet.Add(first);
                        QueuedConsole.WriteImmediate("New first number found at Sequence {0}, First number {1}, Number Count : {2}", S, Numbers[0], Numbers.Count);
                    }
                    Numbers.Insert(first + 1, first);
                    Numbers.RemoveAt(0);
                    S++;
                }
                QueuedConsole.WriteImmediate("Last 3 digits of S : {0}", S % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/at-least-you-got-this-in-i-land/
            /// </summary>
            void AtleastYouGotThisInILand()
            {
                BigInteger _200Factorial = PermutationsAndCombinations.Factorial(200);
                BigInteger _100Factorial = PermutationsAndCombinations.Factorial(100);
                BigInteger _2pow100 = 1;
                Enumerable.Range(0, 100).ForEach(x => { _2pow100 *= 2; });
                BigInteger number = _100Factorial * _2pow100;
                number += _200Factorial + 3;
                List<long> primes = KnownPrimes.GetAllKnownPrimes();
                foreach(long a in primes)
                {
                    if (a > 3)
                    {
                        if (number % a == 0)
                        {
                            QueuedConsole.WriteImmediate("Smallest prime p : {0}", a);
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-sal-gard/
            /// </summary>
            void SecondSmallesFactorMultiFactorial()
            {
                BigInteger _200Factorial = PermutationsAndCombinations.Factorial(200);
                BigInteger number = _200Factorial + 3;

                List<long> primes = KnownPrimes.GetAllKnownPrimes();
                foreach (long a in primes)
                {
                    if (a > 3)
                    {
                        if (number % a == 0)
                        {
                            QueuedConsole.WriteImmediate("Second smallest prime factor is less than equal to : {0}", a);
                            break;
                        }
                    }
                }

                int n = 1, nmax = 9;
                while (++n < nmax)
                {
                    BigInteger nFact = PermutationsAndCombinations.Factorial(n);
                    BigInteger result = TailRecursion.Execute(() => PermutationsAndCombinations.Factorial((int)nFact, 1));
                    BigInteger s = result + nFact + 3;
                    int count = 0;
                    foreach (long a in primes)
                    {
                        if (s % a == 0)
                        {
                            count++;
                            QueuedConsole.WriteImmediate("For n : {0}, {1}th prime factor is : {2}", n, count, a);
                            if(a >= s || count == 2) break;
                        }
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/hash-crash-finals/
            /// </summary>
            void HashCrashCollision()
            {
                int s = 1;
                int n = 1000;
                double lhsmin = double.MaxValue;
                while (++s < n)
                {
                    double d = ((n - 1.0) / n);

                    double lhs = Math.Pow(d, s);
                    lhs = 1.0 - lhs;
                    lhs *= n;
                    lhs /= s;
                    lhsmin = Math.Min(lhsmin, lhs);
                    if(lhs <= 0.5) {
                        QueuedConsole.WriteImmediate("Minimum s : {0}", s);
                        break;
                    }
                }
                QueuedConsole.WriteImmediate("Min p : {0}", lhsmin);
            }

            /// <summary>
            /// https://brilliant.org/problems/rtevsbc-cit-ktggqut/
            /// </summary>
            void CrackTheAuthor()
            {
                string cipherText = @"cit yvthftnes jy mtcctvg on ctzc iqg wttn gcfrotr yjv fgt on evsbcqnqmsgog 
                    qnr yvthftnes qnqmsgog on bqvcoefmqv rqconu wqel cj cit ovqho 
                    kqcitkqcoeoqn qm lonro dij yjvkqmms rtxtmjbtr cit ktcijr cit eobitvg 
                    wvtqlqwmt ws ciog cteinohft uj wqel qc mtqgc cj cit eqtgqv eobitv 
                    onxtnctr ws pfmofg eqtgqv gj ciog ktcijr ejfmr iqxt wttn tzbmjvtr 
                    on emqggoeqm coktg";
                string[] words = cipherText.Splitter(StringSplitter.SplitUsing.Space, StringSplitOptions.RemoveEmptyEntries);
                int n = 0;
                while (++n <= 25)
                {
                    string decryptedText = string.Empty;
                    decryptedText += GenericDefs.Functions.Crypto.Cypher.Caeser.Encipher(words[0], n) + " ";
                    //words.ForEach(x => { decryptedText += GenericDefs.Functions.Crypto.Cypher.Caeser.Encipher(x, n) + " "; });
                    QueuedConsole.WriteImmediate("Rotation : {0}, Text : {1}", n, decryptedText);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/chaotic-sequence/
            /// </summary>
            void ChaoticSequence()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/JrBL9Rau");
                string[] qStrings = html.Splitter(StringSplitter.SplitUsing.LineBreak);

                List<dynamic> Queries = new List<dynamic>();
                qStrings.ForEach(x => {
                    string[] qTokens = x.Splitter(StringSplitter.SplitUsing.Space);
                    Queries.Add(new { k = int.Parse(qTokens[0]), qType = qTokens[1], n = int.Parse(qTokens[2]) });
                });

                Queries.OrderBy(x => x.k);

                HashSet<int> kCollection = new HashSet<int>();
                Queries.ForEach(x => { kCollection.Add(x.k); });

                int n = 10;
                Action<int, List<int>> Generate = null;
                int lexicOrder = 0;
                int sum = 0;
                Generate = delegate (int position, List<int> soFar) {
                    for (int i = 1; i <= n; i++)
                    {
                        List<int> next = new List<int>(soFar);
                        if (next.Contains(i)) continue;

                        next.Add(i);
                        if (position == n - 1)
                        {
                            lexicOrder++;
                            if (kCollection.Contains(lexicOrder))
                            {
                                (Queries.Where(x => x.k == lexicOrder)).ForEach(y =>
                                    {
                                        if(y.qType == "P") {
                                            int p = 0;
                                            foreach(int w in next)
                                            {
                                                p++;
                                                if (w == y.n) { sum += p; break; }
                                            }
                                        } else {
                                            sum += next[y.n - 1];
                                        }
                                    }
                                );
                            }
                        } else { Generate(position + 1, next); }
                    }
                };

                Generate(0, new List<int>());
                QueuedConsole.WriteImmediate("Sum of all queries : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/carrot-distribution/
            /// </summary>
            void CarrotDistribution()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/fpbCnELC");
                string[] numbers = html.Splitter(StringSplitter.SplitUsing.LineBreak);
                List<int> bags = new List<int>();
                numbers.ForEach(x => { bags.Add(int.Parse(x)); });

                List<int> Villagers = new List<int>();
                bags.ForEach(y =>
                {
                    if (Villagers.Count == 0 || Villagers.Count == 1) Villagers.Add(y);
                    else {
                        if (Villagers[Villagers.Count - 1] < Villagers[Villagers.Count - 2]) { Villagers[Villagers.Count - 1] = Villagers[Villagers.Count - 1] + y; }
                        else Villagers.Add(y);
                    }
                });

                if (Villagers[Villagers.Count - 1] < Villagers[Villagers.Count - 2]) { Villagers.RemoveAt(Villagers.Count - 1); }

                int n = 0;
                Villagers.ForEach(y =>
                {
                    bool isSatisfied = n == 0 ? true : Villagers[n] >= Villagers[n - 1];
                    QueuedConsole.WriteImmediate("Villager : {0}, Carrot count: {1}, Has more or equal carrots than previous villager : {2}", ++n, y, isSatisfied);
                });

                QueuedConsole.WriteImmediate("Satisfied villagers count : {0}", Villagers.Count);
            }

            /// <summary>
            /// https://brilliant.org/problems/carrot-distribution-ii/
            /// </summary>
            void CarrotDistribution2()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/fpbCnELC");
                string[] numbers = html.Splitter(StringSplitter.SplitUsing.LineBreak);
                List<int> bags = new List<int>();
                numbers.ForEach(x => { bags.Add(int.Parse(x)); });

                int N = 1000;
                List<int> dp = new List<int>(N), last = new List<int>(N), ps = new List<int>(N);
                dp[0] = 0; last[0] = 0;
                for (int i = 1; i <= N; i++)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (ps[i] - ps[j] >= last[j])
                        {
                            dp[i] = dp[j] + 1;
                            last[i] = ps[i] - ps[j];
                            break;
                        }
                    }
                }

                QueuedConsole.WriteImmediate("Satisfied villagers count : {0}", dp[N]);
            }

            /// <summary>
            /// https://brilliant.org/problems/4096-made-hard/
            /// </summary>
            void _4096MadeHard()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/rcCVvfWS");
                string[] Instructions = html.Splitter(StringSplitter.SplitUsing.LineBreak);

                int n = 4;
                List<int> Tiles = new List<int>() { 0, 0, 2, 4, 0, 0, 4, 8, 0, 2, 16, 32, 0, 2, 2, 16 };
                List<List<int>> Rows = new List<List<int>>() {
                    new List<int>() { 0, 1, 2, 3 },
                    new List<int>() { 4, 5, 6, 7 },
                    new List<int>() { 8, 9, 10, 11 },
                    new List<int>() { 12, 13, 14, 15 }
                };
                List<List<int>> Columns = new List<List<int>>() {
                    new List<int>() { 0, 4, 8, 12 },
                    new List<int>() { 1, 5, 9, 13 },
                    new List<int>() { 2, 6, 10, 14 },
                    new List<int>() { 3, 7, 11, 15 }
                };

                Action<char> Swipe = delegate (char d) {
                    int t = 0;
                    bool isVertical = false;
                    bool isForwardMovement = false;
                    if (d.Equals('l')) t = -1;
                    else if (d.Equals('r')) { t = 1; isForwardMovement = true; }
                    else if (d.Equals('d')) { t = n; isVertical = true; isForwardMovement = true; }
                    else if (d.Equals('u')) { t = -n; isVertical = true; }
                    else { QueuedConsole.WriteImmediate("Token not found. {0}", d); }

                    while (true)
                    {
                        HashSet<int> MergedTiles = new HashSet<int>();
                        int actions = 0;
                        for (int i = 1; i <= n - 1; i++)
                        {
                            int index = isForwardMovement ? n - i - 1 : i;
                            var LineOfTiles = isVertical ? Rows[index] : Columns[index];
                            LineOfTiles.ForEach(x =>
                            {
                                if (Tiles[x] != 0)
                                {
                                    if (Tiles[x + t] == 0) {
                                        Tiles[x + t] = Tiles[x];
                                        Tiles[x] = 0;
                                        actions++;
                                    }
                                    else if (Tiles[x + t] == Tiles[x] && !MergedTiles.Contains(x + t) && !MergedTiles.Contains(x)) {
                                        Tiles[x + t] *= 2;
                                        Tiles[x] = 0;
                                        MergedTiles.Add(x + t);
                                        actions++;
                                    }
                                }
                            });
                        }
                        if (actions == 0) {
                            break;
                        }
                    }
                };

                //int instructionCount = 0;
                Instructions.ForEach(a =>
                {
                    string[] tokens = a.Splitter(StringSplitter.SplitUsing.Space);

                    char token = (tokens[0].ToLower())[0];
                    if (token.Equals('p')) {
                        int index = n * (int.Parse(tokens[1]) - 1) + (int.Parse(tokens[2]) - 1);
                        Tiles[index] = int.Parse(tokens[3]);
                    } else { Swipe(token); }
                    //if(++instructionCount % 5 == 0)
                    //{
                    //    QueuedConsole.WriteImmediate("Sum of numbers in the grid after {0} instructions is : {1}", instructionCount, Tiles.Sum());
                    //}
                });

                QueuedConsole.WriteImmediate("Sum of numbers in the grid : {0}", Tiles.Sum());
            }

            /// <summary>
            /// https://brilliant.org/problems/erratic-systematic-wormhole-mathematics/
            /// https://brilliant.org/problems/erratic-systematic-wormhole-variance/
            /// </summary>
            void ErraticSystematicWormholeMathematics()
            {
                int nmax = 100000;
                long nmaxsq = nmax * nmax;
                int nSimulations = 1000000;
                Random r = new Random();
                int takes = 10, take = 0;
                double averageDisplacement = 0.0;
                while (++take <= takes)
                {
                    int n = 0;
                    double totalDisplacement = 0.0;
                    while (++n <= nSimulations)
                    {
                        double x1 = r.NextDouble();
                        double y1 = r.NextDouble();
                        double x2 = r.NextDouble();
                        double y2 = r.NextDouble();

                        double d = Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2);
                        d = Math.Sqrt(d);

                        totalDisplacement += d;

                        if (n % nSimulations == 0)
                        {
                            QueuedConsole.WriteImmediate("n : {0}, Average displacement : {1}", n, totalDisplacement / n);
                        }
                    }

                    --n;
                    averageDisplacement += totalDisplacement / n;
                }

                --take;
                QueuedConsole.WriteImmediate("Average displacement after {0} takes x {1}simulations : {2}", takes, nSimulations, averageDisplacement / takes);
            }

            /// <summary>
            /// https://brilliant.org/problems/erratic-systematic-wormhole-variance/
            /// </summary>
            void ErraticSystematicWormholeVariance()
            {
                double mu = 0.52140795617015256;

                int nmax = 100000;
                long nmaxsq = nmax * nmax;
                int nSimulations = 1000000;
                Random r = new Random();
                int takes = 10, take = 0;
                double averageDisplacement = 0.0;
                while (++take <= takes)
                {
                    int n = 0;
                    double totalDisplacement = 0.0;
                    while (++n <= nSimulations)
                    {
                        double x1 = r.NextDouble();
                        double y1 = r.NextDouble();
                        double x2 = r.NextDouble();
                        double y2 = r.NextDouble();

                        double d = Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2);

                        totalDisplacement += d;

                        if (n % nSimulations == 0)
                        {
                            QueuedConsole.WriteImmediate("n : {0}, Average E[X^2] : {1}", n, totalDisplacement / n);
                        }
                    }

                    --n;
                    averageDisplacement += totalDisplacement / n;
                }

                --take;
                double EX2 = averageDisplacement / takes;
                QueuedConsole.WriteImmediate("Average E[X^2] after {0} takes x {1} simulations : {2}", takes, nSimulations, EX2);
                QueuedConsole.WriteImmediate("Variance : {0}", EX2 - (mu * mu));
            }

            /// <summary>
            /// https://brilliant.org/problems/a-mammoth-counting/
            /// </summary>
            void AMammothCounting()
            {
                Dictionary<char, int> Alphabet = new Dictionary<char, int>() {
                    { 'A', 3 },{ 'B', 1 },{ 'C', 0 },{ 'D', 1 },{ 'E', 4 },{ 'F', 3 },{ 'G', 1 },{ 'H', 3 },{ 'I', 1 },{ 'J', 0 },{ 'K', 3 },{ 'L', 2 },{ 'M', 4 },{ 'N', 3 }
                    ,{ 'O', 0 },{ 'P', 1 },{ 'Q', 1 },{ 'R', 2 },{ 'S', 0 },{ 'T', 2 },{ 'U', 0 },{ 'V', 2 },{ 'W', 4 },{ 'X', 2 },{ 'Y', 3 },{ 'Z', 3 }
                };

                Dictionary<int, string> _1to19 = new Dictionary<int, string>() {
                    { 1, "ONE" },{ 2, "TWO" },{ 3, "THREE" },{ 4, "FOUR" },{ 5, "FIVE" },{ 6, "SIX" },{ 7, "SEVEN" },{ 8, "EIGHT" },{ 9, "NINE" },{ 10, "TEN" },
                    { 11, "ELEVEN" },{ 12, "TWELVE" },{ 13, "THIRTEEN" },{ 14, "FOURTEEN" },{ 15, "FIFTEEN" },{ 16, "SIXTEEN" },{ 17, "SEVENTEEN" }
                    ,{ 18, "EIGHTEEN" },{ 19, "NINETEEN" },
                };

                Dictionary<int, string> Tens = new Dictionary<int, string>() {
                    {2, "TWENTY" },{3, "THIRTY" },{4, "FORTY" },{5, "FIFTY" },{6, "SIXTY" }, {7, "SEVENTY" },{8, "EIGHTY" },{9, "NINETY" }
                };

                Func<string, int> LineCounter = delegate (string input)
                {
                    int retVal = 0;
                    input.ForEach(x => { retVal += Alphabet[x]; });
                    return retVal;
                };

                int n = 0;
                int sumn = 0;
                int nThousand = LineCounter("THOUSAND");
                int nHundred = LineCounter("HUNDRED");
                while (++n < 10000)
                {
                    int fn = 0;
                    int copy = n;
                    if (copy > 999)
                    {
                        int a = copy / 1000;
                        fn += nThousand + LineCounter(_1to19[a]);
                        copy = copy % 1000;
                    }

                    if (copy > 99)
                    {
                        int a = copy / 100;
                        fn += nHundred + LineCounter(_1to19[a]);
                        copy = copy % 100;
                    }

                    if (copy > 0)
                    {
                        if (copy > 19)
                        {
                            int a = copy / 10;
                            fn += LineCounter(Tens[a]);
                            copy = copy % 10;
                        }

                        if (copy > 0 && copy < 20)
                        {
                            fn += LineCounter(_1to19[copy]);
                        }
                    }
                    if (n == 12 || n == 73 || n == 105 || n == 6001)
                    {
                        QueuedConsole.WriteImmediate("n :{0}, fn :{1}", n, fn);
                    }
                    if (fn == n) { sumn += n; }
                }
                QueuedConsole.WriteImmediate("The sum of the all truthful numbers from 1 to 9999 : {0}", sumn);
            }

            /// <summary>
            /// https://brilliant.org/problems/its-still-possible-to-do-it-by-hand/
            /// </summary>
            void FourthSmallestSequence()
            {
                int n = 0;
                List<List<int>> Streaks = new List<List<int>>();
                ClonedPrimes c = KnownPrimes.CloneKnownPrimes(0, 10000);
                int streak = 0;
                while(Streaks.Count < 4)
                {
                    ++n;
                    int fCount = Factors.GetAllFactorsCount(n, c);
                    if (fCount == 4)
                    {
                        streak++;
                        if (streak >= 3)
                        {
                            Streaks.Add(new List<int>() { n - 2, n - 1, n });
                        }
                    }
                    else streak = 0;
                }
                QueuedConsole.WriteImmediate("Fourth smallest x : {0}", Streaks[3][0]);
            }

            /// <summary>
            /// https://brilliant.org/problems/everything-is-still-a-palindrome/
            /// </summary>
            void EverythingIsStillAPalindrome()
            {
                string word = Http.Request.GetHtmlResponse("http://pastebin.com/raw/em3FD1Z9");
                Dictionary<int, List<int>> StartPositions = new Dictionary<int, List<int>>();
                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = i; j < word.Length; j++)
                    {
                        string s = word.Substring(i, j - i + 1);

                        if (string.Join("", s.Reverse()).Equals(s))
                        {
                            if (StartPositions.ContainsKey(i)) { StartPositions[i].Add(j - i + 1); }
                            else { StartPositions.Add(i, new List<int>() { j - i + 1 }); }
                        }
                    }
                }

                StartPositions.ForEach(y =>
                {
                    y.Value.Sort((a, b) => -1 * a.CompareTo(b));
                });

                int StringLength = 1000;
                Action<int, int> Recursion = null;
                int minPalindromes = StringLength;

                Dictionary<int, int> MinimumPalindromes = new Dictionary<int, int>();
                Recursion = delegate (int sPos, int depth)
                {
                    if (depth >= minPalindromes) return;
                    if (MinimumPalindromes.ContainsKey(sPos))
                    {
                        if (depth + MinimumPalindromes[sPos] >= minPalindromes) return;
                    }

                    List<int> Start = StartPositions[sPos];
                    int nextStart = sPos;
                    for (int i = 0; i < Start.Count; i++)
                    {
                        nextStart = sPos + Start[i];
                        if (nextStart == StringLength) {
                            if (depth < minPalindromes) {
                                minPalindromes = depth + 1;
                            }
                        } else if (StartPositions.ContainsKey(Start[i] + sPos)) {
                            Recursion(nextStart, depth + 1);
                        }
                    }
                };
                
                for (int y = 999; y >= 0; y--)
                {
                    minPalindromes = StringLength;
                    Recursion(y, 0);
                    MinimumPalindromes.Add(y, minPalindromes);
                    QueuedConsole.WriteImmediate("Min. Palindromes required for StartPosition : {0} : {1}", y, minPalindromes);
                }

                QueuedConsole.WriteImmediate("The minimum number of palindromes we need to construct the string: {0}", minPalindromes);
            }

            /// <summary>
            /// https://brilliant.org/problems/can-you-fill-in-these-boxes/
            /// </summary>
            void CanYouFillInTheseBoxes()
            {
                int maxValue = int.MinValue, nDigits = 10;
                HashSet<int> nonZero = new HashSet<int>() { 0, 2, 4, 7 } ;
                Action<int, List<int>> Fill = null;
                Fill = delegate (int depth, List<int> used)
                {
                    int min = 0, max = 9;
                    if (nonZero.Contains(depth)) min = 1;
                    for (int i = min; i <= max; i++)
                    {
                        if (used.Contains(i)) continue;
                        List<int> next = new List<int>(used);
                        next.Add(i);
                        if (depth == nDigits - 1) {
                            int n1 = next[0] * 10 + next[1];
                            int n2 = next[2] * 10 + next[3];
                            int n3 = next[4] * 100 + next[5] * 10 + next[6];
                            int n4 = next[7] * 100 + next[8] * 10 + next[9];

                            int lhs = n1 * n2, rhs = n3 + n4;
                            if (lhs == rhs)
                            {
                                if (lhs < 1000) maxValue = Math.Max(maxValue, lhs);
                            }
                        }
                        else { Fill(depth + 1, next); }
                    }
                };

                Fill(0, new List<int>());
                QueuedConsole.WriteImmediate("Maximum 3 digit value : {0}", maxValue);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-hidden-ap/
            /// </summary>
            void TheHiddenAP()
            {
                string html = Http.Request.GetHtmlResponse(
                    "https://gist.githubusercontent.com/anonymous/7bbd0959b81b2e33589f00adce7a1b2b/raw/a573b4d0c38d6ca367d407c29ea7cec08871252f/gistfile1.txt");
                string[] numbers = html.Splitter(StringSplitter.SplitUsing.Space);
                List<int> set = new List<int>();
                numbers.ForEach(x => { set.Add(int.Parse(x)); });
                set.Sort();
                HashSet<int> s = new HashSet<int>(set);

                int diffMax = set[set.Count - 1] - set[0];
                int CardinalityMax = int.MinValue;
                for (int n = 1; n <= 500; n++)
                {
                    for (int i = 0; i < set.Count; i++)
                    {
                        int sqCount = 1;
                        int curr = set[i];
                        while (true) { 
                            if(s.Contains(curr + n)) { sqCount++; curr += n; }
                            else { break; }
                        }
                        CardinalityMax = Math.Max(CardinalityMax, sqCount);
                    }
                }
                QueuedConsole.WriteImmediate("Largest possible value of |X| : {0}", CardinalityMax);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-lonely-loner/
            /// </summary>
            void TheLonelyLoner()
            {
                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/j8dZqzrY");
                text = text.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numbers = text.Splitter(StringSplitter.SplitUsing.Comma);

                int xor = 0;
                numbers.ForEach(x => { xor = GenericDefs.Functions.Logic.Bitwise.XOR(xor, int.Parse(x)); });

                QueuedConsole.WriteImmediate("Loner is : {0}", xor);
            }

            /// <summary>
            /// https://brilliant.org/problems/help-me-grow-carrots/
            /// </summary>
            void HelpMeGrowCarrots()
            {
                int n = 1001;
                int[,] farm = new int[n, n];

                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/AUFZqh8u");
                text = text.Replace("[", "").Replace("]", "");
                string[] lines = text.Splitter(StringSplitter.SplitUsing.LineBreak);
                lines.ForEach(x =>
                {
                    List<int> nList = Strings.ExtractIntegersFromString(x);
                    farm[nList[0], nList[1]] = 1;
                });

                GenericDefs.Functions.Algorithms.DP.ArrayMatrix.MaxRectangularSubmatrix(farm);
            }

            /// <summary>
            /// https://brilliant.org/problems/counting-more-rectangles/
            /// </summary>
            void CountingRectangles()
            {
                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/ZeXdX0ZK");
                string[] points = text.Splitter(StringSplitter.SplitUsing.LineBreak);

                //int nRectangles = 0;
                int nACount = 0;
                List<Tuple<int, int>> Quadrilateral = new List<Tuple<int, int>>();

                Func<Tuple<int, int>, Tuple<int, int>, int> Distance = delegate (Tuple<int, int> p1, Tuple<int, int> p2)
                {
                    return (int)Math.Pow(p1.Item1 - p2.Item1, 2) + (int)Math.Pow(p1.Item2 - p2.Item2, 2);
                };

                points.ForEach(x =>
                {
                    string[] xy = x.Splitter(StringSplitter.SplitUsing.Space);
                    Quadrilateral.Add(new Tuple<int, int>(int.Parse(xy[0]), int.Parse(xy[1])));

                    if (Quadrilateral.Count == 4)
                    {
                        Quadrilateral.Sort();

                        //Dictionary<int, int> Distances = new Dictionary<int, int>();
                        //for (int a = 0; a < 4; a++)
                        //{
                        //    Tuple<int, int> p1 = Quadrilateral[a];
                        //    for (int b = a + 1; b < 4; b++)
                        //    {
                        //        Tuple<int, int> p2 = Quadrilateral[b];

                        //        int d = (int)Math.Pow(p1.Item1 - p2.Item1, 2) + (int)Math.Pow(p1.Item2 - p2.Item2, 2);
                        //        Distances.AddOrUpdate(d, 1);
                        //    }
                        //}

                        if (Distance(Quadrilateral[0], Quadrilateral[1]) == Distance(Quadrilateral[2], Quadrilateral[3]))
                        {
                            if (Distance(Quadrilateral[0], Quadrilateral[3]) == Distance(Quadrilateral[2], Quadrilateral[1]))
                            {
                                if (Distance(Quadrilateral[0], Quadrilateral[2]) == Distance(Quadrilateral[3], Quadrilateral[1]))
                                {
                                    nACount++;
                                }
                            }
                        }

                        //if (Distances.Keys.Count == 2)
                        //{
                        //    if (Distances.Keys.Min() * 2 == Distances.Keys.Max())
                        //    {
                        //        if (Distances.Values.Sum() == 6 && Distances.Values.Max() == 4)
                        //        {
                        //            nRectangles++;
                        //        }
                        //    }
                        //} else if (Distances.Keys.Count == 3) {
                        //    if (Distances.Keys.Sum() == 2 * Distances.Keys.Max())
                        //    {
                        //        if (Distances.Values.Sum() == 6 && Distances.Values.Max() == 2)
                        //        {
                        //            nRectangles++;
                        //        }
                        //    }
                        //}

                        Quadrilateral.Clear();
                    }
                });

                QueuedConsole.WriteImmediate("Number of rectangles is : {0}", nACount);
            }

            /// <summary>
            /// https://brilliant.org/problems/how-many-rectangles-4/
            /// </summary>
            void CountingRectangles2()
            {
                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/21v3jtXU");
                string[] points = text.Splitter(StringSplitter.SplitUsing.LineBreak);

                int nRectangles = 0;
                List<List<int>> Coordinates = new List<List<int>>();
                points.ForEach(x =>
                {
                    string[] xy = x.Splitter(StringSplitter.SplitUsing.Space);
                    Coordinates.Add(new List<int>() { int.Parse(xy[0]), int.Parse(xy[1]), int.Parse(xy[2]), int.Parse(xy[3]) });

                    if (Coordinates.Count == 2)
                    {
                        int nParallelCount = 0;
                        Dictionary<int, int> Distances = new Dictionary<int, int>();
                        for (int a = 0; a < 4; a++)
                        {
                            int x1 = Coordinates[0][a], y1 = Coordinates[1][a];
                            for (int b = a + 1; b < 4; b++)
                            {
                                int x2 = Coordinates[0][b], y2 = Coordinates[1][b];

                                if (x1 == x2 || y1 == y2) { nParallelCount++; }

                                int d = (int)Math.Pow(x1 - x2, 2) + (int)Math.Pow(y1 - y2, 2);
                                Distances.AddOrUpdate(d, 1);
                            }
                        }

                        if (nParallelCount == 4)
                        {
                            if (Distances.Keys.Count == 2)
                            {
                                if (Distances.Keys.Min() * 2 == Distances.Keys.Max())
                                {
                                    if (Distances.Values.Sum() == 6 && Distances.Values.Max() == 4)
                                    {
                                        nRectangles++;
                                    }
                                }
                            }
                            else if (Distances.Keys.Count == 3)
                            {
                                if (Distances.Keys.Sum() == 2 * Distances.Keys.Max())
                                {
                                    if (Distances.Values.Sum() == 6 && Distances.Values.Max() == 2)
                                    {
                                        nRectangles++;
                                    }
                                }
                            }
                        }
                        
                        Coordinates.Clear();
                    }
                });

                QueuedConsole.WriteImmediate("Number of rectangles is : {0}", nRectangles);
            }

            /// <summary>
            /// https://brilliant.org/problems/thats-all-im-given/
            /// </summary>
            void ThatsAllImGiven()
            {
                int n = 0;
                while (++n>0)
                {
                    BigInteger nFac = PermutationsAndCombinations.Factorial(n);

                    string nFacStr = nFac.ToString();
                    int Snfac = 0;
                    nFacStr.ForEach(x =>
                    {
                        Snfac += (int)char.GetNumericValue(x);
                    });

                    if(nFac % Snfac != 0) break;
                }
                QueuedConsole.WriteImmediate("Smallest n : {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/problems/loud-string/
            /// </summary>
            void LoudString()
            {
                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/63DdpJu9");

                int nSwitches = 0;
                string match = "1010101";
                if (text.IndexOf(match) >= 0)
                {
                    nSwitches += Regex.Matches(text, match).Count;
                    text = text.Replace(match, "1000101");
                }

                match = "10101";
                if (text.IndexOf(match) >= 0)
                {
                    nSwitches += Regex.Matches(text, match).Count;
                    text = text.Replace(match, "10001");
                }

                match = "101";
                if (text.IndexOf(match) >= 0)
                {
                    nSwitches += Regex.Matches(text, match).Count;
                    text = text.Replace(match, "100");
                }

                QueuedConsole.WriteImmediate("Minimum number of switches: {0}", nSwitches);
            }
        }

        internal class Quizzes
        {
            internal void Solve()
            {
                TheFirstEasyNumberGame();
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-basic/?problem=computer-science-problem-47777
            /// </summary>
            void BiggestAverage()
            {
                string data = @"[12, 10, 8, 36, 12, 10, 0, 20, 0, 2]
                    [28, 29, 11, 29, 2, 6, 4, 7, 13, 32]
                    [21, 32, 32, 12, 31, 20, 16, 6, 7, 11]
                    [32, 36, 17, 5, 10, 30, 20, 7, 33, 11]
                    [28, 10, 21, 8, 15, 15, 38, 30, 13, 4]
                    [16, 25, 15, 35, 4, 14, 22, 22, 39, 17]
                    [18, 5, 11, 6, 34, 8, 21, 3, 19, 22]
                    [1, 15, 38, 33, 17, 1, 3, 25, 22, 0]
                    [31, 1, 6, 2, 2, 14, 37, 27, 14, 14]
                    [2, 16, 2, 18, 16, 28, 25, 30, 8, 23]";

                data = data.Replace("[", "");
                double MaxAverage = double.MinValue;
                string[] arrays = data.Split(new char[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string a in arrays)
                {
                    string ai = a.Replace(" ", "");
                    string[] ais = ai.Splitter(StringSplitter.SplitUsing.Comma);
                    int total = 0;
                    foreach(string aij in ais)
                    {
                        int aijNum = int.Parse(aij);
                        if (aijNum > 20) aijNum = 20;
                        total += aijNum;
                    }
                    MaxAverage = Math.Max(MaxAverage, total * 1.0 / ais.Length);
                }
                QueuedConsole.WriteImmediate("Maximum average : {0}", MaxAverage);
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-basic/?problem=computer-science-problem-47761
            /// </summary>
            void Sum()
            {
                int[] a = new int[] { 20, 24, 22, 13, 34, 13, 14, 33, 41, 10, 35, 1, 2, 24, 16, 20, 16, 23, 46, 41, 31, 7, 49, 25, 34, 15, 17, 18, 1, 30, 1, 17, 23, 43, 10, 4, 48, 44, 24, 23, 30, 0, 34, 30, 33, 27, 20, 42, 25, 5 };
                Array.Sort(a);
                Array.Reverse(a);
                List<int> top5 = new List<int>();
                System.Collections.IEnumerator e = a.GetEnumerator();
                while (e.MoveNext())
                {
                    int cur = (int)e.Current;
                    if (!top5.Contains(cur)) top5.Add(cur);

                    if (top5.Count == 5) break;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", top5[2] + top5[4]);
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-basic/?problem=computer-science-problem-47765
            /// </summary>
            void SumSameRemainders()
            {
                int sum = 0;
                int[] a = new int[] { 1, 28, 12, 31, 11, 5, 4, 30, 30, 8, 9, 39, 2, 5, 33, 33, 37, 5, 12, 27, 23, 39, 1, 36, 28, 33, 24, 5, 27, 36 };
                a.ForEach(x => { if (x % 6 == 4) sum += x; });
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-and-lists-level-2-3-challenges/?problem=prime-sum-2
            /// </summary>
            void PrimeSum2()
            {
                int n = 1;
                int sum = 0;
                while (true)
                {
                    int t = n * n + 1;
                    if (t >= 1000) break;

                    if (Prime.IsPrime(t)) sum += t;
                    n++;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-and-lists-level-2-3-challenges/?problem=a-confused-calculator
            /// </summary>
            void AConfusedCalculator()
            {
                Stack<int> s = new Stack<int>();
                s.Push(1); s.Push(2); s.Push(3); s.Push(4); s.Push(5);
                s.Push(s.Pop() + s.Pop());
                s.Push(s.Pop() * s.Pop()); s.Push(s.Pop() + s.Pop());
                s.Push(s.Pop() * s.Pop());
                QueuedConsole.WriteImmediate("Stack numbers are.");
                while(s.Count > 0)
                {
                    QueuedConsole.WriteImmediate("{0}", s.Pop());
                }
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-and-lists-level-4-5-challenges/?problem=the-missing-numbers
            /// </summary>
            void TheMissingNumbers()
            {
                string html = Http.Request.GetHtmlResponse("https://d18l82el6cdm1i.cloudfront.net/uploads/documents/jx6YkhKu-LhQlf8pUGd.txt");
                html = html.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numStr = html.Splitter(StringSplitter.SplitUsing.Comma);
                HashSet<int> set = new HashSet<int>();
                numStr.ForEach(x => { set.Add(int.Parse(x)); });
                int n = 3, nMax = 18003;
                List<int> missing = new List<int>();
                while (true)
                {
                    if (!set.Contains(n)) missing.Add(n);
                    n += 4;
                    if (n > nMax) break;
                }
                int a1 = 0, a2 = 0, a3 = 0;
                if (missing.Count >= 3) { a1 = missing[0]; a2 = missing[1]; a3 = missing[2]; }
                else if (missing.Count == 2) { a1 = missing[0]; a2 = missing[1]; a3 = nMax + 4; }
                else if (missing.Count == 1) { a1 = -1; a2 = missing[0]; a3 = nMax + 4; }

                QueuedConsole.WriteImmediate("a3 - (a1+a2) : {0}", a3 - (a1 + a2));
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-and-lists-level-4-5-challenges/?problem=pythagorean-triplets
            /// </summary>
            void PythagoreanTriplets()
            {
                int[] a = new int[] { 196, 17, 32, 222, 35, 247, 95, 267, 197, 71, 42, 11, 44, 251, 18, 194, 172, 28, 108, 216, 125, 55, 103, 259, 66, 75, 163, 20, 122, 64, 147, 52, 127, 298, 58, 153, 33, 110, 76, 140, 113, 157, 294, 165, 142, 187, 211, 149, 265, 81, 244, 293, 286, 156, 130, 132, 112, 284, 14, 143, 288, 105, 83, 292, 266, 217, 299, 181, 5, 106, 131, 203, 158, 243, 275, 295, 198, 176, 236, 252, 224, 92, 129, 272, 43, 195, 167, 10, 206, 164, 135, 212, 77, 65, 133, 84, 174, 280, 104, 23, 229, 145, 297, 264, 59, 289, 96, 31, 144, 263, 57, 249, 134, 154, 233, 34, 245, 239, 29, 258, 278, 53, 215, 117, 283, 124, 100, 86, 72, 182, 26, 114, 208, 204, 237, 88, 136, 148, 276, 235, 257, 209, 80, 169, 120, 62, 24, 46, 16, 41, 201, 287, 238, 3, 116, 188, 54, 70, 254, 78, 82, 2, 63, 37, 226, 281, 277, 68, 48, 138, 118, 205, 232, 137, 207, 115, 1, 139, 85, 218, 160, 183, 186, 128, 214, 97, 223, 67, 60, 260, 171, 13, 199, 109, 162, 253, 200, 73, 141, 179, 21, 38, 180, 91, 191, 175, 256, 279, 255, 9, 12, 241, 221, 177, 248, 89, 119, 49, 193, 189, 228, 27, 40, 93, 51, 220, 296, 240, 126, 271, 173, 219, 227, 61, 19, 111, 50, 213, 94, 161, 87, 98, 30, 231, 74, 150, 192, 152, 79, 22, 185, 47, 107, 159, 261, 8, 7, 178, 146, 39, 151, 234, 190, 285, 4, 25, 268, 242, 155, 262, 270, 56, 6, 36, 184, 290, 69, 101, 15, 170, 102, 123, 225, 45, 282, 250, 210, 168, 90, 273, 274, 291, 246, 0, 166, 99, 269, 121, 230, 202 };
                List<List<int>> triples = PrimitiveTriples.GetPrimitiveTriples(a.Max());
                HashSet<int> set = new HashSet<int>(a);
                int count = 0;
                foreach(List<int> triple in triples)
                {
                    if (triple.All(x => { return set.Contains(x); })) {
                        count++;
                        int n = 2;
                        while (triple[2] * n <= a.Max())
                        {
                            List<int> mulTriple = new List<int>();
                            triple.ForEach(x => { mulTriple.Add(n * x); });
                            if (mulTriple.All(x => { return set.Contains(x); })) count++;
                            n++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Triples count : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-and-lists-level-4-5-challenges/?problem=differing-pairs
            /// </summary>
            void DifferingPairs()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/L1JKzVE9");
                html = html.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numStr = html.Splitter(StringSplitter.SplitUsing.Comma);
                List<int> list = new List<int>();
                numStr.ForEach(x => { list.Add(int.Parse(x)); });
                list.Sort();
                HashSet<int> set = new HashSet<int>(list);
                int diff = 70, count = 0;
                list.ForEach(x => { if (set.Contains(x + diff)) count++; });
                QueuedConsole.WriteImmediate("count : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/practice/arrays-and-lists-level-4-5-challenges/?problem=inspired-from-youtube-comments
            /// </summary>
            void FindMissingNumber()
            {
                string html = Http.Request.GetHtmlResponse("https://d18l82el6cdm1i.cloudfront.net/uploads/documents/digits-7BjVz9h8is.txt");
                html = html.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numStr = html.Splitter(StringSplitter.SplitUsing.Comma);
                long sum = 0, sumOfN = 0, N = 100000;
                sumOfN = N * (N + 1) / 2;
                numStr.ForEach(x => { sum += int.Parse(x); });

                QueuedConsole.WriteImmediate("Missing number : {0}", sumOfN - sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-basic/?problem=computer-science-problem-24351
            /// </summary>
            void SubstringRepeats()
            {
                string mainStr = "EATAUDRYBERNITAEATVIVANBROOKSEATJENIEATMICHALELASANDRALATIAEATLIDIAEATDEANDRAEATSYBILEATMONROEEATLATRISHAALTAEATDERICKROSANNLEVILIBBYKIRSTENCHARLESEATELLYNEATJANEEEATSTASIAEATJULIETTARANDIEATNORBERTSAGEEATARACELIKATINAMERNAEATISAIASWINNIEEATARLETHAEATMILOCAMIEEATANNABELEATLEANORABERTHASYBLECHANAEATREAGANERICKVALENTINAEATDORETHEASEBASTIANKRISROBERTAEATIVONNEEAT";
                string s = "EAT";
                QueuedConsole.WriteImmediate("{0} occurences : {1} ", s, Strings.CountOccurences(s, mainStr));
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-basic/?problem=computer-science-problem-24675
            /// </summary>
            void CharacterSum()
            {
                string s = "96h11k4959q615948s50922o38h1453ij38w73413d5577lzrqw3780b389750vf100zd29z73j5wh73l6965n85vm77cw10awrjr29265289222238n10013uk10062f9449acbhfgcm35j78q80";
                int sum = 0;
                s.ForEach(x => { if (char.IsDigit(x)) sum += (int)char.GetNumericValue(x); });
                QueuedConsole.WriteImmediate("Sum : {0} ", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-intermediate/?problem=computer-science-problem-24558
            /// </summary>
            void LongestNonRepeatingCharacterSubString()
            {
                string s = "xhbeqirxwobpuhkojsijumtfhsvyhyznuvzooiqxkvllfrpfnweiucjilnwixlucopomethoczbujltfycvbvvhuzstnmjcqgqchrktvsinunbopmgbwyegwkysmcxsdlhsbtcczfvcfvrbsqsxliyxzgzwvwgvvvgxgqyrbfetiwyqyircnysvcpfywdihnkjhwjsww";
                int longestLen = 0;
                for (int x = 0; x < s.Length; x++)
                {
                    HashSet<char> set = new HashSet<char>();
                    for (int y = x; y < s.Length; y++)
                    {
                        if (!set.Contains(s[y])) set.Add(s[y]);
                        else break;
                    }
                    longestLen = Math.Max(longestLen, set.Count);
                    if (longestLen > (s.Length - x)) break;
                }
                QueuedConsole.WriteImmediate("Length of longest substring : {0} ", longestLen);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-intermediate/?problem=computer-science-problem-24808
            /// </summary>
            void CrackCaeserCypher()
            {
                string cText = "ugfyjslmdslagfk gf kgdnafy lzak hjgtdwe lzw sfkowj ak log zmfvjwv sfv lowflq log";
                string[] cWords = cText.Splitter(StringSplitter.SplitUsing.Space);
                for (int i = 1; i <= 25; i++)
                {
                    string s = string.Empty;
                    foreach (string code in cWords)
                    {
                        s += " " + GenericDefs.Functions.Crypto.Cypher.Caeser.Encipher(code, i);
                    }
                    QueuedConsole.WriteAndWaitOnce("DeCoded :: {0}, Rotation :: {1}", s, i);
                }
            }

            void CrackSubstitutionCypher()
            {
                string s1 = "the quick brown fox jumps over the lazy dog";
                string s2 = "fzq kpgwj vxbsa rbh lpnic bdqx fzq uomy ebt";
                Dictionary<char, char> revMapper = new Dictionary<char, char>();
                Enumerable.Range(0, s1.Length).ForEach(x => { revMapper.AddOnce(s2[x], s1[x]); });

                string eText = "qafqx rbxfy fsb oc fzq oacsqx";
                string dText = "";
                eText.ForEach(c => { dText += revMapper[c]; });
                QueuedConsole.WriteImmediate("Deciphered text : {0}", dText);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-3-challenges/?problem=2015-countdown-problem-10-a-very-very-long-number
            /// </summary>
            void Find15Repeats()
            {
                string number = string.Empty;
                int n = 0;
                while (++n < 2016)
                {
                    number += "" + n;
                }
                string fifteen = "15";
                QueuedConsole.WriteImmediate("{0} occurences : {1} ", fifteen, Strings.CountOccurences(fifteen, number));
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-3-challenges/?problem=polynomial-cipher
            /// </summary>
            void PolynomialCipher()
            {
                long E = 13417108788227;
                Dictionary<long, int> factors = Factors.GetPrimeFactorsWithMultiplicity(E - 1, KnownPrimes.CloneKnownPrimes());

                Func<int, BigInteger> PolyCipher = delegate (int x)
                {
                    BigInteger xsq = x * x;
                    BigInteger ret = (2 * xsq * xsq) + (3 * xsq * x) + (xsq) + 4 * x + 1;
                    return ret;
                };
                
                factors.ForEach(x => {
                    QueuedConsole.WriteImmediate("Factor : {0}, Repeats : {1}, E({0}) : {2}", x.Key, x.Value, PolyCipher((int)x.Key));
                });
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-3-challenges/?problem=to-infinity-and-beyond-5
            /// </summary>
            void PalindromicReciprocalSum()
            {
                long n = 0;
                long nMax = (long)Math.Pow(10, 9);
                double sum = 0.0;
                while (++n < nMax)
                {
                    string s = "" + n;
                    string sRev = string.Join("", s.Reverse());
                    if (s.Equals(sRev)) sum += 1.0 / n;
                }
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-3-challenges/?problem=eating-the-number-sounds-interesting
            /// </summary>
            void EatingTheNumber()
            {
                long n = 0;
                long nMax = (long)Math.Pow(10, 9);
                while (++n < nMax)
                {
                    string s = "" + n;
                    string sRev = string.Join("", s.Reverse());
                    if (s.Equals(sRev)) continue;

                    sRev = s;
                    List<long> lDish = new List<long>();
                    lDish.Add(n);

                    while (s.Length > 1)
                    {
                        s = s.Substring(1, s.Length - 1);
                        lDish.Add(long.Parse(s));
                    }

                    List<long> rDish = new List<long>();
                    rDish.Add(n);
                    
                    while (sRev.Length > 1)
                    {
                        sRev = sRev.Substring(0, sRev.Length - 1);
                        rDish.Add(long.Parse(sRev));
                    }

                    if (lDish.Sum() == rDish.Sum()) break;

                }
                QueuedConsole.WriteImmediate("Smallest non-palindromic number with same taste: {0}", n);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-3-challenges/?problem=50100
            /// </summary>
            void ZeroInExponentialString()
            {
                BigInteger b = 1;
                int n = 0;
                while (++n<=100)
                {
                    b *= 5;
                }

                QueuedConsole.WriteImmediate("Number of zeroes : {0}", Strings.CountOccurences("0", b.ToString()) + 100);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-4-5-challenges/?problem=reversed-multiples
            /// </summary>
            void ReversedMultiples()
            {
                long B10000 = 0, N = 10000, bCount = 0;
                for (int i = 1; ; i++)
                {
                    long bn = i + long.Parse(Strings.Reverse(i + ""));
                    if (bn % 13 == 0)
                    {
                        bCount++;
                    }
                    if (bCount == N)
                    {
                        B10000 = i;
                        break;
                    }
                }

                QueuedConsole.WriteImmediate("{0}th brilliant number is :: {1}", N, B10000);
                QueuedConsole.WriteImmediate("Last three digits is :: {0}", B10000 % 1000);
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-4-5-challenges/?problem=palprimes
            /// </summary>
            void PalPrimes()
            {
                ((ISolve)(new _4.PalPrimes())).Solve();
            }

            /// <summary>
            /// https://brilliant.org/practice/problem-solving-dynamic-programming/?problem=edit-distances
            /// </summary>
            void EditDistances()
            {
                string seq1 = "CTCCCAGGGTG";
                string seq2 = "ACGTATAGCTTG";
                QueuedConsole.WriteImmediate("answer : {0}", GenericDefs.Functions.Algorithms.DP.Strings.LevenshteinDistance(seq1, seq2));
            }

            /// <summary>
            /// https://brilliant.org/practice/dynamic-programming-level-3-challenges/?problem=make-a-palindrome
            /// </summary>
            void MinimumInserts()
            {
                string input = "Brilliantforever";
                QueuedConsole.WriteImmediate("Minimum inserts : {0}", 
                    GenericDefs.Functions.Algorithms.DP.Palindrome.ConvertToPalindromeMinInsertions(input.ToCharArray(), input.Length));
            }

            /// <summary>
            /// https://brilliant.org/practice/linear-data-structures-level-3-challenges/?p=3
            /// </summary>
            void LargestDistinctSum()
            {
                string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/XPh7cYjN");
                html = html.Replace("[", "").Replace("]", "").Replace(" ", "");
                string[] numStr = html.Splitter(StringSplitter.SplitUsing.Comma);
                HashSet<int> set = new HashSet<int>();
                numStr.ForEach(x => { set.Add(int.Parse(x)); });

                int a1 = set.Max();
                set.Remove(a1);
                int a2 = set.Max();
                set.Remove(a2);

                QueuedConsole.WriteImmediate("sum : {0}", (a1 + a2));
            }

            /// <summary>
            /// https://brilliant.org/practice/strings-level-1-2-challenges/?problem=string-sting&subtopic=types-and-data-structures&chapter=strings
            /// https://brilliant.org/problems/string-sting/
            /// </summary>
            void LongestSubStringInAlphabeticalOrder()
            {
                string s = "woimoepzbjvxfafpyfpzgmxugjodtemcjcpoxobfgbsmokkmcdpmawcwwaxhqwabzdlplvteszqgtkamxjkswkpnzpxpudxcmigz";
                int longest = int.MinValue;
                for (int i = 0; i < s.Length; i++)
                {
                    int len = 1;
                    char prev = s[i];
                    for (int j = i + 1; j < s.Length; j++)
                    {
                        char current = s[j];
                        if (current.CompareTo(prev) >= 0) {
                            len++;
                            prev = current;
                        } else {
                            longest = Math.Max(longest, len);
                            break;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Longest sub-string : {0}", longest);
            }

            /// <summary>
            /// https://brilliant.org/practice/loops-level-2-challenges/?problem=prime-fibonacci-sounds-crazy-perhaps-craziest&subtopic=programming-languages&chapter=loops
            /// https://brilliant.org/problems/prime-fibonacci-sounds-crazy-perhaps-craziest/
            /// </summary>
            void PrimeFibonacci()
            {
                long max = (long)Math.Pow(10, 9);
                BigInteger sum = 0;
                FibonacciGenerator fg = new FibonacciGenerator(0, 1);
                ClonedPrimes primes = KnownPrimes.CloneKnownPrimes(0, 100000);

                while (fg.Next() <= max)
                {
                    if (Prime.IsPrime(fg.CurrentTerm(), primes))
                    {
                        sum += fg.CurrentTerm();
                    }
                }

                QueuedConsole.WriteImmediate("Sum of terms : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/practice/loops-level-2-challenges/?problem=the-first-easy-number-game&subtopic=programming-languages&chapter=loops
            /// https://brilliant.org/problems/the-first-easy-number-game/
            /// </summary>
            void TheFirstEasyNumberGame()
            {
                int count = 0;
                for (int a = 1; a <= 9; a++)
                {
                    for (int b = 0; b <= 9; b++)
                    {
                        for (int c = 0; c <= 9; c++)
                        {
                            int lhs = (100 * a) + (10 * b) + c;
                            int rhs = (a * a * a) + (b * b * b) + (c * c * c);

                            if (lhs.Equals(rhs)) count++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of 3-digit numbers : {0}", count);
            }
        }

        //Keep this lowest in view.
        string IProblemName.GetName()
        {
            return "Brilliant ComputerScience Collection";
        }
    }
}