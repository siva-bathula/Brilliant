using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using GenericDefs.Classes;
using GenericDefs.Functions;
using System.Numerics;
namespace Brilliant.ComputerScience
{
    public class BinaryPrimes19thSmallestNumber : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/binary-primes/");
        }

        //void ISolve.Solve()
        //{
        //    List<int> primes = Prime.GeneratePrimesSieveOfSundaram(100000);
        //    primes.Sort();

        //    int i = 0, N = 19;
        //    for (int n = 1; ; n++){
        //        string bin = Binary.ToBinary(n);
        //        BigInteger b = BigInteger.Parse(bin);
        //        //mpz_t t = new mpz_t(bin);
                
        //        int f = Convert.ToInt32(bin);
        //        if (primes.Contains(f) && primes.Contains(n)) {
        //            i++;
        //            Console.WriteLine("{0}th smallest number is :: {1}", i, Binary.ToDecimal(bin));
        //            if (i == N) { break; }
        //        }
        //    }
        //    Console.ReadKey();
        //}

        void ISolve.Solve()
        {
            List<long> primes = Prime.LongGeneratePrimesSieveOfSundaram(100000000);
            primes.Sort();

            int i = 0, N = 19;
            for (int n = 1; ; n++)
            {
                string bin = Binary.ToBinary(n);

                long f = Convert.ToInt64(bin);
                if (primes.Contains(f) && primes.Contains(n))
                {
                    i++;
                    Console.WriteLine("{0}th smallest number is :: {1}", i, Binary.ToDecimal(bin));
                    if (i == N) { break; }
                }
            }
            Console.ReadKey();
        }
    }
}