using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// Generic Euler's Totient solver.
    /// a) Find all n such that phi(n) = N. Given N. Also solve find first 20 or 50 (nmax) n.
    /// </summary>
    public class EulerTotientSolver : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/wiki/eulers-totient-function/");
        }

        void ISolve.Solve() {
            SolveForAllnWithGivenPhi(60, 10);
            //SolveForGivenNumber();
        }

        /// <summary>
        /// Euler totient function of the Euler totient function of twelve raised to the power of twelve.
        /// </summary>
        void SolveForGivenNumber() {
            long num = (long)Math.Pow(12, 6);

            long phi = GenericDefs.Functions.EulerTotient.CalculateTotient(num);
            Console.WriteLine("1st order phi :: {0}", phi);
            phi = GenericDefs.Functions.EulerTotient.CalculateTotient(phi);
            Console.WriteLine("2nd order phi :: {0}", phi);
            Console.ReadKey();
        }

        void SolveForAllnWithGivenPhi(int phiValue, int nmax) {
            int nCount = 0;
            long iter = 2;
            while (nCount <= nmax) {
                if (GenericDefs.Functions.EulerTotient.CalculateTotient(iter) == phiValue) {
                    nCount++;
                    Console.WriteLine("{0} has phi value :: {1}, number's found :: {2}", iter, phiValue, nCount);
                }
                ++iter;
            }

            Console.ReadKey();
        }

    }
}