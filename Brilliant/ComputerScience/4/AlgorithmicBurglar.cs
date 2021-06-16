using GenericDefs.Classes;
using GenericDefs.Functions.Algorithms.DP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// Several vaults were broken into last night! Police have strong evidence to believe that the culprit was the infamous Algorithmic Burglar - an efficient and swift robber.
    ///7 different vaults were robbed.Police discovered that the vaults are missing the following amounts of metal     : 1739 lbs, 72 lbs, 212 lbs, 55 lbs, 511 lbs, 1239 lbs, and 99 lbs.
    ///Here are the weights and dollar values for the different bars in the vaults:
    ///Material Weight  Value
    ///Gold        2 lbs   $57
    ///Platinum    5 lbs   $191
    ///Silver      14 lbs  $417
    ///Expensivium 17 lbs  $231
    ///Rhodium     19 lbs  $741
    ///Osmium      13 lbs  $139
    ///Aluminum    1 lbs   $28
    ///Silicon     3 lbs   $117
    ///Iron        5 lbs   $13
    ///Titanium    11 lbs  $9
    ///Potassium   19 lbs  $18
    ///Before the robbery, each vault contained at least 2000 lbs of each metal. Assume the burglar stole whole bars, not fractions of a bar.
    ///Let V be the greatest possible value in dollars of the stolen material.What are the last three digits of V?
    /// </summary>
    public class AlgorithmicBurglar : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/algorithmic-burglar/");
        }

        void ISolve.Solve() {
            Solution2();
        }

        void Solution1()
        {
            List<Vault> vaults = new List<Vault>();
            vaults.Add(new Vault() { VaultNumber = 1, MissingWeight = 1739 });
            vaults.Add(new Vault() { VaultNumber = 2, MissingWeight = 72 });
            vaults.Add(new Vault() { VaultNumber = 3, MissingWeight = 212 });
            vaults.Add(new Vault() { VaultNumber = 4, MissingWeight = 55 });
            vaults.Add(new Vault() { VaultNumber = 5, MissingWeight = 511 });
            vaults.Add(new Vault() { VaultNumber = 6, MissingWeight = 1239 });
            vaults.Add(new Vault() { VaultNumber = 7, MissingWeight = 99 });

            long greatestTheft = 0;
            foreach (Vault v in vaults)
            {
                greatestTheft += MaximizeTheft(v);
            }

            Console.WriteLine("Greatest possible value of V is :: {0}", greatestTheft);
            Console.WriteLine("Last three digits of V is :: {0}", greatestTheft % 1000);
            Console.ReadKey();
        }

        void Solution2() {
            var items = new List<Item>(){
              new Item("Titanium", 11, 9, 2000),
              new Item("Potassium", 19, 18, 2000),
              new Item("Iron", 5, 13, 2000),
              new Item("Osmium", 13, 139, 2000),
              new Item("Expensivium", 17, 239, 2000),
              new Item("Aluminium", 1, 28, 2000),
              new Item("Gold", 2, 57, 2000),
              new Item("Silver", 14, 417, 2000),
              new Item("Platinum", 5, 191, 2000),
              new Item("Rhodium", 19, 741, 2000),
              new Item("Silicon", 3, 117, 2000)
            };


            List<Vault> vaults = new List<Vault>();
            vaults.Add(new Vault() { VaultNumber = 1, MissingWeight = 1739 });
            vaults.Add(new Vault() { VaultNumber = 2, MissingWeight = 72 });
            vaults.Add(new Vault() { VaultNumber = 3, MissingWeight = 212 });
            vaults.Add(new Vault() { VaultNumber = 4, MissingWeight = 55 });
            vaults.Add(new Vault() { VaultNumber = 5, MissingWeight = 511 });
            vaults.Add(new Vault() { VaultNumber = 6, MissingWeight = 1239 });
            vaults.Add(new Vault() { VaultNumber = 7, MissingWeight = 99 });

            long greatestTheft = 0;
            foreach (Vault v in vaults)
            {
                ItemCollection[] ic = BoundedKnapsack.Solve(v.MissingWeight, items);

                greatestTheft += ic[v.MissingWeight].TotalValue;
            }

            Console.WriteLine("Greatest possible value of V is :: {0}", greatestTheft);
            Console.WriteLine("Last three digits of V is :: {0}", greatestTheft % 1000);
            Console.ReadKey();
        }

        WeightedUniqueIntegralPairs<int> u;
        private long MaximizeTheft(Vault v)
        {
            long theftMax = 0, theft = 0;

            u = new WeightedUniqueIntegralPairs<int>();
            ArrayList b = new ArrayList(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int imax = (int)Math.Floor(1.0 * v.MissingWeight / Materials.m[0].Weight);
            for (int i = 1; i <= imax; i++)
            {
                int wLeft = v.MissingWeight - i * Materials.m[0].Weight;
                b[1] = i;


                Loop(wLeft, 2, ref b);
            }

            List<UniqueIntegralPairs.Combination> uc = u.GetCombinations();
            foreach (UniqueIntegralPairs.Combination c in uc)
            {
                theft = 0;
                for (int i = 1; i <= 11; i++)
                {
                    theft += Materials.m[i - 1].Value * (int)c.Pair[i];
                }
                if (theft > theftMax) theftMax = theft;
            }

            return theftMax;
        }

        private bool Loop(int wLeft, int depth, ref ArrayList before)
        {
            bool found = false;
            int wLeftAfter = wLeft;
            ArrayList after = new ArrayList();
            foreach (int b in before) { after.Add(b); }

            int imax = (int)Math.Floor(1.0 * wLeft / Materials.m[depth - 1].Weight);
            for (int i = 1; i <= imax; i++)
            {
                wLeftAfter -= i * Materials.m[depth - 1].Weight;
                after[depth] = i;
                if (wLeftAfter < 0) { found = false; break; }
                if (depth == 11)
                {
                    if (wLeftAfter == 0)
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }
                else
                {
                    found = Loop(wLeftAfter, depth + 1, ref after);
                }
            }

            if (found && depth == 11)
            {
                int weight = Enumerable.Range(1, 11).Sum(i => (int)after[i] * Materials.m[i - 1].Value);
                if (weight > u.MaximumCalculatedValue) {
                    u.RemoveAllCombinations();

                    u.MaximumCalculatedValue = weight;
                    u.AddCombination(after);
                }
            }

            return found;
        }

        private static class Materials
        {
            internal static List<MaterialBar> m;
            static Materials()
            {
                m = new List<MaterialBar>();
                m.Add(new MaterialBar() { Type = MaterialType.Titanium, Weight = 11, Value = 9});
                m.Add(new MaterialBar() { Type = MaterialType.Potassium, Weight = 19, Value = 18 });
                m.Add(new MaterialBar() { Type = MaterialType.Iron, Weight = 5, Value = 13 });
                m.Add(new MaterialBar() { Type = MaterialType.Osmium, Weight = 13, Value = 139 });
                m.Add(new MaterialBar() { Type = MaterialType.Expensivium, Weight = 17, Value = 231 });
                m.Add(new MaterialBar() { Type = MaterialType.Aluminium, Weight = 1, Value = 28 });
                m.Add(new MaterialBar() { Type = MaterialType.Gold, Weight = 2, Value = 57 });
                m.Add(new MaterialBar() { Type = MaterialType.Silver, Weight = 14, Value = 417 });
                m.Add(new MaterialBar() { Type = MaterialType.Platinum, Weight = 5, Value = 191 });
                m.Add(new MaterialBar() { Type = MaterialType.Rhodium, Weight = 19, Value = 741 });
                m.Add(new MaterialBar() { Type = MaterialType.Silicon, Weight = 3, Value = 117 });
            }
        }

        private class MaterialBar
        {
            internal MaterialType Type { get; set; }
            internal int Weight { get; set; }
            internal int Value { get; set; }
        }

        private class Vault
        {
            internal int MissingWeight { get; set; }
            internal int VaultNumber { get; set; }
        }

        private enum MaterialType
        {
            Gold = 1,
            Platinum,
            Silver,
            Expensivium,
            Rhodium,
            Osmium,
            Aluminium,
            Silicon,
            Iron,
            Titanium,
            Potassium
        }
    }
}