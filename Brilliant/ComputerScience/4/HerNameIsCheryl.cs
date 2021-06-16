using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions;
using GenericDefs.Functions.Algorithms.DP;
using System.Numerics;

namespace Brilliant.ComputerScience._4
{
    public class HerNameIsCheryl : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/her-name-is-cheryl/");
        }

        void ISolve.Solve()
        {
            int n = 0;
            bool found = false;
            while (true) {
                n++;
                BigInteger innerN = n;
                int counter = 0;
                while (true) {
                    innerN = innerN + Numbers.ReverseANumber(innerN);
                    if (!Palindrome.IsPalindrome(innerN.ToString()))
                    {
                        counter++;
                    }
                    else {
                        break;
                    }

                    if (counter == 25) {
                        found = true;
                        break;
                    }
                }

                if (found) break;
            }

            Console.WriteLine("Smallest near symmetric integer is :: {0}", n);
            Console.ReadKey();
        }

    }
}