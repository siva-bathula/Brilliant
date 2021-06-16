using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GenericDefs.DotNet
{
    public static class Strings
    {
        public static int CountOccurences(string needle, string haystack)
        {
            return (haystack.Length - haystack.Replace(needle, "").Length) / needle.Length;
        }

        public static string Reverse(string s)
        {
            return string.Join("", s.Reverse());
        }

        public static string LeftRotateShift(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(shift) + key.Substring(0, shift);
        }

        public static string RightRotateShift(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(key.Length - shift) + key.Substring(0, key.Length - shift);
        }

        /// <summary>
        /// Works for any string length. The last chunk can be less than maxChunkSize.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxChunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        /// <summary>
        /// Works for Chunksize % String length == 0.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<string> WholeChunks(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                yield return str.Substring(i, chunkSize);
            }
        }

        /// <summary>
        /// The string of numbers succeeding the decimal point, will be split into chunk of given size,
        /// and arranged into rows having specified columns per row.
        /// </summary>
        /// <param name="columns">Columns to split into</param>
        /// <param name="chunkSize">Chunk of numbers to squeeze into one column.</param>
        /// <param name="input">The string of numbers succeeding the decimal point</param>
        /// <returns></returns>
        public static string DecimalAsTable(int columns, int chunkSize, string input)
        {
            StringBuilder sb = new StringBuilder();
            var strings = ChunksUpto(input, 10);
            int colCount = 0;
            string append = string.Empty;
            foreach (string s in strings)
            {
                colCount++;
                append += s;
                if (colCount < columns) append += " ";
                if (colCount == columns)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(append);
                    append = "";
                    colCount = 0;
                }
            }
            if (!string.IsNullOrEmpty(append))
            {
                sb.Append(Environment.NewLine);
                sb.Append(append);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Extracts all numbers from the string.
        /// </summary>
        /// <param name="str">The string containing numbers.</param>
        /// <returns>An array of the numbers as strings.</returns>
        public static List<string> ExtractNumberStrings(string str)
        {
            List<string> retVal = new List<string>();
            if (str == null) { return retVal; }

            MatchCollection matches = Regex.Matches(str, "(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in matches)
            {
                if (match.Success) { retVal.Add(match.Value); }
            }

            return retVal;
        }

        /// <summary>
        /// Extracts all numbers from the string.
        /// </summary>
        /// <param name="str">The string containing numbers.</param>
        /// <returns>An array of the numbers as strings.</returns>
        public static List<int> ExtractIntegersFromString(string str)
        {
            List<int> retVal = new List<int>();
            if (str == null) { return retVal; }
            StringBuilder sBdr = new StringBuilder();
            string buffer = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(str[i]))
                {
                    buffer += str[i];
                }
                else if (i < str.Length - 1 && str[i] == '-' && char.IsNumber(str[i + 1]))
                {
                    buffer += str[i];
                }
                else
                {
                    int bVal = 0;
                    if (int.TryParse(buffer, out bVal))
                    {
                        retVal.Add(bVal);
                    }
                    buffer = string.Empty;
                }
            }
            return retVal;
        }

        static Dictionary<char, string> CharToNumber = null;
        /// <summary>
        /// If the word is "abc" or "ABc" it will return "010203".
        /// </summary>
        /// <returns></returns>
        public static string WordToNumber(string word)
        {
            if (CharToNumber == null)
            {
                CharToNumber = new Dictionary<char, string>();

                char ch = 'a'; ch--;
                int n = 0;
                Enumerable.Range(1, 26).ForEach(x =>
                {
                    ch++; n++;
                    if (n < 10) CharToNumber.Add(ch, "0" + n);
                    else CharToNumber.Add(ch, "" + n);
                });
            }

            string wordLower = word.ToLower();

            string retVal = string.Empty;
            wordLower.ForEach(x =>
            {
                if (!CharToNumber.ContainsKey(x)) throw new ArgumentException("Only alphabets are allowed. " + x + "is not found in dictionary.");
                retVal += CharToNumber[x];
            });
            return retVal;
        }

        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        public static string ToDisplayNumber(long n)
        {
            IEnumerable<string> chunks = ChunksUpto(string.Join("", (n + "").Reverse()), 3).Reverse();
            List<string> revChunks = new List<string>();
            chunks.ForEach(c => { revChunks.Add(string.Join("", c.Reverse())); });
            return string.Join(",", revChunks);
        }
    }

    public static class StringBuilderSearchHelper
    {
        public static bool Contains(this StringBuilder haystack, string needle)
        {
            return haystack.IndexOf(needle) != -1;
        }
        public static int IndexOf(this StringBuilder haystack, string needle)
        {
            if (haystack == null || needle == null) throw new ArgumentNullException();
            if (needle.Length == 0) return 0;//empty strings are everywhere!
            if (needle.Length == 1)//can't beat just spinning through for it
            {
                char c = needle[0];
                for (int idx = 0; idx != haystack.Length; ++idx)
                    if (haystack[idx] == c) return idx;
                return -1;
            }
            int m = 0;
            int i = 0;
            int[] T = KMPTable(needle);
            while (m + i < haystack.Length)
            {
                if (needle[i] == haystack[m + i])
                {
                    if (i == needle.Length - 1) return m == needle.Length ? -1 : m;//match -1 = failure to find conventional in .NET
                    ++i;
                }
                else
                {
                    m = m + i - T[i];
                    i = T[i] > -1 ? T[i] : 0;
                }
            }
            return -1;
        }
        private static int[] KMPTable(string sought)
        {
            int[] table = new int[sought.Length];
            int pos = 2;
            int cnd = 0;
            table[0] = -1;
            table[1] = 0;
            while (pos < table.Length)
                if (sought[pos - 1] == sought[cnd]) table[pos++] = ++cnd;
                else if (cnd > 0) cnd = table[cnd];
                else table[pos++] = 0;
            return table;
        }
    }

    public static class StringSplitter
    {
        public static readonly char[] Space = new char[] { ' ' };
        public static readonly char[] Comma = new char[] { ',' };
        static readonly string[] LineBreak = new string[] { "\r\n", "\n" };
        static readonly char[] Cap = new char[] { '^' };
        public static string[] Splitter(this string input, SplitUsing split = SplitUsing.Space, StringSplitOptions sso = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentException("String cannot be null or empty.");

            char[] splitter = GetSplitter(split);
            return input.Split(splitter, sso);
        }

        private static char[] GetSplitter(SplitUsing split)
        {
            switch (split)
            {
                case SplitUsing.Comma: return Comma;
                case SplitUsing.Space: return Space;
                case SplitUsing.LineBreak: return Environment.NewLine.ToCharArray();
                case SplitUsing.PCap: return Cap;
                default: return Space;
            }
        }

        public enum SplitUsing
        {
            Space,
            Comma,
            LineBreak,
            PCap
        }
    }
}