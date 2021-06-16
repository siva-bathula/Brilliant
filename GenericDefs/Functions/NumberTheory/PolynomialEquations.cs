using System;
using System.Collections.Generic;
using GenericDefs.DotNet;
using Numerics;
using GenericDefs.Classes.NumberTypes;

namespace GenericDefs.Functions.NumberTheory
{
    public class QuadraticEquation
    {
        public class Root
        {
            public double RPlus;
            public double RMinus;
        }

        public static bool Solve(double a, double b, double c, ref Root r)
        {
            double d = (b * b) - (4 * a * c);
            if (d < 0) return false;

            d = Math.Sqrt(d);
            r.RPlus = (d - b) / (2 * a);
            r.RMinus = (-d - b) / (2 * a);
            return true;
        }

        public class NewtonsIdentities
        {
            Dictionary<int, double> Identities = new Dictionary<int, double>();
            public double NthNewtonsIdentities(double a, double b, double c, int N)
            {
                if (Identities.ContainsKey(N)) return Identities[N];

                double retVal = 0;
                if (N == 0) { retVal = 2; }
                else if (N == 1) { retVal = -b / a; }
                else retVal = (-b / a) * NthNewtonsIdentities(a, b, c, N - 1) + (-c / a) * NthNewtonsIdentities(a, b, c, N - 2);

                Identities.GenericAddOrIncrement(N, retVal);

                //Identities.AddOrUpdate(N, retVal);
                return retVal;
            }
        }

        public static double NthNewtonsIdentities(double a, double b, double c, int N, NewtonsIdentities id = null)
        {
            if (id == null) id = new NewtonsIdentities();
            return id.NthNewtonsIdentities(a, b, c, N);
        }
    }

    public class CubicEquation
    {
        public class NewtonsIdentities
        {
            Dictionary<int, double> Identities = new Dictionary<int, double>();
            public double NthNewtonsIdentities(double a, double b, double c, double d, int N)
            {
                if (Identities.ContainsKey(N)) return Identities[N];

                double retVal = 0;
                if (N == 0) { retVal = 3; }
                else if (N == 1) { retVal = -b / a; }
                else if (N == 2) { retVal = (-b / a) * NthNewtonsIdentities(a, b, c, d, 1) + (-c / a) * 2; }
                else retVal = (-b / a) * NthNewtonsIdentities(a, b, c, d, N - 1)
                        + (-c / a) * NthNewtonsIdentities(a, b, c, d, N - 2)
                        + (-d / a) * NthNewtonsIdentities(a, b, c, d, N - 3);
                Identities.AddOrUpdate(N, retVal);
                return retVal;
            }
        }

        public static double NthNewtonsIdentities(double a, double b, double c, double d, int N, NewtonsIdentities id = null)
        {
            if (id == null) id = new NewtonsIdentities();
            return id.NthNewtonsIdentities(a, b, c, d, N);
        }
    }

    public class NthDegreePolynomialEquation
    {
        int N { get; set; }
        double[] Ai { get; set; }
        ElementarySymmetricCoefficients Ei { get; set; }

        public NthDegreePolynomialEquation(int nthDegree, double[] ai)
        {
            N = nthDegree;
            if (ai == null || ai.Length == 0 || ai.Length < N) throw new ArgumentException("Polynomial coefficients are not well defined.");
            Ai = ai;
            Ei = new ElementarySymmetricCoefficients(ai);
        }

        class ElementarySymmetricCoefficients
        {
            public ElementarySymmetricCoefficients(double[] ai) {
                Ai = ai;
                
                GenerateCoefficients();
            }

            void GenerateCoefficients()
            {
                Coefficients.Add(0, Ai.Length);
                int t = 1;
                int sign = 1;
                while (true)
                {
                    sign *= -1;
                    Coefficients.Add(t, sign * Ai[t] / Ai[0]);
                    if (++t > Ai.Length - 1) break;
                }
            }

            double[] Ai { get; set; }
            Dictionary<int, double> Coefficients = new Dictionary<int, double>();
            internal double GetCoefficient(int i)
            {
                if (!Coefficients.ContainsKey(i)) throw new ArgumentException("Coefficients not found in dictionary : " + i);
                return Coefficients[i];
            }
        }

        Dictionary<int, double> Identities = new Dictionary<int, double>();
        double KthNewtonsIdentities(int n)
        {
            if (Identities.ContainsKey(n)) return Identities[n];

            double retVal = 0;
            if (n < N) {
                if (n == 0) { retVal = N; }
                else {
                    int t = 1; int k = n;
                    int sign = 1;
                    while (true) {
                        k--;
                        retVal += sign * Ei.GetCoefficient(t) * (k > 0 ? KthNewtonsIdentities(k) : 1) * (t == n ? t : 1);
                        sign *= -1;
                        if (++t > n) break;
                    }
                }
            } else {
                int t = 1; int k = n;
                int sign = 1;
                while (true) {
                    k--;
                    retVal += sign * Ei.GetCoefficient(t) * KthNewtonsIdentities(k);
                    sign *= -1;
                    if (++t > N) break;
                }
            }
            Identities.AddOrUpdate(n, retVal);
            return retVal;
        }

        public double NewtonsIdentities(int K)
        {
            return KthNewtonsIdentities(K);
        }
    }

    public class NthDegreePolynomialBigRational
    {
        int N { get; set; }
        BigRational[] Ai { get; set; }
        ElementarySymmetricCoefficients Ei { get; set; }

