using System;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// https://brilliant.org/problems/just-a-simple-josephus-problem/
    /// https://brilliant.org/problems/escape-satan-and-save-mathematics-2/
    /// </summary>
    public class JosephusProblem : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/just-a-simple-josephus-problem/");
        }

        void Solve1()
        {
            long N1 = 489;
            long N = N1, pleft = 0;
            int satanNum = 13;
            GenericDefs.Classes.Collections.CircularLinkedList<long> p = new GenericDefs.Classes.Collections.CircularLinkedList<long>();

            for (long i = 1; i <= N; i++)
            {
                //if (i % 2 == 0) continue;
                p.AddLast(i);
                pleft++;
            }

            GenericDefs.Classes.Collections.LinkedListNode<long> node = null;
            while (pleft > 1)
            {
                int moves = 0;
                do
                {
                    moves++;
                    if (node == null)
                    {
                        node = p.First;
                    }
                    else
                    {
                        if (node.Next == null)
                        {
                            break;
                        }
                        node = node.Next;
                    }
                    if (moves == satanNum - 1) { break; }
                } while (moves < satanNum);
                long pRemoved = node.Next.Value;
                p.Remove(node.Next);
                pleft--;
                Console.WriteLine("Person removed is :: {0}, Count :: {1}", pRemoved, pleft);
                if (pleft == 1) { break; }
            }

            Console.WriteLine("Remaining people is :: {0}", pleft);
            Console.WriteLine("Winning number is :: {0}", p.First.Value);
            Console.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/modified-josephus-problem/
        /// </summary>
        void Solve2()
        {
            GenericDefs.DotNet.QueuedConsole.WriteImmediate("{0}", GenericDefs.Functions.Logic.Josephus.Solve(1159687, 1, 2));
        }

        void ISolve.Solve()
        {
            Solve2();
        }
    }
}