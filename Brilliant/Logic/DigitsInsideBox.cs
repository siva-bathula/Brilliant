using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using System.Linq;

namespace Brilliant.Logic
{
    public class DigitsInsideBox : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/practice/logic-warmups-level-5-challenges/?p=2");
        }

        void ISolve.Solve()
        {
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] digitCounters = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            int i = 0;
            ConsecutiveStreakCounter csc = new ConsecutiveStreakCounter();
            while (true)
            {
                string digitStr = string.Join("", numbers) + string.Join("", digitCounters);
                var counts = digitStr.GroupBy(c => c)
                    .OrderBy(c => c.Key)
                    .ToDictionary(grp => grp.Key, grp => grp.Count());

                bool atleastOneChanged = false;
                for (int j = 0; j < 9; j++)
                {
                    char chJ = char.Parse(j + 1 + "");
                    if (digitCounters[j] != (digitStr.Length - counts[chJ]))
                    {
                        atleastOneChanged = true;
                        digitCounters[j] = (digitStr.Length - counts[chJ]);
                    }
                }

                if (!atleastOneChanged) csc.Add(0);
                if (atleastOneChanged) csc.Add(1);

                i++;
                if (i == 9) i = 0;
                if (csc.IsValueZero && csc.CurrentStreak > 100) break;
            }

            QueuedConsole.WriteImmediate(string.Format("Concatenated numbers : {0}", string.Join("", digitCounters)));
            QueuedConsole.ReadKey();
        }
    }
}