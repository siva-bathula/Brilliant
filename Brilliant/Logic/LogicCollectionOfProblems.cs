using GenericDefs.DotNet;
using GenericDefs.Classes;
using GenericDefs.Functions;
using GenericDefs.Functions.Geometry;
using GenericDefs.Classes.Quirky;

using System.Collections.Generic;
using System.Numerics;
using System;
using System.Collections;
using GenericDefs.Classes.Logic;
using System.Diagnostics;
using System.Linq;

namespace Brilliant.Logic
{
    public class LogicCollectionOfProblems : ISolve, IBrilliant
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
            (new Collection1()).Solve();
        }

        internal class Collection1
        {
            internal void Solve()
            {
                TrickingTheMagician();
            }

            /// <summary>
            /// https://brilliant.org/problems/tricking-the-magician/
            /// </summary>
            void TrickingTheMagician()
            {
                for (int a = 1; a <= 9; a++)
                {
                    for (int b = 1; b <= 9; b++)
                    {
                        if (a == b) continue;
                        if (MathFunctions.GCD(a, b) == 1)
                        {
                            for(int c = 1; c<=9; c++)
                            {
                                if (a == c || b == c) continue;
                                int[] abc = new int[] { a, b, c };
                                if(MathFunctions.GCD(abc) == 1)
                                {
                                    int squareAreas = (a * a) + (b * b) + (c * c);
                                    int squarelengthSum = a + b + c;
                                    if(squareAreas % squarelengthSum == 0)
                                    {
                                        int height = squareAreas / squarelengthSum;
                                        if(!abc.Contains(height))
                                            QueuedConsole.WriteImmediate("a: {0}, b: {1}, c: {2}, a+b+c: {3}, height: {4}", a, b, c, a + b + c, squareAreas / squarelengthSum);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/you-do-not-need-to-know-the-house-number/
            /// </summary>
            void YouDontNeedToKnowTheHouseNumber()
            {
                Dictionary<int, List<Tuple<int, int>>> Digits = new Dictionary<int, List<Tuple<int, int>>>();

                for(int a = 1; a <= 9; a++)
                {
                    for (int b = 1; b <= 9; b++)
                    {
                        if (a == b) continue;

                        int key = (a * b) % 10;
                        Tuple<int, int> tuple = new Tuple<int, int>(a, b);
                        if (!Digits.ContainsKey(key)) Digits.Add(key, new List<Tuple<int, int>>() { tuple });
                        else Digits[key].Add(tuple);
                    }
                }

                foreach(KeyValuePair<int, List<Tuple<int, int>>> kvp in Digits)
                {
                    QueuedConsole.WriteImmediate("{0}", Environment.NewLine);
                    QueuedConsole.WriteImmediate("Last digit : {0}", kvp.Key);
                    foreach (Tuple<int, int> tuple in kvp.Value)
                    {
                        QueuedConsole.WriteImmediate("{0}-{1}", tuple.Item1, tuple.Item2);
                    }
                    QueuedConsole.PressAnyKeyToContinue();
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/the-diciest-challenge/
            /// </summary>
            void DiciestChallenge()
            {
                Dictionary<int, int> Standard = new Dictionary<int, int>();
                Standard.Add(2, 1); Standard.Add(3, 2); Standard.Add(4, 3); Standard.Add(5, 4); Standard.Add(6, 5); Standard.Add(7, 6);
                Standard.Add(8, 5); Standard.Add(9, 4); Standard.Add(10, 3); Standard.Add(11, 2); Standard.Add(12, 1);
                Random r = new Random();
                string standardDice = "123456";
                
                while (1 > 0)
                {
                    List<int> dice1 = new List<int>() { 1 };
                    List<int> dice2 = new List<int>() { 1 };

                    while (dice1.Count < 6) dice1.Add(r.Next(1, 9));
                    while (dice2.Count < 6) dice2.Add(r.Next(1, 9));

                    dice1.Sort(); dice2.Sort();
                    string dice1S = string.Join("", dice1);
                    if (dice1S.Equals(standardDice)) continue;

                    if (dice1.Max() + dice2.Max() == 12)
                    {
                        Dictionary<int, int> DiceSum = new Dictionary<int, int>();
                        dice1.ForEach(d1 =>
                        {
                            dice2.ForEach(d2 =>
                            {
                                DiceSum.AddOrUpdate(d1 + d2, 1);
                            });
                        });
                        bool found = true;
                        foreach(KeyValuePair<int, int> kvp in DiceSum)
                        {
                            if (!Standard.ContainsKey(kvp.Key)) found = false;
                            else if (kvp.Value != Standard[kvp.Key]) found = false;

                            if (!found) break;
                        }

                        if (found)
                        {
                            QueuedConsole.WriteImmediate("DiceA-DiceB : {0}-{1}", string.Join("", dice1), string.Join("", dice2));
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/practice/logic-warmups-level-5-challenges/?p=5
            /// How many distinct 16-digit positive integer(s) satisfy the condition that if I move its first digit to the last digit, 
            /// the resultant number increases by 50%?
            /// </summary>
            void Find16DigitNumbers()
            {
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    List<int> rhs = cr.GetCoefficients().Select(item => item).ToList();
                    rhs.Reverse();

                    List<int> lhs = rhs.Select(item => item).ToList();
                    int first = lhs[lhs.Count - 1];
                    lhs.RemoveAt(lhs.Count - 1);
                    lhs.Insert(0, first);

                    BigInteger bilhs = 3 * BigInteger.Parse(string.Join("", lhs));
                    BigInteger birhs = 2 * BigInteger.Parse(string.Join("", rhs));

                    return bilhs == birhs;
                };

                int n = 16;
                Func<List<int>, bool> SearchAccelerator = delegate (List<int> list)
                {
                    List<int> rhs = list.Select(item => item).ToList();
                    rhs.Reverse();

                    List<int> lhs = rhs.Select(item => item).ToList();
                    int first = lhs[lhs.Count - 1];

                    lhs.RemoveAt(lhs.Count - 1);

                    if (list.Count == n)
                    {
                        lhs.Insert(0, first);
                    }

                    BigInteger bilhs = BigInteger.Parse(string.Join("", lhs));
                    BigInteger birhs = BigInteger.Parse(string.Join("", rhs));

                    if (list.Count == n)
                    {
                        if (bilhs.ToString().Length < n || birhs.ToString().Length < n)
                        {
                            return false;
                        }
                    }

                    bilhs *= 3;
                    birhs *= 2;

                    int lenlhs = bilhs.ToString().Length;
                    int lenrhs = birhs.ToString().Length;
                    int len = lenlhs > lenrhs ? lenrhs : lenlhs;

                    if (list.Count < n)
                    {
                        len--;
                    }

                    string lstr = bilhs.ToString().Substring(lenlhs - len, len);
                    string rstr = birhs.ToString().Substring(lenrhs - len, len);
                    return lstr.Equals(rstr);
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < n; i++)
                {
                    int min = 0, max = 9;

                    Iterator<int> iter = new Iterator<int>(false, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                SpecialCryptRecursiveSolver solver = new SpecialCryptRecursiveSolver(true, cArithm);
                solver.IsDistinct = false;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                UniqueArrangements<int> ua = solver.GetAllSolutions(n, iterators, 0, true, SearchAccelerator);
                sw.Stop();
                QueuedConsole.WriteImmediate("time taken : " + sw.ElapsedMilliseconds);
                List<string> arrangements = ua.Extract();
                foreach (string s in arrangements)
                {
                    QueuedConsole.WriteImmediate("arrangement : " + s);
                }
                QueuedConsole.WriteFinalAnswer("Total possible numbers : " + ua.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/are-you-smarter-than-a-14-year-old/
            /// </summary>
            void AreYouSmarterThan8thGrader()
            {
                int n = 1, An = 1, Bn = 0;
                BigInteger Xn = 1;
                int nMax = 23;
                while (true)
                {
                    n++;
                    string sn = Xn.ToString();
                    string snp1 = string.Empty;
                    foreach (char c in sn.ToCharArray())
                    {
                        if (char.GetNumericValue(c) == 0) snp1 += "1";
                        else if (char.GetNumericValue(c) == 1) snp1 += "10";
                    }
                    Xn = BigInteger.Parse(snp1);
                    sn = Xn.ToString();
                    An = sn.Length;
                    Bn = (sn.ToString().Replace("01", "#")).Count(c => c == '#');
                    QueuedConsole.WriteImmediate(string.Format("A{0} : {1}, B{0} : {2}, X{0} : ", n, An, Bn, Xn.ToString()));
                    if (n == nMax) break;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("A{0} + B{0} : {1}", n, An + Bn));
            }

            /// <summary>
            /// https://brilliant.org/practice/arithmetic-puzzles-level-3-4-challenges/?subtopic=puzzles&chapter=arithmetic-puzzles
            /// </summary>
            void SimpleArithmetic()
            {
                int n = 1;
                int S = 0;
                while (true)
                {
                    string nStr = n + "";
                    int pn = 1;
                    foreach (char ch in nStr)
                    {
                        int chN = (int)char.GetNumericValue(ch);
                        if (chN == 0) continue;
                        pn *= chN;
                    }
                    S += pn;
                    n++;
                    if (n == 1000) break;
                }
                List<long> pFactors = Factors.GetPrimeFactors(S);
                pFactors.Sort();
                QueuedConsole.WriteImmediate("S = " + S);
                QueuedConsole.WriteFinalAnswer("Largest prime factor of S : " + pFactors[pFactors.Count - 1]);
            }

            /// <summary>
            /// https://brilliant.org/practice/arithmetic-puzzles-level-3-4-challenges/?subtopic=puzzles&chapter=arithmetic-puzzles
            /// </summary>
            void _28383Digit()
            {
                int n = 1;
                string nStr = string.Empty;
                while (true)
                {
                    nStr += "" + n;
                    if (nStr.Length >= 28383) break;
                    n++;
                }
                QueuedConsole.WriteFinalAnswer("28383 digit is : " + nStr[28382]);
            }

            /// <summary>
            /// https://brilliant.org/practice/arithmetic-puzzles-level-3-4-challenges/?p=4
            /// </summary>
            void ReadingBook()
            {
                Dictionary<int, List<int[]>> d = new Dictionary<int, List<int[]>>();
                for (int n = 1; n <= 1000; n++)
                {
                    for (int k = 2; k <= 30; k++)
                    {
                        int sum = k * n + (k * (k - 1) / 2);
                        if (sum == 412 || sum == 512)
                        {
                            if (d.ContainsKey(sum))
                            {
                                d[sum].Add(new int[] { n, k });
                            }
                            else
                            {
                                d.Add(sum, new List<int[]>() { new int[] { n, k } });
                            }
                        }
                    }
                }

                foreach (KeyValuePair<int, List<int[]>> kvp in d)
                {
                    QueuedConsole.WriteImmediate(string.Format("Solutions for {0}--", kvp.Key));
                    foreach (int[] t in kvp.Value)
                    {
                        QueuedConsole.WriteImmediate(string.Format("n : {0}, k: {1}", t[0], t[1]));
                    }
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/practice/arithmetic-puzzles-level-3-4-challenges/?p=5
            /// </summary>
            void f2017of2016()
            {
                int n = 0;
                int k = 2016;
                BigInteger fn = new BigInteger(0);
                while (true)
                {
                    if (n == 0) fn = k;
                    n++;
                    fn = (fn.ToString().ToCharArray()).Sum(x => (int)Math.Pow(char.GetNumericValue(x), 2));
                    QueuedConsole.WriteImmediate(string.Format("f {0} ({1}) : {2}", n, k, fn.ToString()));
                    if (n == 2017) break;
                }

                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/eleven-fingers/
            /// </summary>
            void ElevenFingers()
            {
                int[] numbers = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                int count = 0;
                int n = 9;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] num = cr.GetCoefficients();
                    int lhs = numbers[0];
                    for (int i = 0; i < n; i++)
                    {
                        if (num[i] == 0) lhs -= numbers[i + 1];
                        else if (num[i] == 1) lhs += numbers[i + 1];
                    }
                    if (lhs == 11) count++;
                    return lhs == 11;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < n; i++)
                {
                    int min = 0, max = 1;

                    Iterator<int> iter = new Iterator<int>(false, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(n, iterators, null);

                QueuedConsole.WriteFinalAnswer("Possible ways : " + count);
            }

            /// <summary>
            /// https://brilliant.org/problems/separate-them-wisely/
            /// </summary>
            void SeparateThemWisely()
            {
                int n = 1000;
                int count = 0;
                while (true)
                {
                    string nStr = "" + n;
                    if (nStr.IndexOf("0") < 0)
                    {
                        if (nStr.Distinct().Count() == 4)
                        {
                            nStr = n + "" + (n * 3);
                            if (nStr.IndexOf("0") < 0)
                            {
                                if (nStr.Distinct().Count() == 9)
                                {
                                    count++;
                                    QueuedConsole.WriteImmediate("{0}", nStr.Substring(0, 4) + "/" + nStr.Substring(4, 5));
                                }
                            }
                        }
                    }
                    n++;
                    if (n == 10000) break;
                }

                QueuedConsole.WriteImmediate("Number of ways : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/liar-liar-pants-on-fire/
            /// </summary>
            void LiarLiarPantsOnFire()
            {
                int nTruths = 5;
                int nPeople = 5;
                SimpleHashSet<int> shs = new SimpleHashSet<int>(maxLength: 10);
                Func<int, int, List<int>, int> Recursion = null;
                Recursion = delegate (int depth, int nLies, List<int> before)
                {
                    if (nLies == 0)
                    {
                        before.Sort();
                        shs.Add(before);
                        return 0;
                    }
                    else
                    {
                        for (int i = 0; i <= nPeople; i++)
                        {
                            List<int> after = new List<int>(before);
                            if (i == (nPeople - nLies)) continue;
                            after.Add(i);

                            if (depth < (nLies - 1)) Recursion(depth + 1, nLies, after);
                            else { after.Sort(); shs.Add(after); }
                        }
                        return 0;
                    }
                };

                while (true)
                {
                    List<int> before = new List<int>();
                    if (nTruths > 0) Enumerable.Range(1, nTruths).ForEach(x => before.Add(nTruths));
                    Recursion(0, nPeople - nTruths, before);

                    nTruths--;
                    if (nTruths < 0) break;
                }

                QueuedConsole.WriteImmediate("Possible multi-sets of answers : {0}", shs.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/two-numbers-and-alex-and-bill/
            /// </summary>
            void TwoNumbers()
            {
                Dictionary<int, int> solutions = new Dictionary<int, int>();
                for (int a = 100; a < 1000; a++)
                {
                    for (int b = a + 1; b < 1000; b++)
                    {
                        if (Math.Abs(a - b) == 288)
                        {
                            int[] aDigits = Numbers.GetDigitArray(a);
                            int[] bDigits = Numbers.GetDigitArray(b);
                            if (aDigits.Sum() == 12 && aDigits.Distinct().Count() == 3 && !aDigits.Contains(0))
                            {
                                if (bDigits.Sum() == 12 && bDigits.Distinct().Count() == 3 && !bDigits.Contains(0))
                                {
                                    if (aDigits.Union(bDigits).Count() == 6)
                                    {
                                        solutions.Add(a, b);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach(KeyValuePair<int, int> kvp in solutions)
                {
                    QueuedConsole.WriteImmediate("a: {0}, b: {1}, sum last digit : {2}", kvp.Key, kvp.Value, (kvp.Key + kvp.Value) % 10);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/a-lot-of-pawns/
            /// </summary>
            void ALotOfPawns()
            {
                int nMax = 16;
                Dictionary<int, List<int>> attacks = new Dictionary<int, List<int>>();
                Enumerable.Range(0, nMax).ForEach(x => {
                    if (x < 4) { attacks.Add(x, null); }
                    else if (x % 4 == 0) { attacks.Add(x, new List<int>() { x - 3 }); }
                    else if (x % 4 == 3) { attacks.Add(x, new List<int>() { x - 5 }); }
                    else { attacks.Add(x, new List<int>() { x - 3, x - 5 }); }
                });

                Dictionary<int, int> PossibleWays = new Dictionary<int, int>();
                int[] xi = new int[nMax];

                Func<int, bool> Recursion = null;
                Recursion = delegate (int depth)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        xi[depth] = i;
                        if (depth < nMax - 1) Recursion(depth + 1);
                        else {
                            int s = xi.Sum();
                            if (s >= 8) {
                                bool aoru = true;
                                for (int x = 0; x < nMax; x++) {
                                    if (xi[x] == 1 && attacks[x] != null) {
                                        foreach (int y in attacks[x]) { if (xi[y] == 1) { aoru = false; break; } }
                                    }
                                }
                                if (aoru) PossibleWays.AddOrUpdate(s, 1);
                            }
                        }
                    }
                    return true;
                };

                Recursion(0);

                foreach(KeyValuePair<int, int> kvp in PossibleWays)
                {
                    QueuedConsole.WriteImmediate("Pawns : {0}, Ways: {1}", kvp.Key, kvp.Value);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/not-square-not-square-square/
            /// </summary>
            void NotSquareNotSquareSquare()
            {
                int count = 0;
                int n = 4;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] num = cr.GetCoefficients();
                    int p = cr.ExtractValue("0,1,2,0,1,2");
                    int q = int.Parse(cr.ExtractValue("3") + "00" + cr.ExtractValue("3"));
                    int pplusq = p + q;
                    if (MathFunctions.IsSquare(pplusq)) {
                        count++;
                        if (MathFunctions.IsSquare((long)Math.Sqrt(pplusq))) {
                            return true;
                        }
                    }
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                int min = 0, max = 9;
                for (int i = 0; i < n; i++)
                {
                    if (i == 0 || i == 3) min = 1;
                    else min = 0;
                    Iterator<int> iter = new Iterator<int>(false, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(n, iterators, null);
                QueuedConsole.WriteFinalAnswer("count: {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/placing-numbers-in-a-square-1/
            /// </summary>
            void PlacingNumbersInSquare1()
            {
                int[] numbers = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                int n = 12;
                int sum = 25;
                string face1 = "0,1,2,3", face2 = "3,4,5,6", face3 = "6,7,8,9", face4 = "9,10,11,0";
                HashSet<string> sumSet = new HashSet<string>();
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    if(cr.ExtractValueSum(face1) == sum)
                    {
                        sumSet.Add(cr.ExtractValueAsString(face1));
                        if (cr.ExtractValueSum(face2) == sum)
                        {
                            sumSet.Add(cr.ExtractValueAsString(face2));
                            if (cr.ExtractValueSum(face3) == sum)
                            {
                                sumSet.Add(cr.ExtractValueAsString(face3));
                                if (cr.ExtractValueSum(face4) == sum)
                                {
                                    sumSet.Add(cr.ExtractValueAsString(face4));
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < n; i++)
                {
                    int min = 1, max = 12;

                    Iterator<int> iter = new Iterator<int>(false, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(n, iterators, null);

                List<List<int>> sets = ua.ExtractAsSets();
                if (sets.Count > 0)
                {
                    QueuedConsole.WriteImmediate("Possible solutions : {0}", sets.Count);
                    foreach (List<int> set in sets)
                    {
                        QueuedConsole.WriteImmediate("Possible corner sum : {0}", set[0] + set[3] + set[6] + set[9]);
                    }
                } else { QueuedConsole.WriteImmediate("No solution found"); }
            }
            
            /// <summary>
            /// https://brilliant.org/problems/placing-numbers-in-a-square-2-2/
            /// </summary>
            void PlacingNumbersInSquare2()
            {
                int n = 16;
                int[] numbers = new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                
                int MaxSum = int.MinValue;
                string face1 = "0,1,2,3", face2 = "3,4,5,6", face3 = "6,7,8,9", face4 = "9,10,11,0", face5 = "12,13,14,15";
                HashSet<string> sumSet = new HashSet<string>();
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int t1 = cr.ExtractValueSum(face1);
                    int t2 = cr.ExtractValueSum(face2);
                    if(t1 == t2)
                    {
                        if(cr.ExtractValueSum(face3) == t1)
                        {
                            if (cr.ExtractValueSum(face4) == t1)
                            {
                                if (cr.ExtractValueSum(face5) == t1)
                                {
                                    MaxSum = Math.Max(MaxSum, t1);
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < n; i++)
                {
                    int min = 1, max = n;

                    Iterator<int> iter = new Iterator<int>(false, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = max, Min = min });
                    iterators.Add(iter);
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                UniqueArrangements<int> ua = solver.GetAllSolutions(n, iterators, null);

                List<List<int>> sets = ua.ExtractAsSets();
                if (sets.Count > 0)
                {
                    QueuedConsole.WriteImmediate("Possible solutions : {0}", sets.Count);
                    foreach (List<int> set in sets)
                    {
                        QueuedConsole.WriteImmediate("Possible corner sum : {0}", (new CryptCoefficients(set.ToArray())).ExtractValueSum(face5));
                    }
                }
                else { QueuedConsole.WriteImmediate("No solution found"); }
            }

            /// <summary>
            /// https://brilliant.org/problems/seven-little-men-2/
            /// </summary>
            void SevenLittleMen()
            {
                int n = 7;

                Cryptarithm cArithm = new Cryptarithm(true);
                BlackList<int> bl = new BlackList<int>();
                for (int i = 0; i < n; i++)
                {
                    if (i == 0 || i == 3) bl.Add(i, new int[] { 0, 3 });
                    if (i == 1 || i == 4) bl.Add(i, new int[] { 1, 4 });
                    if (i == 2) bl.Add(i, new int[] { 2 });
                    if (i == 5) bl.Add(i, new int[] { 5 });
                    if (i == 6) bl.Add(i, new int[] { 6 });
                }

                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                UniqueArrangements<int> ua = new UniqueArrangements<int>();
                ua = solver.GetAllSolutions(n, bl);
                QueuedConsole.WriteFinalAnswer(string.Format("Different ways to stand : {0}", ua.GetCount()));
            }

            /// <summary>
            /// https://brilliant.org/problems/is-it-a-4-times-4-magic-square/
            /// </summary>
            void MagicSquares4x4()
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

                HashSet<int> NSet = new HashSet<int>();
                
                int n = 4;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    List<long> LineNumbers = new List<long>();

                    foreach (int[] row in rows)
                    {
                        LineNumbers.Add(cr.CoefficientSum(row));
                    }
                    foreach (int[] col in cols)
                    {
                        LineNumbers.Add(cr.CoefficientSum(col));
                    }

                    if (LineNumbers.Count == 8)
                    {
                        long ngcd = MathFunctions.GCD(LineNumbers);
                        if (ngcd >= 2) NSet.Add((int)ngcd);
                    }

                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<int> Init = new List<int>();
                Func<List<int>> initializer = delegate ()
                {
                    if (Init.Count == 0)
                    {
                        Enumerable.Range(1, n * n).ForEach(x =>{ Init.Add(x); });
                    }

                    return Init.ToList();
                };

                RandomGeneratorSimulator r = new RandomGeneratorSimulator(initializer, cArithm);
                r.Simulate(10000000);

                QueuedConsole.WriteImmediate("Possible n values: {0}, Sum of all n : {1}", NSet.Count, NSet.Sum());
            }
            
            /// <summary>
            /// https://brilliant.org/problems/playing-with-dominoes/
            /// </summary>
            void PlayingWithDominoes()
            {
                Func<CryptRule, bool> Verify = delegate (CryptRule rule)
                {
                    int[] input = rule.GetCoefficients();
                    int val = (input[0] + 13) * input[1];
                    if (val % 3 == 0)
                    {
                        val /= 3;
                        val += input[2] + 12;
                        val *= input[3];
                        val += -input[4] - 11 + 7;
                        val *= input[5];
                        if (val % 9 == 0)
                        {
                            val /= 9;
                            val -= 10;
                            if (val == 66)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(Verify) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                BlackList<int> bl = new BlackList<int>();
                Enumerable.Range(0, 6).ForEach(x => bl.Add(x, new int[] { 3, 7, 9 }));

                CryptRecursiveSolver crs = new CryptRecursiveSolver(true, cArithm);
                UniqueArrangements<int> ua = crs.GetAllSolutions(6, bl);

                string answer = string.Join("", ua.ExtractAsSets()[0]);
                answer = answer.Insert(2, "3").Insert(6, "7");
                answer = answer + "9";
                QueuedConsole.WriteImmediate("Answer: {0}",  answer);
            }
        }
    }
}