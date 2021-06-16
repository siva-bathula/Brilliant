using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class Congruentnpown : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/nn-congruence/");
        }

        void ISolve.Solve()
        {
            int count = 0;
            for (int i = 1; i < 100; i++) {
                int nn = i;
                int counter = 1;
                while (true)
                {
                    if (counter == i)
                    {
                        break;
                    }
                    nn *= i;
                    nn %= 100;
                    counter++;
                }

                if ((nn - i) % 100 == 0) {
                    count++;
                }
            }

            GenericDefs.DotNet.QueuedConsole.WriteFinalAnswer(string.Format("Number of positive integers less than 100, satisfying the congruence is :: {0}", count));
        }
    }
}
