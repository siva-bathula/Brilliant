using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Classes;
namespace Brilliant.Combinatorics._5
{
    public class SameScoreProbability : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/probability-of-real-life-situations/");
        }

        void ISolve.Solve()
        {
            int totalScore = 0;
            long numerator = 0, denominator = 0;
            while (totalScore < 31)
            {
                int sumScore = 0;
                UniqueIntegralPairs pairs = new UniqueIntegralPairs();
                for (int m = 0; m <= 10; m++)
                {
                    sumScore = m;
                    if (sumScore > totalScore)
                    {
                        break;
                    }
                    for (int p = 0; p <= 10; p++)
                    {
                        sumScore = m + p;
                        if (sumScore > totalScore)
                        {
                            break;
                        }
                        for (int c = 0; c <= 10; c++)
                        {
                            sumScore = m + p + c;
                            if (sumScore == totalScore)
                            {
                                pairs.AddCombination(new ArrayList() { m, p, c });
                            }
                            else if (sumScore > totalScore)
                            {
                                break;
                            }
                        }
                    }
                }
                
                int[] mScores = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                List<UniqueIntegralPairs.Combination> cList = pairs.GetCombinations();
                denominator += cList.Count;
                foreach (UniqueIntegralPairs.Combination c in cList) {
                    mScores[(int)c.Pair[0]] = mScores[(int)c.Pair[0]] + 1;
                }

                foreach (int i in mScores) {
                    numerator += i * i;
                }

                totalScore++;
                if (totalScore == 31) break;
            }

            denominator *= denominator;
            Console.WriteLine("Numerator :: {0}", numerator);
            Console.WriteLine("Denominator :: {0}", denominator);
            Console.ReadKey();
        }
    }
}