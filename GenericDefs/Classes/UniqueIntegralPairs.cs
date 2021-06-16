using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Classes
{
    /// <summary>UniqueIntegralPairs
    /// (0,1,2) (0,2,1) (1,2,0) (1,0,2) (2,1,0) (2,0,1) are unique pairs.
    /// </summary>
    public class UniqueIntegralPairs
    {
        HashSet<string> Set = new HashSet<string>();
        internal List<Combination> Combinations;
        internal string Delimiter;
        public UniqueIntegralPairs()
        {
            Combinations = new List<Combination>();
        }
        public UniqueIntegralPairs(string delimiter="#")
        {
            Combinations = new List<Combination>();
            Delimiter = delimiter;
        }

        /// <summary>
        /// Checks if this combination is already added to the set.
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public bool ValidCombination(ArrayList l)
        {
            if (Set.Count == 0) return true;
            string key = GetHashKey(l);

            if (Set.Contains(key)) return false;
            return true;
        }

        public bool ValidCombination(int[] l)
        {
            if (Set.Count == 0) return true;
            string key = GetHashKey(l);

            if (Set.Contains(key)) return false;
            return true;
        }

        /// <summary>
        /// Adds the combination l to the existing set if its a valid/unique one(Not already added).
        /// </summary>
        /// <param name="l">The arraylist of objects.</param>
        public bool AddCombination(ArrayList l)
        {
            if (ValidCombination(l))
            {
                string hash = GetHashKey(l);
                Combination c = new Combination() { Pair = l.OfType<int>().ToArray() };
                Combinations.Add(c);
                Set.Add(hash);

                return true;
            }
            return false;
        }

        public bool AddCombination(int[] l)
        {
            if (ValidCombination(l))
            {
                string hash = GetHashKey(l);
                Combination c = new Combination() { Pair = l };
                Combinations.Add(c);
                Set.Add(hash);

                return true;
            }
            return false;
        }

        public int Count() {
            return Combinations.Count;
        }

        public List<Combination> GetCombinations()
        {
            return Combinations;
        }

        private string GetHashKey(ArrayList l)
        {
            string key = string.Empty;
            foreach (var x in l)
            {
                key += x + Delimiter;
            }

            return key;
        }

        private string GetHashKey(int[] l)
        {
            string key = string.Empty;
            foreach (var x in l)
            {
                key += x + Delimiter;
            }

            return key;
        }

        internal void RemoveAllCombinations() {
            Set.Clear();
            Combinations.Clear();
        }

        public class Combination
        {
            public int[] Pair;
        }
    }

    public class WeightedUniqueIntegralPairs<T> : UniqueIntegralPairs
    {
        public T MaximumCalculatedValue { get; set; }
        public WeightedUniqueIntegralPairs() : base()
        {

        }
        public WeightedUniqueIntegralPairs(string delimiter) : base(delimiter)
        {

        }

        public new void RemoveAllCombinations() {
            base.RemoveAllCombinations();
        }
    }
}