using System;
using System.Collections.Generic;
using System.Linq;
using GenericDefs.DotNet;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions
{
    public class EulerTotient
    {
        public static long[] CalcTotients(long n)
        {
            long[] divisors = GetDivisors(n);
            long i;
            var phi = new long[n];
            phi[1] = 1;
            for (i = 1; i < n; ++i) { 
                CalculateTotient(i, phi, divisors);
            }

            return phi;
        }

        public static long CalculateTotient(long n) {
            List<long> pFactors = Factors.GetPrimeFactors(n);

            long totient = n;

            foreach (long p in pFactors) {
                totient *= (p - 1);
                totient /= p;
            }

            return totient;
        }

        public static long CalculateTotient(long n, ClonedPrimes primes)
        {
            if (primes.Primes.Contains(n)) return n - 1;
            List<long> pFactors = Factors.GetPrimeFactors(n, primes);

            long totient = n;

            foreach (long p in pFactors)
            {
                totient *= (p - 1);
                totient /= p;
            }

            return totient;
        }

        /// <summary>
        /// For every integer, the result will contain its lowest divisor.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static long[] GetDivisors(long n)
        {
            var divisors = new long[n];
            divisors[1] = 1;
            long i;
            for (i = 2; i < n; ++i)
            {
                if (divisors[i] != 0)
                    continue;

                for (long j = i; j < n; j += i)
                    divisors[j] = i;
            }
            return divisors;
        }

        private static long CalculateTotient(long i, long[] phi, long[] divisors)
        {
            if (phi[i] != 0)
                return phi[i];

            long div = divisors[i];
            if (div == i)
            {
                phi[i] = i - 1;
                return phi[i];
            }

            long lower = 1;
            int exp = 0;
            while ((i > 1) && (i % div == 0))
            {
                i /= div;
                lower *= div;
                exp++;
            }
            if (i == 1)
            {
                phi[lower] = ((long)Math.Pow(div, exp - 1)) * (div - 1);
                return phi[lower];
            }
            phi[i * lower] = CalculateTotient(i, phi, divisors) *
                                 CalculateTotient(lower, phi, divisors);
            return phi[i * lower];
        }

        /// <summary>
        /// Returns all coprimes of a number, less than the number.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="primes"></param>
        /// <returns></returns>
        public static List<int> GetCoprimes(int n, ClonedPrimes primes)
        {
            List<int> retList = new List<int>();
            retList.Add(1);

            if (primes.Primes.Contains(n))
            {
                return Enumerable.Range(1, n - 1).ToList();
            }

            int a = 1;
            List<long> nPrimes = Factors.GetPrimeFactors(n, primes);        
            while (++a <= n)
            {
                if (n % a == 0) continue;

                bool isCoprime = true;
                foreach (long p in nPrimes)
                {
                    if (a % p == 0) { isCoprime = false; break; }
                    if (p > a) { break; }
                }
                if (isCoprime) retList.Add(a);
            }
            return retList;
        }
    }
}
