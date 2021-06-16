using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace GenericDefs.Classes.NumberTypes.Examine
{
    [ComVisible(false)]
    [Serializable]
    public struct BigRational : IComparable, IComparable<BigRational>, IDeserializationCallback, IEquatable<BigRational>, ISerializable
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct DoubleUlong
        {
            [FieldOffset(0)]
            public double dbl;

            [FieldOffset(0)]
            public ulong uu;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DecimalUInt32
        {
            [FieldOffset(0)]
            public decimal dec;

            [FieldOffset(0)]
            public int flags;
        }

        private const int DoubleMaxScale = 308;

        private const int DecimalScaleMask = 16711680;

        private const int DecimalSignMask = -2147483648;

        private const int DecimalMaxScale = 28;

        private const string c_solidus = "/";

        private BigInteger m_numerator;

        private BigInteger m_denominator;

        private static readonly BigRational s_brZero = new BigRational(BigInteger.Zero);

        private static readonly BigRational s_brOne = new BigRational(BigInteger.One);

        private static readonly BigRational s_brMinusOne = new BigRational(BigInteger.MinusOne);

        private static readonly BigInteger s_bnDoublePrecision = BigInteger.Pow(10, 308);

        private static readonly BigInteger s_bnDoubleMaxValue = (BigInteger)1.7976931348623157E+308;

        private static readonly BigInteger s_bnDoubleMinValue = (BigInteger)(-1.7976931348623157E+308);

        private static readonly BigInteger s_bnDecimalPrecision = BigInteger.Pow(10, 28);

        private static readonly BigInteger s_bnDecimalMaxValue = (BigInteger)79228162514264337593543950335m;

        private static readonly BigInteger s_bnDecimalMinValue = (BigInteger)(-79228162514264337593543950335m);

        public static BigRational Zero
        {
            get
            {
                return BigRational.s_brZero;
            }
        }

        public static BigRational One
        {
            get
            {
                return BigRational.s_brOne;
            }
        }

        public static BigRational MinusOne
        {
            get
            {
                return BigRational.s_brMinusOne;
            }
        }

        public int Sign
        {
            get
            {
                return this.m_numerator.Sign;
            }
        }

        public BigInteger Numerator
        {
            get
            {
                return this.m_numerator;
            }
        }

        public BigInteger Denominator
        {
            get
            {
                return this.m_denominator;
            }
        }

        public BigInteger GetWholePart()
        {
            return BigInteger.Divide(this.m_numerator, this.m_denominator);
        }

        public BigRational GetFractionPart()
        {
            return new BigRational(BigInteger.Remainder(this.m_numerator, this.m_denominator), this.m_denominator);
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is BigRational && this.Equals((BigRational)obj);
        }

        public override int GetHashCode()
        {
            return (this.m_numerator / this.Denominator).GetHashCode();
        }

        int IComparable.CompareTo(object obj)
        {
            int result;
            if (obj == null)
            {
                result = 1;
            }
            else
            {
                if (!(obj is BigRational))
                {
                    throw new ArgumentException("Argument must be of type BigRational", "obj");
                }
                result = BigRational.Compare(this, (BigRational)obj);
            }
            return result;
        }

        public int CompareTo(BigRational other)
        {
            return BigRational.Compare(this, other);
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(this.m_numerator.ToString("R", CultureInfo.InvariantCulture));
            ret.Append("/");
            ret.Append(this.Denominator.ToString("R", CultureInfo.InvariantCulture));
            return ret.ToString();
        }

        public bool Equals(BigRational other)
        {
            bool result;
            if (this.Denominator == other.Denominator)
            {
                result = (this.m_numerator == other.m_numerator);
            }
            else
            {
                result = (this.m_numerator * other.Denominator == this.Denominator * other.m_numerator);
            }
            return result;
        }

        public BigRational(BigInteger numerator)
        {
            this.m_numerator = numerator;
            this.m_denominator = BigInteger.One;
        }

        public BigRational(double value)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentException("Argument is not a number", "value");
            }
            if (double.IsInfinity(value))
            {
                throw new ArgumentException("Argument is infinity", "value");
            }
            int sign;
            int exponent;
            ulong significand;
            bool isFinite;
            BigRational.SplitDoubleIntoParts(value, out sign, out exponent, out significand, out isFinite);
            if (significand == 0uL)
            {
                this = BigRational.Zero;
            }
            else
            {
                this.m_numerator = significand;
                this.m_denominator = 1048576;
                if (exponent > 0)
                {
                    this.m_numerator = BigInteger.Pow(this.m_numerator, exponent);
                }
                else if (exponent < 0)
                {
                    this.m_denominator = BigInteger.Pow(this.m_denominator, -exponent);
                }
                if (sign < 0)
                {
                    this.m_numerator = BigInteger.Negate(this.m_numerator);
                }
                this.Simplify();
            }
        }

        public BigRational(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            if (bits == null || bits.Length != 4 || (bits[3] & 2130771967) != 0 || (bits[3] & 16711680) > 1835008)
            {
                throw new ArgumentException("invalid Decimal", "value");
            }
            if (value == 0m)
            {
                this = BigRational.Zero;
            }
            else
            {
                ulong ul = (ulong)bits[2] << 32 | (uint)bits[1];
                this.m_numerator = (new BigInteger(ul) << 32 | (uint)bits[0]);
                bool isNegative = (bits[3] & -2147483648) != 0;
                if (isNegative)
                {
                    this.m_numerator = BigInteger.Negate(this.m_numerator);
                }
                int scale = (bits[3] & 16711680) >> 16;
                this.m_denominator = BigInteger.Pow(10, scale);
                this.Simplify();
            }
        }

        public BigRational(BigInteger numerator, BigInteger denominator)
        {
            if (denominator.Sign == 0)
            {
                throw new DivideByZeroException();
            }
            if (numerator.Sign == 0)
            {
                this.m_numerator = BigInteger.Zero;
                this.m_denominator = BigInteger.One;
            }
            else if (denominator.Sign < 0)
            {
                this.m_numerator = BigInteger.Negate(numerator);
                this.m_denominator = BigInteger.Negate(denominator);
            }
            else
            {
                this.m_numerator = numerator;
                this.m_denominator = denominator;
            }
            this.Simplify();
        }

        public BigRational(BigInteger whole, BigInteger numerator, BigInteger denominator)
        {
            if (denominator.Sign == 0)
            {
                throw new DivideByZeroException();
            }
            if (numerator.Sign == 0 && whole.Sign == 0)
            {
                this.m_numerator = BigInteger.Zero;
                this.m_denominator = BigInteger.One;
            }
            else if (denominator.Sign < 0)
            {
                this.m_denominator = BigInteger.Negate(denominator);
                this.m_numerator = BigInteger.Negate(whole) * this.m_denominator + BigInteger.Negate(numerator);
            }
            else
            {
                this.m_denominator = denominator;
                this.m_numerator = whole * denominator + numerator;
            }
            this.Simplify();
        }

        public static BigRational Abs(BigRational r)
        {
            return (r.m_numerator.Sign < 0) ? new BigRational(BigInteger.Abs(r.m_numerator), r.Denominator) : r;
        }

        public static BigRational Negate(BigRational r)
        {
            return new BigRational(BigInteger.Negate(r.m_numerator), r.Denominator);
        }

        public static BigRational Invert(BigRational r)
        {
            return new BigRational(r.Denominator, r.m_numerator);
        }

        public static BigRational Add(BigRational x, BigRational y)
        {
            return x + y;
        }

        public static BigRational Subtract(BigRational x, BigRational y)
        {
            return x - y;
        }

        public static BigRational Multiply(BigRational x, BigRational y)
        {
            return x * y;
        }

        public static BigRational Divide(BigRational dividend, BigRational divisor)
        {
            return dividend / divisor;
        }

        public static BigRational Remainder(BigRational dividend, BigRational divisor)
        {
            return dividend % divisor;
        }

        public static BigRational DivRem(BigRational dividend, BigRational divisor, out BigRational remainder)
        {
            BigInteger ad = dividend.m_numerator * divisor.Denominator;
            BigInteger bc = dividend.Denominator * divisor.m_numerator;
            BigInteger bd = dividend.Denominator * divisor.Denominator;
            remainder = new BigRational(ad % bc, bd);
            return new BigRational(ad, bc);
        }

        public static BigRational Pow(BigRational baseValue, BigInteger exponent)
        {
            BigRational result2;
            if (exponent.Sign == 0)
            {
                result2 = BigRational.One;
            }
            else
            {
                if (exponent.Sign < 0)
                {
                    if (baseValue == BigRational.Zero)
                    {
                        throw new ArgumentException("cannot raise zero to a negative power", "baseValue");
                    }
                    baseValue = BigRational.Invert(baseValue);
                    exponent = BigInteger.Negate(exponent);
                }
                BigRational result = baseValue;
                while (exponent > BigInteger.One)
                {
                    result *= baseValue;
                    exponent = --exponent;
                }
                result2 = result;
            }
            return result2;
        }

        public static BigInteger LeastCommonDenominator(BigRational x, BigRational y)
        {
            return x.Denominator * y.Denominator / BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
        }

        public static int Compare(BigRational r1, BigRational r2)
        {
            return BigInteger.Compare(r1.m_numerator * r2.Denominator, r2.m_numerator * r1.Denominator);
        }

        public static bool operator ==(BigRational x, BigRational y)
        {
            return BigRational.Compare(x, y) == 0;
        }

        public static bool operator !=(BigRational x, BigRational y)
        {
            return BigRational.Compare(x, y) != 0;
        }

        public static bool operator <(BigRational x, BigRational y)
        {
            return BigRational.Compare(x, y) < 0;
        }

        public static bool operator <=(BigRational x, BigRational y)
        {
            return BigRational.Compare(x, y) <= 0;
        }

        public static bool operator >(BigRational x, BigRational y)
        {
            return BigRational.Compare(x, y) > 0;
        }

        public static bool operator >=(BigRational x, BigRational y)
        {
            return BigRational.Compare(x, y) >= 0;
        }

        public static BigRational operator +(BigRational r)
        {
            return r;
        }

        public static BigRational operator -(BigRational r)
        {
            return new BigRational(-r.m_numerator, r.Denominator);
        }

        public static BigRational operator ++(BigRational r)
        {
            return r + BigRational.One;
        }

        public static BigRational operator --(BigRational r)
        {
            return r - BigRational.One;
        }

        public static BigRational operator +(BigRational r1, BigRational r2)
        {
            return new BigRational(r1.m_numerator * r2.Denominator + r1.Denominator * r2.m_numerator, r1.Denominator * r2.Denominator);
        }

        public static BigRational operator -(BigRational r1, BigRational r2)
        {
            return new BigRational(r1.m_numerator * r2.Denominator - r1.Denominator * r2.m_numerator, r1.Denominator * r2.Denominator);
        }

        public static BigRational operator *(BigRational r1, BigRational r2)
        {
            return new BigRational(r1.m_numerator * r2.m_numerator, r1.Denominator * r2.Denominator);
        }

        public static BigRational operator /(BigRational r1, BigRational r2)
        {
            return new BigRational(r1.m_numerator * r2.Denominator, r1.Denominator * r2.m_numerator);
        }

        public static BigRational operator %(BigRational r1, BigRational r2)
        {
            return new BigRational(r1.m_numerator * r2.Denominator % (r1.Denominator * r2.m_numerator), r1.Denominator * r2.Denominator);
        }
        
        public static explicit operator sbyte(BigRational value)
        {
            return (sbyte)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }
        
        public static explicit operator ushort(BigRational value)
        {
            return (ushort)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }
        
        public static explicit operator uint(BigRational value)
        {
            return (uint)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }
        
        public static explicit operator ulong(BigRational value)
        {
            return (ulong)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }

        public static explicit operator byte(BigRational value)
        {
            return (byte)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }

        public static explicit operator short(BigRational value)
        {
            return (short)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }

        public static explicit operator int(BigRational value)
        {
            return (int)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }

        public static explicit operator long(BigRational value)
        {
            return (long)BigInteger.Divide(value.m_numerator, value.m_denominator);
        }

        public static explicit operator BigInteger(BigRational value)
        {
            return BigInteger.Divide(value.m_numerator, value.m_denominator);
        }

        public static explicit operator float(BigRational value)
        {
            return (float)((double)value);
        }

        public static explicit operator double(BigRational value)
        {
            double result2;
            if (BigRational.SafeCastToDouble(value.m_numerator) && BigRational.SafeCastToDouble(value.m_denominator))
            {
                result2 = (double)value.m_numerator / (double)value.m_denominator;
            }
            else
            {
                BigInteger denormalized = value.m_numerator * BigRational.s_bnDoublePrecision / value.m_denominator;
                if (denormalized.IsZero)
                {
                    result2 = ((value.Sign < 0) ? BitConverter.Int64BitsToDouble(-9223372036854775808L) : 0.0);
                }
                else
                {
                    double result = 0.0;
                    bool isDouble = false;
                    for (int scale = 308; scale > 0; scale--)
                    {
                        if (!isDouble)
                        {
                            if (BigRational.SafeCastToDouble(denormalized))
                            {
                                result = (double)denormalized;
                                isDouble = true;
                            }
                            else
                            {
                                denormalized /= 10;
                            }
                        }
                        result /= 10.0;
                    }
                    if (!isDouble)
                    {
                        result2 = ((value.Sign < 0) ? double.NegativeInfinity : double.PositiveInfinity);
                    }
                    else
                    {
                        result2 = result;
                    }
                }
            }
            return result2;
        }

        public static explicit operator decimal(BigRational value)
        {
            decimal result;
            if (BigRational.SafeCastToDecimal(value.m_numerator) && BigRational.SafeCastToDecimal(value.m_denominator))
            {
                result = (decimal)value.m_numerator / (decimal)value.m_denominator;
            }
            else
            {
                BigInteger denormalized = value.m_numerator * BigRational.s_bnDecimalPrecision / value.m_denominator;
                if (!denormalized.IsZero)
                {
                    for (int scale = 28; scale >= 0; scale--)
                    {
                        if (BigRational.SafeCastToDecimal(denormalized))
                        {
                            BigRational.DecimalUInt32 dec = default(BigRational.DecimalUInt32);
                            dec.dec = (decimal)denormalized;
                            dec.flags = ((dec.flags & -16711681) | scale << 16);
                            result = dec.dec;
                            return result;
                        }
                        denormalized /= 10;
                    }
                    throw new OverflowException("Value was either too large or too small for a Decimal.");
                }
                result = 0m;
            }
            return result;
        }
        
        public static implicit operator BigRational(sbyte value)
        {
            return new BigRational((BigInteger)value);
        }
        
        public static implicit operator BigRational(ushort value)
        {
            return new BigRational((BigInteger)value);
        }

        public static implicit operator BigRational(uint value)
        {
            return new BigRational((BigInteger)value);
        }
        
        public static implicit operator BigRational(ulong value)
        {
            return new BigRational((BigInteger)value);
        }

        public static implicit operator BigRational(byte value)
        {
            return new BigRational((BigInteger)value);
        }

        public static implicit operator BigRational(short value)
        {
            return new BigRational((BigInteger)value);
        }

        public static implicit operator BigRational(int value)
        {
            return new BigRational((BigInteger)value);
        }

        public static implicit operator BigRational(long value)
        {
            return new BigRational((BigInteger)value);
        }

        public static implicit operator BigRational(BigInteger value)
        {
            return new BigRational(value);
        }

        public static implicit operator BigRational(float value)
        {
            return new BigRational((double)value);
        }

        public static implicit operator BigRational(double value)
        {
            return new BigRational(value);
        }

        public static implicit operator BigRational(decimal value)
        {
            return new BigRational(value);
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            try
            {
                if (this.m_denominator.Sign == 0 || this.m_numerator.Sign == 0)
                {
                    this.m_numerator = BigInteger.Zero;
                    this.m_denominator = BigInteger.One;
                }
                else if (this.m_denominator.Sign < 0)
                {
                    this.m_numerator = BigInteger.Negate(this.m_numerator);
                    this.m_denominator = BigInteger.Negate(this.m_denominator);
                }
                this.Simplify();
            }
            catch (ArgumentException e)
            {
                throw new SerializationException("invalid serialization data", e);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("Numerator", this.m_numerator);
            info.AddValue("Denominator", this.m_denominator);
        }

        private BigRational(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            this.m_numerator = (BigInteger)info.GetValue("Numerator", typeof(BigInteger));
            this.m_denominator = (BigInteger)info.GetValue("Denominator", typeof(BigInteger));
        }

        private void Simplify()
        {
            if (this.m_numerator == BigInteger.Zero)
            {
                this.m_denominator = BigInteger.One;
            }
            BigInteger gcd = BigInteger.GreatestCommonDivisor(this.m_numerator, this.m_denominator);
            if (gcd > BigInteger.One)
            {
                this.m_numerator /= gcd;
                this.m_denominator = this.Denominator / gcd;
            }
        }

        private static bool SafeCastToDouble(BigInteger value)
        {
            return BigRational.s_bnDoubleMinValue <= value && value <= BigRational.s_bnDoubleMaxValue;
        }

        private static bool SafeCastToDecimal(BigInteger value)
        {
            return BigRational.s_bnDecimalMinValue <= value && value <= BigRational.s_bnDecimalMaxValue;
        }

        private static void SplitDoubleIntoParts(double dbl, out int sign, out int exp, out ulong man, out bool isFinite)
        {
            BigRational.DoubleUlong du;
            du.uu = 0uL;
            du.dbl = dbl;
            sign = 1 - ((int)(du.uu >> 62) & 2);
            man = (du.uu & 4503599627370495uL);
            exp = ((int)(du.uu >> 52) & 2047);
            if (exp == 0)
            {
                isFinite = true;
                if (man != 0uL)
                {
                    exp = -1074;
                }
            }
            else if (exp == 2047)
            {
                isFinite = false;
                exp = 2147483647;
            }
            else
            {
                isFinite = true;
                man |= 4503599627370496uL;
                exp -= 1075;
            }
        }

        private static double GetDoubleFromParts(int sign, int exp, ulong man)
        {
            BigRational.DoubleUlong du;
            du.dbl = 0.0;
            if (man == 0uL)
            {
                du.uu = 0uL;
            }
            else
            {
                int cbitShift = BigRational.CbitHighZero(man) - 11;
                if (cbitShift < 0)
                {
                    man >>= -cbitShift;
                }
                else
                {
                    man <<= cbitShift;
                }
                exp += 1075;
                if (exp >= 2047)
                {
                    du.uu = 9218868437227405312uL;
                }
                else if (exp <= 0)
                {
                    exp--;
                    if (exp < -52)
                    {
                        du.uu = 0uL;
                    }
                    else
                    {
                        du.uu = man >> -exp;
                    }
                }
                else
                {
                    du.uu = ((man & 4503599627370495uL) | (ulong)((ulong)((long)exp) << 52));
                }
            }
            if (sign < 0)
            {
                du.uu |= 9223372036854775808uL;
            }
            return du.dbl;
        }

        private static int CbitHighZero(ulong uu)
        {
            int result;
            if ((uu & 18446744069414584320uL) == 0uL)
            {
                result = 32 + BigRational.CbitHighZero((uint)uu);
            }
            else
            {
                result = BigRational.CbitHighZero((uint)(uu >> 32));
            }
            return result;
        }

        private static int CbitHighZero(uint u)
        {
            int result;
            if (u == 0u)
            {
                result = 32;
            }
            else
            {
                int cbit = 0;
                if ((u & 4294901760u) == 0u)
                {
                    cbit += 16;
                    u <<= 16;
                }
                if ((u & 4278190080u) == 0u)
                {
                    cbit += 8;
                    u <<= 8;
                }
                if ((u & 4026531840u) == 0u)
                {
                    cbit += 4;
                    u <<= 4;
                }
                if ((u & 3221225472u) == 0u)
                {
                    cbit += 2;
                    u <<= 2;
                }
                if ((u & 2147483648u) == 0u)
                {
                    cbit++;
                }
                result = cbit;
            }
            return result;
        }
    }
}
