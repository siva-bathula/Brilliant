using System;
using System.Collections.Generic;
using System.Linq;
using GenericDefs.DotNet;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class Palindrome
    {
        public static char[] HiddenPalindrome(string input)
        {
            char[] a = input.ToCharArray();
            int low = int.MaxValue;
            int upper = int.MinValue;
            for (int i = 0; i < input.Length; i++)
            {
                int start = i;
                int end = i;
                while (start >= 0 && end < input.Length)
                {
                    if (a[start] == a[end])
                    {
                        if (end - start > upper - low)
                        {
                            upper = end;
                            low = start;
                        }
                        end++;
                        start--;
                    }
                    else
                    {
                        break;
                    }

                }

            }
            for (int i = 0; i < input.Length; i++)
            {
                int start = i;
                int end = i + 1;
                while (start >= 0 && end < input.Length)
                {
                    if (a[start] == a[end])
                    {
                        if (end - start > upper - low)
                        {
                            upper = end;
                            low = start;
                        }
                        end++;
                        start--;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            return a;
        }

        public static int LongestHiddenPalindrome(string seq)
        {
            int Longest = 0;
            List<int> l = new List<int>();
            int i = 0;
            int palLen = 0;
            int s = 0;
            int e = 0;
            while (i < seq.Length)
            {
                if (i > palLen && seq[i - palLen - 1] == seq[i])
                {
                    palLen += 2;
                    i += 1;
                    continue;
                }
                l.Add(palLen);
                Longest = Math.Max(Longest, palLen);
                s = l.Count - 2;
                e = s - palLen;
                bool found = false;
                for (int j = s; j > e; j--)
                {
                    int d = j - e - 1;
                    if (l[j] == d)
                    {
                        palLen = d;
                        found = true;
                        break;
                    }
                    l.Add(Math.Min(d, l[j]));
                }
                if (!found)
                {
                    palLen = 1;
                    i += 1;
                }
            }
            l.Add(palLen);
            Longest = Math.Max(Longest, palLen);
            return Longest;
        }

        public static bool IsPalindrome(string s)
        {
            char[] chS = s.ToCharArray();
            Array.Reverse(chS);
            string rev = new string(chS);

            return string.Equals(s, rev);
        }

        public static int ConvertToPalindromeMinInsertions(char[] str, int n)
        {
            // Create a table of size n*n. table[i][j] will store
            // minumum number of insertions needed to convert str[i..j]
            // to a palindrome.
            int[,] table = new int[n, n];
            int l, h, gap;

            Enumerable.Range(0, n).ForEach(x => { Enumerable.Range(0, n).ForEach(y => { table[x, y] = 0; }); });

            // Fill the table
            for (gap = 1; gap < n; ++gap)
                for (l = 0, h = gap; h < n; ++l, ++h)
                    table[l, h] = (str[l] == str[h]) ? table[l + 1, h - 1] : (Math.Min(table[l, h - 1], table[l + 1, h]) + 1);

            // Return minimum number of insertions for str[0..n-1]
            return table[0, n - 1];
        }
    }
}