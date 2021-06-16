using GenericDefs.Classes;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using GenericDefs.Classes.NumberTypes.HighPrecisionConverter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brilliant.Logic
{
    /// <summary>
    /// Use this class to solve problems of type. https://brilliant.org/wiki/logic-puzzles-intermediate/
    /// </summary>
    public class Cryptarithms : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/wiki/logic-puzzles-intermediate/");
        }

        void ISolve.Solve()
        {
            Solve7();
        }

        /// <summary>
        /// https://brilliant.org/problems/this-is-very-hard/
        /// </summary>
        void ThisIsVeryHard()
        {
            //thisveryad
            //0123456789
            int max = 0;
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int thisvery = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("4,5,6,7");
                int hard = cr.ExtractValue("1,8,6,9");
                if (thisvery == hard)
                {
                    if (hard > max) max = hard;
                    return true;
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = (i == 1 || i == 0 || i == 4) ? 1 : 0 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators);
            QueuedConsole.WriteImmediate("{0}", max);
        }

        /// <summary>
        /// https://brilliant.org/problems/this-is-easy-5/
        /// </summary>
        void ThisisEasy()
        {
            //thiseay
            //0123456
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int thisis = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("2,3");
                int easy = cr.ExtractValue("4,5,3,6");
                if (thisis == easy)
                {
                    return true;
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 7; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = (i == 2 || i == 0 || i == 4) ? 1 : 0 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(7, iterators);
            QueuedConsole.WriteImmediate("{0}", ua.GetCount());
        }

        /// <summary>
        /// https://brilliant.org/problems/1-2-3-4-5-6-2/
        /// </summary>
        void _1234562()
        {
            //abcdef
            //012345
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int rhs = (cr.ExtractValue("3,4,5"));
                if ((cr.ExtractValue("0,1") * cr.ExtractValue("2")) == rhs)
                {
                    QueuedConsole.WriteImmediate("rhs : {0}", rhs);
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 6; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 6, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(6, iterators);
        }

        void PeepPeep()
        {
            //epqrs
            //01234
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int q = cr.ExtractValue("2"), s = cr.ExtractValue("4"), r = cr.ExtractValue("3");
                if (q < s && s < r)
                {
                    int peep = cr.ExtractValue("1,0,0,1");
                    int rhs = 10000 * cr.ExtractValue("1,2,3") + cr.ExtractValue("4,2,1");
                    if (peep*peep == rhs)
                    {
                        QueuedConsole.WriteImmediate("peeps : {0}", cr.ExtractValue("1,0,0,1,4"));
                        return true;
                    }
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 5; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = i == 2 ? 1 : 0 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(5, iterators);
        }

        void AGramFromMars()
        {
            //marsg
            //01234
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int rhs = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("0,1,2") + cr.ExtractValue("0,1") + cr.ExtractValue("0");
                int gram = cr.ExtractValue("4,2,1,0");
                if (rhs == gram)
                {
                    QueuedConsole.WriteImmediate("A : {0}", cr.ExtractValue("1"));
                    return true;
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 5; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(5, iterators);
        }

        /// <summary>
        /// https://brilliant.org/problems/set-of-moving-last-digits/
        /// </summary>
        void SetOfMovingDigits()
        {
            //abcdel
            //012345
            Dictionary<int, List<int>> Numbers = new Dictionary<int, List<int>>();
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int abcdel = cr.ExtractValue("0,1,2,3,4,5");
                int labcde = cr.ExtractValue("5,0,1,2,3,4");

                if (labcde % abcdel == 0)
                {
                    int m = labcde / abcdel;
                    if (m > 1)
                    {
                        if (Numbers.ContainsKey(m)) { Numbers[m].Add(abcdel); }
                        else Numbers.Add(m, new List<int>() { abcdel });

                        QueuedConsole.WriteImmediate("abcdel : {0}, m : {1}", abcdel, m);
                        return true;
                    }
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 6; i++)
            {
                int min = (i == 0 || i == 5 ? 1 : 0);
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(6, iterators);
            Numbers.ForEach(N =>
            {
                QueuedConsole.WriteImmediate("{0} : {1}", N.Value.Count == 1 ? "O" : "S", N.Value.Sum());
            });
        }

        /// <summary>
        /// https://brilliant.org/problems/what-is-the-number-2/
        /// </summary>
        void Whatisthenumber()
        {
            //abcd
            //0123
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int abcd = cr.ExtractValue("0,1,2,3");
                int dcba = cr.ExtractValue("3,2,1,0");
                int _5ccbaad = int.Parse("5" + cr.ExtractValueAsString("2,2,1,0,0,3"));
                if (abcd*dcba == _5ccbaad)
                {
                    return true;
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                int min = (i == 0 || i == 3 ? 1 : 0);
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(4, iterators);

            CryptSolutions c = new CryptSolutions(ua.ExtractAsSets());
            foreach (CryptCoefficients cc in c.GetSolutions())
            {
                int abcd = cc.ExtractValue("0,1,2,3");
                int dcba = cc.ExtractValue("3,2,1,0");
                QueuedConsole.WriteImmediate("dcba - abcd : {0}", dcba - abcd);
            }
        }

        void SolveCryptogramSum()
        {
            int N = int.MaxValue;
            //abcdef
            //012345
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int ab = cr.ExtractValue("0,1");
                int cd = cr.ExtractValue("2,3");
                int ef = cr.ExtractValue("4,5");
                if (ab + cd == ef)
                {
                    N = Math.Min(N, ef);
                    return true;
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 6; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            solver.GetAllSolutions(6, iterators);
            QueuedConsole.WriteImmediate("N : {0}", N);
        }

        /// <summary>
        /// https://brilliant.org/problems/a-quadruple-date/
        /// </summary>
        void Quad()
        {
            int N = 8;
            //abcdefgh
            //01234567
            Dictionary<int, int> MPairs = new Dictionary<int, int>() { { 1, 5 }, { 2, 6 }, { 3, 7 }, { 4, 8 } };
            Dictionary<int, int> WPairs = new Dictionary<int, int>() { { 5, 1 }, { 6, 2 }, { 7, 3 }, { 8, 4 } };
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] c = cr.GetCoefficients();
                for (int a = 0; a < N - 1; a++)
                {
                    int next = c[a + 1];
                    if (c[a] <= 4) {
                        if (!(next <= 4 || next == MPairs[c[a]])) return false;
                    } else {
                        if (!(next >= 5 || next == WPairs[c[a]])) return false;
                    }
                }
                return true;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < N; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = N, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(N, iterators);
            QueuedConsole.WriteImmediate("{0}", ua.GetCount());
        }

        /// <summary>
        /// https://brilliant.org/problems/impossible-cryptarithm/
        /// </summary>
        void ImpossibleCryptarithm()
        {
            //abcdefghi
            //012345678
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,1,2,3") * cr.ExtractValue("4");
                int rhs = cr.ExtractValue("5,6,7,8");
                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 9; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(9, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                QueuedConsole.WriteImmediate(string.Format("{0}", cc.ExtractValueSum("0,4,7,8")));
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/the-digits-abcabd/
        /// </summary>
        void TheDigitsAbcabd()
        {
            //abcdefghij
            //0123456789

            List<long> primes = KnownPrimes.GetPrimes(1, 100);

            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int abcabd = 10 * cr.ExtractValue("0,1,2,0,1") + (cr.ExtractValue("2") + 1);

                return MathFunctions.IsPerfectSquare(abcabd);
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 3; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = i == 2 ? 8 : 9, Min = i == 0 ? 1 : 0 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(3, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                QueuedConsole.WriteImmediate(string.Format("{0}", 10 * cc.ExtractValue("0,1,2,0,1") + (cc.ExtractValue("2") + 1)));
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/can-you-fill-in-these-boxes-part-ii/
        /// </summary>
        void CanYouFillInTheBoxes2()
        {
            //abcdefghij
            //0123456789

            List<long> primes = KnownPrimes.GetPrimes(1, 100);

            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int abcd = cr.ExtractValue("0,1,2,3");
                int ef = cr.ExtractValue("4,5");
                if (!primes.Contains(ef)) return false;
                if (abcd % ef != 0) return false;
                if (cr.ExtractValueSum("0,1,2,3") != cr.ExtractValueSum("4,5")) return false;

                int gh = cr.ExtractValue("6,7");
                int ij = cr.ExtractValue("8,9");

                int rhs = gh - ij;
                int lhs = abcd / ef;

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                int min = 0;
                if (i == 0 || i == 4 || i == 6 || i == 8) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                QueuedConsole.WriteImmediate(string.Format("{0}", cc.ExtractValue("6,7") - cc.ExtractValue("8,9")));
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/dirty-cryptarithm/
        /// </summary>
        void DirtyCryptarithm()
        {
            //abcde
            //01234
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int abcde = 12 * cr.ExtractValue("0,1,2,3,4");
                int rhs = 1000 * cr.ExtractValue("2,3,4") + cr.ExtractValue("0,1");

                return abcde == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 5; i++)
            {
                int min = 0;
                if (i == 0 || i == 2) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(5, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);

                QueuedConsole.WriteImmediate(string.Format("{0}", cc.ExtractValue("0,1,2,3,4")));
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/mistakes-give-rise-to-problems-9/
        /// </summary>
        void MistakesGiveRiseToProblems()
        {
            //c-d-i-l-v-x
            //0-1-2-3-4-5
            int count = 0;
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int xliv = cr.ExtractValue("5,3,2,4");
                int cdxl = cr.ExtractValue("0,1,5,3");
                int x = cr.ExtractValue("5");
                if (xliv * x == cdxl) { count++; }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 6; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 1 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            solver.GetAllSolutions(6, iterators);
            QueuedConsole.WriteImmediate(string.Format("Number of ordered six-tuplets : {0}", count));
        }

        /// <summary>
        /// https://brilliant.org/problems/seven-eight-nine/
        /// </summary>
        void Solve1()
        {
            Func<CryptRule, bool> rule1 = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                //eightnsv

                // t + e = 10
                // s = e + 1
                // h + n = 9
                if (coeff[4] + coeff[0] == 10)
                {
                    if (coeff[3] + coeff[5] == 9)
                    {
                        if (coeff[6] == coeff[0] + 1)
                        {
                            return true;
                        }
                        else return false;
                    }
                    else { return false; }
                }
                else return false;
            };

            Func<CryptRule, bool> rule2 = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                //eightnsv

                // eight + nine + ten
                long lhs = 10000 * coeff[0] + 1000 * coeff[1] + 100 * coeff[2] + 10 * coeff[3] + coeff[4]
                + 1000 * coeff[5] + 100 * coeff[1] + 10 * coeff[5] + coeff[0]
                + 100 * coeff[4] + 10 * coeff[0] + coeff[5];

                //seven
                long rhs = 10000 * coeff[6] + 1000 * coeff[0] + 100 * coeff[7] + 10 * coeff[0] + coeff[5];

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[2];
            CryptRule r1 = new CryptRule(rule1);
            CryptRule r2 = new CryptRule(rule2);
            rules[0] = r1;
            rules[1] = r2;

            Cryptarithm c = new Cryptarithm(rules);

            bool isFound = false;
            int[] cArr = new int[8];

            for (long l = 10000000; l < 100000000; l++)
            {
                if (!GenericDefs.Functions.Numbers.HasDuplicateDigits(l))
                {
                    CryptCoefficients cc = new CryptCoefficients(GenericDefs.Functions.Numbers.GetDigitArray(l));
                    isFound = c.JustEvaluateRules(cc);
                    if (isFound)
                    {
                        cArr = cc.Coefficients;
                        break;
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine("No solution.");
            }
            else Console.WriteLine("Sum is :: {0} ", cArr.Sum(x => x));
            Console.ReadKey();
        }

        /// <summary>
        /// Permutating digits of cryptogram.
        /// abc + cab = bca. Are there any integer solutions.
        /// </summary>
        void Solve2()
        {
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                int lhs = 100 * coeff[0] + 10 * coeff[1] + coeff[2]
                    + 100 * coeff[2] + 100 * coeff[0] + coeff[1];

                int rhs = 100 * coeff[1] + 10 * coeff[2] + coeff[0];

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };

            Cryptarithm cArithm = new Cryptarithm(rules);

            bool isFound = false;
            int[] cArr = new int[3];

            for (int a = 1; a < 10; a++)
            {
                for (int b = 1; b < 10; b++)
                {
                    for (int c = 1; c < 10; c++)
                    {
                        CryptCoefficients cc = new CryptCoefficients(new int[] { a, b, c });
                        isFound = cArithm.JustEvaluateRules(cc);
                        if (isFound)
                        {
                            cArr = cc.Coefficients;
                            break;
                        }
                    }
                }

            }

            if (!isFound)
            {
                Console.WriteLine("No solution.");
            }
            else Console.WriteLine("Sum is :: {0} ", cArr.Sum(x => x));
            Console.ReadKey();
        }

        /// <summary>
        /// pqrs * 4 = srqp
        /// find (s*r)^(q*p)
        /// </summary>
        void Solve3()
        {
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                int rhs = Convert.ToInt32(string.Join("", coeff.Reverse().Select(x => x.ToString()).ToArray()));
                int lhs = 4 * (Convert.ToInt32(string.Join("", coeff.Select(x => x.ToString()).ToArray())));

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };

            Cryptarithm cArithm = new Cryptarithm(rules);

            bool isFound = false;
            int[] cArr = new int[4];

            int trials = 0;
            for (int p = 1; p < 10; p++)
            {
                for (int q = 0; q < 10; q++)
                {
                    for (int r = 0; r < 10; r++)
                    {
                        for (int s = 0; s < 10; s++)
                        {
                            trials++;
                            int[] arr = new int[] { p, q, r, s };
                            if (!GenericDefs.Functions.Numbers.HasDuplicateDigits(Convert.ToInt64(string.Join("", arr.Select(x => x.ToString()).ToArray()))))
                            {
                                CryptCoefficients cc = new CryptCoefficients(arr);
                                isFound = cArithm.JustEvaluateRules(cc);
                                if (isFound)
                                {
                                    cArr = cc.Coefficients;
                                    break;
                                }
                            }
                        }
                        if (isFound) break;
                    }
                    if (isFound) break;
                }
                if (isFound) break;
            }
            Console.WriteLine("Trials count : {0}", trials);
            if (!isFound)
            {
                QueuedConsole.WriteFinalAnswer("No solution.");
            }
            else
            {
                int tsrqp = Convert.ToInt32(string.Join("", cArr.Reverse().Select(x => x.ToString()).ToArray()));
                int pqrst = Convert.ToInt32(string.Join("", cArr.Select(x => x.ToString()).ToArray()));
                int sr = cArr[3] * cArr[2];
                int pq = cArr[0] * cArr[1];
                System.Numerics.BigInteger b = new System.Numerics.BigInteger(1);
                int i = pq;
                while (true)
                {
                    b *= sr;
                    i--;
                    if (i == 0) break;
                }
                QueuedConsole.WriteFinalAnswer(string.Format("Value of (s*r)^(p*q) :: {0}", b.ToString()));
            }
        }

        /// <summary>
        /// xx * xy = yxxy
        /// </summary>
        void Solve4()
        {
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                int lhs = (10 * coeff[0] + coeff[1]) * (10 * coeff[0] + coeff[0]);

                int rhs = 1000 * coeff[1] + 100 * coeff[0] + 10 * coeff[0] + coeff[1];

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };

            Cryptarithm cArithm = new Cryptarithm(rules);

            bool isFound = false;
            int[] cArr = new int[5];

            int trials = 0;
            for (int p = 1; p < 10; p++)
            {
                for (int q = 1; q < 10; q++)
                {
                    trials++;
                    int[] arr = new int[] { p, q };
                    CryptCoefficients cc = new CryptCoefficients(arr);
                    isFound = cArithm.JustEvaluateRules(cc);
                    if (isFound)
                    {
                        cArr = cc.Coefficients;
                        break;
                    }
                }
                if (isFound) break;
            }
            Console.WriteLine("Trials count : {0}", trials);
            if (!isFound)
            {
                QueuedConsole.WriteFinalAnswer("No solution.");
            }
            else QueuedConsole.WriteFinalAnswer(string.Format("Sum is :: {0} ", Math.Pow(cArr[0], cArr[1])));
        }

        /// <summary>
        /// pqrs = 9*srqp;
        /// </summary>
        void Solve5()
        {
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                int lhs = Convert.ToInt32(string.Join("", coeff.Select(x => x.ToString()).ToArray()));

                int rhs = 9 * (Convert.ToInt32(string.Join("", coeff.Reverse().Select(x => x.ToString()).ToArray())));

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };

            Cryptarithm cArithm = new Cryptarithm(rules);

            bool isFound = false;
            int[] cArr = new int[4];

            int trials = 0;
            for (int p = 1; p < 10; p++)
            {
                for (int q = 0; q < 10; q++)
                {
                    for (int r = 0; r < 10; r++)
                    {
                        for (int s = 0; s < 10; s++)
                        {
                            trials++;
                            int[] arr = new int[] { p, q, r, s };
                            if (!GenericDefs.Functions.Numbers.HasDuplicateDigits(Convert.ToInt64(string.Join("", arr.Select(x => x.ToString()).ToArray()))))
                            {
                                CryptCoefficients cc = new CryptCoefficients(arr);
                                isFound = cArithm.JustEvaluateRules(cc);
                                if (isFound)
                                {
                                    cArr = cc.Coefficients;
                                    break;
                                }
                            }
                            if (isFound) break;
                        }
                        if (isFound) break;
                    }
                    if (isFound) break;
                }
                if (isFound) break;
            }
            Console.WriteLine("Trials count : {0}", trials);
            if (!isFound)
            {
                QueuedConsole.WriteFinalAnswer("No solution.");
            }
            else
            {
                int pqrs = Convert.ToInt32(string.Join("", cArr.Select(x => x.ToString()).ToArray()));
                QueuedConsole.WriteFinalAnswer(string.Format("Value of pqrs : {0} ", pqrs));
            }
        }

        /// <summary>
        /// https://brilliant.org/practice/logic-warmups-level-4-challenges/?p=2
        /// seven + seven + six = twenty
        /// </summary>
        void Solve6()
        {
            //sevnixtwy
            //012345678
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();
                int lhs = cr.ExtractValue("0,1,2,1,3") + cr.ExtractValue("0,1,2,1,3") + cr.ExtractValue("0,4,5");
                int rhs = cr.ExtractValue("6,7,1,3,6,8");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            BlackList<int> bl = new BlackList<int>();
            for (int i = 0; i < 9; i++)
            {
                if (i == 0 || i == 6) bl.Add(i, new int[] { 0 });
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            int[] answer = solver.Solve(9, bl);
            QueuedConsole.WriteFinalAnswer(string.Format("Value of W : {0}", answer[7]));
        }

        /// <summary>
        /// https://brilliant.org/problems/single-cryptogram-quadruple-solutions/
        /// char + ray + saura = ember
        /// </summary>
        void Solve7()
        {
            //c-h-a-r-y-s-u-e-m-b
            //0-1-2-3-4-5-6-7-8-9
            LoggerContext lc = new LoggerContext();
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();
                int lhs = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("3,2,4") + cr.ExtractValue("5,2,6,3,2");
                int rhs = cr.ExtractValue("7,8,9,7,3");

                if (lhs == rhs)
                {
                    string s = "CHAR : " + cr.ExtractValue("0,1,2,3") + ", RAY : " + cr.ExtractValue("3,2,4") + ", SAURA : "
                        + cr.ExtractValue("5,2,6,3,2") + ", EMBER : " + cr.ExtractValue("7,8,9,7,3") + ", LHS : " + lhs + ", RHS : " + rhs;
                    lc.AppendCurrentLog(s);
                    lc.AppendNewLine();

                    QueuedConsole.WriteImmediate(s);
                }
                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                int min = 0;
                if (i == 0 || i == 3 || i == 5 || i == 7) { min = 1; }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators, null);
            List<List<int>> sets = ua.ExtractAsSets();
            int charSum = 0;
            UniqueArrangements<int> exractedCHAR = new UniqueArrangements<int>();
            foreach (List<int> set in sets)
            {
                exractedCHAR.Add(new List<int>() { set[0], set[1], set[2], set[3] });
                //charSum += Convert.ToInt32(set[0] + "" + set[1] + "" + set[2] + "" + set[3]);
            }

            QueuedConsole.WriteImmediate(string.Format("Possible solutions : {0}", exractedCHAR.GetCount()));
            List<List<int>> solutions = exractedCHAR.ExtractAsSets();
            foreach (List<int> set in solutions)
            {
                charSum += Convert.ToInt32(set[0] + "" + set[1] + "" + set[2] + "" + set[3]);
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Sum of possible values of CHAR : {0}", charSum));
            Logger.Log(lc);
        }

        /// <summary>
        /// https://brilliant.org/problems/a-dango-cryptogram-clannad-fanfiction/
        /// </summary>
        void SolveDangoCryptogramClannad()
        {
            List<int> primes = Prime.GeneratePrimesNaiveNMax(10000);
            List<int> dango = new List<int>();
            foreach (int n in primes)
            {
                if (n > 2000)
                {
                    int n5 = 5 * n;
                    string n5str = "" + n5;
                    int ch1 = (int)char.GetNumericValue(n5str[0]);
                    int ch2 = (int)char.GetNumericValue(n5str[1]);
                    int diff = ch2 - ch1;
                    if (diff > 0 && diff <= 2 && ch1 + 3 * diff < 10)
                    {
                        dango.Add(n);
                    }
                }
            }

            List<int>.Enumerator e = dango.GetEnumerator();
            HashSet<int> oSet = new HashSet<int>();
            //d-a-n-g-o-k1-z-o-k2-d-a-i-k3-a-z-k4-u
            //----------0.-1---2.------------1-3.-4
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int dangoVal = e.Current * 5;
                string dangoStr = dangoVal.ToString();
                int d = (int)char.GetNumericValue(dangoStr[0]);
                int a = (int)char.GetNumericValue(dangoStr[1]);
                int o = (int)char.GetNumericValue(dangoStr[4]);
                int i = a + (a - d);
                int k = i + (a - d);

                int[] coeff = cr.GetCoefficients();
                int lhs = dangoVal * (100 * cr.ExtractValue("0,1") + 10 * o + cr.ExtractValue("2"));
                int rhs = 1000 * int.Parse(d + "" + a + "" + i + "" + k + "" + a + "" + cr.ExtractValue("1")) + cr.ExtractValue("3,4");
                int diff = lhs - rhs;
                if (diff > 0 && diff % 100 == 0 && diff / 100 < 10)
                {
                    oSet.Add((diff) / 100);
                    return true;
                }
                else return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 5; i++)
            {
                int min = 0;
                if (i == 0) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
            UniqueArrangements<int> ua = new UniqueArrangements<int>();
            while (e.MoveNext())
            {
                ua.Add(solver.GetAllSolutions(5, iterators, null));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", oSet.Count));
        }

        /// <summary>
        /// https://brilliant.org/problems/inspired-by-myself-2/
        /// </summary>
        void DoubleCryptogram()
        {
            //a-b-c-d
            //0-1-2-3
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();
                int lhs = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("1,2,3,0");
                int rhs = cr.ExtractValue("3,2,1,0") + cr.ExtractValue("2,1,0,3");

                return lhs == rhs;
            };

            Func<List<int>, bool> SearchAccelerator = delegate (List<int> list)
            {
                if (list.Count == 4)
                {
                    return (list[0] + list[1]) == (list[2] + list[3]);
                }
                else if (list.Count == 3) { return (list[0] + list[1]) > list[2]; }
                else return true;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                int min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(4, iterators, null);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                QueuedConsole.WriteImmediate(string.Format("Set : {0}", string.Join("-", set)));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible quadruples : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/mistakes-give-rise-to-problems-20/
        /// </summary>
        void Solve20()
        {
            //a-b-c-d
            //0-1-2-3
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();
                int lhs = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("1,2,3,0");
                int rhs = cr.ExtractValue("3,2,1,0") + cr.ExtractValue("2,1,0,3");

                return lhs == rhs;
            };

            Func<List<int>, bool> SearchAccelerator = delegate (List<int> list)
            {
                if (list.Count == 4)
                {
                    return (list[0] + list[1]) == (list[2] + list[3]);
                }
                else if (list.Count == 3) { return (list[0] + list[1]) > list[2]; }
                else return true;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                int min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(4, iterators, null);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                QueuedConsole.WriteImmediate(string.Format("Set : {0}", string.Join("-", set)));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible quadruples : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/mistakes-give-rise-to-problems-20/
        /// </summary>
        void DoubleRainbow()
        {
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int[] coeff = cr.GetCoefficients();

                int[] arr = new int[14] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                int depth = -1;
                bool retVal = true;
                foreach (int p in coeff)
                {
                    depth++;
                    if (p + depth > 11 || arr[p] == 0 || arr[p + depth + 2] == 0)
                    {
                        retVal = false;
                        break;
                    }
                    arr[p] = 0;
                    arr[p + depth + 2] = 0;
                }

                return retVal;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            Func<object, int, int?> nextAvailable = delegate (object history, int curIndex)
            {
                List<int> hi = (List<int>)history;
                int[] arr = new int[14] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                int depth = -1;
                foreach (int p in hi)
                {
                    depth++;
                    arr[p] = 0;
                    arr[p + depth + 2] = 0;
                }
                depth++;
                int? retVal = null;
                for (int i = 0; i + depth <= 11; i++)
                {
                    if (curIndex < i && arr[i] == 1 && arr[i + depth + 2] == 1)
                    {
                        retVal = i;
                        break;
                    }
                }
                return retVal;
            };

            Func<CustomIterenumerator<int>, int?> customEnum = delegate (CustomIterenumerator<int> ci)
            {
                int depth = ci.Depth;
                int index = ci.CurrentIndex;
                int prev = ci.History == null ? 0 : ((List<int>)ci.History)[depth - 1];
                if (depth == 0) { return 0; }
                else
                {
                    return nextAvailable(ci.History, index);
                }
            };

            Func<CustomIterenumerator<int>, bool> validator = delegate (CustomIterenumerator<int> ci)
            {
                if (ci.CurrentIndex >= 0 && ci.CurrentIndex <= 2) return true;
                else return false;
            };

            CustomIterenumerator<int> iterator = new CustomIterenumerator<int>(customEnum, null);

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(7, iterator);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                int[] arr = new int[14] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                int depth = -1;
                foreach (int p in set)
                {
                    depth++;
                    arr[p] = depth + 1;
                    arr[p + depth + 2] = depth + 1;
                }
                QueuedConsole.WriteImmediate(string.Format("Set : {0}", string.Join("-", arr)));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/twisted-addition-cryptogram/
        /// ten + ten + nine + eight + three = forty
        /// </summary>
        void TwistedAdditionCryptogram()
        {
            //tenighrfoy
            //0123456789
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = 2 * cr.ExtractValue("0,1,2") + cr.ExtractValue("2,3,2,1") + cr.ExtractValue("1,3,4,5,0") + cr.ExtractValue("0,5,6,1,1");
                int rhs = cr.ExtractValue("7,8,6,0,9");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = 0 });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators);
            QueuedConsole.WriteFinalAnswer(string.Format("Solution count : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/twisted-addition-cryptogram/
        /// fire + in + the = hole
        /// </summary>
        void FireInTheHole()
        {
            //firenthol
            //012345678
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("1,4") + cr.ExtractValue("5,6,3");
                int rhs = cr.ExtractValue("6,7,8,3");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 9; i++)
            {
                int min = 0;
                if (i == 0 || i == 1 || i == 5 || i == 6) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(9, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int lhs = cc.ExtractValue("0,1,2,3") + cc.ExtractValue("1,4") + cc.ExtractValue("5,6,3");
                int rhs = cc.ExtractValue("6,7,8,3");

                QueuedConsole.WriteImmediate(string.Format("Lhs : {0}, Rhs : {1}, Set : {2}", lhs, rhs, string.Join("-", set)));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/send-more-money/
        /// </summary>
        void SendMoreMoney()
        {
            //sendmory
            //01234567
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,1,2,3") + cr.ExtractValue("4,5,6,1");
                int rhs = cr.ExtractValue("4,5,2,1,7");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 8; i++)
            {
                int min = 0;
                if (i == 0 || i == 4) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(8, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);

                QueuedConsole.WriteImmediate(string.Format("{0}", cc.ExtractValueSum()));
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/logical-solutions-2/
        /// ghi + jkl = mno
        /// </summary>
        void LogicalSolutions_2()
        {
            //ghijklmno
            //012345678
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,1,2") + cr.ExtractValue("3,4,5");
                int rhs = cr.ExtractValue("6,7,8");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 9; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 3, 6 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(9, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            HashSet<int> solutions = new HashSet<int>();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int lhs = cc.ExtractValue("0,1,2") + cc.ExtractValue("3,4,5");
                int rhs = cc.ExtractValue("6,7,8");
                solutions.Add(rhs);
                QueuedConsole.WriteImmediate(string.Format("Lhs : {0}, Rhs : {1}, Set : {2}", lhs, rhs, string.Join("-", set)));
            }
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
            QueuedConsole.WriteImmediate(string.Format("Possible values of melon : {0}", solutions.Count));
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/logical-solutions/
        /// aaa + bbb + ccc = ddd
        /// </summary>
        void LogicalSolutions_1()
        {
            //abcd
            //0123
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,0,0") + cr.ExtractValue("1,1,1") + cr.ExtractValue("2,2,2");
                int rhs = cr.ExtractValue("3,3,3");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                int min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(4, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            HashSet<int> solutions = new HashSet<int>();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int lhs = cc.ExtractValue("0,0,0") + cc.ExtractValue("1,1,1") + cc.ExtractValue("2,2,2");
                int rhs = cc.ExtractValue("3,3,3");
                solutions.Add(rhs);
                QueuedConsole.WriteImmediate(string.Format("Lhs : {0}, Rhs : {1}, Set : {2}", lhs, rhs, string.Join("-", set)));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/simpson-cryptogram/
        /// memo + from = homer
        /// </summary>
        void SimpsonCryptogram()
        {
            //meofrh
            //012345
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,1,0,2") + cr.ExtractValue("3,4,2,0");
                int rhs = cr.ExtractValue("5,2,0,1,4");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 6; i++)
            {
                int min = 0;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(6, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            HashSet<int> solutions = new HashSet<int>();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int lhs = cc.ExtractValue("0,1,0,2") + cc.ExtractValue("3,4,2,0");
                int rhs = cc.ExtractValue("5,2,0,1,4");
                if (solutions.Add(rhs))
                {
                    QueuedConsole.WriteImmediate(string.Format("Possible Rhs : {0}", rhs));
                }
                QueuedConsole.WriteImmediate(string.Format("Lhs : {0}, Rhs : {1}, Set : {2}", lhs, rhs, string.Join("-", set)));
            }
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
            QueuedConsole.WriteFinalAnswer(string.Format("Possible values of homer : {0}", solutions.Count));
        }

        /// <summary>
        /// https://brilliant.org/problems/long-cryptogram/
        /// water + earth = melon
        /// </summary>
        void WaterMelonCryptogram()
        {
            //waterhmlon
            //0123456789
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int lhs = cr.ExtractValue("0,1,2,3,4") + cr.ExtractValue("3,1,4,2,5");
                int rhs = cr.ExtractValue("6,3,7,8,9");

                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 3, 6 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            HashSet<int> solutions = new HashSet<int>();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int lhs = cc.ExtractValue("0,1,2,3,4") + cc.ExtractValue("3,1,4,2,5");
                int rhs = cc.ExtractValue("6,3,7,8,9");
                if (solutions.Add(rhs))
                {
                    QueuedConsole.WriteImmediate(string.Format("Possible Rhs : {0}", rhs));
                }
                QueuedConsole.WriteImmediate(string.Format("Lhs : {0}, Rhs : {1}, Set : {2}", lhs, rhs, string.Join("-", set)));
            }
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
            QueuedConsole.WriteImmediate(string.Format("Possible values of melon : {0}", solutions.Count));
            QueuedConsole.WriteFinalAnswer(string.Format("Sum of all possible values of melon : {0}", solutions.Sum()));
        }

        /// <summary>
        /// https://brilliant.org/problems/a-problem-by-jingyang-tan/
        /// If each letter represents different digits, and an identical letter means an identical digit, please find out what is R+A+F+F+L+E+S.
        /// Please Enjoy this simple Yet hard question! This question is adapted from the Raffles Instituition Primary Maths World Competition(Raffles Instituition is one of the 
        /// most prestigious high school in Singapore!)
        /// </summary>
        void PukingMultiplicationOut()
        {
            //rafles
            //012345
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int rf = cr.ExtractValue("0,2");
                int les = cr.ExtractValue("3,4,5");
                if (rf * cr.ExtractValue("2") == les)
                {
                    int el = cr.ExtractValue("4,3");
                    if (rf * cr.ExtractValue("1") == el)
                    {
                        if (les + el * 10 == cr.ExtractValue("5,5,5")) return true;
                        else return false;
                    }
                    else return false;
                }
                else return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 6; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 1, 3, 4, 5 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(6, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                QueuedConsole.WriteImmediate(string.Format("raffles: {0}, r+a+f+f+l+e+s : {1}", string.Join("", set),
                    MathFunctions.DigitSum(cc.ExtractValue("0,1,2,2,3,4,5"))));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/cryptograma/
        ///     ABCD
        /// x   BADC 
        ///   B6DDD6B
        /// </summary>
        void Cryptograma()
        {
            //abcd
            //0123
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int m1 = cr.ExtractValue("0,1,2,3");
                int m2 = cr.ExtractValue("1,0,3,2");

                int lhs = m1 * m2;
                int b = cr.ExtractValue("1");
                int d = cr.ExtractValue("3");
                int rhs = int.Parse(b + "6" + d + "" + d + "" + d + "6" + b);
                return lhs == rhs;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 1 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(4, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int ab = cc.ExtractValue("0,1");
                int cd = cc.ExtractValue("2,3");
                QueuedConsole.WriteImmediate(string.Format("ab: {0}, cd : {1}, ab x cd : {2} ", ab, cd, ab * cd));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/berzerk/
        ///     BANANA
        ///  +   LEMON
        ///  +    PEAR
        ///  =  ORANGE
        /// </summary>
        void Berzerk()
        {
            //banlemoprg
            //0123456789
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int banana = cr.ExtractValue("0,1,2,1,2,1");
                int lemon = cr.ExtractValue("3,4,5,6,2");
                int pear = cr.ExtractValue("7,4,1,8");
                int orange = cr.ExtractValue("6,8,1,2,9,4");
                return (banana + lemon + pear) == orange;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 3, 6, 7 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int orange = cc.ExtractValue("6,8,1,2,9,4");
                QueuedConsole.WriteImmediate(string.Format("orange: {0}", orange));
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/mistakes-do-give-rise-to-problems/
        /// bob = dad * (0.hulkhulkhulk......)
        /// gcd(bob, dad) = 1.
        /// https://brilliant.org/problems/cryto-division/
        /// bob = dad * (0.hulkhulkhulk......)
        /// gcd(bob, dad) != 1.
        /// https://brilliant.org/problems/decimal-cryptarithm/
        /// </summary>
        void BodahulkRecurringDecimal()
        {
            //bodahulk
            //01234567
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int bob = cr.ExtractValue("0,1,0");
                int dad = cr.ExtractValue("2,3,2");

                if (MathFunctions.GCD(bob, dad) != 1) return false;
                string hulk = cr.ExtractValueAsString("4,5,6,7");
                double hulkVal = double.Parse("0." + hulk + hulk + hulk);
                if (((int)Math.Ceiling(hulkVal * dad)) == bob)
                {
                    string lhs = DoubleConverter.ToExactString(bob * 1.0 / dad);
                    lhs = lhs.Substring(0, lhs.Length > 20 ? 18 : lhs.Length);
                    if (("0." + hulk + hulk + hulk + hulk + hulk + hulk).StartsWith(lhs)) return true;
                    else return false;
                }
                else return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 8; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 2 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(8, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
            foreach (List<int> set in sets)
            {
                CryptCoefficients cc = new CryptCoefficients(set);
                int eve = cc.ExtractValue("0,1,0");
                int did = cc.ExtractValue("2,3,2");
                int talk = cc.ExtractValue("4,5,6,7");
                QueuedConsole.WriteImmediate("EVE : {0}, DID : {1} , TALK: {2}", eve, did, talk);
                QueuedConsole.WriteImmediate("EVE + DID + TALK: {0}", eve + did + talk);
            }
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/japanese-cryptogram/
        ///     kimono
        /// +    osaka
        /// +  sashimi
        /// =  karaoke
        /// find osaka
        /// </summary>
        void JapaneseCryptogram()
        {
            //kimonsahre
            //0123456789
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int kimono = cr.ExtractValue("0,1,2,3,4,3");
                int osaka = cr.ExtractValue("3,5,6,0,6");
                int sashimi = cr.ExtractValue("5,6,5,7,1,2,1");
                int karaoke = cr.ExtractValue("0,6,8,6,3,0,9");
                return kimono + osaka + sashimi == karaoke;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                int min = 0;
                if ((new HashSet<int>() { 0, 3, 5, 6 }).Contains(i))
                {
                    min = 1;
                }
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(10, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
            foreach (List<int> set in sets)
            {
                int osaka = (new CryptCoefficients(set)).ExtractValue("3,5,6,0,6");
                QueuedConsole.WriteImmediate(string.Format("osaka: {0}", osaka));
            }
            QueuedConsole.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/cryptarithms-2/
        /// </summary>
        void Merica()
        {
            //a-e-i-o-h-w-d-s-t
            //0-1-2-3-4-5-6-7-8
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int hawaii = cr.ExtractValue("4,0,5,0,2,2");
                int idaho = cr.ExtractValue("2,6,0,4,3");
                int iowa = cr.ExtractValue("2,3,5,0");
                int ohio = cr.ExtractValue("3,4,2,3");
                int states = cr.ExtractValue("7,8,0,8,1,7");
                return hawaii + idaho + iowa + ohio == states;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 9; i++)
            {
                int min = 0;
                if (i == 2 || i == 3 || i == 4 || i == 7) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(9, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
            foreach (List<int> set in sets)
            {
                int aeiohwdst = (new CryptCoefficients(set)).ExtractValue("0,1,2,3,4,5,6,7,8");
                QueuedConsole.WriteImmediate(string.Format("aeiohwdst: {0}", aeiohwdst));
            }
        }

        /// <summary>
        /// https://brilliant.org/problems/57th-imo-in-hong-kong/
        /// </summary>
        void ImoInHongKong()
        {
            //i-m-o-n-h-g-k
            //0-1-2-3-4-5-6
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int imo = cr.ExtractValue("0,1,2");
                int _in = cr.ExtractValue("0,3");
                int hong = cr.ExtractValue("4,2,3,5");
                int kong = cr.ExtractValue("6,2,3,5");
                return 2073 + imo + _in + hong == kong;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 7; i++)
            {
                int min = 0;
                if (i == 0 || i == 4 || i == 6) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            UniqueArrangements<int> ua = solver.GetAllSolutions(7, iterators);
            List<List<int>> sets = ua.ExtractAsSets();
            QueuedConsole.WriteImmediate(string.Format("Number of possible solutions : {0}", ua.GetCount()));
        }

        /// <summary>
        /// https://brilliant.org/problems/impossible-cryptarithm-part-3/
        /// </summary>
        void ImpossibleCryptarithmPart3()
        {
            //a-b-c-d-e-f-g-h-i-j
            //0-1-2-3-4-5-6-7-8-9
            long maxValue = 0;
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int abc = cr.ExtractValue("0,1,2");
                int de = cr.ExtractValue("3,4");
                int fgh = cr.ExtractValue("5,6,7");
                int ij = cr.ExtractValue("8,9");
                long v1 = abc * de, v2 = fgh * ij;
                if (v1 == v2) { maxValue = Math.Max(maxValue, v1); return true; }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 10; i++)
            {
                int min = 0;
                if (i == 0 || i == 5) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            solver.GetAllSolutions(10, iterators);
            QueuedConsole.WriteImmediate(string.Format("maxValue : {0}", maxValue));
        }

        /// <summary>
        /// https://brilliant.org/problems/a-kiss-problem/
        /// </summary>
        void AKissProblem()
        {
            HashSet<int> Squares = new HashSet<int>();
            int n = 999, nsqmax = 10000 * 1000;
            while (1 > 0)
            {
                ++n;
                int nsq = n * n;
                if (n >= nsqmax) break;

                Squares.Add(nsq);
            }
            //p-a-s-i-o-n-k
            //0-1-2-3-4-5-6
            int kissValue = 0;
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int passion = cr.ExtractValue("0,1,2,2,3,4,5");
                int kiss = cr.ExtractValue("6,3,2,2");
                if (!Squares.Contains(passion)) return false;

                if ((int)Math.Sqrt(passion) == kiss) { kissValue = kiss; return true; }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 7; i++)
            {
                int min = 0;
                if (i == 0 || i == 6) min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
            solver.GetAllSolutions(7, iterators);
            QueuedConsole.WriteImmediate(string.Format("maxValue : {0}", kissValue));
        }

        /// <summary>
        /// https://brilliant.org/problems/valentine-vinculum-part-2/
        /// </summary>
        void ValentineVinculum()
        {
            //a-b-c-d
            //0-1-2-3
            Dictionary<string, HashSet<int>> PossibleCodes = new Dictionary<string, HashSet<int>>();
            Func<CryptRule, bool> rule = delegate (CryptRule cr)
            {
                int code = cr.ExtractValue("0,1,2,3");
                long p = cr.ExtractProduct("0,1,2,3");
                int coeffSum = cr.CoefficientSum();

                string psq = "" + (p * p);
                if (psq.StartsWith("1"))
                {
                    string key = psq.Substring(1, psq.Length - 1);
                    if (PossibleCodes.ContainsKey(key))
                    {
                        PossibleCodes[key].Add(code);
                    }
                    else PossibleCodes.Add(key, new HashSet<int>() { code });
                }
                return false;
            };

            CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
            Cryptarithm cArithm = new Cryptarithm(rules);

            List<Iterator<int>> iterators = new List<Iterator<int>>();
            for (int i = 0; i < 4; i++)
            {
                int min = 1;
                Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 9, Min = min });
                iterators.Add(iter);
            }

            CryptRecursiveSolver solver = new CryptRecursiveSolver(false, cArithm);
            solver.GetAllSolutions(4, iterators);

            PossibleCodes.Where(kvp => kvp.Value.Count > 1).ForEach(x =>
            {
                if (x.Value.Count > 24)
                {
                    QueuedConsole.WriteImmediate("------Number is 1{0}----------", x.Key);
                    x.Value.ForEach(y =>
                    {
                        QueuedConsole.WriteImmediate("Code : {0}", y);
                    });

                    QueuedConsole.WriteImmediate("----------------");
                }
            });
        }
    }
}