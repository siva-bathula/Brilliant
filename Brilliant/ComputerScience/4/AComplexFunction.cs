using GenericDefs.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// The first 50 natural numbers are written on a board. You apply the following operation 49 times, until you arrive at one final number:
    ///Select any two numbers from the board, a and b.
    ///Erase those two numbers, and replace them with |a-b|.
    ///Determine the sum of all possible values for the final value remaining number on the board.
    /// </summary>
    public class AComplexFunction : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/a-complex-function/");
        }
        
        void ISolve.Solve()
        {
            Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p)
            {
                for (int i = 1; i <= 50; i++)
                {
                    p.Push(i);
                }
                return p;
            };


            Pool<int> pool = new Pool<int>(initializer);
            int takes = 0, a = 0, b = 0;


            List<int> remValues = new List<int>();
            while (takes <= 100000)
            {
                pool.ReInitialize();
                takes++;

                while (!pool.IsEmpty())
                {
                    a = pool.Pop();
                    if (!pool.IsEmpty())
                    {
                        b = pool.Pop();
                        pool.Push(Math.Abs(a - b));
                    }
                    else {
                        if (!remValues.Contains(a)) {
                            remValues.Add(a);
                        }
                    }
                }
            }

            foreach (int r in remValues) {
                Console.WriteLine("remaining : {0}", r);
            }

            Console.WriteLine("Sum :: {0}", remValues.Sum(x => x));
            Console.ReadKey();
        }
    }
}