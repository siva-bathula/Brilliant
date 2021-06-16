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
    /// Summing Sequences
    /// This file contains a list of integers. Let N(s) equal the number of contiguous subsequences within the list that have a sum of s.
    /// Let T = N(11) + N(2) + N(9) + N(14).What are the last three digits of T?
    /// Details and assumptions. The sum of a single number is itself.
    /// </summary>
    public class SummingSequences : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/summing-sequences/");
        }

        void ISolve.Solve()
        {
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.summing-sequences.txt", true);
            List<int> numbers = new List<int>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    numbers.Add(Convert.ToInt32(s));
                }
            }

            int T = N(numbers, 11) + N(numbers, 2) + N(numbers, 9) + N(numbers, 14);

            Console.WriteLine("Last 3 digits of T is :: {0}", T % 1000);
            Console.ReadKey();
        }

        int N(List<int> numbers, int s)
        {
            int subseqCount = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                int curSeqSum = 0;
                for (int j = i; j < numbers.Count; j++)
                {
                    curSeqSum += numbers[j];
                    if (curSeqSum == s) subseqCount++;
                }
            }
            return subseqCount;
        }
    }
}