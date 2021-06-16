using Numerics;
using System;
using System.Linq;

namespace GenericDefs.Functions.NumberTheory
{
    public class BernoulliNumbers
    {
        private static readonly Func<int, BigRational> FromInt = delegate(int s) { return new BigRational(s, 1); };
        private static BigRational CalculateBernoulli(int n)
        {
            var a = InitializeArray(n);
            foreach (var m in Enumerable.Range(1, n))
            {
                a[m] = FromInt(1) / (FromInt(m) + FromInt(1));
                for (var j = m; j >= 1; j--)
                {
                    a[j - 1] = FromInt(j) * (a[j - 1] - a[j]);
                }
            }

            return a[0];
        }

        private static BigRational[] InitializeArray(int n)
        {
            var a = new BigRational[n + 1];
            for (var x = 0; x < a.Length; x++)
            {
                a[x] = FromInt(x + 1);
            }
            return a;
        }

        public static void TestGenerate(int k)
        {
            Enumerable.Range(0, k + 1)
                .Select(n => new { N = n, BernoulliNumber = CalculateBernoulli(n) })
                //.Where(b => !b.BernoulliNumber.Numerator.IsZero)
                .Select(b => string.Format("B({0, 2}) = {1} / {2}", b.N, b.BernoulliNumber.Numerator, b.BernoulliNumber.Denominator))
                .ToList()
                .ForEach(Console.WriteLine);
        }

        public static Tuple<int, BigRational>[] Generate(int k)
        {
            return Enumerable.Range(0, k + 1)
                .Select(n => new Tuple<int, BigRational>(n, CalculateBernoulli(n))).ToArray();
        }
    }
}