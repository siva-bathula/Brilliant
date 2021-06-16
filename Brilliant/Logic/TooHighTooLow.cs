using GenericDefs.DotNet;

namespace Brilliant.Logic
{
    public class TooHighTooLow : ISolve, IBrilliant
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
            //Part1();
            //Part2();
            _1023();
        }

        /// <summary>
        /// https://brilliant.org/problems/guess-that-number/
        /// </summary>
        void Part1()
        {
            QueuedConsole.WriteFinalAnswer("Solution to part 1 : {0}", Solve(10, 3));
        }

        /// <summary>
        /// https://brilliant.org/problems/dont-guess-2-high-or-2-low-2-often/
        /// </summary>
        void Part2()
        {
            QueuedConsole.WriteFinalAnswer("Solution to part 2 : {0}", Solve(2, 2));
        }

        int Solve(int H, int L)
        {
            if (H == 0) return L + 1;
            else if (L == 0) return H + 1;
            return Solve(H - 1, L) + Solve(H, L - 1) + 1;
        }

        /// <summary>
        /// https://brilliant.org/problems/where-did-comrade-otto-go/
        /// </summary>
        void _1023()
        {
            for(int h = 1; h <= 10; h++)
            {
                for (int l = 1; l <= 10; l++)
                {
                    int s = Solve(h, l);
                    if (s >= 1023 && (h + l) <= 11) { QueuedConsole.WriteImmediate("h:{0}, l:{1}, max: {2}", h, l, s); }
                }
            }
        }
    }
}
