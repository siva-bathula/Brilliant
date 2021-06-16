using GenericDefs.DotNet;
using System.Numerics;

namespace Brilliant.ComputerScience._4
{
    public class Ackermann : ISolve
    {

        /// <summary>
        /// https://brilliant.org/problems/too-much-recursion/
        /// </summary>
        void TooMuchRecursion()
        {
            BigInteger A42 = AckermannFunction(4, 2);

            QueuedConsole.WriteImmediate("A(4,2) mod 1000 : {0}", (A42 % 1000).ToString());
        }

        public static BigInteger AckermannFunction(BigInteger m, BigInteger n)
        {
            var stack = new OverflowlessStack<BigInteger>();
            stack.Push(m);
            while (!stack.IsEmpty)
            {
                m = stack.Pop();
            skipStack:
                if (m == 0) n = n + 1;
                else if (m == 1) n = n + 2;
                else if (m == 2) n = n * 2 + 3;
                else if (n == 0) {
                    --m;
                    n = 1;
                    goto skipStack;
                }
                else {
                    stack.Push(m - 1);
                    --n;
                    goto skipStack;
                }
            }
            return n;
        }

        void ISolve.Solve()
        {
            TooMuchRecursion();
        }

        void ISolve.Init()
        {
            
        }
    }
}
