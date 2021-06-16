using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Brilliant.NumberTheory
{
    /// <summary>
    /// Find the last (1,2) digits of n pow n summation.
    /// </summary>
    public class LastDigitOfNpowNSummation : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/last-digit-of-nn/?group=YOl38OAt73BH");
        }

        void ISolve.Solve()
        {
            BigInteger bSum = new BigInteger();
            bSum = 0;
            int nMax = 1001;
            for (int n = 1; n < nMax; n++)
            {
                BigInteger bsi = new BigInteger();
                bsi = 1;
                for(int j = 1; j<= n; j++) { 
                    bsi = bsi * n;
                }
                bSum += bsi;
                Console.WriteLine("Last two digits for n = {0}, is : {1}, sn : {2}, Total sum : {3}", n, bSum % 100, bsi, bSum);
            }

            Console.WriteLine(" Last two digits is : {0}", bSum % 100);
            Console.ReadKey();
        }
    }
}