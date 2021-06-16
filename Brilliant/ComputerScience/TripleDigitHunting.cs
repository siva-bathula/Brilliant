using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience
{
    /// <summary>
    /// Triple Digit Hunting. 
    /// 1234567891011121314151617181920.........9979989991000.
    /// In the giant number above, how many 3-digit substrings consist of the same digit?
    /// </summary>
    public class TripleDigitHunting : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/triple-digit-hunting/?group=0LeSh1O5mL43");
        }

        void ISolve.Solve()
        {
            StringBuilder giantStr = new StringBuilder();
            for (int i = 1; i < 1001; i++)
            {
                giantStr.Append("" + i);
            }
            int tssCount = 0;
            string appStr = giantStr.ToString();
            for (int j = 0; j < 10; j++)
            {
                string s = string.Format("{0}{1}{2}", j, j, j);
                appStr = giantStr.ToString();
                int sc = Regex.Matches(appStr, s).Count;
                //sc = countOccurences(s, giantStr.ToString());
                tssCount += sc;

                //Console.WriteLine("Total number of 3-digit substrings of {0} :: {1}", s, sc);
            }

            appStr = giantStr.ToString();
            tssCount = 0;
            for (int i = 0; i < appStr.Length - 2; i++) {
                if (appStr[i] == appStr[i + 1] && appStr[i+1] == appStr[i + 2]) {
                    Console.WriteLine("{0}, {1}{2}{3}", i, appStr[i], appStr[i+1], appStr[i+2]);
                    tssCount++;
                }
            }

            Console.WriteLine("Total number of 3-digit substrings :: " + tssCount);
            Console.ReadKey();
        }

        int countOccurences(string needle, string haystack)
        {
            int hLength = haystack.Length;
            int hmnLength = haystack.Length - haystack.Replace(needle, "").Length;
            int nLength = needle.Length;
            int occurrences = (hmnLength) / nLength;
            Console.WriteLine("Haystack length : {0}, needle length: {1}, haystack length with needle replace : {2}, occurrences : {3}", hLength, nLength, hmnLength, occurrences);
            return occurrences;
        }
    }
}