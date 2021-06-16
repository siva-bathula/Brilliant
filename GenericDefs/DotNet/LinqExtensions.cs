using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GenericDefs.DotNet
{
    public static class LinqExtensions
    {
        public static bool IsOrdered<T>(this IList<T> input, OrderByDirection direction)
        {
            IOrderedEnumerable<T> orderedList = null;
            if (direction == OrderByDirection.Ascending) orderedList = input.OrderBy(d => d);
            else if (direction == OrderByDirection.Descending) orderedList = input.OrderByDescending(d => d);

            if (input.SequenceEqual(orderedList)) { return true; }

            return false;
        }

        public static List<T> OrderBy<T>(this IEnumerable<T> input, OrderByDirection direction)
        {
            IOrderedEnumerable<T> orderedList = null;
            if (direction == OrderByDirection.Ascending) orderedList = input.OrderBy(d => d);
            else if (direction == OrderByDirection.Descending) orderedList = input.OrderByDescending(d => d);

            return orderedList.ToList();
        }

        public static IEnumerable<IEnumerable<T>> DivideIntoChunks<T>(this ICollection<T> items, int chunkSize)
        {
            if (chunkSize <= 0 || chunkSize > items.Count)
                throw new ArgumentOutOfRangeException("numberOfChunks");
            if (items.Count == chunkSize)
            {
                yield return items;
                yield break;
            }
            
            int numberOfChunks = items.Count / chunkSize;
            int extra = items.Count % chunkSize;

            for (int i = 0; i < numberOfChunks; i++)
                yield return items.Skip(i * chunkSize).Take(chunkSize);

            if (extra == 0) yield break;

            int alreadyReturnedCount = numberOfChunks * chunkSize;
            int toReturnCount = extra == 0 ? 0 : (items.Count - alreadyReturnedCount);
            
            yield return items.Skip(alreadyReturnedCount).Take(toReturnCount);
        }

        public static IEnumerable<IEnumerable<T>> Section<T>(this IEnumerable<T> source, int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException("length");

            var section = new List<T>(length);

            foreach (var item in source)
            {
                section.Add(item);

                if (section.Count == length)
                {
                    yield return section.AsReadOnly();
                    section = new List<T>(length);
                }
            }

            if (section.Count > 0) yield return section.AsReadOnly();
        }

        public static IEnumerable<IEnumerable<T>> Partition<T> (this IEnumerable<T> source, int size)
        {
            T[] array = null;
            int count = 0;
            foreach (T item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }
                array[count] = item;
                count++;
                if (count == size)
                {
                    yield return new ReadOnlyCollection<T>(array);
                    array = null;
                    count = 0;
                }
            }
            if (array != null)
            {
                Array.Resize(ref array, count);
                yield return new ReadOnlyCollection<T>(array);
            }
        }
    }

    public enum OrderByDirection
    {
        Ascending,
        Descending
    }
}