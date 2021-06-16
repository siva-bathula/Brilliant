using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class ALotOfPhis : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/a-lot-of-phis/");
        }

        void ISolve.Solve() {
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.a-lot-of-phis-totients.txt", true);
            List <long> numbers = new List<long>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    numbers.Add(Convert.ToInt64(s));
                }
            }

            BigInteger phiSum = new BigInteger(0);
            foreach (long n in numbers) {
                phiSum += GenericDefs.Functions.EulerTotient.CalculateTotient(n);
            }

            Console.WriteLine("Sum of totients is :: {0}", phiSum);
            Console.WriteLine("Quotient is :: {0}", (long)(phiSum / 1000000007));
            Console.WriteLine("Remainder is :: {0}", (phiSum % 1000000007).ToString());
            Console.ReadKey();
        }
    }
}
