using GenericDefs.Classes;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience
{
    public class WordSearches : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/wordsearches-take-long-unless-you-code/");
        }

        void ISolve.Solve()
        {
            Solve1();
        }

        /// <summary>
        /// https://brilliant.org/problems/wordsearches-take-long-unless-you-code/
        /// </summary>
        void Solve1() {

            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.word_search_grid.words.txt", true);
            Dictionary<string, bool> words = new Dictionary<string, bool>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    words.Add(s.Replace(" ", ""), false);
                }
            }

            stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.word_search_grid.word-grid.txt", true);
            List<string> grid = new List<string>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    grid.Add(s.Replace(" ", ""));
                }
            }

            int finalAnswer = 0;
            int row = 0;
            foreach (string s in grid) {
                row++;
                foreach (var kvp in words.ToArray()) {
                    if (kvp.Value) continue;
                    if (s.IndexOf(kvp.Key) >= 0) {
                        finalAnswer += row + s.IndexOf(kvp.Key) + 1;
                        words[kvp.Key] = true;
                        continue;
                    }
                    string s1 = Strings.Reverse(s);
                    if (s1.IndexOf(kvp.Key) >= 0)
                    {
                        finalAnswer += row + (s1.Length - s1.IndexOf(kvp.Key));
                        words[kvp.Key] = true;
                        continue;
                    }
                }
            }

            QueuedConsole.WriteFinalAnswer(string.Format("Position sum : {0}", finalAnswer));
        }
    }
}