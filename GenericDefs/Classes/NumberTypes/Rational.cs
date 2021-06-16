using System;

namespace GenericDefs.Classes.NumberTypes
{
    // This is Version 2.
    public struct Rational : IComparable, IComparable<Rational>, IEquatable<Rational>
    {
        // Retained from Version 1.
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        // These fields bypass Simplify().
        public static readonly Rational MinValue = new Rational { Numerator = int.MinValue, Denominator = 1 };
        public static readonly Rational MaxValue = new Rational { Numerator = int.MaxValue, Denominator = 1 };
        public static readonly Rational Epsilon = new Rational { Numerator = 1, Denominator = int.MaxValue };
        public static readonly Rational Undefined = new Rational { Numerator = 0, Denominator = 0 };
        public static readonly Rational Zero = new Rational { Numerator = 0, Denominator = 1 };
        public static readonly Rational One = new Rational { Numerator = 1, Denominator = 1 };
        public static readonly Rational MinusOne = new Rational { Numerator = -1, Denominator = 1 };

        public Rational(int numerator, int denominator = 1) : this()
        {
            Numerator = numerator;
            Denominator = denominator;
            // There is a special case where Simplify() could throw an exception:
            //
            //      new Rational(int.MinValue, certainNegativeIntegers)
            //
            // In general, having the contructor throw an exception is bad practice.
            // However given the extremity of this special case and the fact that Rational 
            // is an immutable struct where its inputs are ONLY validated DURING
            // construction, I allow the exception to be thrown here.
            Simplify();
        }

        public static bool TryCreate(int numerator, int denominator, out Rational result)
        {
            try
            {
                result = new Rational(numerator, denominator);
                return true;
            }
            catch
            {
                result = Undefined;
            }
            return false;
        }

        public static bool TryParse(string s, out Rational result)
        {
            try
            {
                result = Rational.Parse(s);
                return true;
            }
            catch
            {
                result = Undefined;
            }
            return false;
        }

        public static Rational Parse(string s)
        {
            // Note that "3 / -4" would return new Rational(-3, 4).
            var tokens = s.Split(new char[] { '/' });

            var numerator = 0;
            var denominator = 0;

            switch (tokens.Length)
            {
                case 1:
                    numerator = GetInteger("Numerator", tokens[0]);
                    denominator = 1;
                    break;
                case 2:
                    numerator = GetInteger("Numerator", tokens[0]);
                    denominator = GetInteger("Denominator", tokens[1]);
                    break;
                default:
                    throw new ArgumentException(string.Format("Invalid input string: '{0}'", s));
            }
            return new Rational(numerator, denominator);
        }

        // This is only called by Parse.
        private static int GetInteger(string desc, string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(desc);
            }
            var result = 0;
            // TODO: Decide whether it's good idea to convert " -  4" to "-4".
            s = s.Replace(" ", string.Empty);
            if (!int.TryParse(s, out result))
            {
                throw new ArgumentException(string.Format("Invalid value for {0}: '{1}'", desc, s));
            }
            return result;
        }

        // Ver 2 Change: uses if's instead of switch(Denominator).  Should be easier for Sam The Maintainer.
        //TODO: consider other overloads of ToString().  Perhaps one to always display a division symbol.
        // For example, new Rational(0, 0).ToString() --> "0/0" instead of "Undefined", or
        //              new Rational(5).ToString()    --> "5/1" instead of "5"
        public override string ToString()
        {
            if (IsUndefined) { return "Undefined"; }
            if (IsInteger) { return Numerator.ToString(); }
            return string.Format("{0}/{1}", Numerator, Denominator);
        }

        public int CompareTo(object other)
        {
            if (other == null) { return 1; }
            if (other is Rational) { return CompareTo((Rational)other); }
            throw new ArgumentException("Argument must be Rational");
        }

        public int CompareTo(Rational other)
        {
            if (IsUndefined)
            {
                // While IEEE decrees that floating point NaN's are not equal to each other,
                // I am not under any decree to adhere to that same specification for Rational.
                return other.IsUndefined ? 0 : -1;
            }
            if (other.IsUndefined) { return 1; }
            return ToDouble().CompareTo(other.ToDouble());
        }

        public bool Equals(Rational other)
        {
            if (IsUndefined) { return other.IsUndefined; }
            return (Numerator == other.Numerator) && (Denominator == other.Denominator);
        }

        public override bool Equals(object other)
        {
            if (other == null) { return false; }
            if (other is Rational) { return Equals((Rational)other); }
            throw new ArgumentException("Argument must be Rational");
        }

