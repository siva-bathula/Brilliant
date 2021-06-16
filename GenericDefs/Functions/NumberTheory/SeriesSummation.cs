using GenericDefs.Classes.NumberTypes;
using Numerics;
using System.Numerics;

namespace GenericDefs.Functions.NumberTheory
{
    public class SeriesSummation
    {
        /// <summary>
        /// Will do summation for 1/n, Sum = 1/1 + 1/2 + 1/3 + ...... + 1/n-1 + 1/n.
        /// </summary>
        /// <returns></returns>
        public static double _1ByN(int N)
        {
            BigRational r = new BigRational(0, 1);
            if (N < 1) throw new System.Exception("N cannot be less than 1.");
            int n = 1;
            while (true)
            {
                r += new BigRational(1, n);
                if (n == N) break;
                n++;
            }

            return NumberConverter.BigRationalToDouble(r);
        }

        /// <summary>
        /// Will do summation for n, Sum = 1 + 2 + 3 + ...... + n-1 + n.
        /// </summary>
        /// <returns></returns>
        public static long SumN(long N)
        {
            return N * (N + 1) / 2;
        }

        /// <summary>
        /// Will do summation for n, Sum = 1 + 2 + 3 + ...... + n-1 + n.
        /// </summary>
        /// <returns></returns>
        public static BigInteger SumN(BigInteger N)
        {
            return N * (N + 1) / 2;
        }

        public static BigInteger SumNCube(long N)
        {
            BigInteger retVal = SumN(new BigInteger(N));
            retVal *= retVal;
            return retVal;
        }
    }
}
