using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace Brilliant.ComputerScience._5
{
    public class LookingForPatterns : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/looking-for-patterns/");
        }

        void ISolve.Solve()
        {
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.looking-for-patterns.txt", true);
            List<string> strings = new List<string>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    strings.Add(s);
                }
            }

            int S = 0;
            foreach(string s in strings)
            {
                int sMax = 0, sCur = 0;
                int n = 0;
                while (true)
                {
                    char ch = s[n];
                    if(ch == 'a' || ch == 'b' || ch == 'c') {
                        sCur++;
                    } else {
                        if (sCur > sMax) sMax = sCur;
                        sCur = 0;
                    }

                    n++;
                    if (n == s.Length) break;
                }
                if (sCur > sMax) sMax = sCur;

                S += sMax;
            }
            QueuedConsole.WriteFinalAnswer("Last three digits of S : {0}", S % 1000);
        }
    }
}