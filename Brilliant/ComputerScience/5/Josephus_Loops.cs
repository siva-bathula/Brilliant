using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions.Logic;
namespace Brilliant.ComputerScience._5
{
    public class Josephus_Loops : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/practice/loops-level-4-5-challenges/?p=2");
        }

        void ISolve.Solve()
        {
            long T = Josephus.Solve(999, 1, 11) + Josephus.Solve(121, 1, 12) + Josephus.Solve(2, 1, 1)
                + Josephus.Solve(15, 1, 16) + Josephus.Solve(99, 1, 3);
            Console.WriteLine("Remainder is :: {0}", T);
            Console.ReadKey();
        }
    }
}