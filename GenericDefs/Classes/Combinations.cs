using Combinatorics.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GenericDefs.Classes
{
    public class Combinations
    {
        public static BigInteger GetAllCombinations(ArrayList al) {
            BigInteger c = new BigInteger(1);
            int totalCount = 0;
            foreach (var v in al) {
                totalCount += (int)v;
            }

            while (totalCount >= 1) {
                c *= totalCount;
                totalCount--;
                if (totalCount == 1) break;
            }

            foreach (var v in al) {
                totalCount = (int)v;
                if (totalCount <= 1) continue;
                else {
                    while (totalCount >= 1)
                    {
                        c /= totalCount;
                        totalCount--;
                        if (totalCount == 1) break;
                    }
                }
            }

            return c;
        }

        public static BigInteger GetAllCombinations(int[] a)
        {
            BigInteger c = new BigInteger(1);
            int totalCount = 0;
            foreach (var v in a)
            {
                totalCount += v;
            }

            while (totalCount >= 1)
            {
                c *= totalCount;
                totalCount--;
                if (totalCount == 1) break;
            }

            foreach (var v in a)
            {
                totalCount = v;
                if (totalCount <= 1) continue;
                else {
                    while (totalCount >= 1)
                    {
                        c /= totalCount;
                        totalCount--;
                        if (totalCount == 1) break;
                    }
                }
            }

            return c;
        }
        
        public static List<IList<T>> GetAllCombinations<T>(IList<T> list, int length)
        {
            Combinations<T> c = new Combinations<T>(list, length, GenerateOption.WithoutRepetition);
            return c.ToList();
        }

        public static List<IList<T>> GetAllCombinations<T>(IList<T> list, int length, bool repetitionAllowed)
        {
            GenerateOption type = repetitionAllowed ? GenerateOption.WithRepetition : GenerateOption.WithoutRepetition;
            Combinations<T> c = new Combinations<T>(list, length, type);
            return c.ToList();
        }
    }
}