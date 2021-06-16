using GenericDefs.DotNet;
using GenericDefs.Functions.Logic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.Quirky;
using System.Text;

namespace AdventOfCode
{
    public class Day6to10
    {
        public static void Solve()
        {
            Day10.Solve();
        }

        internal class Day6
        {
            internal static bool _isPartTwo = true;
            internal class Light
            {
                internal bool IsPartTwo { get { return _isPartTwo; } }
                internal Light(int x, int y) { X = x; Y = y; Brightness = 0; }
                internal int X;
                internal int Y;
                internal bool IsSwitchedOn { get; set; }
                internal void Switch(bool turnOn) {
                    if (IsPartTwo) {
                        if (turnOn) Brightness += 1;
                        else Brightness = Brightness > 0 ? Brightness - 1 : 0;
                    } else IsSwitchedOn = turnOn;
                }
                internal void Toggle() {
                    if (IsPartTwo) Brightness += 2;
                    else IsSwitchedOn = !IsSwitchedOn;
                }

                internal int Brightness { get; set; }
            }

            static void Init()
            {
                for(int i = 0; i< 1000; i++)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        Lights.Add(new Light(i, j));
                    }
                }
            }

            static void TurnOn(int xStart, int yStart, int xEnd, int yEnd)
            {
                var filter = Lights.Where(x => (x.X >= xStart && x.Y >= yStart && x.X <= xEnd && x.Y <= yEnd));
                foreach(Light l in filter)
                {
                    l.Switch(true);
                }
            }

            static void TurnOff(int xStart, int yStart, int xEnd, int yEnd)
            {
                var filter = Lights.Where(x => (x.X >= xStart && x.Y >= yStart && x.X <= xEnd && x.Y <= yEnd));
                foreach (Light l in filter)
                {
                    l.Switch(false);
                }
            }

            static void Toggle(int xStart, int yStart, int xEnd, int yEnd)
            {
                var filter = Lights.Where(x => (x.X >= xStart && x.Y >= yStart && x.X <= xEnd && x.Y <= yEnd));
                foreach (Light l in filter)
                {
                    l.Toggle();
                }
            }

            static List<Light> Lights = new List<Light>();

            internal static void Solve()
            {
                Init();
                FollowInstructions();
            }

