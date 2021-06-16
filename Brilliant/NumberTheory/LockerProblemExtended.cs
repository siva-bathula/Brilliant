using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Classes;

namespace Brilliant.NumberTheory
{
    public class LockerProblemExtended : ISolve, IBrilliant
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
            thisProblem = new Brilliant("https://brilliant.org/problems/the-locker-problem-extended/");
        }

        void ISolve.Solve()
        {
            IntegralCoefficientsAi lockers = new IntegralCoefficientsAi();

            //student 1
            for (int i = 1; i <= 100; i++)
            {
                lockers.AddCoefficient(i, 1);
            }

            //student 2
            int coeff = 0;
            while (coeff <= 100)
            {
                coeff += 2;
                if (coeff <= 100) CloseLocker(lockers, coeff);
            }

            HashSet<int> absent = new HashSet<int>();
            absent.Add(100); absent.Add(81); absent.Add(64);
            absent.Add(49); absent.Add(36); absent.Add(25);
            absent.Add(16); absent.Add(9); absent.Add(4);
            absent.Add(99); absent.Add(98); absent.Add(92);
            absent.Add(90); absent.Add(88); absent.Add(84);
            absent.Add(76); absent.Add(75);
            absent.Add(72); absent.Add(68); absent.Add(63);
            absent.Add(60); absent.Add(56); absent.Add(54);
            absent.Add(52); absent.Add(50); absent.Add(45);
            absent.Add(44); absent.Add(40); absent.Add(28);
            absent.Add(27); absent.Add(24); absent.Add(20);
            absent.Add(8); absent.Add(12); absent.Add(18);
            absent.Add(48); absent.Add(80); absent.Add(96);
            absent.Add(32);

            //100 81 64 49 36 25 16 9 4 99 98 92 90 88 84 76 75 72 68 63 60 56 54 52 50 45 44 40 28 27 24 20 8 12 18 32 48 80 96
            //student 3 to 100
            for (int x = 3; x <= 100; x++)
            {
                int n = 1;
                if (absent.Contains(x)) { continue; }
                while (n * x <= 100)
                {
                    SwitchStateLocker(lockers, n * x);
                    n++;
                }
            }
            int nAbs = absent.Count;
            Console.WriteLine("Students Present : {0}, Absent : {1}.", 100 - nAbs, nAbs);
            //print final state of all lockers
            for (int i = 1; i <= 100; i++)
            {
                if (lockers.GetCoefficientValue(i).Value == 1)
                    Console.WriteLine("L{0} : {1}", i, lockers.GetCoefficientValue(i).Value);
            }

            Console.ReadKey();
        }

        void SwitchStateLocker(IntegralCoefficientsAi lockers, int i)
        {
            int? currVal = lockers.GetCoefficientValue(i);
            if (currVal.Value == 0) OpenLocker(lockers, i);
            if (currVal.Value == 1) CloseLocker(lockers, i);
        }

        void CloseLocker(IntegralCoefficientsAi lockers, int i) {
            lockers.ChangeCoefficient(i, 0);
        }

        void OpenLocker(IntegralCoefficientsAi lockers, int i)
        {
            lockers.ChangeCoefficient(i, 1);
        }
    }
}