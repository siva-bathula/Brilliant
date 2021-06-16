using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs;
using GenericDefs.Classes;
using GenericDefs.DotNet;
using System.IO;

namespace AdventOfCode._2016
{
    public class Day1to5
    {
        public static void Solve()
        {
            Day5.Run();
        }

        internal class Day1
        {
            internal static void Run()
            {
                VisitTwice();
            }

            internal static void FinalBlock()
            {
                int xCor = 0, yCor = 0;
                int dir = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day1.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] instructions = s.Split(new char[] { ',' });
                            foreach (string ins in instructions)
                            {
                                string instruction = ins.Trim();
                                int l = int.Parse(instruction.Substring(1, instruction.Length - 1));
                                if (ins[1] == 'R')
                                {
                                    if (dir == 3) { dir = 0; }
                                    else { dir += 1; }
                                }
                                else if (ins[1] == 'L')
                                {
                                    if (dir == 0) { dir = 3; }
                                    else { dir -= 1; }
                                }

                                if (dir == 0)
                                {
                                    yCor += l;
                                }
                                else if (dir == 1) { xCor += l; }
                                else if (dir == 2) { yCor -= l; }
                                else if (dir == 3) { xCor -= l; }

                                string location = xCor + ":" + yCor;
                                if (Locations.Contains(location))
                                {

                                }
                            }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of blocks: {0}", Math.Abs(xCor) + Math.Abs(yCor));
            }

