using System;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class Strings
    {
        public static int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target)) return 0;
                return target.Length;
            }
            if (string.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++) distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                    distance[currentRow, j] = Math.Min(Math.Min(
                                distance[previousRow, j] + 1,
                                distance[currentRow, j - 1] + 1),
                                distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }

        /// <summary>
        /// Damerau-Levenshtein distance
        /// </summary>
        public class DamerauLevensteinMetric
        {
            private const int DEFAULT_LENGTH = 255;
            private int[] _currentRow;
            private int[] _previousRow;
            private int[] _transpositionRow;

            public DamerauLevensteinMetric()
                : this(DEFAULT_LENGTH)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="maxLength"></param>
            public DamerauLevensteinMetric(int maxLength)
            {
                _currentRow = new int[maxLength + 1];
                _previousRow = new int[maxLength + 1];
                _transpositionRow = new int[maxLength + 1];
            }

            /// <summary>
            /// Damerau-Levenshtein distance is computed in asymptotic time O((max + 1) * min(first.length(), second.length()))
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public int GetDistance(string first, string second, int max)
            {
                int firstLength = first.Length;
                int secondLength = second.Length;

                if (firstLength == 0)
                    return secondLength;

                if (secondLength == 0) return firstLength;

                if (firstLength > secondLength)
                {
                    string tmp = first;
                    first = second;
                    second = tmp;
                    firstLength = secondLength;
                    secondLength = second.Length;
                }

                if (max < 0) max = secondLength;
                if (secondLength - firstLength > max) return max + 1;

                if (firstLength > _currentRow.Length)
                {
                    _currentRow = new int[firstLength + 1];
                    _previousRow = new int[firstLength + 1];
                    _transpositionRow = new int[firstLength + 1];
                }

                for (int i = 0; i <= firstLength; i++)
                    _previousRow[i] = i;

                char lastSecondCh = (char)0;
                for (int i = 1; i <= secondLength; i++)
                {
                    char secondCh = second[i - 1];
                    _currentRow[0] = i;

                    // Compute only diagonal stripe of width 2 * (max + 1)
                    int from = Math.Max(i - max - 1, 1);
                    int to = Math.Min(i + max + 1, firstLength);

                    char lastFirstCh = (char)0;
                    for (int j = from; j <= to; j++)
                    {
                        char firstCh = first[j - 1];

                        // Compute minimal cost of state change to current state from previous states of deletion, insertion and swapping 
                        int cost = firstCh == secondCh ? 0 : 1;
                        int value = Math.Min(Math.Min(_currentRow[j - 1] + 1, _previousRow[j] + 1), _previousRow[j - 1] + cost);

                        // If there was transposition, take in account its cost 
                        if (firstCh == lastSecondCh && secondCh == lastFirstCh)
                            value = Math.Min(value, _transpositionRow[j - 2] + cost);

                        _currentRow[j] = value;
                        lastFirstCh = firstCh;
                    }
                    lastSecondCh = secondCh;

                    int[] tempRow = _transpositionRow;
                    _transpositionRow = _previousRow;
                    _previousRow = _currentRow;
                    _currentRow = tempRow;
                }

                return _previousRow[firstLength];
            }
        }

        /// <summary>
        /// Algorithm:
        /// Compare each character with each succeeding characters.
        /// If there is a match, compare rest of the characters till they differ using an inner loop.
        /// If the length of the matched substring is greater than current max length, update max and other related values.
        /// Test Cases:
        ///  abc
        ///  abcab
        ///  aaa
        ///  abcdabcdab
        ///  123abcabc21
        /// </summary>
        /// <param name="input"></param>
        public static int FindLongestSubstring(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentException("String cannot be null");
            int max = 0;       //Length of longest substring.
            int max_start = 0; //starting index of longest substring
            int max_end = 0;   //ending index of longest substring

            int tempLen = 0;
            int m, n;

            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = i + 1; j < input.Length; j++)
                {
                    if (input[i] == input[j])
                    { //if letters match, compare characters further.
                        for (m = i, n = j; m < input.Length && n < input.Length && input[m] == input[n]; m++, n++)
                            tempLen++; //increment as you keep counting length.
                        if (tempLen > max) { //if length of the substring is greater than current max len
                            max = tempLen; //update max
                            max_start = i; //update start and end indexes of this string.
                            max_end = i + max - 1;
                        }
                        tempLen = 0; //for a fresh start in next iteration
                    }
                }
            }
            return max;
        }
    }
}