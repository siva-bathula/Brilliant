using GenericDefs.Classes.NumberTypes;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Brilliant.Calculus
{
    public class CalculusCollection : ISolve
    {
        /// <summary>
        /// https://brilliant.org/problems/fibonacci-geometric-progression/
        /// </summary>
        void FibonacciGeometricProgression()
        {
            BigInteger f1 = 1, f2 = 1;
            BigRational sum = BigRational.Divide(f1, 10) + BigRational.Divide(f2, 100);
            BigInteger fi, deno = 100;
            int i = 2;
            while (true)
            {
                fi = f1 + f2;
                i++;
                deno *= 10;
                sum += BigRational.Divide(fi, deno);
                f1 = f2;
                f2 = fi;
                //QueuedConsole.WriteImmediate(string.Format("After {0} iterations, Fraction : {1}", i, sum.Numerator + " / " + sum.Denominator));
                if (i >= 20) break;
            }
            int diff = sum.Numerator.ToString().Length - sum.Denominator.ToString().Length;
            int denLen = sum.Denominator.ToString().Length > 16 ? 16 : sum.Denominator.ToString().Length;
            string denominator = sum.Denominator.ToString().Substring(0, denLen);
            string numerator = sum.Numerator.ToString().Substring(0, denLen + diff);
            double sumFrac = Convert.ToInt64(numerator) * 1.0 / Convert.ToInt64(denominator);
            Fraction<long> frac = FractionConverter<long>.RealToFraction(sumFrac, 0.000000000001);
            QueuedConsole.WriteImmediate("Decimal Value : " + sumFrac);
            QueuedConsole.WriteFinalAnswer("Fractional Value : " + frac.N + " / " + frac.D);
        }

        /// <summary>
        /// https://brilliant.org/problems/some-harmonic-sum/
        /// </summary>
        void SomeHarmonicSum()
        {
            int n = 0;
            double Hn = 0.0;
            double invPow2 = 1.0 / 2;
            double sum = 0.0;
            while(++n < 100)
            {
                Hn += 1.0 / n;
                invPow2 /= 2;
                sum += Hn * invPow2 / (n + 1);
            }
            QueuedConsole.WriteImmediate("Sum : {0}", sum);
        }

        /// <summary>
        /// https://brilliant.org/problems/inspired-by-pi-han-goh-11/
        /// </summary>
        void AbelSum()
        {
            long n = 0, nmax = 2000;
            BigRational sum = new BigRational(0, 1);
            BigRational x = new BigRational(999999999999, 1000000000000);
            BigRational xpow = new BigRational(1, 1);
            int sign = -1;
            while (++n <= nmax)
            {
                xpow *= x;
                sign *= -1;
                sum += sign * n * n * xpow;
                if (n % 100 == 0)
                {
                    double s = NumberConverter.BigRationalToDouble(sum);
                    QueuedConsole.WriteImmediate("Sum at n = {0} is : {1}", n, s);
                }
            }

            double sumVal = NumberConverter.BigRationalToDouble(sum);
            QueuedConsole.WriteImmediate("Sum at n = {0} is : {1}", nmax, sumVal);
        }

        /// <summary>
        /// https://brilliant.org/problems/algebra-calculusalgebrus/
        /// </summary>
        void CalculusAlgebrus()
        {
            BigRational sum = new BigRational(0, 1);

            long n = -1;
            while (++n <= 10000)
            {
                sum += new BigRational(1, (3 * n + 1) * (3 * n + 2));
            }
            double val = NumberConverter.BigRationalToDouble(sum);
            QueuedConsole.WriteImmediate("Sum : {0}", val);
        }

        /// <summary>
        /// https://brilliant.org/problems/sum-them-all-up3/
        /// </summary>
        void SumThemAllUp()
        {
            BigRational sum = new BigRational(0, 1);
            long n = 1;
            ClonedPrimes p = KnownPrimes.CloneKnownPrimes(0, 10000);
            while (++n <= 20000)
            {
                if(Factors.GetAllFactorsCount(n, p) % 2 == 0)
                {
                    sum += new BigRational(1, n * n);
                }
            }

            double val = NumberConverter.BigRationalToDouble(sum);
            QueuedConsole.WriteImmediate("Sum : {0}", val);
        }

        /// <summary>
        /// https://brilliant.org/practice/root-approximation-bisection/
        /// </summary>
        void RootApproximationBisection()
        {
            double _5cuberoot = Math.Pow(5, (1.0 / 3.0));
            Func<double, double> Function = delegate (double x)
            {
                double value = x - _5cuberoot;
                return value;
            };

            double xLower = 1.0, xUpper = 2.0;
            double fnLower = Function(xLower), fnUpper = Function(xUpper);
            int nBisections = 0;

            while (++nBisections >= 0)
            {
                double x = (xLower + xUpper) / 2;
                double fx = Function(x);

                if (fx < 0) { xLower = x; }
                else { xUpper = x; }

                double width = xUpper - xLower;
                if (width <= 0.02) break;
            }

            QueuedConsole.WriteImmediate("[{0},{1}]", xLower, xUpper);
        }
        
        void NumericalApprox()
        {
            double pow = 1.0 / 22.0;
            int n = 1886;

            double s1 = 0.0, s2 = 0.0;
            while(++n <= 2015)
            {
                double root = Math.Pow(n, pow);
                if (n < 2015) s1 += root;
                if (n > 1887) s2 += root;
            }
            pow += 1;
            double integral = 1.0 / pow;
            integral *= (Math.Pow(2015, pow) - Math.Pow(1887, pow));

            if (integral > s1) { QueuedConsole.WriteImmediate("I > S1"); }
            else { QueuedConsole.WriteImmediate("I < S1"); }

            if (integral > s2) { QueuedConsole.WriteImmediate("I > S2"); }
            else { QueuedConsole.WriteImmediate("I < S2"); }

            if (s1 > s2) { QueuedConsole.WriteImmediate("S1 > S2"); }
            else { QueuedConsole.WriteImmediate("S2 > S1"); }
        }

        void BisectionTest()
        {
            Func<double, double> f = delegate (double x)
            {
                double fx = 1 - (2 * x);
                fx += Math.Pow((x * (x - 1)), 2);
                return fx;
            };

            GenericDefs.Functions.Calculus.BisectionRootApproximation b = new GenericDefs.Functions.Calculus.BisectionRootApproximation(f, 1, 3);
            QueuedConsole.WriteImmediate("x : {0}", b.Root);
        }

        void ISolve.Init()
        {

        }

        void ISolve.Solve()
        {
            BisectionTest();
        }
    }
}