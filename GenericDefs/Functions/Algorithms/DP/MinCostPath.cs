using System;
using System.Collections.Generic;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class MinCostPath
    {
        public static int FindCost(int[,] A)
        {
            int M = A.GetLength(0);
            int N = A.GetLength(1);
            int[,] solution = new int[M, N];

            solution[0, 0] = A[0, 0];

            // fill the first row
            for (int i = 1; i < N; i++)
            {
                solution[0, i] = A[0, i] + solution[0, i - 1];
            }

            // fill the first column
            for (int i = 1; i < M; i++)
            {
                solution[i, 0] = A[i, 0] + solution[i - 1, 0];
            }

            // path will be either from top or left, choose which ever is minimum
            for (int i = 1; i < M; i++)
            {
                for (int j = 1; j < N; j++)
                {
                    solution[i, j] = A[i, j] + Math.Min(solution[i - 1, j], solution[i, j - 1]);
                }
            }
            return solution[M - 1, N - 1];
        }

        public static List<Tuple<int, int>> FindCostAndPath(int[,] A, ref int minCost)
        {
            int M = A.GetLength(0);
            int N = A.GetLength(1);
            int[,] solution = new int[M, N];

            solution[0, 0] = A[0, 0];

            // fill the first row
            for (int i = 1; i < N; i++)
            {
                solution[0, i] = A[0, i] + solution[0, i - 1];
            }

            // fill the first column
            for (int i = 1; i < M; i++)
            {
                solution[i, 0] = A[i, 0] + solution[i - 1, 0];
            }

            // path will be either from top or left, choose which ever is minimum
            for (int i = 1; i < M; i++)
            {
                for (int j = 1; j < N; j++)
                {
                    solution[i, j] = A[i, j] + Math.Min(solution[i - 1, j], solution[i, j - 1]);
                }
            }
            minCost = solution[M - 1, N - 1];

            int xmin = minCost;
            List<Tuple<int, int>> Path = new List<Tuple<int, int>>();
            int xCur = M - 1, yCur = N - 1;
            while (xmin > 0)
            {
                Path.Add(new Tuple<int, int>(xCur, yCur));
                xmin -= A[xCur, yCur];
                if (xCur >= 1 && solution[xCur - 1, yCur] == xmin)
                {
                    xCur--;
                } else if (yCur >= 1) yCur--;
            }

            Path.Reverse();
            return Path;
        }
    }
}