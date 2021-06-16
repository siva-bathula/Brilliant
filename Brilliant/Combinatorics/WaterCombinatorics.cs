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
    ///You have 10 different empty containers: 6 can contain up to 3 L of water and 4 can contain up to 8 L of water.
    ///How many ways are there to fill up exactly 46 L of water into these containers, such that the number of liters of water in each container is an integer?
    ///Details and Assumptions
    ///The order of filling the containers is irrelevant.
    ///You cannot take water out from any of the  containers.
    ///There are no spills.
    /// </summary>
    public class WaterCombinatorics : ISolve, IBrilliant
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
            int N = 46;
            int aMax = 3;
            int bMax = 8;

            int[] ai = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int a = 0; a <= aMax; a++)
            {
                ai[0] = a;
                for (int k = 1; k < ai.Length; k++)
                {
                    ai[k] = 0;
                }
                int expr = ai.Sum();
                if (expr > N) { break; }
                if (expr == N) { AddCombination(ai, p); }

                for (int b = 0; b <= aMax; b++)
                {
                    ai[1] = b;
                    for (int k = 2; k < ai.Length; k++)
                    {
                        ai[k] = 0;
                    }
                    expr = ai.Sum();
                    if (expr > N) { break; }
                    if (expr == N) { AddCombination(ai, p); }
                    for (int c = 0; c <= aMax; c++)
                    {
                        ai[2] = c;
                        for (int k = 3; k < ai.Length; k++)
                        {
                            ai[k] = 0;
                        }
                        expr = ai.Sum();
                        if (expr > N) { break; }
                        if (expr == N) { AddCombination(ai, p); }
                        for (int d = 0; d <= aMax; d++)
                        {
                            ai[3] = d;
                            for (int k = 4; k < ai.Length; k++)
                            {
                                ai[k] = 0;
                            }
                            expr = ai.Sum();
                            if (expr > N) { break; }
                            if (expr == N) { AddCombination(ai, p); }
                            for (int e = 0; e <= aMax; e++)
                            {
                                ai[4] = e;
                                for (int k = 5; k < ai.Length; k++)
                                {
                                    ai[k] = 0;
                                }
                                expr = ai.Sum();
                                if (expr > N) { break; }
                                if (expr == N) { AddCombination(ai, p); }
                                for (int f = 0; f <= aMax; f++)
                                {
                                    ai[5] = f;
                                    for (int k = 6; k < ai.Length; k++)
                                    {
                                        ai[k] = 0;
                                    }
                                    expr = ai.Sum();
                                    if (expr > N) { break; }
                                    if (expr == N) { AddCombination(ai, p); }
                                    for (int g = 0; g <= bMax; g++)
                                    {
                                        ai[6] = g;
                                        for (int k = 7; k < ai.Length; k++)
                                        {
                                            ai[k] = 0;
                                        }
                                        expr = ai.Sum();
                                        if (expr > N) { break; }
                                        if (expr == N) { AddCombination(ai, p); }

                                        for (int h = 0; h <= bMax; h++)
                                        {
                                            ai[7] = h;
                                            for (int k = 8; k < ai.Length; k++)
                                            {
                                                ai[k] = 0;
                                            }
                                            expr = ai.Sum();
                                            if (expr > N) { break; }
                                            if (expr == N) { AddCombination(ai, p); }
                                            for (int i = 0; i <= bMax; i++)
                                            {
                                                ai[8] = i;
                                                for (int k = 9; k < ai.Length; k++)
                                                {
                                                    ai[k] = 0;
                                                }
                                                expr = ai.Sum();
                                                if (expr > N) { break; }
                                                if (expr == N) { AddCombination(ai, p); }
                                                for (int j = 0; j <= bMax; j++)
                                                {
                                                    ai[9] = j;
                                                    expr = ai.Sum();
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
            Console.WriteLine("Number of ways :: {0}", p.GetCombinations().Count);
            Console.ReadKey();
        }

        void AddCombination(int[] ai, UniqueIntegralPairs p) {
            ArrayList l = new ArrayList();
            foreach (int aic in ai) { l.Add(aic); }
            p.AddCombination(l);
        }
    }
}
