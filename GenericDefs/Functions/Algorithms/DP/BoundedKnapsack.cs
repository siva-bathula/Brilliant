using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.Algorithms.DP
{
    public class BoundedKnapsack
    {
        public static ItemCollection[] Solve(int capacity, List<Item> items)
        {
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

            //Console.WriteLine("Knapsack Capacity: " + capacity + ", Filled Weight: " + ic[capacity].TotalWeight + ", Filled Value: " + ic[capacity].TotalValue);

            //foreach (KeyValuePair<string, int> kvp in ic[capacity].Contents)
            //{
            //    Console.WriteLine(kvp.Key + " : " + kvp.Value + "");
            //}
            //Console.ReadKey();

            return ic;
        }
    }
}