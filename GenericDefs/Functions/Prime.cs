using GenericDefs.DotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;

namespace GenericDefs.Functions
{
    public partial class Prime
    {
        public static ulong MaxPrimeFactor(ulong n)
        {
            unchecked
            {
                while (n > 3 && 0 == (n & 1)) n >>= 1;

                uint k = 3;
                ulong k2 = 9;
                ulong delta = 16;
                while (k2 <= n)
                {
                    if (n % k == 0)
                    {
                        n /= k;
                    }
                    else
                    {
                        k += 2;
                        if (k2 + delta < delta) return n;
                        k2 += delta;
                        delta += 8;
                    }
                }
            }

            return n;
        }

        public static long MaxPrimeFactor(long n)
        {
            unchecked
            {
                while (n > 3 && 0 == (n & 1)) n >>= 1;

                long k = 3;
                long k2 = 9;
                long delta = 16;
                while (k2 <= n)
                {
                    if (n % k == 0)
                    {
                        n /= k;
                    }
                    else
                    {
                        k += 2;
                        if (k2 + delta < delta) return n;
                        k2 += delta;
                        delta += 8;
                    }
                }
            }

            return n;
        }

        public static bool IsPrime(int number)
        {
            int boundary = (int)Math.Floor(Math.Sqrt(number));

            if (number < 2) return false;
            if (number == 2) return true;

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        public static bool IsPrime(long number, ClonedPrimes primes)
        {
            if (number < 2) return false;

            long max = (long)(Math.Sqrt(number));
            var filtered = primes.Primes.Where(x => x <= max);
            foreach (long p in filtered)
            {
                if (number % p == 0) { return false; }
            }

            return true;
        }

        public static bool IsPrime(BigInteger number, ClonedPrimes primes)
        {
            if (number < 2) return false;

            BigInteger num = BigInteger.Parse(number.ToString());
            if (number < long.MaxValue)
            {
                long max = (long)(Math.Sqrt((long)number));
                var filtered = primes.Primes.Where(x => x <= max);
                foreach (long p in filtered)
                {
                    if (num % p == 0) { return false; }
                }
            } else {
                foreach (long p in primes.Primes)
                {
                    if (num % p == 0) { return false; }
                }
            }

            return true;
        }

        public static bool IsPrime(long n, List<int> pMax)
        {
            if (n == 1) return false;
            foreach (int p in pMax)
            {
                if (n % p == 0) return false;
            }

            return true;
        }

        public static List<int> GeneratePrimesNaive(int n)
        {
            List<int> primes = new List<int>();
            primes.Add(2);
            int nextPrime = 3;
            while (primes.Count < n)
            {
                int sqrt = (int)Math.Sqrt(nextPrime);
                bool isPrime = true;
                for (int i = 0; (int)primes[i] <= sqrt; i++)
                {
                    if (nextPrime % primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(nextPrime);
                }
                nextPrime += 2;
            }
            return primes;
        }

        /// <summary>
        /// Returns all primes less than equal to N.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        /// 
        public static List<int> GeneratePrimesNaiveNMax(int n)
        {
            List<int> primes = new List<int>();
            primes.Add(2);
            int nextPrime = 3;
            while (true)
            {
                int sqrt = (int)Math.Sqrt(nextPrime);
                bool isPrime = true;
                for (int i = 0; (int)primes[i] <= sqrt; i++)
                {
                    if (nextPrime % primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(nextPrime);
                }
                nextPrime += 2;
                if (nextPrime > n) break;
            }
            return primes;
        }

        /// <summary>
        /// Returns all primes less than equal to N.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<long> GeneratePrimesNaiveNMax(long n)
        {
            List<long> primes = new List<long>();
            primes.Add(2);
            int nextPrime = 3;
            while (true)
            {
                int sqrt = (int)Math.Sqrt(nextPrime);
                bool isPrime = true;
                for (int i = 0; primes[i] <= sqrt; i++)
                {
                    if (nextPrime % primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(nextPrime);
                }
                nextPrime += 2;
                if (nextPrime > n) break;
            }
            return primes;
        }

        public static int ApproximateNthPrime(int nn)
        {
            double n = (double)nn;
            double p;
            if (nn >= 7022)
            {
                p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
            }
            else if (nn >= 6)
            {
                p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
            }
            else if (nn > 0)
            {
                p = new int[] { 2, 3, 5, 7, 11 }[nn - 1];
            }
            else
            {
                p = 0;
            }
            return (int)p;
        }

        public static long ApproximateNthPrime(long nn)
        {
            double n = (double)nn;
            double p;
            if (nn >= 7022)
            {
                p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
            }
            else if (nn >= 6)
            {
                p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
            }
            else if (nn > 0)
            {
                p = new int[] { 2, 3, 5, 7, 11 }[nn - 1];
            }
            else
            {
                p = 0;
            }
            return (long)p;
        }

        public static BitArray SieveOfSundaram(int limit)
        {
            limit /= 2;
            BitArray bits = new BitArray(limit + 1, true);
            for (int i = 1; 3 * i + 1 < limit; i++)
            {
                for (int j = 1; i + j + 2 * i * j <= limit; j++)
                {
                    bits[i + j + 2 * i * j] = false;
                }
            }
            return bits;
        }

        public static List<int> GeneratePrimesSieveOfSundaram(int n)
        {
            int limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfSundaram(limit);
            List<int> primes = new List<int>();
            primes.Add(2);
            for (int i = 1, found = 1; 2 * i + 1 <= limit && found < n; i++)
            {
                if (bits[i])
                {
                    primes.Add(2 * i + 1);
                    found++;
                }
            }
            return primes;
        }

        public static List<int> GeneratePrimesSieveOfSundaram(long n)
        {
            long limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfSundaram((int)limit);
            List<int> primes = new List<int>();
            primes.Add(2);
            for (int i = 1, found = 1; 2 * i + 1 <= limit && found < n; i++)
            {
                if (bits[i])
                {
                    primes.Add(2 * i + 1);
                    found++;
                }
            }
            return primes;
        }

        public static List<long> LongGeneratePrimesSieveOfSundaram(long n)
        {
            long limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfSundaram((int)limit);
            List<long> primes = new List<long>();
            primes.Add(2);
            for (int i = 1, found = 1; 2 * i + 1 <= limit && found < n; i++)
            {
                if (bits[i])
                {
                    primes.Add(2 * i + 1);
                    found++;
                }
            }
            return primes;
        }

        public static List<PrimeFactor> GeneratePrimeFactors(long num) {
            int approxMaxPrime = Convert.ToInt32(Math.Ceiling(Math.Sqrt(num)));
            List<int> primes = GeneratePrimesSieveOfSundaram(approxMaxPrime);
            primes.Sort();

            long subject = num;
            List<PrimeFactor> pF = new List<PrimeFactor>();
            foreach (int p in primes) {
                int maxP = Convert.ToInt32(Math.Ceiling(Math.Sqrt(subject)));
                PrimeFactor f = new PrimeFactor() { Prime = 0, Mul = 0 };
                bool isPf = false;
                while (subject > 1)
                {
                    if (subject % p == 0)
                    {
                        isPf = true;
                        f.Prime = p;
                        f.Mul = f.Mul + 1;
                        subject /= p;
                    }
                }

                if (isPf) { pF.Add(f); }
            }

            return pF;
        }

        /// <summary>
        /// Assumes that the primes are sorted.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="primes"></param>
        /// <returns></returns>
        public static List<PrimeFactor> GeneratePrimeFactors(long num, List<int> primes)
        {
            long subject = num;
            List<PrimeFactor> pF = new List<PrimeFactor>();
            foreach (int p in primes)
            {
                int maxP = Convert.ToInt32(Math.Ceiling(Math.Sqrt(subject)));
                PrimeFactor f = new PrimeFactor() { Prime = 0, Mul = 0 };
                bool isPf = false;
                while (subject % p == 0)
                {
                    isPf = true;
                    f.Prime = p;
                    f.Mul = f.Mul + 1;
                    subject /= p;
                }

                if (isPf) { pF.Add(f); }
            }

            return pF;
        }
    }

    public partial class Prime
    {
        /// <summary>
        /// Another Implementation from CodeProject.
        /// http://www.codeproject.com/Articles/490085/Eratosthenes-Sundaram-Atkins-Sieve-Implementation
        /// </summary>
        public class NewImpl
        {
            public static int FindPrimeUsingBruteForce(int topCandidate = 1000000)
            {
                int totalCount = 1;
                bool isPrime = true;
                for (long i = 3; i < topCandidate; i += 2)
                {
                    for (int j = 3; j * j <= i; j += 2)
                    {
                        if ((i % j) == 0) { isPrime = false; break; }
                    }
                    if (isPrime) { totalCount++; } else isPrime = true;
                }
                return totalCount;
            }

            public static int FindPrimeUsingSieveOfAtkins(int topCandidate = 1000000)
            {
                int totalCount = 0;

                BitArray isPrime = new BitArray(topCandidate + 1);

                int squareRoot = (int)Math.Sqrt(topCandidate);

                int xSquare = 1, xStepsize = 3;

                int ySquare = 1, yStepsize = 3;

                int computedVal = 0;

                for (int x = 1; x <= squareRoot; x++)
                {
                    ySquare = 1;
                    yStepsize = 3;
                    for (int y = 1; y <= squareRoot; y++)
                    {
                        computedVal = (xSquare << 2) + ySquare;

                        if ((computedVal <= topCandidate) && (computedVal % 12 == 1 || computedVal % 12 == 5))
                            isPrime[computedVal] = !isPrime[computedVal];

                        computedVal -= xSquare;
                        if ((computedVal <= topCandidate) && (computedVal % 12 == 7))
                            isPrime[computedVal] = !isPrime[computedVal];

                        if (x > y)
                        {
                            computedVal -= ySquare << 1;
                            if ((computedVal <= topCandidate) && (computedVal % 12 == 11))
                                isPrime[computedVal] = !isPrime[computedVal];
                        }
                        ySquare += yStepsize;
                        yStepsize += 2;
                    }
                    xSquare += xStepsize;
                    xStepsize += 2;
                }

                for (int n = 5; n <= squareRoot; n++)
                {
                    if (isPrime[n] == true)
                    {
                        int k = n * n;
                        for (int z = k; z <= topCandidate; z += k)
                            isPrime[z] = false;
                    }
                }

                for (int i = 1; i < topCandidate; i++)
                {
                    if (isPrime[i]) totalCount++;
                }
                return (totalCount + 2); // 2 and 3 will be missed in Sieve Of Atkins
            }

            /* http://programmingpraxis.com/2010/02/19/sieve-of-atkin-improved/ */
            public static int FindPrimeUsingSieveOfAtkinsOptimized(int uptoNumber = 1000000)
            {
                int totalCount = 0;

                BitArray isPrime = new BitArray(uptoNumber + 1);

                int squareRoot = (int)Math.Sqrt(uptoNumber);

                int xStepsize = 3;
                int y_limit = 0;
                int n = 0;
                #region 3x^2 + y^2 computation

                int temp = ((int)Math.Sqrt((uptoNumber - 1) / 3));
                for (int i = 0; i < 12 * temp; i += 24)
                {
                    xStepsize += i;
                    y_limit = (12 * (int)Math.Sqrt(uptoNumber - xStepsize)) - 36;
                    n = xStepsize + 16;
                    for (int j = -12; j < y_limit + 1; j += 72)
                    {
                        n += j;
                        isPrime[n] = !isPrime[n];
                    }

                    n = xStepsize + 4;

                    for (int j = 12; j < y_limit + 1; j += 72)
                    {
                        n += j;
                        isPrime[n] = !isPrime[n];
                    }
                }

                #endregion

                #region 4x^2 + y^2 computation

                xStepsize = 0;
                temp = 8 * (int)(Math.Sqrt((uptoNumber - 1) / 4)) + 4;
                for (int i = 4; i < temp; i += 8)
                {
                    xStepsize += i;
                    n = xStepsize + 1;

                    if (xStepsize % 3 != 0)
                    {
                        int tempTwo = 4 * ((int)Math.Sqrt(uptoNumber - xStepsize)) - 3;
                        for (int j = 0; j < tempTwo; j += 8)
                        {
                            n += j;
                            isPrime[n] = !isPrime[n];
                        }

                    }
                    else
                    {
                        y_limit = 12 * (int)Math.Sqrt(uptoNumber - xStepsize) - 36;
                        n = xStepsize + 25;
                        for (int j = -24; j < y_limit + 1; j += 72)
                        {
                            n += j;
                            isPrime[n] = !isPrime[n];
                        }

                        n = xStepsize + 1;

                        for (int j = 24; j < y_limit + 1; j += 72)
                        {
                            n += j;
                            isPrime[n] = !isPrime[n];
                        }
                    }
                }

                #endregion

                #region 3x^2 - y^2 computation
                xStepsize = 1;
                temp = (int)Math.Sqrt(uptoNumber / 2) + 1;
                for (int i = 3; i < temp; i += 2)
                {
                    xStepsize += 4 * i - 4;
                    n = 3 * xStepsize;
                    int s = 4;
                    if (n > uptoNumber)
                    {
                        int min_y = (((int)(Math.Sqrt(n - uptoNumber)) >> 2) << 2);
                        int yy = min_y * min_y;
                        n -= yy;
                        s = 4 * min_y + 4;
                    }
                    else s = 4;

                    for (int j = s; j < 4 * i; j += 8)
                    {
                        n -= j;
                        if (n <= uptoNumber && n % 12 == 11)
                            isPrime[n] = !isPrime[n];
                    }

                }

                xStepsize = 0;
                for (int i = 2; i < temp; i += 2)
                {
                    xStepsize += 4 * i - 4;
                    n = 3 * xStepsize;
                    int s = 0;
                    if (n > uptoNumber)
                    {
                        int min_y = (((int)(Math.Sqrt(n - uptoNumber)) >> 2) << 2) - 1;
                        int yy = min_y * min_y;
                        n -= yy;
                        s = 4 * min_y + 4;
                    }
                    else
                    {
                        n -= 1;
                        s = 0;
                    }
                    for (int j = s; j < 4 * i; j += 8)
                    {
                        n -= j;
                        if (n <= uptoNumber && n % 12 == 11)
                            isPrime[n] = !isPrime[n];
                    }
                }
                #endregion

                #region Eliminate Squares
                for (int i = 5; i < squareRoot + 1; i += 2)
                {
                    if (isPrime[i] == true)
                    {
                        int k = i * i;
                        for (int z = k; z < uptoNumber; z += k)
                            isPrime[z] = false;
                    }
                }

                for (int i = 5; i < uptoNumber; i += 2)
                {
                    if (isPrime[i]) totalCount++;
                }

                #endregion
                return (totalCount + 2);// 2 and 3 will be missed in Sieve Of Atkins
            }

            public static int FindPrimeUsingSieveOfEratosthenes(int topCandidate = 1000000)
            {
                int totalCount = 0;
                BitArray myBA1 = new BitArray(topCandidate + 1);

                /* Set all but 0 and 1 to prime status */
                myBA1.SetAll(true);
                myBA1[0] = myBA1[1] = false;

                /* Mark all the non-primes */
                int thisFactor = 2;
                int lastSquare = 0;
                int thisSquare = 0;

                while (thisFactor * thisFactor <= topCandidate)
                {
                    /* Mark the multiples of this factor */
                    int mark = thisFactor + thisFactor;
                    while (mark <= topCandidate)
                    {
                        myBA1[mark] = false;
                        mark += thisFactor;

                    }

                    /* Print the proven primes so far */
                    thisSquare = thisFactor * thisFactor;
                    for (; lastSquare < thisSquare; lastSquare++)
                    {
                        if (myBA1[lastSquare]) totalCount++;

                    }

                    /* Set thisfactor to next prime */
                    thisFactor++;
                    while (!myBA1[thisFactor]) { thisFactor++; }

                }
                /* Print the remaining primes */
                for (; lastSquare <= topCandidate; lastSquare++)
                {
                    if (myBA1[lastSquare]) { totalCount++; }

                }
                return totalCount;
            }

            public static int FindPrimeUsingSieveOfSundaram(int topCandidate = 1000000)
            {
                int totalCount = 0;
                int k = topCandidate / 2;
                BitArray myBA1 = new BitArray(k + 1);

                /* SET ALL TO PRIME STATUS */
                myBA1.SetAll(true);

                /* SEIVE */
                int maxVal = 0;
                int denominator = 0;
                for (int i = 1; i < k; i++)
                {
                    denominator = (i << 1) + 1;
                    maxVal = (k - i) / denominator;
                    for (int j = i; j <= maxVal; j++)
                    {
                        myBA1[i + j * denominator] = false;
                    }
                }
                int dummy = 0;
                for (int i = 1; i < k; i++)
                {
                    if (myBA1[i])
                    {
                        totalCount++;
                        dummy = (i << 1) + 1; // dummy contains prime number.The code is here not ignore the prime number calcuation part.
                    }
                }
                return (totalCount + 1); // 2 will be missed in Sieve Of Sundaram
            }
        }
        
        public static class RabinMiller
        {
            public static bool IsPrime(int n, int k)
            {
                if (n < 2)
                {
                    return false;
                }
                if (n != 2 && n % 2 == 0)
                {
                    return false;
                }
                int s = n - 1;
                while (s % 2 == 0)
                {
                    s >>= 1;
                }
                Random r = new Random();
                for (int i = 0; i < k; i++)
                {
                    double a = r.Next((int)n - 1) + 1;
                    int temp = s;
                    int mod = (int)Math.Pow(a, (double)temp) % n;
                    while (temp != n - 1 && mod != 1 && mod != n - 1)
                    {
                        mod = (mod * mod) % n;
                        temp = temp * 2;
                    }
                    if (mod != n - 1 && temp % 2 == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

    public class KnownPrimes
    {
        static List<long> _primes = new List<long>();
        static readonly long _default = 1000000;

        private static void InitializeFirstN(long n) {
            InitializeKnownPrimes(n);
        }

        private static object _simpleLock = new object();

        /// <summary>
        /// Use this for single thread implementation.
        /// Use 'CloneKnownPrimes' for parallel implementation.
        /// </summary>
        /// <returns></returns>
        public static List<long> GetAllKnownPrimes()
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            lock (_simpleLock)
            {
                ((IEnumerator)_primes.GetEnumerator()).Reset();
                return _primes;
            }
        }

        /// <summary>
        /// Clones a list of known primes for parallel implementation.
        /// </summary>
        /// <param name="start">Range start</param>
        /// <param name="end">Range end</param>
        /// <returns></returns>
        public static ClonedPrimes CloneKnownPrimes(long start = 0, long end = long.MaxValue)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);

            if(start == 0 && end == long.MaxValue)
            {
                return new ClonedPrimes() { Primes = new List<long>(_primes) };
            }

            var list = _primes.Where(x => x >= start && x <= end);
            return new ClonedPrimes() { Primes = new List<long>(list) };
        }

        /// <summary>
        /// Clones a list of known primes for parallel implementation.
        /// </summary>
        /// <param name="start">Range start</param>
        /// <param name="end">Range end</param>
        /// <returns></returns>
        public static List<long> GetClonedPrimes(long start, long end)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            var list = _primes.Where(x => x >= start && x <= end);
            return new List<long>(list);
        }

        /// <summary>
        /// Clones a list of known primes for parallel implementation.
        /// </summary>
        /// <param name="start">Range start</param>
        /// <param name="count">Total Count from start.</param>
        /// <returns></returns>
        public static List<long> GetClonedPrimes(long start, int count)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            int index = -1;
            _primes.ForEach(x => { if (x < start) index++; });
            if (index < 0) index = 0;
            var list = _primes.GetRange(index, count);
            return new List<long>(list);
        }

        /// <summary>
        /// Gets all primes between 'start' and 'end'.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<long> GetPrimes(long start, long end)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            return _primes.Where(x => x >= start && x <= end).ToList();
        }

        /// <summary>
        /// Gets all primes less than maxValue.
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static List<long> GetPrimes(long maxValue)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            return _primes.Where(x => x <= maxValue).ToList();
        }

        private static bool _isInitialized = false;
        public static List<long> GetAllKnownFirstNPrimes(int n)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            if (_primes.Count < n) {
                InitializeFirstN(n);
                return _primes;
            } else {
                lock (_simpleLock) {
                    return _primes.GetRange(0, n);
                }
            }
        }

        private static void InitializeKnownPrimes(long n)
        {
            lock (_simpleLock)
            {
                _primes.Clear();

                if (!LoadFromFile()) {
                    List<int> primes = Prime.GeneratePrimesNaive((int)n);
                    _primes = primes.ConvertAll(i => (long)i);
                    StorePrimes();
                }
                _primes.Sort();
                _isInitialized = true;
            }
        }

        private static void StorePrimes()
        {
            SerializePrimes p = new SerializePrimes() { Primes = _primes };
            Serialization.SerializeObject(p, "KnownPrimes.xml");
        }

        private static bool LoadFromFile()
        {
            string text = string.Empty;
            string fName =  "";
            Assembly a = Assembly.GetEntryAssembly();
            string pName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (a == null)
            {
                fName = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Data/KnownPrimes/KnownPrimes.xml");
            } else fName = StreamHelper.GetSolutionPath() + "/Data/KnownPrimes/KnownPrimes.xml";

            using (var stream = File.Open(fName, FileMode.Open))
            {
                using (var sr = new StreamReader(stream))
                {
                    text = sr.ReadToEnd().Trim();
                }
            }
            if(string.IsNullOrEmpty(text)) {
                return false;
            } else {
                SerializePrimes p = Serialization.DeSerializeObjectFromXml<SerializePrimes>(text);
                _primes = p.Primes;
                return true;
            }
        }

        public static void AddPrimes(List<long> newPrimes)
        {
            lock (_simpleLock)
            {
                foreach (long p in newPrimes)
                {
                    if (!_primes.Contains(p))
                    {
                        _primes.Add(p);
                    }
                }

                _primes.Sort();
            }
        }

        public static long GetGreatestKnownPrime()
        {
            if (_primes.Count == 0) { throw new Exception("Primes are not generated."); }
            return _primes[_primes.Count - 1];
        }

        public static long GetLeastKnownPrime()
        {
            if (_primes.Count == 0) { throw new Exception("Primes are not generated."); }
            return _primes[0];
        }

        public static bool IsPrime(long n)
        {
            if (!_isInitialized) InitializeKnownPrimes(_default);
            return _primes.Contains(n);
        }

        internal static void ReinitializeIfRequired(long n) {
            if (!_isInitialized || _primes.Count < n)
            {
                InitializeFirstN(n);
            }
        }
    }

    public class ClonedPrimes
    {
        internal List<long> Primes { get; set; }

        private long rMin, rMax;
        public bool Contains(long p)
        {
            return Primes.Contains(p);
        }

        public ClonedPrimes() { }

        public ClonedPrimes(int start, int end)
        {
            Primes = KnownPrimes.GetPrimes(start, end);
            rMin = Primes.Min();
            rMax = Primes.Max();
        }

        public bool IsPrime(long n)
        {
            if(n < rMin || n > rMax) {
                throw new ArgumentException("n is out of range.");
            }
            return Primes.Contains(n);
        }
    }

    [Serializable]
    public class SerializePrimes
    {
        public List<long> Primes { get; set; }
    }

    public class PrimeFactor {
        public int Prime { get; set; }
        /// <summary>
        /// Multiplicity
        /// </summary>
        public int Mul { get; set; }
    }
}