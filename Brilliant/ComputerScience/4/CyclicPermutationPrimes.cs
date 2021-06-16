using GenericDefs.DotNet;
using GenericDefs.Functions;

namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// We define an integer n to be a cyclic prime if all its cyclic permutations are also prime numbers. 
    /// Find the smallest cyclic prime number whose cyclic permutations are all larger than 500.
    /// As an explicit example, 199933 is a cyclic prime because 199933, 999331, 993319, 933199, 331999, 319993 are all prime numbers.
    /// </summary>
    public class CyclicPermutationPrimes : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/one-four-two-eight-five-seven/");
        }

        void ISolve.Solve()
        {
            int n = 500;
            bool found = true;
            while (true)
            {
                string start = "" + n;
                string next = start;
                found = true;
                while (true)
                {
                    int nextNum = int.Parse(next);
                    if (nextNum <= 500 || !Prime.IsPrime(nextNum))
                    {
                        found = false;
                        break;
                    }

                    next = next.Substring(1, start.Length - 1) + next[0];
                    if (start.Equals(next)) break;
                }
                if (found) break;
                else n++;
            }

            QueuedConsole.WriteFinalAnswer(string.Format("Smallest cyclic prime number whose cyclic permutations are all larger than 500 :: {0}", n));
        }
    }
}