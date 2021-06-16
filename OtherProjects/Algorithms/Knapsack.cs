using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherProjects.Algorithms
{
    public class Knapsack
    {
        public void Solve()
        {
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

            int capacity = 99;

            ItemCollection[] ic = new ItemCollection[capacity + 1];

            for (int i = 0; i <= capacity; i++) ic[i] = new ItemCollection();

            for (int i = 0; i < items.Count; i++)
                for (int j = capacity; j >= 0; j--)
                    if (j >= items[i].Weight)
                    {
                        int quantity = Math.Min(items[i].Quantity, j / items[i].Weight);
                        for (int k = 1; k <= quantity; k++)
                        {
                            ItemCollection lighterCollection = ic[j - k * items[i].Weight];
                            int testValue = lighterCollection.TotalValue + k * items[i].Value;
                            if (testValue > ic[j].TotalValue) (ic[j] = lighterCollection.Copy()).AddItem(items[i], k);
                        }
                    }

            Console.WriteLine("Knapsack Capacity: " + capacity + ", Filled Weight: " + ic[capacity].TotalWeight + ", Filled Value: " + ic[capacity].TotalValue);

            foreach (KeyValuePair<string, int> kvp in ic[capacity].Contents)
            {
                Console.WriteLine(kvp.Key + " : " + kvp.Value + "");
            }
            Console.ReadKey();
        }

        private class Item
        {
            public string Description;
            public int Weight;
            public int Value;
            public int Quantity;

            public Item(string description, int weight, int value, int quantity)
            {
                Description = description;
                Weight = weight;
                Value = value;
                Quantity = quantity;
            }
        }

        private class ItemCollection
        {
            public Dictionary<string, int> Contents = new Dictionary<string, int>();
            public int TotalValue;
            public int TotalWeight;

            public void AddItem(Item item, int quantity)
            {
                if (Contents.ContainsKey(item.Description)) Contents[item.Description] += quantity; else Contents[item.Description] = quantity;
                TotalValue += quantity * item.Value;
                TotalWeight += quantity * item.Weight;
            }

            public ItemCollection Copy()
            {
                var ic = new ItemCollection();
                ic.Contents = new Dictionary<string, int>(this.Contents);
                ic.TotalValue = this.TotalValue;
                ic.TotalWeight = this.TotalWeight;
                return ic;
            }
        }

    }
}