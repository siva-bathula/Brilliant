using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs;
using GenericDefs.Classes;
using GenericDefs.DotNet;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016
{
    public class Day6to10
    {
        public static void Solve()
        {
            Day7.Run();
        }

        internal class Day6
        {
            internal static void Run() { RunTwice(); }

            internal static void RunOnce()
            {
                Dictionary<int, Dictionary<char, int>> Positions = new Dictionary<int, Dictionary<char, int>>();
                Dictionary<int, char> MostFrequentlyAppeared = new Dictionary<int, char>();
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day6.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        string[] separator = { "  " };
                        while ((s = sr.ReadLine()) != null)
                        {
                            int index = 0;
                            foreach (char ch in s)
                            {
                                if (Positions.ContainsKey(index))
                                {
                                    if (Positions[index].ContainsKey(ch)) Positions[index][ch] += 1;
                                    else Positions[index].Add(ch, 1);
                                }
                                else
                                {
                                    Positions.Add(index, new Dictionary<char, int>());
                                    Positions[index].Add(ch, 1);
                                }
                                index++;
                            }
                        }

                        foreach (KeyValuePair<int, Dictionary<char, int>> kvp in Positions)
                        {
                            List<KeyValuePair<char,int>> items = kvp.Value.OrderByDescending(p => p.Value).ThenBy(p => p.Key).Take(1).ToList();
                            MostFrequentlyAppeared.Add(kvp.Key, items[0].Key);
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Message : ");
                foreach (KeyValuePair<int, char> kvp in MostFrequentlyAppeared)
                {
                    QueuedConsole.WriteImmediate("{0} : {1}", kvp.Key, kvp.Value);
                }
            }

            internal static void RunTwice()
            {
                Dictionary<int, Dictionary<char, int>> Positions = new Dictionary<int, Dictionary<char, int>>();
                Dictionary<int, char> MostFrequentlyAppeared = new Dictionary<int, char>();
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day6.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        string[] separator = { "  " };
                        while ((s = sr.ReadLine()) != null)
                        {
                            int index = 0;
                            foreach (char ch in s)
                            {
                                if (Positions.ContainsKey(index))
                                {
                                    if (Positions[index].ContainsKey(ch)) Positions[index][ch] += 1;
                                    else Positions[index].Add(ch, 1);
                                }
                                else
                                {
                                    Positions.Add(index, new Dictionary<char, int>());
                                    Positions[index].Add(ch, 1);
                                }
                                index++;
                            }
                        }

                        foreach (KeyValuePair<int, Dictionary<char, int>> kvp in Positions)
                        {
                            List<KeyValuePair<char, int>> items = kvp.Value.OrderBy(p => p.Value).ThenBy(p => p.Key).Take(1).ToList();
                            MostFrequentlyAppeared.Add(kvp.Key, items[0].Key);
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Message : ");
                foreach (KeyValuePair<int, char> kvp in MostFrequentlyAppeared)
                {
                    QueuedConsole.WriteImmediate("{0} : {1}", kvp.Key, kvp.Value);
                }
            }
        }

        internal class Day7
        {
            internal static void Run() { RunOnce(); }

            internal static void RunOnce()
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day7.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        string[] separator = { "  " };
                        List<string> data = new List<string>();
                        while ((s = sr.ReadLine()) != null)
                        {
                            data.Add(s);
                        }
                        Solve(data.ToArray(), false);
                    }
                }
                QueuedConsole.WriteImmediate("Message : ");
            }

            internal static bool SupportTLS(string ip)
            {
                string[] outside = new string[] { };
                string[] inside = new string[] { };
                SplitIp(ip, ref inside, ref outside);
                return true;
            }

            internal static bool SupportSSL(string ip)
            {
                return true;
            }

            internal static int Solve(string[] data, bool ssl = false) {
                if (!ssl) {
                    foreach (string ip in data) {
                        SupportTLS(ip);
                    }
                }
                else {
                    foreach (string ip in data)
                    {
                        SupportSSL(ip);
                    }
                }
                return 0;
            }
            internal static void SplitIp(string ip, ref string[] inside, ref string[] outside) {
                // Split based on []
                string[] split = Regex.Split(ip, @"\[|\]");
                // Divide into inside & outside []
                outside = split.Skip(1).ToArray();
                inside = split.Skip(2).ToArray();
            }
            internal static void RunTwice()
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day7.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        string[] separator = { "  " };
                        while ((s = sr.ReadLine()) != null)
                        {

                        }
                    }
                }
                QueuedConsole.WriteImmediate("Message : ");
            }
        }
    }
}