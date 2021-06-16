using System.Collections.Generic;
using System.Numerics;

namespace GenericDefs.Functions.NumberTheory
{
    public class Fibonacci
    {
        public static List<BigInteger> FirstNSequence(int seq)
        {
            List<BigInteger> b = new List<BigInteger>();
            BigInteger first = new BigInteger(0);
            BigInteger second = new BigInteger(1);
            b.Add(first); b.Add(second);

            for (int i = 2; i <= seq; i++)
            {
                BigInteger bi = new BigInteger();
                bi = first + second;
                b.Add(bi);
                first = second;
                second = bi;
            }
            return b;
        }

        public static BigInteger NthFibonacciNumber(int N)
        {
            BigInteger first = new BigInteger(0);
            BigInteger second = new BigInteger(1);

            for (int i = 2; i <= N; i++)
            {
                BigInteger bi = new BigInteger();
                bi = first + second;
                first = second;
                second = bi;
            }
            return second;
        }

        public static BigInteger SumFirstNSequence(List<BigInteger> b)
        {
            BigInteger bSum = new BigInteger(0);
            foreach (BigInteger bi in b)
            {
                bSum += bi;
            }
            return bSum;
        }

        public static List<int> ZeckendorfIndices(BigInteger b)
        {
            List<int> indices = new List<int>();

            Dictionary<int, BigInteger> fibNumbers = new Dictionary<int, BigInteger>();
            BigInteger first = new BigInteger(1);
            BigInteger second = new BigInteger(1);
            int nFibIndex = 0;
            fibNumbers.Add(nFibIndex, first);
            fibNumbers.Add(++nFibIndex, second);
            bool breakAfter = false;
            while (true)
            {
                BigInteger bi = new BigInteger();
                bi = first + second;
                fibNumbers.Add(++nFibIndex, bi);
                first = second;
                second = bi;
                if (breakAfter) break;
                if (bi > b) breakAfter = true;
            }
            
            Dictionary<int, BigInteger>.KeyCollection keys = fibNumbers.Keys;
            BigInteger nextNum = b;
            while (true)
            {
                int prevKey = -1;
                foreach (int key in keys)
                {
                    if (fibNumbers[key] > nextNum) { break; }
                    prevKey = key;
                }
                indices.Add(prevKey);
                nextNum -= fibNumbers[prevKey];
                if (nextNum == 0) break;
            }

            return indices;
        }
    }

    public class FibonacciGenerator
    {
        int _a0, _a1;
        int _currentN = 0;
        public FibonacciGenerator(int a0=0, int a1=1)
        {
            _a0 = a0;
            _a1 = a1;
            _currentN = 1;

            _curr = a1;
            _prev = a0;
        }

        BigInteger _curr, _prev, _next;

        public int CurrentN() { return _currentN; }
        public BigInteger CurrentTerm() { return _curr;  }
        public BigInteger Next()
        {
            _next = _curr + _prev;

            _prev = _curr;
            _curr = _next;
            _currentN++;

            return _next;
        }
    }
}