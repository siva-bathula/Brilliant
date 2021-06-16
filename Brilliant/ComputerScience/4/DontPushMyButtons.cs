using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// problem/dont-push-my-buttons/
    /// 
    /// Since Jenny is a lazy operator she wants to minimize the number of times she presses the buttons? What is the minimum number of times 
    /// Jenny has to press the buttons in order to completely destroy 466559 items?
    /// </summary>
    public class DontPushMyButtons : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/dont-push-my-buttons/");
        }

        void ISolve.Solve()
        {
            int N = 466559;
            int pushes = 0;
            string sequence = string.Empty;
            while (N > 0)
            {
                PushButton(ref N, ref sequence, ref pushes);
            }

            Console.WriteLine("The sequence of moves is :: {0}", sequence);
            Console.WriteLine("Number of button pushes is :: {0}", pushes);
            Console.ReadKey();
        }

        void PushButton(ref int n, ref string sequence, ref int pushes)
        {
            if (n == 0) return;

            if (n % 3 == 0) { n /= 3; sequence += "b"; }
            else if (n > 1 && (n - 1) % 3 == 0 && n % 2 == 0) { --n; sequence += "r"; }
            else if (n > 2 && (n - 2) % 3 == 0 && (n - 1) % 2 == 0) { --n; sequence += "r"; }
            else if (n % 2 == 0) { n /= 2; sequence += "g"; }
            else
            {
                --n; sequence += "r";
                //if (n > 0 && (n % 2 == 0 && n % 3 != 0)) { --n; sequence += "->r"; pushes++; }
            }

            pushes++;
            if (n > 0) { sequence += "->"; }
        }
    }
}