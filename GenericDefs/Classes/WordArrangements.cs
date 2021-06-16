using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Classes
{
    /// <summary>
    /// Creates a black list of values not allowed againt each index.
    /// Add all indices, if there is no black listed value against an index, add zero to that index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlackList<T>
    {
        Dictionary<int, T[]> NotAllowed = new Dictionary<int, T[]>();
        public T[] Get(int index)
        {
            if (NotAllowed.ContainsKey(index)) return NotAllowed[index];
            return null;
        }
        public void Add(int index, T[] notallowed)
        {
            NotAllowed.Add(index, notallowed);
        }
    }

    public class WordArrangements
    {
        /// <summary>
        /// Checks for positive arrangements.
        /// </summary>
        Func<string, bool> pChecker;
        private SimpleCounter PositiveArrangements;
        bool _needsChecker = false;
        int? _wordDepth = null;
        public BigInteger GetPositiveArrangements()
        {
            return PositiveArrangements.GetCount();
        }

        public WordArrangements() { }

        /// <summary>
        /// Check for positive arrangements based on the func evaluator.
        /// </summary>
        /// <param name="func"></param>
        public WordArrangements(Func<string, bool> func, int? wordDepth = null)
        {
            pChecker = func;
            PositiveArrangements = new SimpleCounter();
            _needsChecker = true;
            if (wordDepth.HasValue) _wordDepth = wordDepth.Value;
        }

        public BigInteger CountWordArrangements(string word, BlackList<char> bl)
        {
            List<char> wordCh = word.ToList();
            SimpleCounter s = new SimpleCounter();

            Dictionary<char, int> repeats = new Dictionary<char, int>();
            foreach (char c in wordCh)
            {
                if (!repeats.ContainsKey(c)) repeats.Add(c, 1);
                else repeats[c] += 1;
            }

            foreach (char c in wordCh)
            {
                if (bl.Get(0) != null)
                {
                    if (bl.Get(0).Contains(c)) continue;
                }
                List<char> unused = wordCh.Select(item => item).ToList();
                unused.Remove(c);
                RecursionWordArrangement(wordCh, new List<char>() { c }, unused, 1, ref s, bl);
            }

            return s.GetCount();
        }

        private void RecursionWordArrangement(List<char> word, List<char> used, List<char> unused, int depth, ref SimpleCounter s, BlackList<char> bl)
        {
            List<char> after;
            List<char> unusedafter;

            foreach (char c in word)
            {
                if (bl.Get(depth) != null)
                {
                    if (bl.Get(depth).Contains(c)) continue;
                }
                if (!unused.Contains(c)) continue;

                after = used.Select(item => item).ToList();
                unusedafter = unused.Select(item => item).ToList();

                after.Add(c);
                unusedafter.Remove(c);
                if (depth == word.Count - 1)
                {
                    s.Increment();
                    return;
                }
                else
                {
                    RecursionWordArrangement(word, after, unusedafter, depth + 1, ref s, bl);
                }
            }
        }

        public UniqueArrangements<char> UniqueWords { get; set; }
        public long GetAllWordArrangements(string word, BlackList<char> bl, List<char> fullWordWithRepeats)
        {
            _needsUnusedCheck = true;

            List<char> wordCh = word.ToList();
            UniqueArrangements<char> u = new UniqueArrangements<char>();

            RecursionWordArrangement(wordCh, new List<char>(), fullWordWithRepeats, 0, ref u, bl);

            UniqueWords = u;
            return u.GetCount();
        }

        public long GetAllCoinTossArrangements(string repeats, BlackList<char> bl)
        {
            _needsUnusedCheck = false;

            List<char> wordCh = repeats.ToList();
            UniqueArrangements<char> u = new UniqueArrangements<char>();

            foreach (char c in wordCh)
            {
                if (bl.Get(0) != null)
                {
                    if (bl.Get(0).Contains(c)) continue;
                }
                RecursionWordArrangement(wordCh, new List<char>() { c }, null, 1, ref u, bl);
            }

            return u.GetCount();
        }

        private bool _needsUnusedCheck = true;
        private void RecursionWordArrangement(List<char> word, List<char> used, List<char> unused, int depth, ref UniqueArrangements<char> u, BlackList<char> bl)
        {
            List<char> after;
            List<char> unusedafter = null;

            foreach (char c in word)
            {
                if (bl != null)
                {
                    if (bl.Get(depth) != null) { if (bl.Get(depth).Contains(c)) continue; }
                }
                if (_needsUnusedCheck && !unused.Contains(c)) continue;

                after = used.Select(item => item).ToList();
                if (_needsUnusedCheck)
                {
                    unusedafter = unused.Select(item => item).ToList();
                    unusedafter.Remove(c);
                }

                after.Add(c);
                if ((!_wordDepth.HasValue && depth == word.Count - 1) || (_wordDepth.HasValue && depth == _wordDepth.Value - 1))
                {
                    if (u.Add(after))
                    {
                        if (_needsChecker)
                        {
                            if (pChecker.Invoke(string.Join("", after)))
                            {
                                PositiveArrangements.Increment();
                            }
                        }
                    }
                }
                else { RecursionWordArrangement(word, after, unusedafter, depth + 1, ref u, bl); }
            }
        }
    }
}
