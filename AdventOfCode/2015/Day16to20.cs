using GenericDefs.Classes;
using GenericDefs.DotNet;
using GenericDefs.Functions;
using GenericDefs.Functions.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day16to20
    {
        public static void Solve() { Day20.Part2(); }

        internal class Day16
        {
            static void Init()
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day16.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            int sInd = s.IndexOf(":");
                            string aName = s.Substring(0, sInd);
                            string[] gifts = (s.Substring(sInd + 1, s.Length - sInd - 1)).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            Dictionary<string, int> aGifts = new Dictionary<string, int>();
                            foreach(string g in gifts)
                            {
                                string[] gift = g.Replace(" ","").Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                aGifts.Add(gift[0], int.Parse(gift[1]));

                                if (TotalGifts.ContainsKey(gift[0])) { TotalGifts[gift[0]] += int.Parse(gift[1]); }
                                else { TotalGifts.Add(gift[0], int.Parse(gift[1])); }
                            }
                            Aunts.Add(aName, aGifts);
                        }
                    }
                }

                mfcsamInput.Add("children", 3);
                mfcsamInput.Add("cats", 7);
                mfcsamInput.Add("samoyeds", 2);
                mfcsamInput.Add("pomeranians", 3);
                mfcsamInput.Add("akitas", 0);
                mfcsamInput.Add("vizslas", 0);
                mfcsamInput.Add("goldfish", 5);
                mfcsamInput.Add("trees", 3);
                mfcsamInput.Add("cars", 2);
                mfcsamInput.Add("perfumes", 1);
            }

            static Dictionary<string, int> mfcsamInput = new Dictionary<string, int>();
            static Dictionary<string, Dictionary<string, int>> Aunts = new Dictionary<string, Dictionary<string, int>>();
            static Dictionary<string, int> TotalGifts = new Dictionary<string, int>();
            internal static void Part1() {
                Init();

                foreach (KeyValuePair<string, Dictionary<string, int>> kvp1 in Aunts)
                {
                    int count = 0;
                    foreach (KeyValuePair<string, int> kvp2 in kvp1.Value)
                    {
                        foreach (KeyValuePair<string, int> kvp3 in mfcsamInput)
                        {
                            if (kvp2.Key == kvp3.Key) { if (kvp2.Value == kvp3.Value) count++; break; }
                        }
                    }
                    if(count == 3) { QueuedConsole.WriteImmediate("Possible aunt sue : {0}", kvp1.Key); }
                }
            }

            internal static void Part2()
            {
                Init();

                foreach (KeyValuePair<string, Dictionary<string, int>> kvp1 in Aunts)
                {
                    int count = 0;
                    foreach (KeyValuePair<string, int> kvp2 in kvp1.Value)
                    {
                        foreach (KeyValuePair<string, int> kvp3 in mfcsamInput)
                        {
                            if (kvp2.Key == kvp3.Key) {
                                if (kvp2.Key == "cats" || kvp2.Key == "trees") {
                                    if (kvp2.Value > kvp3.Value) count++; break;
                                }
                                else if (kvp2.Key == "pomeranians" || kvp2.Key == "goldfish") {
                                    if (kvp2.Value < kvp3.Value) count++; break;
                                }
                                else { if (kvp2.Value == kvp3.Value) count++; break; }
                            }
                        }
                    }
                    if(count >= 3) { QueuedConsole.WriteImmediate("Possible aunt sue : {0}", kvp1.Key); }
                }
            }
        }

        internal class Day17
        {
            static List<int> Containers = new List<int>();
            static void Init()
            {
                Containers.Add(43);
                Containers.Add(3);
                Containers.Add(4);
                Containers.Add(10);
                Containers.Add(21);
                Containers.Add(44);
                Containers.Add(4);
                Containers.Add(6);
                Containers.Add(47);
                Containers.Add(41);
                Containers.Add(34);
                Containers.Add(17);
                Containers.Add(17);
                Containers.Add(44);
                Containers.Add(36);
                Containers.Add(31);
                Containers.Add(46);
                Containers.Add(9);
                Containers.Add(27);
                Containers.Add(38);
            }

            internal static void Part1()
            {
                Init();
                IEnumerable<IEnumerable<int>> subsets = PowerSet.Subsets(Containers);
                UniqueIntegralPairs uip = new UniqueIntegralPairs();
                int c1 = 0, c2 = 0;
                foreach(IEnumerable<int> set in subsets)
                {
                    c1++;
                    List<int> s = set.ToList();
                    if(s.Sum(x=>x) == 150)
                    {
                        c2++;
                        //s.Sort();
                        uip.AddCombination(s.ToArray());
                    }
                }

                QueuedConsole.WriteImmediate("Total possible combinations of containers : {0}", c1);
                QueuedConsole.WriteImmediate("Different combinations of containers : {0}", c2);
                QueuedConsole.WriteImmediate("Unique Arrangements of containers : {0}", uip.Count());
            }

            internal static void Part2()
            {
                Init();
                IEnumerable<IEnumerable<int>> subsets = PowerSet.Subsets(Containers);
                Dictionary<int, int> dContainers = new Dictionary<int, int>();
                int c1 = 0, c2 = 0;
                foreach (IEnumerable<int> set in subsets)
                {
                    c1++;
                    List<int> s = set.ToList();
                    if (s.Sum(x => x) == 150)
                    {
                        c2++;
                        if (dContainers.ContainsKey(s.Count)) dContainers[s.Count] += 1;
                        else dContainers.Add(s.Count, 1);
                    }
                }

                IOrderedEnumerable<KeyValuePair<int, int>> orderedContainers = dContainers.OrderBy(x => x.Key);

                QueuedConsole.WriteImmediate("Total possible combinations of containers : {0}", c1);
                QueuedConsole.WriteImmediate("Different combinations of containers : {0}", c2);
                QueuedConsole.WriteImmediate("Minimum number of containers is : {0}, and possible combinations with this minimum is : {1}", 
                    orderedContainers.First().Key, orderedContainers.First().Value);
            }
        }

        internal class Day18
        {
            internal class Light
            {
                internal Light(int x, int y) { X = x; Y = y; }
                internal int X;
                internal int Y;
                /// <summary>
                /// Current state.
                /// </summary>
                internal bool IsSwitchedOn { get; set; }
                internal void Switch(bool turnOn)
                {
                    IsSwitchedOn = turnOn;
                }
                internal void Toggle()
                {
                    IsSwitchedOn = !IsSwitchedOn;
                }

                private bool? _nextState = null;
                internal void SetNextState(bool state)
                {
                    _nextState = state;
                }

                internal void UpdatePendingChanges()
                {
                    if (_nextState.HasValue) {
                        IsSwitchedOn = _nextState.Value;
                        _nextState = null;
                    }
                }

                internal static string GetKey(int x, int y)
                {
                    return x + "#" + y;
                }
            }

            static void Init()
            {
                for (int i = 1; i <= 100; i++)
                {
                    for (int j = 1; j <= 100; j++)
                    {
                        Lights.Add(Light.GetKey(i,j), new Light(i, j));
                    }
                }

                for (int i = 1; i <= 100; i++)
                {
                    for (int j = 1; j <= 100; j++)
                    {
                        List<string> neighbours = new List<string>();
                        if (Lights.ContainsKey(Light.GetKey(i - 1, j - 1))) neighbours.Add(Light.GetKey(i - 1, j - 1));
                        if (Lights.ContainsKey(Light.GetKey(i, j - 1))) neighbours.Add(Light.GetKey(i, j - 1));
                        if (Lights.ContainsKey(Light.GetKey(i + 1, j - 1))) neighbours.Add(Light.GetKey(i + 1, j - 1));
                        if (Lights.ContainsKey(Light.GetKey(i - 1, j))) neighbours.Add(Light.GetKey(i - 1, j));
                        if (Lights.ContainsKey(Light.GetKey(i + 1, j))) neighbours.Add(Light.GetKey(i + 1, j));
                        if (Lights.ContainsKey(Light.GetKey(i - 1, j + 1))) neighbours.Add(Light.GetKey(i - 1, j + 1));
                        if (Lights.ContainsKey(Light.GetKey(i, j + 1))) neighbours.Add(Light.GetKey(i, j + 1));
                        if (Lights.ContainsKey(Light.GetKey(i + 1, j + 1))) neighbours.Add(Light.GetKey(i + 1, j + 1));
                        Neighbours.Add(Light.GetKey(i, j), neighbours);
                    }
                }

                string s = string.Empty;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day18.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        int row = 0;
                        while ((s = sr.ReadLine()) != null)
                        {
                            row++;
                            int col = 0;
                            foreach (char c in s)
                            {
                                col++;
                                if (c == '#') { Lights[Light.GetKey(row, col)].Switch(true); }
                                if (c == '.') { Lights[Light.GetKey(row, col)].Switch(false); }
                            }
                        }
                    }
                }
            }

            static Dictionary<string, List<string>> Neighbours = new Dictionary<string, List<string>>();
            static Dictionary<string, Light> Lights = new Dictionary<string, Light>();
            internal static void Solve()
            {
                Init();
                Part2();
            }

            internal static void Part1()
            {
                int run = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    foreach(KeyValuePair<string, Light> kvp in Lights)
                    {
                        int nCount = 0;
                        foreach(string s in Neighbours[kvp.Key])
                        {
                            nCount += Lights[s].IsSwitchedOn ? 1 : 0;
                        }
                        if (kvp.Value.IsSwitchedOn) {
                            if (nCount == 2 || nCount == 3) { kvp.Value.SetNextState(true); }
                            else { kvp.Value.SetNextState(false); }
                        } else {
                            if (nCount == 3) { kvp.Value.SetNextState(true); }
                            else { kvp.Value.SetNextState(false); }
                        }
                    }

                    foreach (KeyValuePair<string, Light> kvp in Lights)
                    {
                        kvp.Value.UpdatePendingChanges();
                    }
                    run++;
                    QueuedConsole.WriteImmediate("Run finished : {0}, Elapsed Time : {1} seconds", run, sw.ElapsedMilliseconds * 1.0 / 1000);
                    if (run == 100) break;
                }

                sw.Stop();
                QueuedConsole.WriteImmediate("Number of lights on after 100 steps : {0} ", Lights.Count(x => x.Value.IsSwitchedOn));
            }
            
            internal static void Part2()
            {
                int run = 0;
                HashSet<string> corners = new HashSet<string>();
                corners.Add(Light.GetKey(1, 1));
                corners.Add(Light.GetKey(1, 100));
                corners.Add(Light.GetKey(100, 1));
                corners.Add(Light.GetKey(100, 100));

                foreach(string s in corners)
                {
                    Lights[s].Switch(true);
                }

                while (true)
                {
                    foreach (KeyValuePair<string, Light> kvp in Lights)
                    {
                        if (corners.Contains(kvp.Key)) continue;
                        int nCount = 0;
                        foreach (string s in Neighbours[kvp.Key])
                        {
                            nCount += Lights[s].IsSwitchedOn ? 1 : 0;
                        }
                        if (kvp.Value.IsSwitchedOn)
                        {
                            if (nCount == 2 || nCount == 3) { kvp.Value.SetNextState(true); }
                            else { kvp.Value.SetNextState(false); }
                        }
                        else
                        {
                            if (nCount == 3) { kvp.Value.SetNextState(true); }
                            else { kvp.Value.SetNextState(false); }
                        }
                    }

                    foreach (KeyValuePair<string, Light> kvp in Lights)
                    {
                        kvp.Value.UpdatePendingChanges();
                    }
                    run++;
                    QueuedConsole.WriteImmediate("Run finished : {0}", run);
                    if (run == 100) break;
                }
                
                QueuedConsole.WriteImmediate("Number of lights on after 100 steps : {0} ", Lights.Count(x => x.Value.IsSwitchedOn));
            }
        }

        internal class Day19
        {
            internal class Conversion
            {
                internal string From { get; set; }
                internal string To { get; set; }
                internal Conversion Reverse()
                {
                    return new Conversion() { From = To, To = From };
                }

                private int? _diff = null;
                internal int ToFromDiff() {
                    if (!_diff.HasValue) _diff = To.Length - From.Length;
                    return _diff.Value;
                }
            }
            static List<Conversion> Conversions = new List<Conversion>();
            static string Chemical = string.Empty;
            static void Init()
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day19.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            if(s.IndexOf("=>") >= 0) {
                                string[] c = s.Replace(" ","").Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                                Conversions.Add(new Conversion() { From = c[0], To = c[1] });
                            } else if (!string.IsNullOrEmpty(s)) {
                                Chemical = s;
                            }
                        }
                    }
                }
            }

            internal static void Solve()
            {
                Init();
                Part2();
            }

            internal static void Part1()
            {
                HashSet<string> NewChemicals = new HashSet<string>();
                QueuedConsole.WriteImmediate("Chemical length : {0}", Chemical.Length);
                int found = 0, notfound = 0, repeats = 0;
                foreach (Conversion c in Conversions)
                {
                    string chemical = "" + Chemical;
                    if(chemical.IndexOf(c.From, 0, StringComparison.Ordinal) >= 0) {
                        string[] splits = chemical.Split(new string[] { c.From }, StringSplitOptions.None);
                        QueuedConsole.WriteImmediate("From : {0}, To: {1}, Expected new chemical length : {2}", c.From, c.To, Chemical.Length + (c.To.Length - c.From.Length));
                        int n = 0;
                        while (true)
                        {
                            string newChemical = string.Empty;
                            for(int i=0; i< splits.Length; i++) {
                                if (i == n) newChemical += splits[i] + c.To;
                                else {
                                    newChemical += splits[i];
                                    if (i < splits.Length - 1) newChemical += c.From;
                                }
                            }
                            if (!NewChemicals.Add(newChemical)) repeats++;
                            n++;
                            if (n == splits.Length - 1) break;
                        }
                        found++;
                    } else {
                        notfound++;
                        QueuedConsole.WriteImmediate("Not found in chemical : {0}", c.From);
                    }
                }
                QueuedConsole.WriteImmediate("New chemicals formed : {0}", NewChemicals.Count);
                QueuedConsole.WriteImmediate("Total conversions : {0}", Conversions.Count);
                QueuedConsole.WriteImmediate("Total conversions made for : {0} Conversions", found);
                QueuedConsole.WriteImmediate("Total conversions made for : {0} Conversions", notfound);
                QueuedConsole.WriteImmediate("Total repeats found for : {0} Conversions", repeats);
            }

            internal static void Part2()
            {
                int count = 0, shuffleCount = 0;
                int minLastLength = int.MaxValue;
                while (true) {
                    count = 0;
                    string chemical = new string(Chemical.ToCharArray());
                    List<Conversion> shuffle = ShuffleCollections.Shuffle(Conversions);
                    shuffleCount++;
                    count = RecursiveElimination(ref chemical, shuffle, count, 1, 5);

                    if (chemical.Length < minLastLength) {
                        minLastLength = chemical.Length;
                        //QueuedConsole.WriteImmediate("Shuffle result. Min. last length : {0}, Replacements required : {1}", minLastLength, count);
                    }
                    if (shuffleCount % 10000 == 0) QueuedConsole.WriteImmediate("Shuffle count : {0}", shuffleCount);
                    if (chemical == "e") {
                        QueuedConsole.WriteImmediate("Replacements required : {0}, Shuffles required : {1}", count, shuffleCount);
                    }
                }
            }

            internal static int RecursiveElimination(ref string chemical, List<Conversion> shuffle, int count, int depth, int maxDepth)
            {
                foreach (Conversion c in shuffle)
                {
                    if (chemical.IndexOf(c.To) >= 0)
                    {
                        chemical = Regex.Replace(chemical, c.To,
                        (match) => {
                            count++;
                            return match.Result(c.From);
                        });
                    }
                }
                if (depth < maxDepth) return RecursiveElimination(ref chemical, shuffle, count, depth + 1, maxDepth);
                else return count;
            }
        }

        internal class Day20
        {
            static int Input = 29000000;
            internal static void Part1()
            {
                int houseNum = 1;
                long maxGifts = 0;
                ClonedPrimes cPrimes = KnownPrimes.CloneKnownPrimes(1, 1000000);
                while (true)
                {
                    long giftCount = 10 * Factors.GetAllFactorsSum(houseNum, cPrimes);
                    maxGifts = Math.Max(giftCount, maxGifts);

                    if (maxGifts % 100 == 0) QueuedConsole.WriteImmediate("Max Gifts : {0}, House count : {1}", maxGifts, houseNum);

                    if (giftCount >= Input) break;
                    else houseNum++;
                }

                QueuedConsole.WriteImmediate("House number : {0}", houseNum);
            }

            internal static void Part2()
            {
                long houseNum = 1;
                long maxGifts = 0, prevMaxGifts = 0;
                ClonedPrimes cPrimes = KnownPrimes.CloneKnownPrimes(1, 1000000);
                while (true)
                {
                    HashSet<long> factors = Factors.GetAllFactors(houseNum, cPrimes);

                    long giftCount = 11 * factors.Where(x => (x * 50 >= houseNum)).Sum();
                    maxGifts = Math.Max(giftCount, maxGifts);

                    if (maxGifts - prevMaxGifts >= 1000) {
                        QueuedConsole.WriteImmediate("Max Gifts : {0}, House count : {1}", maxGifts, houseNum);
                        prevMaxGifts = maxGifts;
                    }

                    if (giftCount >= Input) break;
                    else houseNum++;
                }

                QueuedConsole.WriteImmediate("House number : {0}", houseNum);
            }
        }
    }
}