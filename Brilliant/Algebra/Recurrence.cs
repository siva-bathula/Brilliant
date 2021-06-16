using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs;
using GenericDefs.Classes;
using GenericDefs.DotNet;

namespace Brilliant.Algebra
{
    public class Recurrence : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/still-recurring-theme/");
        }

        void ISolve.Solve()
        {
            long aip0 = 1, aip1 = 0, aip2 = 2;
            long aip3 = 0;
            int set = 3;
            while (true) {
                aip3 = aip1 + aip0 - aip2;
                aip0 = aip1;
                aip1 = aip2;
                aip2 = aip3;
                set++;
                if (set == 2015) break;
            }

            string answer = string.Format("a2016 is : {0}, ", aip3);
            QueuedConsole.WriteFinalAnswer(answer);
        }

    }
}