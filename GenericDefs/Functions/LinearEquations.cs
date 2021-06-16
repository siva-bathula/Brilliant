using Numerics;
using GenericDefs.DotNet;
using System.Linq;

namespace GenericDefs.Functions
{
    public class LinearEquations
    {
        /// <summary>
        /// Solve simultaneous N x N system of equations.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static double[] Solve(double[,] X, double[] Y)
        {
            double[] YBefore = Y.ToArray();

            int i, j, k, k1, N;
            N = Y.Length;
            for (k = 0; k < N; k++)
            {
                k1 = k + 1;
                for (i = k; i < N; i++)
                {
                    if (X[i, k] != 0)
                    {
                        for (j = k1; j < N; j++)
                        {
                            X[i, j] /= X[i, k];
                        }
                        Y[i] /= X[i, k];
                    }
                }
                for (i = k1; i < N; i++)
                {
                    if (X[i, k] != 0)
                    {
                        for (j = k1; j < N; j++)
                        {
                            X[i, j] -= X[k, j];
                        }
                        Y[i] -= Y[k];
                    }
                }
            }
            for (i = N - 2; i >= 0; i--)
            {
                for (j = N - 1; j >= i + 1; j--)
                {
                    Y[i] -= X[i, j] * Y[j];
                }
            }
            int count = 0;
            Enumerable.Range(0, YBefore.Count()).ForEach(e =>
            {
                if (YBefore[e] == Y[e]) count++;
            });

            if (count == YBefore.Count())
            {
                throw new System.Exception("Solution doesn't exist for this system of equations.");
            }

            return Y;
        }

        /// <summary>
        /// Solve simultaneous N x N system of equations.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static BigRational[] Solve(BigRational[,] X, BigRational[] Y)
        {
            int i, j, k, k1, N;
            N = Y.Length;
            try
            {
                for (k = 0; k < N; k++)
                {
                    k1 = k + 1;
                    for (i = k; i < N; i++)
                    {
                        if (X[i, k] != 0)
                        {
                            for (j = k1; j < N; j++)
                            {
                                X[i, j] /= X[i, k];
                            }
                            Y[i] /= X[i, k];
                        }
                    }
                    for (i = k1; i < N; i++)
                    {
                        if (X[i, k] != 0)
                        {
                            for (j = k1; j < N; j++)
                            {
                                X[i, j] -= X[k, j];
                            }
                            Y[i] -= Y[k];
                        }
                    }
                }
                for (i = N - 2; i >= 0; i--)
                {
                    for (j = N - 1; j >= i + 1; j--)
                    {
                        Y[i] -= X[i, j] * Y[j];
                    }
                }
            } catch(System.Exception ex) {
                QueuedConsole.WriteImmediate("Dumping exception.");
                QueuedConsole.WriteImmediate("Message : {0}", ex.Message);
                QueuedConsole.WriteImmediate("Message : {1}", ex.StackTrace);
            }
            return Y;
        }
    }
}