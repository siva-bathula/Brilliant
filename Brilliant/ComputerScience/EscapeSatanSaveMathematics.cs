using System;
namespace Brilliant.ComputerScience
{
    /// <summary>
    /// https://brilliant.org/problems/escape-the-even-demon/
    /// https://brilliant.org/problems/escape-satan-and-save-mathematics-2/
    /// </summary>
    public class EscapeSatanSaveMathematics : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/escape-satan-and-save-mathematics-2/");
        }

        void ISolve.Solve()
        {
            int N = (int)Math.Pow(10, 7), pleft = (int)Math.Pow(10, 7);
            int satanNum = 666;
            GenericDefs.Classes.Collections.CircularLinkedList<int> p = new GenericDefs.Classes.Collections.CircularLinkedList<int>();

            for (int i = 1; i <= N; i++)
            {
                p.AddLast(i);
            }

            GenericDefs.Classes.Collections.LinkedListNode<int> node = null;
            while (pleft > 1)
            {
                int moves = 0;
                do
                {
                    moves++;
                    if (node == null) {
                        node = p.First;
                    } else {
                        node = node.Next;
                    }
                    if (moves == satanNum - 1) { break; }
                } while (moves < satanNum);
                int pRemoved = node.Next.Value;
                p.Remove(node.Next);
                pleft--;
                Console.WriteLine("Person removed is :: {0}, Count :: {1}", pRemoved, pleft);
                if (pleft == 1) { break; }
            }

            Console.WriteLine("Remaining people is :: {0}", pleft);
            Console.WriteLine("Winning number is :: {0}", p.First.Value);
            Console.ReadKey();
        }
    }
}