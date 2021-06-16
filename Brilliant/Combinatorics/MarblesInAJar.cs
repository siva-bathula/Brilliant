using GenericDefs.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics
{
    /// <summary>
    /// You have a large jar containing 75 black and 75 white marbles. You also have access to an arbitrarily large amount of marbles that are not in the jar. 
    /// You begin removing marbles from the jar according to the following rules.
    ///Select 2 marbles from inside the jar at random.
    /// If the chosen marbles are both white, remove them both and put a black marble from outside the jar into the jar.
    /// If at least one of the chosen marbles is black, remove a black marble and put the remaining marble back into the jar.
    /// You repeat this process until only 1 marble remains in the jar. What color is it?
    /// </summary>
    public class MarblesInAJar : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/marbles-in-a-jar/");
        }
        
        // 0 = white, 1 = black.
        void ISolve.Solve()
        {
            Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p) {
                for (int i = 0; i < 75; i++)
                {
                    p.Push(0); p.Push(1);
                }
                return p;
            };

            Pool<int> pool = new Pool<int>(initializer);
            int takes = 0;
            while (takes <= 100) {
                pool.ReInitialize();
                takes++;
                int marble1 = -1, marble2 = -1, lastMarble = -1;
                while (!pool.IsEmpty()) {
                    marble1 = pool.Pop();
                    lastMarble = marble1;

                    if (!pool.IsEmpty()) {
                        marble2 = pool.Pop();
                        lastMarble = marble2;

                        if (marble1 == 0 && marble2 == 0) {
                            pool.Push(1);
                        }
                        else if (marble1 == 1 || marble2 == 1)
                        {
                            if (marble1 == 1) { pool.Push(marble2); }
                            else if (marble2 == 1) { pool.Push(marble1); }
                        }
                    }
                }
                Console.WriteLine("Take {0} , Last marble left is :: {1}", takes, lastMarble);
            }
            Console.ReadKey();
        }
    }
}