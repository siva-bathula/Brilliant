using GenericDefs.Classes.NumberTypes;
using GenericDefs.DotNet;
using GenericDefs.Functions.CoordinateGeometry;
using GenericDefs.Functions.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Brilliant.Geometry
{
    public class CollectionOfProblems : ISolve, IBrilliant
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
            (new Collection1()).Solve();
        }

        internal class Collection1
        {
            internal void Solve()
            {
                TunnelVision();
            }

            /// <summary>
            /// https://brilliant.org/problems/tunnel-vision/
            /// </summary>
            void TunnelVision()
            {
                int n = 0;
                double xmin = 0.0, xmax = 1.0;
                double ymin = 0.0, ymax = 1.0;
                while(++n <= 200)
                {
                    double length = xmax - xmin;
                    double prevLength = length;
                    length *= 0.75;
                    double diff = (prevLength - length);
                    double halfdiff = 0.5 * diff;
                    int position = n % 4;
                    if(position == 1) {
                        xmin += halfdiff;
                        xmax -= halfdiff;
                        ymin += diff;
                    } else if(position == 2) {
                        xmin += diff;
                        ymax -= halfdiff;
                        ymin += halfdiff;
                    } else if (position == 3) {
                        xmin += halfdiff;
                        xmax -= halfdiff;
                        ymax -= diff;
                    } else if (position == 0) {
                        xmax -= diff;
                        ymax -= halfdiff;
                        ymin += halfdiff;
                    }
                }
                Fraction<int> fr = FractionConverter<int>.RealToFraction(xmin / ymin, 0.0000000001);
                QueuedConsole.WriteImmediate("a/b: {0} / {1}, x : {2}, y: {3}", fr.N, fr.D, xmin, ymin);
            }

            /// <summary>
            /// https://brilliant.org/problems/crazy-new-year-firework/
            /// </summary>
            void CrazyNewYearFirework()
            {
                double xn = 2017.0, yn = 1.0;
                int n = 0;
                BigInteger tn = 1;
                while (++n < 366)
                {
                    tn *= 2;
                    int thetaDeg = (int)(tn % 360);

                    double thetaRad = ((Math.PI) * thetaDeg) / 180.0;

                    double mn = Math.Tan(thetaRad);
                    double d = xn + ((yn - 2016) * mn);
                    d /= (1 + (mn * mn));
                    double xnp1 = (2 * d) - xn;
                    double ynp1 = (2*d*mn) - yn + 4032;
                    
                    QueuedConsole.WriteImmediate("n: {0}, xnp1: {1}, ynp1: {2}, floor(xnp1+ynp1): {3}", n, xnp1, ynp1, Math.Floor(xnp1 + ynp1));
                    xn = xnp1;
                    yn = ynp1;
                }
            }

            /// <summary>
            /// https://brilliant.org/problems/inspired-by-aakash-khandelwal/
            /// 2^n = 45k +/- 1;
            /// </summary>
            void FindN()
            {
                int n = 0;
                BigInteger an = new BigInteger(1);
                while (true)
                {
                    n++;
                    an *= 2;
                    if (((an - 1) % 45 == 0) || ((an + 1) % 45 == 0)) break;
                }

                QueuedConsole.WriteFinalAnswer("n : " + n);
            }

            /// <summary>
            /// https://brilliant.org/problems/triangles-and-circles-but-not-geometry-part-3/
            /// </summary>
            void FindAcuteAngledTriangles()
            {
                RegularPolygon p = new RegularPolygon(192, 10000);
                QueuedConsole.WriteAndWaitOnce("Count of acute angled triangles : {0}", GenericDefs.Functions.Combinatorics.Polygon.CountAllAcuteAngleTriangles(p));
                QueuedConsole.ReadKey();
            }

            /// <summary>
            /// https://brilliant.org/problems/apply-morleys-theorem/
            /// </summary>
            void ApplyMorleysTheorem()
            {
                int a = 6, b = 8, c = 10;
                List<double> incAngles = PropertiesOfTriangle.GetAnglesOfTriangleInRadians(a, b, c);
                double R = PropertiesOfTriangle.GetCircumRadius(a, b, c);
                double aMT = 8 * R * Math.Sin(incAngles[0] / 3) * Math.Sin(incAngles[1] / 3) * Math.Sin(incAngles[2] / 3);
                double area = Math.Sqrt(3) * aMT * aMT / 4;
                QueuedConsole.WriteImmediate("Area of XYZ : {0}", area.ToString("0.###"));
            }

            /// <summary>
            /// https://brilliant.org/problems/any-way-you-slice-it/
            /// </summary>
            void AnywayYouSliceIt()
            {
                double val1 = Math.PI / 3, val2 = 2 * val1;
                double error = 0.000001;

                double slope1 = 0, slope2 = 0;
                for (double a = Math.PI / 6; ; a += error)
                {
                    if (Math.Abs(a + Math.Sin(a) - val1) <= error)
                    {
                        slope1 = Math.Tan(a / 2);
                        QueuedConsole.WriteImmediate("a1 : {0}, slope1: {1}", a, slope1);
                        break;
                    }
                }
                for (double a = Math.PI / 4; ; a += error)
                {
                    if (Math.Abs(a + Math.Sin(a) - val2) <= error)
                    {
                        slope2 = Math.Tan(a / 2);
                        QueuedConsole.WriteImmediate("a2 : {0}, slope2: {1}", a, slope2);
                        break;
                    }
                }

                double sumSlopes = 1000*(slope1 + slope2);
                QueuedConsole.WriteImmediate("1000*sum slopes: {0}, Required value : {1}", sumSlopes, Math.Ceiling(sumSlopes));
            }
        }
    }
}