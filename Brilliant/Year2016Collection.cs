using GenericDefs.Classes.NumberTypes;
using GenericDefs.DotNet;
using Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using GDF = GenericDefs.Functions;

namespace Brilliant
{
    public class Year2016Collection : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/");
        }

        void ISolve.Solve()
        {
            MoreProblemsIn2016Part9();
        }

        /// <summary>
        /// https://brilliant.org/problems/2016-is-absolutely-awesome-17/
        /// </summary>
        internal static void _17()
        {
            System.Numerics.BigInteger b1 = 1;
            System.Numerics.BigInteger b2 = 1;
            System.Numerics.BigInteger b3 = 0;
            int k = 2;
            while (true)
            {
                k++;
                b3 = b1 + b2;
                if (b3.ToString().EndsWith("2016"))
                {
                    break;
                }
                b1 = b2;
                b2 = b3;
            }

            QueuedConsole.WriteFinalAnswer(string.Format("Final answer : {0}", k));
        }

        /// <summary>
        /// https://brilliant.org/problems/mobiusphi-and-2016/
        /// </summary>
        internal static void OddNumberMobiusAndPhi2016()
        {
            HashSet<int> set = new HashSet<int>();
            int n = 2016;
            while (true)
            {
                n++;
                if (GDF.EulerTotient.CalculateTotient(n) == 2016) set.Add(n);
                if (n == 32000)
                {
                    break;
                }
            }

            List<int> phi2016 = set.ToList();
            foreach (int t in phi2016)
            {
                if (!(GDF.NumberTheory.MobiusFunction.GetMobiusValue((long)t) == -1))
                {
                    set.Remove(t);
                    continue;
                }

                if (!GDF.MathFunctions.IsOdd(t))
                {
                    set.Remove(t);
                    continue;
                }
            }
            QueuedConsole.WriteFinalAnswer(string.Format("Final answer : {0}", set.Sum()));
        }

        /// <summary>
        /// https://brilliant.org/problems/more-problems-in-2016-part-4/
        /// </summary>
        internal static void MoreProblemsIn2016Part4()
        {
            int n = 7;
            int sum = 0;
            while (true)
            {
                sum += ((n + 1) * (n + 1) * (n + 1)) + (3 * (n + 1)) + 10;
                n++;
                if (n > 9) break;
            }
            
            QueuedConsole.WriteAndWaitOnce("f(8) + f(9) + f(10) : {0}", sum);
            HashSet<long> factors = GDF.Factors.GetAllFactors(sum);
            BigRational value = new BigRational(0, 1);
            foreach(long factor in factors)
            {
                value += new BigRational(1, GDF.EulerTotient.CalculateTotient(factor));
            }

            Fraction<long> fr = FractionConverter<long>.BigRationalToFraction(value);
            QueuedConsole.WriteFinalAnswer("p: {0}, q: {1}, (p-q)-214 : {2}", fr.N, fr.D, (fr.N - fr.D - 214));
        }

        /// <summary>
        /// https://brilliant.org/problems/more-problems-in-2016-part-9-fixed/
        /// </summary>
        internal static void MoreProblemsIn2016Part9()
        {
            long n = 17 * 23 * 41 * 43;
            long phin = GDF.EulerTotient.CalculateTotient(n);
            HashSet<long> factors = GDF.Factors.GetAllFactors(n);

            long phi1 = GDF.EulerTotient.CalculateTotient(factors.Count + factors.Sum(x => x));

            double value1 = 1; 
            foreach(long t in factors)
            {
                value1 /= GDF.EulerTotient.CalculateTotient(t);
            }
            value1 = Math.Log(value1);
            value1 /= Math.Log(phin);
            QueuedConsole.WriteImmediate("The required sum is : {0}", value1 + phi1);
            QueuedConsole.ReadKey();
        }
    }
}