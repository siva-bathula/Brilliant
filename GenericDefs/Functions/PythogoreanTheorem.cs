using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GenericDefs.Functions
{
    public class PythogoreanTheorem
    {
        public static void FindTriples(int maxValue, PythogoreanTriples t) {
            HashSet<int> primT = new HashSet<int>();
            if (t == null) t = new PythogoreanTriples(maxValue);
            int a = 0, b = 0, c = 0, product = 0;
            int maxSec = Convert.ToInt32(Math.Pow(maxValue, (1.0 / 3.0)));
            for (int i = 1; i <= maxValue; i++)
            {
                a = i;
                for (int j = 1; j <= maxValue; j++)
                {
                    b = j;
                    product = (i * i) + (j * j);
                    if (Math.Sqrt(product) % 1 == 0)
                    {
                        c = int.Parse(Math.Sqrt(product).ToString());
                        bool isSecondary = false;
                        int abc = a * b * c;
                        foreach (int pT in primT)
                        {
                            for (int k = 1; k <= maxSec; k++)
                            {
                                if (((double)abc / (k * k * k)) % 1.0 == 0.0)
                                {
                                    isSecondary = true;
                                }
                            }
                        }

                        if (isSecondary) { t.AddSecondaryTriple(a, b, c); }
                        else { primT.Add(a * b * c); t.AddPrimTriple(a, b, c); }
                    }
                }
            }
        }

        public static bool IsTriple(long a, long b, long c)
        {
            return ((a * a) + (b * b) == c * c);
        }
    }

    public class PythogoreanTriples
    {
        public int MaxValue { get; set; }
        public bool IsEmpty { get; set; }

        public PythogoreanTriples(int max)
        {
            MaxValue = max;
            PrimaryTriples = new List<Triple>();
            SecondaryTriples = new List<Triple>();
            PythogoreanTheorem.FindTriples(max, this);
        }

        public class Triple
        {
            public int a, b, c;
        }

        List<Triple> PrimaryTriples;
        List<Triple> SecondaryTriples;
        List<Triple> AllTriples;
        public void AddPrimTriple(int a, int b, int c)
        {
            Triple t1 = new Triple() { a = a, b = b, c = c };
            PrimaryTriples.Add(t1);
        }

        public void AddSecondaryTriple(int a, int b, int c)
        {
            Triple t2 = new Triple() { a = a, b = b, c = c };
            SecondaryTriples.Add(t2);
        }

        public List<Triple> GetPrimaryTriples()
        {
            return PrimaryTriples;
        }

        public List<Triple> GetSecondaryTriples()
        {
            return SecondaryTriples;
        }

        public List<Triple> GetAllTriples()
        {
            if (AllTriples == null || AllTriples.Count == 0)
            {
                AllTriples = new List<Triple>();
                AllTriples = PrimaryTriples.Concat(SecondaryTriples).ToList();
            }

            return AllTriples;
        }
    }

    public class PrimitiveTriples
    {
        static List<List<int>> _triples = null;
        static object _syncLock = new object();
        private static List<List<int>> LoadFromFile()
        {
            if (_triples == null)
            {
                lock (_syncLock)
                {
                    if (_triples == null)
                    {
                        _triples = new List<List<int>>();
                        string text = string.Empty;
                        string fName = StreamHelper.GetSolutionPath() + "/Data/PythagoreanTriples/PrimitiveTriples.txt";
                        using (var stream = File.Open(fName, FileMode.Open))
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                while ((text = sr.ReadLine()) != null)
                                {
                                    string[] spArray = text.Split(StringSplitter.Space, StringSplitOptions.RemoveEmptyEntries);
                                    List<int> triple = new List<int>() { int.Parse(spArray[1]), int.Parse(spArray[2]), int.Parse(spArray[3]) };
                                    triple.Sort();
                                    if (PythogoreanTheorem.IsTriple(triple[0], triple[1], triple[2])) _triples.Add(triple);
                                }
                            }
                        }
                    }
                }
            }

            return _triples;
        }

        private static List<List<int>> GetTriples()
        {
            if (_triples != null) return _triples;
            return LoadFromFile();
        }

        public static List<List<int>> GetPrimitiveTriples()
        {
            return GetTriples();
        }

        /// <summary>
        /// Max. value of hypotenuse.
        /// </summary>
        /// <param name="hMax"></param>
        /// <returns></returns>
        public static List<List<int>> GetPrimitiveTriples(int hMax)
        {
            List<List<int>> triples = GetTriples();
            return (triples.Where(x => { return x[2] <= hMax; })).ToList();
        }
    }
}