            static HashSet<string> Locations = new HashSet<string>();
            internal static void VisitTwice()
            {
                int xCor = 0, yCor = 0;
                int dir = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day1.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] instructions = s.Split(new char[] { ',' });
                            foreach (string ins in instructions)
                            {
                                string instruction = ins.Trim();
                                int l = int.Parse(instruction.Substring(1, instruction.Length - 1));
                                if (ins[1] == 'R')
                                {
                                    if (dir == 3) { dir = 0; }
                                    else { dir += 1; }
                                }
                                else if (ins[1] == 'L')
                                {
                                    if (dir == 0) { dir = 3; }
                                    else { dir -= 1; }
                                }
                                bool found = false;
                                if (dir == 0)
                                {
                                    for (int y = yCor + 1; y <= yCor + l; y++)
                                    {
                                        string loc = xCor + ":" + y;
                                        if (!Locations.Contains(loc)) Locations.Add(loc);
                                        else
                                        {
                                            found = true;
                                            QueuedConsole.WriteImmediate("Repeated location distance: {0}", Math.Abs(xCor) + Math.Abs(y));
                                        }
                                    }
                                    yCor += l;
                                }
                                else if (dir == 1) {
                                    for (int x = xCor + 1; x <= xCor + l; x++)
                                    {
                                        string loc = x + ":" + yCor;
                                        if (!Locations.Contains(loc)) Locations.Add(loc);
                                        else
                                        {
                                            found = true;
                                            QueuedConsole.WriteImmediate("Repeated location distance: {0}", Math.Abs(x) + Math.Abs(yCor));
                                        }
                                    }
                                    xCor += l;
                                }
                                else if (dir == 2) {
                                    for (int y = yCor - 1; y >= yCor - l; y--)
                                    {
                                        string loc = xCor + ":" + y;
                                        if (!Locations.Contains(loc)) Locations.Add(loc);
                                        else
                                        {
                                            found = true;
                                            QueuedConsole.WriteImmediate("Repeated location distance: {0}", Math.Abs(xCor) + Math.Abs(y));
                                        }
                                    }
                                    yCor -= l;
                                }
                                else if (dir == 3) {
                                    for (int x = xCor - 1; x >= xCor - l; x--)
                                    {
                                        string loc = x + ":" + yCor;
                                        if (!Locations.Contains(loc)) Locations.Add(loc);
                                        else
                                        {
                                            found = true;
                                            QueuedConsole.WriteImmediate("Repeated location distance: {0}", Math.Abs(x) + Math.Abs(yCor));
                                        }
                                    }
                                    xCor -= l;
                                }

                                if (found) break;
                            }
                        }
                    }
                }
            }
        }

        internal class Day2
        {
            internal class Keypad
            {
                public int ActiveKey = 5;
                public void Move(char instruction)
                {
                    if (ActiveKey == 1)
                    {
                        if (instruction == 'R') ActiveKey = 2;
                        if (instruction == 'L') ActiveKey = 1;
                        if (instruction == 'U') ActiveKey = 1;
                        if (instruction == 'D') ActiveKey = 4;
                    }
                    else if (ActiveKey == 2)
                    {
                        if (instruction == 'R') ActiveKey = 3;
                        if (instruction == 'L') ActiveKey = 1;
                        if (instruction == 'U') ActiveKey = 2;
                        if (instruction == 'D') ActiveKey = 5;
                    }
                    else if (ActiveKey == 3)
                    {
                        if (instruction == 'R') ActiveKey = 3;
                        if (instruction == 'L') ActiveKey = 2;
                        if (instruction == 'U') ActiveKey = 3;
                        if (instruction == 'D') ActiveKey = 6;
                    }
                    else if (ActiveKey == 4)
                    {
                        if (instruction == 'R') ActiveKey = 5;
                        if (instruction == 'L') ActiveKey = 4;
                        if (instruction == 'U') ActiveKey = 1;
                        if (instruction == 'D') ActiveKey = 7;
                    }
                    else if (ActiveKey == 5)
                    {
                        if (instruction == 'R') ActiveKey = 6;
                        if (instruction == 'L') ActiveKey = 4;
                        if (instruction == 'U') ActiveKey = 2;
                        if (instruction == 'D') ActiveKey = 8;
                    }
                    else if (ActiveKey == 6)
                    {
                        if (instruction == 'R') ActiveKey = 6;
                        if (instruction == 'L') ActiveKey = 5;
                        if (instruction == 'U') ActiveKey = 3;
                        if (instruction == 'D') ActiveKey = 9;
                    }
                    else if (ActiveKey == 7)
                    {
                        if (instruction == 'R') ActiveKey = 8;
                        if (instruction == 'L') ActiveKey = 7;
                        if (instruction == 'U') ActiveKey = 4;
                        if (instruction == 'D') ActiveKey = 7;
                    }
                    else if (ActiveKey == 8)
                    {
                        if (instruction == 'R') ActiveKey = 9;
                        if (instruction == 'L') ActiveKey = 7;
                        if (instruction == 'U') ActiveKey = 5;
                        if (instruction == 'D') ActiveKey = 8;
                    }
                    else if (ActiveKey == 9)
                    {
                        if (instruction == 'R') ActiveKey = 9;
                        if (instruction == 'L') ActiveKey = 8;
                        if (instruction == 'U') ActiveKey = 6;
                        if (instruction == 'D') ActiveKey = 9;
                    }
                }
            }

            internal static void Run()
            {
                RunTwice();
            }

            internal static void RunOnce()
            {
                string code = "";
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day2.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        Keypad kp = new Keypad();
                        while (!sr.EndOfStream)
                        {
                            s = sr.ReadLine();
                            foreach(char ins in s)
                            {
                                kp.Move(ins);
                            }
                            code += kp.ActiveKey;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Code : {0}", code);
            }

            internal class Keypad2
            {
                public int ActiveKey = 5;
                public void Move(char instruction)
                {
                    if (ActiveKey == 1)
                    {
                        if (instruction == 'R') ActiveKey = 1;
                        if (instruction == 'L') ActiveKey = 1;
                        if (instruction == 'U') ActiveKey = 1;
                        if (instruction == 'D') ActiveKey = 3;
                    }
                    else if (ActiveKey == 2)
                    {
                        if (instruction == 'R') ActiveKey = 3;
                        if (instruction == 'L') ActiveKey = 2;
                        if (instruction == 'U') ActiveKey = 2;
                        if (instruction == 'D') ActiveKey = 6;
                    }
                    else if (ActiveKey == 3)
                    {
                        if (instruction == 'R') ActiveKey = 4;
                        if (instruction == 'L') ActiveKey = 2;
                        if (instruction == 'U') ActiveKey = 1;
                        if (instruction == 'D') ActiveKey = 7;
                    }
                    else if (ActiveKey == 4)
                    {
                        if (instruction == 'R') ActiveKey = 4;
                        if (instruction == 'L') ActiveKey = 3;
                        if (instruction == 'U') ActiveKey = 4;
                        if (instruction == 'D') ActiveKey = 8;
                    }
                    else if (ActiveKey == 5)
                    {
                        if (instruction == 'R') ActiveKey = 6;
                        if (instruction == 'L') ActiveKey = 5;
                        if (instruction == 'U') ActiveKey = 5;
                        if (instruction == 'D') ActiveKey = 5;
                    }
                    else if (ActiveKey == 6)
                    {
                        if (instruction == 'R') ActiveKey = 7;
                        if (instruction == 'L') ActiveKey = 5;
                        if (instruction == 'U') ActiveKey = 2;
                        if (instruction == 'D') ActiveKey = 10;
                    }
                    else if (ActiveKey == 7)
                    {
                        if (instruction == 'R') ActiveKey = 8;
                        if (instruction == 'L') ActiveKey = 6;
                        if (instruction == 'U') ActiveKey = 3;
                        if (instruction == 'D') ActiveKey = 11;
                    }
                    else if (ActiveKey == 8)
                    {
                        if (instruction == 'R') ActiveKey = 9;
                        if (instruction == 'L') ActiveKey = 7;
                        if (instruction == 'U') ActiveKey = 4;
                        if (instruction == 'D') ActiveKey = 12;
                    }
                    else if (ActiveKey == 9)
                    {
                        if (instruction == 'R') ActiveKey = 9;
                        if (instruction == 'L') ActiveKey = 8;
                        if (instruction == 'U') ActiveKey = 9;
                        if (instruction == 'D') ActiveKey = 9;
                    }
                    else if (ActiveKey == 10)
                    {
                        if (instruction == 'R') ActiveKey = 11;
                        if (instruction == 'L') ActiveKey = 10;
                        if (instruction == 'U') ActiveKey = 6;
                        if (instruction == 'D') ActiveKey = 10;
                    }
                    else if (ActiveKey == 11)
                    {
                        if (instruction == 'R') ActiveKey = 12;
                        if (instruction == 'L') ActiveKey = 10;
                        if (instruction == 'U') ActiveKey = 7;
                        if (instruction == 'D') ActiveKey = 13;
                    }
                    else if (ActiveKey == 12)
                    {
                        if (instruction == 'R') ActiveKey = 12;
                        if (instruction == 'L') ActiveKey = 11;
                        if (instruction == 'U') ActiveKey = 8;
                        if (instruction == 'D') ActiveKey = 12;
                    }
                    else if (ActiveKey == 13)
                    {
                        if (instruction == 'R') ActiveKey = 13;
                        if (instruction == 'L') ActiveKey = 13;
                        if (instruction == 'U') ActiveKey = 11;
                        if (instruction == 'D') ActiveKey = 13;
                    }
                }
            }

            internal static void RunTwice()
            {
                string code = "";
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day2.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        Keypad2 kp = new Keypad2();
                        while (!sr.EndOfStream)
                        {
                            s = sr.ReadLine();
                            foreach (char ins in s)
                            {
                                kp.Move(ins);
                            }
                            code += kp.ActiveKey + "-";
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Code : {0}", code);
            }
        }

        internal class Day3
        {
            static bool IsValidTriangle(int a, int b, int c)
            {
                bool isValid = false;
                if(b+c > a)
                {
                    if(a+b > c)
                    {
                        if (a + c > b) isValid = true;
                    }
                }
                return isValid;
            }

            internal static void Run() { RunTwice(); }

            internal static void RunOnce()
            {
                int nValidTriangles = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day3.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        string[] separator = { "  " };
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] sides = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (IsValidTriangle(int.Parse(sides[0]), int.Parse(sides[1]), int.Parse(sides[2]))) nValidTriangles++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of valid triangles: {0}", nValidTriangles);
            }

            internal static void RunTwice()
            {
                int nValidTriangles = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day3.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s1 = string.Empty;
                        string s2 = string.Empty;
                        string s3 = string.Empty;
                        string[] separator = { "  " };
                        while ((s1 = sr.ReadLine()) != null)
                        {
                            s2 = sr.ReadLine();
                            s3 = sr.ReadLine();
                            string[] sides1 = s1.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            string[] sides2 = s2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            string[] sides3 = s3.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (IsValidTriangle(int.Parse(sides1[0]), int.Parse(sides2[0]), int.Parse(sides3[0])))
                                nValidTriangles++;
                            if (IsValidTriangle(int.Parse(sides1[1]), int.Parse(sides2[1]), int.Parse(sides3[1])))
                                nValidTriangles++;
                            if (IsValidTriangle(int.Parse(sides1[2]), int.Parse(sides2[2]), int.Parse(sides3[2])))
                                nValidTriangles++;
                        }
                    }
                }
                QueuedConsole.WriteImmediate("Number of valid triangles: {0}", nValidTriangles);
            }
        }

        internal class Day4
        {
            internal static void Run() { RunTwice(); }

            internal static void RunOnce()
            {
                int sumofSectorIds = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day4.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] tokens = s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            
                            string last = tokens.Last();
                            string[] tokenslast = last.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);
                            int sectorId = int.Parse(tokenslast[0]);
                            string checkSum = tokenslast[1].Substring(0, tokenslast[1].Length - 1);

                            Dictionary<char, int> Letters = new Dictionary<char, int>();
                            tokens = tokens.Take(tokens.Count() - 1).ToArray();
                            foreach(string token in tokens)
                            {
                                foreach(char t in token)
                                {
                                    if (Letters.ContainsKey(t)) { Letters[t] += 1; }
                                    else Letters.Add(t, 1);
                                }
                            }
                            string genCheckSum = string.Empty;
                            var items = Letters.OrderByDescending(p => p.Value).ThenBy(p => p.Key).Take(5).ToList();
                            
                            foreach (var item in items)
                            {
                                genCheckSum += item.Key;
                            }

                            if (checkSum.Equals(genCheckSum)) { sumofSectorIds += sectorId; }
                        }
                    }
                }
                QueuedConsole.WriteImmediate("sumofSectorIds: {0}", sumofSectorIds);
            }

            internal static void RunTwice()
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode._2016.Data.Day4.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] tokens = s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                            string last = tokens.Last();
                            string[] tokenslast = last.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);
                            int sectorId = int.Parse(tokenslast[0]);
                            string checkSum = tokenslast[1].Substring(0, tokenslast[1].Length - 1);

                            string roomName = string.Empty;
                            Dictionary<char, int> Letters = new Dictionary<char, int>();
                            tokens = tokens.Take(tokens.Count() - 1).ToArray();
                            List<string> shiftedTokens = new List<string>();
                            tokens.ForEach(token => shiftedTokens.Add(GenericDefs.Functions.Crypto.Cypher.Caeser.Encipher(token, sectorId)));
                            shiftedTokens.ForEach(st => roomName += st + " ");
                            roomName = roomName.ToLower();
                            if (roomName.Contains("north") && roomName.Contains("pole")) QueuedConsole.WriteImmediate("{0}  {1}", roomName, sectorId);
                        }
                    }
                }
            }
        }

        internal class Day5
        {
            internal static void Run() { RunTwice(); }

            internal static void RunOnce()
            {
                string doorId = "ffykfhsq";
                long index = 0;
                string password = "";
                int pLength = 0;
                while (true)
                {
                    ++index;
                    string toHash = doorId + index;

                    string md5 = GenericDefs.Functions.Crypto.Hash.MD5.CreateMD5(toHash);
                    if(md5.StartsWith("00000"))
                    {
                        pLength++;
                        password += md5[5];
                    }

                    if (pLength == 8) break;
                }

                QueuedConsole.WriteImmediate("password: {0}", password);
            }

            internal static void RunTwice()
            {
                string doorId = "ffykfhsq";
                long index = 0;
                Dictionary<int, char> Password = new Dictionary<int, char>();
                while (true)
                {
                    ++index;

                    string md5 = GenericDefs.Functions.Crypto.Hash.MD5.CreateMD5(doorId + index);
                    if (md5.StartsWith("00000"))
                    {
                        char p6 = md5[5];
                        char p7 = md5[6];

                        int parsedInt = 0;
                        if (int.TryParse("" + p6, out parsedInt))
                        {
                            if (parsedInt > 7 || Password.ContainsKey(parsedInt)) continue;
                            else Password.Add(parsedInt, p7);
                        }
                    }

                    if (Password.Count == 8) break;
                }

                QueuedConsole.WriteImmediate("password: ");
                foreach (KeyValuePair<int, char> kvp in Password)
                {
                    QueuedConsole.WriteImmediate("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                }
            }
        }
    }
}