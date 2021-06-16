using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace GenericDefs.Functions.Algorithms
{
    public class ShuffleCollections
    {
        private static Random rand = new Random();
        public static List<T> Shuffle<T>(IList<T> list)
        {
            List<T> retVal = list.ToList();
            int n = retVal.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = retVal[k];
                retVal[k] = retVal[n];
                retVal[n] = value;
            }
            return retVal;
        }

        public static List<T> CryptoShuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            List<T> retVal = list.ToList();
            int n = retVal.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = retVal[k];
                retVal[k] = retVal[n];
                retVal[n] = value;
            }

            return retVal;
        }
    }
}