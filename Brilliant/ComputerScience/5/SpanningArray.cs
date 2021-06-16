using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brilliant.ComputerScience._5
{
    public class SpanningArray : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/spanning-array/");
        }

        void ISolve.Solve()
        {
            List<int> A = new List<int>();
            List<int> B = new List<int>();
            using (var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.spanning-array.txt", true))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string s = string.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        s = (s.Substring(1, s.Length - 2)).Replace(" ","");
                        var sSplit = (s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                        if (A.Count == 0) sSplit.ForEach(x => A.Add(int.Parse(x)));
                        else sSplit.ForEach(x => B.Add(int.Parse(x)));
                    }
                }
            }

            int longestSpan = int.MinValue;
            for(int span = 2; span <= A.Count; span++)
            {
                for(int i = 0; i<= A.Count - span; i++)
                {
                    if(A.GetRange(i,span).Sum() == B.GetRange(i, span).Sum()) {
                        longestSpan = span;
                        break;
                    }
                }
            }

            QueuedConsole.WriteImmediate("Longest Span : {0}", longestSpan);
        }
    }
}