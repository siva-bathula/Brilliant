using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions;
namespace Brilliant.ComputerScience
{
    /// <summary>
    /// Tricky sums indeed! Computer Science Level 5 
    /// Let the function f(n) denote the sum of all natural numbers less than or equal to.However, this function has one trick - 
    /// if the number to be added  is a power of 2 (i.e ), then instead of adding, we subtract the number. What is sum (i = 4 to 10000) f(n)?
    /// (1) = 1, f(2) = -2, f(3) = 6 , f(4) = -4
    /// </summary>
    public class TrickySums : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/tricky-sums-indeed/");
        }

        void ISolve.Solve()
        {
            long totalSum = 0;
            List<long> sums = new List<long>();
            for (int i = 4; i <= 10000; i++) {
                long sum = 0;
                for (int j = 1; j <= i; j++) {
                    if (Numbers.IsPowerOfTwo(Convert.ToUInt64(j))) { sum = sum - j; }
                    else { sum += j; }
                }
                sums.Add(sum);
            }

            foreach (long s in sums) {
                totalSum += s;
            }
            Console.WriteLine("Total sum :: {0}", totalSum);
            Console.ReadKey();
        }
    }
}