using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.Classes;
using GenericDefs.DotNet;
using GenericDefs.Classes.NumberTypes;

namespace Brilliant.Combinatorics._5
{
    public class TrollGCD : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/troll-gcd/");
        }

        void ISolve.Solve()
        {
            long MMax = 0, kMax = 0;
            long M1Max = 0, k1Max = 0;
            for (long k = 30; k <= 70; k++) {
                Fraction<long> fr = new Fraction<long>((k + 1) * (k + 2) * (k + 3), (98 - k) * (99 - k) * (100 - k), false);
                Fraction<long> fr1 = new Fraction<long>((98 - k) * (99 - k) * (100 - k), (k + 1) * (k + 2) * (k + 3), false);
                if (fr.N > MMax) { MMax = fr.N; kMax = k; }
                if (fr1.D > M1Max) { M1Max = fr1.D; k1Max = k; }
            }

            QueuedConsole.WriteImmediate(string.Format("Trial 1"));
            QueuedConsole.WriteImmediate(string.Format("maximum value of M :: {0}", MMax));
            QueuedConsole.WriteImmediate(string.Format("k :: {0}", kMax));
            QueuedConsole.WriteImmediate(string.Format("M mod 1000 :: {0}", MMax % 1000));
            QueuedConsole.WriteImmediate(string.Format("Trial 2"));
            QueuedConsole.WriteImmediate(string.Format("maximum value of M :: {0}", M1Max));
            QueuedConsole.WriteImmediate(string.Format("k :: {0}", k1Max));
            QueuedConsole.WriteFinalAnswer(string.Format("M mod 1000 :: {0}", M1Max % 1000));
        }
    }
}