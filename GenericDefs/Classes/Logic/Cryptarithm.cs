using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GenericDefs.Classes.Logic
{
    public class Cryptarithm
    {
        CryptRule[] rules;
        bool noRules = false;

        public Cryptarithm(CryptRule[] rules)
        {
            this.rules = rules;
        }

        public Cryptarithm(bool JustRecursion)
        {
            noRules = true;
        }

        public bool JustEvaluateRules(CryptCoefficients cNew)
        {
            if (noRules) return true;
            if (rules == null || rules.Count() == 0) { throw new ArgumentException("Rules were not set."); }

            bool eval = true;
            foreach (CryptRule r in rules)
            {
                r.SetCoefficients(cNew);
                eval = r.Evaluate();
                if (!eval) break;
            }

            return eval;
        }
    }

    public class CryptRecursiveSolver
    {
        /// <summary>
        /// </summary>
        /// <param name="_isdistinct">If all characters have distinct values. If cryptic word is SEVEN then S!= E and E != V and V!= N</param>
        public CryptRecursiveSolver(bool isdistinct, Cryptarithm c)
        {
            IsDistinct = isdistinct;
            _cryptarithm = c;
        }
        internal Cryptarithm _cryptarithm;
        public bool IsDistinct { get; set; }

        /// <summary>
        /// Returns first solution found, which satisfies all conditions.
        /// </summary>
        /// <param name="variables">Number of variables</param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public int[] Solve(int variables, BlackList<int> bl)
        {
            List<int> finalAnswer = new List<int>();
            for (int i = 0; i < variables; i++)
            {
                if (bl.Get(0) != null)
                {
                    if (bl.Get(0).Contains(i)) continue;
                }
                if (RecursionDistinct(new List<int>() { i }, 1, variables, bl, ref finalAnswer))
                {
                    finalAnswer.Add(i);
                    break;
                }
            }
            finalAnswer.Reverse();
            return (finalAnswer.ToArray());
        }

        /// <summary>
        /// Returns first solution found, which satisfies all conditions.
        /// </summary>
        /// <param name="variables">Number of variables</param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public UniqueArrangements<int> GetAllSolutions(int variables, BlackList<int> bl)
        {
            UniqueArrangements<int> ua = new UniqueArrangements<int>();

            int maxValue = variables;
            Enumerable.Range(0, variables).ForEach(x =>
            {
                int max = bl.Get(0).Max();
                if (max > maxValue) maxValue = max;
            });
            RecursionDistinctFullSearch(new List<int>(), 0, variables, maxValue, bl, ref ua);
            return ua;
        }

        /// <summary>
        /// Use this if more than one solution is possible.
        /// </summary>
        /// <param name="variables">Number of variables</param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public UniqueArrangements<int> GetAllSolutions(int variables, List<Iterator<int>> iterators, Func<List<int>, bool> searchAccelerator = null)
        {
            UniqueArrangements<int> ua = new UniqueArrangements<int>();
            int depth = 0;

            Func<int, bool> eval = delegate (int iter)
            {
                if (iterators[depth].IsIncrement) return iter <= iterators[depth].Range.Max;
                else return iter >= iterators[depth].Range.Min;
            };
            for (int i = iterators[depth].IsIncrement ? iterators[depth].Range.Min : iterators[depth].Range.Max;
                eval.Invoke(i);
                i += iterators[depth].IsIncrement ? 1 : -1)
            {
                RecursiveFullSearch(new List<int>() { i }, 1, variables, ref ua, iterators, searchAccelerator);
            }
            return ua;
        }

        internal void RecursiveFullSearch(List<int> used, int depth, int maxDepth, ref UniqueArrangements<int> ua,
            List<Iterator<int>> iterators, Func<List<int>, bool> searchAccelerator = null)
        {
            Func<int, bool> eval = delegate (int iter)
            {
                if (iterators[depth].IsIncrement) return iter <= iterators[depth].Range.Max;
                else return iter >= iterators[depth].Range.Min;
            };
            for (int i = iterators[depth].IsIncrement ? iterators[depth].Range.Min : iterators[depth].Range.Max;
                eval.Invoke(i);
                i += iterators[depth].IsIncrement ? 1 : -1)
            {
                if (IsDistinct) if (used.Contains(i)) continue;

                List<int> usedCopy = used.Select(item => item).ToList();
                usedCopy.Add(i);

                if (searchAccelerator != null)
                {
                    if (!searchAccelerator.Invoke(usedCopy)) continue;
                }

                if (depth == maxDepth - 1)
                {
                    int[] arr = usedCopy.ToArray();
                    CryptCoefficients cc = new CryptCoefficients(arr);
                    if (_cryptarithm.JustEvaluateRules(cc))
                    {
                        ua.Add(usedCopy);
                        break;
                    }
                    else { continue; }
                }
                else {
                    RecursiveFullSearch(usedCopy, depth + 1, maxDepth, ref ua, iterators, searchAccelerator);
                }
            }
        }

        internal bool RecursionDistinct(List<int> used, int depth, int maxDepth, BlackList<int> bl, ref List<int> finalAnswer)
        {
            bool found = false;
            List<int> usedafter;

            for (int i = 0; i < maxDepth; i++)
            {
                if (bl.Get(depth) != null)
                {
                    if (bl.Get(depth).Contains(i)) continue;
                }
                if (used.Contains(i)) continue;

                usedafter = used.Select(item => item).ToList();

                usedafter.Add(i);
                if (depth == maxDepth - 1)
                {
                    int[] arr = usedafter.ToArray();
                    CryptCoefficients cc = new CryptCoefficients(arr);
                    if (_cryptarithm.JustEvaluateRules(cc)) { found = true; }
                    else { found = false; continue; }
                }
                else
                {
                    found = RecursionDistinct(usedafter, depth + 1, maxDepth, bl, ref finalAnswer);
                }

                if (found) { finalAnswer.Add(i); break; }
                else continue;
            }

            return found;
        }

        internal void RecursionDistinctFullSearch(List<int> used, int depth, int maxDepth, int maxValue, BlackList<int> bl, ref UniqueArrangements<int> ua)
        {
            for (int i = 0; i < maxValue; i++)
            {
                if (bl.Get(depth) != null)
                {
                    if (bl.Get(depth).Contains(i)) continue;
                }
                if (used.Contains(i)) continue;

                List<int> usedafter = new List<int>(used);
                usedafter.Add(i);

                if (depth == maxDepth - 1)
                {
                    int[] arr = usedafter.ToArray();
                    CryptCoefficients cc = new CryptCoefficients(arr);
                    if (_cryptarithm.JustEvaluateRules(cc)) {
                        ua.Add(usedafter);
                    }
                    else { continue; }
                } else { RecursionDistinctFullSearch(usedafter, depth + 1, maxDepth, maxValue, bl, ref ua); }
            }
        }

        /// <summary>
        /// Use this when having a customer generator function and iterator.
        /// </summary>
        public UniqueArrangements<int> GetAllSolutions(int maxDepth, CustomIterenumerator<int> iterator)
        {
            UniqueArrangements<int> u = new UniqueArrangements<int>();
            int nextDepth = 1;
            for (int? i = iterator.MoveNext(); iterator.IsValid(); i = iterator.MoveNext())
            {
                if (i == null) break;
                iterator.Depth = 0;
                iterator.History = null;
                List<int> xi = new List<int> { i.Value };
                Recursion(xi, nextDepth, maxDepth, ref u, iterator);
            }

            return u;
        }

        internal void Recursion(List<int> xibefore, int depth, int maxDepth, ref UniqueArrangements<int> u, CustomIterenumerator<int> iterator)
        {
            int index = depth;
            int nextDepth = depth + 1;

            List<int> xiafter = new List<int>();
            CustomIterenumerator<int> localIter = iterator.Clone();
            localIter.Depth = depth;
            localIter.History = xibefore.Select(item => item).ToList();
            for (int? i = localIter.MoveNext(); localIter.IsValid(); i = localIter.MoveNext())
            {
                if (i == null) break;
                if (iterator.HasCoefficientGenerator())
                {
                    iterator.GenerateCoefficients(i.Value);
                } else {
                    xiafter = xibefore.Select(item => item).ToList();
                    xiafter.Add(i.Value);
                }
                if (index == maxDepth - 1)
                {
                    CryptCoefficients cc = new CryptCoefficients(xiafter);
                    if (_cryptarithm.JustEvaluateRules(cc))
                    {
                        u.Add(xiafter);
                        break;
                    }
                    else { continue; }
                } else {
                    Recursion(xiafter, nextDepth, maxDepth, ref u, iterator);
                }
            }
        }
    }

    public class RandomGeneratorSimulator
    {
        private PoolRandomArrayGenerator<int> _pool;
        internal Cryptarithm _cryptarithm;
        public RandomGeneratorSimulator(Func<List<int>> initializer, Cryptarithm c)
        {
            _pool = new PoolRandomArrayGenerator<int>(initializer);
            _cryptarithm = c;
        }

        public void Simulate(int NSimulations)
        {
            int n = 0;
            while (++n <= NSimulations)
            {
                CryptCoefficients cc = new CryptCoefficients(_pool.GetNewArray());
                _cryptarithm.JustEvaluateRules(cc);
            }
        }
    }

    public class SpecialCryptRecursiveSolver : CryptRecursiveSolver
    {
        public SpecialCryptRecursiveSolver(bool isdistinct, Cryptarithm c) : base(isdistinct, c) {
        }

        /// <summary>
        /// To iterate over variables in reverse order.
        /// </summary>
        public bool Isdecremental { get; set; }
        /// <summary>
        /// If a,b,c is a solution, then so are b,c,a and c,b,a.
        /// Add all those at once.
        /// </summary>
        public bool IsSymmetric { get; set; }
        private int _startIndex = 0;
        private int _endIndex = 0;

        private int GetIndexFromDepth(int depth, int maxDepth)
        {
            if (depth >= 0) return depth;
            else {
                return (maxDepth + depth % maxDepth) % maxDepth;
            }
        }

        /// <summary>
        /// Use this only if distinct is false or if more than one solution is possible.
        /// </summary>
        /// <param name="variables">Number of variables</param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public UniqueArrangements<int> GetAllSolutions(int variables, List<Iterator<int>> iterators,
            int startIndex, bool isDecremental,
            Func<List<int>, bool> searchAccelerator = null)
        {
            Isdecremental = isDecremental;
            _startIndex = startIndex;
            if (Isdecremental) _endIndex = _startIndex + 1;
            else _endIndex = GetIndexFromDepth(_startIndex - 1, variables);
            UniqueArrangements<int> ua = new UniqueArrangements<int>();
            int index = GetIndexFromDepth(startIndex, variables);
            int nextDepth = Isdecremental ? startIndex - 1 : startIndex + 1;

            Func<int, bool> eval = delegate (int iter)
            {
                if (iterators[index].IsIncrement) return iter <= iterators[index].Range.Max;
                else return iter >= iterators[index].Range.Min;
            };
            for (int i = iterators[index].IsIncrement ? iterators[index].Range.Min : iterators[index].Range.Max;
                eval.Invoke(i);
                i += iterators[index].IsIncrement ? 1 : -1)
            {
                int next = Isdecremental ? -1 : 1;
                RecursiveFullSearch(new List<int>() { i }, next, variables, ref ua, iterators, searchAccelerator);
            }
            return ua;
        }

        internal new void RecursiveFullSearch(List<int> used, int depth, int maxDepth, ref UniqueArrangements<int> ua,
            List<Iterator<int>> iterators, Func<List<int>, bool> searchAccelerator = null)
        {
            List<int> usedCopy;
            int index = GetIndexFromDepth(depth, maxDepth);
            int nextDepth = Isdecremental ? depth - 1 : depth + 1;

            Func<int, bool> eval = delegate (int iter)
            {
                if (iterators[index].IsIncrement) return iter <= iterators[index].Range.Max;
                else return iter >= iterators[index].Range.Min;
            };
            for (int i = iterators[index].IsIncrement ? iterators[index].Range.Min : iterators[index].Range.Max;
                eval.Invoke(i); i += iterators[index].IsIncrement ? 1 : -1) {
                if (IsDistinct) if (used.Contains(i)) continue;

                usedCopy = used.Select(item => item).ToList();
                usedCopy.Add(i);

                if (searchAccelerator != null)
                {
                    if (!searchAccelerator.Invoke(usedCopy)) continue;
                }

                if (index == _endIndex)
                {
                    int[] arr = usedCopy.ToArray();
                    CryptCoefficients cc = new CryptCoefficients(arr);
                    if (_cryptarithm.JustEvaluateRules(cc))
                    {
                        ua.Add(usedCopy);
                        break;
                    }
                    else { continue; }
                }
                else {
                    RecursiveFullSearch(usedCopy, nextDepth, maxDepth, ref ua, iterators, searchAccelerator);
                }
            }
        }

        /// <summary>
        /// Use this only if distinct is false or if more than one solution is possible.
        /// </summary>
        /// <param name="variables">Number of variables</param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public BigInteger CountSolutions(int variables, List<Iterator<int>> iterators,
            int startIndex, bool isDecremental,
            Func<List<int>, bool> searchAccelerator = null)
        {
            Isdecremental = isDecremental;
            _startIndex = startIndex;
            if (Isdecremental) _endIndex = _startIndex + 1;
            else _endIndex = GetIndexFromDepth(_startIndex - 1, variables);
            SimpleCounter ua = new SimpleCounter();
            int index = GetIndexFromDepth(startIndex, variables);
            int nextDepth = Isdecremental ? startIndex - 1 : startIndex + 1;

            Func<int, bool> eval = delegate (int iter)
            {
                if (iterators[index].IsIncrement) return iter <= iterators[index].Range.Max;
                else return iter >= iterators[index].Range.Min;
            };
            for (int i = iterators[index].IsIncrement ? iterators[index].Range.Min : iterators[index].Range.Max;
                eval.Invoke(i);
                i += iterators[index].IsIncrement ? 1 : -1)
            {
                int next = Isdecremental ? -1 : 1;
                RecursiveFullSearchCounter(new List<int>() { i }, next, variables, ref ua, iterators, searchAccelerator);
            }
            return ua.GetCount();
        }

        internal void RecursiveFullSearchCounter(List<int> used, int depth, int maxDepth, ref SimpleCounter ua,
            List<Iterator<int>> iterators, Func<List<int>, bool> searchAccelerator = null)
        {
            List<int> usedCopy;
            int index = GetIndexFromDepth(depth, maxDepth);
            int nextDepth = Isdecremental ? depth - 1 : depth + 1;

            Func <int, bool> eval = delegate (int iter)
            {
                if (iterators[index].IsIncrement) return iter <= iterators[index].Range.Max;
                else return iter >= iterators[index].Range.Min;
            };
            for (int i = iterators[index].IsIncrement ? iterators[index].Range.Min : iterators[index].Range.Max;
                eval.Invoke(i);
                i += iterators[index].IsIncrement ? 1 : -1)
            {
                if (IsDistinct) if (used.Contains(i)) continue;

                usedCopy = used.Select(item => item).ToList();
                usedCopy.Add(i);

                if (searchAccelerator != null)
                {
                    if (!searchAccelerator.Invoke(usedCopy)) continue;
                }

                if (index == _endIndex)
                {
                    CryptCoefficients cc = new CryptCoefficients(usedCopy);
                    if (_cryptarithm.JustEvaluateRules(cc))
                    {
                        ua.Increment();
                        break;
                    }
                    else { continue; }
                }
                else {
                    RecursiveFullSearchCounter(usedCopy, nextDepth, maxDepth, ref ua, iterators, searchAccelerator);
                }
            }
        }

    }

    public class CryptRule
    {
        private readonly Func<CryptRule, bool> rule;
        CryptCoefficients c;
        public CryptRule(Func<CryptRule, bool> cRule) { rule = cRule; }

        internal void SetCoefficients(CryptCoefficients coeff)
        {
            c = coeff;
        }

        public int[] GetCoefficients()
        {
            return c.Coefficients;
        }

        public int CoefficientSum()
        {
            return c.Coefficients.Sum();
        }

        public int CoefficientSum(int[] indices)
        {
            int sum = 0;
            foreach (int t in indices) sum += c.Coefficients[t];
            return sum;
        }

        internal bool Evaluate()
        {
            return rule.Invoke(this);
        }

        /// <summary>
        /// Input, Comma separated indices.
        /// 0,1,2,3,7. Will form a number like coeff[0]coeff[1]coeff[2]coeff[3]coeff[7]
        /// </summary>
        /// <returns></returns>
        public int ExtractValue(string indices)
        {
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            string e = string.Empty;
            foreach (string s in indexArr)
            {
                e += c.Coefficients[int.Parse(s)];
            }
            return int.Parse(e);
        }

        public long ExtractProduct(string indices)
        {
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            long product = 1;
            foreach (string s in indexArr)
            {
                product *= c.Coefficients[int.Parse(s)];
            }
            return product;
        }

        /// <summary>
        /// Input, Comma separated indices.
        /// 0,1,2,3,7. Will form a number like coeff[0] + coeff[1] + coeff[2] + coeff[3] + coeff[7]
        /// </summary>
        /// <returns></returns>
        public int ExtractValueSum(string indices)
        {
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            int e = 0;
            foreach (string s in indexArr)
            {
                e += c.Coefficients[int.Parse(s)];
            }
            return e;
        }

        /// <summary>
        /// Input, Comma separated indices.
        /// 0,1,2,3,7. Will form a number like coeff[0]coeff[1]coeff[2]coeff[3]coeff[7]
        /// </summary>
        /// <returns></returns>
        public string ExtractValueAsString(string indices)
        {
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            string e = string.Empty;
            foreach (string s in indexArr)
            {
                e += c.Coefficients[int.Parse(s)];
            }
            return e;
        }
    }

    public class CryptSolutions
    {
        List<CryptCoefficients> Sets;

        public CryptSolutions(List<List<int>> sets)
        {
            Sets = new List<CryptCoefficients>();
            sets.ForEach(s =>
            {
                Sets.Add(new CryptCoefficients(s));
            });
        }

        public int NumberOfSolutions() { return Sets.Count; }
        public List<CryptCoefficients> GetSolutions() { return Sets; }
    }

    public class CryptCoefficients
    {
        public int[] Coefficients;

        public CryptCoefficients(int[] coeff)
        {
            Coefficients = coeff;
        }

        public CryptCoefficients(List<int> coeff)
        {
            Coefficients = coeff.ToArray();
        }

        /// <summary>
        /// Input, Comma separated indices.
        /// 0,1,2,3,7. Will form a number like coeff[0]coeff[1]coeff[2]coeff[3]coeff[7]
        /// </summary>
        /// <returns></returns>
        public int ExtractValue(string indices)
        {
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            string e = string.Empty;
            foreach (string s in indexArr)
            {
                e += Coefficients[int.Parse(s)];
            }
            return int.Parse(e);
        }

        /// <summary>
        /// Input, Comma separated indices.
        /// 0,1,2,3,7. Will return sum of coeff[0] + coeff[1] + coeff[2] + coeff[3] + coeff[7]
        /// If null it will return sum of all coefficients.
        /// </summary>
        /// <returns></returns>
        public int ExtractValueSum(string indices = null)
        {
            if (indices == null)
            {
                return Coefficients.Sum();
            }

            int e = 0;
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string s in indexArr)
            {
                e += Coefficients[int.Parse(s)];
            }
            return e;
        }

        /// <summary>
        /// Input, Comma separated indices.
        /// 0,1,2,3,7. Will form a number like coeff[0]coeff[1]coeff[2]coeff[3]coeff[7]
        /// </summary>
        /// <returns></returns>
        public string ExtractValueAsString(string indices)
        {
            string[] indexArr = indices.Splitter(StringSplitter.SplitUsing.Comma, StringSplitOptions.RemoveEmptyEntries);
            string e = string.Empty;
            foreach (string s in indexArr)
            {
                e += Coefficients[int.Parse(s)];
            }
            return e;
        }
    }
}