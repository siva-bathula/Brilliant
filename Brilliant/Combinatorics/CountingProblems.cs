using GenericDefs.Classes;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brilliant.Combinatorics
{
    public class CountingProblems : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            //example
            thisProblem = new Brilliant("https://brilliant.org/problems/inspired-by-chess-board/");
        }

        void ISolve.Solve()
        {
            RookPlacements();
        }

        /// <summary>
        /// Count right angled triangles on chess board.
        /// https://brilliant.org/problems/inspired-by-chess-board/
        /// </summary>
        internal static void CountingRightAngledTriangles()
        {
            //////////////////////////////////////////////////////////
            /////// For this problem->
            /////// Assumes vertices on edges of squares and not at centre of square.
            /////// Do not consider the right angles formed by diagonals.
            //////////////////////////////////////////////////////////
            Dictionary<int, int> rows = new Dictionary<int, int>();
            Dictionary<int, int> cols = new Dictionary<int, int>();
            List<int> squares = new List<int>();
            int n = 9;
            for (int i = 1; i <= n * n; i++)
            {
                squares.Add(i);
                rows.Add(i, (i - 1) / n);
                cols.Add(i, (i - 1) % n);
            }

            Dictionary<int, int> lrDiagonals = new Dictionary<int, int>();
            Dictionary<int, int> rlDiagonals = new Dictionary<int, int>();

            for (int i = 1; i <= n * n; i++)
            {
                if (!lrDiagonals.ContainsKey(i)) {
                    lrDiagonals.Add(i, i);
                    int j = i;
                    while (true)
                    {
                        j += n - 1;
                        if (j > n * n) break;
                        if (rows[j] > rows[i] && cols[j] < cols[i]) lrDiagonals.Add(j, i);
                        else break;
                    }
                }

                if (!rlDiagonals.ContainsKey(i))  {
                    rlDiagonals.Add(i, i);
                    int j = i;
                    while (true)
                    {
                        j += n + 1;
                        if (j > n * n) break;
                        if (cols[j] > cols[i] && rows[j] > rows[i]) rlDiagonals.Add(j, i);
                        else break;
                    }
                }
            }

            List<IList<int>> combinations = Combinations.GetAllCombinations(squares, 3);
            QueuedConsole.WriteImmediate("Total possible combinations : " + combinations.Count);
            UniqueArrangements<int> ua = new UniqueArrangements<int>();
            foreach (IList<int> c in combinations)
            {
                int r1 = rows[c[0]], c1 = cols[c[0]];
                int r2 = rows[c[1]], c2 = cols[c[1]];
                int r3 = rows[c[2]], c3 = cols[c[2]];
                int count = 0;
                if (r1 == r2) count++;
                if (r2 == r3) count++;
                if (r1 == r3) count++;
                if (count == 1) {
                    count = 0;
                    if (c1 == c2) count++;
                    if (c2 == c3) count++;
                    if (c1 == c3) count++;
                    if (count == 1) {
                        List<int> cAdd = c.ToList();
                        cAdd.Sort();
                        ua.Add(cAdd);
                    }
                }
                else {
                    int ld1 = lrDiagonals[c[0]], rd1 = rlDiagonals[c[0]];
                    int ld2 = lrDiagonals[c[1]], rd2 = rlDiagonals[c[1]];
                    int ld3 = lrDiagonals[c[2]], rd3 = rlDiagonals[c[2]];
                    count = 0;
                    if (ld1 == ld2) count++;
                    if (ld2 == ld3) count++;
                    if (ld1 == ld3) count++;

                    if (count == 1) {
                        count = 0;
                        if (rd1 == rd2) count++;
                        if (rd2 == rd3) count++;
                        if (rd1 == rd3) count++;
                        if (count == 1) {
                            List<int> cAdd = c.ToList();
                            cAdd.Sort();
                            //ua.Add(cAdd);
                        }
                    }

                }
            }
            QueuedConsole.WriteFinalAnswer("Number of right angled triangles in an {0} x {0} board : {1}", n, ua.GetCount());
        }

        /// <summary>
        /// https://brilliant.org/problems/exhausting-numbers/
        /// </summary>
        internal static void ExhaustingNumbers()
        {
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                if (cr.ExtractValue("0,1,2,3") % 11 == 0) return true;
                else return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 4, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(4, iterators);

            QueuedConsole.WriteFinalAnswer("number of such four digit numbers divisible by 11 : {0}", ua.GetCount());
        }

        /// <summary>
        /// https://brilliant.org/problems/wtf/
        /// </summary>
        internal static void CountingShoes()
        {
            string word = "abcdefghijkl";
            List<char> unused = new List<char>();
            word.ForEach(x => { unused.Add(x); unused.Add(x); });

            List<IList<char>> combinations = Combinations.GetAllCombinations(unused, 4);
            int cT = 0, cP = 0;
            foreach (IList<char> c in combinations)
            {
                cT++;
                bool found = false;
                char[] sch = c.Distinct().ToArray();
                foreach (char ch in sch)
                {
                    int t = 0;
                    foreach (char ch1 in c)
                    {
                        if (ch1.Equals(ch)) t++;
                        if (t == 2)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                if (found) cP++;
            }

            Fraction<long> frac = new Fraction<long>(cP, cT, false);
            QueuedConsole.WriteFinalAnswer(string.Format("Probability : a/b: {0}/{1}, a + b : {2}", frac.N, frac.D, frac.N + frac.D));
        }

        /// <summary>
        /// https://brilliant.org/problems/good-sequences/
        /// </summary>
        internal static void GoodSequences()
        {
            SimpleCounter goodSequences = new SimpleCounter();
            for (int a = 1; a <= 5; a++)
            {
                for (int b = 1; b <= 5; b++)
                {
                    for (int c = 1; c <= 5; c++)
                    {
                        for (int d = 1; d <= 5; d++)
                        {
                            for (int e = 1; e <= 5; e++)
                            {
                                List<int> seq = new List<int>() { a, b, c, d, e };
                                List<int> set = (new SortedSet<int>(seq)).ToList();

                                bool c1 = true;
                                for (int i = set.Count - 1; i >= 0; i--) {
                                    if (set[i] == 1) continue;
                                    if (!seq.Contains(set[i] - 1)) { c1 = false; break; }
                                }
                                if (c1) {
                                    bool c2 = true;
                                    string seqStr = string.Join("#", seq);
                                    for (int j = 0; j < set.Count - 1; j++)
                                    {
                                        if (seqStr.IndexOf("" + set[j]) >= seqStr.LastIndexOf("" + (set[j] + 1))) { c2 = false; break; }
                                    }
                                    if (c2) {
                                        goodSequences.Increment();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            QueuedConsole.WriteImmediate("Number of good sequences of length 5 : {0}", (goodSequences.GetCount()).ToString());
        }

        /// <summary>
        /// https://brilliant.org/problems/elven-mystical-numbers/
        /// </summary>
        internal static void ElvenMysticalNumbers1()
        {
            int count = 0;
            int nDigits = 11;
            int MaxDepth = (nDigits - 1) / 2;

            Action<int, int, List<int>> Finder = null;
            Finder = delegate (int depth, int maxN, List<int> numbers) {
                for (int t = numbers.Count > 0 ? numbers[numbers.Count - 1] : 1; t < maxN; t++)
                {
                    List<int> after = new List<int>(numbers);
                    after.Add(t);
                    if(depth == MaxDepth - 1) {
                        count++;
                        continue;
                    } else { Finder(depth + 1, maxN, after); }
                }
            };

            Enumerable.Range(2, 8).ForEach(x => {
                Finder(0, x, new List<int>());
            });
            QueuedConsole.WriteImmediate("Number of EMN's : {0}", count);
        }

        /// <summary>
        /// https://brilliant.org/problems/getting-to-1653/
        /// </summary>
        internal static void GettingTo1653()
        {
            int MaxSteps = 15;
            int LeastSteps = MaxSteps;
            int N = 1653;
            Action<int, int> Step = null;
            Step = delegate (int steps, int n)
            {
                if (steps >= MaxSteps) return;
                if (n > N) return;

                if (n == N) { LeastSteps = Math.Min(LeastSteps, steps); }

                int next = n;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0) next = 2 * n;
                    else if (i == 1) next = 3 * n;
                    else next = n + 1;

                    Step(steps + 1, next);
                }
            };

            Step(0, 0);

            QueuedConsole.WriteImmediate("Least steps : {0}", LeastSteps);
        }

        /// <summary>
        /// https://brilliant.org/problems/arrangement-of-counters/
        /// </summary>
        internal static void RookPlacements()
        {
            int nWays = 0;
            int N = 8;

            Dictionary<string, List<Tuple<int, int>>> ThreatenedSquares = new Dictionary<string, List<Tuple<int, int>>>();
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    string key = row + ":" + col;
                    ThreatenedSquares.Add(key, new List<Tuple<int, int>>());
                    List<Tuple<int, int>> list = ThreatenedSquares[key];
                    for (int a = 0; a < N; a++)
                    {
                        if (a != col) list.Add(new Tuple<int, int>(row, a));
                        if (a != row) list.Add(new Tuple<int, int>(a, col));
                    }
                }
            }

            Func<int, int, int[,], int[,]> GetFreeSquares = delegate (int row, int col, int[,] squares)
            {
                int[,] retArr = (int[,])squares.Clone();
                for (int i = 0; i < N; i++)
                {
                    if (retArr[row, i] == 0) retArr[row, i] = -1;
                    if (retArr[i, col] == 0) retArr[i, col] = -1;

                    ThreatenedSquares[row + ":" + col].ForEach(s => { retArr[s.Item1, s.Item2] = -1; });
                }

                return retArr;
            };

            Action<int, int[,]> PlaceRook = null;
            PlaceRook = delegate (int depth, int[,] freesquares)
            {
                int row = depth;
                for (int i = 0; i < N; i++)
                {
                    if (freesquares[row, i] != 0) continue;

                    int[,] next = GetFreeSquares(row, i, freesquares);
                    next[row, i] = 1;
                    if (depth == N - 1)
                    {
                        nWays++;
                        continue;
                    }
                    else { PlaceRook(depth + 1, next); }
                }
            };

            PlaceRook(0, new int[N, N]);
            QueuedConsole.WriteImmediate("Number of ways: {0}", nWays);
        }
    }
}