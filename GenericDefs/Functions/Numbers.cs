using GenericDefs.Classes;
using Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GenericDefs.Functions
{
    public class Numbers
    {
        public enum Parity
        {
            AllDigitsEven,
            AllDigitsOdd,
            IsEven,
            IsOdd
        }

        public static bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        public static bool HasDuplicateDigits(long n)
        {
            int[] used = new int[10];

            if (n > 9999999999) return false;

            for (int i = 0; i < 10; i++) { used[i] = 0; }

            while (n != 0)
            {
                if (used[n % 10] == 1) return true;

                used[n % 10] = 1;
                n /= 10;
            }

            return false;
        }

        /// <summary>
        /// A number checker to test if a number has duplicate digits, or it has all ten digits (0-9).
        /// </summary>
        /// <param name="n">The number to check for.</param>
        /// <param name="duplicatesAllowed">To check if duplicates are allowed.</param>
        /// <param name="shouldHaveAllDigits">To check if all digits 0-9 are required.</param>
        /// <returns></returns>
        public static bool GenericDigitChecker(long n, bool duplicatesAllowed, bool shouldHaveAllDigits)
        {
            bool hasDuplicates = false;
            bool hasAllDigits = false;
            if (!duplicatesAllowed)
            {
                if (n > 9999999999) return false;
            }

            long n1 = n;
            bool finalAnswer = true;
            if (!duplicatesAllowed)
            {
                int[] used = new int[10];
                for (int i = 0; i < 10; i++) { used[i] = 0; }

                while (n != 0)
                {
                    if (used[n % 10] == 1) { hasDuplicates = true; break; }

                    used[n % 10] = 1;
                    n /= 10;
                }
                if (hasDuplicates)
                {
                    return false;
                }
            }

            if (finalAnswer && shouldHaveAllDigits)
            {
                int[] digitArray = GetDigitArray(n1);
                if (digitArray.Distinct().Count() == 9 && !digitArray.Contains(0)) hasAllDigits = true;
                else hasAllDigits = digitArray.Distinct().Count() == 10;
                if (!hasAllDigits)
                {
                    finalAnswer = false;
                }
            }

            return finalAnswer;
        }

        public static bool ParityChecker(long n, Parity p)
        {
            if (p == Parity.IsEven) { return Even(n); }
            if (p == Parity.IsOdd) { return Odd(n); }
            List<int> digits = GetDigits(n);
            if (p == Parity.AllDigitsEven) return digits.TrueForAll(Even);
            if (p == Parity.AllDigitsOdd) return digits.TrueForAll(Odd);

            return true;
        }

        private static bool Even(long n)
        {
            return n % 2 == 0;
        }

        private static bool Even(int n)
        {
            return n % 2 == 0;
        }

        private static bool Odd(long n)
        {
            return n % 2 != 0;
        }

        private static bool Odd(int n)
        {
            return n % 2 != 0;
        }

        public static int[] GetDigitArray(long num)
        {
            List<int> listOfInts = GetDigits(num);
            return listOfInts.ToArray();
        }

        private static List<int> GetDigits(long num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add((int)(num % 10));
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts;
        }

        public static long ReverseANumber(long n)
        {
            char[] chN = n.ToString().ToCharArray();
            Array.Reverse(chN);
            return Convert.ToInt64(new string(chN));
        }

        public static BigInteger ReverseANumber(BigInteger n)
        {
            char[] chN = n.ToString().ToCharArray();
            Array.Reverse(chN);
            return BigInteger.Parse(new string(chN));
        }

        public static bool IsSmithNumber(long n)
        {
            Dictionary<long, int> factors = Factors.GetPrimeFactorsWithMultiplicity(n);

            long lhs = 0, rhs = MathFunctions.DigitSum(n);
            foreach (KeyValuePair<long, int> kvp in factors)
            {
                int nC = kvp.Value;
                while (true)
                {
                    nC--;
                    lhs += MathFunctions.DigitSum(kvp.Key);
                    if (nC == 0) break;
                }
            }

            return (lhs == rhs);
        }

        public static BigInteger Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

        public class UnusualFunctions
        {
            /// <summary>
            /// Check if a number can be expressed as a sum of squares of integers.
            /// N = a^2 + b^2; (includes 0)
            /// </summary>
            /// <returns></returns>
            public static UniqueIntegralPairs NumberAsSumOfTwoSquares(long n)
            {
                UniqueIntegralPairs u = new UniqueIntegralPairs();
                HashSet<int> pSq = LastDigitsOfPossibleSquares();
                Dictionary<int, HashSet<int>> setD = LastDigitSetsOfPossibleSquares();
                
                long sqrt = (long)Math.Sqrt(n);
                for (int a = 0; a <= sqrt; a++)
                {
                    long diff = n - (a * a);
                    int ew = (int)(diff % 10);
                    if (!pSq.Contains(ew)) continue;

                    long sqrt1 = (long)Math.Sqrt(diff);
                    for (int b = a; b <= sqrt1; b++)
                    {
                        if (diff - b * b == 0)
                        {
                            u.AddCombination(new int[] { a, b });
                        }
                    }
                }

                return u;
            }

            private static HashSet<int> LastDigitsOfPossibleSquares()
            {
                HashSet<int> pSq = new HashSet<int>();
                pSq.Add(1); pSq.Add(4); pSq.Add(5); pSq.Add(6); pSq.Add(9); pSq.Add(0);
                return pSq;
            }

            private static Dictionary<int, HashSet<int>> LastDigitSetsOfPossibleSquares()
            {
                Dictionary<int, HashSet<int>> setD = new Dictionary<int, HashSet<int>>();
                setD.Add(0, new HashSet<int>() { 0 });
                setD.Add(1, new HashSet<int>() { 1, 9 });
                setD.Add(4, new HashSet<int>() { 2, 8 });
                setD.Add(5, new HashSet<int>() { 5 });
                setD.Add(6, new HashSet<int>() { 4, 6 });
                setD.Add(9, new HashSet<int>() { 3, 7 });
                return setD;
            }
        }

        public class BigIntegerArithmetic
        {
            /// <summary>
            /// a is divided by b, returns a string with a precision of maxDigits.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="maxDigits"></param>
            /// <returns></returns>
            public static string Divide(BigInteger a, BigInteger b, int maxDigits)
            {
                int index = 0;

                BigInteger result = 0;
                string retVal = (a / b).ToString() + ".";
                BigInteger carry = a % b;
                while (index < maxDigits)
                {
                    result = carry;
                    retVal += result / b;
                    carry = (result % b) * 10;
                    index++;
                }

                return retVal;
            }

            /// <summary>
            /// a is divided by b, returns a string with a precision of maxDigits.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="maxDigits"></param>
            /// <returns></returns>
            public static string DecimalPartOfDivision(BigInteger a, BigInteger b, int maxDigits)
            {
                int index = 0;
                BigInteger result = 0;
                string retVal = string.Empty;
                BigInteger carry = a % b;
                while (index < maxDigits)
                {
                    result = carry;
                    retVal += result / b;
                    carry = (result % b) * 10;
                    index++;
                }

                return retVal;
            }
        }
    }

    public class BaseConversion
    {
        public static BigRational ConvertToBase10(string wholePart, string decimalPart, int fromBase)
        {
            Dictionary<char, int> digitsInBase = new Dictionary<char, int>();
            int dCounter = 0;
            char c = 'a';
            while (true) {
                char ch;
                if (dCounter < 10) ch = dCounter.ToString()[0];
                else { ch = c; c++; }
                digitsInBase.Add(ch, dCounter);
                dCounter++;
                if (dCounter == fromBase) break;
            }

            wholePart = string.Join("", wholePart.Reverse());

            BigInteger wholeNum = new BigInteger(0);
            BigRational retVal = new BigRational(0, 1);
            BigInteger bPow = new BigInteger(1);
            int n = 0;
            if (!string.IsNullOrEmpty(wholePart))
            {
                while (true)
                {
                    wholeNum += digitsInBase[wholePart[n]] * bPow;

                    n++;
                    if (n == wholePart.Length) break;
                    bPow *= fromBase;
                }
            }

            n = 0;
            bPow = 1;
            if (!string.IsNullOrEmpty(decimalPart))
            {
                while (true)
                {
                    bPow *= fromBase;
                    retVal += new BigRational(digitsInBase[decimalPart[n]], bPow);

                    n++;
                    if (n == decimalPart.Length) break;
                }
            }
            retVal += wholeNum;

            return retVal;
        }
        
        public static string ConvertFromBaseAToBaseB(string wholePart, string decimalPart, int fromBase, int toBase, int precision)
        {
            Dictionary<char, int> digitsInBaseA = new Dictionary<char, int>();
            int dCounter = 0;
            char c = 'a';
            while (true)
            {
                char ch;
                if (dCounter < 10) ch = dCounter.ToString()[0];
                else { ch = c; c++; }
                digitsInBaseA.Add(ch, dCounter);
                dCounter++;
                if (dCounter == fromBase) break;
            }

            Dictionary<int, char> digitsInBaseB = new Dictionary<int, char>();
            dCounter = 0;
            c = 'a';
            while (true)
            {
                char ch;
                if (dCounter < 10) ch = dCounter.ToString()[0];
                else { ch = c; c++; }
                digitsInBaseB.Add(dCounter, ch);
                dCounter++;
                if (dCounter == toBase) break;
            }

            wholePart = string.Join("", wholePart.Reverse());

            BigInteger wholeNumBase10 = new BigInteger(0);
            string retVal = string.Empty;
            BigInteger bPow = new BigInteger(1);
            BigInteger tPow = new BigInteger(1);
            int n = 0;
            if (!string.IsNullOrEmpty(wholePart))
            {
                while (true)
                {
                    wholeNumBase10 += digitsInBaseA[wholePart[n]] * bPow;

                    n++;
                    if (n == wholePart.Length) break;
                    bPow *= fromBase;
                }

                string wholeNumBaseBStr = string.Empty;
                BigInteger wholeNumBase10C = new BigInteger(wholeNumBase10.ToByteArray());
                while (true)
                {
                    wholeNumBaseBStr = digitsInBaseB[(int)(wholeNumBase10C % toBase)] + "" + wholeNumBaseBStr;

                    wholeNumBase10C /= toBase;
                    if (wholeNumBase10C == 0) break;
                }
                retVal = wholeNumBaseBStr + ".";
            }
            
            if (!string.IsNullOrEmpty(decimalPart))
            {
                n = 0;
                bPow = 1;
                BigRational fractionA10 = new BigRational(0, 1);
                while (true)
                {
                    bPow *= fromBase;
                    fractionA10 += new BigRational(digitsInBaseA[decimalPart[n]], bPow);

                    n++;
                    if (n == decimalPart.Length) break;
                }

                n = 0;
                fractionA10 = fractionA10.GetFractionPart();
                BigInteger fa10Nume = fractionA10.Numerator;
                BigInteger fa10Den = fractionA10.Denominator;
                while (true)
                {
                    fa10Nume *= toBase;
                    retVal += "" + fa10Nume / fa10Den;
                    fa10Nume %= fa10Den;
                    n++;
                    if (n == precision) break;
                }
            }

            return retVal;
        }

        public static string ConvertFromBaseAToBaseB(string wholePart,int fromBase, int toBase)
        {
            Dictionary<char, int> digitsInBaseA = new Dictionary<char, int>();
            int dCounter = 0;
            char c = 'a';
            while (true)
            {
                char ch;
                if (dCounter < 10) ch = dCounter.ToString()[0];
                else { ch = c; c++; }
                digitsInBaseA.Add(ch, dCounter);
                dCounter++;
                if (dCounter == fromBase) break;
            }

            Dictionary<int, char> digitsInBaseB = new Dictionary<int, char>();
            dCounter = 0;
            c = 'a';
            while (true)
            {
                char ch;
                if (dCounter < 10) ch = dCounter.ToString()[0];
                else { ch = c; c++; }
                digitsInBaseB.Add(dCounter, ch);
                dCounter++;
                if (dCounter == toBase) break;
            }

            wholePart = string.Join("", wholePart.Reverse());

            BigInteger wholeNumBase10 = new BigInteger(0);
            string retVal = string.Empty;
            BigInteger bPow = new BigInteger(1);
            BigInteger tPow = new BigInteger(1);
            int n = 0;
            if (!string.IsNullOrEmpty(wholePart))
            {
                while (true)
                {
                    wholeNumBase10 += digitsInBaseA[wholePart[n]] * bPow;

                    n++;
                    if (n == wholePart.Length) break;
                    bPow *= fromBase;
                }

                string wholeNumBaseBStr = string.Empty;
                BigInteger wholeNumBase10C = new BigInteger(wholeNumBase10.ToByteArray());
                while (true)
                {
                    wholeNumBaseBStr = digitsInBaseB[(int)(wholeNumBase10C % toBase)] + "" + wholeNumBaseBStr;

                    wholeNumBase10C /= toBase;
                    if (wholeNumBase10C == 0) break;
                }
                retVal = wholeNumBaseBStr;
            }

            return retVal;
        }
    }
}