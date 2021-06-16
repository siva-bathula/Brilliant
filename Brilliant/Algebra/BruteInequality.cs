using GenericDefs.DotNet;
using System;

namespace Brilliant.Algebra
{
    public class BruteInequality : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("");
        }

        void ISolve.Solve()
        {
            Collection1.AwesomeInequality();
        }

        internal class Collection1
        {
            /// <summary>
            /// https://brilliant.org/problems/awesome-inequality/
            /// How many distinct 16-digit positive integer(s) satisfy the condition that if I move its first digit to the last digit, 
            /// the resultant number increases by 50%?
            /// </summary>
            internal static void AwesomeInequality()
            {
                double epsilon = 0.00005;
                double min = 100.0;
                double root1 = 0.0, root2 = 0.0, root3 = 0.0;
                for(double t1 = 2.0; t1 <= 3.0; t1 += epsilon)
                {
                    double r1 = FindFirstRoot(t1);
                    for (double t2 = 2.0; t2 <= 4.0; t2 += epsilon)
                    {
                        double r2 = FindFirstRoot(t2);
                        double r3 = 1.0 / (r1 * r2);

                        double dSum = r1 + (1.0 / r1) + r2 + (1.0 / r2) + r3 + (1.0 / r3);
                        if (!(Math.Abs(8.0 - dSum) < 0.0000000001)) continue;

                        double value = (r1 + r2 + r3) * ((1.0 / r1) + (1.0 / r2) + (1.0 / r3));

                        if (value < min)
                        {
                            min = value;
                            root1 = r1;
                            root2 = r2;
                            root3 = r3;
                        }
                    }
                }

                QueuedConsole.WriteImmediate(string.Format("Min value occurs at : {0}, {1}, {2}", root1, root2, root3));
                QueuedConsole.WriteFinalAnswer(string.Format("Minimum value of expression is : " + min));

                double x = 1.0;
                double sq11 = Math.Sqrt(11.0);
                double y = (Math.Sqrt(8.0 - 2 * sq11) + sq11 - 1) / 2;
                double z = (5 - sq11 + Math.Sqrt(35 - 10 * sq11));

                double p1 = (x / y) + (y / z) + (z / x);
                double p2 = (y / x) + (z / y) + (x / z);

                QueuedConsole.WriteImmediate(string.Format("Check p1 + p2 is 8 : {0}", p1 + p2));
                QueuedConsole.WriteImmediate(string.Format("p1*p2 = {0}", p1 * p2));
                QueuedConsole.WriteImmediate(string.Format("22 * sqrt(11) - 57 : {0}", 22 * sq11 - 57));
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// x + 1 / x = t
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            internal static double FindFirstRoot(double t)
            {
                return 0.5 * (t + Math.Sqrt(t * t - 4));
            }
        }
    }
}