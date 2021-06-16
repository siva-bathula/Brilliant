using GenericDefs.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GenericDefs.Functions
{
    public class Factors
    {
        public static List<long> GetPrimeFactors(long n)
        {
            List<long> knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> pfactors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    pfactors.Add(p);
                    while (n % p == 0) n /= p;
                }
            }

            return pfactors;
        }

        public static List<long> GetPrimeFactors(long n, List<long> primes = null)
        {
            List<long> knownPrimes = primes;
            if (primes == null || primes.Count == 0) knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> pfactors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    pfactors.Add(p);
                    while (n % p == 0) n /= p;
                }
            }

            return pfactors;
        }

        public static long GetLeastPrimeFactor(long n, ClonedPrimes cPrimes = null)
        {
            List<long> knownPrimes;
            if (cPrimes != null) knownPrimes = cPrimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();
            
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    return p;
                }
            }

            return n;
        }

        public static List<long> GetPrimeFactors(long n, ClonedPrimes cPrimes = null)
        {
            List<long> knownPrimes;
            if (cPrimes != null) knownPrimes = cPrimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> pfactors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    pfactors.Add(p);
                    while (n % p == 0) n /= p;
                }
            }

            return pfactors;
        }

        public static bool HasPrimeFactors(long n, SortedSet<long> pFactors)
        {
            if (n == 1) return false;
            foreach (long p in pFactors)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    while (n % p == 0) n /= p;
                }
            }
            return n == 1;
        }

        public static bool HasPrimeFactors(BigInteger n, SortedSet<long> pFactors)
        {
            if (n == 1) return false;
            foreach (long p in pFactors)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    while (n % p == 0) n /= p;
                }
            }
            return n == 1;
        }
        public static bool IsGCDOne(long a, HashSet<long> bPrimeFactors)
        {
            List<long> knownPrimes = KnownPrimes.GetAllKnownPrimes();
            bool retVal = true;
            long maxbFactor = bPrimeFactors.Max();
            long min = Math.Min(a, maxbFactor);
            foreach (long p in knownPrimes)
            {
                if (p > min) { break; }
                if (a % p == 0)
                {
                    if (bPrimeFactors.Contains(p)) { retVal = false; break; }
                    while (a % p == 0) a /= p;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Returns prime factors along with number of times each prime factor divides the number.
        /// </summary>
        /// <param name="n">Number of times the prime number divides the number. If divisible by 9 then 3^2, by 81 then 3^4 etc.</param>
        /// <returns></returns>
        public static Dictionary<long, int> GetPrimeFactorsWithMultiplicity(long n, ClonedPrimes cPrimes = null)
        {
            List<long> knownPrimes;
            if (cPrimes != null) knownPrimes = cPrimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();

            Dictionary<long, int> pfactors = new Dictionary<long, int>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; };
                if (n % p == 0)
                {
                    int t = 0;
                    while (n % p == 0) {
                        n /= p;
                        t++;
                        if (n % p != 0) break;
                    }
                    pfactors.Add(p, t);
                }
            }

            return pfactors;
        }

        /// <summary>
        /// Returns prime factors along with number of times each prime factor divides the number.
        /// </summary>
        /// <param name="n">Number of times the prime number divides the number. If divisible by 9 then 3^2, by 81 then 3^4 etc.</param>
        /// <returns></returns>
        public static Dictionary<long, int> GetPrimeFactorsWithMultiplicity(BigInteger n, ClonedPrimes cPrimes = null)
        {
            List<long> knownPrimes;
            if (cPrimes != null) knownPrimes = cPrimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();

            Dictionary<long, int> pfactors = new Dictionary<long, int>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; };
                if (n % p == 0)
                {
                    int t = 0;
                    while (n % p == 0)
                    {
                        n /= p;
                        t++;
                        if (n % p != 0) break;
                    }
                    pfactors.Add(p, t);
                }
            }

            return pfactors;
        }

        /// <summary>
        /// Returns prime factors(with repeats if 2^3 is a divisor then the list will have 2 added three times).
        /// </summary>
        /// <param name="n">The number.</param>
        /// <returns></returns>
        public static List<long> GetPrimeFactorsWithRepeats(long n, ClonedPrimes cPrimes = null)
        {
            List<long> knownPrimes;
            if (cPrimes != null) knownPrimes = cPrimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> pfactors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; };
                if (n % p == 0)
                {
                    while (n % p == 0)
                    {
                        n /= p;
                        pfactors.Add(p);
                        if (n % p != 0) break;
                    }
                }
            }

            return pfactors;
        }

        public static List<long> GetPrimeFactors(BigInteger n)
        {
            List<long> knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> pfactors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    pfactors.Add(p);
                    while (n % p == 0) n /= p;
                }
            }

            return pfactors;
        }

        public static List<long> GetPrimeFactors(BigInteger n, ClonedPrimes cPrimes = null)
        {
            List<long> knownPrimes;
            if (cPrimes != null) knownPrimes = cPrimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> pfactors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > n) { break; }
                if (n % p == 0)
                {
                    pfactors.Add(p);
                    while (n % p == 0) n /= p;
                }
            }

            return pfactors;
        }

        /// <summary>
        /// Returns a set of all factors of a number.
        /// Power set of prime factors.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static HashSet<long> GetAllFactors(long num) {
            Dictionary<long, int> pFactors = GetPrimeFactorsWithMultiplicity(num);
            List<long> divisors = new List<long>();
            divisors.Add(1);

            foreach (KeyValuePair<long, int> kvp in pFactors)
            {
                int mul = kvp.Value;
                while (true)
                {
                    divisors.Add(kvp.Key);
                    mul--;
                    if (mul == 0) break;
                }
            }

            var pSet = PowerSet.GetPowerSet(divisors);
            HashSet<long> factors = new HashSet<long>();
            foreach (var p in pSet)
            {
                var factor = p.Select(x => x).Aggregate((long)1, (x, y) => x * y);
                factors.Add(factor);
            }

            return factors;
        }

        public static HashSet<long> GetAllFactors(long num, ClonedPrimes cprimes = null)
        {
            List<long> divisors = GetPrimeFactorsWithRepeats(num, cprimes);
            return PowerSet.GetFactorsSet(divisors);
        }

        public static HashSet<long> GetAllOddFactors(long num, ClonedPrimes cprimes = null)
        {
            List<long> knownPrimes;
            if (cprimes != null) knownPrimes = cprimes.Primes;
            else knownPrimes = KnownPrimes.GetAllKnownPrimes();

            List<long> divisors = new List<long>();
            foreach (long p in knownPrimes)
            {
                if (p > num) { break; };
                if (num % p == 0)
                {
                    while (num % p == 0)
                    {
                        num /= p;
                        if (p != 2) divisors.Add(p);
                        if (num % p != 0) break;
                    }
                }
            }

            return PowerSet.GetFactorsSet(divisors);
        }

        /// <summary>
        /// Returns a set of all factors of a number in sorted order.
        /// Power set of prime factors.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static SortedSet<long> GetAllSortedFactors(long num)
        {
            Dictionary<long, int> pFactors = GetPrimeFactorsWithMultiplicity(num);
            List<long> divisors = new List<long>();
            divisors.Add(1);

            foreach (KeyValuePair<long, int> kvp in pFactors)
            {
                int mul = kvp.Value;
                while (true)
                {
                    divisors.Add(kvp.Key);
                    mul--;
                    if (mul == 0) break;
                }
            }

            var pSet = PowerSet.GetPowerSet(divisors);
            SortedSet<long> factors = new SortedSet<long>();
            foreach (var p in pSet)
            {
                var factor = p.Select(x => x).Aggregate((long)1, (x, y) => x * y);
                factors.Add(factor);
            }

            return factors;
        }

        /// <summary>
        /// Returns a set of all factors of a number.
        /// Power set of prime factors.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int GetAllFactorsCount(long num, ClonedPrimes cprimes = null)
        {
            Dictionary<long, int> pFactors = GetPrimeFactorsWithMultiplicity(num, cprimes);
            List<long> divisors = new List<long>();
            divisors.Add(1);

            foreach (KeyValuePair<long, int> kvp in pFactors)
            {
                int mul = kvp.Value;
                while (true)
                {
                    divisors.Add(kvp.Key);
                    mul--;
                    if (mul == 0) break;
                }
            }
            
            HashSet<long> factors = new HashSet<long>(PowerSet.GetAllFactors(divisors));
            return factors.Count;
        }

        /// <summary>
        /// Returns a set of all factors of a number.
        /// Power set of prime factors.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int GetAllFactorsCount(BigInteger num, ClonedPrimes cprimes = null)
        {
            Dictionary<long, int> pFactors = GetPrimeFactorsWithMultiplicity(num, cprimes);
            List<long> divisors = new List<long>();
            divisors.Add(1);

            foreach (KeyValuePair<long, int> kvp in pFactors)
            {
                int mul = kvp.Value;
                while (true)
                {
                    divisors.Add(kvp.Key);
                    mul--;
                    if (mul == 0) break;
                }
            }

            return PowerSet.GetAllFactorsCount(divisors);
        }

        /// <summary>
        /// Returns the sum of factors. Power set of prime factors.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long GetAllFactorsSum(long num, ClonedPrimes cprimes = null)
        {
            List<long> divisors = GetPrimeFactorsWithRepeats(num, cprimes);
            return PowerSet.GetFactorsSet(divisors).Sum();
        }
    }
}