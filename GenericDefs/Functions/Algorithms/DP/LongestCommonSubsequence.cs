using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class LongestCommonSubsequence
    {
        public static void LCS(char[] str1, char[] str2, ref int lcsLength, ref string lcsString)
        {
            int[,] l = new int[str1.Length, str2.Length];
            int lcs = -1;
            string substr = string.Empty;
            int end = -1;

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] == str2[j])
                    {
                        if (i == 0 || j == 0)
                        {
                            l[i, j] = 1;
                        }
                        else
                            l[i, j] = l[i - 1, j - 1] + 1;
                        if (l[i, j] > lcs)
                        {
                            lcs = l[i, j];
                            end = i;
                        }

                    }
                    else
                        l[i, j] = 0;
                }
            }

            for (int i = end - lcs + 1; i <= end; i++)
            {
                substr += str1[i];
            }
            lcsLength = lcs;
            lcsString = substr;
        }

        public static int GetLCS(string str1, string str2)
        {
            int[,] table;
            return GetLCSInternal(str1, str2, out table);
        }

        private static int GetLCSInternal(string str1, string str2, out int[,] matrix)
        {
            matrix = null;

            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return 0;
            }

            int[,] table = new int[str1.Length + 1, str2.Length + 1];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                table[i, 0] = 0;
            }
            for (int j = 0; j < table.GetLength(1); j++)
            {
                table[0, j] = 0;
            }

            for (int i = 1; i < table.GetLength(0); i++)
            {
                for (int j = 1; j < table.GetLength(1); j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                        table[i, j] = table[i - 1, j - 1] + 1;
                    else
                    {
                        if (table[i, j - 1] > table[i - 1, j])
                            table[i, j] = table[i, j - 1];
                        else
                            table[i, j] = table[i - 1, j];
                    }
                }
            }

            matrix = table;
            return table[str1.Length, str2.Length];
        }
    }

    public class LongestIncreasingSubsequence
    {
        /// <summary>
        /// <seealso cref="http://www.8bitavenue.com/2011/11/dynamic-programming-longest-increasing-sub-sequence/"/>
        /// </summary>
        /// <param name="A"></param>
        /// <param name="length"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int[] GetLongestIncreasingSubsequence(int[] A, out int length, out int end)
        {
            // This variable saves the length of the longest 
            // increasing subsequence satisfying the condition
            // i < j and A[i] <= A[j] 
            int maxi = 0;

            // This variable saves the length of the longest 
            // increasing subsequence across all (j) values.
            int maxj = 0;

            // This variable points to the (j) value at which 
            // the longest increasing subsequences ends. 
            // It is going to be the start point to print the
            // individual elements in the solution using back
            // pointers array b[j]
            end = 0;

            int n = A.Length;
            int[] L = new int[n];
            int[] b = new int[n];

            // The length of the increasing subsequence 
            // ending at position (1) is also (1) because it only 
            // contains one number which is A[1]
            L[0] = 1;

            // We already initialized L(1) so we compute the 
            // rest of L(j) values as (j) goes from (2) to (n)
            for (int j = 1; j < n; j++)
            {
                // Recall that b[j] points to the array 
                // element in (A) from which the current 
                // subsequence was extended so we initially 
                // set b[j] = j in case the current sequence 
                // was not extended (started new sequence) 
                // then we update that later in case it was 
                // indeed extended
                b[j] = j;

                // For all i < j find the longest 
                // increasing sub sequence from which the 
                // current sequence ending at (j) was extended 
                for (int i = 0; i < j; i++)
                {
                    // Pay attention here. We have two conditions.
                    // The first condition A[i] <= A[j] is used to
                    // make sure the sequence ending at (j) is indeed
                    // increasing by extending a previous subsequence
                    // ending at some position (i). The second condition
                    // L(i) > maxi is used to find the longest increasing
                    // subsequence from which to extend the current sequence
                    // at (j).
                    if (A[i] <= A[j] && L[i] > maxi)
                    {
                        // Update maxi whenever a longer 
                        // subsequence is found
                        maxi = L[i];

                        // Save the (i) value at which the 
                        // longest sub sequence was found
                        // This (i) value is a back pointer
                        // needed to construct the actual solution
                        b[j] = i;
                    }
                }

                // Refer back to the explanation at the 
                // beginning of the article. We showed that
                // L(j) = 1 + Max (L(i)) where i < j and A[i] <= A[j]
                L[j] = 1 + maxi;

                // Reset the variable maxi for the next (j) value
                maxi = 0;

                // Just populating L(j) is not the solution however 
                // we need to find the largest value in L(j)
                if (L[j] >= maxj)
                {
                    // Keep updating until we get the largest value
                    maxj = L[j];

                    // Save the (j) value at which the longest 
                    // increasing subsequence (the solution we 
                    // are looking for) ends. This value is used 
                    // as the start point to generate the actual 
                    // solution using back pointers.
                    end = j;
                }
            }

            length = maxj;
            return b;
        }

        static public int LongestIncreasingSeq(int[] s)
        {
            int[] l = new int[s.Length];  // DP table for max length[i]
            int[] p = new int[s.Length];  // DP table for predeccesor[i]
            int max = int.MinValue;

            l[0] = 1;

            for (int i = 0; i < s.Length; i++)
                p[i] = -1;

            for (int i = 1; i < s.Length; i++)
            {
                l[i] = 1;
                for (int j = 0; j < i; j++)
                {
                    if (s[j] < s[i] && l[j] + 1 > l[i])
                    {
                        l[i] = l[j] + 1;
                        p[i] = j;
                        if (l[i] > max)
                            max = l[i];
                    }
                }
            }
            return max;
        }
    }
}