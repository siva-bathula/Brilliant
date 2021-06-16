using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDefs.Functions.Algorithms.Sort
{
    public static class Sort
    {
        /// <summary>
        /// The bubble sort is generally considered to be the simplest sorting algorithm. Because of its simplicity and ease of visualization, it is often taught in 
        /// introductory computer science courses. 
        /// Because of its abysmal O(n2) performance, it is not used often for large (or even medium-sized) datasets.
        /// The bubble sort works by passing sequentially over a list, comparing each value to the one immediately after it. 
        /// If the first value is greater than the second, their positions are switched. Over a number of passes, at most equal to the number of elements in the list, all of the values drift 
        /// into their correct positions (large values "bubble" rapidly toward the end, pushing others down around them). Because each pass finds the maximum item and puts it at the end, 
        /// the portion of the list to be sorted can be reduced at each pass. A boolean variable is used to track whether any changes have been made in the current pass; when a pass 
        /// completes without changing anything, the algorithm exits.
        /// https://rosettacode.org/wiki/Sorting_algorithms/Bubble_sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void BubbleSort<T>(this List<T> list) where T : IComparable
        {
            bool madeChanges;
            int itemCount = list.Count;
            do
            {
                madeChanges = false;
                itemCount--;
                for (int i = 0; i < itemCount; i++)
                {
                    if (list[i].CompareTo(list[i + 1]) > 0)
                    {
                        T temp = list[i + 1];
                        list[i + 1] = list[i];
                        list[i] = temp;
                        madeChanges = true;
                    }
                }
            } while (madeChanges);
        }

        /// <summary>
        /// First find the smallest element in the array and exchange it with the element in the first position, then find the second smallest element and exchange it 
        /// with the element in the second position, and continue in this way until the entire array is sorted. Its asymptotic complexity is O(n2) making it inefficient 
        /// on large arrays. Its primary purpose is for when writing data is very expensive (slow) when compared to reading, eg. writing to flash memory or EEPROM. 
        /// No other sorting algorithm has less data movement.
        /// </summary>
        public static T[] SelectionSort<T>(T[] list) where T : IComparable
        {
            int k;
            T temp;

            for (int i = 0; i < list.Length; i++)
            {
                k = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j].CompareTo(list[k]) < 0)
                    {
                        k = j;
                    }
                }
                temp = list[i];
                list[i] = list[k];
                list[k] = temp;
            }

            return list;
        }

        /// <summary>
        /// An O(n2) sorting algorithm which moves elements one at a time into the correct position. The algorithm consists of inserting one element at a time into the 
        /// previously sorted part of the array, moving higher ranked elements up as necessary. To start off, the first (or smallest, or any arbitrary) element of the unsorted 
        /// array is considered to be the sorted part. Although insertion sort is an O(n2) algorithm, its simplicity, low overhead, good locality of reference and efficiency 
        /// make it a good choice in two cases: (i) small n, (ii) as the final finishing - off algorithm for O(n logn) algorithms such as mergesort and quicksort.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entries"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        public static void InsertionSort<T>(T[] entries, int first, int last) where T : IComparable
        {
            for (var i = first + 1; i <= last; i++)
            {
                var entry = entries[i];
                var j = i;
                while (j > first && entries[j - 1].CompareTo(entry) > 0)
                    entries[j] = entries[--j];
                entries[j] = entry;
            }
        }

        /// <summary>
        /// Heapsort is an in-place sorting algorithm with worst case and average complexity of O(n logn).
        /// The basic idea is to turn the array into a binary heap structure, which has the property that it allows efficient retrieval and removal of the maximal element. 
        /// We repeatedly "remove" the maximal element from the heap, thus building the sorted list from back to front. Heapsort requires random access, so can only be used on an 
        /// array-like data structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void HeapSort<T>(T[] array)
        {
            HeapSort(array, 0, array.Length, Comparer<T>.Default);
        }

        /// <summary>
        /// Heapsort is an in-place sorting algorithm with worst case and average complexity of O(n logn).
        /// The basic idea is to turn the array into a binary heap structure, which has the property that it allows efficient retrieval and removal of the maximal element. 
        /// We repeatedly "remove" the maximal element from the heap, thus building the sorted list from back to front. Heapsort requires random access, so can only be used on an 
        /// array-like data structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="comparer"></param>
        public static void HeapSort<T>(T[] array, int offset, int length, IComparer<T> comparer)
        {
            HeapSort(array, offset, length, comparer.Compare);
        }

        /// <summary>
        /// Heapsort is an in-place sorting algorithm with worst case and average complexity of O(n logn).
        /// The basic idea is to turn the array into a binary heap structure, which has the property that it allows efficient retrieval and removal of the maximal element. 
        /// We repeatedly "remove" the maximal element from the heap, thus building the sorted list from back to front. Heapsort requires random access, so can only be used on an 
        /// array-like data structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="comparison"></param>
        public static void HeapSort<T>(T[] array, int offset, int length, Comparison<T> comparison)
        {
            // build binary heap from all items
            for (int i = 0; i < length; i++)
            {
                int index = i;
                T item = array[offset + i]; // use next item

                // and move it on top, if greater than parent
                while (index > 0 && comparison(array[offset + (index - 1) / 2], item) < 0)
                {
                    int top = (index - 1) / 2;
                    array[offset + index] = array[offset + top];
                    index = top;
                }
                array[offset + index] = item;
            }

            for (int i = length - 1; i > 0; i--)
            {
                // delete max and place it as last
                T last = array[offset + i];
                array[offset + i] = array[offset];

                int index = 0;
                // the last one positioned in the heap
                while (index * 2 + 1 < i)
                {
                    int left = index * 2 + 1, right = left + 1;
                    if (right < i && comparison(array[offset + left], array[offset + right]) < 0)
                    {
                        if (comparison(last, array[offset + right]) > 0) break;

                        array[offset + index] = array[offset + right];
                        index = right;
                    }
                    else
                    {
                        if (comparison(last, array[offset + left]) > 0) break;

                        array[offset + index] = array[offset + left];
                        index = left;
                    }
                }
                array[offset + index] = last;
            }
        }
    }

    /// <summary>
    /// The merge sort is a recursive sort of order n*log(n).
    /// It is notable for having a worst case and average complexity of O(n* log(n)), and a best case complexity of O(n) (for pre-sorted input). 
    /// The basic idea is to split the collection into smaller groups by halving it until the groups only have one element or no elements(which are both entirely sorted groups). 
    /// Then merge the groups back together so that their elements are in order.This is how the algorithm gets its "divide and conquer" description.
    /// Write a function to sort a collection of integers using the merge sort.The merge sort algorithm comes in two parts: a sort function and a merge function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MergeSort<T> where T : IComparable
    {
        #region Constants
        private const int mergesDefault = 6;
        private const int insertionLimitDefault = 12;
        #endregion

        #region Properties
        protected int[] Positions { get; set; }

        private int merges;
        public int Merges
        {
            get { return merges; }
            set
            {
                // A minimum of 2 merges are required
                if (value > 1) merges = value;
                else throw new ArgumentOutOfRangeException();

                if (Positions == null || Positions.Length != merges) Positions = new int[merges];
            }
        }

        public int InsertionLimit { get; set; }
        #endregion

        #region Constructors
        public MergeSort(int merges, int insertionLimit)
        {
            Merges = merges;
            InsertionLimit = insertionLimit;
        }

        public MergeSort()
          : this(mergesDefault, insertionLimitDefault)
        {
        }
        #endregion

        #region Sort Methods
        public void Sort(T[] entries)
        {
            // Allocate merge buffer
            var entries2 = new T[entries.Length];
            Sort(entries, entries2, 0, entries.Length - 1);
        }

        // Top-Down K-way Merge Sort
        public void Sort(T[] entries1, T[] entries2, int first, int last)
        {
            var length = last + 1 - first;
            if (length < 2)
                return;
            else if (length < InsertionLimit)
            {
                Algorithms.Sort.Sort.InsertionSort(entries1, first, last);
                return;
            }

            var left = first;
            var size = ceiling(length, Merges);
            for (var remaining = length; remaining > 0; remaining -= size, left += size)
            {
                var right = left + Math.Min(remaining, size) - 1;
                Sort(entries1, entries2, left, right);
            }

            Merge(entries1, entries2, first, last);
            Array.Copy(entries2, first, entries1, first, length);
        }
        #endregion

        #region Merge Methods
        public void Merge(T[] entries1, T[] entries2, int first, int last)
        {
            Array.Clear(Positions, 0, Merges);
            // This implementation has a quadratic time dependency on the number of merges
            for (var index = first; index <= last; index++)
                entries2[index] = remove(entries1, first, last);
        }

        private T remove(T[] entries, int first, int last)
        {
            var entry = default(T);
            var found = (int?)null;
            var length = last + 1 - first;

            var index = 0;
            var left = first;
            var size = ceiling(length, Merges);
            for (var remaining = length; remaining > 0; remaining -= size, left += size, index++)
            {
                var position = Positions[index];
                if (position < Math.Min(remaining, size))
                {
                    var next = entries[left + position];
                    if (!found.HasValue || entry.CompareTo(next) > 0)
                    {
                        found = index;
                        entry = next;
                    }
                }
            }

            // Remove entry
            Positions[found.Value]++;
            return entry;
        }
        #endregion

        #region Math Methods
        private static int ceiling(int numerator, int denominator)
        {
            return (numerator + denominator - 1) / denominator;
        }
        #endregion
    }

    public class MergeSortAlgorithm
    {
        public int NComparisons { get; set; }
        
        public int[] MergeSort(int[] array)
        {
            // If list size is 0 (empty) or 1, consider it sorted and return it
            // (using less than or equal prevents infinite recursion for a zero length array).
            if (array.Length <= 1)
            {
                return array;
            }

            // Else list size is > 1, so split the list into two sublists.
            int middleIndex = (array.Length) / 2;
            int[] left = new int[middleIndex];
            int[] right = new int[array.Length - middleIndex];

            Array.Copy(array, left, middleIndex);
            Array.Copy(array, middleIndex, right, 0, right.Length);

            // Recursively call MergeSort() to further split each sublist
            // until sublist size is 1.
            left = MergeSort(left);
            right = MergeSort(right);

            // Merge the sublists returned from prior calls to MergeSort()
            // and return the resulting merged sublist.
            return Merge(left, right);
        }

        int[] Merge(int[] left, int[] right)
        {
            // Convert the input arrays to lists, which gives more flexibility 
            // and the option to resize the arrays dynamically.
            List<int> leftList = left.OfType<int>().ToList();
            List<int> rightList = right.OfType<int>().ToList();
            List<int> resultList = new List<int>();

            // While the sublist are not empty merge them repeatedly to produce new sublists 
            // until there is only 1 sublist remaining. This will be the sorted list.
            while (leftList.Count > 0 || rightList.Count > 0)
            {
                NComparisons++;
                if (leftList.Count > 0 && rightList.Count > 0)
                {
                    // Compare the 2 lists, append the smaller element to the result list
                    // and remove it from the original list.
                    if (leftList[0] <= rightList[0])
                    {
                        resultList.Add(leftList[0]);
                        leftList.RemoveAt(0);
                    }

                    else
                    {
                        resultList.Add(rightList[0]);
                        rightList.RemoveAt(0);
                    }
                }

                else if (leftList.Count > 0)
                {
                    resultList.Add(leftList[0]);
                    leftList.RemoveAt(0);
                }

                else if (rightList.Count > 0)
                {
                    resultList.Add(rightList[0]);
                    rightList.RemoveAt(0);
                }
            }

            // Convert the resulting list back to a static array
            int[] result = resultList.ToArray();
            return result;
        }
    }
}