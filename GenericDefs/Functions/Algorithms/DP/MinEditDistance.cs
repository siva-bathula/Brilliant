using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDefs.Functions.Algorithms.DP
{
    /// <summary>
    /// Calculates the minimum edit distance (Levenshtein) between two lists of items
    /// </summary>
    /// <typeparam name="T">Type of item (e.g. characters or string)</typeparam>
    public class MinEditDistance<T>
    {
        public MinEditDistance(List<T> list1, List<T> list2)
        {
            InsCost = 1;
            DelCost = 1;
            SubCost = 1;
            List1 = list1;
            List2 = list2;
        }

        List<T> List1;
        List<T> List2;

        Distance[,] D;

        // The costs of inserts, deletes, substitutions. Normally run SubCost with 2, others at 1
        public int InsCost { get; set; }
        public int DelCost { get; set; }
        public int SubCost { get; set; }

        /// <summary>
        /// Calculates the minimum edit distance using weights in InsCost etc.
        /// </summary>
        /// <returns></returns>
        public int CalcMinEditDistance()
        {
            if (List1 == null || List2 == null)
            {
                throw new ArgumentNullException();
            }
            if (List1.Count == 0 || List2.Count == 0)
            {
                throw new ArgumentException("Zero length list");
            }
            // prepend dummy value
            List1.Insert(0, default(T));
            List2.Insert(0, default(T));

            int lenList1 = List1.Count;
            int lenList2 = List2.Count;
            D = new Distance[lenList1, lenList2];
            for (int i = 0; i < lenList1; i++)
            {
                D[i, 0].val = DelCost * i;
                D[i, 0].backTrace = Direction.Left;
            }
            for (int j = 0; j < lenList2; j++)
            {
                D[0, j].val = InsCost * j;
                D[0, j].backTrace = Direction.Up;
            }
            for (int i = 1; i < lenList1; i++)
                for (int j = 1; j < lenList2; j++)
                {
                    int d1 = D[i - 1, j].val + DelCost;
                    int d2 = D[i, j - 1].val + InsCost;
                    int d3 = (EqualityComparer<T>.Default.Equals(List1[i], List2[j])) ? D[i - 1, j - 1].val : D[i - 1, j - 1].val + SubCost;
                    D[i, j].val = Math.Min(d1, Math.Min(d2, d3));
                    // back trace
                    if (D[i, j].val == d1)
                    {
                        D[i, j].backTrace |= Direction.Left;
                    }
                    if (D[i, j].val == d2)
                    {
                        D[i, j].backTrace |= Direction.Up;
                    }
                    if (D[i, j].val == d3)
                    {
                        D[i, j].backTrace |= Direction.Diag;
                    }
                }
            return D[lenList1 - 1, lenList2 - 1].val;
        }

        /// <summary>
        /// Returns alignment strings. The default value for T indicates an insertion or deletion in the appropriate string. align1 and align2 will
        /// have the same length padded with default(T) regardless of the input string lengths.
        /// </summary>
        /// <param name="align1">First string alignment</param>
        /// <param name="align2">Second string alignment</param>
        public void Alignment(out List<T> align1, out List<T> align2)
        {
            if (D == null)
            {
                throw new Exception("Distance matrix is null");
            }
            int i = List1.Count - 1;
            int j = List2.Count - 1;

            align1 = new List<T>();
            align2 = new List<T>();
            while (i > 0 || j > 0)
            {
                Direction dir = D[i, j].backTrace;
                int dVal, dDiag = int.MaxValue, dLeft = int.MaxValue, dUp = int.MaxValue;
                if ((dir & Direction.Diag) == Direction.Diag)
                {
                    // always favour diagonal as this is a match on items
                    dDiag = -1;
                }
                if ((dir & Direction.Up) == Direction.Up)
                {
                    dUp = D[i, j - 1].val;
                }
                if ((dir & Direction.Left) == Direction.Left)
                {
                    dLeft = D[i - 1, j].val;
                }
                dVal = Math.Min(dDiag, Math.Min(dLeft, dUp));
                if (dVal == dDiag)
                {
                    align1.Add(List1[i]);
                    align2.Add(List2[j]);
                    i--;
                    j--;
                }
                else if (dVal == dUp)
                {
                    align1.Add(default(T));
                    align2.Add(List2[j]);
                    j--;
                }
                else if (dVal == dLeft)
                {
                    align1.Add(List1[i]);
                    align2.Add(default(T));
                    i--;
                }
            }
            align1.Reverse();
            align2.Reverse();
        }

        /// <summary>
        /// Writes out the "D" matrix showing the edit distances and the back tracing directions
        /// </summary>
        public void Write()
        {
            int lenList1 = List1.Count;
            int lenList2 = List2.Count;
            Console.Write("      ");
            for (int i = 0; i < lenList1; i++)
            {
                if (i == 0)
                    Console.Write(string.Format("{0, 6}", "*"));
                else
                    Console.Write(string.Format("{0, 6}", List1[i]));
            }
            Console.WriteLine();
            for (int j = 0; j < lenList2; j++)
            {
                if (j == 0)
                    Console.Write(string.Format("{0, 6}", "*"));
                else
                    Console.Write(string.Format("{0, 6}", List2[j]));
                for (int i = 0; i < lenList1; i++)
                {
                    Console.Write(string.Format("{0,6:###}", D[i, j].val));
                }
                Console.WriteLine();
                Console.Write("      ");
                for (int i = 0; i < lenList1; i++)
                {
                    WriteBackTrace(D[i, j].backTrace);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Writes out one backtrace item
        /// </summary>
        /// <param name="d"></param>
        void WriteBackTrace(Direction d)
        {
            string s = string.Empty;
            s += ((d & Direction.Diag) == Direction.Diag) ? "\u2196" : "";
            s += ((d & Direction.Up) == Direction.Up) ? "\u2191" : "";
            s += ((d & Direction.Left) == Direction.Left) ? "\u2190" : "";
            Console.Write(string.Format("{0,6}", s));
        }

        /// <summary>
        /// Represents one cell in the 'D' matrix
        /// </summary>
        public struct Distance
        {
            public int val;
            public Direction backTrace;
        }

        /// <summary>
        /// Direction(s) for one cell in the 'D' matrix.
        /// </summary>
        [FlagsAttribute]
        public enum Direction
        {
            None = 0,
            Diag = 1,
            Left = 2,
            Up = 4
        }

        // example from Speach & Language Processing, Jurafsky & Martin P. 108, cites Krushkal (1983)
        [TestMethod]
        public void LevenshteinAltDistance()
        {
            string s1 = "EXECUTION";
            string s2 = "INTENTION";
            int m = Align(s1, s2, 2, 1, 1, "*EXECUTION", "INTE*NTION");
            Assert.AreEqual(8, m);
        }

        public static int Align(string s1, string s2, int subCost = 1, int delCost = 1, int insCost = 1, string alignment1 = null, string alignment2 = null)
        {
            List<char> l1 = s1.ToList();
            List<char> l2 = s2.ToList();
            MinEditDistance<char> med = new MinEditDistance<char>(l1, l2);
            List<char> a1, a2;
            med.SubCost = subCost;
            med.DelCost = delCost;
            med.InsCost = insCost;
            int m = med.CalcMinEditDistance();
            Console.WriteLine("Min edit distance: " + m);
            med.Write();

            med.Alignment(out a1, out a2);
            foreach (char c in a1)
            {
                if (c == default(char)) Console.Write("*");
                else Console.Write(c);
            }
            Console.WriteLine();
            foreach (char c in a2)
            {
                if (c == default(char)) Console.Write("*");
                else Console.Write(c);
            }
            Console.WriteLine();
            if (alignment1 != null && alignment2 != null)
            {
                Assert.AreEqual(alignment1, new string(a1.ToArray()).Replace('\0', '*'));
                Assert.AreEqual(alignment2, new string(a2.ToArray()).Replace('\0', '*'));
            }
            return m;
        }
    }
}
