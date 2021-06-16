using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.Algorithms.Sort
{
    public static class BogoSorter
    {
        public static int Sort<T>(List<T> list) where T : IComparable
        {
            int shuffles = 0;
            while (!list.isSorted())
            {
                list.Shuffle();
                shuffles++;
            }
            return shuffles;
        }

        /// <summary>
        /// Shuffle only a sub-set of the entire pack, which are not sorted.
        /// ex. {1,2,7,4,9,80,65,81} shuffle only {4,9,80,65} and merge with the rest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static int BogoVariation<T>(List<T> list) where T : IComparable
        {
            int shuffles = 0;
            List<T> sorted = new List<T>(list);
            sorted.Sort();

            while (!list.isSorted())
            {
                shuffles++;

                int startIndex = 0, endIndex = list.Count - 1;
                bool foundStart = false, foundEnd = false;
                for (int i = 0, j = list.Count - 1; i < list.Count; i++, j--)
                {
                    if (!foundStart && !list[i].Equals(sorted[i]))
                    {
                        foundStart = true;
                        startIndex = i;
                    }
                    if (!foundEnd && !list[j].Equals(sorted[j]))
                    {
                        foundEnd = true;
                        endIndex = j;
                    }
                }

                List<T> range = list.GetRange(startIndex, endIndex - startIndex + 1);
                range.Shuffle();

                for (int i = startIndex, j = 0; i <= endIndex; i++, j++)
                {
                    list[i] = range[j];
                }
            }
            return shuffles;
        }

        private static bool isSorted<T>(this IList<T> list) where T : IComparable
        {
            if (list.Count <= 1)
                return true;
            for (int i = 1; i < list.Count; i++)
                if (list[i].CompareTo(list[i - 1]) < 0) return false;
            return true;
        }

        private static void Shuffle<T>(this IList<T> list)
        {
            Random rand = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int swapIndex = rand.Next(list.Count);
                T temp = list[swapIndex];
                list[swapIndex] = list[i];
                list[i] = temp;
            }
        }
    }
}