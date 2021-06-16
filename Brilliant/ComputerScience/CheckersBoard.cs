using GenericDefs.DotNet;
using GenericDefs.Classes.NumberTypes;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;

namespace Brilliant.ComputerScience
{
    public class CheckersBoard : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant(" https://brilliant.org/problems/packed-checkers/");
        }

        void ISolve.Solve()
        {
            Part1();
        }

        Dictionary<int, List<int>> Neighbours { get; set; }
        int N { get; set; }

        void Init()
        {
            Neighbours = new Dictionary<int, List<int>>();
            Enumerable.Range(0, N * N).ForEach(x =>
            {
                Neighbours.Add(x, new List<int>());

                int xModN = x % N;
                if (xModN == 0) Neighbours[x].Add(x + 1);
                else if (xModN == N - 1) Neighbours[x].Add(x - 1);
                else
                {
                    Neighbours[x].Add(x + 1);
                    Neighbours[x].Add(x - 1);
                }

                int n = 0;
                while (true)
                {
                    if (xModN == n)
                    {
                        if ((x - n) / N < N - 1)
                        {
                            Neighbours[x].Add(x + N);
                        }
                        if ((x - n) / N > 0)
                        {
                            Neighbours[x].Add(x - N);
                        }
                        break;
                    }
                    ++n;
                }
            });
        }

        void Part1()
        {
            N = 8;
            Init();
            Start();
        }

        void Start()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int[] checkers = new int[N * N];
            Enumerable.Range(0, N * N).ForEach(x => { checkers[x] = 0; });
            Number<int> MaximumCheckers = new Number<int>(int.MinValue);
            PlaceChecker(0, checkers, MaximumCheckers);
            sw.Stop();
            QueuedConsole.WriteImmediate("Time taken : {0} milli sec. ~ {1} sec.", sw.ElapsedMilliseconds, sw.ElapsedMilliseconds * 1.0 / 1000);
            QueuedConsole.WriteImmediate("Maximum checkers : {0}", MaximumCheckers.Value);
        }

        void PlaceChecker(int curSquare, int[] checkers, Number<int> maxCheckers)
        {
            int[] after = checkers.ToArray();
            bool isPlaced = false;
            if (ValidCheckerPlacement(curSquare, checkers)) {
                after[curSquare] = 1;
                isPlaced = true;
                if (!ValidateNeighbouringCascade(curSquare, after)) { isPlaced = false; after[curSquare] = 0; }
            }

            if (curSquare == (N * N) - 1) {
                maxCheckers.Value = Math.Max(maxCheckers.Value, after.Sum());
            } else {
                PlaceChecker(curSquare + 1, after, maxCheckers);
                if(isPlaced) {
                    after[curSquare] = 0;
                    PlaceChecker(curSquare + 1, after, maxCheckers);
                }
            }
        }

        bool ValidCheckerPlacement(int curSquare, int[] checkers)
        {
            int sum = 0;
            Neighbours[curSquare].ForEach(x => { sum += checkers[x]; });
            return sum < 2;
        }

        bool ValidateSquare(int curSquare, int[] checkers)
        {
            int sum = 0;
            Neighbours[curSquare].ForEach(x => { sum += checkers[x]; });
            return sum <= 2;
        }

        bool ValidateNeighbouringCascade(int curSquare, int [] checkers)
        {
            return Neighbours[curSquare].All(x => { return ValidateSquare(x, checkers); });
        }
    }
}