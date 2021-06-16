using GenericDefs.Classes;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using GenericDefs.Classes.NumberTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Linq;

namespace Brilliant.Combinatorics
{
    public class PathAndGridWalkingProblems : ISolve, IBrilliant, IProblemName
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
            //(new GridWalking()).Solve();
            (new ClimbingStairs()).Solve();
            //(new PathProblems()).Solve();
        }

        internal class PathProblems
        {
            internal void Solve()
            {
                FindDiamonds();
            }

            /// <summary>
            /// https://brilliant.org/problems/a-cat-walk/
            /// </summary>
            void GridWalkWithObstacles()
            {
                HashSet<string> obstacles = new HashSet<string>();
                obstacles.Add("4, 4");
                obstacles.Add("4, 7");
                obstacles.Add("7, 4");
                obstacles.Add("7, 7");
                int n = 1;
                Stopwatch sw = new Stopwatch();
                while (true)
                {
                    sw.Start();
                    BigInteger nPaths = GenericDefs.Functions.Algorithms.DP.Path.CountPathsWithObstacles(n + 1, obstacles);
                    sw.Stop();
                    double time = sw.ElapsedMilliseconds * 1.0 / 1000;
                    sw.Reset();
                    QueuedConsole.WriteImmediate(string.Format("n: {0}, number of paths : {1}, time taken : {2}", n, nPaths.ToString(), time));
                    n++;
                    if (n == 16) break;
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/sudoku-hybrid/
            /// </summary>
            void HybridSudoku()
            {
                Action<int, int, int, int, Number<int>> Move = null;
                Move = delegate (int x, int y, int xMax, int yMax, Number<int> paths)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;
                    if (x == xMax && y == 0) return;
                    if (y == yMax && x == 0) return;
                    if (x == xMax && y == yMax) { paths.Value += 1; return; }
                    Move(x + 1, y, xMax, yMax, paths);
                    Move(x, y + 1, xMax, yMax, paths);
                };
                Number<int> ForwardPaths = new Number<int>(0);
                Move(0, 0, 4, 4, ForwardPaths);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/edsger-hates-swim/
            /// </summary>
            void EdsgerDogstra()
            {
                var stream = Utility.GetEmbeddedResourceStream("Brilliant.Combinatorics.data.edsgerdogstra.txt", true);
                HashSet<string> obstacles = new HashSet<string>();
                using (StreamReader sr = new StreamReader(stream))
                {
                    string s = "";
                    int row = 1;
                    while ((s = sr.ReadLine()) != null)
                    {
                        int col = 1;
                        foreach (char ch in s)
                        {
                            if (ch == '#') obstacles.Add(row + "," + col);
                            col++;
                        }
                        row++;
                    }
                }

                Stopwatch sw = new Stopwatch();
                int n = 1;
                while (true)
                {
                    if (obstacles.Contains(n + "," + n))
                    {
                        QueuedConsole.WriteImmediate(string.Format("{0},{0} is an obstacle. Skipping this.", n));
                    }
                    else
                    {
                        sw.Start();
                        BigInteger nPaths = GenericDefs.Functions.Algorithms.DP.Path.CountPathsWithObstacles(n, obstacles);
                        sw.Stop();
                        double time = sw.ElapsedMilliseconds * 1.0 / 1000;
                        sw.Reset();
                        QueuedConsole.WriteImmediate(string.Format("n: {0}, number of paths : {1}, time taken : {2}", n, nPaths.ToString(), time));
                    }
                    n++;
                    if (n > 25) break;
                }
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/heist-of-the-diamond-2/
            /// </summary>
            void FindDiamonds()
            {
                Action<int, int, int, int, Number<int>> Move = null;
                Move = delegate (int x, int y, int xMax, int yMax,Number<int> paths)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;
                    if (x == xMax && y == yMax) { paths.Value += 1; return; }
                    Move(x + 1, y, xMax, yMax, paths);
                    Move(x, y + 1, xMax, yMax, paths);
                };
                Number<int> ForwardPaths = new Number<int>(0);
                Move(0, 0, 3, 3, ForwardPaths);
                Number<int> ReturnPaths = new Number<int>(0);
                Move(0, 0, 3, 1, ReturnPaths);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value * ReturnPaths.Value);
            }
        }

        internal class GridWalking
        {
            internal void Solve()
            {
                //(new GridWalkCountWord()).Solve();

                GridWalking1();                
            }

            /// <summary>
            /// https://brilliant.org/problems/grid-walking-1/
            /// </summary>
            void GridWalking1()
            {
                SimpleCounter c = new SimpleCounter();
                int xMax = 10, yMax = 7;
                Action<int, int, Tuple<int, int>, Tuple<int, int>, List<string>> Move = null;
                Move = delegate (int curX, int curY, Tuple<int, int> prev, Tuple<int, int> sbprev, List<string> moves)
                {
                    List<string> nmoves = new List<string>(moves);
                    nmoves.Add(curX + ":" + curY);
                    if (curX > xMax) return;
                    if (curY > yMax) return;
                    else if (curX == xMax && curY == yMax) {
                        c.Increment();
                        return;
                    }
                    else
                    {
                        Tuple<int, int> nPrev = new Tuple<int, int>(curX, curY);
                        Move(curX + 1, curY, nPrev, prev, nmoves);
                        if (prev == null) Move(curX, curY + 1, nPrev, prev, nmoves);
                        if (prev != null)
                        {
                            if (prev.Item1 != nPrev.Item1)
                            {
                                Move(curX, curY + 1, nPrev, prev, nmoves);
                            }
                        }
                    }
                    return;
                };

                Move(0, 0, null, null, new List<string>());
                QueuedConsole.WriteImmediate("Number of ways : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/climbing-the-staircase/
            /// </summary>
            void ClimbingStaircase()
            {
                SimpleCounter c = new SimpleCounter();
                int xMax = 5, yMax = 5;
                Action<int, int> Move = null;
                Move = delegate (int curX, int curY)
                {
                    if (curX > xMax) return;
                    if (curY > yMax) return;
                    if (curX + 1 - curY < 0) return;
                    else if (curX == xMax && curY == yMax) { c.Increment(); return; }
                    else
                    {
                        Move(curX + 1, curY);
                        Move(curX, curY + 1);
                    }
                    return;
                };

                Move(0, 0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/do-u-like-to-play-chess-wont-like-after-seeing/
            /// </summary>
            void InsectsMeeting()
            {
                Action<int, int, int, int, int> Move = null;
                int xMax = 9, yMax = 9;
                Number<int> Paths = new Number<int>(0);
                Move = delegate (int xA, int yA, int xB, int yB, int moves)
                {
                    if (xA > xMax) return;
                    if (yA > yMax) return;
                    if (xB < 0) return;
                    if (yB < 0) return;

                    if (xA == xB && yA == yB)
                    {
                        Paths.Value += 1;
                        return;
                    }

                    if (moves >= 12) return;

                    Move(xA + 1, yA, xB - 1, yB, moves + 1);
                    Move(xA, yA + 1, xB - 1, yB, moves + 1);
                    Move(xA + 1, yA, xB, yB - 1, moves + 1);
                    Move(xA, yA + 1, xB, yB - 1, moves + 1);
                };
                Move(1, 1, 9, 9, 0);
                QueuedConsole.WriteImmediate("Number of ways of meeting : {0}", Paths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/that-creepy-extra-area-under-the-staris/
            /// </summary>
            void ThatCreepyExtraAreaUnderTheStairs()
            {
                Action<int, int, int, int, int> Move = null;
                int xMax = 5, yMax = 10;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y, int xprev, int yprev, int area)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;
                    
                    int areaN = area;
                    areaN += (x - xprev) * y;

                    if (x == xMax && y == yMax)
                    {
                        if(areaN % 5 ==1) ForwardPaths.Value += 1;
                        return;
                    }

                    Move(x + 1, y, x, y, areaN);
                    Move(x, y + 1, x, y, areaN);
                };
                Move(0, 0, 0, 0, 0);
                QueuedConsole.WriteImmediate("Number of paths : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/cross-twice-or-more/
            /// </summary>
            void CrossTwiceOrMore()
            {
                Action<int, int, int, int, int> Move = null;
                int xMax = 10, yMax = 10;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y, int nCrossed, int xprev, int yprev)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;

                    if (x == xMax && y == yMax)
                    {
                        if (nCrossed >= 2)
                        {
                            ForwardPaths.Value += 1;
                        }
                        return;
                    }

                    if (x == y && x > 0)
                    {
                        if (xprev < yprev)
                        {
                            Move(x + 1, y, nCrossed + 1, x, y);
                            Move(x, y + 1, nCrossed, x, y);
                        }
                        else if (xprev > yprev)
                        {
                            Move(x + 1, y, nCrossed, x, y);
                            Move(x, y + 1, nCrossed + 1, x, y);
                        }
                    }
                    else
                    {
                        Move(x + 1, y, nCrossed, x, y);
                        Move(x, y + 1, nCrossed, x, y);
                    }
                };
                Move(0, 0, 0, 0, 0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/quartermeter-push-obstacles/
            /// </summary>
            void QuartermeterObstacles()
            {
                HashSet<string> Obstacles = new HashSet<string>();
                Obstacles.Add("2:2");
                Obstacles.Add("4:4");
                Obstacles.Add("7:7");

                Action<int, int, HashSet<string>> Move = null;
                int xMax = 10, yMax = 10, xmaxSq = xMax * xMax;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y, HashSet<string> obstacles)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;

                    int dSquared = (xMax - x) * (xMax - x) + (y * y);
                    if (dSquared > xmaxSq) return;

                    if (x == xMax && y == yMax)
                    {
                        ForwardPaths.Value += 1;
                        return;
                    }

                    if (x < xMax)
                    {
                        string oKey = (x + 1) + ":" + y;
                        if (obstacles.Contains(oKey))
                        {
                            HashSet<string> oNext = new HashSet<string>(obstacles);
                            if (x + 1 < xMax)
                            {
                                oNext.Remove(oKey);
                                int osq = ((xMax - x - 2) * (xMax - x - 2)) + (y * y);
                                if (osq <= xmaxSq)
                                {
                                    oNext.Add((x + 2) + ":" + y);
                                    Move(x + 1, y, oNext);
                                }
                            }
                        }
                        else Move(x + 1, y, obstacles);
                    }
                    if (y < yMax)
                    {
                        string oKey = x + ":" + (y + 1);
                        if (obstacles.Contains(oKey))
                        {
                            HashSet<string> oNext = new HashSet<string>(obstacles);
                            if (y + 1 < yMax)
                            {
                                oNext.Remove(oKey);
                                int osq = ((xMax - x) * (xMax - x)) + ((y + 2) * (y + 2));
                                if (osq <= xmaxSq)
                                {
                                    oNext.Add(x + ":" + (y + 2));
                                    Move(x, y + 1, oNext);
                                }
                            }
                        }
                        else Move(x, y + 1, obstacles);
                    }
                };
                Move(0, 0, Obstacles);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/it-is-difficult/
            /// </summary>
            void GridWalk()
            {
                SimpleCounter c = new SimpleCounter();
                int xMax = 6, yMax = 5;
                Func<int, int, bool> Move = null;
                Move = delegate (int curX, int curY) {
                    if (curX > xMax) return false;
                    if (curY > yMax) return false;
                    else if (curX == xMax && curY == yMax) { c.Increment(); return true; }
                    else {
                        Move(curX + 1, curY);
                        Move(curX, curY + 1);
                    }
                    return false;
                };

                Move(0,0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/a-4-dimensional-random-walk/
            /// </summary>
            [SearchKeyword("Incorrect")]
            void FourDimensionalGridWalk()
            {
                int count = 0;
                int countMax = 1000000;
                long tMoves = 0;
                int dMax = 6;
                int nMax = 20;
                Action<int, int, int, int, int> Move = null;
                Move = delegate (int x, int y, int z, int t, int nMoves) {
                    if (count == countMax) return;
                    if (Math.Abs(x) > dMax || Math.Abs(y) > dMax || Math.Abs(z) > dMax || Math.Abs(t) > dMax || nMoves > nMax) return;
                    if(x == 3 && y == 3 && z == 3 && t == 0) { count++; tMoves += nMoves; return; }
                    else {
                        Move(x + 1, y, z, t, nMoves + 1);
                        Move(x - 1, y, z, t, nMoves + 1);
                        Move(x, y + 1, z, t, nMoves + 1);
                        Move(x, y - 1, z, t, nMoves + 1);
                        Move(x, y, z + 1, t, nMoves + 1);
                        Move(x, y, z - 1, t, nMoves + 1);
                        Move(x, y, z, t + 1, nMoves + 1);
                        Move(x, y, z, t - 1, nMoves + 1);
                    }
                };

                Move(1, 1, 1, 0, 0);
                QueuedConsole.WriteImmediate("Max. count : {0}, Total moves : {1}", count, tMoves);
                QueuedConsole.WriteImmediate("Average moves : {0}", tMoves * 1.0 / count);
            }

            /// <summary>
            /// https://brilliant.org/problems/3d-grid-walk/
            /// </summary>
            [SearchKeyword("Incorrect")]
            void ThreeDimensionalGridWalk()
            {
                int count = 0;
                int countMax = 100000;
                long tMoves = 0;
                int dMax = 6;
                int nMax = 20;
                Action<int, int, int, int> Move = null;
                Move = delegate (int x, int y, int z, int nMoves) {
                    if (count == countMax) return;
                    if (Math.Abs(x) > dMax || Math.Abs(y) > dMax || Math.Abs(z) > dMax || nMoves > nMax) return;
                    if (x == 3 && y == 3 && z == 3) { count++; tMoves += nMoves; return; }
                    else
                    {
                        Move(x + 1, y, z, nMoves + 1);
                        Move(x - 1, y, z, nMoves + 1);
                        Move(x, y + 1, z, nMoves + 1);
                        Move(x, y - 1, z, nMoves + 1);
                        Move(x, y, z + 1, nMoves + 1);
                        Move(x, y, z - 1, nMoves + 1);
                    }
                };

                Move(1, 1, 1, 0);
                QueuedConsole.WriteImmediate("Max. count : {0}, Total moves : {1}", count, tMoves);
                QueuedConsole.WriteImmediate("Average moves : {0}", tMoves * 1.0 / count);
            }

            /// <summary>
            /// https://brilliant.org/problems/walk-through-the-grid/
            /// </summary>
            void WalkThroughTheGrid()
            {
                Action<int, int> Move = null;
                int xMax = 5, yMax = 5;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    if (x == 1 && (y == 1 || y == 4)) return;
                    if (x == 4 && (y == 1 || y == 4)) return;
                    if (x > xMax) return;
                    if (y > yMax) return;
                    if (x == xMax && y == yMax)
                    {
                        ForwardPaths.Value += 1;
                        return;
                    }
                    Move(x + 1, y);
                    Move(x, y + 1);
                };
                Move(0, 0);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/one-more-extremely-typical-grid-walk-problem/
            /// </summary>
            void TypicalGridWalkProblem()
            {
                Action<int, int> Move = null;
                int xMax = 3, yMax = 4;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;
                    if (x == xMax && y == yMax)
                    {
                        ForwardPaths.Value += 1;
                        return;
                    }
                    Move(x + 1, y);
                    Move(x, y + 1);
                    Move(x + 1, y + 1);
                };
                Move(0, 0);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/point-a-to-b/
            /// </summary>
            void PointAToB()
            {
                Action<int, int> Move = null;
                int xMax = 8, yMax = 6;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    if (x > xMax) return;
                    if (4 * y - 3 * x > 0) return;

                    if (x == xMax && y == yMax)
                    {
                        ForwardPaths.Value += 1;
                        return;
                    }
                    Move(x + 1, y);
                    Move(x, y + 1);
                };
                Move(0, 0);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/quartermeterpath/
            /// </summary>
            void PointAToBCircular()
            {
                Action<int, int> Move = null;
                int xMax = 10, yMax = 10, xmaxSq = xMax * xMax;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;

                    int dSquared = (xMax - x) * (xMax - x) + (y * y);
                    if (dSquared > xmaxSq) return;

                    if (x == xMax && y == yMax)
                    {
                        ForwardPaths.Value += 1;
                        return;
                    }
                    Move(x + 1, y);
                    Move(x, y + 1);
                };
                Move(0, 0);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/going-to-the-castle-part-5/
            /// </summary>
            void GoingToTheCastle()
            {
                Action<int, int, bool> Move = null;
                int xMax = 5, yMax = 4;
                Number<int> ForwardPaths = new Number<int>(0);
                Move = delegate (int x, int y, bool tookIceCream)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;

                    if (x == xMax && y == yMax)
                    {
                        if (tookIceCream) ForwardPaths.Value += 1;
                        return;
                    }

                    bool next = tookIceCream;

                    if ((x == 1 && y == 1) || (x == 4 && y == 3)) next = true;

                    Move(x + 1, y, next);
                    Move(x, y + 1, next);
                };
                Move(0, 0, false);

                QueuedConsole.WriteImmediate("Number of ways : {0}", ForwardPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/walking-at-random-combinatorics/
            /// </summary>
            void WalkingAtRandom()
            {
                Action<int, int, List<string>> Move = null;
                int xMax = 4, yMax = 4;
                Number<int> NPaths = new Number<int>(0);
                Move = delegate (int x, int y, List<string> moves)
                {
                    string key = x + ":" + y;

                    if (x > xMax) return;
                    if (y > yMax) return;

                    if (x == 0 && y == 0 && moves.Count >= 3)
                    {
                        NPaths.Value += 1;
                        return;
                    }
                    if (moves.Contains(key)) return;

                    List<string> next = new List<string>(moves);
                    next.Add(key);

                    if (x < xMax) Move(x + 1, y, next);

                    if (x > 0) Move(x - 1, y, next);

                    if (y < yMax) Move(x, y + 1, next);

                    if (y > 0) Move(x, y - 1, next);
                };
                Move(0, 0, new List<string>() );

                QueuedConsole.WriteImmediate("Number of paths : {0}", NPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/walking-at-random-combinatorics/
            /// </summary>
            void KingOnChessBoard()
            {
                Action<int, int> Move = null;
                int xMax = 8, yMax = 8;
                Number<int> NPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    if (x > xMax) return;
                    if (y > yMax) return;
                    if (x == xMax && y == yMax) { NPaths.Value++; return; }
                    if (x < xMax) Move(x + 1, y);
                    if (y < yMax) Move(x, y + 1);
                };
                Move(1, 1);

                QueuedConsole.WriteImmediate("Number of paths : {0}", NPaths.Value);
            }

            void WillyGoesToSchool()
            {
                HashSet<string> NotAllowed = new HashSet<string>() { "2:2", "2:3", "3:2", "3:3" };
                Action<int, int> Move = null;
                int xMax = 5, yMax = 5;
                Number<int> NPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    string key = x + ":" + y;
                    if (x > xMax) return;
                    if (y > yMax) return;
                    if (NotAllowed.Contains(key)) return;

                    if (x == xMax && y == yMax) { NPaths.Value++; return; }
                    if (x < xMax) Move(x + 1, y);
                    if (y < yMax) Move(x, y + 1);
                };
                Move(0, 0);

                QueuedConsole.WriteImmediate("Number of paths : {0}", NPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/some-grid/
            /// </summary>
            void Grid()
            {
                Action<int, int> Move = null;
                int xMax = 7, yMin = -1;
                Number<int> NPaths = new Number<int>(0);
                Move = delegate (int x, int y)
                {
                    if (x > xMax) return;
                    if (y < yMin || y > 0) return;

                    if (x == xMax && y == yMin) { NPaths.Value++; return; }
                    if (x < xMax)
                    {
                        Move(x + 1, y);
                        if (y > yMin) Move(x, y - 1);
                        if (y == yMin) Move(x + 1, y + 1);
                    } else if (x == xMax && y == 0) Move(x, y - 1);
                };
                Move(0, 0);

                QueuedConsole.WriteImmediate("Number of paths : {0}", NPaths.Value);
            }

            void Meet()
            {
                Action<int, int, HashSet<string>> Move = null;
                int xMax = 2, yMax = 3;
                Number<int> NPaths = new Number<int>(0);
                Move = delegate (int x, int y, HashSet<string> trace)
                {
                    string key = x + ":" + y;
                    if (x > xMax) return;
                    if (y > yMax || y < 0) return;
                    if (trace.Contains(key)) return;

                    if (x == xMax && y == yMax) { NPaths.Value++; return; }

                    HashSet<string> next = new HashSet<string>(trace);
                    next.Add(key);

                    if (x < xMax) Move(x + 1, y, next);
                    if (y < yMax) Move(x, y + 1, next);
                    if (y > 0) Move(x, y - 1, next);
                };
                Move(0, 0, new HashSet<string>());

                QueuedConsole.WriteImmediate("Number of paths : {0}", NPaths.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/just-write-the-word/
            /// </summary>
            internal class GridWalkCountWord
            {
                char _empty = '~';
                void Init()
                {
                    SquareMoves = new Dictionary<int, List<int>>();
                    WordGrid = new char[N * N];
                    Enumerable.Range(0, N * N).ForEach(x =>
                    {
                        SquareMoves.Add(x, new List<int>());
                        int xModN = x % N;
                        if (xModN == 0) SquareMoves[x].Add(x + 1);
                        else if (xModN == N - 1) SquareMoves[x].Add(x - 1);
                        else
                        {
                            SquareMoves[x].Add(x + 1);
                            SquareMoves[x].Add(x - 1);
                        }
                        int n = 0;
                        while (true)
                        {
                            if (xModN == n)
                            {
                                if ((x - n) / N < N - 1)
                                {
                                    SquareMoves[x].Add(x + N);
                                    if (n < N - 1) SquareMoves[x].Add(x + N + 1);
                                    if (n > 0) SquareMoves[x].Add(x + N - 1);
                                }
                                if ((x - n) / N > 0)
                                {
                                    SquareMoves[x].Add(x - N);
                                    if (n < N - 1) SquareMoves[x].Add(x - N + 1);
                                    if (n > 0) SquareMoves[x].Add(x - N - 1);
                                }
                                break;
                            }
                            ++n;
                        }

                        int rowNum = (x / N) + 1;
                        int colNum = xModN + 1;

                        if (colNum < WordLength) WordGrid[x] = ReverseWord[colNum - 1];
                        if (colNum == WordLength) WordGrid[x] = ReverseWord[WordLength - 1];
                        if (colNum > WordLength) WordGrid[x] = Word[colNum - WordLength];

                        int ct = colNum > WordLength ?  2 * WordLength - colNum: colNum;
                        int rt = rowNum > WordLength ? 2 * WordLength - rowNum : rowNum;

                        if (rowNum < WordLength && rowNum < colNum && rowNum < ct) WordGrid[x] = ReverseWord[rowNum - 1];
                        if (rowNum > WordLength && rowNum > colNum && colNum > rt) WordGrid[x] = Word[rowNum - WordLength];
                    });

                    //Enumerable.Range(0, N * N).ForEach(x =>
                    //{
                    //    QueuedConsole.Write("{0} ", WordGrid[x]);
                    //    if ((x + 1) % N == 0)
                    //    {
                    //        QueuedConsole.WriteImmediate(Environment.NewLine);
                    //        QueuedConsole.Flush();
                    //    }
                    //});

                    //Enumerable.Range(0, N * N).ForEach(x => {
                    //    QueuedConsole.WriteImmediate("{0} : {1}", x, string.Join(",", SquareMoves[x]));
                    //});
                }

                int N { get; set; }
                Dictionary<int, List<int>> SquareMoves { get; set; }
                string Word { get; set; }
                string ReverseWord;
                char[] WordGrid { get; set; }
                int WordLength { get; set; }

                internal void Solve()
                {
                    JustWriteTheWord();
                    Init();
                    Start();
                }

                /// <summary>
                /// https://brilliant.org/problems/just-write-the-word/
                /// </summary>
                void JustWriteTheWord()
                {
                    Word = "SODALG";
                    //Word = "ABCD";
                    WordLength = Word.Length;
                    ReverseWord = string.Join("", Word.Reverse());
                    N = 2 * (Word.Length - 1) + 1;
                }

                int AlternateSolution(int depth)
                {
                    if (depth == 1) return 0;
                    else return 3 * (AlternateSolution(depth - 1) - 4) + 20;
                }

                void Start()
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int startSquare = (N * N) / 2;
                    SimpleCounter counter = new SimpleCounter();
                    Move(startSquare, WordGrid, "", counter);
                    sw.Stop();
                    QueuedConsole.WriteImmediate("Time taken : {0} milli sec. ~ {1} sec.", sw.ElapsedMilliseconds, sw.ElapsedMilliseconds * 1.0 / 1000);
                    QueuedConsole.WriteImmediate("Number of ways : {0}", counter.GetCount());
                    QueuedConsole.WriteImmediate("Alternate solution with recursion.");
                    QueuedConsole.WriteImmediate("Number of ways : {0}", AlternateSolution(Word.Length));
                }

                void Move(int curSquare, char[] grid, string wordSoFar, SimpleCounter counter)
                {
                    char[] after = grid.ToArray();
                    string next = wordSoFar + after[curSquare];
                    after[curSquare] = _empty;

                    if (next.Equals(Word)) { counter.Increment(); return; }
                    if (next.Length == WordLength) return;

                    foreach (int sq in SquareMoves[curSquare])
                    {
                        if (!after[sq].Equals(_empty)) Move(sq, after, next, counter);
                    }
                }
            }
        }

        internal class ClimbingStairs
        {
            internal void Solve()
            {
                StrangeStairClimbing();
            }

            /// <summary>
            /// https://brilliant.org/problems/climbing-stairs-2/
            /// </summary>
            void ClimbingStairs2()
            {
                Func<CustomIterenumerator<int>, int?> customEnum = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex == 0) { return 1; }
                    else if (ci.CurrentIndex == 1) { return 2; }
                    else if (ci.CurrentIndex == 2) { return 3; }
                    else if (ci.CurrentIndex == 3) { return 4; }
                    else return null;
                };

                Func<CustomIterenumerator<int>, bool> validator = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex >= 0 && ci.CurrentIndex <= 3) return true;
                    else return false;
                };

                CustomIterenumerator<int> iterator = new CustomIterenumerator<int>(customEnum, validator);

                SimpleCounter u = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation2.SolveCount(20, iterator);
                QueuedConsole.WriteFinalAnswer("Count : {0}", u.GetCount().ToString());
            }

            /// <summary>
            ///https://brilliant.org/problems/strange-stair-climbing/
            /// </summary>
            void StrangeStairClimbing()
            {
                Func<CustomIterenumerator<int>, int?> customEnum = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex == 0) { return 2; }
                    else if (ci.CurrentIndex == 1) { return 3; }
                    else if (ci.CurrentIndex == 2) { return 5; }
                    else if (ci.CurrentIndex == 3) { return 7; }
                    else if (ci.CurrentIndex == 4) { return 11; }
                    else if (ci.CurrentIndex == 5) { return 13; }
                    else if (ci.CurrentIndex == 6) { return 17; }
                    else if (ci.CurrentIndex == 7) { return 19; }
                    else if (ci.CurrentIndex == 8) { return 23; }
                    else if (ci.CurrentIndex == 9) { return 29; }
                    else return null;
                };

                Func<CustomIterenumerator<int>, bool> validator = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex >= 0 && ci.CurrentIndex <= 10) return true;
                    else return false;
                };

                CustomIterenumerator<int> iterator = new CustomIterenumerator<int>(customEnum, validator);

                //SimpleCounter uip = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation2.SolveCount(30, iterator);

                UniqueArrangements<int> uip = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation2.Solve(30, iterator);
                List<List<int>> sets = uip.ExtractAsSets();
                LoggerContext lc = new LoggerContext();
                foreach (List<int> set in sets)
                {
                    lc.AppendCurrentLog(string.Format("{0}", string.Join("-> ", set)));
                    lc.AppendNewLine();
                }
                Logger.Log(lc);
                QueuedConsole.WriteFinalAnswer("Count : {0}", uip.GetCount().ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/long-legged-larry/
            /// </summary>
            void LongLeggedLarry()
            {
                Func<CustomIterenumerator<int>, int?> customEnum = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex == 0) return 2;
                    else if (ci.CurrentIndex == 1) return 3;
                    else if (ci.CurrentIndex == 2)
                    {
                        if (ci.History == null)
                        {
                            return null;
                        }
                        List<int> history = (List<int>)ci.History;

                        return history.Sum();
                    }
                    else return 0;
                };

                Func<CustomIterenumerator<int>, bool> validator = delegate (CustomIterenumerator<int> ci)
                {
                    if (ci.CurrentIndex >= 0 && ci.CurrentIndex <= 2) return true;
                    else return false;
                };

                CustomIterenumerator<int> iterator = new CustomIterenumerator<int>(customEnum, validator);

                UniqueArrangements<int> uip = GenericDefs.Functions.Algorithms.DP.Knapsack.Variation2.Solve(20, iterator);
                List<List<int>> sets = uip.ExtractAsSets();
                LoggerContext lc = new LoggerContext();
                foreach (List<int> set in sets)
                {
                    lc.AppendCurrentLog(string.Format("{0}", string.Join("-> ", set)));
                    lc.AppendNewLine();
                }
                Logger.Log(lc);
                QueuedConsole.WriteFinalAnswer(string.Format("Count : {0}", uip.GetCount()));
            }

            /// <summary>
            /// https://brilliant.org/problems/long-legged-larry/
            /// </summary>
            void LongLeggedLarry_1()
            {
                SimpleCounter c = new SimpleCounter();
                int sMax = 20;
                Action<int> Move = null;
                Move = delegate (int curX) {
                    if (curX > sMax) return;
                    else if (curX == sMax) { c.Increment(); return; }
                    else
                    {
                        Move(curX + 2);
                        Move(curX + 3);
                        if (curX > 3) Move(curX + curX);
                    }
                    return;
                };

                Move(0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", c.GetCount());
            }

            /// <summary>
            /// https://brilliant.org/problems/the-snake-and-the-ladder-2/
            /// </summary>
            void SnakeAndLadder2()
            {
                long counter = 0;
                int sMax = 35;
                Func<int, bool> Move = null;
                Move = delegate (int curX) {
                    if (curX > sMax) return false;
                    else if (curX == sMax) { counter++; return true; }
                    else
                    {
                        Move(curX + 1);
                        if (curX + 2 <= sMax) Move(curX + 2);
                        if (curX + 3 <= sMax) Move(curX + 3);
                        if (curX + 4 <= sMax) Move(curX + 4);
                        if (curX + 5 <= sMax) Move(curX + 5);
                        if (curX + 6 <= sMax) Move(curX + 6);
                    }
                    return false;
                };

                Move(0);
                QueuedConsole.WriteImmediate("Number of ways : {0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-snake-and-a-ladder/
            /// </summary>
            void SnakeAndLadder1()
            {
                long nWays = 0;
                long nThrows = 0;
                int sMax = 35;
                Action<int, int> Move = null;
                Move = delegate (int x, int d) {
                    if (x > sMax) return;
                    else if (x == sMax) { nWays++; nThrows += d; return; }
                    else {
                        Move(x + 1, d + 1);
                        if (x + 2 <= sMax) Move(x + 2, d + 1);
                        if (x + 3 <= sMax) Move(x + 3, d + 1);
                        if (x + 4 <= sMax) Move(x + 4, d + 1);
                        if (x + 5 <= sMax) Move(x + 5, d + 1);
                        if (x + 6 <= sMax) Move(x + 6, d + 1);
                    }
                    return;
                };

                Move(0, 0);
                QueuedConsole.WriteImmediate("Number of ways : {0}, Number of throws : {1}", nWays, nThrows);
                QueuedConsole.WriteImmediate("Average throws : {0}", nThrows * 1.0 / nWays);
            }
        }

        //Keep this lowest in view.
        string IProblemName.GetName()
        {
            return "Grid walking and Path Problems";
        }
    }
}