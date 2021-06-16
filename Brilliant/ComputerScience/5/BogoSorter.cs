using GenericDefs.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class BogoSorter : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/the-devil/");
        }

        void ISolve.Solve()
        {
            Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p)
            {
                for (int i = 1; i <= 15; i++)
                {
                    p.Push(i);
                }
                return p;
            };

            Pool<int> pool = new Pool<int>(initializer);

            int trials = 0, maxTrials = 100000;
            BigInteger X = new BigInteger(0);
            while (true)
            {
                trials++;
                X += GenericDefs.Functions.Algorithms.Sort.BogoSorter.BogoVariation(GetNewList());
                if (trials >= maxTrials) break;
            }

            Console.WriteLine("X :: {0}, Trials : {1}", X.ToString(), trials);
            Console.WriteLine("Average shuffles :: {0}", (long)(X / trials));
            Console.ReadKey();
        }

        List<int> GetNewList() {
            Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p) {
                for (int i = 1; i <= 15; i++)
                {
                    p.Push(i);
                }
                return p;
            };

            Pool<int> pool = new Pool<int>(initializer);

            List<int> nl = new List<int>();
            while (true) {
                nl.Add(pool.Pop());
                if (pool.IsEmpty()) break;
            }

            for (int i = 0; i < 15; i++) {
                if (nl[i] == i + 1) {
                    RandomSwap(ref nl, i);
                }
            }

            return nl;
        }

        void RandomSwap(ref List<int> nl, int index) {
            Random rn = new Random();
            int rIn = rn.Next(nl.Count);
            while (rIn == index)
            {
                rIn = rn.Next(nl.Count);
                if (rIn != index) break;
            }
            int temp = nl[rIn];
            nl[rIn] = nl[index];
            nl[index] = temp;
        }
    }
}
