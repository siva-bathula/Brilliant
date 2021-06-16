using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
namespace ProjectEuler
{
    /// <summary>
    /// Billionaire - Problem 267
    ///You are given a unique investment opportunity.
    ///Starting with £1 of capital, you can choose a fixed proportion, f, of your capital to bet on a fair coin toss repeatedly for 1000 tosses.
    ///Your return is double your bet for heads and you lose your bet for tails.
    ///For example, if f = 1 / 4, for the first toss you bet £0.25, and if heads comes up you win £0.5 and so then have £1.5.You then bet £0.375 and if the second toss is tails, you have £1.125.
    ///Choosing f to maximize your chances of having at least £1, 000, 000, 000 after 1, 000 flips, what is the chance that you become a billionaire ?
    ///All computations are assumed to be exact(no rounding), but give your answer rounded to 12 digits behind the decimal point in the form 0.abcdefghijkl.
    /// </summary>
    public class N267Billionaire : IProblem
    {
        public void Solve()
        {
            double f = 0.5;
            double alpha = 1e-4;
            double fold = 0;

            while (Math.Abs(f - fold) > 1e-4)
            {
                double fdev = (L(f + 1e-6) - L(f)) / 1e-6;
                fold = f;
                f -= alpha * fdev;
            }

            BigInteger totalComb = BigInteger.Pow(2, 1000);
            BigInteger winningComb = 0;

            for (int h = (int)Math.Ceiling(L(f)); h <= 1000; h++)
            {
                winningComb += Choose(1000, h);
            }
            
            Console.WriteLine("The chance of winning is maximized at {0}", f);
            Console.WriteLine("The chance of winning is {0}", (double)winningComb / (double)totalComb);
            Console.ReadKey();
        }

        private double L(double f)
        {
            return (9 * Math.Log(10) - 1000 * Math.Log(1 - f)) / (Math.Log(1 + 2 * f) - Math.Log(1 - f));
        }

        public static BigInteger Choose(int n, int k)
        {
            k = Math.Min(k, n - k);

            BigInteger res = 1;
            for (int i = 1; i <= k; i++)
            {
                res *= n - k + i;
                res /= i;
            }

            return res;
        }
    }
}