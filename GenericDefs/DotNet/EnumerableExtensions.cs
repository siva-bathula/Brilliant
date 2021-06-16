using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GenericDefs.DotNet
{
    public static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

    public static class DictionaryExtension
    {
        /// <summary>
        /// Adds a new key with the specified value to the dictionary, otherwise increments existing key with the value.
        /// Not for parallel implementation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<T>(this Dictionary<T, int> dictionary, T key, int value)
        {
            if (dictionary.ContainsKey(key)) { dictionary[key] += value; }
            else dictionary.Add(key, value);
        }

        /// <summary>
        /// Adds a new key with the specified value to the dictionary, otherwise increments existing key with the value.
        /// Not for parallel implementation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<T>(this Dictionary<T, double> dictionary, T key, double value)
        {
            if (dictionary.ContainsKey(key)) { dictionary[key] += value; }
            else dictionary.Add(key, value);
        }

        /// <summary>
        /// Adds a new key with the specified value to the dictionary, otherwise increments existing key with the value.
        /// Not for parallel implementation.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="a"></param>
        public static void GenericAddOrIncrement<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 a)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T2), "a"), paramB = Expression.Parameter(typeof(T2), "b");
            BinaryExpression body = Expression.Add(paramA, paramB);
            Func<T2, T2, T2> add = Expression.Lambda<Func<T2, T2, T2>>(body, paramA, paramB).Compile();
            if (dictionary.ContainsKey(key)) { T2 b = dictionary[key]; dictionary[key] = add(a, b); }
            else { dictionary.Add(key, a); }
        }

        public static void GenericAddOrReplace<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 a)
        {
            if (dictionary.ContainsKey(key)) { dictionary[key] = a; }
            else { dictionary.Add(key, a); }
        }

        /// <summary>
        /// Adds a key-value pair once, if it doesn't exist. If it is already added, does nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOnce<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (!dictionary.ContainsKey(key)) { dictionary.Add(key, value); }
        }

        public static Dictionary<T1, T2> OrderByValue<T1, T2>(this Dictionary<T1, T2> input, OrderByDirection direction)
        {
            IOrderedEnumerable<KeyValuePair<T1, T2>> orderedList = null;
            if (direction == OrderByDirection.Ascending) orderedList = input.OrderBy(d => d.Value);
            else if (direction == OrderByDirection.Descending) orderedList = input.OrderByDescending(d => d.Value);

            return orderedList.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

    public static class EnumerableExtensions
    {
        public static bool HasDuplicates<T>(this IEnumerable<T> subjects)
        {
            return HasDuplicates(subjects, EqualityComparer<T>.Default);
        }

        public static bool HasDuplicates<T>(this IEnumerable<T> subjects, IEqualityComparer<T> comparer)
        {
            if (subjects == null) throw new ArgumentNullException("subjects");

            if (comparer == null) throw new ArgumentNullException("comparer");

            var set = new HashSet<T>(comparer);

            foreach (var s in subjects)
                if (!set.Add(s)) return true;

            return false;
        }

        public static IEnumerable<T> PairwiseForEach<T>(this IEnumerable<T> enumerable, Func<T, T, T> func)
        {
            var copy = enumerable.Select(x => x);
            foreach (T e in enumerable)
            {
                copy = copy.SkipWhile(x => !x.Equals(e));
                if (copy.Count() == 0) break;
                foreach (T c in copy)
                    yield return func(e, c);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration) action(item);
        }
    }

    public class TupleEqualityComparer : IEqualityComparer<Tuple<int, int, int, int>>
    {
        public bool Equals(Tuple<int, int, int, int> t1, Tuple<int, int, int, int> t2)
        {
            if (t2 == null && t1 == null) return true;
            else if (t1 == null | t2 == null) return false;
            else if (t1.Item2 == t2.Item2 && t1.Item1 != t2.Item1) return true;
            else return false;
        }

        public int GetHashCode(Tuple<int, int, int, int> t)
        {
            int hCode = (t.Item1 + t.Item2) * (t.Item3 + t.Item4);
            return hCode.GetHashCode();
        }
    }

    public class TEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> getEquals;
        private readonly Func<T, int> getHashCode;

        public TEqualityComparer(Func<T, T, bool> equals, Func<T, int> hashCode)
        {
            getEquals = equals;
            getHashCode = hashCode;
        }

        public bool Equals(T x, T y)
        {
            return getEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return getHashCode(obj);
        }
    }

    public static class EnumExtensions
    {
        public static string ToClassName(this BrilliantProblems val)
        {
            object[] attributes = val.GetType().GetField(val.ToString()).GetCustomAttributes(false);
            return attributes.Length > 0 ? ((ClassnameAttribute)attributes[0]).ClassName : string.Empty;
        }

        public static string ToClassName(this ProjectEulerProblems val)
        {
            object[] attributes = val.GetType().GetField(val.ToString()).GetCustomAttributes(false);
            return attributes.Length > 0 ? ((ClassnameAttribute)attributes[0]).ClassName : string.Empty;
        }

        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue) where TEnum : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            TEnum result;
            return Enum.TryParse(value, true, out result) ? result : defaultValue;
        }
    }
}