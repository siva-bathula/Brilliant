using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs;
using GenericDefs.Classes;
using GenericDefs.Functions;
using System.Collections;

namespace Brilliant.ComputerScience
{
    public class AllBuzzkillForJust1NumberPart2 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/all-buzzkill-for-one-no-part-2/");
        }

        void ISolve.Solve()
        {
            Solve4();
        }

        /// <summary>
        /// Let 1 l.t. p,q  l.t. 1000 , such that p,q are both prime numbers with phi(p) is the sum of all quadratic residues of q and if for largest p,  
        /// q = 10A + B, where A, B are positive integers. A is the third last digit of my number and B is the last digit of my number.
        /// </summary>
        void Solve4()
        {
            List<long> primes = KnownPrimes.GetAllKnownPrimes().GetRange(0, 1000);
            Dictionary<long, long> phiP = new Dictionary<long, long>();

            foreach (long p in primes)
            {
                if (p > 1000) break;
                if (!phiP.ContainsKey(p)) phiP.Add(p, EulerTotient.CalculateTotient(p));
            }

            Dictionary<long, long> qQR = new Dictionary<long, long>();
            foreach (long q in primes)
            {
                if (q > 1000) break;

                int qrSum = (int)QuadraticResidue.GetAllQuadraticResidues(q).Sum(x => x);
                if (!qQR.ContainsKey(q)) qQR.Add(q, qrSum);
            }

            Dictionary<long, long> pqs = new Dictionary<long, long>();

            foreach (KeyValuePair<long, long> kvpq in qQR)
            {
                foreach (KeyValuePair<long, long> kvp in phiP)
                {
                    if (kvpq.Value == kvp.Value)
                    {
                        if (!pqs.ContainsKey(kvp.Key))
                        {
                            pqs.Add(kvp.Key, kvpq.Key);
                        }
                        else
                        {
                            if (pqs[kvp.Key] < kvpq.Key)
                            {
                                pqs[kvp.Key] = kvpq.Key;
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<long, long> pq in pqs)
            {
                Console.WriteLine("p: {0}, q: {1}", pq.Key, pq.Value);
            }
            Console.ReadKey();
        }

        /// <summary>
        /// The 7th smallest positive integer p which is 2 less than a prime number and whose sum of quadratic residue is 1 less than a prime number, 
        /// then reversing digits of p gives you the next 2 digits of my number.
        /// </summary>
        void Solve3() {
            int i = 1, pCount = 0;
            List<long> primes = KnownPrimes.GetAllKnownPrimes().GetRange(0, 300);
            while (i <= 200) {
                bool pCond = false; bool qrCond = false;
                int qrSum = (int)QuadraticResidue.GetAllQuadraticResidues(i).Sum(x => x);

                foreach (long p in primes) {
                    if (!pCond) {
                        if (i + 2 == p) {
                            pCond = true;
                        }
                    }
                    if (!qrCond)
                    {
                        if (qrSum + 1 == p)
                        {
                            qrCond = true;
                        }
                    }

                    if (pCond && qrCond) {
                        ++pCount;
                        Console.WriteLine("{0} th smallest integer is :: {1} ", pCount, i);
                        break;
                    }
                }

                if (pCount == 7) { break; }
                i++;
            }

            Console.WriteLine("Reverse the digits of the 7th smallest above.");
            Console.ReadKey();
        }

        /// <summary>
        /// The sum of all the quadratic residues of 472 is 177 times k, here k gives the next three digit of my number.
        /// </summary>
        void Solve2() {
            List<long> residues = QuadraticResidue.GetAllQuadraticResidues(472);

            Console.WriteLine("Sum is :: {0}", residues.Sum(x => x));
            Console.WriteLine("k is :: {0}", residues.Sum(x => x) / 177);
            Console.ReadKey();
        }

        /// <summary>
        /// If m and n are positive integers and coprime to each other, if the number of integer solution(s) to the equation m + n = a is 36,
        ///  then the 5th smallest such a, will give you my first two digits.
        /// </summary>
        void Solve1()
        {
            int m = 0, n = 0, a = 36, aCount = 0; ;

            //while (a > 0)
            //{
            //    m = 0;
            //    UniquePairedIntegralSolutions u = new UniquePairedIntegralSolutions(2);
            //    while (m <= a)
            //    {
            //        m++;
            //        n = 0;
            //        while (n <= a)
            //        {
            //            n++;
            //            if (CoPrime(m, n))
            //            {
            //                if (m + n == a)
            //                {
            //                    u.AddIfUnique(new ArrayList() { m, n });
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    if (u.GetUniqueSolutions().Count == 36)
            //    {
            //        ++aCount;
            //        Console.WriteLine("Possible a :: {0}", a);

            //        if (aCount == 10)
            //        {
            //            Console.WriteLine("5th smallest a is :: {0}", a);
            //            break;
            //        }
            //    }
            //    a++;
            //}

            while (a > 0)
            {
                m = 0;
                UniqueIntegralPairs u = new UniqueIntegralPairs();
                while (m <= a)
                {
                    m++;
                    n = 0;
                    while (n <= a)
                    {
                        n++;
                        if (CoPrime(m, n))
                        {
                            if (m + n == a)
                            {
                                u.AddCombination(new ArrayList() { m, n });
                                break;
                            }
                        }
                    }
                }
                if (u.GetCombinations().Count == 36)
                {
                    ++aCount;
                    Console.WriteLine("Possible a :: {0}", a);

                    if (aCount == 10)
                    {
                        Console.WriteLine("5th smallest a is :: {0}", a);
                        break;
                    }
                }
                a++;
            }
            Console.ReadKey();
        }

        Dictionary<string, bool> CoPrimes;
        bool CoPrime(int m, int n)
        {
            if (CoPrimes == null)
            {
                CoPrimes = new Dictionary<string, bool>();
            }

            string hash = m < n ? m + "%$" + n : n + "%$" + m;
            if (!CoPrimes.ContainsKey(hash))
            {
                CoPrimes.Add(hash, MathFunctions.CoPrime(m, n));
            }
            return CoPrimes[hash];
        }
    }
}