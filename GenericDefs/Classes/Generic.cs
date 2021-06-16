using System;
using System.Collections.Generic;

namespace GenericDefs.Classes
{
    public class Generic
    {
    }

    public class CoefficientsAi
    {
        public class Ai
        {
            public int i;
            public double value;
        }
        Dictionary<int, Ai> Coefficients;

        public CoefficientsAi() { Coefficients = new Dictionary<int, Ai>(); }

        public bool HasCoefficient(int i)
        {
            if (Coefficients.ContainsKey(i)) { return true; }
            return false;
        }
        public double? GetCoefficient(int i)
        {
            if (Coefficients.ContainsKey(i)) { return (Coefficients[i]).value; }
            return null;
        }
        public void AddCoefficient(int i, double val)
        {
            if (HasCoefficient(i)) throw new ArgumentException("Coefficient already found : " + i);
            Coefficients.Add(i, new Ai() { i = i, value = val });
        }
    }

    public class IntegralCoefficientsAi
    {
        public class Ai
        {
            public int i;
            public int value;
        }
        Dictionary<int, Ai> Coefficients;

        public IntegralCoefficientsAi() { Coefficients = new Dictionary<int, Ai>(); }

        public bool HasCoefficient(int i)
        {
            if (Coefficients.ContainsKey(i)) { return true; }
            return false;
        }
        public int? GetCoefficientValue(int i)
        {
            if (Coefficients.ContainsKey(i)) { return (Coefficients[i]).value; }
            return null;
        }
        public void AddCoefficient(int i, int val)
        {
            if (HasCoefficient(i)) throw new ArgumentException("Coefficient already found : " + i);
            Coefficients.Add(i, new Ai() { i = i, value = val });
        }
        public void ChangeCoefficient(int i, int newVal)
        {
            if (!HasCoefficient(i)) throw new ArgumentException("Coefficient not found : " + i);
            Ai a = Coefficients[i];
            a.value = newVal;
        }
    }

    /// <summary>
    /// (a,b,c) represents a unique solution. +/- a, +/- b, +/- c are repeats so are (a,c,b) (c,a,b) and so on.
    /// </summary>
    public class UniquePairedIntegralSolutions<T>
    {
        HashSet<string> Set = new HashSet<string>();
        List<T[]> Solutions;
        string Delimiter;

        public UniquePairedIntegralSolutions(string delimiter = "$#")
        {
            Solutions = new List<T[]>();
            if (!string.IsNullOrEmpty(delimiter)) Delimiter = delimiter;
        }

        public bool CheckIfUnique(T[] t)
        {
            if (Set.Count == 0) return true;

            bool isUnique = true;
            if (Solutions.Count == 0) return isUnique;

            Array.Sort(t);
            string key = GetHashKey(t);

            if (Set.Contains(key)) return false;
            return true;
        }

        public void AddIfUnique(T[] t)
        {
            Array.Sort(t);
            if (CheckIfUnique(t))
            {
                Solutions.Add(t);
                string hash = GetHashKey(t);
                Set.Add(hash);
            }
        }

        private string GetHashKey(T[] l)
        {
            string key = string.Empty;
            foreach (var x in l)
            {
                key += x + Delimiter;
            }

            return key;
        }

        public List<T[]> GetUniqueSolutions() { return Solutions; }
    }
}