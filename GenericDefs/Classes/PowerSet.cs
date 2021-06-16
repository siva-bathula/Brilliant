using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GenericDefs.Classes
{
    public class PowerSet
    {
        /// <summary>
        /// Returns factors of a number.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetFactors<T>(List<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                select from i in Enumerable.Range(0, list.Count) where (m & (1 << i)) != 0 select list[i];
        }

        /// <summary>
        /// Returns all subsets of a set. Includes empty set. 2 ^ n subsets.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetPowerSet<T>(IEnumerable<T> input)
        {
            var seed = new List<IEnumerable<T>>() { Enumerable.Empty<T>() } as IEnumerable<IEnumerable<T>>;
            return input.Aggregate(seed, (a, b) => a.Concat(a.Select(x => x.Concat(new List<T>() { b }))));
        }

        public static int GetAllFactorsCount<T>(IEnumerable<T> source)
        {
            IEnumerable<IEnumerable<T>> subsets = Subsets(source);
            return subsets.Count();
        }

        public static IEnumerable<T> GetAllFactors<T>(IEnumerable<T> source)
        {
            IEnumerable<IEnumerable<T>> subsets = Subsets(source);
            foreach (IEnumerable<T> set in subsets)
            {
                if (typeof(T) == typeof(int)) {
                    int factor = 1;
                    foreach (T f in set)
                    {
                        int fC = (int)Convert.ChangeType(f, typeof(int));
                        factor *= fC;
                    }
                    yield return (T)Convert.ChangeType(factor, typeof(T));
                } else if (typeof(T) == typeof(long)) {
                    long factor = 1;
                    foreach (T f in set)
                    {
                        long fC = (long)Convert.ChangeType(f, typeof(long));
                        factor *= fC;
                    }
                    yield return (T)Convert.ChangeType(factor, typeof(T));
                } else if (typeof(T) == typeof(BigInteger)) {
                    BigInteger factor = 1;
                    foreach (T f in set)
                    {
                        BigInteger fC = (BigInteger)Convert.ChangeType(f, typeof(BigInteger));
                        factor *= fC;
                    }
                    yield return (T)Convert.ChangeType(factor, typeof(T));
                }
            }
        }

        public static HashSet<T> GetFactorsSet<T>(IEnumerable<T> source)
        {
            IEnumerable<IEnumerable<T>> subsets = Subsets(source);
            HashSet<T> retVal = new HashSet<T>();
            foreach (IEnumerable<T> set in subsets)
            {
                if (typeof(T) == typeof(int))
                {
                    int factor = 1;
                    foreach (T f in set)
                    {
                        int fC = (int)Convert.ChangeType(f, typeof(int));
                        factor *= fC;
                    }
                    retVal.Add((T)Convert.ChangeType(factor, typeof(T)));
                } else if (typeof(T) == typeof(long)) {
                    long factor = 1;
                    foreach (T f in set)
                    {
                        long fC = (long)Convert.ChangeType(f, typeof(long));
                        factor *= fC;
                    }
                    retVal.Add((T)Convert.ChangeType(factor, typeof(T)));
                } else if (typeof(T) == typeof(BigInteger)) {
                    BigInteger factor = 1;
                    foreach (T f in set)
                    {
                        BigInteger fC = (BigInteger)Convert.ChangeType(f, typeof(BigInteger));
                        factor *= fC;
                    }
                    retVal.Add((T)Convert.ChangeType(factor, typeof(T)));
                }
            }
            return retVal;
        }

        public static IEnumerable<IEnumerable<T>> Subsets<T>(IEnumerable<T> source)
        {
            List<T> list = source.ToList();
            int length = list.Count;
            long max = (long)Math.Pow(2, list.Count);

            for (long count = 0; count < max; count++)
            {
                List<T> subset = new List<T>();
                uint rs = 0;
                while (rs < length)
                {
                    if ((count & (1u << (int)rs)) > 0)
                    {
                        subset.Add(list[(int)rs]);
                    }
                    rs++;
                }
                yield return subset;
            }
        }
    }
}