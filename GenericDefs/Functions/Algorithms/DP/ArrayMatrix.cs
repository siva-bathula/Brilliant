using System;
using System.Collections.Generic;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class ArrayMatrix
    {
        /// <summary>
        /// Find Maximum rectangular sub-matrix of 0's.
        /// http://stuckinaninfiniteloop.blogspot.in/2011/09/finding-maximum-zero-submatrix-with-c.html
        /// </summary>
        /// <param name="matrix"></param>
        public static void MaxRectangularSubmatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0); // Number of rows
            int m = matrix.GetLength(1); // Number of columns

            int maxArea = -1, tempArea = -1;

            // Top-left corner (x1, y1); bottom-right corner (x2, y2)
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            // Maximum row containing a 1 in this column
            int[] d = new int[m];

            // Initialize array to -1
            for (int i = 0; i < m; i++)
            {
                d[i] = -1;
            }

            // Furthest left column for rectangle
            int[] d1 = new int[m];

            // Furthest right column for rectangle
            int[] d2 = new int[m];

            Stack<int> stack = new Stack<int>();

            // Work down from top row, searching for largest rectangle
            for (int i = 0; i < n; i++)
            {
                // 1. Determine previous row to contain a '1'
                for (int j = 0; j < m; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        d[j] = i;
                    }
                }

                stack.Clear();

                // 2. Determine the left border positions
                for (int j = 0; j < m; j++)
                {
                    while (stack.Count > 0 && d[stack.Peek()] <= d[j])
                    {
                        stack.Pop();
                    }

                    // If stack is empty, use -1; i.e. all the way to the left
                    d1[j] = (stack.Count == 0) ? -1 : stack.Peek();

                    stack.Push(j);
                }

                stack.Clear();

                // 3. Determine the right border positions
                for (int j = m - 1; j >= 0; j--)
                {
                    while (stack.Count > 0 && d[stack.Peek()] <= d[j])
                    {
                        stack.Pop();
                    }

                    d2[j] = (stack.Count == 0) ? m : stack.Peek();

                    stack.Push(j);
                }

                // 4. See if we've found a new maximum submatrix
                for (int j = 0; j < m; j++)
                {
                    // (i - d[j]) := current row - last row in this column to contain a 1
                    // (d2[j] - d1[j] - 1) := right border - left border - 1
                    tempArea = (i - d[j]) * (d2[j] - d1[j] - 1);

                    if (tempArea > maxArea)
                    {
                        maxArea = tempArea;

                        // Top left
                        x1 = d1[j] + 1;
                        y1 = d[j] + 1;

                        // Bottom right
                        x2 = d2[j] - 1;
                        y2 = i;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine(maxArea);
            Console.WriteLine(String.Format("({0}, {1}) to ({2}, {3})", x1, y1, x2, y2));

            Console.WriteLine();
        }

        /// <summary>
        /// Find Maximum rectangular sub-matrix of 1's.
        /// Adapted from here - http://algorithms.tutorialhorizon.com/dynamic-programming-maximum-size-square-sub-matrix-with-all-1s/
        /// </summary>
        /// <param name="arrA"></param>
        /// <returns></returns>
        public static int MaxSquareSubMatrix(int[,] arrA)
        {
            int row = arrA.GetLength(0), cols = arrA.GetLength(1);
            int[,] sub = new int[row, cols];
            // copy the first row
            for (int i = 0; i < cols; i++)
            {
                sub[0, i] = arrA[0, i];
            }
            // copy the first column
            for (int i = 0; i < row; i++)
            {
                sub[i, 0] = arrA[i, 0];
            }

            // for rest of the matrix
            // check if arrA[i][j]==1
            for (int i = 1; i < row; i++)
            {
                for (int j = 1; j < cols; j++)
                {
                    if (arrA[i, j] == 1)
                    {
                        sub[i, j] = Math.Min(sub[i - 1, j - 1],
                                Math.Min(sub[i, j - 1], sub[i - 1, j])) + 1;
                    } else { sub[i, j] = 0; }
                }
            }
            // find the maximum size of sub matrix
            int maxSize = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (sub[i, j] > maxSize)
                    {
                        maxSize = sub[i, j];
                    }
                }
            }
            return maxSize;
        }
    }

    public class Matrix
    {
        public static int MatrixChainMultiplicationOrder(int[] p,  int j, int i= 1)
        {
            if (i == j)
                return 0;

            int min = int.MaxValue;

            // place parenthesis at different places between first
            // and last matrix, recursively calculate count of
            // multiplications for each parenthesis placement and
            // return the minimum count
            for (int k = i; k < j; k++)
            {
                int count = MatrixChainMultiplicationOrder(p, k, i) +
                            MatrixChainMultiplicationOrder(p, j, k + 1) +
                            p[i - 1] * p[k] * p[j];

                if (count < min)
                    min = count;
            }

            // Return minimum count
            return min;
        }

        public static int MatrixChainOrder(int[] p)
        {
            int n = p.Length;
            /* For simplicity of the program, one extra row and one
            extra column are allocated in m[][].  0th row and 0th
            column of m[][] are not used */
            int[,] m = new int[n, n];

            int i, j, k, L, q;

            /* m[i,j] = Minimum number of scalar multiplications needed
            to compute the matrix A[i]A[i+1]...A[j] = A[i..j] where
            dimension of A[i] is p[i-1] x p[i] */

            // cost is zero when multiplying one matrix.
            for (i = 1; i < n; i++)
                m[i, i] = 0;

            // L is chain length.
            for (L = 2; L < n; L++)
            {
                for (i = 1; i < n - L + 1; i++)
                {
                    j = i + L - 1;
                    if (j == n) continue;
                    m[i, j] = int.MaxValue;
                    for (k = i; k <= j - 1; k++)
                    {
                        // q = cost/scalar multiplications
                        q = m[i, k] + m[k + 1, j] + p[i - 1] * p[k] * p[j];
                        if (q < m[i, j])
                            m[i, j] = q;
                    }
                }
            }

            return m[1, n - 1];
        }
    }
}
