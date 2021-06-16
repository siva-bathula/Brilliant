using GenericDefs.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Combinatorics
{
    /// <summary>
    ///In how many ways 20 can be written as a sum of odd numbers. 19 + 1, 11 + 7 + 1 + 1 etc.
    /// </summary>
    public class Oddition20 : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/jee-combinatorics/");
        }

        void ISolve.Solve()
        {
            UniqueIntegralPairs p = new UniqueIntegralPairs("@#$");
            int N = 4;

            int[] ai = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] oi = new int[10] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
            for (int a = 0; a <= 20; a++)
            {
                ai[0] = a;
                for (int k = 1; k < ai.Length; k++)
                {
                    ai[k] = 0;
                }
                int expr = FindSumExpression(ai, oi);
                if (expr > N) { break; }
                if (expr == N) { AddCombination(ai, p); }

                for (int b = 0; b <= 6; b++)
                {
                    ai[1] = b;
                    for (int k = 2; k < ai.Length; k++)
                    {
                        ai[k] = 0;
                    }
                    expr = FindSumExpression(ai, oi);
                    if (expr > N) { break; }
                    if (expr == N) { AddCombination(ai, p); }
                    for (int c = 0; c <= 4; c++)
                    {
                        ai[2] = c;
                        for (int k = 3; k < ai.Length; k++)
                        {
                            ai[k] = 0;
                        }
                        expr = FindSumExpression(ai, oi);
                        if (expr > N) { break; }
                        if (expr == N) { AddCombination(ai, p); }
                        for (int d = 0; d <= 2; d++)
                        {
                            ai[3] = d;
                            for (int k = 4; k < ai.Length; k++)
                            {
                                ai[k] = 0;
                            }
                            expr = FindSumExpression(ai, oi);
                            if (expr > N) { break; }
                            if (expr == N) { AddCombination(ai, p); }
                            for (int e = 0; e <= 1; e++)
                            {
                                ai[4] = e;
                                for (int k = 5; k < ai.Length; k++)
                                {
                                    ai[k] = 0;
                                }
                                expr = FindSumExpression(ai, oi);
                                if (expr > N) { break; }
                                if (expr == N) { AddCombination(ai, p); }
                                for (int f = 0; f <= 1; f++)
                                {
                                    ai[5] = f;
                                    for (int k = 6; k < ai.Length; k++)
                                    {
                                        ai[k] = 0;
                                    }
                                    expr = FindSumExpression(ai, oi);
                                    if (expr > N) { break; }
                                    if (expr == N) { AddCombination(ai, p); }
                                    for (int g = 0; g <= 1; g++)
                                    {
                                        ai[6] = g;
                                        for (int k = 7; k < ai.Length; k++)
                                        {
                                            ai[k] = 0;
                                        }
                                        expr = FindSumExpression(ai, oi);
                                        if (expr > N) { break; }
                                        if (expr == N) { AddCombination(ai, p); }

                                        for (int h = 0; h <= 1; h++)
                                        {
                                            ai[7] = h;
                                            for (int k = 8; k < ai.Length; k++)
                                            {
                                                ai[k] = 0;
                                            }
                                            expr = FindSumExpression(ai, oi);
                                            if (expr > N) { break; }
                                            if (expr == N) { AddCombination(ai, p); }
                                            for (int i = 0; i <= 1; i++)
                                            {
                                                ai[8] = i;
                                                for (int k = 9; k < ai.Length; k++)
                                                {
                                                    ai[k] = 0;
                                                }
                                                expr = FindSumExpression(ai, oi);
                                                if (expr > N) { break; }
                                                if (expr == N) { AddCombination(ai, p); }
                                                for (int j = 0; j <= 1; j++)
                                                {
                                                    ai[9] = j;
                                                    expr = FindSumExpression(ai, oi);
                                                    if (expr > N) { break; }
                                                    if (expr == N) { AddCombination(ai, p); }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            long count = 0;
            List<UniqueIntegralPairs.Combination> pairs = p.GetCombinations();
            foreach (UniqueIntegralPairs.Combination c in pairs)
            {
                List<int> nP = new List<int>();
                for (int i = 0; i < 10; i++)
                {
                    int iter = (int)c.Pair[i];
                    while (iter > 0)
                    {
                        iter--;
                        nP.Add(oi[i]);
                    }
                }
                count += Permutations.GetPermutationsCount(nP, nP.Count);
            }
            Console.WriteLine("Number of ways :: {0}", count);
            Console.ReadKey();
        }

        int FindSumExpression(int[] ai, int[] oi)
        {
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += ai[i] * oi[i];
                if (sum > 1)
                {
                    sum += 0; 
                }
            }
            return sum;
        }

        void AddCombination(int[] ai, UniqueIntegralPairs p)
        {
            ArrayList l = new ArrayList();
            foreach (int aic in ai) { l.Add(aic); }
            p.AddCombination(l);
        }
    }
}