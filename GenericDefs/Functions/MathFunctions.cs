using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GenericDefs.DotNet;

namespace GenericDefs.Functions
{
    public class MathFunctions
    {
        public static bool IsSquare(long n)
        {
            if (n == 0) return true;
            long i = 1;
            for (;;)
            {
                if (n < 0) return false;
                if (n == 0) return true;
                n -= i;
                i += 2;
            }
        }

        public static int Signum(double x)
        {
            int retVal = 0;
            if (x > 0) retVal = 1;
            if (x < 0) retVal = -1;

            return retVal;
        }

        /// <summary>
        /// Square Root approach. Works for all int and long l.e.q. 10^15,
        /// </summary>
        /// <returns></returns>
        public static bool IsPerfectSquare(long n)
        {
            if (n == 0) return true;
            string nstr = "" + n;
            char last = nstr[nstr.Length - 1];
            if (last == '2' || last == '3' || last == '7' || last == '8') return false;

            double result = Math.Sqrt(n);
            return result % 1 == 0;
        }

        public static int GCD(int[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static long GCD(List<long> numbers)
        {
            return numbers.Aggregate(GCD);
        }

        public static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static BigInteger LCM(BigInteger a, BigInteger b)
        {
            return BigInteger.Abs(a * b) / GCD(a, b);
        }

        public static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }

        public static long LCM(long[] numbers)
        {
            return numbers.Aggregate(LCM);
        }

        public static BigInteger LCM(BigInteger[] numbers)
        {
            return numbers.Aggregate(LCM);
        }

        public static bool IsOdd(long number)
        {
            return (number % 2 != 0);
        }

        public static bool CoPrime(long u, long v)
        {
            if (((u | v) & 1) == 0) return false;

            while ((u & 1) == 0) u >>= 1;
            if (u == 1) return true;

            do
            {
                while ((v & 1) == 0) v >>= 1;
                if (v == 1) return true;

                if (u > v) { long t = v; v = u; u = t; }
                v -= u;
            } while (v != 0);

            return false;
        }

        public static long DigitSum(long n)
        {
            long sum = 0;
            while (n != 0)
            {
                sum += n % 10;
                n /= 10;
            }

            return sum;
        }

        public static BigInteger DigitSum(BigInteger n)
        {
            BigInteger sum = 0;
            while (n != 0)
            {
                int p10 = (int)(n % 10);
                sum += p10;
                n -= p10;
                n /= 10;
            }

            return sum;
        }

        public static int DigitSum(int n)
        {
            int sum = 0;
            while (n != 0)
            {
                sum += n % 10;
                n /= 10;
            }

            return sum;
        }

        public static Tuple<long, long, long> EuclideanGCD(long a, long b)
        {
            //////
            //Logic wrong here. May not work
            /////
            //return ExtendedEuclideanGCD(a, b);
            if (a % b == 0) return new Tuple<long, long, long>(1, 0, 1);
            else
            {
                Tuple<long, long, long> xy = EuclideanGCD(b, a % b);
                return new Tuple<long, long, long>(xy.Item1, xy.Item3, xy.Item2 - (xy.Item3 * (a / b)));
            }
        }

        private static Tuple<long, long, long> ExtendedEuclideanGCD(long a, long b)
        {
            if (a % b == 0) return new Tuple<long, long, long>(1, 0, 1);
            else {
                Tuple<long, long, long> xy = ExtendedEuclideanGCD(b, a % b);
                return new Tuple<long, long, long>(xy.Item1, xy.Item3, xy.Item2 - (xy.Item3 * (a / b)));
            }
        }

        public static void ExtendedEuclidBezout(long a, long b, ref long x, ref long y, ref long gcd)
        {
            long q, r, x1, x2, y1, y2;

            if (b == 0)
            {
                gcd = a; x = 1; y = 0;
                return;
            }
            x2 = 1; x1 = 0; y2 = 0; y1 = 1;
            while (b > 0)
            {
                q = a / b;
                r = a - (q * b);
                x = x2 - (q * x1);
                y = y2 - (q * y1);
                a = b;
                b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }
            gcd = a; x = x2; y = y2;
        }

        public static long EuclideanGCDAlgo(long a, long b)
        {
            if (a % b == 0) return b;
            else if (b % a == 0) return a;

            if (a > b) return EuclideanGCDAlgo(a % b, b);
            else return EuclideanGCDAlgo(a, b % a);
        }

        public static BigInteger[] Extended_GCD(BigInteger A, BigInteger B)
        {
            BigInteger[] result = new BigInteger[3];
            if (A < B) //if A less than B, switch them
            {
                BigInteger temp = A;
                A = B;
                B = temp;
            }
            BigInteger r = B;
            BigInteger q = 0;
            BigInteger x0 = 1;
            BigInteger y0 = 0;
            BigInteger x1 = 0;
            BigInteger y1 = 1;
            BigInteger x = 0, y = 0;
            while (r > 1)
            {
                r = A % B;
                q = A / B;
                x = x0 - q * x1;
                y = y0 - q * y1;
                x0 = x1;
                y0 = y1;
                x1 = x;
                y1 = y;
                A = B;
                B = r;
            }
            result[0] = r;
            result[1] = x;
            result[2] = y;
            return result;
        }

        public static int MultiplicativeInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
        public static long MultiplicativeInverse(long a, long n)
        {
            long i = n, v = 0, d = 1;
            while (a > 0)
            {
                long t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
        public static BigInteger MultiplicativeInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}