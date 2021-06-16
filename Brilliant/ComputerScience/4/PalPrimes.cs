using GenericDefs.DotNet;
using GenericDefs.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// A palindromic-prime or PalPrime is a prime number that is also a palindrome. The first few PalPrimes are 2, 3, 5, 7, 11, 101, 131, 151, 181..... 
    /// Let S be the sum of the digits of the largest PalPrime N such that N is less than 10^9 . What is the value of S ?
    /// </summary>
    public class PalPrimes : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/palprimes-3/");
        }

        void ISolve.Solve()
        {
            long n = (long)Math.Pow(10, 9);
            long palPrime = 0;
            List<int> primes = Prime.GeneratePrimesNaiveNMax(100000);
            while (true)
            {
                n--;
                if (n % 2 == 0 || n % 3 == 0 || n % 5 == 0 || n % 7 == 0 || n % 11 == 0 || n % 13 == 0 || n % 17 == 0 || n % 19 == 0) continue;
                if (n.ToString().Equals(string.Join("", n.ToString().Reverse())))
                {
                    if (Prime.IsPrime(n, primes))
                    {
                        palPrime = n;
                        break;
                    }
                }
            }

            QueuedConsole.WriteFinalAnswer(string.Format("Palprime : {0}, Sum of digits S : {1}", palPrime, MathFunctions.DigitSum(palPrime)));
        }
    }
}