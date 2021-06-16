using System;

namespace GenericDefs.Functions.Algorithms.Sort
{
    /// <summary>
    /// The task is to sort an array (or list) elements using the quicksort algorithm. The elements must have a strict weak order and the index of the 
    /// array can be of any discrete type. For languages where this is not possible, sort an array of integers.
    /// Quicksort, also known as partition-exchange sort, uses these steps.
    /// Choose any element of the array to be the pivot. Divide all other elements (except the pivot) into two partitions.
    /// All elements less than the pivot must be in the first partition. All elements greater than the pivot must be in the second partition.
    /// Use recursion to sort both partitions. Join the first sorted partition, the pivot, and the second sorted partition.
    /// The best pivot creates partitions of equal length (or lengths differing by 1). The worst pivot creates an empty partition(for example, if the pivot is 
    /// the first or last element of a sorted array). The runtime of Quicksort ranges from O(n log n) with the best pivots, to O(n2) with the worst pivots, 
    /// where n is the number of elements in the array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuickSort<T> where T : IComparable
    {
        #region Constants
        private const int insertionLimitDefault = 16;
        private const int pivotSamples = 5;
        #endregion

        #region Properties
        public int InsertionLimit { get; set; }
        protected Random Random { get; set; }
        #endregion

        #region Constructors
        public QuickSort() : this(insertionLimitDefault, new Random())
        {
        }

        public QuickSort(int insertionLimit, Random random)
        {
            InsertionLimit = insertionLimit;
            Random = random;
        }
        #endregion

        #region Sort Methods
        public void Sort(T[] entries)
        {
            Sort(entries, 0, entries.Length - 1);
        }

        public void Sort(T[] entries, int first, int last)
        {
            var length = last + 1 - first;
            // Elide tail recursion by looping over the longer partition
            while (length > 1)
            {
                if (length < InsertionLimit)
                {
                    Algorithms.Sort.Sort.InsertionSort(entries, first, last);
                    return;
                }

                var median = Pivot(entries, first, last);

                var left = first;
                var right = last;
                Partition(entries, median, ref left, ref right);

                var leftLength = right + 1 - first;
                var rightLength = last + 1 - left;

                if (leftLength < rightLength) {
                    Sort(entries, first, right);
                    first = left;
                    length = rightLength;
                } else {
                    Sort(entries, left, last);
                    last = right;
                    length = leftLength;
                }
            }
        }

        private T Pivot(T[] entries, int first, int last)
        {
            var length = last + 1 - first;
            var sampleSize = Math.Min(pivotSamples, length);
            var right = first + sampleSize - 1;
            for (var left = first; left <= right; left++)
            {
                // Random sampling avoids pathological cases
                var random = Random.Next(left, last + 1);
                // Sample without replacement
                if (left != random) Swap(entries, left, random);
            }
            Algorithms.Sort.Sort.InsertionSort(entries, first, right);
            return entries[first + sampleSize / 2];
        }

        private static void Partition(T[] entries, T pivot, ref int left, ref int right)
        {
            while (left <= right)
            {
                while (pivot.CompareTo(entries[left]) > 0)
                    left++;                       // pivot follows entry
                while (pivot.CompareTo(entries[right]) < 0)
                    right--;                      // pivot precedes entry

                if (left < right)               // Move entries to their correct partition
                    Swap(entries, left++, right--);
                else if (left == right) {       // No swap needed
                    left++;
                    right--;
                }
            }
        }

        public static void Swap(T[] entries, int index1, int index2)
        {
            var entry = entries[index1];
            entries[index1] = entries[index2];
            entries[index2] = entry;
        }
        #endregion
    }
}