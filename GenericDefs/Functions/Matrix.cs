using System;
using System.Numerics;

namespace GenericDefs.Functions
{
    public class Matrix
    {
        public static int[,] Multiply(int[,] a, int[,] b)
        {
            int[,] c;
            if (a.GetLength(1) == b.GetLength(0))
            {
                c = new int[a.GetLength(0), b.GetLength(1)];
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    for (int j = 0; j < c.GetLength(1); j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < a.GetLength(1); k++) // OR k<b.GetLength(0)
                            c[i, j] = c[i, j] + a[i, k] * b[k, j];
                    }
                }

                return c;
            }
            else
            {
                throw new Exception("\n Number of columns in First Matrix should be equal to Number of rows in Second Matrix.");
            }
        }

        public static BigInteger[,] Multiply(BigInteger[,] a, BigInteger[,] b)
        {
            BigInteger[,] c;
            if (a.GetLength(1) == b.GetLength(0))
            {
                c = new BigInteger[a.GetLength(0), b.GetLength(1)];
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    for (int j = 0; j < c.GetLength(1); j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < a.GetLength(1); k++) // OR k<b.GetLength(0)
                            c[i, j] = c[i, j] + a[i, k] * b[k, j];
                    }
                }

                return c;
            }
            else
            {
                throw new Exception("\n Number of columns in First Matrix should be equal to Number of rows in Second Matrix.");
            }
        }

        public static int[,] Pow(int[,] a, int n)
        {
            if (n <= 1)
            {
                throw new Exception("Can only raise to powers greater than 1.");
            }
            int[,] c = Multiply(a, a);
            int nPow = 2;
            while (1 > 0)
            {
                c = Multiply(a, c);
                if (++nPow == n) break;
            }

            return c;
        }

        public static BigInteger[,] Pow(BigInteger[,] a, int n)
        {
            if (n <= 1)
            {
                throw new Exception("Can only raise to powers greater than 1.");
            }
            BigInteger[,] c = Multiply(a, a);
            int nPow = 2;
            while (1 > 0)
            {
                c = Multiply(a, c);
                if (++nPow == n) break;
            }

            return c;
        }
    }
}
