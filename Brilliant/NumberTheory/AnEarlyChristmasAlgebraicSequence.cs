using System;
using GenericDefs;
using GenericDefs.Classes;

namespace Brilliant.NumberTheory
{
    public class AnEarlyChristmasAlgebraicSequence : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get
            {
                return thisProblem;
            }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/an-early-christmas-algebraic-sequence/");
        }

        void ISolve.Solve()
        {
            IntegralCoefficientsAi cList = new IntegralCoefficientsAi();
            cList.AddCoefficient(1, 2);

            int aCurrent = 2;
            int nCurrent = 0;
            int coefCurrent = 1;
            while (aCurrent <= 10000) {
                Console.WriteLine("n = {0}, coefficient = {1},  value = {2}", nCurrent, coefCurrent, aCurrent);
                aCurrent += (int)GenericDefs.Functions.Prime.MaxPrimeFactor(aCurrent);
                coefCurrent++;
                if (aCurrent <= 10000) { nCurrent++; }
            }

            Console.WriteLine("Value of n: {0}; Value of Coefficient a - n+1 : {1}.",nCurrent, aCurrent);
            Console.ReadKey();
        }
    }
}