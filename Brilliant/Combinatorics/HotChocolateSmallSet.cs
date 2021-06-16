using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics
{
    /// <summary>
    /// Extremely Hot Chocolate!
    /// Today, Brian is generous and wants to offer a hot chocolate to all of his friends. Unfortunately, Brian has enough money only for n-1 hot chocolates, so he decided to play a game.
    /// The game starts by placing his  friends around a table, and numbering them from 0 to n in clockwise order.
    /// Person 0 starts off with an empty plastic cup.
    /// Every person who directly receives the cup from another person around the table, wins a hot chocolate and leaves the game.
    /// Once person p is gone, the cup is taken by the person to the left of p.
    /// The game ends when there is only one person left, who unfortunately will not receive a hot chocolate.
    /// In the first move, person 0 gives the cup to person 1.
    /// Let us denote as Ln the last person in a game of people.
    /// </summary>
    public class HotChocolateSmallSet : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/extremely-hot-chocolate/");
        }

        void ISolve.Solve()
        {
            int n = 5;

            List<int> set = new List<int>();
            for (int i = 0; i < n; i++) {
                set.Add(i);
            }
            set.Sort();

            int curIndex = 0;
            int personsRemoved = 0;
            while (set.Count > 1) {
                set.RemoveAt(curIndex + 1);
                personsRemoved++;
                curIndex++;
                if (curIndex == set.Count - 1) { set.RemoveAt(0); curIndex = 0; personsRemoved++; }
                if (curIndex > set.Count - 1) { curIndex = 0; }

                if (personsRemoved % 10000 == 0) {
                    Console.WriteLine("Total persons left at the table is : {0}", set.Count);
                    Console.WriteLine("The first person is : {0}", set[0]);
                    Console.WriteLine("The last person is : {0}", set[set.Count -1 ]);
                }
            }

            Console.WriteLine("Total persons left at the table is : {0}", set.Count);
            Console.WriteLine("The last person is : {0}", set[0]);
            Console.ReadKey();
        }
    }
}