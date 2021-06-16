using GenericDefs.Classes;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics._5
{
    /// <summary>
    /// https://brilliant.org/problems/inspired-by-arjen-vreugdenhil-3/
    /// </summary>
    public class RecursiveSummation : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/inspired-by-arjen-vreugdenhil-3/");
        }

        void ISolve.Solve()
        {
            Maxxyz();
        }

        void Minxyz()
        {
            int sum = 0;
            UniqueIntegralPairs p = new UniqueIntegralPairs();
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    for (int z = 1; z <= 10; z++)
                    {
                        int[] arr = new int[] { x, y, z };

                        if (p.AddCombination(arr))
                        {
                            sum += arr.Min();
                        }
                    }
                }
            }

            QueuedConsole.WriteFinalAnswer("Sum : " + sum);
        }

        /// <summary>
        /// https://brilliant.org/problems/inspired-by-inspired-by-me/
        /// </summary>
        void Maxxyz()
        {
            int sum = 0;
            UniqueIntegralPairs p = new UniqueIntegralPairs();
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    for (int z = 1; z <= 10; z++)
                    {
                        int[] arr = new int[] { x, y, z };

                        if (p.AddCombination(arr))
                        {
                            sum += arr.Max();
                        }
                    }
                }
            }

            QueuedConsole.WriteFinalAnswer("Sum : " + sum);
        }
    }
}