        // Mofified code that was stolen from:
        // http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/ndp/clr/src/BCL/System/Double@cs/1305376/Double@cs
        // The hashcode for a double is the absolute value of the integer representation of that double.
        [System.Security.SecuritySafeCritical]  // auto-generated
        public unsafe override int GetHashCode()
        {
            if (Numerator == 0)
            {
                // Ensure that 0 and -0 have the same hash code
                return 0;
            }
            double d = ToDouble();
            long value = *(long*)(&d);
            return unchecked((int)value) ^ ((int)(value >> 32));
        }

        public static bool operator ==(Rational rat1, Rational rat2)
        {
            return rat1.Equals(rat2);
        }

        public static bool operator !=(Rational rat1, Rational rat2)
        {
            return !rat1.Equals(rat2);
        }

        // Version 2 Change for operators { +, -, *, / } :
        // Removed goofy call to Simplify() and rely upon constructor.
        // I use local variable n and d for better readability for Sam the Maintainer,
        // who's failing eyesight may miss a comma here and there.

        public static Rational operator +(Rational rat1, Rational rat2)
        {
            if (rat1.IsUndefined || rat2.IsUndefined)
            {
                return Undefined;
            }
            var n = (rat1.Numerator * rat2.Denominator) + (rat1.Denominator * rat2.Numerator);
            var d = rat1.Denominator * rat2.Denominator;
            return new Rational(n, d);
        }

        public static Rational operator -(Rational rat1, Rational rat2)
        {
            if (rat1.IsUndefined || rat2.IsUndefined)
            {
                return Undefined;
            }
            var n = (rat1.Numerator * rat2.Denominator) - (rat1.Denominator * rat2.Numerator);
            var d = rat1.Denominator * rat2.Denominator;
            return new Rational(n, d);
        }

        public static Rational operator *(Rational rat1, Rational rat2)
        {
            if (rat1.IsUndefined || rat2.IsUndefined)
            {
                return Undefined;
            }
            var n = rat1.Numerator * rat2.Numerator;
            var d = rat1.Denominator * rat2.Denominator;
            return new Rational(n, d);
        }

        public static Rational operator /(Rational rat1, Rational rat2)
        {
            if (rat1.IsUndefined || rat2.IsUndefined)
            {
                return Undefined;
            }
            // fixed math error from Version 1
            var n = rat1.Numerator * rat2.Denominator;
            var d = rat1.Denominator * rat2.Numerator;
            return new Rational(n, d);
        }

        // Ver 2 Change: now void - no longer returns Rational.
        // The simplified Denominator will always be >= 0 for any Rational.
        // For a Rational to be negative, the simplified Numerator will be negative.
        // Thus a Rational(3, -4) would simplify to Rational(-3, 4).
        private void Simplify()
        {
            // These corner cases are very quick checks that means slightly longer code.
            // Yet I feel their explicit handling makes their logic more clear to future maintenance.
            // More importantly, it bypasses modulus and division when its not absolutely needed.
            if (IsUndefined)
            {
                Numerator = 0;
                return;
            }
            if (Numerator == 0)
            {
                Denominator = 1;
                return;
            }
            if (IsInteger)
            {
                return;
            }
            if (Numerator == Denominator)
            {
                Numerator = 1;
                Denominator = 1;
                return;
            }
            if (Denominator < 0)
            {
                // One special corner case when unsimplified Denominator is < 0 and Numerator equals int.MinValue.
                if (Numerator == int.MinValue)
                {
                    ReduceOrThrow();
                    return;
                }
                // Simpler and faster than mutiplying by -1
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
            // We only perform modulus and division if we absolutely must.
            Reduce();
        }

        private void Reduce()
        {
            // Reduce() is never called if Numerator or Denominator equals 0.
            var greatestCommonDivisor = GreatestCommonDivisor(Numerator, Denominator);
            Numerator /= greatestCommonDivisor;
            Denominator /= greatestCommonDivisor;
        }

        // Ver 2 Change: now void - no longer returns Rational.
        // Very special one off case: only called when unsimplified Numerater equals int.MinValue and Denominator is negative.
        // Some combinations produce a valid Rational, such as Rational(int.MinValue, int.MinValue), equivalent to Rational(1).
        // Others are not valid, such as Rational(int.MinValue, -1) because the Numerator would need to be (int.MaxValue + 1).
        private void ReduceOrThrow()
        {
            try
            {
                Reduce();
            }
            catch
            {
                throw new ArgumentException(string.Format("Invalid Rational(int.MinValue, {0})", Denominator));
            }
        }

        public bool IsUndefined { get { return (Denominator == 0); } }

        public bool IsInteger { get { return (Denominator == 1); } }

        public double ToDouble()
        {
            if (IsUndefined) { return double.NaN; }
            return (double)Numerator / (double)Denominator;
        }

        /// http://en.wikipedia.org/wiki/Euclidean_algorithm
        private static int GreatestCommonDivisor(int a, int b)
        {
            return (b == 0) ? a : GreatestCommonDivisor(b, a % b);
        }

    } //end struct
}