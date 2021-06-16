namespace GenericDefs.Functions.Logic
{
    public class Bitwise
    {
        public static long AND(long x, long y)
        {
            return x & y;
        }
        public static long OR(long x, long y)
        {
            return x | y;
        }
        public static long XOR(long x, long y)
        {
            return x ^ y;
        }
        public static int XOR(int x, int y)
        {
            return x ^ y;
        }
        public static long Unary(long x)
        {
            return ~x;
        }
        public static long BinaryLeftShift(long x, int bits)
        {
            return x << bits;
        }
        public static long BinaryRighttShift(long x, int bits)
        {
            return x >> bits;
        }
    }
}
