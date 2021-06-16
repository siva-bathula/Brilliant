using GenericDefs.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions.Algorithms.DP;
namespace Brilliant.ComputerScience._4
{
    public class DiceWithKSides : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/dice-with-k-sides/");
        }

        void ISolve.Solve()
        {
            int T = Dice.FindWays(20, 7, 12) + Dice.FindWays(31, 6, 4) + Dice.FindWays(15, 3, 7) + Dice.FindWays(111, 17, 7) + Dice.FindWays(17, 3, 57)
                + Dice.FindWays(1, 2, 2) + Dice.FindWays(10, 17, 12) + Dice.FindWays(9, 3, 0);

            Console.WriteLine("T is :: {0}", T);

            Console.ReadKey();
        }
    }
}