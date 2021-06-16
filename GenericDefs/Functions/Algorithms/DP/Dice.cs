using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class Dice
    {
        /// <summary>
        /// Find number of ways of rolling n dice of m faces numbered 1 to m together to make a sum-face of all dice
        /// after rolling as x.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int FindWays(int x, int n, int m)
        {
            int[,] table = new int[n + 1, x + 1];

            for (int j = 1; j <= m && j <= x; j++)
            {
                table[1, j] = 1;
            }

            for (int i = 2; i <= n; i++)
            {
                for (int j = 1; j <= x; j++)
                {
                    for (int k = 1; k <= m && k < j; k++)
                    {
                        table[i, j] += table[i - 1, j - k];
                    }
                }
            }

            return table[n, x];
        }
    }
}