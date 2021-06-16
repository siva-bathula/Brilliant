using GenericDefs.DotNet;
using GenericDefs.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// The prime counting function, denoted by pi(x) counts the number of primes less or equal to x.
    /// Consider the process of continually finding the value of of pi(x) until it equals zero.
    /// 1000 has a pi chain of length 8 and 20 has a pi chain of length 5.
    /// How many integers have a pi chain of length 9?
    /// </summary>
    public class PiChains : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/pi-chains/");
        }

        void ISolve.Solve()
        {
            int xMax = 1000000;
            List<int> primes = Prime.GeneratePrimesNaiveNMax(xMax);
            
            Dictionary<int, int> pix = new Dictionary<int, int>();
            int x = 0;
            while (true)
            {
                x++;
                if (x == xMax) break;
                pix.Add(x, primes.Count(y => y <= x));
            }
            x = 1;
            List<int> pichain9 = new List<int>();
            while (true)
            {
                x++;
                if (x == xMax) break;
                int pichain = 0;
                int tp = x;
                while (true)
                {
                    tp = pix[tp];
                    pichain++;
                    if (tp == 1) { pichain++; break; }
                }
                if (pichain == 9) pichain9.Add(x);
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Integers with pichain 9 : {0}", pichain9.Count));
        }
    }
}