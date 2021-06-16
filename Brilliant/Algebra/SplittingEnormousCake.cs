using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Algebra
{
    /// <summary>
    /// A mathematician has an enormous cake and many friends with whom to share it.First, she gives a quarter of the cake to the first friend.
    /// She then gives a fifth of the remaining cake to the next friend.She then gives a sixth of the now remaining cake to the next friend.
    /// This process continues until she gives a hundredth of the remaining cake to the last friend.She finally takes the portion that is left over for herself.
    /// What percentage of the original cake does she have?
    /// </summary>
    public class SplittingEnormousCake : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/day-18-splitting-an-enormous-cake/");
        }

        void ISolve.Solve()
        {
            decimal cakeleft = 10000000000, cakeInit = 10000000000;
            for (int i = 4; i <= 100; i++) {
                cakeleft = cakeleft * (i - 1) / i;
            }

            Console.WriteLine("Percentage cake left : {0}", (cakeleft / cakeInit) * 100);
            Console.ReadKey();
        }
    }
}