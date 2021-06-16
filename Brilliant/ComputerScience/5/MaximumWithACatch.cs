using GenericDefs.DotNet;
using System.Collections.Generic;

namespace Brilliant.ComputerScience._5
{
    public class MaximumWithACatch : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/maximum-with-a-catch/");
        }

        void ISolve.Solve()
        {
            List<int[]> lists = new List<int[]>();
            lists.Add(new int[] { 868344, 5702470, 4164686, 6909034, 9938057, 2325752, 2873259 });
            lists.Add(new int[] { 3281382, 6656834, 2949466, 8313351, 1693510, 7965124 });
            lists.Add(new int[] { 6170425, 3484246, 2039180, 761139, 2069051, 3570668, 2348921 });
            lists.Add(new int[] { 9698787, 37364, 8030130, 6391829, 6969085, 1247325 });
            lists.Add(new int[] { 8416661, 9855125, 6534506, 9285004, 8073946, 3215543, 6194037 });
            lists.Add(new int[] { 8012002, 1583648, 7049465, 4639438 });
            lists.Add(new int[] { 6407988, 3097441, 2604562, 5094765, 6581687, 7160093, 5855903 });
            lists.Add(new int[] { 5678899, 3457932, 1234793, 1298400 });

            Recursion(lists, 0, 0);
            QueuedConsole.WriteFinalAnswer("Maximum possible value of S : {0}", maxS);
        }

        int maxS = 0;
        void Recursion(List<int[]> lists, int depth, int before)
        {
            foreach (int x in lists[depth])
            {
                int after = before;
                int mod = x;
                mod %= 123;
                mod *= x;
                mod %= 123;
                mod *= x;
                mod %= 123;
                mod += 3;
                mod %= 123;
                after += mod;
                after %= 123;
                if (depth == lists.Count - 1) {
                    if (after > maxS) maxS = after;
                }
                else {
                    Recursion(lists, depth + 1, after);
                }
            }
        }
    }
}