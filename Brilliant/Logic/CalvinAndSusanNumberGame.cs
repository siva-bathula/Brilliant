using GenericDefs.DotNet;
using GenericDefs.Functions;
using System.Linq;

namespace Brilliant.Logic
{
    public class CalvinAndSusanNumberGame : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("");
        }

        void ISolve.Solve()
        {
            Part2();
        }

        /// <summary>
        /// https://brilliant.org/problems/calvin-and-susan-play-a-number-game-1/
        /// </summary>
        void Part1()
        {
            string calvinNumber = "1";
            string susanNumber = string.Empty;
            int n = 0;
            while (true)
            {
                char cN = calvinNumber[n];
                if (char.GetNumericValue(cN) == 1) susanNumber += "122";
                else if (char.GetNumericValue(cN) == 2) susanNumber += "111";
                n++;
                calvinNumber += susanNumber[n];
                if (susanNumber.Length > 2012) break;
            }
            QueuedConsole.WriteImmediate("Sum of digits from positions 2006 to 2012 ({0}) : {1}", susanNumber.Substring(2005, 7), 
                MathFunctions.DigitSum(int.Parse(susanNumber.Substring(2005, 7))));
        }

        /// <summary>
        /// https://brilliant.org/problems/calvin-and-susan-play-a-number-game-2/
        /// </summary>
        void Part2()
        {
            string calvinNumber = "1";
            string susanNumber = string.Empty;
            int n = 0;
            while (true)
            {
                char cN = calvinNumber[n];
                if (char.GetNumericValue(cN) == 1) susanNumber += "112";
                else if (char.GetNumericValue(cN) == 2) susanNumber += "111";
                n++;
                calvinNumber += susanNumber[n];
                if (susanNumber.Length > 2187) break;
            }
            calvinNumber = susanNumber.Substring(0, 2187);
            QueuedConsole.WriteImmediate("Number of times 1's appear in calvin's number : {0}", (calvinNumber.ToCharArray()).Count(x => char.GetNumericValue(x) == 1));
            
            n = 0;
            int count = 0;
            while (true)
            {
                if (calvinNumber.Substring(n, 5).Equals("11111")) count++;
                n++;
                if ((n + 5) > calvinNumber.Length) break;
            }
            QueuedConsole.WriteImmediate("Number of times five consecutive 1's appear in calvin's number : {0}", count);
        }
    }
}