using GenericDefs.Classes;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using GenericDefs.Functions.Geometry;
using Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Brilliant.Combinatorics
{
    public class CombinatoricsCollection : ISolve, IBrilliant, IProblemName
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
            //(new Partitions()).Solve();
            //ExpectedValue.TheTravellerAndThreeKegs();
        }

        internal class Collection1
        {
            internal void Solve()
            {
                ProperSequences();
            }

            /// <summary>
            /// https://brilliant.org/problems/dice-numbers-creating-triangles/
            /// Potsawee has a fair six-sided die. He throws the die 3 times, and the numbers shown on the upper side each time are (a,b,c) respectively. 
            /// Find the probability that the lengths (a,b,c) are able to create an isosceles triangle. Give your answer to three significant figures.
            /// </summary>
            void DiceNumbersCreatingTriangles()
            {
                int numerator = 0;
                int denominator = 0;
                for (int a = 1; a <= 6; a++)
                {
                    for (int b = 1; b <= 6; b++)
                    {
                        for (int c = 1; c <= 6; c++)
                        {
                            denominator++;
                            if (a == b || b == c || a == c)
                            {
                                if (PropertiesOfTriangle.IsValidTriangle(a, b, c))
                                {
                                    numerator++;
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Numerator : {0}, Denominator : {1}, Answer : {2}", numerator, denominator, ((numerator * 1.0) / denominator).ToString("0:00")));
            }

            /// <summary>
            /// https://brilliant.org/problems/push-your-limits-actually-not-that-hard/
            /// How many ways to share 15 different candies to 3 people and each must have at least one candy?
            /// This problem can be generalize to N people and M different candies
            /// </summary>
            void DistributingCandies()
            {
                UniqueIntegralPairs u = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(15, 3, 1);
                List<UniqueIntegralPairs.Combination> pairs = u.GetCombinations();
                BigInteger b = new BigInteger(0);
                foreach (UniqueIntegralPairs.Combination c in pairs)
                {
                    b += PermutationsAndCombinations.nCr(15, c.Pair[0]) * PermutationsAndCombinations.nCr(15 - c.Pair[0], c.Pair[1]);
                }

                QueuedConsole.WriteFinalAnswer("Number of ways of distributing : " + b.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/jee-advanced-2016-7/
            /// </summary>
            void ComputeRemainder()
            {
                long finalRem = 0;
                int n = 0;
                while (true)
                {
                    n++;
                    if (n > 2016) break;

                    BigInteger remfi = 1;
                    int div = 1;
                    int i = 0;
                    while (true)
                    {
                        i++;
                        if (i > n)
                        {
                            break;
                        }
                        remfi *= (n + i);

                        if (remfi % 2 == 0) remfi /= 2;
                        else div *= 2;
                    }
                    remfi /= div;
                    remfi %= 100;
                    finalRem += (int)remfi;
                }

                finalRem %= 100;
                QueuedConsole.WriteFinalAnswer(string.Format("Remainder is : {0}", finalRem));
            }

            /// <summary>
            /// https://brilliant.org/problems/intermediate-not-talking-about-level-part-i/
            /// </summary>
            void WordArrangements1()
            {
                string word = "INTERMEDIATE";
                BlackList<char> bl = new BlackList<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    if (i == 0) bl.Add(i, new char[] { 'I' });
                    if (i == word.Length - 1) bl.Add(i, new char[] { 'E' });
                }

                WordArrangements wa = new WordArrangements();
                BigInteger n = wa.CountWordArrangements(word, bl);
                QueuedConsole.WriteFinalAnswer(string.Format("Word arrangements : {0}", n.ToString()));
            }

            /// <summary>
            /// https://brilliant.org/problems/no-change/
            /// Problem converted to arrangements of 'ababababab'. Positive/Total.
            /// </summary>
            void WordArrangements2()
            {
                string word = "ab";
                Func<string, bool> rule = delegate (string s)
                {
                    char[] sch = s.ToCharArray();
                    int t = 0;
                    foreach (char ch in sch)
                    {
                        if (ch == 'a') { t++; }
                        else if (ch == 'b') { t--; }
                        if (t < 0) break;
                    }

                    return t == 0;
                };
                BlackList<char> bl = new BlackList<char>();
                WordArrangements wa = new WordArrangements(rule, 10);

                List<char> unused = new List<char>() { 'a', 'b', 'a', 'b', 'a', 'b', 'a', 'b', 'a', 'b' };

                BigInteger n = wa.GetAllWordArrangements(word, bl, unused);

                Fraction<long> frac = new Fraction<long>((long)wa.GetPositiveArrangements(), (long)n, false);
                QueuedConsole.WriteFinalAnswer(string.Format("a: {0}, b: {1}", frac.N, frac.D));
            }

            /// <summary>
            /// a x b x c rectangular prism. with 1x1x1 cubes. External cubes = cubes having atleast one face on the outside. 
            /// Internal = not exposed. Find external = internal.
            /// </summary>
            void RectangularPrismExternalCubesInternalCubes()
            {
                //solve for abc = 2*(a-2)*(b-2)*(c-2). 
                UniquePairedIntegralSolutions<int> u = new UniquePairedIntegralSolutions<int>("%");
                int nMax = 1000, aMax = 1000;
                ConsecutiveStreakCounter csc = new ConsecutiveStreakCounter();
                bool stepOut = false;
                for (int a = aMax; a >= 2; a--)
                {
                    for (int b = 2; b <= nMax; b++)
                    {
                        for (int c = b; c <= nMax; c++)
                        {
                            long lhs = a * b * c;
                            long rhs = 2 * (a - 2) * (b - 2) * (c - 2);
                            int diff = lhs - rhs > 0 ? 1 : (lhs - rhs < 0 ? -1 : 0);
                            if (diff == 0)
                            {
                                u.AddIfUnique(new int[] { a, b, c });
                                csc.Add(0);
                            }
                            else if (diff > 0)
                            {
                                csc.Add(1);
                            }
                            else
                            {
                                csc.Add(-1);
                            }
                            int value = csc.IsNegative ? -1 : (csc.IsPositive ? 1 : 0);
                            //QueuedConsole.WriteImmediate(string.Format(" .... {0} ... {1}", csc.CurrentStreak, value));
                            if (csc.CurrentStreak >= 100000000000)
                            {
                                //stepOut = true;
                            }

                            if (stepOut) break;
                        }
                        if (stepOut) break;
                    }
                    if (stepOut) break;
                }

                List<int[]> cList = u.GetUniqueSolutions();
                QueuedConsole.WriteImmediate(string.Format("Possible unique prisms : {0}", cList.Count));
                foreach (int[] uc in cList)
                {
                    QueuedConsole.WriteImmediate(string.Format("a x b x c : {0} x {1} x {2}", uc[0], uc[1], uc[2]));
                }
                QueuedConsole.WriteFinalAnswer("Check the combinations above.");
            }

            /// <summary>
            /// https://brilliant.org/problems/no-three-tails/
            /// Problem converted to arrangements of 'hthththththt' with no three consecutive tails. Positive/Total.
            /// </summary>
            void NoThreeTails()
            {
                string word = "ht";
                Func<string, bool> rule = delegate (string s)
                {
                    return !(s.IndexOf("ttt") >= 0);
                };
                BlackList<char> bl = new BlackList<char>();
                WordArrangements wa = new WordArrangements(rule, 12);
                BigInteger n = wa.GetAllCoinTossArrangements(word, bl);

                QueuedConsole.WriteFinalAnswer(string.Format("a: {0}, b: {1}", wa.GetPositiveArrangements(), n.ToString()));
            }

            void Quadruples()
            {
                ///////////////////////////////
                ///////////Doesnt work because, currently simplecounter cannot check for repeats over very large permutations.
                ///////////Better way is to find distinct pairs and then find all permutations of those.
                ///////////////////////////////
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    return cr.CoefficientSum() == 2016;
                };

                int n = 4;
                Func<List<int>, int, bool> SearchAccelerator = delegate (List<int> list, int index)
                {
                    if (index < n - 1) return list.Sum() <= 2016;
                    else return true;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < n; i++)
                {
                    int min = 0, max = 1007;

                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                UniqueArrangements<int> s = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(2016, 4, 0, 1007);
                int fc = s.GetFlatCount();
                long total = 0;
                for (int i = 0; i < fc; i++)
                {
                    List<string> objects = s.GetFlatByIndexAsString(i);
                    foreach (string set in objects)
                    {
                        List<int> t = s.ExtractSet(set);
                        int td = t.Distinct().Count();
                        if (td == 4) { total += 24; }
                        if (td == 3) { total += 12; }
                        if (td == 2)
                        {
                            if (t.GroupBy(x => x).Where(g => g.Count() == 2).Count() == 2)
                            {
                                total += 6;
                            }
                            else { total += 4; }
                        }
                        if (td == 1) { total += 1; }
                    }
                }
                QueuedConsole.WriteFinalAnswer("Total possible numbers : " + total);
            }

            void QuadruplesIterationVersion()
            {
                /////////////////////////////////////////////
                ////////////////Answer - 682791315
                /////////////////////////////////////////////
                IterationStat stat = new IterationStat(30);
                stat.Current = 0;
                stat.Total = 1009 * 1008 / 2;
                stat.Percentage = 0.0;

                int tCount = 0;
                long totalWays = 0;
                stat.Start();
                HashSet<string> solset = new HashSet<string>();
                for (int a = 0; a <= 1007; a++)
                {
                    for (int b = a; b <= 1007; b++)
                    {
                        stat.Update(++tCount);
                        for (int c = b; c <= 1007; c++)
                        {
                            int sum = a + b + c;
                            if (sum > 2016) break;
                            for (int d = c; d <= 1007; d++)
                            {
                                int sum1 = a + b + c + d;
                                if (sum1 > 2016) break;

                                if (sum1 == 2016)
                                {
                                    List<int> set = new List<int>() { a, b, c, d };
                                    if (solset.Add(string.Join("-", set)))
                                    {
                                        int td = set.Distinct().Count();
                                        if (td == 4) { totalWays += 24; }
                                        else if (td == 3) { totalWays += 12; }
                                        else if (td == 2)
                                        {
                                            if (set.GroupBy(x => x).Where(g => g.Count() == 2).Count() == 2)
                                            {
                                                totalWays += 6;
                                            }
                                            else { totalWays += 4; }
                                        }
                                        else if (td == 1)
                                        {
                                            totalWays += 1;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                stat.Stop();
                QueuedConsole.WriteFinalAnswer("Total possible numbers : " + totalWays);
            }

            /// <summary>
            /// https://brilliant.org/problems/daniels-quadruple-differences/
            /// </summary>
            void DanielsQuadrupleDifferences()
            {
                Func<CustomIterenumerator<int>, int?> customEnum = delegate (CustomIterenumerator<int> ci)
                {
                    int depth = ci.History == null ? 0 : ((List<int>)ci.History).Count;
                    int prev = ci.History == null ? 0 : ((List<int>)ci.History)[depth - 1];
                    if (ci.History == null)
                    {
                        if (!ci.Current().HasValue) return 1;
                        else
                        {
                            int cV = ci.Current().Value + 1;
                            if (depth == 0)
                            {
                                if (cV > 7) return null;
                            }
                            return cV;
                        }
                    }
                    else
                    {
                        if (!ci.Current().HasValue) return prev + 1;
                        else
                        {
                            int cV = ci.Current().Value + 1;
                            if (depth == 1)
                            {
                                if (cV > 8) return null;
                            }
                            else if (depth == 2)
                            {
                                if (cV > 9) return null;
                            }
                            else if (depth == 3)
                            {
                                if (cV > 10) return null;
                            }
                            return cV;
                        }
                    }
                };

                Func<int, int, int> diff = delegate (int a, int b)
                {
                    return Math.Abs(a - b);
                };

                Func<CustomIterenumerator<int>, int, List<int>, bool> recursion = null;
                Dictionary<int, UniqueArrangements<int>> ua = new Dictionary<int, UniqueArrangements<int>>();
                recursion = delegate (CustomIterenumerator<int> ite, int depth, List<int> before)
                {
                    CustomIterenumerator<int> lc = ite.Clone();
                    lc.History = before.Select(x => x).ToList();
                    for (int? i = lc.MoveNext(); lc.IsValid(); i = lc.MoveNext())
                    {
                        lc.History = before.Select(x => x).ToList();
                        if (!i.HasValue) break;
                        List<int> after = before.Select(x => x).ToList();
                        after.Add(i.Value);
                        if (depth == 3)
                        {
                            int k = EnumerableExtensions.PairwiseForEach(after, diff).ToList().Sum();
                            if (ua.ContainsKey(k))
                            {
                                after.Sort();
                                ua[k].Add(after);
                                continue;
                            }
                            else
                            {
                                UniqueArrangements<int> u = new UniqueArrangements<int>();
                                u.Add(after);
                                ua.Add(k, u);
                            }
                        }
                        else
                        {
                            recursion(lc, depth + 1, after);
                        }
                    }
                    return true;
                };

                CustomIterenumerator<int> iterator = new CustomIterenumerator<int>(customEnum, null);
                iterator.History = null;
                for (int? i = iterator.MoveNext(); iterator.IsValid(); i = iterator.MoveNext())
                {
                    if (!i.HasValue) break;
                    List<int> list = new List<int>();
                    list.Add(i.Value);
                    recursion(iterator, 1, list);
                }

                int sum = 0;
                long N = 0;
                foreach (KeyValuePair<int, UniqueArrangements<int>> kvp in ua)
                {
                    if (kvp.Value.GetCount() > N)
                    {
                        N = kvp.Value.GetCount();
                        sum = kvp.Key;
                    }
                    else if (kvp.Value.GetCount() == N)
                    {
                        sum += kvp.Key;
                    }
                }

                QueuedConsole.WriteFinalAnswer("Sum : " + sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/buttons/
            /// </summary>
            void PressButtons()
            {
                int count = 0;
                int n = 1;
                while (true)
                {
                    count += 1 + n * (25 - n);
                    n++;
                    if (n == 26) break;
                }
                QueuedConsole.WriteFinalAnswer("count : " + count);
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// dog = 0
            /// </summary>
            void SolveDogInt()
            {
                //d-o-g
                //0-1-2
                UniqueArrangements<int> ua = new UniqueArrangements<int>();

                for (int d = -7; d <= 14; d++)
                {
                    for (int o = -7; o <= 14; o++)
                    {
                        for (int g = -7; g <= 14; g++)
                        {
                            if (d + o + g == 0)
                            {
                                List<int> dog = new List<int>() { d, o, g };
                                ua.Add(dog);
                            }
                        }
                    }
                }

                QueuedConsole.WriteFinalAnswer(string.Format("Possible solutions : {0}", ua.GetCount()));
            }

            /// <summary>
            /// https://brilliant.org/problems/number-tumble/
            /// Positive integral solutions for, a + b + c + d = 20.
            /// </summary>
            void NumberTumble()
            {
                SimpleCounter sc = new SimpleCounter();
                for (int a = 1; a <= 20; a++)
                {
                    int val = a;
                    for (int b = 1; b <= 20; b++)
                    {
                        if (a == b) continue;
                        val = a + b;
                        if (val > 20) break;
                        for (int c = 1; c <= 20; c++)
                        {
                            if (c == b || c == a) continue;
                            val = a + b + c;
                            if (val > 20) break;
                            for (int d = 1; d <= 20; d++)
                            {
                                if (c == d || d == a || d == b) continue;
                                val = a + b + c + d;
                                if (val > 20) break;

                                if (val == 20) sc.Increment();
                            }
                        }
                    }
                }

                QueuedConsole.WriteFinalAnswer(string.Format("Possible solutions : {0}", sc.GetCount()));
            }

            /// <summary>
            /// 8-sided dice rolled 3 times. Given a + b = c, then probability of a or b or c is 2, is given by d/e reduced.
            /// </summary>
            void DiceRolled()
            {
                int d = 0, e = 0;
                for (int a = 1; a <= 8; a++)
                {
                    for (int b = 1; b <= 8; b++)
                    {
                        for (int c = 1; c <= 8; c++)
                        {
                            if (a + b == c)
                            {
                                e++;
                                if (a == 2 || b == 2 || c == 2) d++;
                            }
                        }
                    }
                }

                Fraction<int> fr = new Fraction<int>(d, e, false);
                QueuedConsole.WriteFinalAnswer("a/b : {0},{1}", fr.N, fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/day-24-deranged-distribution/
            /// </summary>
            void DerangedDistribution()
            {
                string word = "0123456";
                Func<string, bool> rule = delegate (string s)
                {
                    char[] sch = s.ToCharArray();
                    int count = 0;
                    int index = 0;
                    foreach (char ch in sch)
                    {
                        if (char.GetNumericValue(ch) == index) count++;
                        index++;
                    }
                    return count >= 2;
                };
                BlackList<char> bl = new BlackList<char>();
                WordArrangements wa = new WordArrangements(rule, 7);
                List<char> unused = new List<char>() { '0', '1', '2', '3', '4', '5', '6' };
                BigInteger n = wa.GetAllWordArrangements(word, bl, unused);
                QueuedConsole.WriteFinalAnswer(string.Format("Number of ways to distribute the presents: {0}", (long)wa.GetPositiveArrangements()));
            }

            /// <summary>
            /// https://brilliant.org/problems/divisibility-by-3/
            /// Number of good n l.t. 10^6;
            /// </summary>
            void DivisibilityBy3AndOddDigits()
            {
                int n = 1;
                int nGoodNumbers = 0;
                while (true)
                {
                    string nStr = n + "";
                    bool add = true;
                    foreach (char ch in nStr.ToCharArray())
                    {
                        if (char.GetNumericValue(ch) % 2 == 0)
                        {
                            add = false;
                            break;
                        }
                    }
                    if (add && n % 3 == 0) nGoodNumbers++;
                    n += 2;
                    if (n >= 1000000) break;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Number of good numbers: {0}", nGoodNumbers));
            }

            /// <summary>
            /// https://brilliant.org/problems/probability-of-a-match/
            /// </summary>
            void ProbabilityOfMatch()
            {
                string word = "01";
                Func<string, bool> rule = delegate (string s)
                {
                    if (s.IndexOf("000") >= 0) return true;
                    else return false;
                };
                BlackList<char> bl = new BlackList<char>();
                WordArrangements wa = new WordArrangements(rule, 7);
                List<char> unused = new List<char>() { '0', '1', '0', '1', '0', '1', '0', '1', '0', '1', '0', '1', '0', '1' };
                BigInteger n = wa.GetAllWordArrangements(word, bl, unused);
                QueuedConsole.WriteFinalAnswer(string.Format("p : {0}, q: {1}", (long)wa.GetPositiveArrangements(), n.ToString()));
            }

            /// <summary>
            /// https://brilliant.org/problems/exchange-of-coats/
            /// </summary>
            void ExchangeOfCoats()
            {
                string word = "012345";
                Func<string, bool> rule = delegate (string s)
                {
                    int index = 0;
                    int count = 0;
                    foreach (char ch in s)
                    {
                        if (char.GetNumericValue(ch) == index) count++;
                        index++;
                    }
                    return count == 2;
                };
                BlackList<char> bl = new BlackList<char>();
                WordArrangements wa = new WordArrangements(rule, 6);
                List<char> unused = new List<char>() { '0', '1', '2', '3', '4', '5' };
                BigInteger n = wa.GetAllWordArrangements(word, bl, unused);
                QueuedConsole.WriteFinalAnswer(string.Format("number of ways : {0}", wa.GetPositiveArrangements().ToString()));
            }

            /// <summary>
            /// https://brilliant.org/problems/semi-magic-squares/
            /// The squares of a 3 x 3 grid are filled with non-negative integers such that the sum of each row and the sum of each column is  How many different ways can the squares be filled?
            /// The numbers in each grid square does not need to be distinct. Rotations and reflections are distinct arrangements.
            /// </summary>
            void SemiMagicSquares()
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
                    foreach(int[] row in rows)
                    {
                        if (cr.CoefficientSum(row) != 7) return false;
                    }
                    foreach (int[] col in cols)
                    {
                        if (cr.CoefficientSum(col) != 7) return false;
                    }
                    return true;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < 9; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 7, Min = 0 });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(9, iterators);

                QueuedConsole.WriteFinalAnswer("The squares can be filled in : {0} ways.", ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/billion-multiplication/
            /// How many triples of positive integers (a, b, c) satisfy a leq b leq c and
            /// abc = 1, 000, 000, 000?
            /// </summary>
            void BillionMultiplication()
            {
                int[] x = new int[3] { 0, 1, 2 };
                int[] y = new int[3] { 3, 4, 5 };
                Dictionary<int, long> powersOf2 = new Dictionary<int, long>();
                Dictionary<int, long> powersOf5 = new Dictionary<int, long>();
                long p2 = 1, p5 = 1;
                for (int i = 0; i < 10; i++)
                {
                    powersOf2.Add(i, p2);
                    powersOf5.Add(i, p5);
                    if (i < 9) {
                        p2 *= 2;
                        p5 *= 5;
                    }
                }
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    if (cr.CoefficientSum(x) != 9) return false;
                    if (cr.CoefficientSum(y) != 9) return false;
                    for (int i = 0; i < 2; i++)
                    {
                        if (powersOf2[coeff[x[i]]] * powersOf5[coeff[y[i]]] > powersOf2[coeff[x[i + 1]]] * powersOf5[coeff[y[i + 1]]]) return false;
                    }
                    return true;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < 6; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 0 });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(6, iterators);

                QueuedConsole.WriteFinalAnswer("The squares can be filled in : {0} ways.", ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/its-v-versus-x-and-y/
            /// </summary>
            void VversusXandY()
            {
                int n = 1;
                while (true)
                {
                    List<int> numbers = new List<int>();
                    for(int i = 1; i <= 3 * n; i++)
                    {
                        numbers.Add(i);
                    }
                    List<IList<int>> combinations = Combinations.GetAllCombinations(numbers, 2);
                    int numerator = 0, denominator = 0;
                    foreach(IList<int> c in combinations)
                    {
                        long xy = (c[0] * c[0] * c[0]) + (c[1] * c[1] * c[1]);
                        denominator++;
                        if (xy % 3 == 0) numerator++;
                    }
                    QueuedConsole.WriteImmediate("n = {0}, probability : {1}/{2}", n, numerator, denominator);
                    n++;
                    if (n == 10) break;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/intervals-2/
            /// </summary>
            void Intervals2()
            {
                int iCount = 0;
                int length = 1;
                int nStart = 1, nMax = 10000;
                int tile = 1729;
                while (true)
                {
                    int i = nStart;
                    int iStart = 0, iEnd = 0;
                    while (true)
                    {
                        iStart = i;
                        iEnd = i + length;
                        if (iStart <= tile && iEnd >= tile) iCount++;
                        i++;
                        if (i + length > nMax) break;
                    }
                    length++;
                    if (length > nMax) break;
                }

                QueuedConsole.WriteImmediate("Intervals that contain the tile with the cross : {0}", iCount);
            }

            /// <summary>
            /// https://brilliant.org/problems/question-for-abhay-tiwari-2/
            /// </summary>
            void DividingDiamonds()
            {
                int vertices = 2, n = 1;
                while (true)
                {
                    n++;
                    vertices = (2 * vertices) - 1;
                    if (n == 12) break;
                }
                QueuedConsole.WriteImmediate("Number of triangles in the {0}th term : {1}", n, (4 * PermutationsAndCombinations.nCr(vertices, 2)) + 4);
            }

            /// <summary>
            /// https://brilliant.org/problems/drawing-balls-without-replacement/
            /// </summary>
            void DrawingBallsWithoutReplacement()
            {
                string word = "aaaaaaa";
                int i = 0;
                List<int> unused = new List<int>();
                word.ForEach(x => { unused.Add(++i); });

                List<IList<int>> combinations = Combinations.GetAllCombinations(unused, 2);
                int cT = 0;
                long sum = 0;
                foreach (IList<int> c in combinations)
                {
                    cT++;
                    sum += c[0] * c[1];
                }

                Fraction<long> frac = new Fraction<long>(sum, cT, false);
                QueuedConsole.WriteFinalAnswer(string.Format("Expected value of xy : (a/b): {0}/{1}, a + b : {2}", frac.N, frac.D, frac.N + frac.D));
            }

            /// <summary>
            /// https://brilliant.org/problems/counting-principlespart-6-2/
            /// </summary>
            void FiveDigitNumbersSum()
            {
                //////////////////////////////////////////////////
                ////////////// Do not run this problem.
                //////////////////////////////////////////////////
                string word = "23579";
                List<int> unused = new List<int>();
                word.ForEach(x => { int cVal = (int)char.GetNumericValue(x); unused.Add(cVal); unused.Add(cVal); unused.Add(cVal); unused.Add(cVal); });

                IEnumerable<IEnumerable<int>> permutations = Permutations.GetPermutationsWithoutRepetition(unused, 5);
                long sum = 0;
                UniqueArrangements<int> ua = new UniqueArrangements<int>();
                foreach (IEnumerable<int> c in permutations)
                {
                    ua.Add(c.ToList());
                }

                ua.Add(new List<int>() { 2, 2, 2, 2, 2 });
                ua.Add(new List<int>() { 3, 3, 3, 3, 3 });
                ua.Add(new List<int>() { 5, 5, 5, 5, 5 });
                ua.Add(new List<int>() { 7, 7, 7, 7, 7 });
                ua.Add(new List<int>() { 9, 9, 9, 9, 9 });

                List<List<int>> sets = ua.ExtractAsSets();
                SortedSet<int> sortedSet = new SortedSet<int>();
                foreach(List<int> set in sets)
                {
                    string s = set[0] + "" + set[1] + "" + set[2] + "" + set[3] + "" + set[4];
                    sortedSet.Add(int.Parse(s));
                    sum += int.Parse(s);
                }

                sortedSet.ForEach(x => QueuedConsole.WriteImmediate("{0}", x));
                QueuedConsole.WriteFinalAnswer(string.Format("Sum of all five digit numbers: {0}", sum));
            }

            /// <summary>
            /// https://brilliant.org/problems/random-answer-correct/
            /// </summary>
            void RandomAnswerCorrect()
            {
                BigInteger d = 1, n = 21;
                int n1 = 63, n2 = 13 * 13 * 26;
                for(int i= 0; i<10; i++)
                {
                    d *= n2;
                    n *= n1;
                }
                d += n;
                QueuedConsole.WriteImmediate("Last three digits : {0}", (d % 1000).ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/easy-probability-4/
            /// </summary>
            void EasyProbability4()
            {
                Fraction<int> fr = new Fraction<int>(5000, 3333 * 10001, false);
                QueuedConsole.WriteImmediate("Probability k/l : {0}/{1}, k-l : {2}", fr.N, fr.D, fr.D - fr.N);
            }

            /// <summary>
            /// https://brilliant.org/problems/bomb-the-target-part-3/
            /// </summary>
            void BombTheTarget_Part3()
            {
                int n = 17, d = 24;
                int rem = 1;
                while (true)
                {
                    rem *= n;
                    rem %= 1000;
                    d--;
                    if (d == 0) break;
                }
                QueuedConsole.WriteImmediate("Last three digits of f^g : {0}", rem);
            }

            /// <summary>
            /// https://brilliant.org/problems/be-careful-with-your-licence-plates/
            /// </summary>
            void LicensePlates()
            {
                Dictionary<int, int> digits = new Dictionary<int, int>();
                digits.Add(0, 10);
                Enumerable.Range(1, 9).ForEach(x => digits.Add(x, x));

                Dictionary<char, int> characters = new Dictionary<char, int>();
                char ch = 'A';
                int t = 1;
                Enumerable.Range(1, 26).ForEach(x => characters.Add(ch++, t++));

                List<IList<int>> digitCombinations = Combinations.GetAllCombinations(digits.Keys.ToList(), 3, true);
                List<IList<char>> charCombinations = Combinations.GetAllCombinations(characters.Keys.ToList(), 3, true);

                HashSet<string> uniqueChecker = new HashSet<string>();
                Dictionary<int, int> PossibleDigitSumCombinations = new Dictionary<int, int>();
                foreach(IList<int> dc in digitCombinations)
                {
                    int[] arrDC = dc.ToArray();
                    Array.Sort(arrDC);
                    if (uniqueChecker.Add(string.Join("", arrDC)))
                    {
                        int sum = 0;
                        arrDC.ForEach(x => sum += digits[x]);

                        int possDC = 0;
                        int distinctArrDC = arrDC.Distinct().Count();
                        if (distinctArrDC == 3) possDC = 6;
                        else if (distinctArrDC == 2) possDC = 3;
                        else if (distinctArrDC == 1) possDC = 1;

                        PossibleDigitSumCombinations.AddOrUpdate(sum, possDC);
                    }
                }
                uniqueChecker.Clear();

                int totalPlates = 0;
                foreach(IList<char> chc in charCombinations)
                {
                    char[] arrChC = chc.ToArray();
                    Array.Sort(arrChC);
                    if(uniqueChecker.Add(string.Join("", arrChC)))
                    {
                        int possCHC = 0;
                        int distinctArrDC = arrChC.Distinct().Count();
                        if (distinctArrDC == 3) possCHC = 6;
                        else if (distinctArrDC == 2) possCHC = 3;
                        else if (distinctArrDC == 1) possCHC = 1;

                        int sum = 0;
                        arrChC.ForEach(x => sum += characters[x]);

                        if (PossibleDigitSumCombinations.ContainsKey(100 - sum))
                        {
                            totalPlates += possCHC * PossibleDigitSumCombinations[100 - sum];
                        }
                    }
                }

                QueuedConsole.WriteImmediate("Digit Combinations with repetitions : {0}", digitCombinations.Count);
                QueuedConsole.WriteImmediate("Character Combinations with repetitions : {0}", charCombinations.Count);
                QueuedConsole.WriteImmediate("Different License Plates costing 100$ : {0}", totalPlates);
            }

            /// <summary>
            /// https://brilliant.org/problems/asymmetric-polya/
            /// </summary>
            void AsymmetricPolya()
            {
                BigInteger bNu = PermutationsAndCombinations.nCrBig(60, 15);
                BigInteger bDe = PermutationsAndCombinations.nPrBig(63, 16);
                bNu *= 3;
                bNu *= PermutationsAndCombinations.Factorial(15);
                BigIntFraction fr = new BigIntFraction(bNu, bDe, false);

                QueuedConsole.WriteImmediate("a : {0}, b : {1}", fr.N, fr.D);
                QueuedConsole.WriteImmediate("a + b : {0}", fr.N + fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-jee-advanced-problem/
            /// </summary>
            void AJeeAdvancedProblem()
            {
                int pArrangements = 0;
                int totalArrangements = 0;
                int n = 10;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    totalArrangements++;
                    int[] coeff = cr.GetCoefficients();
                    for (int i = 0; i < n-1; i++) {
                        if (Math.Abs(coeff[i] - coeff[i + 1]) != 1) return false;
                    }
                    pArrangements++;
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < 10; i++)
                {
                    int min = 1, max = 5;

                    Iterator<int> iter = new Iterator<int>(false, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                SpecialCryptRecursiveSolver solver = new SpecialCryptRecursiveSolver(false, cArithm);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                solver.GetAllSolutions(n, iterators, 0, false, null);
                sw.Stop();
                QueuedConsole.WriteImmediate("time taken : " + sw.ElapsedMilliseconds);
                QueuedConsole.WriteImmediate("Total positive arrangements : {0}, Total arrangements : {1}", pArrangements, totalArrangements);

                Fraction<int> fr = new Fraction<int>(pArrangements, totalArrangements, false);
                QueuedConsole.WriteImmediate("Probability : {0} = {1}", fr.N + "/" + fr.D, fr.GetValue());

            }

            /// <summary>
            /// https://brilliant.org/problems/quite-a-knight-math-version/
            /// </summary>
            [SearchKeyword("Incorrect")]
            void QuiteAKnight()
            {
                int LeastMoves = 14;
                int XDest = 22, YDest = 18;
                int nSequences = 0;
                Func<int, int, int, bool> Recursion = null;
                Recursion = delegate (int curX, int curY, int nMoves) {
                    if (nMoves > LeastMoves) return false;
                    if (curX < -3 || curY < -3) return false;
                    if (curX > XDest + 2) return false;
                    if (curY > YDest + 2) return false;
                    if (curX > XDest && curY > YDest) return false;
                    if (curX == XDest && curY == YDest && nMoves<= LeastMoves) { QueuedConsole.WriteImmediate("Sequence counter : {0}", ++nSequences);  return true; }
                    else {
                        Recursion(curX + 2, curY + 1, nMoves + 1);
                        Recursion(curX + 1, curY + 2, nMoves + 1);
                        Recursion(curX - 2, curY + 1, nMoves + 1);
                        Recursion(curX - 1, curY + 2, nMoves + 1);
                        Recursion(curX + 2, curY - 1, nMoves + 1);
                        Recursion(curX + 1, curY - 2, nMoves + 1);
                        if (curX > XDest - 2 && curY > YDest - 2) {
                            Recursion(curX - 2, curY - 1, nMoves + 1);
                            Recursion(curX - 1, curY - 2, nMoves + 1);
                        }
                    }
                    return false;
                };

                Recursion(0, 0, 0);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-number-theory-problem-by-lemuel-liverosk/
            /// </summary>
            void PythagoreanTriplesNotDivisibleBy12()
            {
                int cMax = 2016;
                List<List<int>> primitiveTriples = GenericDefs.Functions.PrimitiveTriples.GetPrimitiveTriples();
                int nNotDivisible = 0;
                int tPossibilities = 0;
                foreach (List<int> pT in primitiveTriples)
                {
                    if (pT[2] < cMax)
                    {
                        int n = 0, t = 0;
                        while (true)
                        {
                            t++;
                            if (t * pT[2] > cMax) { t--; break; }
                            if ((t * t * pT[0] * pT[1]) % 12 != 0) n++;
                        }
                        tPossibilities += t;
                        nNotDivisible += n;
                    }
                }
                Probability<int> p = new Probability<int>(nNotDivisible, tPossibilities);
                QueuedConsole.WriteImmediate("Numerator : {0}, Denominator : {1}, Probability : {2}", nNotDivisible, tPossibilities, p.ToDouble().ToString("0.###"));
            }

            void DefeatHydra()
            {
                double p = (Math.Sqrt(5) - 1) / 2.0;
                p *= Math.Pow(10, 10);
                p = Math.Floor(p);
                QueuedConsole.WriteImmediate("Math.Floor(10^10*p) : {0}", (long)p);
            }

            /// <summary>
            /// https://brilliant.org/problems/casework-on-x/
            /// </summary>
            void CaseworkOnX()
            {
                SimpleCounter c = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation3.Solutions(new int[] { 6, 3, 2 }, 2016);
                QueuedConsole.WriteImmediate("Number of triples : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/greater-and-greater/
            /// </summary>
            void GreaterAndGreater()
            {
                int count = 0;
                for (int A = 1; A < 30; A++)
                {
                    for (int B = A + 1; B < 30; B++)
                    {
                        for (int C = B + 2; C < 30; C++)
                        {
                            for (int D = C + 3; D < 30; D++)
                            {
                                for (int E = D + 4; E < 30; E++)
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of ways : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/empty-and-divide/
            /// </summary>
            void EmptyAndDivide()
            {
                Dictionary<string, bool> MNSimulation = new Dictionary<string, bool>();
                int max = 1000;

                Func<int, int, string> GetKey = delegate (int m, int n) {
                    return m + "#$" + n;
                };
                MNSimulation.Add(GetKey(1, 1), false);

                Func<int, int, bool, Player> SimulateGame = null;
                SimulateGame = delegate (int m, int n, bool firstPlayerTurn)
                {
                    Player FirstPlayer = new Player();

                    string key = firstPlayerTurn ? GetKey(m, n) : GetKey(n, m);
                    if (MNSimulation.ContainsKey(key))
                    {
                        if (firstPlayerTurn) FirstPlayer.HasWon = MNSimulation[key];
                        else FirstPlayer.HasWon = !MNSimulation[key];
                        return FirstPlayer;
                    }

                    int m2 = 0, n2 = 0;
                    if (firstPlayerTurn)
                    {
                        bool isEven = n % 2 == 0;
                        if (isEven) {
                            m2 = n / 2;
                            n2 = n / 2;

                            string key2 = GetKey(m2, n2);
                            FirstPlayer.HasWon = (SimulateGame(m2, n2, !firstPlayerTurn)).HasWon;
                            MNSimulation.GenericAddOrReplace(key2, FirstPlayer.HasWon);
                            return FirstPlayer;
                        } else {
                            m2 = (n - 1) / 2;
                            n2 = n - m2;
                            
                            string key2 = GetKey(m2, n2);
                            if (m2 == 0) FirstPlayer.HasWon = false;
                            else FirstPlayer.HasWon = (SimulateGame(m2, n2, !firstPlayerTurn)).HasWon;

                            if (!FirstPlayer.HasWon) {
                                m2 = (n + 1) / 2;
                                n2 = n - m2;
                                if (n2 == 0) FirstPlayer.HasWon = false;
                                else {
                                    key2 = GetKey(m2, n2);
                                    FirstPlayer.HasWon = (SimulateGame(m2, n2, !firstPlayerTurn)).HasWon;
                                }
                            }

                            MNSimulation.GenericAddOrReplace(key2, FirstPlayer.HasWon);
                            return FirstPlayer;
                        }
                    } else {
                        bool isEven = m % 2 == 0;
                        if (isEven) {
                            m2 = m / 2;
                            n2 = m / 2;

                            string key2 = GetKey(n2, m2);
                            FirstPlayer.HasWon = (SimulateGame(m2, n2, !firstPlayerTurn)).HasWon;
                            MNSimulation.GenericAddOrReplace(key2, !FirstPlayer.HasWon);
                            return FirstPlayer;
                        } else {
                            m2 = (m - 1) / 2;
                            n2 = m - m2;
                            
                            string key2 = GetKey(n2, m2);
                            if (m2 == 0) FirstPlayer.HasWon = true;
                            else FirstPlayer.HasWon = (SimulateGame(m2, n2, !firstPlayerTurn)).HasWon;

                            if (FirstPlayer.HasWon) {
                                m2 = (m + 1) / 2;
                                n2 = m - m2;
                                if (n2 == 0) FirstPlayer.HasWon = true;
                                else {
                                    key2 = GetKey(n2, m2);
                                    FirstPlayer.HasWon = (SimulateGame(m2, n2, !firstPlayerTurn)).HasWon;
                                }
                            }

                            MNSimulation.GenericAddOrReplace(key2, !FirstPlayer.HasWon);
                            return FirstPlayer;
                        }
                    }
                };

                int count = 0;
                for (int m = 2; m < max; m++)
                {
                    for (int n = 2; n < max; n++)
                    {
                        if ((SimulateGame(m, n, true)).HasWon) count++;
                    }
                }
                QueuedConsole.WriteImmediate("Number of first player wins : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/coin-flipping-3/
            /// </summary>
            void CoinFlipping()
            {
                int maxFlips = 10;
                Action<int, List<int>> Flip = null;
                int positiveArrangements = 0, totalFlips = 0;
                string pattern = "11111";
                Flip = delegate (int flips, List<int> flipsSoFar) {
                    for(int i = 0; i <= 1; i++)
                    {
                        List<int> flipsAfter = new List<int>(flipsSoFar);
                        flipsAfter.Add(i);
                        if(flips == maxFlips - 1)
                        {
                            totalFlips++;

                            string flipPattern = string.Join("", flipsAfter);
                            if(flipPattern.IndexOf(pattern) >= 0) { positiveArrangements++; }
                        } else {
                            Flip(flips + 1, flipsAfter);
                        }
                    }
                };

                Flip(0, new List<int>());

                Fraction<int> fr = new Fraction<int>(positiveArrangements, totalFlips, false);
                QueuedConsole.WriteImmediate("a/b : {0}/{1}, a+b : {2}", fr.N, fr.D, fr.N + fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/combinatorial-madness/
            /// </summary>
            void CombinatorialMadness()
            {
                UniqueIntegralPairs uip = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(15, 5, 1);
                long sum = 0;
                uip.GetCombinations().ForEach(x =>
                {
                    int p = 1;
                    Enumerable.Range(0, 5).ForEach(y => { p *= x.Pair[y]; });
                    sum += p;
                });
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/more-tic-tac-toe/
            /// </summary>
            void TicTacToe()
            {
                int n = 3, nMax = n * n;
                List<List<int>> Rows = new List<List<int>>() {
                    new List<int>() {1,2,3 },
                    new List<int>() {4,5,6 },
                    new List<int>() {7,8,9 }
                };
                List<List<int>> Columns = new List<List<int>>() {
                    new List<int>() {1,4,7 },
                    new List<int>() {2,5,8 },
                    new List<int>() {3,6,9 }
                };
                List<List<int>> Diagonals = new List<List<int>>() {
                    new List<int>() {1,5,9 },
                    new List<int>() {3,5,7 }
                };

                List<List<List<int>>> PossibleWinScenarios = new List<List<List<int>>>();
                PossibleWinScenarios.Add(Rows);
                PossibleWinScenarios.Add(Columns);
                PossibleWinScenarios.Add(Diagonals);

                int drawsCount = 0;
                HashSet<string> EndPositionDraws = new HashSet<string>();
                Action<int, HashSet<int>, HashSet<int>, HashSet<int>> Move = null;
                Move = delegate (int nMoves, HashSet<int> aMoves, HashSet<int> bMoves, HashSet<int> used)
                {
                    bool isAMove = (nMoves - 1) % 2 == 0;
                    for (int i = 1; i <= nMax; i++)
                    {
                        if (used.Contains(i)) continue;

                        HashSet<int> a2 = isAMove ? new HashSet<int>(aMoves) : aMoves;
                        HashSet<int> b2 = !isAMove? new HashSet<int>(bMoves) : bMoves;
                        HashSet<int> used2 = new HashSet<int>(used);

                        used2.Add(i);
                        if (isAMove) { a2.Add(i); }
                        else { b2.Add(i); }

                        if (nMoves == nMax) {
                            bool isDraw = true;

                            foreach(List<List<int>> Scenarios in PossibleWinScenarios)
                            {
                                foreach (List<int> r in Scenarios)
                                {
                                    int aCount = 0, bCount = 0;
                                    foreach (int ri in r)
                                    {
                                        if (a2.Contains(ri)) aCount++;
                                        else if (b2.Contains(ri)) bCount++;
                                    }
                                    if (aCount == n || bCount == n) { isDraw = false; }
                                    if (!isDraw) break;
                                }

                                if (!isDraw) break;
                            }

                            if (isDraw) {
                                char[] Positions = new char[9];
                                a2.ForEach(x => { Positions[x - 1] = 'a'; });
                                b2.ForEach(x => { Positions[x - 1] = 'b'; });
                                string finalPosition = string.Join("-", Positions);

                                drawsCount++;
                                EndPositionDraws.Add(finalPosition);
                            }
                        } else {
                            bool isWon = false;
                            if (nMoves >= 5)
                            {
                                foreach (List<List<int>> Scenarios in PossibleWinScenarios)
                                {
                                    foreach (List<int> r in Scenarios)
                                    {
                                        int aCount = 0, bCount = 0;
                                        foreach (int ri in r)
                                        {
                                            if (a2.Contains(ri)) aCount++;
                                            else if (b2.Contains(ri)) bCount++;
                                        }
                                        if (aCount == n || bCount == n) { isWon = true; }
                                        if (isWon) break;
                                    }

                                    if (isWon) break;
                                }
                                if (isWon) { return; }
                            }

                            if(!isWon) Move(nMoves + 1, a2, b2, used2);
                        }
                    }
                };

                Move(1, new HashSet<int>(), new HashSet<int>(), new HashSet<int>());

                QueuedConsole.WriteImmediate("Number of different end position draws : {0}", EndPositionDraws.Count);
                QueuedConsole.WriteImmediate("Total Draw Count : {0}", drawsCount);

                EndPositionDraws.ForEach(x =>
                {
                    QueuedConsole.WriteImmediate("-------------------");
                    string[] positions = x.Split(new char[] { '-' });
                    QueuedConsole.WriteImmediate("{0}", string.Join("-", positions.Take(3)));
                    QueuedConsole.WriteImmediate("{0}", string.Join("-", positions.Skip(3).Take(3)));
                    QueuedConsole.WriteImmediate("{0}", string.Join("-", positions.Skip(6).Take(3)));
                    QueuedConsole.WriteImmediate("-------------------");
                });
            }

            /// <summary>
            /// https://brilliant.org/problems/proper-brackets/
            /// </summary>
            void ProperSequences()
            {
                int n = 8, maxDepth = 2 * n;
                Action<int, List<int>, Dictionary<int, List<int>>, string> Add = null;
                SimpleCounter counter = new SimpleCounter();
                Add = delegate (int depth, List<int> used, Dictionary<int, List<int>> occurrences, string sequence) {
                    for (int i = 0; i < n; i++)
                    {
                        if (used.Where(x => x == i).Count() == 2) continue;

                        List<int> next = new List<int>(used);
                        next.Add(i);

                        string nextSeq = sequence + i;

                        Dictionary<int, List<int>> nextOccur = occurrences.ToDictionary(p => p.Key, q => q.Value.ToList());
                        if (nextOccur.ContainsKey(i)) nextOccur[i].Add(depth);
                        else nextOccur.Add(i, new List<int>() { depth });

                        if (depth == maxDepth - 1) {
                            counter.Increment();
                        } else {
                            var filter = nextOccur.Where(x => x.Value.Count == 2);
                            bool inCorrect = false;
                            foreach(KeyValuePair<int, List<int>> x in filter)
                            {
                                string subS = nextSeq.Substring(x.Value[0] + 1, x.Value[1] - x.Value[0] - 1);
                                if (subS.Length % 2 == 0) {
                                    foreach (char c in subS)
                                    {
                                        if (nextOccur[(int)char.GetNumericValue(c)].Count != 2) { inCorrect = true; break; }
                                    }
                                }
                                else { inCorrect = true; break; }
                            }

                            if (inCorrect) { continue; }
                            else Add(depth + 1, next, nextOccur, nextSeq);
                        }
                    }
                };

                Add(0, new List<int>(), new Dictionary<int, List<int>>(), string.Empty);
                QueuedConsole.WriteImmediate("For n: {0}, Proper sequences : {1}", n, counter.GetCount());
            }
        }

        internal class Collection2
        {
            internal void Solve()
            {
                TossingACoin();
            }

            /// <summary>
            /// https://brilliant.org/problems/tossing-a-coin/
            /// </summary>
            void TossingACoin()
            {
                Dictionary<int, int> d = new Dictionary<int, int>() { { 11, 0 }, { 12, 0 }, { 13, 0 }, { 14, 0 }, { 15, 0 }, { 16, 0 }, { 17, 0 } };
                string[] _h = new string[7];
                for (int i = 0; i < 7; i++) { Enumerable.Range(1, 11 + i).ForEach(n => { _h[i] += "h"; }); }

                Probability<int> p = new Probability<int>();
                Action<int, string> Toss = null;
                Toss = delegate (int depth, string tosses)
                {
                    for (int n = 0; n < 2; n++)
                    {
                        string next = tosses + "";
                        if (n == 0) next += "h";
                        else next += "t";

                        if (depth == 16)
                        {
                            if (next.Contains(_h[0]))
                            {
                                p.IncrementFavorable();
                                for (int a = 0; a < 7; a++)
                                {
                                    if (next.StartsWith(_h[a] + "t") || next.Contains("t" + _h[a] + "t") || next.EndsWith("t" + _h[a]) || next.Equals(_h[a])) d[11 + a] += 1;
                                }
                            } else p.JustIncrement();
                        } else Toss(depth + 1, next);
                    }
                };

                Toss(0, "");
                QueuedConsole.WriteImmediate("{0}", p);
                d.ForEach(kvp => { QueuedConsole.WriteImmediate("{0} : {1}", kvp.Key, kvp.Value); });
            }

            /// <summary>
            /// https://brilliant.org/problems/money-problem-3/
            /// </summary>
            void MoneyProblem()
            {
                List<int> Coins = new List<int>() { 1, 8, 32, 80, 350 };

                int TotalCoins = 21;
                int maxDiff = 0;
                for (int n = 1; n < 2000; n++)
                {
                    int coins = 0;
                    int unused = n;
                    while (unused > 0)
                    {
                        unused -= Coins.Where(x => x <= unused).Max();
                        coins++;
                    }
                    if (TotalCoins - coins < 6)
                    {
                        maxDiff = Math.Max(maxDiff, 6 - (TotalCoins - coins));
                    }
                }
                QueuedConsole.WriteImmediate("{0}", TotalCoins + maxDiff);
            }

            /// <summary>
            /// https://brilliant.org/problems/lazy-larrys-random-algorithm/
            /// </summary>
            void LazyLarrysRandomAlgorithm()
            {
                int nsimulations = 0, nMax = 1000000;
                string compare = "abcd";
                string word = "cdab";
                int expectedValueTotal = 0;

                List<int> array = Enumerable.Range(0, 4).ToList();
                Func<Pool<int>, Pool<int>> init = delegate (Pool<int> p)
                {
                    p.AddRange(array);
                    return p;
                };

                Pool<int> pool = new Pool<int>(init);
                while (++nsimulations <= nMax)
                {
                    string s = "" + word;
                    while (!(s.Equals(compare)))
                    {
                        expectedValueTotal++;
                        int a1 = pool.Pop(), a2 = pool.Pop();
                        char c1 = s[a1], c2 = s[a2];
                        if (a1 < a2 && c1 > c2)
                        {
                            s = s.ReplaceAt(a1, c2);
                            s = s.ReplaceAt(a2, c1);
                        } else if (a2 < a1 && c2 > c1) {
                            s = s.ReplaceAt(a1, c2);
                            s = s.ReplaceAt(a2, c1);
                        }
                        pool.ReInitialize();
                    }
                    if (nsimulations % 100000 == 0)
                        QueuedConsole.WriteImmediate("After {1} simulations, expected value : {0}", expectedValueTotal * 1.0 / nsimulations, nsimulations);
                }
                --nsimulations;
            }

            /// <summary>
            /// https://brilliant.org/problems/at-least-i-have-2-shoes-even-if-they-do-not-match/
            /// </summary>
            void AtleastIhave2shoes()
            {
                int favorable = 0;
                int total = 120 * 120;
                Enumerable.Range(0, 6).ForEach(n =>
                {
                    int ncr = ((int)PermutationsAndCombinations.nCr(5, n));
                    int dn = PermutationsAndCombinations.Derangements(n);
                    int d5mn = PermutationsAndCombinations.Derangements(5 - n);
                    favorable +=  ncr * dn * d5mn;
                });
                Probability<int> P = new Probability<int>(favorable, total);
                QueuedConsole.WriteImmediate("{0}", P);
            }

            /// <summary>
            /// https://brilliant.org/problems/too-much-2/
            /// </summary>
            void Toomuch2()
            {
                BigInteger number = PermutationsAndCombinations.Factorial(9);
                number *= 9;
                BigInteger sum = 0;
                Enumerable.Range(1, 10).ForEach(n =>
                {
                    sum += number / PermutationsAndCombinations.Factorial(10 - n);
                });
                QueuedConsole.WriteImmediate("{0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/cool-6-digit-numbers/
            /// </summary>
            void Cool6DigitNumbers()
            {
                int count = 0;
                int n = 99999;
                while (++n <= 1000000)
                {
                    int[] dA = Numbers.GetDigitArray(n);
                    bool isCool = true;
                    for (int i = 1; i < dA.Length; i++)
                    {
                        if(dA[i] < dA[i - 1]) { isCool = false; break; }
                    }
                    if (isCool) count++;
                }
                QueuedConsole.WriteImmediate("{0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/count-and-count/
            /// </summary>
            void CountAndCount()
            {
                int n = 0, A = 0, B = 0;
                while(++n <= 100000)
                {
                    string s = "" + n;
                    s.ForEach(c =>
                    {
                        if (c == '1') A++;
                        if (c == '2') B++;
                    });
                }
                QueuedConsole.WriteImmediate("A-B : {0}", A - B);
            }

            /// <summary>
            /// https://brilliant.org/problems/thats-odd-3/
            /// </summary>
            void ThatsOdd()
            {
                Probability<int> P = new Probability<int>();
                int MaxSimulations = 20000000;
                int n = 0;
                while (++n <= MaxSimulations)
                {
                    Random r = new Random();
                    double x = r.NextDouble();
                    double y = r.NextDouble();
                    int round = (int)Math.Round(x / y);
                    if (MathFunctions.IsOdd(round)) P.IncrementFavorable();
                    else P.JustIncrement();
                    if (n % 1000000 == 0) QueuedConsole.WriteImmediate("P : {0}, [10000P] : {1}, after simulations : {2}", P.ToDouble(), Math.Ceiling(10000 * P.ToDouble()), n);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/parking-problems/
            /// </summary>
            void ParkingProblems()
            {
                Probability<int> P = new Probability<int>();
                Action<int, List<int>> Park = null;
                Park = delegate (int depth, List<int> used)
                {
                    for (int a = 1; a <= 10; a++)
                    {
                        if (used.Contains(a)) continue;
                        List<int> next = new List<int>(used);
                        next.Add(a);
                        if(depth < 5) { Park(depth + 1, next); }
                        else {
                            List<int> numbers = Enumerable.Range(1, 10).ToList();
                            next.ForEach(n => { numbers.Remove(n); });
                            numbers.Sort();
                            bool canPark = false;
                            foreach(int n in numbers)
                            {
                                if (numbers.Contains(n + 1))
                                {
                                    P.IncrementFavorable();
                                    canPark = true;
                                    break;
                                }
                            }
                            if (!canPark) P.JustIncrement();
                        }
                    }
                };
                Park(0, new List<int>());
                QueuedConsole.WriteImmediate("{0}", P);
            }

            /// <summary>
            /// https://brilliant.org/problems/random-number-generator/
            /// </summary>
            void RandomNumberGenerator4000()
            {
                int MaxSimulations = 10000000;
                int n = 0;
                long TotalNumbers = 0;
                while (++n <= MaxSimulations)
                {
                    double sum = 0.0;
                    int numbers = 0;
                    Random r = new Random();
                    while (++numbers > 0 && sum < 1.0)
                    {
                        sum += r.NextDouble();
                    }

                    TotalNumbers += numbers;
                    if (n % 1000000 == 0) QueuedConsole.WriteImmediate("expected number after {0} simulations : {1}", n, (TotalNumbers * 1.0) / n);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/hhh/
            /// </summary>
            void CoinFlipThreeConsecutiveheads()
            {
                Probability<int> P = new Probability<int>();
                Action<int, string> Flip = null;
                Flip = delegate (int depth, string flips)
                {
                    for (int a = 1; a <= 2; a++)
                    {
                        string next = flips;
                        if (a == 1) { next += "h"; }
                        else { next += "t"; }

                        if (depth == 9) {
                            if (next.Contains("hhh")) P.IncrementFavorable();
                            else P.JustIncrement();
                        } else {
                            Flip(depth + 1, next);
                        }
                    }
                };
                Flip(0,"");
                QueuedConsole.WriteImmediate("a/b : {0}", P);
            }

            /// <summary>
            /// https://brilliant.org/problems/hin-dus-tan/
            /// </summary>
            void Hindustan()
            {
                Dictionary<int, char> NumToChar = new Dictionary<int, char>();
                NumToChar.Add(1, 'h');
                NumToChar.Add(2, 'i');
                NumToChar.Add(3, 'n');
                NumToChar.Add(4, 'd');
                NumToChar.Add(5, 'u');
                NumToChar.Add(6, 's');
                NumToChar.Add(7, 't');
                NumToChar.Add(8, 'a');
                NumToChar.Add(9, 'n');

                Action<int, string, List<int>> Add = null;
                int count = 0;
                List<string> tags = new List<string>() { "hin", "dus", "tan" };
                Add = delegate (int depth, string word, List<int> used)
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        if (used.Contains(n)) continue;
                        string next = word + NumToChar[n];

                        if (depth == 8)
                        {
                            bool found = false;
                            foreach(string tag in tags)
                            {
                                if (next.Contains(tag))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                count++;
                            }
                        }
                        else
                        {
                            List<int> usedNext = new List<int>(used);
                            usedNext.Add(n);
                            Add(depth + 1, next, usedNext);
                        }
                    }
                };
                Add(0, "", new List<int>());
                count /= 2;
                QueuedConsole.WriteImmediate("x: {0}, digitsum: {1}", count, MathFunctions.DigitSum(count));
            }

            /// <summary>https://brilliant.org/problems/solve-the-simultaneous-equations/
            /// </summary>
            void SolveTheSimultaneousEquations()
            {
                double average = 0.0;
                int nSimulations = 500000;
                int n = 0;
                int nMax = 50;
                while (++n > 0)
                {
                    int sim = 0;
                    Probability<int> probability = new Probability<int>();
                    while (++sim <= nSimulations)
                    {
                        Random r = new Random();
                        List<Tuple<int, double>> points = new List<Tuple<int, double>>();
                        int id = 0;
                        Tuple<int, double> p1 = new Tuple<int, double>(++id, r.NextDouble());
                        Tuple<int, double> p2 = new Tuple<int, double>(++id, r.NextDouble());
                        points.Add(p1);
                        points.Add(p2);

                        for (int a = 1; a <= 9; a++)
                        {
                            points.Add(new Tuple<int, double>(++id, r.NextDouble()));
                        }

                        points.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                        int index1 = points.FindIndex(a => a.Item1 == p1.Item1);
                        int index2 = points.FindIndex(a => a.Item1 == p2.Item1);

                        if (Math.Abs(index1 - index2) == 1) probability.IncrementFavorable();
                        else probability.JustIncrement();
                    }

                    QueuedConsole.WriteImmediate("simulation {0} : {1}", n, probability.ToDouble());
                    average += probability.ToDouble();

                    if (n == nMax) break;
                }

                QueuedConsole.WriteImmediate("average probability after {0} simulations : {1}", n, average / n);
            }

            /// <summary>
            /// https://brilliant.org/problems/presents-for-all/
            /// </summary>
            void PresentsForAll()
            {
                int NElves = 7;
                int NChildren = 4;
                Probability<int> a = new Probability<int>();
                ParityObjects p = new ParityObjects(4);
                Action<int, List<int>> Gift = null;
                Gift = delegate (int nElf, List<int> gifts)
                {
                    for(int i = 0; i < NChildren; i++)
                    {
                        List<int> next = new List<int>(gifts);
                        next.Add(i);
                        if(nElf == NElves)
                        {
                            p.Reset();
                            next.ForEach(n =>
                            {
                                p.SetParity(n, true);
                            });
                            if (p.IsAllEven()) a.IncrementFavorable();
                            else a.JustIncrement();
                        } else Gift(nElf + 1, next);
                    }
                };

                Gift(1, new List<int>());

                QueuedConsole.WriteImmediate("{0}", a.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/pay-as-you-enter/
            /// </summary>
            void PayAsYouEnter()
            {
                HashSet<string> QSet = new HashSet<string>();
                int max = 5, maxDepth = 10;
                Action<int, int, int, List<int>> Add = null;
                Add = delegate (int depth, int nA, int nB, List<int> used)
                {
                    if (nA < nB) return;
                    for (int i = 0; i <= 1; i++)
                    {
                        int nextA = nA;
                        int nextB = nB;

                        List<int> next = new List<int>(used);

                        if (i == 0) {
                            if (nA < max) { nextA++; next.Add(0); }
                            else continue;
                        } else if (i == 1) {
                            if (nB < max) { nextB++; next.Add(1); }
                            else continue;
                        }

                        if (depth == maxDepth - 1) {
                            if (nextA == max && nextB == max)
                            {
                                QSet.Add(string.Join(".", next));
                            }
                        } else {
                            Add(depth + 1, nextA, nextB, next);
                        }
                    }
                };

                Add(0, 0, 0, new List<int>());
                QueuedConsole.WriteImmediate("count: {0}", QSet.Count);
            }

            /// <summary>
            /// https://brilliant.org/problems/divisible-sequences/
            /// </summary>
            void DivisibleSequences()
            {
                int n = 360;
                ClonedPrimes primes = KnownPrimes.CloneKnownPrimes(1, 360);
                Dictionary<long, List<long>> NFactors = new Dictionary<long, List<long>>();
                List<long> fn = (Factors.GetAllFactors(n, primes)).OrderBy(OrderByDirection.Descending);

                NFactors.Add(n, fn);

                int count = 0;
                Action<int, long> Add = null;
                Add = delegate (int length, long prev)
                {
                    List<long> factors = null;
                    if (NFactors.ContainsKey(prev))
                    {
                        factors = NFactors[prev];
                    } else {
                        factors = (Factors.GetAllFactors(prev, primes)).OrderBy(OrderByDirection.Descending);

                        NFactors.Add(prev, factors);
                    }
                    int len = length + 1;
                    foreach (long p in factors)
                    {
                        if (len == 5) {
                            count++;
                        } else {
                            Add(len, p);
                        }
                    }
                };

                Add(1, n);
                QueuedConsole.WriteImmediate("The number of divisible sequences of length 5 starting from the number 360: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/squares-12/
            /// </summary>
            void Squares12()
            {
                List<List<int>> Rows = new List<List<int>>();
                Enumerable.Range(0, 100).ForEach(x =>
                {
                    List<int> row = null;
                    if (x % 10 == 0)
                    {
                        row = new List<int>();
                        Rows.Add(row);
                    }
                    else { row = Rows.Last(); }

                    if (x == 12) row.Add(40);
                    else if (x == 39) row.Add(13);
                    else row.Add(x + 1);
                });

                Dictionary<int, int> SumGrid = new Dictionary<int, int>();
                for (int x = 0; x < 8; x++)
                {
                    List<int> row1 = Rows[x], row2 = Rows[x + 1], row3 = Rows[x + 2];
                    for (int y = 0; y < 8; y++)
                    {
                        int sum = row1[y] + row1[y + 1] + row1[y + 2];
                        sum += row2[y] + row2[y + 1] + row2[y + 2];
                        sum += row3[y] + row3[y + 1] + row3[y + 2];

                        if (SumGrid.ContainsKey(sum)) { SumGrid[sum] = SumGrid[sum] + 1; }
                        else { SumGrid.Add(sum, 1); }
                    }
                }
                long nWays = 0;
                SumGrid.ForEach(x =>
                {
                    if (x.Value > 1)
                    {
                        QueuedConsole.WriteImmediate("Sum:{0}, grids: {1}", x.Key, x.Value);
                        nWays += PermutationsAndCombinations.nCr(x.Value, 2);
                    }
                });

                QueuedConsole.WriteImmediate("Number of ways: {0}", nWays);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-animal-evolution/
            /// </summary>
            void TheAnimalEvolution()
            {
                int n = 6;
                Dictionary<int, long> Combinations = new Dictionary<int, long>();
                Enumerable.Range(0, n + 1).ForEach(x =>
                {
                    Combinations.Add(x, PermutationsAndCombinations.nCr(n, x));
                });

                long nWays = 0;
                for (int le = 0; le <= n; le++)
                {
                    int lp = n - le;

                    int bh = lp;

                    int te = lp;

                    nWays += Combinations[le] * Combinations[bh] * Combinations[te];
                }

                QueuedConsole.WriteImmediate("Number of ways : {0}", nWays);
            }

            /// <summary>
            /// https://brilliant.org/problems/half-linearly-expressible/
            /// </summary>
            void HalfLinearlyExpressible()
            {
                int nPairs = 0;
                for (int x = 1; x <= 100; x++)
                {
                    for (int y = 1; y <= 100; y++)
                    {
                        int count = 0;
                        for (int n = 1; n <= 60; n++)
                        {
                            bool found = false;
                            for (int a = 0; a <= 100; a++)
                            {
                                for (int b = 0; b <= 100; b++)
                                {
                                    int rhs = a * x + b * y;
                                    if (rhs > n) break;

                                    if (rhs == n) { found = true; }

                                    if (found) break;
                                }
                                if (found) break;
                            }

                            if (found) count++;
                        }
                        if (count == 30) nPairs++;
                    }
                }

                QueuedConsole.WriteImmediate("Number of pairs: {0}", nPairs);
            }

            /// <summary>
            /// https://brilliant.org/problems/just-one-0/
            /// </summary>
            void JustOneZero()
            {
                int n = 0, counter = 0;
                while (n <= 2016)
                {
                    string nStr = n + "";
                    if (nStr.Where(x => x.Equals('0')).Count() == 1) counter++;
                    n++;
                }

                Fraction<int> fr = new Fraction<int>(counter, n, false);
                QueuedConsole.WriteImmediate("a+b : {0}", fr.D + fr.N);
            }

            /// <summary>
            /// https://brilliant.org/problems/product-of-digits-in-an-11-digit-integer/
            /// </summary>
            void ProductOfDigitsIn11DigitNumber()
            {
                HashSet<long> set = new HashSet<long>();
                Action<int, int, int, int, int, int> ChooseNum = null;
                ChooseNum = delegate (int depth, int n1s, int n2s, int n3s, int n5s, int n7s)
                {
                    for (int x = 1; x <= 7; x++)
                    {
                        int n1sn = n1s, n2sn = n2s, n3sn = n3s, n5sn = n5s, n7sn = n7s;

                        if (x == 4 || x == 6) continue;

                        if (x == 1)
                        {
                            if (n1sn == 1) continue;
                            else n1sn++;
                        }
                        else if (x == 2)
                        {
                            if (n2sn == 2) continue;
                            else n2sn++;
                        }
                        else if (x == 3)
                        {
                            if (n3sn == 2) continue;
                            else n3sn++;
                        }
                        else if (x == 5)
                        {
                            if (n5sn == 4) continue;
                            else n5sn++;
                        }
                        else if (x == 7)
                        {
                            if (n7sn == 11) continue;
                            else n7sn++;
                        }

                        if (depth == 10)
                        {
                            long num = (long)(Math.Pow(2, n2sn) * Math.Pow(3, n3sn) * Math.Pow(5, n5sn) * Math.Pow(7, n7sn));
                            set.Add(num);
                        }
                        else
                        {
                            ChooseNum(depth + 1, n1sn, n2sn, n3sn, n5sn, n7sn);
                        }

                    }
                };

                ChooseNum(0, 0, 0, 0, 0, 0);
                QueuedConsole.WriteImmediate("Distinct possibilities: {0}", set.Count);
            }

            /// <summary>
            /// https://brilliant.org/problems/lucky-13/
            /// </summary>
            void SumOfDigits13()
            {
                int n = 1;
                int counter = 0;
                while (++n <= 1000000)
                {
                    if (MathFunctions.DigitSum(n) == 13) counter++;
                }
                QueuedConsole.WriteImmediate("Number of integers: {0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/where-can-i-get-30-other-people/
            /// </summary>
            void WhereCanIGet30OtherPeople()
            {
                List<int> Init = new List<int>();
                Enumerable.Range(1, 2015).ForEach(x => { Init.Add(x); });
                Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p) {
                    p.AddRange(Init.ToList());
                    return p;
                };

                Pool<int> pool = new Pool<int>(initializer);
                Dictionary<int, int> Winners = new Dictionary<int, int>();
                int a = 0;
                while (++a <= 2015)
                {
                    int nSimulations = 1500;
                    if (a == 2015) nSimulations = 1;

                    int simCounter = 0;
                    Dictionary<int, int> Simulation = new Dictionary<int, int>();
                    while(++simCounter <= nSimulations)
                    {
                        pool.ReInitialize();
                        IEnumerable<int> subset = pool.Pop(a);
                        int w = subset.Sum() % 31;
                        Simulation.AddOrUpdate(w, 1);
                    }

                    IOrderedEnumerable<KeyValuePair<int, int>> s = Simulation.OrderByDescending(key => key.Value);
                    int max = 0;
                    foreach (var kvp in s)
                    {
                        if (max == 0) max = kvp.Value;
                        if (kvp.Value == max) Winners.AddOrUpdate(kvp.Key, a);
                        else break;
                    }
                }

                IOrderedEnumerable<KeyValuePair<int, int>> wOrdered = Winners.OrderByDescending(key => key.Value);
                int rank = 0;
                foreach (var kvp in wOrdered)
                {
                    rank++;
                    if (rank <= 10) { QueuedConsole.WriteImmediate("S: {0}, Sum: {1}", kvp.Key, kvp.Value); }
                    else break;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/saturday-problem-of-combinatorics/
            /// </summary>
            void SaturdayProblemOfCombinatorics()
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

                long counter = 0;
                Action<int, List<int>, int[]> Fill = null;
                Fill = delegate (int depth, List<int> filled, int[] ri)
                {
                    for (int t = 1; t <= 16; t++)
                    {
                        if (filled.Contains(t)) continue;

                        List<int> next = new List<int>(filled);
                        next.Add(t);

                        int[] rinext = new int[8];
                        Enumerable.Range(0, 8).ForEach(x => { rinext[x] = ri[x]; });

                        if (depth == 0) { rinext[0] += t; rinext[4] += t; }
                        else if (depth == 1) { rinext[0] += t; rinext[5] += t; }
                        else if (depth == 2) { rinext[0] += t; rinext[6] += t; }
                        else if (depth == 3) { rinext[0] += t; rinext[7] += t; }
                        else if (depth == 4) { rinext[1] += t; rinext[4] += t; }
                        else if (depth == 5) { rinext[1] += t; rinext[5] += t; }
                        else if (depth == 6) { rinext[1] += t; rinext[6] += t; }
                        else if (depth == 7) { rinext[1] += t; rinext[7] += t; }
                        else if (depth == 8) { rinext[2] += t; rinext[4] += t; }
                        else if (depth == 9) { rinext[2] += t; rinext[5] += t; }
                        else if (depth == 10) { rinext[2] += t; rinext[6] += t; }
                        else if (depth == 11) { rinext[2] += t; rinext[7] += t; }
                        else if (depth == 12) { rinext[3] += t; rinext[4] += t; }
                        else if (depth == 13) { rinext[3] += t; rinext[5] += t; }
                        else if (depth == 14) { rinext[3] += t; rinext[6] += t; }
                        else if (depth == 15) { rinext[3] += t; rinext[7] += t; }

                        int p = 0;
                        if (depth == 3) p = 0;
                        else if (depth == 7) p = 1;
                        else if (depth == 11) p = 2;
                        else if (depth == 12) p = 4;
                        else if (depth == 13) p = 5;
                        else if (depth == 14) p = 6;
                        else if (depth == 15) p = 7;

                        if (p > 0)
                        {
                            if (p == 7)
                            {
                                if (rinext[3] % 2 == 0) continue;
                                if (rinext[7] % 2 == 0) continue;
                            }
                            else
                            {
                                if (rinext[p] % 2 == 0) continue;
                            }
                        }

                        if (depth == 15)
                        {
                            counter++;
                        }
                        else Fill(depth + 1, next, rinext);
                    }
                };
                Fill(0, new List<int>(), new int[8]);

                QueuedConsole.WriteImmediate("Number of arrangements: {0}", counter);

                long _8fact = (long)PermutationsAndCombinations.Factorial(8);
                int c = 0;
                while (counter % _8fact == 0)
                {
                    counter /= _8fact;
                    c++;
                }

                Dictionary<long, int> d = Factors.GetPrimeFactorsWithMultiplicity(counter);

                string nStr = string.Empty;
                d.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(nStr)) nStr += "x ";
                    nStr += x.Key + "^" + x.Value + " ";
                });

                QueuedConsole.WriteImmediate("Number can be written as : {0} x (8!)^{1}.", nStr, c);
                QueuedConsole.WriteImmediate("Answer: {0}", _8fact + counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/tricky-seating-arrangements/
            /// </summary>
            void TrickySeatingArrangements()
            {
                string[] c = new string[] { "0.1", "1.0", "2.3", "3.2", "4.5", "5.4", "6.7", "7.6", "8.9", "9.8" };

                int count = 0;
                Action<int, string, List<int>> Seat = null;
                Seat = delegate (int depth, string a, List<int> used)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (used.Contains(i)) continue;
                        List<int> next = new List<int>(used);

                        string sNext = a + "." + i;
                        next.Add(i);
                        if(depth == 9)
                        {
                            bool valid = true;
                            foreach(string s in c)
                            {
                                if (sNext.Contains(s)) { valid = false; break; }
                            }
                            if (valid) count++;
                        } else { Seat(depth + 1, sNext, next); }
                    }
                };

                Seat(0, null, new List<int>());
                QueuedConsole.WriteImmediate("Number of seating arrangements: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/tetrahedron-hopping/
            /// </summary>
            void TetrahedronHopping()
            {
                long nWays = 0;
                Action<int, int> Move = null;
                Move = delegate (int nMoves, int state)
                {
                    if (nMoves == 17)
                    {
                        if (state == 0) nWays++;
                        return;
                    }

                    int next = nMoves + 1;
                    if(state == 0)
                    {
                        Move(next, 1);
                        Move(next, 2);
                        Move(next, 3);
                    } else if(state == 1) {
                        Move(next, 0);
                        Move(next, 2);
                        Move(next, 3);
                    } else if (state == 2) {
                        Move(next, 0);
                        Move(next, 1);
                        Move(next, 3);
                    } else if (state == 3) {
                        Move(next, 0);
                        Move(next, 1);
                        Move(next, 2);
                    }

                };
                Move(0, 0);
                QueuedConsole.WriteImmediate("Possible ways: {0}", nWays);
            }

            /// <summary>
            /// https://brilliant.org/problems/tedious-permutation/
            /// </summary>
            void TediousPermutation()
            {
                int count = 0;
                Action<int, string, HashSet<int>> Add = null;
                Add = delegate (int depth, string permutation, HashSet<int> used) {
                    for(int i = 1; i <= 9; i++)
                    {
                        if (used.Contains(i)) continue;

                        string pN = permutation + i;
                        HashSet<int> next = new HashSet<int>(used);
                        next.Add(i);

                        if (depth == 8)
                        {
                            if(pN.IndexOf("1") < pN.IndexOf("2"))
                            {
                                if (pN.IndexOf("3") < pN.IndexOf("4"))
                                {
                                    if (pN.IndexOf("5") < pN.IndexOf("6"))
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                        else Add(depth + 1, pN, next);
                    }
                };

                Add(0, "", new HashSet<int>());
                QueuedConsole.WriteImmediate("NUmber of permutations: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/are-you-ready-for-brimo/
            /// </summary>
            void AreYouReadyForBrimo()
            {
                BigInteger nWaysSitting = PermutationsAndCombinations.Factorial(9);
                int count = 0;
                Action<int, string, int, List<int>> Add = null;
                Add = delegate (int depth, string permutation, int prev, List<int> used)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        int cur = i % 2 == 0 ? i / 2 : i / 2 + 1;

                        if (prev == cur) continue;
                        if (used.Count(x => x == cur) > 1) continue;

                        string pN = permutation + cur;
                        List<int> next = new List<int>(used);
                        next.Add(cur);

                        if (depth == 9)
                        {
                            if (pN[0] != pN[9])
                            {
                                count++;
                            }
                        }
                        else Add(depth + 1, pN, cur, next);
                    }
                };

                Add(0, "", -1, new List<int>());
                QueuedConsole.WriteImmediate("Number of permutations: {0}", nWaysSitting * count);
            }

            /// <summary>
            /// https://brilliant.org/problems/how-do-i-start-2/
            /// </summary>
            void HowDoIStart2()
            {
                int nWays = 0;
                Action<int, string, List<int>> Add = null;
                Add = delegate (int depth, string permutation, List<int> used)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        string pN = permutation + i;
                        List<int> next = new List<int>(used);
                        next.Add(i);

                        if(depth >= 4)
                        {
                            if (pN.IndexOf("1111") >= 0) continue;
                        }

                        if (depth == 11) { nWays++; }
                        else Add(depth + 1, pN, next);
                    }
                };

                Add(0, "", new List<int>());
                QueuedConsole.WriteImmediate("Number of permutations: {0}", nWays);
            }

            /// <summary>
            /// https://brilliant.org/problems/book-has-pages-right/
            /// </summary>
            void BookHasPagesRight()
            {
                long count = 0;
                int N = 101;
                Action<int, int, List<int>> Add = null;
                Add = delegate (int prev, int sum, List<int> pages)
                {
                    for (int i = prev + 2; i < N; i+=2)
                    {
                        int sNext = sum + i + i + 1;

                        List<int> pagesN = new List<int>(pages);
                        pagesN.Add(i); pagesN.Add(i + 1);

                        if (sNext == N) {
                            QueuedConsole.WriteImmediate("{0}", string.Join("-", pagesN));
                            count++;
                            continue;
                        }
                        if (sNext > N) return;

                        Add(i, sNext, pagesN);
                    }
                };

                Add(-1, 0, new List<int>());
                QueuedConsole.WriteImmediate("count: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/subsets-without-consecutive-integers/
            /// </summary>
            void SubsetsWithoutConsecutiveIntegers()
            {
                long count = 1;
                int N = 14;
                Action<int, int, HashSet<int>> Add = null;
                Add = delegate (int depth, int prev, HashSet<int> used)
                {
                    for (int i = prev + 1; i <= N; i++)
                    {
                        if (used.Contains(i) || used.Contains(i - 1) || used.Contains(i + 1)) continue;

                        HashSet<int> next = new HashSet<int>(used);
                        next.Add(i);

                        count++;

                        if (depth + 1 > 6)
                        {
                            continue;
                        }

                        Add(depth +1, i, next);
                    }
                };

                Add(0, 0, new HashSet<int>());
                QueuedConsole.WriteImmediate("count: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/line-up-3/
            /// </summary>
            void LineUp()
            {
                BigInteger nWays = PermutationsAndCombinations.nCrBig(34, 6);
                nWays -= PermutationsAndCombinations.nCrBig(33, 5);

                int d = 0;
                int count = 0;
                int amax = 0, bmax = 0;
                Action<int, int, int> Add = null;
                Add = delegate (int na, int nb, int depth)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        if (na == amax && i == 0) continue;

                        int nan = na;
                        int nbn = nb;
                        if (i == 0) nan += 1;
                        if (i == 1) nbn += 1;

                        if (nbn > nan) continue;

                        if (nan == nbn && depth == d - 1) count++;
                        else Add(nan, nbn, depth + 1);
                    }
                };

                int b = 5;
                int n = 33;

                while (1 > 0)
                {
                    d += 2;
                    n -= 2;
                    b--;
                    amax++;
                    bmax++;

                    count = 0;
                    Add(0, 0, 0);

                    nWays -= PermutationsAndCombinations.nCrBig(n, b) * count;

                    if (d == 10) break;
                }
                QueuedConsole.WriteImmediate("Number of ways: {0}", nWays.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/whats-the-number-11/
            /// </summary>
            void DigitSum18()
            {
                int count = 0;
                Enumerable.Range(1, 1000000).ForEach(x =>
                {
                    if (MathFunctions.DigitSum(x) == 18) count++;
                });
                QueuedConsole.WriteImmediate("Count: {0}", count);
            }
        }

        internal class ExpectedValue
        {
            /// <summary>
            /// https://brilliant.org/problems/the-traveller-and-three-kegs/
            /// </summary>
            internal static void TheTravellerAndThreeKegs()
            {
                int RemainingWater = 0, Trials = 0;
                Action<List<int>, int> Drink = null;
                Drink = delegate (List<int> kegs, int depth)
                {
                    for (int i = 0; i < kegs.Count; i++)
                    {
                        List<int> next = new List<int>(kegs);

                        if (next[i] == 1) next.RemoveAt(i);
                        else next[i] -= 1;

                        if(next.Count == 1) {
                            Trials++;
                            RemainingWater += next[0];
                        } else Drink(next, depth + 1);
                    }
                };

                Drink(new List<int>() { 1, 2, 3 }, 1);
                Fraction<int> fr = new Fraction<int>(RemainingWater, Trials, false);
                QueuedConsole.WriteImmediate("a/b: {0}/{1}", fr.N, fr.D);
            }
        }

        internal class Distribution
        {
            /// <summary>
            /// https://brilliant.org/problems/grouping-the-marbles/
            /// </summary>
            internal static void GroupingTheMarbles()
            {
                UniqueIntegralPairs uip = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation1.Solve(15, 5, 1, true);
                QueuedConsole.WriteImmediate("Number of ways to distribute marbles : {0}", uip.Count());
            }
        }

        internal class Derangement
        {
            /// <summary>
            /// https://brilliant.org/problems/hat-party/
            /// </summary>
            internal static void HatParty()
            {
                int n = 6;
                dynamic[] persons = new dynamic[n];
                for (int i = 0; i < n; i++)
                {
                    dynamic p = new ExpandoObject();
                    p.ID = i;
                    p.Hat = i;
                    persons[i] = p;
                }
                int cP = 0, cT = 0;
                Func<int, int> Recursion = null;
                Recursion = delegate (int depth) {
                    for (int i = 0; i < n; i++)
                    {
                        persons[depth].Hat = i;
                        if (depth == (n - 1))
                        {
                            bool found = true;
                            List<int> hats = new List<int>();
                            foreach (dynamic p in persons)
                            {
                                hats.Add(p.Hat);
                                if (p.ID == p.Hat) { found = false; }
                            }
                            if (hats.Distinct().Count() == n)
                            {
                                cT++;
                                if (found) { cP++; }
                            }
                            else if (i < n) continue;
                            return 0;
                        }
                        else { Recursion(depth + 1); }
                    }
                    return 0;
                };

                Recursion(0);
                Fraction<int> fr = new Fraction<int>(cP, cT, false);
                QueuedConsole.WriteImmediate("Positive arrangements : {0}, Total arrangements : {1}", cP, cT);
                QueuedConsole.WriteImmediate("Probability a/b : {0}/{1}, decimal value : {2}", fr.N, fr.D, fr.N * 1.0 / fr.D);
            }

            /// <summary>
            /// https://brilliant.org/problems/goofing-around/
            /// </summary>
            internal static void GoofingAround()
            {
                string word = "fooling";
                int n = word.Length;
                char[] wCA = word.ToCharArray();
                Array.Sort(wCA);
                string wordSorted = new string(wCA);
                dynamic[] characters = new dynamic[n];
                for (int i = 0; i < n; i++)
                {
                    dynamic p = new ExpandoObject();
                    p.ID = i;
                    p.Character = word[i];
                    characters[i] = p;
                }
                int cP = 0;
                Func<int, int> Recursion = null;
                UniqueArrangements<char> ua = new UniqueArrangements<char>();
                Recursion = delegate (int depth) {
                    for (int i = 0; i < n; i++)
                    {
                        characters[depth].Character = word[i];
                        if (depth == (n - 1))
                        {
                            bool found = true;
                            char[] charWord = new char[n];
                            int t = 0;
                            foreach (dynamic p in characters)
                            {
                                charWord[t] = p.Character;
                                t++;
                                if (((char)word[p.ID]).Equals((char)p.Character)) { found = false; }
                            }

                            char[] sortedCharArr = charWord.ToArray();
                            Array.Sort(sortedCharArr);
                            string wCASorted = new string(sortedCharArr);
                            if (!wCASorted.Equals(wordSorted)) continue;

                            if (ua.Add((new string(charWord)).ToList()))
                            {
                                if (found) { cP++; }
                            }
                            else if (i < n) continue;
                            return 0;
                        }
                        else { Recursion(depth + 1); }
                    }
                    return 0;
                };

                Recursion(0);
                QueuedConsole.WriteImmediate("Total words : {0}", ua.GetCount());
                QueuedConsole.WriteImmediate("Positive arrangements : {0}, Total arrangements : {1}", cP, ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/goofing-around/
            /// </summary>
            internal static void GoofingAround2()
            {
                string word = "goofing";
                int n = word.Length;
                char[] wCA = word.ToCharArray();
                Array.Sort(wCA);
                string wordSorted = new string(wCA);
                dynamic[] characters = new dynamic[n];
                for (int i = 0; i < n; i++)
                {
                    dynamic p = new ExpandoObject();
                    p.ID = i;
                    p.Character = word[i];
                    characters[i] = p;
                }
                int cP = 0;
                Func<int, int> Recursion = null;
                UniqueArrangements<char> ua = new UniqueArrangements<char>();
                Recursion = delegate (int depth) {
                    for (int i = 0; i < n; i++)
                    {
                        characters[depth].Character = word[i];
                        if (depth == (n - 1))
                        {
                            bool found = true;
                            char[] charWord = new char[n];
                            int t = 0;
                            foreach (dynamic p in characters)
                            {
                                charWord[t] = p.Character;
                                t++;
                                if (((char)word[p.ID]).Equals((char)p.Character)) { found = false; }
                            }

                            char[] sortedCharArr = charWord.ToArray();
                            Array.Sort(sortedCharArr);
                            string wCASorted = new string(sortedCharArr);
                            if (!wCASorted.Equals(wordSorted)) continue;

                            if (ua.Add((new string(charWord)).ToList()))
                            {
                                if (found) { cP++; }
                            }
                            else if (i < n) continue;
                            return 0;
                        }
                        else { Recursion(depth + 1); }
                    }
                    return 0;
                };

                Recursion(0);
                QueuedConsole.WriteImmediate("Total words : {0}", ua.GetCount());
                QueuedConsole.WriteImmediate("Positive arrangements : {0}, Total arrangements : {1}", cP, ua.GetCount());
            }
        }

        internal class SetsAndCombinations
        {
            /// <summary>
            /// https://brilliant.org/problems/power-sum-2/
            /// </summary>
            internal static void PowerSum()
            {
                IEnumerable<IEnumerable<int>> subsets = PowerSet.GetPowerSet(new List<int>() { 1, 2, 3, 4, 5 });
                BigRational br = new BigRational(0, 1);
                foreach (IEnumerable<int> subset in subsets)
                {
                    int d = 1;
                    foreach (int t in subset)
                    {
                        if (t == 0) continue;
                        d *= t;
                    }
                    if (subset.Count() == 0) continue;
                    BigRational brset = new BigRational(1, d);
                    string strset = string.Join(",", subset);
                    QueuedConsole.WriteImmediate(string.Format("set : {0}, sum : {1}", strset, 1.0 / d));
                    br += brset;
                }

                QueuedConsole.WriteImmediate(string.Format("Power sum : {0}", 1.0 * (long)br.Numerator / (long)br.Denominator));
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/bodygaurds-in-a-territory/
            /// Four letters, two "a" and two "b" are filled into 16 cells of a matrix as given in the above figure. It is required each cell contains at 
            /// most one letter and each row or column cannot contain same letters. In how many ways can the matrix be filled?
            /// </summary>
            internal static void BodyguardsInTerritory()
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= 16; i++)
                {
                    list.Add(i);
                }

                Dictionary<int, List<int>> rows = new Dictionary<int, List<int>>();
                for (int i = 1; i <= 4; i++)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        int key = 4 * n + i;
                        List<int> iRowList = new List<int>();
                        int j = 1;
                        while (true)
                        {
                            if (i != j) iRowList.Add(4 * n + j);
                            j++;
                            if (j > 4) break;
                        }
                        rows.Add(key, iRowList);
                    }
                }

                Dictionary<int, List<int>> columns = new Dictionary<int, List<int>>();
                for (int i = 1; i <= 16; i++)
                {
                    int iMod = i % 4;
                    List<int> iColList = new List<int>();
                    foreach (int t in list)
                    {
                        if (t != i && t % 4 == iMod) iColList.Add(t);
                    }
                    columns.Add(i, iColList);
                }

                Func<IList<int>, bool> rowRule = delegate (IList<int> s)
                {
                    List<int> sRows = rows[s[0]];
                    if (sRows.Contains(s[1])) return false;
                    else return true;
                };
                Func<IList<int>, bool> colRule = delegate (IList<int> s)
                {
                    List<int> sCols = columns[s[0]];
                    if (sCols.Contains(s[1])) return false;
                    else return true;
                };
                SimpleCounter sc = new SimpleCounter();
                List<IList<int>> combinations = Combinations.GetAllCombinations(list, 4);
                foreach (IList<int> c in combinations)
                {
                    List<IList<int>> aCombinations = Combinations.GetAllCombinations(c, 2);
                    foreach (IList<int> a in aCombinations)
                    {
                        if (rowRule.Invoke(a) && colRule.Invoke(a))
                        {
                            IList<int> b = c.Except(a).ToList();
                            if (rowRule.Invoke(b) && colRule.Invoke(b))
                            {
                                sc.Increment();
                            }
                        }
                    }
                }

                QueuedConsole.WriteFinalAnswer("Matrix can be filled in : {0} ways.", sc.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/gotta-read-good-books-but-that-doesnt-mean-you/
            /// </summary>
            internal static void SkipAdjacentVolumesPermutations()
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= 10; i++) { list.Add(i); }
                List<IList<int>> combinations = Combinations.GetAllCombinations(list, 4);
                SimpleCounter sc = new SimpleCounter();
                foreach (IList<int> p in combinations)
                {
                    bool fail = false;
                    foreach (int pl in p)
                    {
                        if (p.Contains(pl - 1) || p.Contains(pl + 1))
                        {
                            fail = true;
                            break;
                        }
                    }
                    if (!fail) sc.Increment();
                }
                QueuedConsole.WriteFinalAnswer("Number of ways : {0}", sc.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/find-the-average-value/
            /// </summary>
            internal static void FindAverageOfPermutations()
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= 10; i++) { list.Add(i); }
                IEnumerable<IEnumerable<int>> permutations = Permutations.GetPermutations(list, 10);
                SimpleCounter sc = new SimpleCounter();
                foreach (IEnumerable<int> p in permutations)
                {
                    int absoluteValue = 0;
                    IEnumerator<int> enumerator = p.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        int val = enumerator.Current;
                        enumerator.MoveNext();
                        val -= enumerator.Current;
                        absoluteValue += Math.Abs(val);
                    }
                    sc.Increment(absoluteValue);
                }
                QueuedConsole.WriteFinalAnswer("Number of ways : {0}, Absolute Value : {1}, Average value : {2}"
                    , permutations.Count(), sc.GetCount().ToString(), ((double)sc.GetCount()) / permutations.Count());
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/counting-the-number-of-elements-of-a-set/
            /// </summary>
            internal static void CountingNumberOfElements()
            {
                Dictionary<int, List<int>> AiSets = new Dictionary<int, List<int>>();
                for(int i=1; i<= 15; i++)
                {
                    AiSets.Add(i, new List<int>() { i, i + 1 });
                }
                List<int> keys = AiSets.Keys.ToList();

                int n= 1;
                UniqueArrangements<int> ua = new UniqueArrangements<int>();
                while (true)
                {
                    List<IList<int>> combinations = Combinations.GetAllCombinations(keys, n);
                    foreach(IList<int> c in combinations)
                    {
                        SortedSet<int> set = new SortedSet<int>();
                        c.ForEach(x => (AiSets[x]).ForEach(y => set.Add(y)));
                        ua.Add(set.ToList());
                    }
                    n++;
                    if (n > 15) { n--; break; }
                }
                QueuedConsole.WriteImmediate("Distinct Elements of F{0} : {1}", n, ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/combinatorics-olympiad-problem-2/
            /// </summary>
            internal static void MultiSetCircularPermutations()
            {
                char[] chars = new char[3] { 'r', 'w', 'b' };
                int[] ai = new int[6];
                HashSet<string> ua = new HashSet<string>();
                for(ai[0] = 0; ai[0] < 3; ai[0]++)
                {
                    for (ai[1] = 0; ai[1] < 3; ai[1]++)
                    {
                        for (ai[2] = 0; ai[2] < 3; ai[2]++)
                        {
                            for (ai[3] = 0; ai[3] < 3; ai[3]++)
                            {
                                for (ai[4] = 0; ai[4] < 3; ai[4]++)
                                {
                                    for (ai[5] = 0; ai[5] < 3; ai[5]++)
                                    {
                                        List<int> distinctAi = ai.Distinct().ToList();
                                        bool found = true;
                                        foreach(int d in distinctAi)
                                        {
                                            found = ai.Count(y => y == d) != 2 ? false : true;
                                            if (!found) break;
                                        }

                                        if (found) {
                                            string key = "";
                                            ai.ForEach(x => key += chars[x]);
                                            found = false;
                                            for (int i = 0; i < 6; i++)
                                            {
                                                if(ua.Contains((Strings.RightRotateShift(key.ToString(), i)))) {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (!found) ua.Add(key);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                QueuedConsole.WriteImmediate("The the number of circular 6-permutations of the multiset : {0}", ua.Count);
            }

            /// <summary>
            /// https://brilliant.org/problems/modular-subsets/
            /// </summary>
            internal static void ModularSubsets()
            {
                int nMod0 = 1024;
                BigInteger tSubSets = new BigInteger(nMod0);
                Func<CustomIterenumerator<int>, int?> customEnum = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex == 0) { return 1; }
                    else if (ci.CurrentIndex == 1) { return 2; }
                    else return null;
                };

                Func<CustomIterenumerator<int>, bool> validator = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex >= 0 && ci.CurrentIndex <= 1) return true;
                    else return false;
                };

                CustomIterenumerator<int> iterator = new CustomIterenumerator<int>(customEnum, validator);

                for (int i = 1; i <= 10; i++)
                {
                    QueuedConsole.WriteImmediate("Solving for i: {0}", i);
                    UniqueArrangements<int> uip = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation2.Solve(3 * i, iterator, true);
                    List<List<int>> sets = uip.ExtractAsSets();
                    foreach (List<int> set in sets)
                    {
                        int n2 = 0; int n1 = 0;
                        set.ForEach(x => { if (x == 2) n2++; else if (x == 1) n1++; });
                        if (n1 > 10 || n2 > 10) continue;

                        tSubSets += PermutationsAndCombinations.nCr(10, n1) * PermutationsAndCombinations.nCr(10, n2) * nMod0;
                    }
                    QueuedConsole.WriteImmediate("Done Solving for i: {0}. Total subsets so far: {1}", i, tSubSets.ToString());
                }

                QueuedConsole.WriteFinalAnswer("Number of subsets with sum of elements having 0 mod 3 : {0}", tSubSets.ToString());
            }
        }

        internal class Partitions
        {
            internal void Solve()
            {
                MultiplicativePartitionsofANumber();
            }

            /// <summary>
            /// https://brilliant.org/problems/two-kinds-of-partitions/
            /// </summary>
            void TwoKindsOfPartitions()
            {
                long p12 = 0;
                int N = 12;
                Action<int, int> Add = null;
                Add = delegate (int prev, int sum)
                {
                    for (int i = prev; i <= N; i++)
                    {
                        int sNext = sum + i;
                        if (sNext == N)
                        {
                            p12++;
                            continue;
                        }
                        if (sNext > N) return;

                        Add(i, sNext);
                    }
                };

                Add(1, 0);

                long q12 = 0, _2N = 2 * N;
                Action<int, int, List<int>> Add1 = null;
                Add1 = delegate (int prev, int sum, List<int> used)
                {
                    for (int i = prev; i <= _2N; i++)
                    {
                        List<int> next = new List<int>(used);
                        next.Add(i);

                        int sNext = sum + i;
                        if (next.Count() == N)
                        {
                            if (sNext == _2N)
                            {
                                q12++;
                            }
                            continue;
                        }
                        if (sNext > _2N) return;

                        Add1(i, sNext, next);
                    }
                };

                Add1(1, 0, new List<int>());
                QueuedConsole.WriteImmediate("p12: {0}, q12: {1}, p12-q12:{2}", p12, q12, p12 - q12);
            }

            /// <summary>
            /// https://brilliant.org/problems/limit-inferior-of-difference-of-consecutive-primes/
            /// </summary>
            void MultiplicativePartitionsofANumber()
            {
                long count = 0;
                int N = 6, N7 = 1, N2 = 7, N5 = 7;
                Action<int, int, int, int, List<int>> Add = null;
                Add = delegate (int depth, int sum7, int sum2, int sum5, List<int> used2)
                {
                    for (int i = 0; i <= N7; i++)
                    {
                        int s7 = sum7 + i;
                        if (s7 > N7) continue;
                        for (int j = 0; j <= N2; j++)
                        {
                            int s2 = sum2 + j;
                            if (s2 > N2) continue;

                            List<int> next = new List<int>(used2);
                            next.Add(j);
                            for (int k = 0; k <= N5; k++)
                            {
                                int s5 = sum5 + k;
                                if (s5 > N5) continue;
                                if (next.Count == 6)
                                {
                                    if (s2 == N2 && s5 == N5 && s7 == N7) count++;
                                    continue;
                                } else if (depth <= N) Add(depth + 1, s7, s2, s5, next);
                            }
                        }
                    }
                };

                Add(0, 0, 0, 0, new List<int>());
                QueuedConsole.WriteImmediate("Number of ways: {0}", count);
            }
        }

        //Keep this lowest in view.
        string IProblemName.GetName()
        {
            return "Combinatorics - Collection of Problems";
        }
    }
}