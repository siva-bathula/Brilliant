using GenericDefs.Classes;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.NumberTypes;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using GenericDefs.Functions.Algorithms.DP;
using GenericDefs.Functions.Algorithms.Sort;
using GenericDefs.Functions.NumberTheory;
using Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Brilliant.ComputerScience
{
    public class ComputerScienceCollection : ISolve, IBrilliant, IProblemName
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
            (new Collection()).Solve();
        }

        internal class Collection
        {
            void SolveUsingBiggerStack()
            {
                System.Threading.Thread t = new System.Threading.Thread(MysteryFunction, 1000000);
                t.Start();
                t.Join();
            }

            internal void Solve()
            {
                //SolveUsingBiggerStack();
                //ItsBeenALongDay();
                //SumOfPowers();
                //FractalExploration();
                PermutationPower();
            }

            /// <summary>
            /// https://brilliant.org/problems/permutation-power/
            /// </summary>
            void PermutationPower()
            {
                for (int n = 100; n <= 100; n++)
                {
                    List<int> set = Enumerable.Range(1, n).ToList();
                    string identity = string.Join("", set);
                    int fsMax = 0;
                    int count = 0;
                    List<int> permutation = new List<int>(set);
                    Random rnd = new Random();
                    while (++count <= 100000000)
                    {
                        rnd.Shuffle(permutation);

                        int fs = 0;
                        List<int> oset = new List<int>(permutation);

                        while (++fs >= 0)
                        {
                            List<int> osetNew = new List<int>(n);
                            for (int a = 0; a < n; a++)
                            {
                                osetNew.Add(oset[permutation[a] - 1]);
                            }
                            if (string.Join("", osetNew).Equals(identity)) { break; }
                            oset = osetNew.ToList();
                        }
                        fsMax = Math.Max(fsMax, fs);
                        if(count % 500 == 0) QueuedConsole.WriteImmediate("count: {0}, fsigma: {1} ", count, fsMax);
                    }
                    QueuedConsole.WriteImmediate("n: {0}, f(sigma): {1}", n, fsMax);
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/smart-matrix-multiplication/
            /// </summary>
            void SmartMatrixMultiplication()
            {
                List<int> numbers = new List<int>();
                ((Http.Request.GetHtmlResponse("http://pastebin.com/raw/e06yJM12"))
                 //"30 13 16 32 2 6 19 11 19 16 27 7 18 1 32 21 12 3 8 2"
                    .Splitter(StringSplitter.SplitUsing.LineBreak)).ForEach(x =>
                    {
                        numbers.Add(int.Parse(x));
                    }
                );
                QueuedConsole.WriteImmediate("{0}", GenericDefs.Functions.Algorithms.DP.Matrix.MatrixChainOrder(numbers.ToArray()));
            }

            /// <summary>
            /// https://brilliant.org/problems/connect-the-dots-3/
            /// </summary>
            void ConnectTheDots3()
            {
                Random r = new Random();
                long nSimulations = 0;
                long nFavourable = 0;
                while (++nSimulations <= 10000000)
                {
                    List<List<int>> points = new List<List<int>>();
                    Enumerable.Range(0, 4).ForEach(n =>
                    {
                        points.Add(new List<int>() { r.Next(1, 100), r.Next(1, 100) });
                    });

                    if (GenericDefs.Functions.CoordinateGeometry.Functions.IsValidQuadrilateral(points)) nFavourable++;
                }
                QueuedConsole.WriteImmediate("{0}", nFavourable * 1.0 / nSimulations);
            }

            /// <summary>
            /// https://brilliant.org/problems/creating-sorted-arrays/?ref_id=1280449
            /// </summary>
            void CreatingSortedArrays()
            {
                BigInteger b = 1;
                for(int i= 1; i <= 567; i++)
                {
                    b *= (1366 + i);
                    b /= i;
                }

                BigInteger mod = BigInteger.Pow(10, 9) + 7;
                b = b % (mod);
                QueuedConsole.WriteImmediate("{0}", b);
            }

            /// <summary>
            /// https://brilliant.org/problems/t9-titans/
            /// </summary>
            void T9Titans()
            {
                Dictionary<char, int> Map = new Dictionary<char, int>() {
                    { 'a', 2}, { 'b', 2 }, { 'c', 2 },
                    { 'd', 3}, { 'e', 3 }, { 'f', 3 },
                    { 'g', 4}, { 'h', 4 }, { 'i', 4 },
                    { 'j', 5}, { 'k', 5 }, { 'l', 5 },
                    { 'm', 6}, { 'n', 6 }, { 'o', 6 },
                    { 'p', 7}, { 'q', 7 }, { 'r', 7 }, {'s', 7 },
                    { 't', 8}, { 'u', 8 }, { 'v', 8 },
                    { 'w', 9}, { 'x', 9 }, { 'y', 9 }, {'z', 9 }
                };

                Dictionary<BigInteger, int> Sequences = new Dictionary<BigInteger, int>();
                (Http.Request.GetHtmlResponse("https://gist.githubusercontent.com/anonymous/26bd072c3eb43d4eea443516e77c8e37/raw/52c3bfe7b71ce49ab8820ef3c8688d8f3ddde651/gistfile1.txt"))
                    .Splitter(StringSplitter.SplitUsing.LineBreak).ForEach(x =>
                    {
                        StringBuilder s = new StringBuilder();
                        x.ForEach(c => {
                            if (Map.ContainsKey(c)) s.Append(Map[c]);
                            else s.Append(1);
                        });
                        Sequences.AddOrUpdate(BigInteger.Parse(s.ToString()), 1);
                    }
                );

                var oSequence = Sequences.OrderByValue(OrderByDirection.Descending);

                var first = oSequence.First();
                QueuedConsole.WriteImmediate("{0} : {1}", first.Key, first.Value);
            }

            /// <summary>
            /// https://brilliant.org/problems/pretty-numbers/
            /// </summary>
            void PrettyNumbers()
            {
                HashSet<uint> numbers = new HashSet<uint>();
                ((Http.Request.GetHtmlResponse("https://gist.githubusercontent.com/anonymous/fc32a98bd0b1317ade5dd4ba4b806009/raw/4a3324c12ba25005167d5c0c8ff186df747a916c/gistfile1.txt"))
                    .Splitter(StringSplitter.SplitUsing.Space)).ForEach(x =>
                    {
                        numbers.Add(uint.Parse(x));
                    }
                );

                HashSet<long> PowersOfTwo = new HashSet<long>();
                Enumerable.Range(0, 32).ForEach(n =>
                {
                    PowersOfTwo.Add((long)Math.Pow(2, n));
                });

                HashSet<uint> copy = new HashSet<uint>(numbers);
                int nPairs = 0;
                numbers.ForEach(n =>
                {
                    StringBuilder negation = new StringBuilder(Binary.ToBinary(~n));
                    if (copy.Contains(~n)) nPairs++;

                    for (int i = 0; i < negation.Length; i++)
                    {
                        negation[i] = negation[i] == '1' ? '0' : '1';
                        uint negate = Binary.ToUInt(negation.ToString());
                        if (copy.Contains(negate))
                        {
                            if (PowersOfTwo.Contains(GenericDefs.Functions.Logic.Bitwise.XOR(n, negate) + 1)) nPairs++;
                        }
                    }

                    if (copy.Contains(n)) copy.Remove(n);
                });
                QueuedConsole.WriteImmediate("Number of unordered pairs : {0}", nPairs);
            }

            /// <summary>
            /// https://brilliant.org/problems/you-complete-me-2/
            /// </summary>
            void YouCompleteMe2()
            {
                HashSet<uint> numbers = new HashSet<uint>();
                ((Http.Request.GetHtmlResponse("https://gist.githubusercontent.com/anonymous/fc32a98bd0b1317ade5dd4ba4b806009/raw/4a3324c12ba25005167d5c0c8ff186df747a916c/gistfile1.txt"))
                    .Splitter(StringSplitter.SplitUsing.Space)).ForEach(x =>
                    {
                        numbers.Add(uint.Parse(x));
                    }
                );

                int nPairs = 0;
                numbers.ForEach(n =>
                {
                    if (numbers.Contains(~n)) { nPairs++; }
                });
                QueuedConsole.WriteImmediate("Number of complementary pairs : {0}", nPairs);
            }

            /// <summary>
            /// https://brilliant.org/problems/this-or-that/
            /// </summary>
            void ThisOrThat()
            {
                Dictionary<int, int> BitPositionCounters = new Dictionary<int, int>();

                int N = 0;
                ((Http.Request.GetHtmlResponse("https://gist.githubusercontent.com/anonymous/b876f801562bd8dcbc73373c040edbee/raw/495c9ca38f4a03626777ae7502ad1c0f8f3e75ed/gistfile1.txt"))
                    .Splitter(StringSplitter.SplitUsing.Space)).ForEach(x =>
                    {
                        int key = 0;
                        ((Binary.ToBinary(int.Parse(x))).Reverse()).ForEach(d =>
                        {
                            if (char.GetNumericValue(d) == 1) BitPositionCounters.AddOrUpdate(key, 1);
                            key++;
                        });
                        N++;
                    });

                BigInteger sum = 0;
                BigInteger NPairs = PermutationsAndCombinations.nCrBig(N, 2);
                BitPositionCounters.ForEach(b =>
                {
                    sum += (BigInteger.Pow(2, b.Key)) * (NPairs - (b.Value < N - 1 ? PermutationsAndCombinations.nCrBig(N - b.Value, 2) : 0));
                });
                QueuedConsole.WriteImmediate("sum : {0}, in binary : {1}", sum, Binary.ToBinary(sum));
            }

            /// <summary>
            /// https://brilliant.org/problems/breaking-rsa-small-e/
            /// </summary>
            void BreakingRSASmallE()
            {
                BigInteger n = BigInteger.Parse("6865653949821276403125067");
                BigInteger p = BigInteger.Parse("726455342971");
                BigInteger q = BigInteger.Parse("9450896075377");

                if (n == p * q)
                {
                    QueuedConsole.WriteImmediate("factored correctly.");
                }

                BigInteger phi = (p - 1) * (q - 1);
                BigInteger e = BigInteger.Parse("3");

                if (phi % e == 0)
                {
                    QueuedConsole.WriteImmediate("e divides phi?");
                }

                BigInteger[] d = GenericDefs.Functions.MathFunctions.Extended_GCD(e, phi);
                BigInteger c = BigInteger.Parse("309717089812744704");
                BigInteger m = BigInteger.ModPow(c, d[1], n);
            }

            /// <summary>
            /// https://brilliant.org/problems/concatenated-primes/
            /// </summary>
            void ConcatenatedPrimes()
            {
                string s = "";
                List<int> primes = Prime.GeneratePrimesNaiveNMax(10000);
                foreach(int p in primes)
                {
                    s += "" + p;
                    if (s.Length > 1000) break;
                }
                QueuedConsole.WriteImmediate("1000th digit : {0}", s[1000]);
            }

            /// <summary>
            /// https://brilliant.org/problems/number-overload/
            /// </summary>
            void NumberOverload()
            {
                int N = 0;
                string L = string.Empty;
                using (var stream = Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.NoO.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        int sCounter = 0;
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (sCounter == 0) { N = int.Parse(s); sCounter++; }
                            else L = s.ToString();
                        }
                    }
                }

                int nDigits = 1000;
                int A = 0, B = 0, C = 0;
                while (1 > 0)
                {
                    BigInteger number = BigInteger.Parse(L.Substring(0, nDigits));
                    N -= nDigits;

                    int a = (int)(number % 9);
                    int b = (int)(number % 77);
                    int c = (int)(number % 41);

                    int n = N;
                    int mul = nDigits;
                    if (n > 0)
                    {
                        while (1 > 0)
                        {

                            a *= mul;
                            a %= 9;

                            b *= mul;
                            b %= 77;

                            c *= mul;
                            c %= 41;

                            if (mul == nDigits) n -= 3;
                            if (n == 0) break;

                            if (n < 3)
                            {
                                mul = (int)Math.Pow(10, n);
                                n = 0;
                            }
                        }
                    }

                    A += a;
                    B += b;
                    C += c;

                    if (N > 0)
                    {
                        L = L.Substring(nDigits, N);
                    } else break;
                }

                A %= 9;
                B %= 77;
                C %= 41;

                QueuedConsole.WriteImmediate("{0}", 3 * A - 7 * B + 10 * C);
            }

            /// <summary>
            /// https://brilliant.org/problems/a-kings-problem/
            /// </summary>
            void AKingsProblem()
            {
                List<Tuple<long, long>> AB = new List<Tuple<long, long>>();
                using (var stream = Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.King.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] input = s.Splitter(StringSplitter.SplitUsing.Space);
                            AB.Add(new Tuple<long, long>(long.Parse(input[0]), long.Parse(input[1])));
                        }
                    }
                }

                BigInteger sum = 0;
                AB.ForEach(x =>
                {
                    //sum += RemainderCalculator.RemainderWithNestedPowers(new long[] { x.Item1, x.Item2 }, 100);
                    sum += BigInteger.ModPow(x.Item1, x.Item2, 100);
                });
                QueuedConsole.WriteImmediate("{0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/consecutive-primes/
            /// </summary>
            void ConsecutivePrimes()
            {
                List<long> primes = KnownPrimes.GetAllKnownFirstNPrimes(1000000);
                long sum = 0, sumPrev = 0;
                long prev = 0;
                foreach (long p in primes)
                {
                    long dsum = MathFunctions.DigitSum(p);
                    if(sumPrev > 0)
                    {
                        if(dsum == sumPrev)
                        {
                            sum = prev + p;
                            break;
                        }
                    }

                    sumPrev = dsum;
                    prev = p;
                }
                QueuedConsole.WriteImmediate("a+b : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/hotel-puzzle/
            /// </summary>
            void HotelPuzzle()
            {
                Dictionary<int, bool> Rooms = new Dictionary<int, bool>();
                Enumerable.Range(1, 1000).ForEach(n => { Rooms.Add(n, true); });

                Enumerable.Range(1, 1000).ForEach(w =>
                {
                    Enumerable.Range(1, 1000).ForEach(n =>
                    {
                        if (n % w == 0)
                        {
                            Rooms[n] = !Rooms[n];
                        }
                    });
                });
                QueuedConsole.WriteImmediate("Number of open doors : {0}", (Rooms.Where(kvp => kvp.Value == true)).Count());
            }

            /// <summary>
            /// https://brilliant.org/problems/a-programmer-quit-his-job-and-left-this-message-in/
            /// </summary>
            void MysteryFunction()
            {
                Func<List<int>, List<int>> Mystery = null;
                Mystery = delegate (List<int> input)
                {
                    if (input.Count <= 1) return input;

                    int i0 = input[0];

                    List<int> workingInput = input.GetRange(1, input.Count - 1);
                    List<int> l1 = (workingInput.Where(x => x <= i0)).ToList();
                    List<int> l2 = (workingInput.Where(x => x > i0)).ToList();

                    List<int> retVal = new List<int>();
                    retVal.AddRange(Mystery(l1));
                    retVal.Add(i0);
                    retVal.AddRange(Mystery(l2));
                    return retVal;
                };

                List<int> theList = new List<int>() { 1, 4, 2, 3, 3, 45, 6, 7, 8, 5, 4, 3, 2, 21, 2, 3, 4, 5, 6, 7 };
                List<int> newList = Mystery(theList);
                int sum = 0;
                Enumerable.Range(0, 5).ForEach(x =>
                {
                    sum += newList[x];
                });
                QueuedConsole.WriteImmediate("Sum : {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/divisible-fibonacci-sounds-crazy/
            /// </summary>
            void DivisibleFibonacci()
            {
                long Nmax = (long)Math.Pow(10, 9);
                BigInteger sum = 0;
                FibonacciGenerator fg = new FibonacciGenerator();
                while (1 > 0)
                {
                    BigInteger b = fg.Next();
                    if (b > Nmax) break;

                    if (b % 3 == 0) sum += b;
                }
                QueuedConsole.WriteImmediate("Sum: {0}", sum.ToString());
            }

            /// <summary>
            /// https://brilliant.org/problems/i-exist-sometimes/
            /// </summary>
            void IExistSometimes()
            {
                int n = 9;
                long count = 0;
                Enumerable.Range(1, n).ForEach(N =>
                  {
                      long c1 = 0;
                      int a = N;
                      int b = 0, c = 0;
                      Enumerable.Range(1, N).ForEach(x =>
                      {
                          c1 += (long)(Math.Pow(10, --a) * Math.Pow(8, b) * Math.Pow(9, c));

                          if (b == 0) { b++; }
                          else { c++; }
                      });
                      QueuedConsole.WriteImmediate("10^{0} : {1}", N, c1);

                      count += c1;
                  });
                QueuedConsole.WriteImmediate("10^{0} : {1}", n, count);
            }

            /// <summary>
            /// https://brilliant.org/problems/successive-totients/
            /// </summary>
            void SuccessiveTotients()
            {
                Dictionary<int, int> Totients = new Dictionary<int, int>();
                Totients.Add(1, 0);

                ClonedPrimes p = KnownPrimes.CloneKnownPrimes(1, 2014);

                Func<int, int> Totient = null;
                Totient = delegate (int x)
                {
                    if (Totients.ContainsKey(x))
                    {
                        int tVal = Totients[x];
                        if (tVal <= 1) return tVal;
                        return 1 + Totient(tVal);
                    }

                    int totient = (int)EulerTotient.CalculateTotient(x, p);
                    Totients.AddOnce(x, totient);

                    if (totient <= 1) return totient;

                    return 1 + Totient(totient);
                };

                int sum = 0;
                Enumerable.Range(1, 2014).ForEach(n =>
                {
                    sum += Totient(n);
                });
                QueuedConsole.WriteImmediate("sum : {0}, last three digits of sum : {1}", sum, sum % 1000);
            }

            /// <summary>
            /// https://brilliant.org/problems/find-arrays-in-an-array/
            /// </summary>
            void FindArrayInArray()
            {
                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/tnr6kwi4");
                string[] array = text.Splitter(StringSplitter.SplitUsing.Space);

                List<long> Numbers = new List<long>();
                array.ForEach(x =>
                {
                    Numbers.Add(long.Parse(x));
                });

                text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/YrDBUFTp");
                string[] array2 = text.Splitter(StringSplitter.SplitUsing.Space);

                array2.ForEach(x =>
                {
                    Numbers.Add(long.Parse(x));
                });

                long counter = 0;
                Dictionary<long, List<int>> Map = new Dictionary<long, List<int>>();
                for (int i = 0; i < Numbers.Count; i++)
                {
                    if (Map.ContainsKey(Numbers[i])) { Map[Numbers[i]].Add(i); }
                    else { Map.Add(Numbers[i], new List<int>() { i }); }
                }
                Map.ForEach(x =>
                {
                    int n = x.Value.Count;
                    if (n > 1) {
                        counter += n * (n - 1) / 2;
                    }
                });
                QueuedConsole.WriteImmediate("Number of sub-arrays: {0}", counter);
            }

            /// <summary>
            /// https://brilliant.org/problems/welcome-to-the-rice-fields/
            /// </summary>
            void RiceFields()
            {
                int n = 1000;
                int[,] farm = new int[n, n];

                int row = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.rice-field.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] line = s.Splitter(StringSplitter.SplitUsing.Space);
                            int col = 0;
                            line.ForEach(x =>
                            {
                                farm[row, col] = int.Parse(x);
                                col++;
                            });
                            row++;
                        }
                    }
                }

                int maxSize = GenericDefs.Functions.Algorithms.DP.ArrayMatrix.MaxSquareSubMatrix(farm);
                QueuedConsole.WriteImmediate("Side length: {0}, max area : {1}", maxSize, maxSize * maxSize);
            }

            /// <summary>
            /// https://brilliant.org/problems/stacked-carts/
            /// </summary>
            void StackedCarts()
            {
                int N = 16;

                int count = 0;
                SimpleHashSet sh = new SimpleHashSet(maxLength: 10000000);
                Action<int, Stack<int>, Stack<int>, Stack<int>> Operate = null;
                Operate = delegate (int depth, Stack<int> LeftTrack, Stack<int> SpurTrack, Stack<int> RightTrack) {
                    for (int i = 0; i <= 1; i++)
                    {
                        Stack<int> nextLeft = new Stack<int>(LeftTrack);
                        Stack<int> nextSpur = new Stack<int>(SpurTrack);
                        Stack<int> nextRight = new Stack<int>(RightTrack);
                        if (i == 0)
                        {
                            if (nextLeft.Count == 0) continue;

                            nextSpur.Push(nextLeft.Pop());
                        }
                        else
                        {
                            if (nextSpur.Count == 0) continue;

                            nextRight.Push(nextSpur.Pop());
                        }

                        if(nextRight.Count == N)
                        {
                            sh.AddQueue(string.Join("-", nextRight));
                            count++;
                        } else {
                            Operate(depth + 1, nextLeft, nextSpur, nextRight);
                        }
                    }
                };
                
                Stack<int> Init = new Stack<int>();
                Enumerable.Range(1, N).ForEach(x => { Init.Push(x); });
                Operate(0, Init, new Stack<int>(), new Stack<int>());
                QueuedConsole.WriteImmediate("For N : {0}, Number of ways permuting the carts : {1}, Counter: {2}", N, sh.GetCount(), count);
            }

            /// <summary>
            /// https://brilliant.org/problems/intersecting-hoppers/
            /// </summary>
            void IntersectingHoppers()
            {
                int nCases = 0;
                string text = Http.Request.GetHtmlResponse("http://pastebin.com/raw/dSsae1XP");
                string[] inputs = text.Splitter(StringSplitter.SplitUsing.LineBreak);
                inputs.ForEach(x =>
                {
                    string[] input = x.Splitter(StringSplitter.SplitUsing.Space);
                    int nMax = 150000;
                    int n = 0;
                    HashSet<int> x1 = new HashSet<int>(), x2 = new HashSet<int>();
                    x1.Add(int.Parse(input[0]));
                    x2.Add(int.Parse(input[2]));
                    int v1 = int.Parse(input[1]);
                    int v2 = int.Parse(input[3]);

                    int x1i = x1.First(), x2i = x2.First();
                    while (++n <= nMax)
                    {
                        x1i += v1;
                        x2i += v2;

                        x1.Add(x1i);
                        x2.Add(x2i);

                        if (x1.Contains(x2i) || x2.Contains(x1i))
                        {
                            nCases++;
                            break;
                        }

                        if (n % 10000 == 0)
                        {
                            bool found = false;
                            foreach(int a in x1)
                            {
                                if (x2.Contains(a))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                foreach(int b in x2)
                                {
                                    if (x1.Contains(b))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }

                            if (found)
                            {
                                nCases++;
                                break;
                            }
                        }
                    }
                });
                QueuedConsole.WriteImmediate("Number of cases : {0}", nCases);
            }

            /// <summary>
            /// https://brilliant.org/practice/simulation-techniques-level-4-5-challenges/?problem=playing-tic-tac-toe&subtopic=cryptography-and-simulations&chapter=simulation-techniques
            /// </summary>
            void TicTacToeExpectedValue()
            {
                int n = 3, nMax = n * n;
                List<List<int>> Rows = new List<List<int>>() {
                    new List<int>() {1,2,3 },
                    new List<int>() {4,5,6 },
                    new List<int>() {7,8,9 }
                };
                List<List<int>> Columns = new List<List<int>>() {
                    new List<int>() {1,4,7 },
                    new List<int>() {2,5,8 },
                    new List<int>() {3,6,9 }
                };
                List<List<int>> Diagonals = new List<List<int>>() {
                    new List<int>() {1,5,9 },
                    new List<int>() {3,5,7 }
                };

                List<List<List<int>>> PossibleWinScenarios = new List<List<List<int>>>();
                PossibleWinScenarios.Add(Rows);
                PossibleWinScenarios.Add(Columns);
                PossibleWinScenarios.Add(Diagonals);

                Func<Pool<int>, Pool<int>> initializer = delegate (Pool<int> p) {
                    for (int i = 1; i <= 9; i++)
                    {
                        p.Push(i);
                    }
                    return p;
                };

                int nSim = 0;
                Pool<int> grid = new Pool<int>(initializer);
                double nTotalMoves = 0;
                while (++nSim <= 2000000)
                {
                    grid.ReInitialize();
                    int nMoves = 0;
                    bool isATurn = false;
                    HashSet<int> aMoves = new HashSet<int>();
                    HashSet<int> bMoves = new HashSet<int>();
                    bool isGameOver = false;
                    while (true)
                    {
                        isATurn = !isATurn;
                        ++nMoves;

                        int cur = grid.Pop();

                        if (isATurn) aMoves.Add(cur);
                        else bMoves.Add(cur);

                        if (nMoves >= 5)
                        {
                            foreach (List<List<int>> Scenarios in PossibleWinScenarios)
                            {
                                foreach (List<int> r in Scenarios)
                                {
                                    int aCount = 0, bCount = 0;
                                    foreach (int ri in r)
                                    {
                                        if (aMoves.Contains(ri)) aCount++;
                                        else if (bMoves.Contains(ri)) bCount++;
                                    }
                                    if (aCount == n || bCount == n) { isGameOver = true; }
                                    if (isGameOver) break;
                                }

                                if (isGameOver) break;
                            }
                        }

                        if (isGameOver) break;

                        if (nMoves == nMax) { isGameOver = true; break; }
                    }
                    if (isGameOver) nTotalMoves += nMoves;
                }
                nSim--;
                QueuedConsole.WriteImmediate("Expected value of number of moves : {0} for {1} simulations", nTotalMoves / nSim, nSim);
            }

            /// <summary>
            /// https://brilliant.org/problems/magnet-2/
            /// </summary>
            void Magnet2()
            {
                Func<Pool<int>, Pool<int>> pool1Init = delegate (Pool<int> p)
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        p.Push(i);
                    }
                    return p;
                };

                int nSimulations = 0;
                Random r = new Random();
                Random intGen = new Random();

                Pool<int> pool1 = new Pool<int>(pool1Init);
                pool1.ReInitialize();

                Func<int> GetQuadrant = delegate ()
                {
                    if (pool1.IsEmpty()) pool1.ReInitialize();

                    int p1 = pool1.Pop();

                    if (p1 <= 70) return 1;
                    if (p1 >= 71 && p1 <= 80) return 2;
                    if (p1 >= 81 && p1 <= 90) return 3;
                    if (p1 >= 91 && p1 <= 100) return 4;
                    else
                    {
                        int rNInt = intGen.Next(1, 3);
                        if (rNInt % 3 == 1) return 2;
                        else if (rNInt % 3 == 2) return 3;
                        else return 4;
                    }
                };

                Dictionary<int, int> XOffset = new Dictionary<int, int>() { { 1, 0 }, { 2, 1 }, { 3, 0 }, { 4, 1 } };
                Dictionary<int, int> YOffset = new Dictionary<int, int>() { { 1, 1 }, { 2, 1 }, { 3, 0 }, { 4, 0 } };

                double totalDistance = 0;
                while (++nSimulations <= 10000000)
                {
                    int p1q = GetQuadrant(), p2q = GetQuadrant();

                    double x1 = r.NextDouble() + XOffset[p1q];
                    double y1 = r.NextDouble() + YOffset[p1q];
                    double x2 = r.NextDouble() + XOffset[p2q];
                    double y2 = r.NextDouble() + YOffset[p2q];

                    double d = Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2);
                    d = Math.Sqrt(d);
                    totalDistance += d;
                    if (nSimulations % 100000 == 0) QueuedConsole.WriteImmediate("Average distance : {0} for {1} simulations", totalDistance / nSimulations, nSimulations);
                }
                nSimulations--;
                QueuedConsole.WriteImmediate("Average distance : {0} for {1} simulations", totalDistance / nSimulations, nSimulations);
            }

            /// <summary>
            /// https://brilliant.org/problems/sighsigma/
            /// </summary>
            void SigmaSigh()
            {
                List<long> primes = KnownPrimes.GetAllKnownFirstNPrimes(1000);
                BigInteger p = 1;
                primes.ForEach(x =>
                {
                    p *= (x * x + 1);
                });

                QueuedConsole.WriteImmediate("Number of digits: {0}", p.ToString().Length);
            }

            /// <summary>
            /// https://brilliant.org/problems/geometry-meets-computer-science/
            /// </summary>
            void GeometryPossibleTriangles()
            {
                #region Input
                string input = @"556739
                    923625
                    745437
                    949244
                    985473
                    534499
                    891463
                    822538
                    743998
                    239362
                    765292
                    192312
                    759941
                    884649
                    222779
                    933769
                    941635
                    724231
                    996484
                    718176
                    915516
                    246793
                    356518
                    767317
                    893381
                    255793
                    182666
                    431264
                    869496
                    219435
                    657891
                    672188
                    277963
                    946336
                    799775
                    138389
                    616986
                    596487
                    499345
                    879115
                    283889
                    187125
                    871854
                    553252
                    719371
                    867687
                    865272
                    917499
                    742713
                    135349
                    667464
                    762791
                    859639
                    287193
                    527156
                    962476
                    979612
                    359525
                    524338
                    228329
                    895861
                    881232
                    621975
                    116767
                    813742
                    363222
                    476341
                    742935
                    842348
                    145561
                    933555
                    764574
                    374912
                    227328
                    769985
                    294937
                    722482
                    173281
                    143349
                    826794
                    722864
                    522627
                    982558
                    943483
                    321476
                    781381
                    274141
                    346183
                    978339
                    456674
                    979265
                    936942
                    984924
                    765593
                    697835
                    332579
                    884389
                    827493
                    984988
                    519665
                    584643";
                #endregion

                int n = 0;
                double MaxArea = 0.0;
                string[] tStr = input.Splitter(StringSplitter.SplitUsing.LineBreak);
                tStr.ForEach(x => {
                    string xi = x.Trim(' ');
                    int[] coord = new int[6];
                    Enumerable.Range(0,6).ForEach(y => {
                        coord[y] = (int)char.GetNumericValue(xi[y]);
                    });
                    IEnumerable<IEnumerable<int>> tCoord = coord.Section(2);
                    IEnumerable<int>[] collection = (tCoord).ToArray();
                    List<IEnumerable<int>> list = tCoord.ToList();
                    ++n;
                    if (!GenericDefs.Functions.CoordinateGeometry.Functions.Collinear(list[0].ToList(), list[1].ToList(), list[2].ToList()))
                    {
                        double area = GenericDefs.Functions.CoordinateGeometry.CoordinatesOfTriangle.GetArea(collection[0].ToArray(), collection[1].ToArray(), collection[2].ToArray());
                        MaxArea = Math.Max(MaxArea, area);
                        QueuedConsole.WriteImmediate("n: {0}, area: {1}", n, area);
                    }
                });

                QueuedConsole.WriteImmediate("Maximum area: {0}", MaxArea);
            }

            /// <summary>
            /// https://brilliant.org/problems/do-the-bare-minimum/
            /// </summary>
            void DoTheBareMinimum()
            {
                List<int> MinimumSquares = new List<int>();
                MinimumSquares.Add(0);
                MinimumSquares.Add(1);
                MinimumSquares.Add(2);
                MinimumSquares.Add(3);

                Func<int, int> FindMinSquares = delegate (int n)
                {
                    if (MinimumSquares.Count > n) return MinimumSquares[n];

                    int[] dp = new int[n + 1];
                    int t = 0;
                    MinimumSquares.ForEach(x =>
                    {
                        if (t < n)
                        {
                            if (t < MinimumSquares.Count)
                            {
                                dp[t] = MinimumSquares[t];
                                t++;
                            }
                        }
                    });
                    dp[n] = n;
                    // getMinSquares rest of the table using recursive
                    // formula
                    for (int i = 4; i <= n; i++)
                    {
                        // Go through all smaller numbers to
                        // to recursively find minimum
                        for (int x = 1; x <= i; x++)
                        {
                            int temp = x * x;
                            if (temp > i) break;
                            else dp[i] = Math.Min(dp[i], 1 + dp[i - temp]);
                        }
                    }

                    MinimumSquares.Add(dp[n]);

                    return MinimumSquares[n];
                };

                int sum = 0, N = 1;
                while(++N <= 2015)
                {
                    sum += FindMinSquares(N);
                }

                QueuedConsole.WriteImmediate("Required value: {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/proper-cutting/
            /// </summary>
            void ProperCutting()
            {
                string word = Http.Request.GetHtmlResponse("https://d18l82el6cdm1i.cloudfront.net/uploads/documents/kPgzFEkL-VrXWyb6vIx.txt");
                Dictionary<int, List<int>> StartPositions = new Dictionary<int, List<int>>();
                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = i; j < word.Length; j++)
                    {
                        string s = word.Substring(i, j - i + 1);

                        if (string.Join("", s.Reverse()).Equals(s))
                        {
                            if (StartPositions.ContainsKey(i)) { StartPositions[i].Add(j - i + 1); }
                            else { StartPositions.Add(i, new List<int>() { j - i + 1 }); }
                        }
                    }
                }

                StartPositions.ForEach(y =>
                {
                    y.Value.Sort((a, b) => -1 * a.CompareTo(b));
                });

                int StringLength = word.Length;
                Action<int, int> Recursion = null;
                int minPalindromes = StringLength;

                Dictionary<int, int> MinimumPalindromes = new Dictionary<int, int>();
                Recursion = delegate (int sPos, int depth)
                {
                    if (depth >= minPalindromes) return;
                    if (MinimumPalindromes.ContainsKey(sPos))
                    {
                        if (depth + MinimumPalindromes[sPos] >= minPalindromes) return;
                    }

                    List<int> Start = StartPositions[sPos];
                    int nextStart = sPos;
                    for (int i = 0; i < Start.Count; i++)
                    {
                        nextStart = sPos + Start[i];
                        if (nextStart == StringLength)
                        {
                            if (depth < minPalindromes)
                            {
                                minPalindromes = depth + 1;
                            }
                        }
                        else if (StartPositions.ContainsKey(Start[i] + sPos))
                        {
                            Recursion(nextStart, depth + 1);
                        }
                    }
                };

                for (int y = word.Length - 1; y >= 0; y--)
                {
                    minPalindromes = StringLength;
                    Recursion(y, 0);
                    MinimumPalindromes.Add(y, minPalindromes);
                    QueuedConsole.WriteImmediate("Min. Palindromes required for StartPosition : {0} : {1}", y, minPalindromes);
                }

                QueuedConsole.WriteImmediate("The minimum number of palindromes we need to construct the string: {0}", minPalindromes);
            }

            /// <summary>
            /// https://brilliant.org/problems/yet-unusual-sorting/
            /// </summary>
            void YetUnusualSorting()
            {
                string word = Http.Request.GetHtmlResponse("http://pastebin.com/raw/MnuymxVD");
                string[] numbers = word.Splitter(StringSplitter.SplitUsing.Comma);

                List<int> N = new List<int>();
                numbers.ForEach(x =>
                {
                    N.Add(int.Parse(x));
                });
                int cost = N.Count, length = 0, end = 0;
                int[] sequence = LongestIncreasingSubsequence.GetLongestIncreasingSubsequence(N.ToArray(), out length, out end);
                cost -= length;
                QueuedConsole.WriteImmediate("cost: {0}", cost);
            }

            /// <summary>
            /// https://brilliant.org/problems/pythagorean-primes/
            /// </summary>
            void FindPythogoreanPrimes()
            {
                List<string> numbers = new List<string>();
                using (var stream = Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.pythogorean-primes.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            s = s.Trim();
                            numbers.Add(s);
                        }
                    }
                }

                int count = 0;
                List<long> primes = KnownPrimes.GetAllKnownPrimes();
                long max = primes.Max();
                int sCount = 0;
                foreach (string s in numbers)
                {
                    sCount++;
                    BigInteger b = BigInteger.Parse(s);
                    if (b % 4 == 1)
                    {
                        bool isPrime = true;
                        foreach (long prime in primes)
                        {
                            if(b % prime == 0)
                            {
                                isPrime = false;
                                break;
                            }
                        }
                        if (isPrime)
                        {
                            long pSearch = max;
                            while (pSearch <= 5 * max)
                            {
                                if (b % pSearch == 0)
                                {
                                    isPrime = false;
                                    break;
                                }

                                pSearch += 2;
                            }
                            count++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Count : {0}", count);
            }

            /// <summary>
            /// https://brilliant.org/problems/counting-zeroes/
            /// 100559404365
            /// </summary>
            void CountingZeroes()
            {
                long n = -1, N = 0;

                n = 92745325227;
                N = 92391159543;

                while (++n >= 0)
                {
                    N += ("" + n).Count(x => x == '0');

                    if (n == N && n > 1)
                    {
                        QueuedConsole.WriteImmediate("Smallest n: {0}", n);
                        break;
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/take-me-there/
            /// </summary>
            void TakeMeThere()
            {
                string word = Http.Request.GetHtmlResponse("http://pastebin.com/raw/qC5z2Vir");
                string[] numbers = word.Splitter(StringSplitter.SplitUsing.LineBreak);
                int N = 30000, nTotal = N;
                List<Tuple<long, long>> People = new List<Tuple<long, long>>();
                numbers.ForEach(x =>
                {
                    string[] ni = x.Splitter(StringSplitter.SplitUsing.Space, StringSplitOptions.RemoveEmptyEntries);
                    if(ni.Count() > 1)
                    {
                        long li = long.Parse(ni[0]);
                        long hi = long.Parse(ni[1]);
                        if (hi < nTotal) nTotal--;
                        else if (li >= nTotal) nTotal--;
                        else
                        {
                            People.Add(new Tuple<long, long>(li, hi));
                        }
                    }
                });

                QueuedConsole.WriteImmediate("People in list after first sort: {0}", People.Count);

                while (1 > 0)
                {
                    List<Tuple<long, long>> Sorted = new List<Tuple<long, long>>();
                    int nTotalBefore = nTotal;
                    foreach (Tuple<long, long> x in People)
                    {
                        long li = x.Item1;
                        long hi = x.Item2;
                        if (hi < nTotal) nTotal--;
                        else if (li >= nTotal) nTotal--;
                        else
                        {
                            Sorted.Add(new Tuple<long, long>(li, hi));
                        }
                    }
                    if (nTotal == nTotalBefore) break;
                    People = new List<Tuple<long, long>>(Sorted);
                }
                QueuedConsole.WriteImmediate("Maximum number of people: {0}", People.Count);
            }

            /// <summary>
            /// https://brilliant.org/problems/the-ultimate-primes/
            /// </summary>
            void TheUltimatePrimes()
            {
                int primeCounter = 0;
                BigInteger b = 1;
                int n = 0;
                ClonedPrimes p = KnownPrimes.CloneKnownPrimes();
                while (1 > 0)
                {
                    n++;
                    b *= 2;

                    BigInteger prime = b - 3;
                    if (Prime.IsPrime(prime, p))
                    {
                        if(prime < 99999999)
                        {
                            primeCounter++;
                            QueuedConsole.WriteImmediate("{0}th prime number: {1}", primeCounter, n);
                        }
                        else if (prime > 99999999 && prime.IsProbablePrime(1000))
                        {
                            primeCounter++;
                            QueuedConsole.WriteImmediate("{0}th prime number: {1}", primeCounter, n);
                        }
                    }
                    if(primeCounter == 27)
                    {
                        QueuedConsole.WriteImmediate("27th value of n : {0}", n);
                        break;
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/order-the-integers/
            /// </summary>
            void OrderTheIntegers()
            {
                int count = 0, curN = 0;
                Action<int, List<int>, int> Add = null;
                Add = delegate (int depth, List<int> used, int sum)
                {
                    for (int i = 1; i <= curN; i++)
                    {
                        if (used.Contains(i)) continue;
                        if (used.Count > 0) {
                            if (sum % i != 0) continue;
                        }

                        List<int> next = new List<int>(used);
                        next.Add(i);
                        int sumNext = sum + i;

                        if (depth == curN - 1) {
                            count++;
                        } else Add(depth + 1, next, sumNext);
                    }
                };
                Enumerable.Range(3, 19).ForEach(n =>
                {
                    count = 0;
                    curN = n;
                    Add(0, new List<int>(), 0);
                    QueuedConsole.WriteImmediate("f({0}): {1}", n, count);
                });
            }

            /// <summary>
            /// https://brilliant.org/problems/sorting-the-exponentials/
            /// </summary>
            void SortingTheExponentials()
            {
                string word = Http.Request.GetHtmlResponse("http://pastebin.com/raw/wPK0513n");
                string[] numbers = word.Splitter(StringSplitter.SplitUsing.Comma);

                List<Tuple<int, int, double>> N = new List<Tuple<int, int, double>>();
                numbers.ForEach(s =>
                {
                    string[] nArr = s.Splitter(StringSplitter.SplitUsing.PCap, StringSplitOptions.RemoveEmptyEntries);
                    int n1 = int.Parse(nArr[0]);
                    int n2 = int.Parse(nArr[1]);

                    double log = n2 * Math.Log10(n1);
                    N.Add(new Tuple<int, int, double>(n1, n2, log));
                });

                N.Sort((x, y) =>
                {
                    return x.Item3.CompareTo(y.Item3);
                });

                QueuedConsole.WriteImmediate("Sum exponents of even indices: {0}", (N.Where((item, index) => index % 2 == 0)).Sum(x => x.Item2));
            }

            /// <summary>
            /// https://brilliant.org/problems/its-been-a-long-day/
            /// </summary>
            void ItsBeenALongDay()
            {
                int n = 3;
                BigInteger N = 0, Nprev = 1;
                
                Func<int, BigInteger, bool> Add = null;
                Add = delegate (int depth, BigInteger b)
                {
                    if (depth == 0)
                    {
                        BigInteger s = (Nprev - 1);
                        while (1 > 0)
                        {
                            s++;
                            BigInteger next = (s * n) + 1;
                            if (Add(depth + 1, next)) break;
                        }
                        //QueuedConsole.WriteImmediate("n:{0}, s: {1}", n, s);
                        return true;
                    } else {
                        if (b % (n - 1) != 0) return false;
                        BigInteger next = (b * n / (n - 1)) + 1;

                        if (depth == n - 1) { N = next; return true; }
                        else return Add(depth + 1, next);
                    }
                };

                BigInteger S = 0;
                while (1 > 0)
                {
                    N = 0;
                    Add(0, 0);
                    if (n > 4) { S += N; }
                    Nprev = N;
                    //QueuedConsole.WriteImmediate("n: {0}, N: {1}", n, N);
                    if (++n > 1000) {
                        --n;
                        break;
                    }
                }

                QueuedConsole.WriteImmediate("n:{0}, Digitsum of S: {1}", n, MathFunctions.DigitSum(S));
            }

            /// <summary>
            /// https://brilliant.org/problems/cs-or-nt/
            /// </summary>
            void CsOrNot()
            {
                BigInteger sum = BigInteger.Parse("50653597");
                List<long> primes = KnownPrimes.GetClonedPrimes(0, 1000000);

                Action<int, int, HashSet<long>> CalculateM = delegate (int x, int nValue, HashSet<long> set)
                {
                    int nm = 1;
                    if (x <= 46340)
                    {
                        int b = x;
                        while (b != 1)
                        {
                            b *= x;
                            nm++;

                            b %= nValue;
                        }
                    } else {
                        long b = x;
                        while (b != 1)
                        {
                            b *= x;
                            nm++;

                            b %= nValue;
                        }
                    }
                    set.Add(nm);
                };

                Action<int, HashSet<long>> CalculateCoprimesAndM = delegate (int nValue, HashSet<long> set)
                {
                    if (primes.Contains(nValue)) {
                        Enumerable.Range(1, nValue - 1).ForEach(x => { CalculateM(x, nValue, set); });
                    } else {
                        int a = 1;
                        List<long> nPrimes = Factors.GetPrimeFactors(nValue, primes);
                        while (++a <= nValue)
                        {
                            if (nValue % a == 0) continue;

                            bool isCoprime = true;
                            foreach (long p in nPrimes)
                            {
                                if (a % p == 0) { isCoprime = false; break; }
                                if (p > a) { break; }
                            }
                            if (isCoprime) { CalculateM(a, nValue, set); }
                        }
                    }
                };

                int n = 19698;
                while (++n <= 1000000)
                {
                    if (n > 1)
                    {
                        HashSet<long> set = new HashSet<long>();

                        CalculateCoprimesAndM(n, set);

                        long lcm = MathFunctions.LCM(set.ToArray());
                        sum += lcm;
                        if (n % 1000 == 0)
                        {
                            QueuedConsole.WriteImmediate("n:{0}, sum so far: {1}", n, sum);
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Sum: {0}", sum);
            }

            /// <summary>
            /// https://brilliant.org/problems/n-queens-reloaded/
            /// </summary>
            void NQueens()
            {
                int nWays = 0;
                int N = 12;

                Dictionary<string, List<Tuple<int, int>>> ThreatenedSquares = new Dictionary<string, List<Tuple<int, int>>>();
                for (int row = 0; row < N; row++)
                {
                    for (int col = 0; col < N; col++)
                    {
                        int i = row, j = col;
                        string key = i + ":" + j;
                        ThreatenedSquares.Add(key, new List<Tuple<int, int>>());
                        List<Tuple<int, int>> list = ThreatenedSquares[key];
                        while (true)
                        {
                            i += 1; j -= 1;
                            if (j < 0 || i == N || j == N) { break; }

                            list.Add(new Tuple<int, int>(i, j));
                        }
                        i = row; j = col;
                        while (true)
                        {
                            i += 1; j += 1;
                            if (i == N || j == N) { break; }

                            list.Add(new Tuple<int, int>(i, j));
                        }
                        i = row; j = col;
                        while (true)
                        {
                            i -= 1; j += 1;
                            if (i < 0 || i == N || j == N) { break; }

                            list.Add(new Tuple<int, int>(i, j));
                        }
                        i = row; j = col;
                        while (true)
                        {
                            i -= 1; j -= 1;
                            if (i < 0 || j < 0 || i == N || j == N) { break; }

                            list.Add(new Tuple<int, int>(i, j));
                        }
                        for (int a = -2; a <= 2; a++)
                        {
                            if (a == 0) continue;
                            for (int b = -2; b <= 2; b++)
                            {
                                if (b == 0) continue;

                                i = row + a; j = col + b;

                                if (i < 0 || i >= N || j < 0 || j >= N) continue;
                                list.Add(new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }

                Func<int, int, int[,], int[,]> GetFreeSquares = delegate (int row, int col, int[,] squares)
                {
                    int[,] retArr = (int[,])squares.Clone();
                    for (int i = 0; i < N; i++)
                    {
                        if (retArr[row, i] == 0) retArr[row, i] = -1;
                        if (retArr[i, col] == 0) retArr[i, col] = -1;

                        ThreatenedSquares[row + ":" + col].ForEach(s => { retArr[s.Item1, s.Item2] = -1; });
                    }

                    return retArr;
                };

                Action<int, int[,]> PlaceQueen = null;
                PlaceQueen = delegate (int depth, int[,] freesquares)
                {
                    int row = depth;
                    for (int i = 0; i < N; i++)
                    {
                        if (freesquares[row, i] != 0) continue;

                        int[,] next = GetFreeSquares(row, i, freesquares);
                        next[row, i] = 1;
                        if (depth == N - 1)
                        {
                            nWays++;
                            continue;
                        } else { PlaceQueen(depth + 1, next); }
                    }
                };

                PlaceQueen(0, new int[N, N]);
                QueuedConsole.WriteImmediate("Number of ways: {0}", nWays);
            }

            /// <summary>
            /// https://brilliant.org/problems/sum-of-powers-4/
            /// </summary>
            [SearchKeyword("Incorrect")]
            void SumOfPowers()
            {
                List<IList<int>> c = Combinations.GetAllCombinations(Enumerable.Range(1, 30).ToList(), 5);
                List<string> n5 = new List<string>();
                c.ForEach(x =>
                {
                    BigInteger b = 0;
                    x.ForEach(y =>
                    {
                        b += BigInteger.Pow(y, 5);
                    });
                    n5.Add(b.ToString());
                });

                c.Clear();

                c = Combinations.GetAllCombinations(Enumerable.Range(1, 60).ToList(), 4);
                List<string> n4 = new List<string>();
                c.ForEach(x =>
                {
                    BigInteger b = 0;
                    x.ForEach(y =>
                    {
                        b += BigInteger.Pow(y, 4);
                    });
                    n4.Add(b.ToString());
                });

                c.Clear();

                c = Combinations.GetAllCombinations(Enumerable.Range(1, 100).ToList(), 3);
                List<string> n3 = new List<string>();
                c.ForEach(x =>
                {
                    BigInteger b = 0;
                    x.ForEach(y =>
                    {
                        b += BigInteger.Pow(y, 3);
                    });
                    n3.Add(b.ToString());
                });

                c.Clear();

                c = Combinations.GetAllCombinations(Enumerable.Range(1, 200).ToList(), 2);
                List<string> n2 = new List<string>();
                c.ForEach(x =>
                {
                    BigInteger b = 0;
                    x.ForEach(y =>
                    {
                        b += BigInteger.Pow(y, 2);
                    });
                    n2.Add(b.ToString());
                });

                foreach (string x in n5)
                {
                    if (n2.Contains(x) && n3.Contains(x) && n4.Contains(x))
                    {
                        QueuedConsole.WriteImmediate("Possible n : {0}", x);
                    }
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/primes-12/
            /// </summary>
            void Primes12()
            {
                int largest = 0;
                List<long> primes = KnownPrimes.GetPrimes(100000, 1000000);
                primes.ForEach(x =>
                {
                    string s = "" + x;
                    if (s.IndexOfAny(new char[] { '0', '2', '4', '6', '8' }) < 0)
                    {
                        bool isValid = true;
                        for (int i = 0; i < 5; i++)
                        {
                            s = s.Substring(1, 5) + s.Substring(0, 1);
                            if (!primes.Contains(int.Parse(s))) { isValid = false; break; }
                        }
                        if (isValid) largest = Math.Max(largest, (int)x);
                    }
                });
                QueuedConsole.WriteImmediate("Largest: {0}", largest);
            }

            /// <summary>
            /// https://brilliant.org/problems/fractal-exploration/
            /// </summary>
            void FractalExploration()
            {
                QueuedConsole.WriteImmediate("{0}", MathFunctions.DigitSum(TailRecursion.Execute(() => PermutationsAndCombinations.Factorial(512, 1))));

                BigInteger b = BigInteger.Parse("3892643213082624");

                List<long> primes = KnownPrimes.GetAllKnownFirstNPrimes(1000);
                primes.Sort();

                Dictionary<int, int> map = new Dictionary<int, int>();
                primes.ForEach(x =>
                {
                    while (b % x == 0)
                    {
                        b /= x;
                        map.AddOrUpdate((int)x, 1);
                    }
                });
                map.ForEach(x =>
                {
                    QueuedConsole.WriteImmediate("Factor: {0} ^ {1}", x.Key, x.Value);
                });
            }

            /// <summary>
            /// https://brilliant.org/problems/avoid-the-snails/
            /// </summary>
            void AvoidTheSnails()
            {
                int N = 1000;
                int[,] Input = new int[N, N];
                using (var stream = Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.snail-input.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        int row = -1;
                        while ((s = sr.ReadLine()) != null)
                        {
                            row++;
                            string[] numbers = s.Splitter(StringSplitter.SplitUsing.Space);
                            Enumerable.Range(0, N).ForEach(y =>
                            {
                                Input[row, y] = int.Parse(numbers[y]);
                            });
                        }
                    }
                }

                int[,] Snails = new int[N, N];

                Enumerable.Range(0, N).ForEach(x =>
                {
                    Enumerable.Range(0, N).ForEach(y =>
                    {
                        Snails[N - x - 1, y] = Input[x, y];
                    });
                });

                int minCost = 0;
                List<Tuple<int, int>> Path = MinCostPath.FindCostAndPath(Snails, ref minCost);
                QueuedConsole.WriteImmediate("The minimum number of snails : {0}", minCost);
            }
        }

        //Keep this lowest in view.
        string IProblemName.GetName()
        {
            return "Brilliant ComputerScience Collection 1";
        }
    }
}