using GenericDefs.Classes;
using GenericDefs.DotNet;
using GenericDefs.Functions.Algorithms.DP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics
{
    public class BaseballMultiHit : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/a-very-expensive-random-number-generator/");
        }

        void ISolve.Solve()
        {
            long NMultiHitGames = 0;
            Action<List<int>> CountMultiHit = delegate (List<int> ai)
            {
                NMultiHitGames += ai[1] + ai[2] + ai[3];
            };
            SimpleCounter c = Knapsack.Variation3.Solutions(new int[] { -1, 1, 2, 3 }, 654, CountMultiHit);
            double average = NMultiHitGames * 1.0 / (long)c.GetCount();
            QueuedConsole.WriteImmediate("{0}", average);
        }
    }
}