            static void FollowInstructions()
            {
                string s = string.Empty;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day6.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] strArr = s.Split(new char[] { ' ' });
                            int sStart = 0, sEnd = 0;
                            bool isToggle = false;
                            if (s.IndexOf("turn") >= 0) {
                                sStart = 2; sEnd = 4;
                            }
                            else if (s.IndexOf("toggle") >= 0) { isToggle = true; sStart = 1; sEnd = 3; }
                            string[] start = strArr[sStart].Split(new char[] { ',' });
                            string[] end = strArr[sEnd].Split(new char[] { ',' });
                            if (!isToggle) {
                                if (s.IndexOf("off") >= 0) { TurnOff(int.Parse(start[0]), int.Parse(start[1]), int.Parse(end[0]), int.Parse(end[1])); }
                                else if (s.IndexOf("on") >= 0) { TurnOn(int.Parse(start[0]), int.Parse(start[1]), int.Parse(end[0]), int.Parse(end[1])); }
                            } else if (isToggle) {
                                Toggle(int.Parse(start[0]), int.Parse(start[1]), int.Parse(end[0]), int.Parse(end[1]));
                            }
                        }
                    }
                }

                long totalBrightness = Lights.Sum(x => x.Brightness);
                QueuedConsole.WriteImmediate("Number of lights lit : {0}", totalBrightness);
            }
        }

        internal class Day7
        {
            internal class Wire:Signal
            {
                public string Name { get; set; }
            }

            internal class Signal
            {
                public long? Value { get; set; }
            }

            internal enum OperationType
            {
                OR,
                AND,
                RIGHTSHIFT,
                LEFTSHIFT,
                UNARY,
                ASSIGNMENT
            }

            internal class Instruction
            {
                public Instruction(OperationType ot, Signal s1, Signal s2, Wire rhs, string ins) {
                    OT = ot;
                    S1 = s1;
                    S2 = s2;
                    WireRHS = rhs;
                    ActualInstruction = ins;
                    if (ot == OperationType.ASSIGNMENT) IsAssignment = true;
                    else if (ot == OperationType.UNARY) IsUnary = true;
                }
                internal OperationType OT { get; set; }
                Signal S1 { get; set; }
                Signal S2 { get; set; }
                internal Wire WireRHS { get; set; }
                bool IsAssignment { get; set; }
                bool IsUnary { get; set; }
                string ActualInstruction { get; set; }
                bool FinishedExecuting { get; set; }
                internal bool Execute()
                {
                    if (FinishedExecuting) return false;
                    if (S1 != null && S1 is Wire) {
                        S1 = Wires[((Wire)S1).Name];
                    }
                    if (S2 != null && S2 is Wire) {
                        S2 = Wires[((Wire)S2).Name];
                    }
                    WireRHS = Wires[WireRHS.Name];
                    bool isFinished = false;
                    if (OT == OperationType.ASSIGNMENT) {
                        if (S1.Value.HasValue) { WireRHS.Value = S1.Value.Value; isFinished = true; }
                    } else if (OT == OperationType.UNARY) {
                        if (S1.Value.HasValue) { WireRHS.Value = Bitwise.Unary(S1.Value.Value); isFinished = true; }
                    } else if (OT == OperationType.AND) {
                        if (S1.Value.HasValue && S2.Value.HasValue) {
                            WireRHS.Value = Bitwise.AND(S1.Value.Value, S2.Value.Value);
                            isFinished = true;
                        }
                    } else if (OT == OperationType.OR) {
                        if (S1.Value.HasValue && S2.Value.HasValue) {
                            WireRHS.Value = Bitwise.OR(S1.Value.Value, S2.Value.Value);
                            isFinished = true;
                        }
                    } else if (OT == OperationType.RIGHTSHIFT) {
                        if (S1.Value.HasValue && S2.Value.HasValue) {
                            WireRHS.Value = Bitwise.BinaryRighttShift(S1.Value.Value, (int)S2.Value.Value);
                            isFinished = true;
                        }
                    } else if (OT == OperationType.LEFTSHIFT) {
                        if (S1.Value.HasValue && S2.Value.HasValue) {
                            WireRHS.Value = Bitwise.BinaryLeftShift(S1.Value.Value, (int)S2.Value.Value);
                            isFinished = true;
                        }
                    }

                    Wires[WireRHS.Name] = WireRHS;
                    if (isFinished) FinishedExecuting = true;
                    return FinishedExecuting;
                }
            }

            static Dictionary<string, Wire> Wires = new Dictionary<string, Wire>();
            static List<Instruction> Instructions = new List<Instruction>();

            static void Init()
            {
                string s = string.Empty;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day7.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            string actIns = s;
                            string[] strArr = s.Replace(" ","").Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                            Wire wRhs;
                            if (!Wires.ContainsKey(strArr[1])) {
                                wRhs = new Wire() { Name = strArr[1], Value = null };
                                Wires.Add(strArr[1], wRhs);
                            } else wRhs = Wires[strArr[1]];
                            OperationType oT = GetOperationType(actIns);
                            int n1 = 0;
                            if(oT == OperationType.ASSIGNMENT) {
                                Signal s1;
                                if (IsNumeric(strArr[0], ref n1)) {
                                    s1 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(strArr[0])) {
                                        s1 = new Wire() { Name = strArr[0], Value = null };
                                        Wires.Add(strArr[0], (Wire)s1);
                                    }
                                    else s1 = Wires[strArr[0]];
                                }
                                Instruction instruction = new Instruction(oT, s1, null, wRhs, actIns);
                                Instructions.Add(instruction);
                            } else if (oT == OperationType.UNARY) {
                                string unaryStr = strArr[0].Replace(" ", "").Replace("NOT", "");
                                Signal s1;
                                if (!Wires.ContainsKey(unaryStr)) {
                                    s1 = new Wire() { Name = unaryStr, Value = null };
                                    Wires.Add(unaryStr, (Wire)s1);
                                } else s1 = Wires[unaryStr];
                                Instruction instruction = new Instruction(oT, s1, null, wRhs, actIns);
                                Instructions.Add(instruction);
                            } else if (oT == OperationType.AND) {
                                string[] andStr = strArr[0].Replace(" ", "").Split(new string[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
                                Signal s1, s2;
                                if (IsNumeric(andStr[0], ref n1)) {
                                    s1 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(andStr[0]))
                                    {
                                        s1 = new Wire() { Name = andStr[0], Value = null };
                                        Wires.Add(andStr[0], (Wire)s1);
                                    }
                                    else s1 = Wires[andStr[0]];
                                }

                                if (IsNumeric(andStr[1], ref n1)) {
                                    s2 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(andStr[1]))
                                    {
                                        s2 = new Wire() { Name = andStr[1], Value = null };
                                        Wires.Add(andStr[1], (Wire)s2);
                                    }
                                    else s2 = Wires[andStr[1]];
                                }
                                Instruction instruction = new Instruction(oT, s1, s2, wRhs, actIns);
                                Instructions.Add(instruction);
                            } else if (oT == OperationType.OR) {
                                string[] andStr = strArr[0].Replace(" ", "").Split(new string[] { "OR" }, StringSplitOptions.RemoveEmptyEntries);
                                Signal s1, s2;
                                if (IsNumeric(andStr[0], ref n1)) {
                                    s1 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(andStr[0]))
                                    {
                                        s1 = new Wire() { Name = andStr[0], Value = null };
                                        Wires.Add(andStr[0], (Wire)s1);
                                    }
                                    else s1 = Wires[andStr[0]];
                                }

                                if (IsNumeric(andStr[1], ref n1)) {
                                    s2 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(andStr[1]))
                                    {
                                        s2 = new Wire() { Name = andStr[1], Value = null };
                                        Wires.Add(andStr[1], (Wire)s2);
                                    }
                                    else s2 = Wires[andStr[1]];
                                }
                                Instruction instruction = new Instruction(oT, s1, s2, wRhs, actIns);
                                Instructions.Add(instruction);
                            } else if (oT == OperationType.RIGHTSHIFT) {
                                string[] andStr = strArr[0].Replace(" ", "").Split(new string[] { "RSHIFT" }, StringSplitOptions.RemoveEmptyEntries);
                                Signal s1, s2;
                                if (IsNumeric(andStr[0], ref n1)) {
                                    s1 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(andStr[0]))
                                    {
                                        s1 = new Wire() { Name = andStr[0], Value = null };
                                        Wires.Add(andStr[0], (Wire)s1);
                                    }
                                    else s1 = Wires[andStr[0]];
                                }

                                if (IsNumeric(andStr[1], ref n1)) {
                                    s2 = new Signal() { Value = n1 };
                                }
                                else {
                                    if (!Wires.ContainsKey(andStr[1]))
                                    {
                                        s2 = new Wire() { Name = andStr[1], Value = null };
                                        Wires.Add(andStr[1], (Wire)s2);
                                    }
                                    else s2 = Wires[andStr[1]];
                                }
                                Instruction instruction = new Instruction(oT, s1, s2, wRhs, actIns);
                                Instructions.Add(instruction);
                            } else if (oT == OperationType.LEFTSHIFT) {
                                string[] andStr = strArr[0].Replace(" ", "").Split(new string[] { "LSHIFT" }, StringSplitOptions.RemoveEmptyEntries);
                                Signal s1, s2;
                                if (IsNumeric(andStr[0], ref n1)) {
                                    s1 = new Signal() { Value = n1 };
                                }
                                else {
                                    if (!Wires.ContainsKey(andStr[0]))
                                    {
                                        s1 = new Wire() { Name = andStr[0], Value = null };
                                        Wires.Add(andStr[0], (Wire)s1);
                                    }
                                    else s1 = Wires[andStr[0]];
                                }

                                if (IsNumeric(andStr[1], ref n1)) {
                                    s2 = new Signal() { Value = n1 };
                                } else {
                                    if (!Wires.ContainsKey(andStr[1]))
                                    {
                                        s2 = new Wire() { Name = andStr[1], Value = null };
                                        Wires.Add(andStr[1], (Wire)s2);
                                    }
                                    else s2 = Wires[andStr[1]];
                                }
                                Instruction instruction = new Instruction(oT, s1, s2, wRhs, actIns);
                                Instructions.Add(instruction);
                            }
                        }
                    }
                }
            }

            static bool IsNumeric(string s, ref int nValue)
            {
                return int.TryParse(s, out nValue);
            }

            internal static void Solve()
            {
                Init();
                int count = 0;
                while (true)
                {
                    foreach (Instruction ins in Instructions)
                    {
                        if (ins.Execute()) count++;
                    }
                    if (count == Instructions.Count) break;
                }

                QueuedConsole.WriteImmediate("Value supplied to a : {0}", Wires["a"].Value);
            }

            internal static void Part2()
            {
                Init();
                Wires["b"].Value = 16076;
                int count = 1;
                while (true)
                {
                    foreach (Instruction ins in Instructions)
                    {
                        if (ins.WireRHS.Name == "b") continue;
                        if (ins.Execute()) count++;
                    }
                    if (count == Instructions.Count) break;
                }

                QueuedConsole.WriteImmediate("Value supplied to a : {0}", Wires["a"].Value);
            }

            static OperationType GetOperationType(string s)
            {
                if (s.IndexOf("AND") >= 0) return OperationType.AND;
                else if (s.IndexOf("OR") >= 0) return OperationType.OR;
                else if (s.IndexOf("RSHIFT") >= 0) return OperationType.RIGHTSHIFT;
                else if (s.IndexOf("LSHIFT") >= 0) return OperationType.LEFTSHIFT;
                else if (s.IndexOf("NOT") >= 0) return OperationType.UNARY;
                return OperationType.ASSIGNMENT;
            }
        }

        internal class Day8
        {
            static List<string> Literals = new List<string>();
            static void Init()
            {
                string s = string.Empty;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day8.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            Literals.Add(s);
                        }
                    }
                }
            }

            internal static void Part1()
            {
                Init();
                int totalCount = 0;
                int inmemoryCount = 0;
                foreach(string s in Literals)
                {
                    totalCount += s.Length;
                    inmemoryCount += System.Text.RegularExpressions.Regex.Unescape(s).Length;
                    if (s.StartsWith("\"") && s.EndsWith("\"")) inmemoryCount -= 2;
                }

                QueuedConsole.WriteImmediate("number of characters in literals minus in memory : {0}", totalCount - inmemoryCount);
            }

            internal static void Part2()
            {
                Init();
                int totalCount = 0;
                int inmemoryCount = 0;
                foreach (string s in Literals)
                {
                    totalCount += s.Length;
                    inmemoryCount += (s.Replace("\\", "\\\\").Replace("\"", "\\\"")).Length + 2;
                }

                QueuedConsole.WriteImmediate("number of characters in literals minus in memory : {0}", inmemoryCount - totalCount);
            }
        }

        internal class Day9
        {
            static Dictionary<string, int> Distances = new Dictionary<string, int>();
            static Dictionary<string, int> Cities = new Dictionary<string, int>();
            static void Init()
            {
                string s = string.Empty;
                int cityId = 0;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day9.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] strArr = s.Replace(" ","").Split(new char[] { '=' });
                            string[] aToB = strArr[0].Split(new string[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                            
                            if (!Cities.ContainsKey(aToB[0])) {
                                cityId++;
                                Cities.Add(aToB[0], cityId);
                            }
                            if (!Cities.ContainsKey(aToB[1])) {
                                cityId++;
                                Cities.Add(aToB[1], cityId);
                            }
                            Distances.Add(Cities[aToB[0]] + "-" + Cities[aToB[1]], int.Parse(strArr[1]));
                            Distances.Add(Cities[aToB[1]] + "-" + Cities[aToB[0]], int.Parse(strArr[1]));
                        }
                    }
                }
            }

            internal static void FindShortestRoute()
            {
                Init();
                int minDistance = int.MaxValue - 1;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    int distance = 0;
                    for (int i = 0; i < coeff.Count() -1; i++)
                    {
                        distance += Distances[coeff[i] + "-" + coeff[i + 1]];
                    }
                    if (distance < minDistance) minDistance = distance;
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < 8; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 8, Min = 1 });
                    iterators.Add(iter);
                }
                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                solver.GetAllSolutions(8, iterators);

                QueuedConsole.WriteImmediate("Distance of the shortest route : {0}", minDistance);
            }

            internal static void FindLongestRoute()
            {
                Init();
                int maxDistance = int.MinValue;
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    int distance = 0;
                    for (int i = 0; i < coeff.Count() - 1; i++)
                    {
                        distance += Distances[coeff[i] + "-" + coeff[i + 1]];
                    }
                    if (distance > maxDistance) maxDistance = distance;
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < 8; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = 8, Min = 1 });
                    iterators.Add(iter);
                }
                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                solver.GetAllSolutions(8, iterators);

                QueuedConsole.WriteImmediate("Distance of the longest route : {0}", maxDistance);
            }
        }

        internal class Day10
        {
            static string input = "1113122113";

            internal static void Solve()
            {
                int n = 50;
                int count = 0;
                StringBuilder nextInput = new StringBuilder();
                string curInput = input;
                while (true)
                {
                    string buffer = string.Empty;
                    foreach (char ch in curInput)
                    {
                        if (string.IsNullOrEmpty(buffer)) { buffer += ch; }
                        else if (buffer[0] == ch) buffer += ch;
                        else
                        {
                            nextInput.Append(buffer.Length + "" + buffer[0]);
                            buffer = "" + ch;
                        }
                    }
                    if(!string.IsNullOrEmpty(buffer)) nextInput.Append(buffer.Length + "" + buffer[0]);
                    count++;
                    curInput = nextInput.ToString();
                    if (count == n) break;
                    else nextInput = new StringBuilder();
                }
                QueuedConsole.WriteImmediate("Length of result : {0}", nextInput.Length);
            }
        }
    }
}