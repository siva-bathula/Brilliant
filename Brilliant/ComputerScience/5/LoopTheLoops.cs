using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brilliant.ComputerScience._5
{
    public class LoopTheLoops : ISolve, IBrilliant, IProblemName
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get
            {
                return thisProblem;
            }
        }

        string IProblemName.GetName()
        {
            return "Loop the Loops - Rectangular grid.";
        }

        int M { get; set; }
        int N { get; set; }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/loop-the-loops-medium-2/");
        }

        void ISolve.Solve()
        {
            Solve2();
        }

        void InitializeGrid()
        {
            if (Squares == null) Squares = new Dictionary<string, int>();
            if (Squares != null) Squares.Clear();

            int SqId = 0;
            Enumerable.Range(0, M).ForEach(row =>
            {
                Enumerable.Range(0, N).ForEach(col =>
                {
                    Squares.Add(row + ":" + col, ++SqId);
                });
            });
        }

        Dictionary<string, int> Squares { get; set; }
        char[] splitter = new char[] { ':' };
        int NLoops { get; set; }
        void EnumeratePaths()
        {
            NLoops = 0;
            Dictionary<string, string> UniqueLoops = new Dictionary<string, string>();
            HashSet<string> Loopset = new HashSet<string>();
            Action<int, int, int, List<int>> Add = null;
            Add = delegate (int start, int x, int y, List<int> used)
            {
                int sqNum = Squares[x + ":" + y];
                if (used.Contains(sqNum)) return;

                if (used.Count > 1 && sqNum == start)
                {
                    used.Add(start);
                    List<int> copy = new List<int>(used);
                    string ordered = string.Join(":", used);
                    used.Sort();
                    string sorted = string.Join(":", used);
                    if (Loopset.Add(sorted)) {
                        if (used.Count > 12) UniqueLoops.Add(sorted, ordered);
                        NLoops++;
                    } else {
                        int sqLength = used.Count;
                        if (sqLength >= 12)
                        {
                            string prev = UniqueLoops[sorted];
                            string[] oSquaresPrev = prev.Split(splitter);

                            int first = int.Parse(oSquaresPrev[0]);
                            int second = int.Parse(oSquaresPrev[1]);
                            int last = int.Parse(oSquaresPrev[sqLength - 1]);

                            int index1 = copy.IndexOf(first);
                            int index2 = copy.IndexOf(second);
                            int indexlast = copy.IndexOf(last);

                            List<int> copy1 = copy.GetRange(index1, sqLength - index1);
                            copy1.AddRange(copy.GetRange(0, sqLength - copy1.Count));

                            index1 = copy1.IndexOf(first);
                            index2 = copy1.IndexOf(second);
                            indexlast = copy1.IndexOf(last);

                            if (index2 > indexlast)
                            {
                                copy1 = copy1.GetRange(1, sqLength - 1);
                                copy1.Add(first);
                                copy1.Reverse();
                            }
                            string cur = string.Join(":", copy1);
                            if (!cur.Equals(prev) && !UniqueLoops.ContainsKey(cur))
                            {
                                NLoops++;
                                UniqueLoops.Add(cur, cur);
                            }
                        }
                    }
                    return;
                }

                List<int> next = null;
                if (sqNum == start && used.Count == 0) next = used;
                else {
                    next = new List<int>(used);
                    next.Add(sqNum);
                }

                if (x < M - 1) Add(start, x + 1, y, next);
                if (y < N - 1) Add(start, x, y + 1, next);
                if (y > 0) Add(start, x, y - 1, next);
                if (x > 0) Add(start, x - 1, y, next);
            };
            Enumerable.Range(1, M * N).ForEach(SqId =>
            {
                string key = Squares.Where(kvp => kvp.Value == SqId).First().Key;
                string[] keyVal = key.Split(splitter);
                int row = int.Parse(keyVal[0]);
                int col = int.Parse(keyVal[1]);
                Add(Squares[key], row, col, new List<int>());
            });

            Dictionary<int, int> LoopLengths = new Dictionary<int, int>();
            Loopset.ForEach(x => {
                LoopLengths.AddOrUpdate(x.Split(splitter).Count(), 1);
            });
        }

        /// <summary>
        /// https://brilliant.org/problems/loop-the-loops-easy/
        /// </summary>
        void Solve1()
        {
            M = 2;
            N = 3;
            InitializeGrid();
            EnumeratePaths();
            QueuedConsole.WriteImmediate("Loops for {0}x{1} grid : {2}", M, N, NLoops);

            M = 3;
            N = 3;
            InitializeGrid();
            EnumeratePaths();
            QueuedConsole.WriteImmediate("Loops for {0}x{1} grid : {2}", M, N, NLoops);

            M = 3;
            N = 4;
            InitializeGrid();
            EnumeratePaths();
            QueuedConsole.WriteImmediate("Loops for {0}x{1} grid : {2}", M, N, NLoops);
        }

        /// <summary>
        /// https://brilliant.org/problems/loop-the-loops-medium-2/
        /// </summary>
        void Solve2()
        {
            M = 4;
            N = 4;
            InitializeGrid();
            EnumeratePaths();
            QueuedConsole.WriteImmediate("Loops for {0}x{1} grid : {2}", M, N, NLoops);

            M = 5;
            N = 5;
            InitializeGrid();
            EnumeratePaths();
            QueuedConsole.WriteImmediate("Loops for {0}x{1} grid : {2}", M, N, NLoops);
        }
    }
}