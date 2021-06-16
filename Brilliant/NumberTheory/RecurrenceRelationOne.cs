using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Functions;

using System.Numerics;
namespace Brilliant.NumberTheory
{
    /// <summary>
    /// a0 = 0, a1 = 1, ai = 2a1-i - ai-2 +2 ; i>= 2;
    /// Find a1000.
    /// </summary>
    public class RecurrenceRelationOne : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/recurrence-relation-5/");
        }

        void ISolve.Solve()
        {
            BigInteger ai = 0, a0 = 0, a1 = 1;

            int N = 1000;
            for (int i = 2; i <= N; i++)
            {
                ai = 2 + (2 * a1) - a0;
                a0 = a1;
                a1 = ai;
            }
            Console.WriteLine("a1000 is : {0} ", ai);
            Console.ReadKey();
        }
    }
}