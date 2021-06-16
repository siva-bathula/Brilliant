using GenericDefs.DotNet;
using GenericDefs.OtherProjects;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler._100._91_100
{
    public class Problem96 : IProblem
    {
        void IProblem.Solve()
        {
            var stream = Utility.GetEmbeddedResourceStream("ProjectEuler.data._100._96.p096_sudoku.txt", true);
            List<string> sudokuProblems = new List<string>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = string.Empty;
                string buffer = string.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.StartsWith("Grid"))
                    {
                        if (!string.IsNullOrEmpty(buffer)) sudokuProblems.Add(buffer.Replace("0", "."));
                        buffer = string.Empty;
                        continue;
                    }
                    buffer += s;
                }
                if (!string.IsNullOrEmpty(buffer)) sudokuProblems.Add(buffer.Replace("0", "."));
            }

            int finalAnswer = 0;
            string[] keys = new string[] { "A1", "A2", "A3" };
            int count = 0;
            foreach (string problem in sudokuProblems)
            {
                string sol = (SudokuSolver.ConcatSolution(SudokuSolver.Search(SudokuSolver.ParseGrid(problem)), keys));
                QueuedConsole.WriteImmediate(string.Format("Grid {0}, Solution : {1}", count++, sol));
                finalAnswer += int.Parse(sol);
            }

            QueuedConsole.WriteFinalAnswer("Sum of digits is : " + finalAnswer);
        }
    }
}