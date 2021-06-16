using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using System.Numerics;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.DotNet;
using System;

namespace GenericDefs.Classes
{
    public class Permutations
    {
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> QuickPerm<T>(IEnumerable<T> set, string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter)) { delimiter = "@#$"; }
            int N = set.Count();
            int[] a = new int[N];
            int[] p = new int[N];

            var yieldRet = new T[N];

            List<T> list = new List<T>(set);

            int i, j, tmp; // Upper Index i; Lower Index j

            for (i = 0; i < N; i++)
            {
                // initialize arrays; a[N] can be any type
                a[i] = i + 1; // a[i] value is not revealed and can be arbitrary
                p[i] = 0; // p[i] == i controls iteration and index boundaries for i
            }
            yield return list;
            //display(a, 0, 0);   // remove comment to display array a[]
            i = 1; // setup first swap points to be 1 and 0 respectively (i & j)
            while (i < N)
            {
                if (p[i] < i)
                {
                    j = i % 2 * p[i]; // IF i is odd then j = p[i] otherwise j = 0
                    tmp = a[j]; // swap(a[j], a[i])
                    a[j] = a[i];
                    a[i] = tmp;

                    //MAIN!

                    for (int x = 0; x < N; x++)
                    {
                        yieldRet[x] = list[a[x] - 1];
                    }
                    yield return yieldRet;
                    //display(a, j, i); // remove comment to display target array a[]

                    // MAIN!

                    p[i]++; // increase index "weight" for i by one
                    i = 1; // reset index i to 1 (assumed)
                }
                else
                {
                    // otherwise p[i] == i
                    p[i] = 0; // reset p[i] to zero
                    i++; // set new index value for i (increase by one)
                } // if (p[i] < i)
            } // while(i < N)
        }

        public static long GetPermutationsCount<T>(IEnumerable<T> list, int length)
        {
            return (new Permutations<T>(list.ToList(), GenerateOption.WithoutRepetition)).Count;
        }

        public static List<IList<T>> GetPermutationsWithoutRepetition<T>(IEnumerable<T> list, int length)
        {
            return (new Permutations<T>(list.ToList(), GenerateOption.WithoutRepetition)).ToList();
        }

        public static IEnumerable<T> RandomPermutation<T>(IEnumerable<T> set)
        {
            Random rnd = new Random();
            return set.OrderBy(x => rnd.Next());
        }
    }

    public class PermutationsAndCombinations
    {
        public static BigInteger DerangementsBig(int n)
        {
            if (n == 0) return new BigInteger(1);
            if (n == 1) return new BigInteger(0);

            BigInteger retVal = 0;
            BigInteger fN = factorialBig(n);
            int sign = -1;
            Enumerable.Range(2, n - 1).ForEach(a =>
            {
                sign *= -1;
                retVal += (sign) * (fN / factorialBig(a));
            });
            return retVal;
        }

        public static int Derangements(int n)
        {
            if (n == 0) return 1;
            if (n == 1) return 0;

            int retVal = 0;
            int fN = (int)Factorial(n);
            int sign = -1;
            Enumerable.Range(2, n - 1).ForEach(a =>
            {
                sign *= -1;
                retVal += (sign) * (fN / (int)factorialBig(a));
            });
            return retVal;
        }

        public static long nCr(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(r) * Factorial(n - r);
            return nPr(n, r) / factorial(r);
        }

        public static BigInteger nCrBig(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(r) * Factorial(n - r);
            return nPrBig(n, r) / factorialBig(r);
        }

        public static BigInteger nPrBig(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivisionBig(n, n - r);
        }

        public static long nPr(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }

        private static long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static BigInteger FactorialDivisionBig(int topFactorial, int divisorFactorial)
        {
            BigInteger result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static long factorial(int i)
        {
            if (i <= 1) return 1;
            return i * factorial(i - 1);
        }

        private static BigInteger factorialBig(int i)
        {
            if (i <= 1) return 1;
            return i * factorialBig(i - 1);
        }

        public static BigInteger Factorial(int i)
        {
            if (i <= 1) return 1;
            return i * Factorial(i - 1);
        }

        public static RecursionResult<BigInteger> Factorial(int n, BigInteger product)
        {
            if (n < 2) return TailRecursion.Return(product);
            return TailRecursion.Next(() => Factorial(n - 1, n * product));
        }
    }

    public class Probability<T> where T : struct, IConvertible
    {
        Fraction<T> Fr { get; set; }
        Number<T> Favorable { get; set; }
        Number<T> Total { get; set; }

        bool _isCounter = false;
        public Probability() {
            _isCounter = true;
            Favorable = new Number<T>(default(T));
            Total = new Number<T>(default(T));
        }

        public Probability(T favorable, T total)
        {
            _isCounter = true;
            Favorable = new Number<T>(favorable);
            Total = new Number<T>(total);
            Fr = new Fraction<T>(favorable, total, false);
        }

        /// <summary>
        /// Donot call in conjunction with JustIncrement(). Internally calls JustIncrement().
        /// </summary>
        public void IncrementFavorable() { Favorable.Increment(); Total.Increment(); }
        public void JustIncrement() { Total.Increment(); }
        
        public double ToDouble() {
            if (_isCounter) {
                if (Total.Value.Equals(default(T))) throw new ArgumentException("0/0 is invalid");
                Fr = new Fraction<T>(Favorable.Value, Total.Value, false);
            }
            return Fr.GetValue();
        }

        /// <summary>
        /// Returns the string representation of the probability being calculated.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_isCounter)
            {
                if (Total.Value.Equals(default(T))) throw new ArgumentException("0/0 is invalid");
                string output = string.Empty;
                output += Favorable.Value.ToString() + " / " + Total.Value.ToString();
                output += Environment.NewLine;
                output += "Fully reduced fraction A/B = ";
                Fr = new Fraction<T>(Favorable.Value, Total.Value, false);
                output += Fr.ToString();
                output += Environment.NewLine;
                output += "B - A = " + (Fr.D.ToInt64(null) - Fr.N.ToInt64(null)).ToString();

                return output;
            }
            return Fr.ToString();
        }
    }
}