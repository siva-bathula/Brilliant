using GenericDefs.DotNet;
using GenericDefs.Functions;
using Numerics;
using System.Numerics;
using System;
using GenericDefs.Classes.NumberTypes;
using System.Collections.Generic;

namespace Brilliant.NumberTheory
{
    public class RiemannZeta : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/wiki/riemann-zeta-function/");
        }

        void ISolve.Solve()
        {
            ExploringDivisorFunction.Function1();
        }

        /// <summary>
        /// https://brilliant.org/problems/mystical-summation-that-requires-explaination/
        /// </summary>
        internal static void MysticalSummation()
        {
            int n = 1;
            int nMax = 100000;
            BigRational diffVal = new BigRational(0, 1);
            while (true)
            {
                int fCount = Factors.GetAllFactorsCount(n);
                if (fCount < 8)
                {
                    BigInteger deno = n;
                    deno *= n;
                    BigRational a = new BigRational(1, deno);
                    diffVal += a;
                }
                n++;
                if (n > nMax) break;
            }

            Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(diffVal);
            double value = fr.N * 1.0 / fr.D;
            double Avalue = Math.PI * Math.PI / 6.0 - value;
            QueuedConsole.WriteImmediate("A : {0}, Floor(10000*A) : {1}", Avalue, Math.Floor(10000 * Avalue));
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/ordinary-problem/
        /// </summary>
        internal static void OrdinaryProblem()
        {
            int n = 1;
            int nMax = 10000;
            BigRational S = new BigRational(0, 1);
            while (true)
            {
                HashSet<long> factors = Factors.GetAllFactors(n);
                int nC = 0;
                BigInteger deno = 1;
                while (true)
                {
                    deno *= n;
                    nC++;
                    if (nC == 8) break;
                }
                BigInteger nume = 0;
                foreach (long t in factors)
                {
                    nC = 0;
                    BigInteger sigmaAdd = 1;
                    while (true)
                    {
                        sigmaAdd *= t;
                        nC++;
                        if (nC == 4) break;
                    }
                    nume += sigmaAdd;
                }
                BigRational a = new BigRational(nume, deno);
                S += a;
                n++;
                if (n > nMax) break;
            }

            Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(S);
            double frValue = fr.N * 1.0 / fr.D;
            double value = Math.Pow(Math.PI, 12) / frValue;
            QueuedConsole.WriteImmediate("n : {0}, pi^12/S : {1}", n - 1, value);
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/zeta-gcd/
        /// </summary>
        static void ZetaGCD()
        {
            BigRational sum = new BigRational(0, 1);
            long n = 1, nMax = 50000;
            HashSet<long> f2016 = new HashSet<long>(Factors.GetPrimeFactors(2016));
            while (true)
            {
                if (Factors.IsGCDOne(n, f2016))
                {
                    BigInteger npow4 = 1;
                    int count = 0;
                    while (true) { npow4 *= n; count++; if (count == 4) break; }
                    sum += new BigRational(1, npow4);
                }
                n++;
                if (n > nMax) break;
            }
            Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(sum);
            double abyb = fr.N * 1.0 / (Math.Pow(Math.PI, 4) * fr.D);
            Fraction<long> ab = FractionConverter<long>.RealToFraction(abyb, 0.0000000001);
            QueuedConsole.WriteImmediate("a : {0}, b: {1}, c: {2}, a+b+c: {3}", ab.N, ab.D, 4, ab.N + ab.D + 4);
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        internal class ExploringDivisorFunction
        {
            /// <summary>
            /// https://brilliant.org/problems/exploring-divisor-function-1/
            /// </summary>
            internal static void Function1()
            {
                BigRational sum = new BigRational(0, 1);
                long n = 1, nMax = 50000;
                ClonedPrimes cPrimes = KnownPrimes.CloneKnownPrimes(1, 50000);
                while (true)
                {
                    BigInteger npow2 = n;
                    npow2 *= n;
                    if (n % 2017 == 0) sum += new BigRational(Factors.GetAllFactorsCount(2017 * n, cPrimes), npow2);
                    else sum += new BigRational(2 * Factors.GetAllFactorsCount(n), npow2);
                    n++;
                    if (n > nMax) break;
                }
                Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(sum);
                double abyb = fr.N * 1.0 / (Math.Pow(Math.PI, 4) * fr.D);
                Fraction<long> ab = FractionConverter<long>.RealToFraction(abyb, 0.0000000001);
                QueuedConsole.WriteImmediate("a : {0}, b: {1}, c: {2}, a+b+c: {3}", ab.N, 4, ab.D, ab.N + ab.D + 4);
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/exploring-divisor-function-2/
            /// </summary>
            internal static void Function2()
            {
                BigRational sum = new BigRational(0, 1);
                long n = 1, nMax = 50000;
                while (true)
                {
                    BigInteger npow2 = 1;
                    int count = 0;
                    while (true) { npow2 *= n; count++; if (count == 2) break; }

                    sum += new BigRational(Factors.GetAllFactorsCount(2015 * n), npow2);
                    n++;
                    if (n > nMax) break;
                }
                Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(sum);
                double abyb = fr.N * 1.0 / (Math.Pow(Math.PI, 2) * fr.D);
                Fraction<long> ab = FractionConverter<long>.RealToFraction(abyb, 0.0000000001);
                QueuedConsole.WriteImmediate("a : {0}, b: {1}, c: {2}, a+b+c: {3}", ab.N, 2, ab.D, ab.N + ab.D + 2);
                QueuedConsole.ReadKey();
            }
        }
    }
}