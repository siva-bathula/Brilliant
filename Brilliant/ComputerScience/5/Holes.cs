using GenericDefs.DotNet;
using System.Collections.Generic;
using System.Linq;

namespace Brilliant.ComputerScience._5
{
    public class Holes : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/holes-medium/");
        }

        void ISolve.Solve()
        {
            List<int> input = new List<int>();
            Enumerable.Range(1, 9).ForEach(x => input.Add(x));
            input.Reverse();
            //Medium(input);
            Hard(input);
        }

        void Easy(List<int> input)
        {
            QueuedConsole.WriteImmediate("Output sequence : {0}", string.Join("->", CrossHole(CrossHole(CrossHole(input, 3), 5), 2)));
        }

        void Medium(List<int> input)
        {
            List<int> mInput = CrossHole(CrossHole(CrossHole(input, 2), 5), 3);
            mInput.Reverse();
            QueuedConsole.WriteImmediate("Input sequence : {0}", string.Join("->", mInput));
            QueuedConsole.WriteImmediate("Checking for output sequence.");
            Easy(mInput);
        }

        void Hard(List<int> input)
        {
            List<int> holes = new List<int>();
            string holesStr = "3 5 9 8 5 1 8 5 4 5 3 8 6 7 2 6 3 9 2 5 2 7 8 6 7 3 6 9 2 5";
            (holesStr.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries)).ForEach(x => holes.Add(int.Parse(x)));
            holes.Reverse();
            int n = 0;
            while (true)
            {
                input = CrossHole(input, holes[n]);
                n++;
                if (n == holes.Count) break;
            }
            input.Reverse();
            QueuedConsole.WriteImmediate("Input sequence : {0}", string.Join("->", input));
        }

        List<int> CrossHole(List<int> inputArr, int depth)
        {
            List<int> output = inputArr.GetRange(depth, inputArr.Count - depth);
            output.AddRange(Enumerable.Reverse(inputArr.GetRange(0, depth)));

            return output;
        }
    }
}
