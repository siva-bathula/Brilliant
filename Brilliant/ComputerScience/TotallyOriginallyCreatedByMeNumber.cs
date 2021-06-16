using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// The solution below gives the answer at the last break point. But the code has bugs. 
    /// </summary>
    public class TotallyOriginallyCreatedByMeNumber : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/totallyoriginallycreatedbyme-number/");
        }

        int _currentDigitCount = 0;
        void ISolve.Solve()
        {
            int digitCount = 1;
            BigInteger prod = new BigInteger(1);
            string actualNumber = string.Empty;
            while (digitCount > 0) {
                digitCount++;
                _currentDigitCount = digitCount;
                BigInteger curLHS = new BigInteger(0); BigInteger curRHS = new BigInteger(0);
                if (digitCount == 6) { break; }
                if (Loop(digitCount, ref curLHS, ref curRHS, ref prod, ref actualNumber)) {
                    break;
                }
            }

            Console.WriteLine("Product is :: {0}", prod);
            Console.WriteLine("The number is :: {0}", actualNumber);
            Console.ReadKey();
        }

        bool Loop(int digitsLeft, ref BigInteger curLHS, ref BigInteger curRHS, ref BigInteger prod, ref string actualNumber) {
            //The solution below gives the answer at the last break point. But the code has bugs. 
            bool found = false, def = false;
            BigInteger lLHS = new BigInteger(0); BigInteger lRHS = new BigInteger(0);
            int lDigitsleft = digitsLeft;
            for (int a=1; a<= 9; a++) {
                found = def;
                lDigitsleft = digitsLeft;
                lLHS = curLHS;
                lRHS = curRHS;
                lRHS += a * (BigInteger)Math.Pow(10, lDigitsleft - 1);
                lLHS += (BigInteger)Math.Pow(a, a);
                lDigitsleft--;
                if (lDigitsleft > 0) {
                    found = Loop(lDigitsleft, ref lLHS, ref lRHS, ref prod, ref actualNumber);
                }
                if (lDigitsleft == 0) {
                    if (lLHS.Equals(lRHS)) {
                        found = true;
                    }
                }
                if (found) {
                    if (digitsLeft == _currentDigitCount) {
                        if (curLHS.Equals(curRHS)) {
                            
                        }
                    }
                    prod *= a;
                    actualNumber = a.ToString() + actualNumber;
                    break;
                }
            }

            curLHS = lLHS;
            curRHS = lRHS;
            if (found) { return true; }
            else { return false; }
        }
    }
}