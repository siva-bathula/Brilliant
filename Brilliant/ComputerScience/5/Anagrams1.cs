using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    /// <summary>
    /// Two strings are considered anagrams if the characters of one string can be rearranged to form the second string. For instance, "cat" and "act" 
    /// are anagrams because the letters of "cat" can be rearranged to spell "act". "cat" and "cat" are also anagrams of each other.
    /// This file contains a line-separated list of strings.Let A be the number of strings in the file that are an anagram of another string in the file. What is A?
    /// </summary>
    public class Anagrams1 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/anagrams/");
        }

        void ISolve.Solve() {
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.anagrams-1.txt", true);
            List<string> words = new List<string>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    words.Add(string.Concat(s.OrderBy(c => c)));
                }
            }

            int anagramCount = 0;
            for (int i = 0; i < words.Count; i++) {
                for (int j = 0; j < words.Count; j++)
                {
                    if (i == j) continue;

                    else if (words[i] == words[j]) {
                        anagramCount++;
                        break;
                    }
                }
            }
            
            Console.WriteLine("Anagrams is :: {0}", anagramCount);
            Console.ReadKey();
        }
    }
}
