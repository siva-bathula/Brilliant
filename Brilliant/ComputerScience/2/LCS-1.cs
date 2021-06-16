using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._2
{
    /// <summary>
    /// sequence_1 = GGCAAGGTACTTCCGGTCTTAATGAATGGCCGGG AAAGGTACGCACGCGGTATGGGGGGGTGAAGGGGCGAATAGACAGGC TCCCCTCTCACTCGCTAGGAGGCAATTGTATAAGAATGCATACTGCA TCGATACATAAAACGTCTCCATCGCTTGCCCAAGTTGTGAAGTGTCT ATCACCCCTAGGCCCGTTTCCCGCA
    /// sequence_2 = GGCTGGCGTTTTGAATCCTCGGTCCCCCTTGTCT ATCCAGATTAATCCAATTCCCTCATTTAGGACCCTACCAAGTCAACA TTGGTATATGAATGCGACCTCGAAGAGGCCGCCTAAAAATGACAGTG GTTGGTGCTCTAAACTTCATTTGGTTAACTCGTGTATCAGCGCGATA GGCTGTTAGAGGTTTAATATTGTAT
    /// </summary>
    public class LCS_1 : ISolve, IBrilliant
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
            string sequence_1 = "GGCAAGGTACTTCCGGTCTTAATGAATGGCCGGGAAAGGTACGCACGCGGTATGGGGGGGTGAAGGGGCGAATAGACAGGCTCCCCTCTCACTCGCTAGGAGGCAATTGTATAAGAATGCATACTGCATCGATACATAAAACGTCTCCATCGCTTGCCCAAGTTGTGAAGTGTCTATCACCCCTAGGCCCGTTTCCCGCA";
            string sequence_2 = "GGCTGGCGTTTTGAATCCTCGGTCCCCCTTGTCTATCCAGATTAATCCAATTCCCTCATTTAGGACCCTACCAAGTCAACATTGGTATATGAATGCGACCTCGAAGAGGCCGCCTAAAAATGACAGTGGTTGGTGCTCTAAACTTCATTTGGTTAACTCGTGTATCAGCGCGATAGGCTGTTAGAGGTTTAATATTGTAT";

            int lcs = 0;
            string lcsString = string.Empty;
            GenericDefs.Functions.Algorithms.DP.LongestCommonSubsequence.LCS(sequence_1.ToCharArray(), sequence_2.ToCharArray(), ref lcs, ref lcsString);

            lcs = GenericDefs.Functions.Algorithms.DP.LongestCommonSubsequence.GetLCS(sequence_1, sequence_2);

            Console.WriteLine("LCS length is :: {0}, lcs is :: {1}" , lcs, lcsString);
            Console.ReadKey();
        }
    }
}
