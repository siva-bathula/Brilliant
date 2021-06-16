using GenericDefs.Classes;
using GenericDefs.Classes.Logic;
using GenericDefs.Classes.Quirky;
using GenericDefs.DotNet;
using GenericDefs.Functions.Algorithms.DP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    using static Day11to15.Day14.Reindeer;
    public class Day11to15
    {
        public static void Solve() { Day15.Part2(); }

        public class Day11
        {
            static string input = "vzbxxyzz"; //"vzbxkghb"; //"abcdefgh";
            static List<string> Straights = new List<string>();

            static void Init()
            {
                for(char a = 'a'; a <= 'x'; a++)
                {
                    if (inValid.Count(x => x.Equals(a)) > 0) continue;

                    char b = (char)(a + 1);
                    if (inValid.Count(x => x.Equals(b)) > 0) b = (char)(b + 1);
                    char c = (char)(b + 1);
                    if (inValid.Count(x => x.Equals(b)) > 0) c = (char)(c + 1);

                    Straights.Add(a + "" + b + "" + c);
                }
            }

            internal static void Part1()
            {
                Init();
                QueuedConsole.WriteImmediate("Next password after {0} is : {1}", input, NextPassword(input));
            }

            static string NextPassword(string s)
            {
                int index = s.Length - 1;
                char curCh = s[index];
                string workingString = s;
                while (true)
                {
                    workingString = NextString(workingString);
                    if (IsValidPassword(workingString)) break;
                }

                return workingString;
            }

            static string NextString(string s)
            {
                int index = s.Length - 1;
                string suffix = string.Empty;
                while (true)
                {
                    char c = s[index];
                    s = s.Substring(0, s.Length - 1);
                    if (c == 'z') {
                        c = 'a';
                        suffix = c + suffix;
                        index--;
                        if (index < 0) break;
                    } else {
                        char d = (char)(c + 1);
                        if (d == 'i' || d == 'o' || d == 'l') d = (char)(d + 1);
                        s = s + d + suffix;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(s)) return suffix;
                return s;
            }

            static char[] inValid = new char[] { 'i', 'o', 'l' };
            static bool IsValidPassword(string sInput) {
                string s = sInput;
                if (s.IndexOfAny(inValid) >= 0) return false;
                if (Straights.Count(x => s.IndexOf(x) >= 0) > 0) {
                    int nCount = 0;
                    int i = 0;
                    bool found = false;
                    while (true) {
                        string c1c1 = s[i] + "" + s[i];
                        while (true) {
                            int index = s.IndexOf(c1c1);
                            if (index >= 0) {
                                nCount++;
                                if (nCount >= 2) {
                                    found = true;
                                    break;
                                }
                                s = s.Remove(index, 2).Insert(index, "@#");
                                if (i > 0) i--;
                            }
                            else break;
                        }

                        if (found) break;
                        i++;
                        if (i >= s.Length) break;
                    }
                    if (found) {
                        return true;
                    }
                }
                return false;
            }
        }

        public class Day12
        {
            internal static void Solve()
            {
                int sum = 0;
                dynamic jsonObject = null;
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day12.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            jsonObject = Json.CreateDynamicObject(s);
                            sum = GenericDefs.DotNet.Strings.ExtractIntegersFromString(s).Sum(x=>x);
                        }
                    }
                }
                TraverseObjectProperties(jsonObject);
                QueuedConsole.WriteImmediate("Part1 : Sum of all integers : {0}", sum);
                QueuedConsole.WriteImmediate("Part2 : Sum of all integers without red : {0}", Value);
            }

            static object _syncRoot = new object();
            static int _value = 0;
            /// <summary>
            /// setter is used as sum function.
            /// </summary>
            static int Value {
                get { return _value; }
                set {
                    lock (_syncRoot) {
                        _value += value;
                    }
                }
            }

            static HashSet<string> objectTypes = new HashSet<string>();
            internal static void TraverseObjectProperties(dynamic d)
            {
                objectTypes.Add(d.GetType().ToString());
                bool hasRed = false;
                if (d.GetType() == typeof(int)) {
                    Value = int.Parse(d + "");
                    return;
                } else if (d.GetType() == typeof(string)) {
                    return;
                } else if (d.GetType() == typeof(Dictionary<string, object>)) {
                    var oKvp = (Dictionary<string, object>)d;
                    foreach (KeyValuePair<string, object> kvp in oKvp)
                    {
                        if (kvp.Key.ToLower().Contains("red")) { hasRed = true; break; }
                        if (kvp.Value.GetType() == typeof(string))
                        {
                            if (kvp.Value.ToString().ToLower().Contains("red")) { hasRed = true; break; }
                        }
                    }
                    if (hasRed) return;
                    foreach (KeyValuePair<string, object> kvp in oKvp)
                    {
                        if (kvp.Value.GetType() == typeof(int)) { Value = int.Parse(kvp.Value + ""); }
                        else if (kvp.Value.GetType() == typeof(string)) continue;
                        else TraverseObjectProperties(kvp.Value);
                    }
                    return;
                } else if (d.GetType() == typeof(object[])) {
                    foreach (object o in d) {
                        TraverseObjectProperties(o);
                    }
                } else if (d.GetType() == typeof(KeyValuePair<string, object>)) {
                    var kvp = (KeyValuePair<string, object>)d;
                    if (kvp.Value.GetType() == typeof(int)) { Value = int.Parse(kvp.Value + ""); }
                    else if (kvp.Value.GetType() == typeof(string)) return;
                    else TraverseObjectProperties(kvp.Value);
                } else {
                    return;
                }
            }
        }

        public class Day13
        {
            internal static void Init(bool isPart2)
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day13.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        int pCount = 0;
                        while ((s = sr.ReadLine()) != null)
                        {
                            int n = GenericDefs.DotNet.Strings.ExtractIntegersFromString(s)[0];
                            Type hType = Type.None;
                            if (s.IndexOf("gain") >= 0) hType = Type.Gain;
                            else hType = Type.Lose;
                            string[] spArr = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            Person P1;
                            if (Persons.ContainsKey(spArr[0])) { P1 = Persons[spArr[0]]; }
                            else {
                                pCount++;
                                P1 = new Person() { Id = pCount, Name = spArr[0] };
                                Persons.Add(P1.Name, P1);
                            }
                            Person P2;
                            string p2Name = spArr[spArr.Length - 1].Substring(0, spArr[spArr.Length - 1].Length - 1);
                            if (Persons.ContainsKey(p2Name)) { P2 = Persons[p2Name]; }
                            else {
                                pCount++;
                                P2 = new Person() { Id = pCount, Name = p2Name };
                                Persons.Add(P2.Name, P2);
                            }

                            Scenario c = new Scenario() { Neighbour = P2, UnitChange = n, ChangeType = hType };

                            if (Scenarios.ContainsKey(P1.Id)) { Scenarios[P1.Id].Add(c); }
                            else { Scenarios.Add(P1.Id, new List<Scenario>() { c }); }
                        }

                        if (isPart2) {
                            Person me = new Person() { Id = Persons.Count + 1, Name = "Me" };
                            List<Scenario> meSc = new List<Scenario>();
                            foreach(KeyValuePair<string, Person> kvp in Persons)
                            {
                                Scenario c = new Scenario() { ChangeType = Type.None, UnitChange = 0, Neighbour = kvp.Value };
                                meSc.Add(c);
                            }
                            Persons.Add(me.Name, me);
                            foreach (KeyValuePair<int, List<Scenario>> kvp in Scenarios) {
                                Scenario c = new Scenario() { ChangeType = Type.None, UnitChange = 0, Neighbour = me };
                                kvp.Value.Add(c);
                            }
                            Scenarios.Add(me.Id, meSc);
                        }
                    }
                }
            }

            static int optimalArrangement = 0;
            internal static void Solve()
            {
                Init(true);
                Func<CryptRule, bool> rule = delegate (CryptRule cr)
                {
                    int[] coeff = cr.GetCoefficients();
                    int curArrangement = 0;
                    for(int i = 0; i< coeff.Length; i++)
                    {
                        int n1 = -1, n2 = -1;
                        if (i == 0) n1 = coeff[coeff.Length - 1];
                        else n1 = coeff[i - 1];
                        if (i < coeff.Length - 1) n2 = coeff[i + 1];
                        else if (i == coeff.Length - 1) n2 = coeff[0];

                        List<Scenario> ithScenarios = Scenarios[coeff[i]];
                        foreach (Scenario c in ithScenarios)
                        {
                            if (c.ChangeType == Type.None) continue;
                            if (c.Neighbour.Id == n1 || c.Neighbour.Id == n2) {
                                curArrangement += (c.ChangeType == Type.Gain ? 1 : -1) * c.UnitChange;
                            }
                        }
                    }
                    if (optimalArrangement < curArrangement) optimalArrangement = curArrangement;
                    return false;
                };

                CryptRule[] rules = new CryptRule[] { new CryptRule(rule) };
                Cryptarithm cArithm = new Cryptarithm(rules);

                List<Iterator<int>> iterators = new List<Iterator<int>>();
                for (int i = 0; i < Persons.Count; i++)
                {
                    Iterator<int> iter = new Iterator<int>(true, new Range<int>() { IsMinInclusive = true, IsMaxInclusive = true, Max = Persons.Count, Min = 1 });
                    iterators.Add(iter);
                }
                CryptRecursiveSolver solver = new CryptRecursiveSolver(true, cArithm);
                solver.GetAllSolutions(Persons.Count, iterators);

                QueuedConsole.WriteImmediate("Total change in happiness for the optimal seating arrangement of the actual guest list : {0}", optimalArrangement);
            }

            static Dictionary<int, List<Scenario>> Scenarios = new Dictionary<int, List<Scenario>>();

            internal class Person
            {
                internal string Name;
                internal int Id;
            }

            internal class Scenario
            {
                internal Person Neighbour { get; set; }
                internal int UnitChange { get; set; }
                internal Type ChangeType { get; set; }
            }

            static Dictionary<string, Person> Persons = new Dictionary<string, Person>();
            internal enum Type
            {
                Gain,
                Lose,
                None
            }
        }

        public class Day14
        {
            static string input = @"Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.
                Blitzen can fly 13 km/s for 4 seconds, but then must rest for 49 seconds.
                Rudolph can fly 20 km/s for 7 seconds, but then must rest for 132 seconds.
                Cupid can fly 12 km/s for 4 seconds, but then must rest for 43 seconds.
                Donner can fly 9 km/s for 5 seconds, but then must rest for 38 seconds.
                Dasher can fly 10 km/s for 4 seconds, but then must rest for 37 seconds.
                Comet can fly 3 km/s for 37 seconds, but then must rest for 76 seconds.
                Prancer can fly 9 km/s for 12 seconds, but then must rest for 97 seconds.
                Dancer can fly 37 km/s for 1 seconds, but then must rest for 36 seconds.";
            internal class Reindeer
            {
                internal string Name { get; set; }
                internal int Speed { get; set; }
                internal int FlyingTime { get; set; }
                internal int RestingTime { get; set; }

                List<int> SnapShots = new List<int>();
                internal int Fly(int nSec)
                {
                    int distanceTravelledInInstance = 0;
                    int totalTimeLeft = nSec;
                    bool restNow = false;
                    while (true)
                    {
                        if (restNow) {
                            if (totalTimeLeft > RestingTime) totalTimeLeft -= RestingTime;
                            else totalTimeLeft = 0;
                        } else {
                            if (totalTimeLeft > FlyingTime) {
                                totalTimeLeft -= FlyingTime;
                                distanceTravelledInInstance += FlyingTime * Speed;
                            } else {
                                distanceTravelledInInstance += totalTimeLeft * Speed;
                                totalTimeLeft = 0;
                            }
                        }
                        restNow = !restNow;
                        if (totalTimeLeft <= 0) break;
                    }
                    SnapShots.Add(distanceTravelledInInstance);
                    _DistanceFromStart = SnapShots.Sum();
                    return distanceTravelledInInstance;
                }

                internal void FlyOverRide(int n, bool isNew)
                {
                    if (!isNew && SnapShots.Count > 0) {
                        SnapShots.RemoveAt(SnapShots.Count - 1);
                    }

                    Fly(n);
                }

                internal class FlyInstance {
                    internal Reindeer Reindeer { get; set; }
                    internal int TotalTime { get; set; }
                    internal int TimeFlied { get; set; }
                    internal int IncrementalValue { get; set; }
                    internal bool Increment()
                    {
                        if (TimeFlied >= TotalTime) return false;
                        TimeFlied += IncrementalValue;
                        Reindeer.FlyOverRide(TimeFlied, TimeFlied == 0);
                        return true;
                    }
                }

                internal FlyInstance FlyInstanceIncremental(int n, int increment)
                {
                    return new FlyInstance() { IncrementalValue = increment, TotalTime = n, Reindeer = this };
                }

                private int _DistanceFromStart = 0;
                internal int GetDistanceFromStart()
                {
                    return _DistanceFromStart;
                }

                private int _CurrentPoints = 0;
                internal void AwardPoints(int n)
                {
                    _CurrentPoints += n;
                }

                internal int GetCurrentPoints() { return _CurrentPoints; }
            }

            static List<Reindeer> Reindeers = new List<Reindeer>();
            static void Init()
            {
                string[] inputs = input.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in inputs)
                {
                    List<int> numbers = GenericDefs.DotNet.Strings.ExtractIntegersFromString(s);
                    string[] spArr = s.Split(new char[] { ' ' });
                    Reindeers.Add(new Reindeer() { Name = spArr[0], FlyingTime = numbers[1], Speed = numbers[0], RestingTime = numbers[2] });
                }
            }

            internal static void Part1()
            {
                Init();
                int n = 2503;
                List<int> instances = new List<int>();
                foreach(Reindeer r in Reindeers)
                {
                    instances.Add(r.Fly(n));
                }
                QueuedConsole.WriteImmediate("Distance travelled by winning reindeer : {0}", instances.Max());
            }

            internal static void Part2()
            {
                Init();
                int n = 2503;

                List<FlyInstance> flyInstances = new List<FlyInstance>();
                foreach (Reindeer r in Reindeers)
                {
                    flyInstances.Add(r.FlyInstanceIncremental(n, 1));
                }

                while (true)
                {
                    foreach (FlyInstance fi in flyInstances)
                    {
                        fi.Increment();
                    }
                    int max = Reindeers.Max(x => x.GetDistanceFromStart());
                    foreach (Reindeer r in Reindeers) { if (r.GetDistanceFromStart() == max) r.AwardPoints(1); }

                    n--;
                    if (n == 0) break;
                }
                QueuedConsole.WriteImmediate("Winning reindeer has : {0} points", Reindeers.Max(x=> x.GetCurrentPoints()));
            }
        }

        public class Day15
        {
            internal class Ingredient
            {
                internal string Name { get; set; }
                internal int Capacity { get; set; }
                internal int Durability { get; set; }
                internal int Flavor { get; set; }
                internal int Texture { get; set; }
                internal int Calories { get; set; }
            }

            static Dictionary<int,Ingredient> Ingredients = new Dictionary<int, Ingredient>();
            static void Init()
            {
                int nCount = 0;
                Ingredients.Add(++nCount, new Ingredient() { Name = "Frosting", Capacity = 4, Durability = -2, Flavor = 0, Texture = 0, Calories = 5 });
                Ingredients.Add(++nCount, new Ingredient() { Name = "Candy", Capacity = 0, Durability = 5, Flavor = -1, Texture = 0, Calories = 8 });
                Ingredients.Add(++nCount, new Ingredient() { Name = "Butterscotch", Capacity = -1, Durability = 0, Flavor = 5, Texture = 0, Calories = 6 });
                Ingredients.Add(++nCount, new Ingredient() { Name = "Sugar", Capacity = 0, Durability = 0, Flavor = -2, Texture = 2, Calories = 1 });
            }

            internal static void Part1()
            {
                Init();
                UniqueIntegralPairs uip = Knapsack.Variation1.Solve(100, 4, 0);
                
                long totalScoreOfHighestScoringCookie = 0;
                List<UniqueIntegralPairs.Combination> combinations = uip.GetCombinations();

                foreach (UniqueIntegralPairs.Combination c in combinations)
                {
                    long capacity = 0, durability = 0, flavor = 0, texture = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        capacity += Ingredients[i + 1].Capacity * c.Pair[i];
                        durability += Ingredients[i + 1].Durability * c.Pair[i];
                        flavor += Ingredients[i + 1].Flavor * c.Pair[i];
                        texture += Ingredients[i + 1].Texture * c.Pair[i];
                    }
                    long t = (capacity < 0 ? 0 : capacity) * (durability < 0 ? 0 : durability) * (flavor < 0 ? 0 : flavor) * (texture < 0 ? 0 : texture);
                    totalScoreOfHighestScoringCookie = Math.Max(t, totalScoreOfHighestScoringCookie);
                }
                QueuedConsole.WriteImmediate("Total score of the highest-scoring cookie : {0}", totalScoreOfHighestScoringCookie);
            }

            internal static void Part2()
            {
                Init();
                UniqueIntegralPairs uip = Knapsack.Variation1.Solve(100, 4, 0);

                long totalScoreOfHighestScoringCookie = 0;
                List<UniqueIntegralPairs.Combination> combinations = uip.GetCombinations();

                foreach (UniqueIntegralPairs.Combination c in combinations)
                {
                    long capacity = 0, durability = 0, flavor = 0, texture = 0, calories = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        capacity += Ingredients[i + 1].Capacity * c.Pair[i];
                        durability += Ingredients[i + 1].Durability * c.Pair[i];
                        flavor += Ingredients[i + 1].Flavor * c.Pair[i];
                        texture += Ingredients[i + 1].Texture * c.Pair[i];
                        calories += Ingredients[i + 1].Calories * c.Pair[i];
                    }
                    if (calories != 500) continue;
                    long t = (capacity < 0 ? 0 : capacity) * (durability < 0 ? 0 : durability) * (flavor < 0 ? 0 : flavor) * (texture < 0 ? 0 : texture);
                    totalScoreOfHighestScoringCookie = Math.Max(t, totalScoreOfHighestScoringCookie);
                }
                QueuedConsole.WriteImmediate("Total score of the highest-scoring cookie : {0}", totalScoreOfHighestScoringCookie);
            }
        }
    }
}