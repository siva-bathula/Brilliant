using GenericDefs.DotNet;

namespace Brilliant.NumberTheory._5
{
    /// <summary>
    /// It is given that p is a given prime number such that : For some positive integers x and y, 
    /// x^3 + 2y^3 + 4z^3 - 6xyz = 1;
    /// find the largest value of.
    /// </summary>
    public class CubicDiophantineFamily : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/cubic-diophantine-family/");
        }

        void ISolve.Solve()
        {
            long xAns = 0;
            bool found = false;
            for (long x = 500; x <= 4000; x++)
            {
                if (x % 2 == 0) continue;
                for (long y = 1; y <= 10000; y++)
                {
                    long lhs = (x * x * x) + (2 * y * y * y) - 1;
                    for (long z = 1; z <= 10000; z++)
                    {
                        long rhs = 6 * x * y * z - (4 * z * z * z);
                        if (z > 10 && rhs < 0) break;
                        if (lhs == rhs)
                        {
                            xAns = x;
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                if (found) break;
            }

            QueuedConsole.WriteFinalAnswer("Unique solution for x is :: " + xAns);
        }
    }
}