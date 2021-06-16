using GenericDefs.Classes;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    /// <summary>
    /// Let N is a Sangar Number if it satisfies the following conditions :
    /// a.There are integers A,B,C such that AC X BC = NC. AC = A-append-C
    /// b. N,A,B,C don't have leading zeros. 
    /// c. 1 (greater than equal to) N,A,B,C (less than equal to) 10^9.
    /// </summary>
    public class SangarNumber : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/sangar-number/");
        }

        void ISolve.Solve()
        {
            int N = 100000000;
            int[] logDigits = new int[N + 1];

            long n = 0;

            while (n < N)
            {
                n++;
                logDigits[n] = (int)Math.Floor(Math.Log10(n) + 1.0) == 0 ? 1 : (int)Math.Floor(Math.Log10(n) + 1.0);
            }

            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.sangar.txt", true);
            List<long> numbers = new List<long>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    numbers.Add(Convert.ToInt64(s));
                }
            }

            List<long> cList = new List<long>();
            long c = 1;
            while (c < N)
            {
                long dc = (long)Math.Pow(10, logDigits[c]);
                if (dc < 10) dc = 10;
                if ((c * (c - 1)) % dc == 0)
                {
                    cList.Add(c);
                }

                c++;
            }

            cList.Sort();

            List<long> sangarList = new List<long>();
            int nLeftToCheck = numbers.Count;

            string logDump = string.Empty;
            foreach (long ni in numbers)
            {
                nLeftToCheck--;
                //if (ni <= 161462945) continue;

                SpecialConsole console = QueuedConsole.GetSpecialConsole(2, 2);
                console.LazySuppressWrite(string.Format("Numbers left to check : {0}", nLeftToCheck));
                
                bool sFound = false;
                foreach (long ci in cList)
                {
                    int dc = logDigits[ci];

                    long tenpowdc = (long)Math.Pow(10, dc);

                    for (int bi = 1; bi <= ni; bi++)
                    {
                        long numerator = (ni - (bi * ci)) - (ci * (ci - 1) / tenpowdc);
                        long denominator = (bi * tenpowdc) + ci;

                        if (numerator < denominator) break;

                        if (numerator % denominator == 0)
                        {
                            string s = string.Format("Sangar Number found. a: {0}, b: {1}, c: {2}, n : {3}", numerator / denominator, bi, ci, ni);
                            logDump += s;
                            Console.WriteLine(s);
                            sangarList.Add(ni);
                            sFound = true;
                            break;
                        }
                    }
                    if (sFound) { break; }
                }
            }

            Console.WriteLine("Number of Sangar numbers is :: {0}", sangarList.Count);
            if (!string.IsNullOrEmpty(logDump)) Logger.Log(logDump);
            Console.ReadKey();
        }
    }
}