using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Classes;
namespace Brilliant.Combinatorics._5
{
    public class PaperCorrection : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/who-will-check-your-paper/");
        }

        void ISolve.Solve()
        {
            int tT = 7;
            long totalOutcomes = 0, favOutcomes = 0;
            for (int a = 1; a <= tT; a++)
            {
                for (int b = 1; b <= tT; b++)
                {
                    for (int c = 1; c <= tT; c++)
                    {
                        for (int d = 1; d <= tT; d++)
                        {
                            totalOutcomes++;

                            if ((new List<int>() { a, b, c, d }).Distinct<int>().Count() == 3)
                            {
                                favOutcomes++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Numerator :: {0}", favOutcomes);
            Console.WriteLine("Denominator :: {0}", totalOutcomes);
            Console.WriteLine("Probability :: {0}", (double)favOutcomes / totalOutcomes);
            Console.WriteLine("The value of k :: {0}", (double)favOutcomes * 49 / (36 * totalOutcomes));


            Console.ReadKey();
        }
    }
}