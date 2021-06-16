using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDefs.Functions
{
    public class RemainderCalculator
    {
        /// <summary>
        /// Calculates remainder, given a problem like a^b^c^d^e mod n.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static long RemainderWithNestedPowers(int[] numbers, int divisor)
        {
            Dictionary<int, Dictionary<long, long>> cycles = new Dictionary<int, Dictionary<long, long>>();
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                if (i == 0) cycles.Add(i, GetCycles(numbers[i], divisor));
                else cycles.Add(i, GetCycles(numbers[i], cycles[i - 1].Count));
            }

            int j = numbers.Length - 1;
            long rem = 0;
            while (true)
            {
                long key = 0;
                if (j == numbers.Length - 1) key = numbers[j];
                else key = rem;
                key %= cycles[j - 1].Count;
                if (key == 0) key = cycles[j - 1].Count;

                rem = cycles[j - 1][key];
                j--;
                if (j == 0) break;
            }

            return rem;
        }

        public static long RemainderWithNestedPowers(long[] numbers, long divisor)
        {
            Dictionary<int, Dictionary<long, long>> cycles = new Dictionary<int, Dictionary<long, long>>();
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                if (i == 0) cycles.Add(i, GetCycles(numbers[i], divisor));
                else cycles.Add(i, GetCycles(numbers[i], cycles[i - 1].Count));
            }

            int j = numbers.Length - 1;
            long rem = 0;
            while (true)
            {
                long key = 0;
                if (j == numbers.Length - 1) key = numbers[j];
                else key = rem;
                key %= cycles[j - 1].Count;
                if (key == 0) key = cycles[j - 1].Count;

                rem = cycles[j - 1][key];
                j--;
                if (j == 0) break;
            }

            if (rem < 0)
            {
                return divisor + rem;
            }

            return rem;
        }

        /// <summary>
        /// Calculates remainder, given a problem like a^b^c^d^e mod n.
        /// https://brilliant.org/problems/advanced-modular-arithmetic-3/
        /// https://brilliant.org/problems/advanced-modular-arithmetic-2/
        /// https://brilliant.org/problems/1234567890-2/
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static long RemainderWithNestedPowers1(long[] numbers, long divisor)
        {
            Dictionary<long, Cycle<long>> cycles = new Dictionary<long, Cycle<long>>();
            for (long i = 0; i < numbers.Length - 1; i++)
            {
                if (i == 0) cycles.Add(i, GetCycles1(numbers[i], divisor));
                else cycles.Add(i, GetCycles1(numbers[i], cycles[i - 1].CycleLength));
            }

            int j = numbers.Length - 1;
            long rem = 0;
            bool isCascading = numbers.Count() > 2 ? true : false;
            bool isBasePower = false;
            while (true)
            {
                long key = 0;
                Cycle<long> c = cycles[j - 1];
                if (j == 1) isBasePower = true;

                if (j == numbers.Length - 1) key = numbers[j];
                else key = rem;

                key %= c.CycleLength;
                if (key == 0) key = c.CycleLength;

                long key1 = key;
                if (c.HasPrefix)
                {
                    if (isCascading) {
                        key1 -= c.FirstCyclePrefixLength;
                        if (key1 <= 0) key1 += (c.CycleLength + c.FirstCyclePrefixLength);
                        else if (isBasePower) key1 += c.FirstCyclePrefixLength;
                    } else {
                        key1 += c.FirstCyclePrefixLength;
                    }
                }
                rem = c.CycleDetail.RepeatingCycle[key1];
                j--;
                if (j == 0) break;
            }

            return rem;
        }

        private static int MaxTrialCount = 100000;

        /// <summary>
        /// Calculates the cycles at which the remainders repeat, of a number and its successive powers w.r.t. a divisor.
        /// </summary>
        /// <param name="N"></param>
        /// <param name="divisor"></param>
        /// <returns>A dictionary with successive powers as keys and their 'mod value' w.r.t. divisor as values.</returns>
        public static Dictionary<long, long> GetCycles(long N, long divisor)
        {
            Dictionary<long, long> d = new Dictionary<long, long>();
            long n = 1;
            int trials = 0;
            while (true)
            {
                n *= N;
                n %= divisor;

                if (!d.ContainsValue(n)) { d.Add(++trials, n); }
                else { break; }

                if (trials >= MaxTrialCount) {
                    throw new Exception("Maximum trial count exceeded. ");
                }
            }
            return d;
        }

        /// <summary>
        /// Calculates the cycles at which the remainders repeat, of a number and its successive powers w.r.t. a divisor.
        /// </summary>
        /// <param name="N"></param>
        /// <param name="divisor"></param>
        /// <returns>A dictionary with successive powers as keys and their 'mod value' w.r.t. divisor as values.</returns>
        public static Dictionary<long, long> GetCycles(int N, int divisor)
        {
            Dictionary<long, long> d = new Dictionary<long, long>();
            int n = 1;
            int trials = 0;
            while (true)
            {
                trials++;
                n *= N;
                n %= divisor;

                if (!d.ContainsValue(n))
                {
                    d.Add(trials, n);
                }
                else {
                    break;
                }

                if (trials >= MaxTrialCount) { throw new Exception("Maximum trial count exceeded. "); }
            }
            return d;
        }

        /// <summary>
        /// Calculates the cycles at which the remainders repeat, of a number and its successive powers w.r.t. a divisor.
        /// </summary>
        /// <param name="N"></param>
        /// <param name="divisor"></param>
        /// <returns>A dictionary with successive powers as keys and their 'mod value' w.r.t. divisor as values.</returns>
        static Cycle<long> GetCycles1(long N, long divisor)
        {
            Cycle<long> C = new Cycle<long>();
            Dictionary<long, long> d = new Dictionary<long, long>();
            long n = 1;
            int trials = 0;
            while (true)
            {
                trials++;
                n *= N;
                n %= divisor;

                if (!d.ContainsValue(n))
                {
                    d.Add(trials, n);
                }
                else
                {
                    if(n != d[1])
                    {
                        C.HasPrefix = true;
                        int prefixlength = (int)(d.FirstOrDefault(x => x.Value == n).Key) - 1;
                        C.FirstCycleLength = d.Count;
                        C.FirstCyclePrefixLength = prefixlength;
                        C.CycleLength = d.Count - prefixlength;
                        C.CycleDetail.PrefixCycle = d.Take(prefixlength).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                        C.CycleDetail.RepeatingCycle = d.Skip(prefixlength).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    } else {
                        C.HasPrefix = false;
                        C.FirstCycleLength = d.Count;
                        C.FirstCyclePrefixLength = 0;
                        C.CycleLength = d.Count;
                        C.CycleDetail.RepeatingCycle = d;
                    }
                    break;
                }

                if (trials >= MaxTrialCount) { throw new Exception("Maximum trial count exceeded. "); }
            }
            return C;
        }

        public static long ModularInverse(long inverseOf, int divisor)
        {
            Tuple<long, long, long> xy = MathFunctions.EuclideanGCD(inverseOf, divisor);
            return xy.Item2 % divisor;
        }
        
        /// <summary>
        /// l = a + (n * b), a = first cycle prefix. b = cycle length.
        /// </summary>
        internal class Cycle<T>
        {
            internal bool HasPrefix;
            internal long CycleLength;
            internal long FirstCyclePrefixLength;
            internal long FirstCycleLength;
            internal Cycles CycleDetail;
            internal Cycle()
            {
                CycleDetail = new Cycles();
            }

            internal class Cycles
            {
                internal Dictionary<T, T> PrefixCycle;
                internal Dictionary<T, T> RepeatingCycle;
            }
        }
    }
}