using System;
using System.Numerics;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// https://brilliant.org/wiki/graph-theory/
    /// An AVL tree is a balanced binary search tree where every node in the tree satisfies the following property:
    /// The height difference between its left and right children is at most 1.
    /// </summary>
    public class AVLTreeNodes : ISolve, IBrilliant
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
            Collection.AVLTreeNodesMinimumNodes();
        }

        internal static class Collection
        {
            /// <summary>
            /// https://brilliant.org/problems/a-computer-science-problem-by-thaddeus-abiy/"
            /// Let be the N(h) minimum number of nodes in an AVL tree of height h.
            /// Find N(152363) mod 2^12.
            /// N(h) = 1 + N(h−1) + N(h−2)
            /// </summary>
            internal static void AVLTreeNodesMod2pow12()
            {
                int N = 152363, iterator = 1;
                BigInteger nim2 = 0, nim1 = 1, ni = 0;
                while (iterator <= N)
                {
                    iterator++;
                    ni = nim1 + nim2 + 1;
                    nim2 = nim1;
                    nim1 = ni;
                }

                Console.WriteLine("Minimum nodes in AVL tree for height {0} is :: {1}", N, ni);
                Console.WriteLine("Ni mod 2^12 is :: {0}", ni % 4096);
                Console.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/a-computer-science-problem-by-thaddeus-abiy/"
            /// </summary>
            internal static void AVLTreeNodesMinimumNodes()
            {
                int N = 11, iterator = 1;
                long nim2 = 0, nim1 = 1, ni = 0;
                while (iterator <= N)
                {
                    iterator++;
                    ni = nim1 + nim2 + 1;
                    nim2 = nim1;
                    nim1 = ni;
                    Console.WriteLine("Minimum nodes in AVL tree for height {0} is :: {1}", iterator, ni);
                }

                Console.ReadKey();
            }
        }
    }
}