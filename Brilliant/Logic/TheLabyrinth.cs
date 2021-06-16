using GenericDefs.Classes.Logic;
using System;
using System.Collections.Generic;
namespace Brilliant.Logic
{
    public class TheLabyrinth : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/the-labyrinth/");
        }

        void ISolve.Solve()
        {
            int m = 2, N = 100, startChamber = 1;
            while(m <= 10) {
                Console.WriteLine("---------------------------");
                Console.WriteLine("Starting iteration for square :: {0} x {0}", m);
                Labyrinth l = new Labyrinth(m);

                Console.WriteLine("Created labyrith of {0} x {0}", m);
                Console.WriteLine("Now moving from chamber 1...");
                l.StartMoving(startChamber, N);
                Console.WriteLine("Total number of moves :: {0}", N);

                List<int> uc = l.GetUnEnteredChambers();
                foreach (int c in uc) {
                    Console.WriteLine("This chamber was never entered :: {0}", c);
                }
                Console.WriteLine("Ending iteration for square :: {0} x {0}", m);
                Console.WriteLine("---------------------------");
                m++;
            }
            Console.ReadKey();
        }
    }
}