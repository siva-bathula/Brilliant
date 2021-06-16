using GenericDefs.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// Let f(n) denote the Euler's totient function. Then there exists only one integer a less eq. 10pow10 such that
    /// f(a) = f(a+1) + f(a+2)
    /// Find the remainder when that number is divided by 1729.
    /// 
    /// https://brilliant.org/wiki/eulers-totient-function/
    /// </summary>
    public class EulersTotient10pow10adiv1729 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/this-is-why-we-code/?group=yct3djA7sZpU&ref_id=1134521");
        }

        void ISolve.Solve()
        {
            long phi0 = 0, phi1 = 0, phi2 = 0, phi = 0;
            long a = 0;
            //List<int> primes = Prime.GeneratePrimesNaive(1000000000);
            List<int> primes = Prime.GeneratePrimesSieveOfSundaram(100000);
            List<PrimeFactor> primeF = Prime.GeneratePrimeFactors(1, primes);
            phi0 = CalculatePhi(1, primeF);
            primeF = Prime.GeneratePrimeFactors(2, primes);
            phi1 = CalculatePhi(2, primeF);
            primeF = Prime.GeneratePrimeFactors(3, primes);
            phi2 = CalculatePhi(3, primeF);
            for (long i = 4; ; i++) {
                primeF = Prime.GeneratePrimeFactors(i, primes);
                phi = CalculatePhi(i, primeF);

                phi0 = phi1;
                phi1 = phi2;
                phi2 = phi;

                if (phi0 == phi1 && phi1 == phi2) {
                    a = i - 2;
                    break;
                }
            }

            Console.WriteLine("a is :: {0} ", a);
            Console.WriteLine("remainder when divided by 1729 is :: {0}", (a % 1729));
            Console.ReadKey();
        }

        long CalculatePhi(long phiVal, List<PrimeFactor> primeF)
        {
            foreach (PrimeFactor pf in primeF)
            {
                phiVal = (phiVal * (pf.Prime - 1)) / pf.Prime;
            }
            return phiVal;
        }
    }
}