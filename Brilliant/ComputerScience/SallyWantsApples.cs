using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using GenericDefs.Classes;

namespace Brilliant.ComputerScience
{
    public class SallyWantsApples : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/sally-wants-all-apples/");
        }

        void ISolve.Solve()
        {
            (new _2()).Solve();
        }

        internal class _1
        {
            /// <summary>
            /// https://brilliant.org/problems/sally-wants-all-apples/
            /// </summary>
            void Part1()
            {
                nGrid = 4;
            }

            void Part2()
            {
                DiagonalMoveAllowed = true;
                nGrid = 5;
            }

            internal void Solve()
            {
                Part2();
                doublenGrid = nGrid * 1.0;
                nGridSq = nGrid * nGrid;
                Matrix = new int[nGrid, nGrid];
                for (int i = 0; i < nGrid; i++) { for (int j = 0; j < nGrid; j++) { Matrix[i, j] = 0; } }
                Matrix[nGrid / 2, nGrid / 2] = 1;
                Recursion(Matrix, (1 + (nGrid * nGrid)) / 2, 0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", count);
            }

            private static int count = 0;
            private static int[,] Matrix = null;
            private static int nGrid;
            private static int nGridSq;
            private static double doublenGrid;
            private static bool DiagonalMoveAllowed = false;

            private static void Recursion(int[,] mx, int cell, int step)
            {
                if (step < nGridSq - 1)
                {
                    List<int> neighbours = Neighbours(cell, Matrix);
                    foreach (int x in neighbours)
                    {
                        int[] coords = NIndexToCoordinates(x);
                        mx[coords[1], coords[0]] = 1;

                        Recursion(mx, x, step + 1);
                        mx[coords[1], coords[0]] = 0;
                    }
                }
                else count++;
            }

            private static List<int> Neighbours(int n, int[,] mx)
            {
                List<int> neighbours = new List<int>();
                int[] coords = null;
                if ((Math.Floor((n - 1) / doublenGrid) == (Math.Floor((n - nGrid - 1) / doublenGrid) + 1)))
                { //top one
                    coords = NIndexToCoordinates(n - nGrid);
                    if (IsValidGridNumber(n - nGrid))
                    {
                        if (mx[coords[1], coords[0]] == 0)
                        {
                            neighbours.Add(n - nGrid);
                        }
                    }
                }
                if (DiagonalMoveAllowed)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        if (IsValidGridNumber(n - nGrid + i) && Math.Floor((n - 1) / doublenGrid) == (Math.Floor((n - nGrid + i - 1) / doublenGrid) + 1))
                        {
                            coords = NIndexToCoordinates(n - nGrid + i);
                            if (mx[coords[1], coords[0]] == 0)
                            {
                                neighbours.Add(n - nGrid + i);
                            }
                        }
                    }
                }
                for (int i = -1; i <= 1; i += 2)
                {  //middle 2
                    if (IsValidGridNumber(n + i) && Math.Floor((n - 1) / doublenGrid) == Math.Floor((n + i - 1) / doublenGrid))
                    {
                        coords = NIndexToCoordinates(n + i);
                        if (mx[coords[1], coords[0]] == 0)
                        {
                            neighbours.Add(n + i);
                        }
                    }
                }
                if (Math.Floor((n - 1) / doublenGrid) == Math.Floor((n + nGrid - 1) / doublenGrid) - 1)
                { //bottom one
                    coords = NIndexToCoordinates(n + nGrid);
                    if (IsValidGridNumber(n + nGrid))
                    {
                        if (mx[coords[1], coords[0]] == 0)
                        {
                            neighbours.Add(n + nGrid);
                        }
                    }
                }
                if (DiagonalMoveAllowed)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        if (IsValidGridNumber(n + nGrid + i) && Math.Floor((n - 1) / doublenGrid) == (Math.Floor((n + nGrid + i - 1) / doublenGrid) + 1))
                        {
                            coords = NIndexToCoordinates(n + nGrid + i);
                            if (mx[coords[1], coords[0]] == 0)
                            {
                                neighbours.Add(n + nGrid + i);
                            }
                        }
                    }
                }
                return neighbours;
            }

            private static bool IsValidGridNumber(int gridNum)
            {
                return gridNum >= 1 && gridNum <= nGridSq;
            }

            private static int[] NIndexToCoordinates(int n)
            { //convert numerical index to coordinates. Index is 1 and [0, 0].
                int y = (int)Math.Floor((n - 1) / (doublenGrid));
                return new int[] { (n - nGrid * y - 1), y }; //x, y. x, y = [0, 0] at top left
            }
        }

        internal class _2
        {
            Dictionary<int, List<int>> SquareMoves { get; set; }
            bool IsPart2 { get; set; }
            void Init()
            {
                SquareMoves = new Dictionary<int, List<int>>();
                Enumerable.Range(0, 25).ForEach(x => {
                    SquareMoves.Add(x, new List<int>());
                    int xMod5 = x % 5;
                    if(xMod5 == 0) {
                        SquareMoves[x].Add(x + 1);
                        if (x / 5 < 4) {
                            SquareMoves[x].Add(x + 5);
                            if (IsPart2) { SquareMoves[x].Add(x + 6); }
                        }
                        if (x / 5 > 0) {
                            SquareMoves[x].Add(x - 5);
                            if (IsPart2) { SquareMoves[x].Add(x - 4); }
                        }
                    } else if(xMod5 == 1) {
                        SquareMoves[x].Add(x + 1);
                        SquareMoves[x].Add(x - 1);
                        if ((x - 1) / 5 < 4) {
                            SquareMoves[x].Add(x + 5);
                            if (IsPart2) { SquareMoves[x].Add(x + 4); SquareMoves[x].Add(x + 6); }
                        }
                        if ((x - 1) / 5 > 0) {
                            SquareMoves[x].Add(x - 5);
                            if (IsPart2) { SquareMoves[x].Add(x - 4); SquareMoves[x].Add(x - 6); }
                        }
                    } else if (xMod5 == 2) {
                        SquareMoves[x].Add(x + 1);
                        SquareMoves[x].Add(x - 1);
                        if ((x - 2) / 5 < 4) {
                            SquareMoves[x].Add(x + 5);
                            if (IsPart2) { SquareMoves[x].Add(x + 4); SquareMoves[x].Add(x + 6); }
                        }
                        if ((x - 2) / 5 > 0) {
                            SquareMoves[x].Add(x - 5);
                            if (IsPart2) { SquareMoves[x].Add(x - 4); SquareMoves[x].Add(x - 6); }
                        }
                    } else if (xMod5 == 3) {
                        SquareMoves[x].Add(x + 1);
                        SquareMoves[x].Add(x - 1);
                        if ((x - 3) / 5 < 4) {
                            SquareMoves[x].Add(x + 5);
                            if (IsPart2) { SquareMoves[x].Add(x + 4); SquareMoves[x].Add(x + 6); }
                        }
                        if ((x - 3) / 5 > 0) {
                            SquareMoves[x].Add(x - 5);
                            if (IsPart2) { SquareMoves[x].Add(x - 4); SquareMoves[x].Add(x - 6); }
                        }
                    } else if (xMod5 == 4) {
                        SquareMoves[x].Add(x - 1);
                        if ((x - 4) / 5 < 4) {
                            SquareMoves[x].Add(x + 5);
                            if (IsPart2) { SquareMoves[x].Add(x + 4); }
                        }
                        if ((x - 4) / 5 > 0) {
                            SquareMoves[x].Add(x - 5);
                            if (IsPart2) { SquareMoves[x].Add(x - 6); }
                        }
                    }
                });
            }

            internal void Solve()
            {
                Part2();
            }

            void Part1()
            {
                IsPart2 = false;
                Init();
                Start();
            }

            void Part2()
            {
                IsPart2 = true;
                Init();
                Start();
            }

            void Start()
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int[] apples = new int[25];
                int startSquare = 12;
                Enumerable.Range(0, 25).ForEach(x => { apples[x] = 1; });
                apples[startSquare] = 0;
                SimpleCounter counter = new SimpleCounter();
                Move(startSquare, apples, 0, counter);
                sw.Stop();
                QueuedConsole.WriteImmediate("Time taken : {0} milli sec. ~ {1} sec.", sw.ElapsedMilliseconds, sw.ElapsedMilliseconds * 1.0 / 1000);
                QueuedConsole.WriteImmediate("Number of ways : {0}", counter.GetCount());
            }

            void Move(int curSquare, int[] apples, int applesEaten, SimpleCounter counter)
            {
                int[] after = apples.ToArray();
                int applesAfter = applesEaten + 1;
                if (applesAfter == 25) { counter.Increment(); return; }
                after[curSquare] = 0;
                foreach(int sq in SquareMoves[curSquare])
                {
                    if (apples[sq] == 1) Move(sq, after, applesAfter, counter);
                }
            }
        }
    }
}