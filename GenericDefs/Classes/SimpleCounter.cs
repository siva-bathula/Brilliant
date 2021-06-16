using System.Numerics;
namespace GenericDefs.Classes
{
    public class SimpleCounter
    {
        BigInteger b = new BigInteger(0);
        public void Increment() { b++; }
        public void Increment(int n) { b += n; }
        public void Increment(BigInteger n) { b += n; }
        public static SimpleCounter operator ++(SimpleCounter obj) { obj.b++; return obj; }
        public static SimpleCounter operator --(SimpleCounter obj) { obj.b--; return obj; }
        public void Decrement() { b--; }
        public void Decrement(int n) { b -= n; }
        public void Decrement(BigInteger n) { b -= n; }
        public BigInteger GetCount() { return b; }
    }
}