using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using GenericDefs.Functions.Geometry;
using GenericDefs.Classes;
using Numerics;
using GenericDefs.Classes.NumberTypes;

namespace Brilliant.NumberTheory
{
    /// <summary>
    /// 
    /// </summary>
    public class WhoLovesGeometry : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/lets-see-who-loves-geometry/");
        }

        void ISolve.Solve()
        {
            double tABC = PropertiesOfTriangle.GetArea(14, 15, 13);
            List<double> incAngles = PropertiesOfTriangle.GetAnglesOfTriangle(14, 15, 13);
            double tBPR = PropertiesOfTriangle.GetAreaWithIncludedAngle((double)14.0 * 13 / 28, (double)13.0 * 14 / 29, incAngles[1]);
            double tPCQ = PropertiesOfTriangle.GetAreaWithIncludedAngle((double)14.0 * 15 / 28, (double)15.0 * 14 / 27, incAngles[2]);
            double tARQ = PropertiesOfTriangle.GetAreaWithIncludedAngle((double)13.0 * 15 / 29, (double)15.0 * 13 / 27, incAngles[0]);

            double tPQR = tABC - (tBPR + tPCQ + tARQ);

            Fraction<int> fMN = FractionConverter<int>.RealToFraction(tPQR, 0.00000001);

            Console.WriteLine("Area of triangle PQR : {0}", tPQR);
            Console.WriteLine("Approach 1");
            Console.WriteLine("M: {0}", fMN.N);
            Console.WriteLine("N: {0}", fMN.D);

            Console.WriteLine("Approach 2");

            BigRational br = new BigRational(tPQR);
            Console.WriteLine("M: {0}", br.Numerator.ToString());
            Console.WriteLine("N: {0}", br.Denominator.ToString());
            Console.ReadKey();
        }
    }
}