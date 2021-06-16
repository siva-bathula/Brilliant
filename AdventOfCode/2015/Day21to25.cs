using GenericDefs.Classes;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode
{
    public class Day21to25
    {
        public static void Solve()
        {
            Day24_Copied.Solve();
        }

        internal class Day21
        {
            internal class Player
            {
                internal int HitPoints { get; set; }
                internal int Damage { get; set; }
                internal int ArmorValue { get; set; }
                internal int GoldSpent { get; set; }

                internal bool Attack(Player p)
                {
                    if (Damage <= 0) return false;
                    p.TakeHit(this);
                    return true;
                }

                private void TakeHit(Player p)
                {
                    if (p.HitPoints <= 0) return;
                    if (HitPoints <= 0) return;
                    int diff = p.Damage - ArmorValue;
                    if (diff <= 0) diff = 1;

                    HitPoints -= diff;
                }

                ShopItem Weapon { get; set; }
                internal bool BuyWeapon(ShopItem s)
                {
                    if (Weapon != null) return false;

                    Weapon = s;
                    Damage = Weapon.Damage;
                    ArmorValue += Weapon.Armor;
                    GoldSpent += Weapon.Cost;
                    return true;
                }

                internal bool RemoveWeapon()
                {
                    if (Weapon == null) return false;

                    Damage -= Weapon.Damage;
                    ArmorValue -= Weapon.Armor;
                    GoldSpent -= Weapon.Cost;
                    Weapon = null;
                    return true;
                }

                ShopItem Armor { get; set; }
                internal bool BuyArmor(ShopItem s)
                {
                    if (Armor != null) return false;

                    Armor = s;
                    Damage += Armor.Damage;
                    ArmorValue += Armor.Armor;
                    GoldSpent += Armor.Cost;
                    return true;
                }

                internal bool RemoveArmor()
                {
                    if (Armor == null) return false;

                    Damage -= Armor.Damage;
                    ArmorValue -= Armor.Armor;
                    GoldSpent -= Armor.Cost;
                    Armor = null;
                    return true;
                }

                List<ShopItem> Rings { get; set; }
                internal bool BuyRing(ShopItem s)
                {
                    if (Rings == null) Rings = new List<ShopItem>();
                    if (Rings.Count == 2) return false;

                    Rings.Add(s);
                    Damage += s.Damage;
                    ArmorValue += s.Armor;
                    GoldSpent += s.Cost;
                    return true;
                }

                internal bool RemoveRings()
                {
                    if (Rings == null || Rings.Count == 0) return false;

                    foreach(ShopItem s in Rings)
                    {
                        Damage -= s.Damage;
                        ArmorValue -= s.Armor;
                        GoldSpent -= s.Cost;
                    }
                    Rings.Clear();
                    return true;
                }

                internal bool IsDead()
                {
                    return HitPoints <= 0;
                }

                internal void ResetHitPoints()
                {
                    HitPoints = 100;
                }
            }

            internal class ShopItem
            {
                internal int Cost { get; set; }
                internal int Damage { get; set; }
                internal int Armor { get; set; }
                internal string Name { get; set; }
            }

            static void Init()
            {
                Weapons.Add(new ShopItem() { Name = "Dagger", Cost = 8, Damage = 4, Armor = 0});
                Weapons.Add(new ShopItem() { Name = "Shortsword", Cost = 10, Damage = 5, Armor = 0 });
                Weapons.Add(new ShopItem() { Name = "Warhammer", Cost = 25, Damage = 6, Armor = 0 });
                Weapons.Add(new ShopItem() { Name = "Longsword", Cost = 40, Damage = 7, Armor = 0 });
                Weapons.Add(new ShopItem() { Name = "Greataxe", Cost = 74, Damage = 8, Armor = 0 });
                
                Armor.Add(new ShopItem() { Name = "Nothing", Cost = 0, Damage = 0, Armor = 0 });
                Armor.Add(new ShopItem() { Name = "Leather", Cost = 13, Damage = 0, Armor = 1 });
                Armor.Add(new ShopItem() { Name = "Chainmail", Cost = 31, Damage = 0, Armor = 2 });
                Armor.Add(new ShopItem() { Name = "Splintmail", Cost = 53, Damage = 0, Armor = 3 });
                Armor.Add(new ShopItem() { Name = "Bandedmail", Cost = 75, Damage = 0, Armor = 4 });
                Armor.Add(new ShopItem() { Name = "Platemail", Cost = 102, Damage = 0, Armor = 5 });

                Rings.Add(new ShopItem() { Name = "Damage +1", Cost = 25, Damage = 1, Armor = 0 });
                Rings.Add(new ShopItem() { Name = "Damage +2", Cost = 50, Damage = 2, Armor = 0 });
                Rings.Add(new ShopItem() { Name = "Damage +3", Cost = 100, Damage = 3, Armor = 0 });
                Rings.Add(new ShopItem() { Name = "Defense +1", Cost = 20, Damage = 0, Armor = 1 });
                Rings.Add(new ShopItem() { Name = "Defense +2", Cost = 40, Damage = 0, Armor = 2 });
                Rings.Add(new ShopItem() { Name = "Defense +3", Cost = 80, Damage = 0, Armor = 3 });
            }

            static Player Boss = new Player() { HitPoints = 100, ArmorValue = 2, Damage = 8 };
            static Player You = new Player() { HitPoints = 100 };

            static List<ShopItem> Armor = new List<ShopItem>();
            static List<ShopItem> Weapons = new List<ShopItem>();
            static List<ShopItem> Rings = new List<ShopItem>();
            internal static void Solve()
            {
                Init();
                SimulateFight(false);
            }

            internal static void SimulateFight(bool isPart2)
            {
                List<IList<ShopItem>> twoRingCombinations = Combinations.GetAllCombinations(Rings, 2);
                int expenditure = isPart2 ? 0: int.MaxValue;
                foreach(ShopItem w in Weapons)
                {
                    You.RemoveWeapon();
                    You.BuyWeapon(w);

                    if (Simulate()) { if (!isPart2) expenditure = Math.Min(expenditure, You.GoldSpent); }
                    else { if (isPart2) expenditure = Math.Max(expenditure, You.GoldSpent); }

                    You.ResetHitPoints();
                    Boss.ResetHitPoints();
                    foreach(ShopItem a in Armor)
                    {
                        You.RemoveArmor();
                        You.BuyArmor(a);

                        if (Simulate()) { if (!isPart2) expenditure = Math.Min(expenditure, You.GoldSpent); }
                        else { if (isPart2) expenditure = Math.Max(expenditure, You.GoldSpent); }

                        You.ResetHitPoints();
                        Boss.ResetHitPoints();

                        foreach(ShopItem r in Rings)
                        {
                            You.RemoveRings();
                            You.BuyRing(r);

                            if (Simulate()) { if (!isPart2) expenditure = Math.Min(expenditure, You.GoldSpent); }
                            else { if (isPart2) expenditure = Math.Max(expenditure, You.GoldSpent); }

                            You.ResetHitPoints();
                            Boss.ResetHitPoints();
                        }

                        foreach (IList<ShopItem> rings in twoRingCombinations)
                        {
                            You.RemoveRings();
                            You.BuyRing(rings[0]);
                            You.BuyRing(rings[1]);

                            if (Simulate()) { if (!isPart2) expenditure = Math.Min(expenditure, You.GoldSpent); }
                            else { if (isPart2) expenditure = Math.Max(expenditure, You.GoldSpent); }

                            You.ResetHitPoints();
                            Boss.ResetHitPoints();
                        }
                    }
                }

                if(!isPart2) QueuedConsole.WriteImmediate("Least expenditure to win the fight : {0}", expenditure);
                else QueuedConsole.WriteImmediate("Maximum expenditure and still lose the fight : {0}", expenditure);
            }

            internal static bool Simulate()
            {
                while (true)
                {
                    if (!You.IsDead()) You.Attack(Boss);
                    else return false;
                    if (!Boss.IsDead()) Boss.Attack(You);
                    else return true;
                }
            }
        }

        internal class Day22
        {
            internal class Player
            {
                internal bool IsYou { get; set; }
                internal int HitPoints { get; set; }
                internal int Damage { get; set; }
                internal int ManaAvailable { get; set; }
                internal int ManaSpent { get; set; }

                internal bool Attack(Player p)
                {
                    if (IsYou) HitPoints += Spells.Where(x => !x.IsExpired).Sum(x => x.Heal);
                    if (HitPoints <= 0) return false;
                    p.TakeHit(this);
                    if (IsYou) Spells.Where(x=>!x.IsExpired).ForEach(x => x.Cast());
                    
                    if (IsYou) {
                        var spells = Spells.Where(x => !x.IsExpired).ToList();
                        Spells.Clear();
                        Spells = new List<Spell>(spells);
                    }
                    return true;
                }

                private void TakeHit(Player p)
                {
                    if (IsYou) HitPoints += Spells.Where(x => !x.IsExpired).Sum(x => x.Heal);

                    if (p.HitPoints <= 0) return;
                    if (HitPoints <= 0) return;

                    int diff = 0;
                    if(IsYou) diff = Spells.Where(x=> !x.IsExpired).Sum(x=> x.Damage);
                    else diff = Damage - Spells.Where(x => !x.IsExpired).Sum(x => x.Armor);
                    if (diff <= 0) diff = 1;

                    HitPoints -= diff;

                    if (IsYou) Spells.Where(x => x.IsEffect && !x.IsExpired).ForEach(x => x.Cast());

                    if (IsYou) {
                        var spells = Spells.Where(x => !x.IsExpired).ToList();
                        Spells.Clear();
                        Spells = new List<Spell>(spells);
                    }
                }

                List<Spell> Spells { get; set; }
                internal bool BuySpell(Spell s)
                {
                    if (Spells == null) Spells = new List<Spell>();

                    Spells.Add(s);
                    ManaSpent += s.ManaCost;
                    return true;
                }

                internal bool RemoveSpells()
                {
                    if (Spells == null) return false;

                    Spells.Clear();
                    return true;
                }

                internal bool IsDead()
                {
                    return HitPoints <= 0;
                }

                internal void Reset()
                {
                    if (IsYou) {
                        ManaAvailable = 500;
                        ManaSpent = 0;
                    }
                    HitPoints = 100;
                }
            }

            internal class Spell
            {
                internal string Name { get; set; }
                internal int ManaCost { get; set; }
                internal int ManaRecharge { get; set; }
                internal int Damage { get; set; }
                internal int Armor { get; set; }
                internal int Heal { get; set; }
                internal bool IsExpired { get; set; }
                internal bool IsEffect { get; set; }
                internal int Turns { get; set; }
                internal int TurnsCompleted { get; set; }

                internal void Cast()
                {
                    if (!IsExpired) {
                        TurnsCompleted++;
                        if (Turns == TurnsCompleted) { IsExpired = true; }
                    }
                }
            }

            static Player Boss = new Player() { HitPoints = 71, Damage = 10, IsYou = false };
            static Player You = new Player() { HitPoints = 50, ManaAvailable = 500, IsYou = true };

            static List<Spell> Spells = new List<Spell>();
            static void Init() {
                Spells.Add(new Spell() { Name = "Magic Missile", Damage = 4, Turns = 1, IsEffect = false });
                Spells.Add(new Spell() { Name = "Drain", ManaCost = 73, Damage = 2, Heal = 2, Turns = 1, IsEffect = false });
                Spells.Add(new Spell() { Name = "Shield", ManaCost = 113, Turns = 6, Armor = 7, IsEffect = true });
                Spells.Add(new Spell() { Name = "Poison", ManaCost = 173, Turns = 6, Damage = 3, IsEffect = true });
                Spells.Add(new Spell() { Name = "Recharge", ManaCost = 229, Turns = 5, ManaRecharge = 101, IsEffect = true });
            }
        }

        internal class Day22_Copied
        {
            internal static void Main()
            {
                var queueOfStates = new Queue<GameState>();
                queueOfStates.Enqueue(new GameState(true));

                var spells = new[] {
                    Spell.Create("Magic Missle", 53, damage: 4),
                    Spell.Create("Drain", 73, damage: 2, heal: 2),
                    Spell.Create("Shield", 113, armour: 7, duration: 6),
                    Spell.Create("Poison", 173, damage: 3, duration: 6),
                    Spell.Create("Recharge", 229, manaCharge: 101, duration: 5)
                };

                var bestGame = default(GameState);
                var roundProcessed = 0;

                while (queueOfStates.Count > 0)
                {
                    if (queueOfStates.Peek().RoundNumber > roundProcessed)
                    {
                        ++roundProcessed;
                        QueuedConsole.WriteImmediate("Finished round {0}...", roundProcessed);
                    }

                    var gameState = queueOfStates.Dequeue();
                    if (bestGame != null && gameState.TotalManaSpent >= bestGame.TotalManaSpent) continue;

                    foreach (var spell in spells.Except(gameState.ActiveSpells.Keys).Where(x => gameState.PlayerMana >= x.Mana))
                    {
                        var newGameState = new GameState(gameState);
                        var result = newGameState.TakeTurn(spell);
                        if (result == GameResult.Continue)
                        {
                            queueOfStates.Enqueue(newGameState);
                        }
                        else if (result == GameResult.Win)
                        {
                            if (bestGame == null || newGameState.TotalManaSpent < bestGame.TotalManaSpent)
                            {
                                bestGame = newGameState;
                            }
                        }
                    }
                }

                QueuedConsole.WriteImmediate("Least mana spent to win : {0}", bestGame.TotalManaSpent);
            }

            internal class Spell
            {
                private Spell() { }

                public static Spell Create(string name, int mana, int damage = 0, int heal = 0, int armour = 0, int manaCharge = 0, int duration = 0)
                {
                    return new Spell { Name = name, Mana = mana, Damage = damage, Heal = heal, Armour = armour, ManaCharge = manaCharge, Duration = duration };
                }

                public string Name { get; private set; }
                public int Mana { get; private set; }
                public int Duration { get; private set; }
                public int Damage { get; private set; }
                public int Heal { get; private set; }
                public int Armour { get; private set; }
                public int ManaCharge { get; private set; }
            }

            internal enum GameResult { Win, Loss, Continue }

            internal class GameState
            {
                public GameState(bool hardMode)
                {
                    HardMode = hardMode;
                    RoundNumber = 0;
                    TotalManaSpent = 0;
                    PlayerHealth = 50 - (hardMode ? 1 : 0);
                    PlayerMana = 500;
                    BossHealth = 71;
                    BossAttack = 10;
                    ActiveSpells = new Dictionary<Spell, int>();
                }

                public GameState(GameState g)
                {
                    HardMode = g.HardMode;
                    RoundNumber = g.RoundNumber;
                    TotalManaSpent = g.TotalManaSpent;
                    PlayerHealth = g.PlayerHealth;
                    PlayerMana = g.PlayerMana;
                    BossHealth = g.BossHealth;
                    BossAttack = g.BossAttack;
                    ActiveSpells = new Dictionary<Spell, int>(g.ActiveSpells);
                }

                public bool HardMode { get; private set; }
                public int RoundNumber { get; set; }
                public int TotalManaSpent { get; set; }
                public int PlayerHealth { get; set; }
                public int PlayerMana { get; set; }
                public int BossHealth { get; set; }
                public int BossAttack { get; set; }
                public Dictionary<Spell, int> ActiveSpells { get; set; }

                internal GameResult TakeTurn(Spell spell)
                {
                    ++RoundNumber;

                    // Middle of my turn (no active spells at start!)
                    CastSpell(spell);

                    // Boss turn
                    ProcessActiveSpells();
                    if (BossHealth <= 0) return GameResult.Win;

                    PlayerHealth -= Math.Max(1, BossAttack - ActiveSpells.Sum(x => x.Key.Armour));
                    if (PlayerHealth <= 0) return GameResult.Loss;

                    // Beginning of next turn
                    if (HardMode)
                    {
                        PlayerHealth -= 1;
                        if (PlayerHealth <= 0) return GameResult.Loss;
                    }

                    ProcessActiveSpells();
                    if (BossHealth <= 0) return GameResult.Win;

                    return GameResult.Continue;
                }

                void CastSpell(Spell spell)
                {
                    TotalManaSpent += spell.Mana;
                    PlayerMana -= spell.Mana;
                    if (spell.Duration == 0) ProcessSpell(spell);
                    else ActiveSpells.Add(spell, spell.Duration);
                }

                void ProcessActiveSpells()
                {
                    foreach(Spell spell in ActiveSpells.Keys)
                    {
                        ProcessSpell(spell);
                    }
                    ActiveSpells.ToList().ForEach(x => {
                        if (x.Value == 1) ActiveSpells.Remove(x.Key);
                        else ActiveSpells[x.Key] = x.Value - 1;
                    });
                }

                void ProcessSpell(Spell spell)
                {
                    BossHealth -= spell.Damage;
                    PlayerHealth += spell.Heal;
                    PlayerMana += spell.ManaCharge;
                }
            }
        }

        internal class Day23
        {
            internal enum InstructionType
            {
                Increment,
                Half,
                Triple,
                Jump,
                JumpIfOne,
                JumpIfEven,
                Nothing
            }

            internal enum RegisterType
            {
                a, b, Nothing
            }

            internal class Instruction
            {
                internal InstructionType Type { get; set; }
                internal int Value { get; set; }
                private RegisterType _rType = RegisterType.Nothing;
                internal RegisterType RType { get { return _rType; } set { _rType = value; } }
            }

            internal static class Registers
            {
                internal static int a { get; set; }
                internal static int b { get; set; }
            }

            static Dictionary<int, Instruction> Instructions = new Dictionary<int, Instruction>();
            static void Init()
            {
                using (var stream = Utility.GetEmbeddedResourceStream("AdventOfCode.Data.Day23.txt", true))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string s = string.Empty;
                        char[] commaArr = new char[] { ',' };
                        char[] singleSpaceArr = new char[] { ' ' };
                        string aRegister = "a";
                        string bRegister = "b";
                        int iCount = 0;
                        while ((s = sr.ReadLine()) != null)
                        {
                            iCount++;
                            if (s.IndexOf(",") >= 0) {
                                string[] spArray1 = s.Split(commaArr, StringSplitOptions.RemoveEmptyEntries);
                                Instruction i = new Instruction() { Value = int.Parse(spArray1[1].Replace(" ", "")) };
                                string[] spArrJ = spArray1[0].Split(singleSpaceArr, StringSplitOptions.RemoveEmptyEntries);

                                if (spArrJ[0].IndexOf("jio") >= 0) i.Type = InstructionType.JumpIfOne;
                                else if (spArrJ[0].IndexOf("jie") >= 0) i.Type = InstructionType.JumpIfEven;

                                if (spArrJ[1].Equals(aRegister)) i.RType = RegisterType.a;
                                else if (spArrJ[1].Equals(bRegister)) i.RType = RegisterType.b;
                                Instructions.Add(iCount, i);
                            } else {
                                string[] spArrJ = s.Split(singleSpaceArr, StringSplitOptions.RemoveEmptyEntries);
                                Instruction i = new Instruction();
                                if (spArrJ[0].Equals("jmp")) {
                                    i.RType = RegisterType.Nothing;
                                    i.Type = InstructionType.Jump;
                                    i.Value = int.Parse(spArrJ[1]);
                                } else if (spArrJ[0].Equals("inc")) {
                                    i.Type = InstructionType.Increment;
                                    if (spArrJ[1].Equals(aRegister)) i.RType = RegisterType.a;
                                    else if (spArrJ[1].Equals(bRegister)) i.RType = RegisterType.b;
                                } else if (spArrJ[0].Equals("hlf")) {
                                    i.Type = InstructionType.Half;
                                    if (spArrJ[1].Equals(aRegister)) i.RType = RegisterType.a;
                                    else if (spArrJ[1].Equals(bRegister)) i.RType = RegisterType.b;
                                } else if (spArrJ[0].Equals("tpl")) {
                                    i.Type = InstructionType.Triple;
                                    if (spArrJ[1].Equals(aRegister)) i.RType = RegisterType.a;
                                    else if (spArrJ[1].Equals(bRegister)) i.RType = RegisterType.b;
                                }
                                Instructions.Add(iCount, i);
                            }
                        }
                    }
                }
            }

            internal static void Solve()
            {
                Init();
                int n = 1;
                Registers.a = 1;
                while (true)
                {
                    if (!Instructions.ContainsKey(n)) break;
                    Instruction ins = Instructions[n];
                    Execute(ins, ref n);
                }

                QueuedConsole.WriteImmediate("Register b : {0}", Registers.b);
            }

            static void Execute(Instruction ins, ref int n)
            {
                if(ins.Type == InstructionType.Half) {
                    if (ins.RType == RegisterType.a) Registers.a /= 2;
                    else if (ins.RType == RegisterType.b) Registers.b /= 2;
                    n++;
                } else if (ins.Type == InstructionType.Increment) {
                    if (ins.RType == RegisterType.a) Registers.a += 1;
                    else if (ins.RType == RegisterType.b) Registers.b += 1;
                    n++;
                } else if (ins.Type == InstructionType.Triple) {
                    if (ins.RType == RegisterType.a) Registers.a *= 3;
                    else if (ins.RType == RegisterType.b) Registers.b *= 3;
                    n++;
                } else if (ins.Type == InstructionType.Jump) {
                    n+= ins.Value;
                } else if (ins.Type == InstructionType.JumpIfOne) {
                    if (ins.RType == RegisterType.a) { if (Registers.a == 1) { n += ins.Value; } else { n++; } }
                    else if (ins.RType == RegisterType.b) { if (Registers.b == 1) { n += ins.Value; } else { n++; } }
                } else if (ins.Type == InstructionType.JumpIfEven) {
                    if (ins.RType == RegisterType.a) { if (Registers.a % 2 == 0) { n += ins.Value; } else { n++; } }
                    else if (ins.RType == RegisterType.b) { if (Registers.b % 2 == 0) { n += ins.Value; } else { n++; } }
                }
            }
        }

        internal class Day24
        {
            static List<int> Input = new List<int> { 1, 2, 3, 7, 11, 13, 17, 19, 23, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113 };

            static void Init()
            {
                QueuedConsole.WriteImmediate("Total sum : {0}, Is it divisible by 3? : {1}", Input.Sum(), Input.Sum() % 3 == 0);
                QueuedConsole.WriteImmediate("Total count : {0}", Input.Count());
                QueuedConsole.WriteImmediate("1/3 sum : {0}", Input.Sum() / 3);
                SumUp(Input, Input.Sum() / 3);
                //QueuedConsole.WriteImmediate("Possible subsets : {0}", (GetSubsets(Input.Sum() / 3)).Count());
            }

            internal static void Solve()
            {
                Init();
            }

            static void SumUp(List<int> numbers, int target)
            {
                SumUpRecursive(numbers, target, new List<int>());
            }

            static void SumUpRecursive(List<int> numbers, int target, List<int> partial)
            {
                int s = 0;
                foreach (int x in partial) s += x;

                if (s == target) QueuedConsole.WriteImmediate("sum(" + string.Join(",", partial.ToArray()) + ")=" + target);

                if (s >= target) return;

                for (int i = 0; i < numbers.Count; i++)
                {
                    List<int> remaining = new List<int>();
                    int n = numbers[i];
                    for (int j = i + 1; j < numbers.Count; j++) remaining.Add(numbers[j]);

                    List<int> partial_rec = new List<int>(partial);
                    partial_rec.Add(n);
                    SumUpRecursive(remaining, target, partial_rec);
                }
            }

            static IEnumerable<IEnumerable<int>> GetSubsets(int sum)
            {
                List<int> list = Input.ToList();
                int length = list.Count;
                long max = (long)Math.Pow(2, list.Count);
                HashSet<string> set = new HashSet<string>();
                for (long count = 0; count < max; count++)
                {
                    List<int> subset = new List<int>();
                    uint rs = 0;
                    while (rs < length)
                    {
                        if ((count & (1u << (int)rs)) > 0)
                        {
                            subset.Add(list[(int)rs]);
                            if (subset.Sum() > sum) break;
                        }
                        rs++;
                    }
                    if (subset.Sum() != sum) continue;
                    if (set.Add(string.Join("#", subset)))
                        yield return subset;
                }
            }
        }

        internal class Day24_Copied
        {
            static List<int> Input = new List<int> { 1, 2, 3, 7, 11, 13, 17, 19, 23, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113 };

            internal static void Solve()
            {
                var sum = Input.Sum();
                var part1 = MinProduct(Input, sum / 3, 3, 0, 1, 0, 0, BigInteger.MinusOne);
                var part2 = MinProduct(Input, sum / 4, 4, 0, 1, 0, 0, BigInteger.MinusOne);
                Console.WriteLine("Part1: " + part1);
                Console.WriteLine("Part2: " + part2);
            }

            static BigInteger MinProduct(List<int> inputs, int weightrequirement, int partitions, int index, BigInteger cumulativeproduct, int cumulativeweight, int used, BigInteger bestresult)
            {
                if (bestresult != BigInteger.MinusOne && cumulativeproduct >= bestresult) return bestresult;
                if (cumulativeweight == weightrequirement)
                {
                    if (CanPartitionRest(inputs, weightrequirement, partitions - 1, 0, 0, used))
                    {
                        return cumulativeproduct;
                    }
                    return BigInteger.MinusOne;
                }
                if (index >= inputs.Count || cumulativeweight > weightrequirement)
                {
                    return BigInteger.MinusOne;
                }
                var lhs = MinProduct(inputs, weightrequirement, partitions, index + 1, cumulativeproduct * inputs[index], cumulativeweight + inputs[index], used | (1 << index), bestresult);
                if (bestresult == BigInteger.MinusOne)
                {
                    bestresult = lhs;
                }
                else if (lhs != BigInteger.MinusOne)
                {
                    bestresult = BigInteger.Min(lhs, bestresult);
                }
                var rhs = MinProduct(inputs, weightrequirement, partitions, index + 1, cumulativeproduct, cumulativeweight, used, bestresult);
                if (lhs == BigInteger.MinusOne) return rhs;
                if (rhs == BigInteger.MinusOne) return lhs;
                return BigInteger.Min(lhs, rhs);
            }

            private static bool CanPartitionRest(List<int> inputs, int weightrequirement, int needed, int index, int cumulativeweight, int used)
            {
                if (cumulativeweight > weightrequirement) return false;
                if (index >= inputs.Count) return false;
                if (needed == 0)
                    return used == ((1 << inputs.Count) - 1);
                if (cumulativeweight == weightrequirement) return CanPartitionRest(inputs, weightrequirement, needed - 1, 0, 0, used);
                if ((used & (1 << index)) != 0) return CanPartitionRest(inputs, weightrequirement, needed, index + 1, cumulativeweight, used);
                return CanPartitionRest(inputs, weightrequirement, needed, index + 1, cumulativeweight + inputs[index], used | (1 << index))
                        || CanPartitionRest(inputs, weightrequirement, needed, index + 1, cumulativeweight, used);
            }

        }

        internal class Day25
        {
            static class Input {
                internal static int Row = 2981;
                internal static int Col = 3075;
                static int? _n = null;
                internal static int N { get { if (!_n.HasValue) { _n = GetNValue(); } return _n.Value; } }
            }
            internal static void Solve()
            {
                int n = 1;
                long code = 20151125;
                int mul = 252533, d = 33554393;
                while (true)
                {
                    code = code * mul % d;
                    n++;
                    if (n == Input.N) break;
                }

                QueuedConsole.WriteImmediate("Code : {0}", code);
            }

            internal static int GetNValue()
            {
                int n = 0;
                int startCol = 1, curCol = 0;
                int startRow = 1, curRow = 0;
                bool found = false;
                while (true)
                {
                    curCol = 0;
                    curRow = 0;
                    while (true)
                    {
                        if (curCol == 0) curCol = startCol;
                        else curCol++;
                        if (curRow == 0) curRow = startRow;
                        else curRow--;
                        if (curRow <= 0) break;

                        n++;

                        if (curCol == Input.Col && curRow == Input.Row) { found = true; break; }
                    }
                    if (found) break;
                    else startRow++;
                }

                return n;
            }
        }
    }
}