        public NthDegreePolynomialBigRational(int nthDegree, BigRational[] ai)
        {
            N = nthDegree;
            if (ai == null || ai.Length == 0 || ai.Length < N) throw new ArgumentException("Polynomial coefficients are not well defined.");
            Ai = ai;
            Ei = new ElementarySymmetricCoefficients(ai);
        }

        class ElementarySymmetricCoefficients
        {
            public ElementarySymmetricCoefficients(BigRational[] ai)
            {
                Ai = ai;
                GenerateCoefficients();
            }

            void GenerateCoefficients()
            {
                Coefficients.Add(0, Ai.Length);
                int t = 1;
                int sign = 1;
                while (true)
                {
                    sign *= -1;
                    Coefficients.Add(t, sign * Ai[t] / Ai[0]);
                    if (++t > Ai.Length - 1) break;
                }
            }

            BigRational[] Ai { get; set; }
            Dictionary<int, BigRational> Coefficients = new Dictionary<int, BigRational>();
            internal BigRational GetCoefficient(int i)
            {
                if (!Coefficients.ContainsKey(i)) throw new ArgumentException("Coefficients not found in dictionary : " + i);
                return Coefficients[i];
            }
        }

        Dictionary<int, BigRational> Identities = new Dictionary<int, BigRational>();
        BigRational KthNewtonsIdentities(int n)
        {
            if (Identities.ContainsKey(n)) return Identities[n];

            BigRational retVal = 0;
            if (n < N)
            {
                if (n == 0) { retVal = N; }
                else
                {
                    int t = 1; int k = n;
                    int sign = 1;
                    while (true)
                    {
                        k--;
                        retVal += sign * Ei.GetCoefficient(t) * (k > 0 ? KthNewtonsIdentities(k) : 1) * (t == n ? t : 1);
                        sign *= -1;
                        if (++t > n) break;
                    }
                }
            } else {
                int t = 1; int k = n;
                int sign = 1;
                while (true)
                {
                    k--;
                    retVal += sign * Ei.GetCoefficient(t) * KthNewtonsIdentities(k);
                    sign *= -1;
                    if (++t > N) break;
                }
            }
            Identities.GenericAddOrIncrement(n, retVal);
            return retVal;
        }

        public BigRational NewtonsIdentities(int K)
        {
            return KthNewtonsIdentities(K);
        }
    }

    public class NthDegreePolynomialReverseNewtonIdentities
    {
        int N { get; set; }
        Dictionary<int, BigRational> Identities { get; set; }
        ElementarySymmetricCoefficients Ei { get; set; }

        public NthDegreePolynomialReverseNewtonIdentities(int nThDegree, double[] FirstNNewtonIdentities)
        {
            N = nThDegree;

            Identities = new Dictionary<int, BigRational>();
            int n = 0;
            FirstNNewtonIdentities.ForEach(x =>
            {
                Fraction<int> fr = FractionConverter<int>.RealToFraction(x);
                Identities.Add(++n, new BigRational(fr.N, fr.D));

                GC.Collect();
            });

            Ei = new ElementarySymmetricCoefficients(FirstNNewtonIdentities, Identities);
        }

        BigRational KthNewtonsIdentities(int n)
        {
            if (Identities.ContainsKey(n)) return Identities[n];

            BigRational retVal = 0;
            if (n < N)
            {
                if (n == 0) { retVal = N; }
                else
                {
                    int t = 1; int k = n;
                    int sign = 1;
                    while (true)
                    {
                        k--;
                        retVal += sign * Ei.GetCoefficient(t) * (k > 0 ? KthNewtonsIdentities(k) : 1) * (t == n ? t : 1);
                        sign *= -1;
                        if (++t > n) break;
                    }
                }
            }
            else
            {
                int t = 1; int k = n;
                int sign = 1;
                while (true)
                {
                    k--;
                    retVal += sign * Ei.GetCoefficient(t) * KthNewtonsIdentities(k);
                    sign *= -1;
                    if (++t > N) break;
                }
            }
            Identities.GenericAddOrIncrement(n, retVal);
            return retVal;
        }

        public BigRational NewtonsIdentities(int K)
        {
            return KthNewtonsIdentities(K);
        }

        class ElementarySymmetricCoefficients
        {
            Dictionary<int, BigRational> FirstNIdentities { get; set; }
            int N { get; set; }

            internal ElementarySymmetricCoefficients(double[] FirstNNewtonIdentities, Dictionary<int, BigRational> identities)
            {
                FirstNIdentities = new Dictionary<int, BigRational>();
                N = FirstNNewtonIdentities.Length;
                identities.ForEach(x =>
                {
                    FirstNIdentities.Add(x.Key, new BigRational(x.Value.Numerator, x.Value.Denominator));
                });
                GenerateCoefficients();
            }

            void GenerateCoefficients()
            {
                Coefficients.Add(0, N);
                int t = 0;
                while (++t <= N)
                {
                    int tCopy = t;
                    BigRational lhs = new BigRational(t, 1);
                    BigRational rhs = new BigRational(0, 1);
                    rhs -= FirstNIdentities[t];
                    int sign = 1;
                    int n = 0;
                    while (--tCopy > 0)
                    {
                        ++n;
                        rhs += sign * Coefficients[n] * FirstNIdentities[tCopy];
                        sign *= -1;
                    }

                    sign *= -1;
                    Coefficients.Add(t, sign * rhs / lhs);
                }
            }

            Dictionary<int, BigRational> Coefficients = new Dictionary<int, BigRational>();
            internal BigRational GetCoefficient(int i)
            {
                if (!Coefficients.ContainsKey(i)) throw new ArgumentException("Coefficients not found in dictionary : " + i);
                return Coefficients[i];
            }
        }
    }
}