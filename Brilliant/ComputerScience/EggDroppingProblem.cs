using GenericDefs.DotNet;
using System;

namespace Brilliant.ComputerScience
{
    public class EggDroppingProblem : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("");
        }

        void ISolve.Solve()
        {
            EggDroppingReloaded();
        }

        /// <summary>
        /// https://brilliant.org/practice/dynamic-programming-level-3-challenges/?problem=super-strong-eggs-i
        /// </summary>
        void Part1()
        {
            int nEggs = 0;
            int nFloors = 100;

            long nTrials = 0;
            long nOptimal = long.MaxValue;
            while (++nEggs <100)
            {
                nTrials = EggDrop(nEggs, nFloors);
                QueuedConsole.WriteImmediate("Number of trials : {0} with {1} Eggs", nTrials, nEggs);
                if(nOptimal == Math.Min(nOptimal, nTrials))
                {
                    QueuedConsole.WriteImmediate("Optimal solution for {0} floors is : {1}", nFloors, nOptimal);
                    break;
                }
                nOptimal = Math.Min(nOptimal, nTrials);
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/super-strong-eggs-ii/
        /// </summary>
        void Part2()
        {
            int nEggs = 2;
            int nFloors = 100;
            QueuedConsole.WriteImmediate("Number of trials : {0} with {1} Eggs", EggDrop(nEggs, nFloors), nEggs);
        }

        /// <summary>
        /// https://brilliant.org/problems/super-strong-eggs-iii/
        /// </summary>
        void Part3()
        {
            int nEggs = 3;
            int nFloors = 100;
            QueuedConsole.WriteImmediate("Number of trials : {0} with {1} Eggs", EggDrop(nEggs, nFloors), nEggs);
        }

        /// <summary>
        /// https://brilliant.org/practice/egg-dropping/?problem=dynamic-programming-2
        /// </summary>
        void Part4()
        {
            int nEggs = 3;
            int nFloors = 105;
            QueuedConsole.WriteImmediate("Number of trials : {0} with {1} Eggs", EggDrop(nEggs, nFloors), nEggs);
        }

        /// <summary>
        /// https://brilliant.org/problems/egg-dropping-reloaded/
        /// </summary>
        void EggDroppingReloaded()
        {
            int nEggs = 2;
            int nFloors = 100;
            QueuedConsole.WriteImmediate("Number of trials : {0} with {1} Eggs", EggDrop(nEggs, nFloors), nEggs);
            nEggs = 3;
            QueuedConsole.WriteImmediate("Number of trials : {0} with {1} Eggs", EggDrop(nEggs, nFloors), nEggs);
        }

        long EggDrop(long n, long k)
        {
            /* A 2D table where every eggFloor[i][j] will represent minimum
            number of trials needed for i eggs and j floors. */
            long[,] eggFloor = new long[n + 1, k + 1];
            long res;
            long i, j, x;

            // We need one trial for one floor and 0 trials for 0 floors
            for (i = 1; i <= n; i++)
            {
                eggFloor[i, 1] = 1;
                eggFloor[i, 0] = 0;
            }

            // We always need j trials for one egg and j floors.
            for (j = 1; j <= k; j++) eggFloor[1, j] = j;

            // Fill rest of the entries in table using optimal substructure
            // property
            for (i = 2; i <= n; i++)
            {
                for (j = 2; j <= k; j++)
                {
                    eggFloor[i, j] = long.MaxValue;
                    for (x = 1; x <= j; x++)
                    {
                        res = 1 + Math.Max(eggFloor[i - 1, x - 1], eggFloor[i, j - x]);
                        if (res < eggFloor[i, j]) eggFloor[i, j] = res;
                    }
                }
            }

            // eggFloor[n][k] holds the result
            return eggFloor[n, k];
        }
    }
}
