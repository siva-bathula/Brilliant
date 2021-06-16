using GenericDefs.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    public class TipTheScales : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/tip-the-scales/");
        }

        void ISolve.Solve()
        {
            List<int> weights = new List<int>() { 1, 3, 9, 27 };

            Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (p.RangedPool)
                    {
                        if (weights[i] >= p.MinValue && weights[i] <= p.MaxValue) p.Push(weights[i]);
                    }
                }
                return p;
            };

            Pool<int> pool = new Pool<int>(initializer);

            int lhs = 0, rhs = 0;
            int totalSumOfRHS = 0;
            for (int i = 1; i <= 40; i++)
            {
                lhs = i;
                if (weights.Contains(i))
                {
                    totalSumOfRHS += i;
                    continue;
                }
                var lessThani = weights.TakeWhile(p => p < i);
                var greaterThani = weights.SkipWhile(p => p < i);
                if (greaterThani.Count() > 0)
                {
                    pool.RangedPool = true;
                    pool.MinValue = lessThani.First();
                    if (lessThani.Sum(X => X) >= i)
                    {
                        pool.MaxValue = weights.TakeWhile(p => p < lessThani.Last()).Last();
                        rhs = lessThani.Last();
                        Calibrate(pool, ref lhs, ref rhs);
                        totalSumOfRHS += rhs;
                    }
                    else
                    {
                        pool.MaxValue = lessThani.Last();
                        rhs = greaterThani.First();
                        Calibrate(pool, ref lhs, ref rhs);
                        totalSumOfRHS += rhs;
                    }
                }
                else
                {
                    rhs = lessThani.Last();
                    pool.RangedPool = true;
                    pool.MinValue = lessThani.First();
                    pool.MaxValue = weights.TakeWhile(p => p < lessThani.Last()).Last();
                    Calibrate(pool, ref lhs, ref rhs);
                    totalSumOfRHS += rhs;
                }
            }

            Console.WriteLine("Total sum :: {0}", totalSumOfRHS);
            Console.ReadKey();
        }

        void Calibrate(Pool<int> p, ref int lhs, ref int rhs)
        {
            int lhs1 = lhs, rhs1 = rhs;
            while (lhs1 != rhs1)
            {
                if (!p.IsEmpty())
                {
                    if (lhs1 < rhs1)
                    {
                        lhs1 += p.Pop();
                    }
                    else {
                        rhs1 += p.Pop();
                    }
                }
                else
                {
                    p.ReInitialize();
                    lhs1 = lhs;
                    rhs1 = rhs;
                }
            }

            lhs = lhs1;
            rhs = rhs1;
        }
    }
}