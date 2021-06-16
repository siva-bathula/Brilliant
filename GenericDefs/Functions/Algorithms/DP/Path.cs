using System.Collections.Generic;
using System.Numerics;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class Path
    {
        /// <summary>
        /// Count paths in n x n grids.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int CountPaths(int n)
        {
            return Recursive(n, 1, 1);
        }

        internal static int Recursive(int n, int i, int j)
        {
            //border - only 1 path
            if (i == n || j == n)
            {
                return 1;
            }
            return Recursive(n, i + 1, j) + Recursive(n, i, j + 1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">n value in the n x n grid.</param>
        /// <param name="obstacles">comma separated x,y obstacle</param>
        /// <returns></returns>
        public static BigInteger CountPathsWithObstacles(int n, HashSet<string> obstacles)
        {
            return RecursiveWithObstacles(n, 1, 1, obstacles);
        }

        internal static BigInteger RecursiveWithObstacles(int n, int i, int j, HashSet<string> obstacles)
        {
            //hit obstacles.
            if (obstacles.Contains(i + "," + j)) return 0;
            //border - only 1 path
            if (i == n && j == n)
            {
                return 1;
            }
            else if (i == n && j < n)
            {
                return RecursiveWithObstacles(n, i, j + 1, obstacles);
            }
            else if (j == n && i < n)
            {
                return RecursiveWithObstacles(n, i + 1, j, obstacles);
            }
            return RecursiveWithObstacles(n, i + 1, j, obstacles) + RecursiveWithObstacles(n, i, j + 1, obstacles);
        }
    }
}