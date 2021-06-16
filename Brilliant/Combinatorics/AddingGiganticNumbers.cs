using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Classes;
namespace Brilliant.Combinatorics
{
    /// <summary>
    /// Adding Gigantic Numbers! Combinatorics Level 3 
    /// a+b+c = 10. Sum( 10!/a!b!c!).
    /// Let a,b,c be non-negative numbers.Find the value of the summation above.
    /// </summary>
    public class AddingGiganticNumbers : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/not-bad-4/");
        }

        void ISolve.Solve()
        {
            Dictionary<int, BigInteger> factorials = new Dictionary<int, BigInteger>();
            factorials.Add(0, 1);
            for (int i = 1; i <= 10; i++) {
                BigInteger fi = new BigInteger();
                fi = 1;
                for (int j = 1; j <= i; j++) {
                    fi *= j;
                }
                factorials.Add(i, fi);
            }

            UniqueIntegralPairs p = new UniqueIntegralPairs("$%#");
            for (int a = 0; a <= 10; a++) {
                for (int b = 0; b <= 10; b++)
                {
                    if (10 >= a + b)
                    {
                        int d = (10 - a - b); 
                        ArrayList l = new ArrayList();
                        l.Add(a); l.Add(b); l.Add(d);

                        p.AddCombination(l);
                    }
                }
            }

            BigInteger bSum = new BigInteger();
            bSum = 0;
            List<UniqueIntegralPairs.Combination> c = p.GetCombinations();
            foreach (UniqueIntegralPairs.Combination ci in c) {
                int csum = Convert.ToInt32("" + (factorials[10] / (factorials[(int)ci.Pair[0]] * factorials[(int)ci.Pair[1]] * factorials[(int)ci.Pair[2]])).ToString());
                Console.WriteLine("Combination :: {0}, {1}, {2} Sum :: {3}", (int)ci.Pair[0], (int)ci.Pair[1], (int)ci.Pair[2], csum);
                bSum += csum;
            }

            Console.WriteLine("Total sum :: {0}", bSum);
            Console.ReadKey();
        }
    }
}