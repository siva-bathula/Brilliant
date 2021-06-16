using GenericDefs.Functions;
using Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace GenericDefs.Classes.NumberTypes
{
    public interface IFractionValue
    {
        double GetValue();
    }

    public class BigIntFraction : IFractionValue
    {
        /// <summary>
        /// Will simplify the fraction so gcd of numerator and denominator is 1.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="issimplified"></param>
        public BigIntFraction(BigInteger n, BigInteger d, bool issimplified)
        {
            N = n;
            D = d;
            IsSimplified = issimplified;
            _oneTimeSimplifyDone = false;

            if (!IsSimplified) { TrySimplify(); }
        }

        public BigIntFraction(Func<BigInteger[], BigInteger[]> func, BigInteger[] arg)
        {
            BigInteger[] result = func.Invoke(arg);
            N = result[0];
            D = result[1];
            _oneTimeSimplifyDone = false;

            IsSimplified = false;
            TrySimplify();
        }

        static ClonedPrimes _primes;
        static void GetClonedPrimes()
        {
            _primes = KnownPrimes.CloneKnownPrimes(0, 100000);
        }

        internal void TryApproximate()
        {
            if (N > long.MaxValue || D > long.MaxValue)
            {
                BigRational rational = new BigRational(N, D);
                Fraction<long> approxFrac = FractionConverter<long>.BigRationalToFraction(rational);
                N = (BigInteger)Convert.ChangeType(approxFrac.N, typeof(BigInteger));
                D = (BigInteger)Convert.ChangeType(approxFrac.D, typeof(BigInteger));
            }
        }

        bool _oneTimeSimplifyDone { get; set; }
        internal void TrySimplify()
        {
            if (_primes == null) GetClonedPrimes();

            List<long> dFactors = Factors.GetPrimeFactors(D, _primes);
            foreach (long fac in dFactors)
            {
                while (true)
                {
                    if (N % fac == 0 && D % fac == 0)
                    {
                        N /= fac;
                        D /= fac;
                    }
                    else break;
                }
            }
            TryApproximate();
        }

        public BigInteger N { get; private set; }
        public BigInteger D { get; private set; }
        public bool IsSimplified { get; private set; }

        /// <summary>
        /// Calculates the double precision value of the fraction.
        /// </summary>
        /// <returns></returns>
        public double GetValue()
        {
            return NumberConverter.BigRationalToDouble(new BigRational(N, D));
        }

        /// <summary>
        /// Returns the string representation of this fraction.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return N.ToString() + " / " + D.ToString();
        }
    }

    public struct Fraction<T> : IFractionValue where T : struct, IConvertible
    {
        /// <summary>
        /// Will simplify the fraction so gcd of numerator and denominator is 1.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="issimplified"></param>
        public Fraction(T n, T d, bool issimplified)
        {
            N = n;
            D = d;
            IsSimplified = issimplified;
            _oneTimeApproxDone = false;
            
            if (!IsSimplified) { TrySimplify(); }
        }

        public Fraction(Func<T[], T[]> func, T[] arg)
        {
            T[] result = func.Invoke(arg);
            N = result[0];
            D = result[1];
            _oneTimeApproxDone = false;

            IsSimplified = false;
            TrySimplify();
        }

        static ClonedPrimes _primes;
        static void GetClonedPrimes()
        {
            _primes = KnownPrimes.CloneKnownPrimes(0, 100000);
        }

        bool _oneTimeApproxDone { get; set; }
        internal void TrySimplify() {
            if (_primes == null) GetClonedPrimes();
            long n = N.ToInt64(null);
            long d = D.ToInt64(null);

            List<long> dFactors = Factors.GetPrimeFactors(d, _primes);
            foreach (long fac in dFactors)
            {
                while (true)
                {
                    if (n % fac == 0 && d % fac == 0)
                    {
                        n /= fac;
                        d /= fac;
                    } else break;
                }
            }

            N = (T)Convert.ChangeType(n, typeof(T));
            D = (T)Convert.ChangeType(d, typeof(T));
        }

        public T N { get; private set; }
        public T D { get; private set; }
        public bool IsSimplified { get; private set; }

        /// <summary>
        /// Calculates the double precision value of the fraction.
        /// </summary>
        /// <returns></returns>
        public double GetValue()
        {
            return N.ToDouble(null) / D.ToDouble(null);
        }

        /// <summary>
        /// Returns the string representation of this fraction.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return N.ToString() + " / " + D.ToString();
        }
    }

    public class FractionConverter<T> where T : struct, IConvertible {
        public static Fraction<T> RealToFraction(double value, double error = 0.0000001)
        {
            if (error <= 0.0 || error >= 1.0)
            {
                throw new ArgumentOutOfRangeException("error", "Must be between 0 and 1 (exclusive).");
            }

            int sign = Math.Sign(value);

            if (sign == -1)
            {
                value = Math.Abs(value);
            }

            if (sign != 0)
            {
                // error is the maximum relative error; convert to absolute
                error *= value;
            }

            int n = (int)Math.Floor(value);
            value -= n;

            if (value < error)
            {
                return new Fraction<T>((T)Convert.ChangeType(sign * n, typeof(T)), (T)Convert.ChangeType(1, typeof(T)), false);
            }

            if (1 - error < value)
            {
                return new Fraction<T>((T)Convert.ChangeType(sign * (n+1), typeof(T)), (T)Convert.ChangeType(1, typeof(T)), false);
            }

            // The lower fraction is 0/1
            int lower_n = 0;
            int lower_d = 1;

            // The upper fraction is 1/1
            int upper_n = 1;
            int upper_d = 1;

            while (true)
            {
                // The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
                int middle_n = lower_n + upper_n;
                int middle_d = lower_d + upper_d;

                if (middle_d * (value + error) < middle_n)
                {
                    // real + error < middle : middle is our new upper
                    upper_n = middle_n;
                    upper_d = middle_d;
                }
                else if (middle_n < (value - error) * middle_d)
                {
                    // middle < real - error : middle is our new lower
                    lower_n = middle_n;
                    lower_d = middle_d;
                }
                else
                {
                    // Middle is our best fraction
                    return new Fraction<T>((T)Convert.ChangeType((n * middle_d + middle_n) * sign, typeof(T)), (T)Convert.ChangeType(middle_d, typeof(T)), false);
                }
            }
        }

        /// <summary>
        /// Will work for int, long.
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static Fraction<T> BigRationalToFraction(BigRational sum, double epsilon = 0.000000000001)
        {
            int nMax = 25;
            if (typeof(T) == typeof(int)) nMax = 9;
            if (typeof(T) == typeof(long)) nMax = 16;
            int diff = sum.Numerator.ToString().Length - sum.Denominator.ToString().Length;
            int denLen = sum.Denominator.ToString().Length > nMax ? nMax : sum.Denominator.ToString().Length;
            string denominator = sum.Denominator.ToString().Substring(0, denLen);
            int numLen = denLen;
            if (denLen + diff > 0) numLen += diff;
            string numerator = sum.Numerator.ToString().Substring(0, numLen);
            double sumFrac = Convert.ToInt64(numerator) * 1.0 / Convert.ToInt64(denominator);
            if (denLen + diff < 0)
            {
                sumFrac /= Math.Pow(10, -diff);
            }

            Fraction<T> frac = RealToFraction(sumFrac, 0.000000000001);
            return frac;
        }
    }

    public class NumberConverter
    {
        public static double BigRationalToDouble(BigRational sum, double epsilon = 0.000000000001)
        {
            int nMax = 15;
            string sumNumeratorString = sum.Numerator.ToString();
            string sumDenominatorString = sum.Denominator.ToString();

            int diff = sumNumeratorString.Length - sumDenominatorString.Length;
            int denLen = sumDenominatorString.Length;
            int numLen = sumNumeratorString.Length;
            if(numLen > nMax)
            {
                int diff1 = numLen - nMax;
                numLen -= diff1;
                denLen -= diff1;
            }
            if (denLen > nMax) {
                int diff1 = denLen - nMax;
                numLen -= diff1;
                denLen -= diff1;
            }

            if (numLen <= 0) return 0.0;

            string numerator = sumNumeratorString.Substring(0, numLen);
            string denominator = sumDenominatorString.Substring(0, denLen);
            return Convert.ToInt64(numerator) * 1.0 / Convert.ToInt64(denominator);
        }

        public static string DoubleToFraction(double num, double epsilon = 0.0001, int maxIterations = 20)
        {
            double[] d = new double[maxIterations + 2];
            d[1] = 1;
            double z = num;
            double n = 1;
            int t = 1;

            int wholeNumberPart = (int)num;
            double decimalNumberPart = num - Convert.ToDouble(wholeNumberPart);

            while (t < maxIterations && Math.Abs(n / d[t] - num) > epsilon)
            {
                t++;
                z = 1 / (z - (int)z);
                d[t] = d[t - 1] * (int)z + d[t - 2];
                n = (int)(decimalNumberPart * d[t] + 0.5);
            }

            return string.Format((wholeNumberPart > 0 ? wholeNumberPart.ToString() + " " : "") + "{0}/{1}",
                                 n.ToString(),
                                 d[t].ToString()
                                );
        }
    }
}