using GenericDefs.Classes.Collections;
namespace GenericDefs.Functions.Logic
{
    public class Josephus
    {
        /// <summary>
        /// The Josephus problem solver. https://brilliant.org/problems/just-a-simple-josephus-problem/
        /// </summary>
        /// <param name="N">The initial count of persons.</param>
        /// <param name="sIndex">0 if people are seated starting 0, or 1 if people are seated starting 1.</param>
        /// <param name="satanNum">The nth person who will be ejected from the current iterator.</param>
        public static long Solve(int N, int sIndex, int satanNum)
        {
            long N1 = N - (1 - sIndex), pleft = 0;
            CircularLinkedList<long> p = new CircularLinkedList<long>();

            for (long i = sIndex; i <= N1; i++)
            {
                p.AddLast(i);
                pleft++;
            }

            LinkedListNode<long> node = p.First.Previous;
            while (pleft > 1)
            {
                int moves = 0;
                do
                {
                    if (moves == satanNum - 1) { break; }
                    moves++;
                    
                    if (node.Next == null) { break; }
                    node = node.Next;
                } while (moves < satanNum);
                long pRemoved = node.Next.Value;
                p.Remove(node.Next);
                pleft--;
                if (pleft == 1) { break; }
            }

            return p.First.Value;
        }
    }
}
