using GenericDefs.Classes;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace GenericDefs.Functions.Algorithms.DP
{
    /// <summary>
    /// 0-1 Knapsack pruning
    /// code adapted from
    /// http://stackoverflow.com/questions/8125309/0-1-knapsack-algorithm
    /// </summary>
    public class Knapsack
    {
        /// <summary>
        /// https://brilliant.org/problems/algorithmic-burglar/
        /// </summary>
        /// <param name="args"></param>
        public void Solve()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Action<object> write = Console.Write;

            var sw = new Stopwatch();
            sw.Start();
            write("Running ..\n\n");

            var ks = new KnapsackPruning
            {
                Capacity = 1739,
                Items = new List<Item>
                {
                    new Item {Value = 57, Weight = 2},
                    new Item {Value = 191, Weight = 5},
                    new Item {Value = 417, Weight = 14},
                    new Item {Value = 231, Weight = 17},
                    new Item {Value = 741, Weight = 19},
                    new Item {Value = 139, Weight = 13},
                    new Item {Value = 28, Weight = 1},
                    new Item {Value = 117, Weight = 3},
                    new Item {Value = 13, Weight = 5},
                    new Item {Value = 9, Weight = 11},
                    new Item {Value = 18, Weight = 19}
                }.ToArray()
            };

            var result = ks.Run();

            sw.Stop();

            write(string.Format("Capacity: {0}\n", ks.Capacity));
            write(string.Format("Items: {0}\n", ks.Items.Length));
            write(string.Format("Best value: {0}\n", result.BestValue));
            write("Include:\n");
            result.Include.ForEach(i => write(i + "\n"));

            write(string.Format("\nDuration: {0}\nPress a key to exit\n",
                sw.Elapsed.ToString()));

            Console.ReadKey();
        }

        public class Variation1
        {
            /// <summary>
            /// Distributing M identical objects to N people.
            /// Each person gets atleast x objects.
            /// a1x1 + a2x2 + .... + aNxN = M, where a1, a2, a3, ...., aN are 1.
            /// </summary>
            public static UniqueIntegralPairs Solve(int M, int N, int x, bool IsUnordered = false)
            {
                int[] ai = new int[N];
                int[] xi = new int[N];
                for (int i = 0; i < N; i++)
                {
                    ai[i] = 1;
                }
                UniqueIntegralPairs u = new UniqueIntegralPairs("#");
                for (int i = x; i <= M; i++)
                {
                    xi[0] = i;
                    Recursion(ai, M - i, xi, 1, ref u, x, IsUnordered);
                }

                return u;
            }

            public static BigInteger SolveAndGetCount(int M, int N, int xMin, int xMax)
            {
                int[] ai = new int[N];
                int[] xi = new int[N];
                for (int i = 0; i < N; i++)
                {
                    ai[i] = 1;
                }
                SimpleCounter u = new SimpleCounter();
                for (int i = xMin; i <= xMax; i++)
                {
                    xi[0] = i;
                    Recursion(ai, M - i, xi, 1, ref u, xMin, xMax);
                }

                return u.GetCount();
            }

            /// <summary>
            /// Distributing M objects to N people.
            /// Each person gets atleast xMin objects and not exceeding xMax objects.
            /// x1 + x2 + .... + xN = M.
            /// if (0,1,2) is a unique solution, then you need to find possible arrangements (0,2,1), (1,2,0) etc.
            /// </summary>
            /// <param name="M"></param>
            /// <param name="N"></param>
            /// <param name="x"></param>
            /// <returns></returns>
            public static UniqueArrangements<int> Solve(int M, int N, int xMin, int xMax)
            {
                int[] xi = new int[N];
                UniqueArrangements<int> u = new UniqueArrangements<int>();
                for (int i = xMin; i <= xMax; i++)
                {
                    xi[0] = i;
                    Recursion(M - i, xi, 1, ref u, xMin, xMax);
                }

                return u;
            }

            /// <summary>
            /// Keeps searching recursively for more solutions even after a solution was found.
            /// To find all possible solutions.
            /// </summary>
            /// <param name="ai"></param>
            /// <param name="wLeft"></param>
            /// <param name="xibefore"></param>
            /// <param name="depth"></param>
            /// <param name="u"></param>
            /// <param name="xMin"></param>
            /// <returns></returns>
            internal static void Recursion(int[] ai, int wLeft, int[] xibefore, int depth, ref SimpleCounter u, int xMin, int xMax)
            {
                int wLeftAfter = wLeft;
                int[] xiafter = new int[xibefore.Length];
                Array.Copy(xibefore, xiafter, xibefore.Length);

                for (int i = xMin; i <= xMax; i++)
                {
                    wLeftAfter = wLeft - (i * ai[depth]);
                    xiafter[depth] = i;
                    if (wLeftAfter < 0) { break; }
                    if (depth == ai.Length - 1)
                    {
                        if (wLeftAfter == 0)
                        {
                            u.Increment();
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Recursion(ai, wLeftAfter, xiafter, depth + 1, ref u, xMin, xMax);
                    }
                }
            }

            /// <summary>
            /// Keeps searching recursively for more solutions even after a solution was found.
            /// To find all possible solutions.
            /// </summary>
            /// <param name="ai"></param>
            /// <param name="wLeft"></param>
            /// <param name="xibefore"></param>
            /// <param name="depth"></param>
            /// <param name="u"></param>
            /// <param name="xMin"></param>
            /// <returns></returns>
            internal static void Recursion(int[] ai, int wLeft, int[] xibefore, int depth, ref UniqueIntegralPairs u, int xMin, bool IsUnordered = false)
            {
                int wLeftAfter = wLeft;
                int[] xiafter = new int[xibefore.Length];
                Array.Copy(xibefore, xiafter, xibefore.Length);

                int imax = wLeft / ai[depth];
                for (int i = xMin; i <= imax; i++)
                {
                    wLeftAfter = wLeft - (i * ai[depth]);
                    xiafter[depth] = i;
                    if (wLeftAfter < 0) { break; }
                    if (depth == ai.Length - 1)
                    {
                        if (wLeftAfter == 0)
                        {
                            if (IsUnordered) Array.Sort(xiafter);
                            u.AddCombination(xiafter);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Recursion(ai, wLeftAfter, xiafter, depth + 1, ref u, xMin, IsUnordered);
                    }
                }
            }

            /// <summary>
            /// Keeps searching recursively for more solutions even after a solution was found.
            /// To find all possible solutions.
            /// </summary>
            /// <param name="ai"></param>
            /// <param name="wLeft"></param>
            /// <param name="xibefore"></param>
            /// <param name="depth"></param>
            /// <param name="u"></param>
            /// <param name="xMin"></param>
            /// <returns></returns>
            internal static void Recursion(int wLeft, int[] xibefore, int depth, ref UniqueArrangements<int> u, int xMin, int xMax)
            {
                int wLeftAfter = wLeft;
                int[] xiafter = new int[xibefore.Length];
                Array.Copy(xibefore, xiafter, xibefore.Length);
                int iMax = wLeft < xMax ? wLeft : xMax;
                int maxDepth = xibefore.Count() - 1;
                for (int i = xMin; i <= iMax; i++)
                {
                    wLeftAfter = wLeft - i;
                    xiafter[depth] = i;
                    if (wLeftAfter < 0) { break; }
                    if (depth == maxDepth)
                    {
                        if (wLeftAfter == 0) {
                            List<int> solution = xiafter.Select(x => x).ToList();
                            u.Add(solution);
                            break;
                        } else { continue; }
                    }
                    else { Recursion(wLeftAfter, xiafter, depth + 1, ref u, xMin, xMax); }
                }
            }
        }

        /// <summary>
        /// Use this when depth is not known for Variation1.
        /// </summary>
        public class Variation2
        {
            /// <summary>
            /// Distributing M objects to some(unknown) people.
            /// x1 + x2 + .. + xk = M, where k is variable(not fixed).
            /// </summary>
            public static UniqueArrangements<int> Solve(int M, CustomIterenumerator<int> iterator, bool isDistinct = false)
            {
                UniqueArrangements<int> u = new UniqueArrangements<int>();
                for (int? i = iterator.MoveNext(); iterator.IsValid(); i = iterator.MoveNext())
                {
                    if (i == null) break;
                    iterator.History = null;
                    List<int> xi = new List<int> { i.Value };
                    Recursion(M, xi, ref u, iterator, isDistinct);
                }

                return u;
            }

            public static SimpleCounter SolveCount(int M, CustomIterenumerator<int> iterator)
            {
                SimpleCounter u = new SimpleCounter();
                for (int? i = iterator.MoveNext(); iterator.IsValid(); i = iterator.MoveNext())
                {
                    if (i == null) break;
                    iterator.History = null;
                    List<int> xi = new List<int> { i.Value };
                    RecursionCount(M, xi, ref u, iterator);
                }

                return u;
            }

            internal static void Recursion(int M, List<int> xibefore, ref UniqueArrangements<int> u, CustomIterenumerator<int> iterator, bool isDistinct = false)
            {
                List<int> xiafter = new List<int>();
                CustomIterenumerator<int> localIter = iterator.Clone();
                localIter.History = xibefore.Select(item => item).ToList();
                for (int? i = localIter.MoveNext(); localIter.IsValid(); i = localIter.MoveNext())
                {
                    if (i == null) break;
                    xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i.Value);
                    if (M - xiafter.Sum() < 0)
                    {
                        return;
                    }
                    if (M == xiafter.Sum())
                    {
                        if (isDistinct) xiafter.Sort();
                        u.Add(xiafter);
                        //if (!u.Add(xiafter))
                        //{
                        //    Console.WriteLine("Not added : {0}", string.Join(",", xiafter));
                        //}
                        return;
                    }
                    else
                    {
                        Recursion(M, xiafter, ref u, iterator, isDistinct);
                    }
                }
            }

            internal static void RecursionCount(int M, List<int> xibefore, ref SimpleCounter u, CustomIterenumerator<int> iterator)
            {
                List<int> xiafter = new List<int>();
                CustomIterenumerator<int> localIter = iterator.Clone();
                localIter.History = xibefore.Select(item => item).ToList();
                for (int? i = localIter.MoveNext(); localIter.IsValid(); i = localIter.MoveNext())
                {
                    if (i == null) break;
                    xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i.Value);
                    if (M - xiafter.Sum() < 0)
                    {
                        return;
                    }
                    if (M == xiafter.Sum())
                    {
                        u.Increment();
                        return;
                    }
                    else
                    {
                        RecursionCount(M, xiafter, ref u, iterator);
                    }
                }
            }
        }

        /// <summary>
        /// Maximum possible weight of the sack, of different types of objects, for ex. a of object type1, b of objects type 2 and so on, with
        /// a*w1 + b*w2 + .... l.e.q Constraint(max weight). 
        /// Best possible (a,b,c, ....) closest to max. weight is the optimum weight of the knapsack.
        /// Unique solutions, for when total weight equals MaxWeight.
        /// </summary>
        public class Variation3
        {
            public static double Solve(double[] weights, double MaxWeight)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight / weights[0]);
                IEnumerable<int> range = Enumerable.Range(0, weights.Length);
                Number<double> maxValue = new Number<double>(0);
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xi = new List<int>() { i };
                    Recursion(weights, 1, MaxWeight, xi, maxValue, range);
                }
                return maxValue.Value;
            }

            public static SimpleCounter Solutions(double[] weights, double MaxWeight)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight / weights[0]);
                IEnumerable<int> range = Enumerable.Range(0, weights.Length);
                SimpleCounter counter = new SimpleCounter();
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xi = new List<int>() { i };
                    Recursion(weights, 1, MaxWeight, xi, counter, range);
                }
                return counter;
            }

            public static SimpleCounter Solutions(int[] weights, int MaxWeight, Action<List<int>> Callback = null)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight * 1.0 / weights[0]);
                IEnumerable<int> range = Enumerable.Range(0, weights.Length);
                SimpleCounter counter = new SimpleCounter();
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xi = new List<int>() { i };
                    Recursion(weights, 1, MaxWeight, xi, counter, range, Callback);
                }
                return counter;
            }

            public static UniqueArrangements<int> UniqueSolutions(int[] weights, int MaxWeight)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight * 1.0 / weights[0]);
                UniqueArrangements<int> ua = new UniqueArrangements<int>();
                IEnumerable<int> range = Enumerable.Range(0, weights.Length);
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xi = new List<int>() { i };
                    Recursion(weights, 1, MaxWeight, xi, ua, range);
                }
                return ua;
            }

            internal static void Recursion(double[] weights, int depth, double MaxWeight, List<int> xibefore, Number<double> value, IEnumerable<int> range)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight / weights[depth]);
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i);
                    double sum = 0;
                    range.ForEach(x => { if (x < xiafter.Count) { sum += weights[x] * xiafter[x]; } });
                    if (sum > MaxWeight) { return; }
                    if (depth == weights.Length - 1) {
                        if (sum <= MaxWeight) { value.Value = Math.Max(sum, value.Value); return; }
                        else continue;
                    }
                    else { Recursion(weights, depth + 1, MaxWeight, xiafter, value, range); }
                }
            }

            internal static void Recursion(double[] weights, int depth, double MaxWeight, List<int> xibefore, SimpleCounter counter, IEnumerable<int> range)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight / weights[depth]);
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i);
                    double sum = 0;
                    range.ForEach(x => { if (x < xiafter.Count) { sum += weights[x] * xiafter[x]; } });
                    if (sum > MaxWeight) { return; }
                    if (depth == weights.Length - 1) {
                        if (sum == MaxWeight) { counter.Increment(); return; }
                        else continue;
                    }
                    else { Recursion(weights, depth + 1, MaxWeight, xiafter, counter, range); }
                }
            }

            internal static void Recursion(int[] weights, int depth, int MaxWeight, List<int> xibefore, SimpleCounter counter, IEnumerable<int> range, Action<List<int>> Callback = null)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight * 1.0 / weights[depth]);
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i);
                    double sum = 0;
                    range.ForEach(x => { if (x < xiafter.Count) { sum += weights[x] * xiafter[x]; } });
                    if (sum > MaxWeight) { return; }
                    if (depth == weights.Length - 1)
                    {
                        if (sum == MaxWeight) { counter.Increment(); if (Callback != null) Callback.Invoke(xiafter); return; }
                        else continue;
                    }
                    else { Recursion(weights, depth + 1, MaxWeight, xiafter, counter, range, Callback); }
                }
            }

            internal static void Recursion(int[] weights, int depth, int MaxWeight, List<int> xibefore, UniqueArrangements<int> ua, IEnumerable<int> range)
            {
                int maxVal = (int)Math.Ceiling(MaxWeight * 1.0 / weights[depth]);
                for (int i = 0; i <= maxVal; i++)
                {
                    List<int> xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i);
                    double sum = 0;
                    range.ForEach(x => { if (x < xiafter.Count) { sum += weights[x] * xiafter[x]; } });
                    if (sum > MaxWeight) { return; }
                    if (depth == weights.Length - 1)
                    {
                        if (sum == MaxWeight) {
                            ua.Add(xiafter);
                        }
                        else continue;
                    }
                    else { Recursion(weights, depth + 1, MaxWeight, xiafter, ua, range); }
                }
            }
        }
    }

    public class KnapsackPruning
    {
        public double Capacity;
        public Item[] Items { get; set; }

        double _bestVal;
        bool[] _bestItems;
        int N;

        void Recursive(bool[] chosen, int depth, double w, double v, double remainingVal)
        {
            if (w > Capacity) { return; }
            if (v + remainingVal < _bestVal) { return; }
            if (depth == chosen.Length)
            {
                _bestVal = v;
                Array.Copy(chosen, _bestItems, chosen.Length);
                return;
            }

            remainingVal -= Items[depth].Value;
            chosen[depth] = false;

            Recursive(chosen, depth + 1, w, v, remainingVal);

            chosen[depth] = true;
            w += Items[depth].Weight;
            v += Items[depth].Value;

            Recursive(chosen, depth + 1, w, v, remainingVal);
        }

        public Data Run()
        {
            N = Items.Length;
            var chosen = new bool[N];
            _bestItems = new bool[N];
            _bestVal = 0;

            var totalValue = Items.Sum(i => i.Value);

            Recursive(chosen, 0, 0, 0, totalValue);

            var data = new Data { BestValue = _bestVal };
            for (var i = 0; i < N; i++)
            {
                if (_bestItems[i]) { data.Include.Add(Items[i]); }
            }

            return data;
        }
    }

    public class Item
    {
        public string Description { get; set; }
        private static int _counter;
        public double Id { get; private set; }
        public int Value { get; set; } // value
        public int Weight { get; set; } // weight
        public int Quantity { get; set; }
        public Item() { Id = ++_counter; }
        public Item(string description, int weight, int value, int quantity)
        {
            Id = ++_counter;
            Description = description;
            Value = value;
            Quantity = quantity;
            Weight = weight;
        }
        public override string ToString()
        {
            return string.Format("Id: {0} \tValue: {1} \tWeight: {2}",
                Id, Value, Weight);
        }
    }

    public class ItemCollection
    {
        public Dictionary<string, int> Contents = new Dictionary<string, int>();
        public int TotalValue;
        public int TotalWeight;

        public void AddItem(Item item, int quantity)
        {
            if (Contents.ContainsKey(item.Description)) Contents[item.Description] += quantity; else Contents[item.Description] = quantity;
            TotalValue += quantity * item.Value;
            TotalWeight += quantity * item.Weight;
        }

        public ItemCollection Copy()
        {
            var ic = new ItemCollection();
            ic.Contents = new Dictionary<string, int>(this.Contents);
            ic.TotalValue = this.TotalValue;
            ic.TotalWeight = this.TotalWeight;
            return ic;
        }
    }

    public class Data
    {
        public List<Item> Include = new List<Item>();
        public double BestValue { get; set; }
    }

    public class FrobeniusSolver
    {
        /// <summary>
        /// Gives the largest Frobenius Number N, such that N cannot be expressed in terms of ai's.
        /// N != Summation(ai*xi); May not work!!
        /// </summary>
        /// <returns></returns>
        public static bool FrobeniusNumber(int[] ai, ref int? frobenius)
        {

            int[] bounds = new int[ai.Length];
            Array.Copy(ai, bounds, ai.Length);
            Array.Sort(bounds);
            int uBound = ((bounds[bounds.Length - 1] - 2) * (bounds[bounds.Length - 1] - 2) / 2) - 1;

            int n = uBound;
            bool frobFound = false;
            while (true)
            {
                int[] xi = new int[ai.Length];
                if (!Recursion(ai, n, 0))
                {
                    frobFound = true;
                    break;
                }
                n--;
                if (n == 0)
                {
                    break;
                }
            }
            if (frobFound) frobenius = n;
            return frobFound;
        }

        /// <summary>
        /// Gives the Nth largest Frobenius Number.
        /// Frobenius Number != Summation(ai*xi); May not work!!
        /// </summary>
        /// <returns></returns>
        public static bool NthFrobeniusNumber(long[] ai, ref long? frobenius, int N)
        {
            long[] bounds = new long[ai.Length];
            Array.Copy(ai, bounds, ai.Length);
            Array.Sort(bounds);
            long uBound = ((bounds[bounds.Length - 1] - 2) * (bounds[bounds.Length - 1] - 2) / 2) - 1;

            long n = uBound;
            bool nthfrobFound = false;
            while (true)
            {
                int[] xi = new int[ai.Length];
                if (!Recursion(ai, n, 0))
                {
                    N--;
                    if (N == 0)
                    {
                        nthfrobFound = true;
                        break;
                    }
                }
                n--;
                if (n == 0)
                {
                    break;
                }
            }
            if (nthfrobFound) frobenius = n;
            return nthfrobFound;
        }

        /// <summary>
        /// Gives the Nth largest Frobenius Number.
        /// Frobenius Number != Summation(ai*xi); May not work!!
        /// </summary>
        /// <returns></returns>
        public static bool NthFrobeniusNumber(int[] ai, ref int? frobenius, int N)
        {
            int[] bounds = new int[ai.Length];
            Array.Copy(ai, bounds, ai.Length);
            Array.Sort(bounds);
            int uBound = ((bounds[bounds.Length - 1] - 2) * (bounds[bounds.Length - 1] - 2) / 2) - 1;

            int n = uBound;
            bool nthfrobFound = false;
            while (true)
            {
                int[] xi = new int[ai.Length];
                if (!Recursion(ai, n, 0))
                {
                    N--;
                    if (N == 0)
                    {
                        nthfrobFound = true;
                        break;
                    }
                }
                n--;
                if (n == 0)
                {
                    break;
                }
            }
            if (nthfrobFound) frobenius = n;
            return nthfrobFound;
        }
        internal static bool Recursion(long[] ai, long wLeft, int depth)
        {
            bool found = false;
            long wLeftAfter = wLeft;
            //int[] after = new int[before.Length];
            //Array.Copy(before, after, before.Length);

            long imax = wLeft / ai[depth];
            for (int i = 0; i <= imax; i++)
            {
                wLeftAfter = wLeft - i * ai[depth];
                //after[depth] = i;
                if (wLeftAfter < 0) { found = false; break; }
                if (depth == ai.Length - 1)
                {
                    if (wLeftAfter == 0) { found = true; break; }
                    else { found = false; continue; }
                }
                else { found = Recursion(ai, wLeftAfter, depth + 1); }

                if (found) break;
                if (!found) continue;
            }

            return found;
        }

        internal static bool Recursion(int[] ai, int wLeft, int depth)
        {
            bool found = false;
            int wLeftAfter = wLeft;
            //int[] after = new int[before.Length];
            //Array.Copy(before, after, before.Length);

            int imax = wLeft / ai[depth];
            for (int i = 0; i <= imax; i++)
            {
                wLeftAfter = wLeft - i * ai[depth];
                //after[depth] = i;
                if (wLeftAfter < 0) { found = false; break; }
                if (depth == ai.Length - 1) {
                    if (wLeftAfter == 0) { found = true; break; }
                    else { found = false; continue; }
                } else { found = Recursion(ai, wLeftAfter, depth + 1); }

                if (found) break;
                if (!found) continue;
            }

            return found;
        }
    }
}