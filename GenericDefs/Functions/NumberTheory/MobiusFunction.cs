using GenericDefs.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.NumberTheory
{
    /// <summary>
    /// Möbius (or Moebius) function mu(n). mu(1) = 1; mu(n) = (-1)^k if n is the product of k different primes; otherwise mu(n) = 0.
    /// https://www.artofproblemsolving.com/wiki/index.php?title=Mobius_function
    /// https://oeis.org/A008683
    /// </summary>
    public class MobiusFunction
    {
        public static int GetMobiusValue(long n, ClonedPrimes cPrimes = null) {
            if (n == 1) return 1;
            var mu = 1;

            Dictionary<long, int> factors = null;
            if(cPrimes ==null) factors = Factors.GetPrimeFactorsWithMultiplicity(n);
            else factors = Factors.GetPrimeFactorsWithMultiplicity(n, cPrimes);

            bool isSquareFree = true;
            int kFactors = 0;
            foreach (KeyValuePair<long, int> kvp in factors) {
                if (kvp.Value >= 2)
                {
                    isSquareFree = false;
                    break;
                }
                kFactors++;
            }

            if (isSquareFree)
            {
                if (MathFunctions.IsOdd(kFactors)) mu = -1;
                else mu = 1;
            }
            else {
                mu = 0;
            }
            
            return mu;
        }

        public static int MobiusSumOfDivisors(long n)
        {
            ClonedPrimes cPrimes = KnownPrimes.CloneKnownPrimes(0, 10000);
            HashSet<long> set = Factors.GetAllFactors(n, cPrimes);
            int retVal = 0;
            set.ForEach(x =>
            {
                retVal += GetMobiusValue(x, cPrimes);
            });

            return retVal;
        }

        /// <summary>
        /// Gets a list of mobius values for numbers less than equal to max.
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static int[] GetFirstNMobiusValues(int N)
        {
            var sqrt = (int)Math.Floor(Math.Sqrt(N));
            var mu = new int[N + 1];
            for (int i = 1; i <= N; i++)
                mu[i] = 1;
            for (int i = 2; i <= sqrt; i++)
            {
                if (mu[i] == 1)
                {
                    for (int j = i; j <= N; j += i)
                        mu[j] *= -i;
                    for (int j = i * i; j <= N; j += i * i)
                        mu[j] = 0;
                }
            }
            for (int i = 2; i <= N; i++)
            {
                if (mu[i] == i)
                    mu[i] = 1;
                else if (mu[i] == -i)
                    mu[i] = -1;
                else if (mu[i] < 0)
                    mu[i] = 1;
                else if (mu[i] > 0)
                    mu[i] = -1;
            }
            return mu;
        }
    }
}