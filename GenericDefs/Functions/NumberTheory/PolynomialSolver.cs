using Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.NumberTheory
{
    internal class Constants
    {
        internal const double OneHalf = 1.0 / 2;
        internal const double OneThird = 1.0 / 3;
        internal static double SquareRoot3 = Math.Sqrt(3);
    }

    public class Root<T>
    {
        public Root(bool isComplex)
        {
            IsComplex = isComplex;
            IsReal = !isComplex;
        }

        public bool IsComplex
        {
            get;
        }

        public bool IsReal
        {
            get;
        }

        public T RealPart
        {
            get; internal set;
        }

        public T ImaginaryPart
        {
            get; internal set;
        }

        public new string ToString()
        {
            string retVal = string.Empty;

            retVal = RealPart.ToString();
            if (IsComplex) retVal += string.Format(" i({0})", ImaginaryPart);

            return retVal;
        }
    }

    public class Quadratic
    {
        public List<Root<double>> Roots
        {
            get; private set;
        }

        public bool IsComplex
        {
            get; private set;
        }
        double a, b, c;

        public Quadratic(double[] ai, bool solveLater = false)
        {
            a = ai[0]; b = ai[1]; c = ai[2];
            if (!solveLater) Solve();
        }

        bool _isSolved = false;
        public void Solve()
        {
            if (_isSolved) return;
            if (a.Equals(0)) throw new Exception("Quadratic cannot have leading coefficient zero.");

            double d = (b * b) - (4 * a * c);
            if (d < 0) { d *= -1; IsComplex = true; }
            else IsComplex = false;

            d = Math.Sqrt(d);
            d /= (2 * a);

            b *= -1;
            b /= (2 * a);

            List<Root<double>> roots = new List<Root<double>>() { new Root<double>(IsComplex), new Root<double>(IsComplex) };
            if (IsComplex)
            {
                roots[0].RealPart = b;
                roots[0].ImaginaryPart = d;

                roots[1].RealPart = b;
                roots[1].ImaginaryPart = -d;
            }
            else
            {
                roots[0].RealPart = b + d;
                roots[1].RealPart = b - d;
            }

            Roots = roots;
        }
    }

    public class Cubic
    {
        public List<Root<double>> Roots
        {
            get; private set;
        }

        public bool HasComplex
        {
            get; private set;
        }

        double a, b, c, d;

        public Cubic(double[] ai, bool solveLater = false)
        {
            a = ai[0]; b = ai[1]; c = ai[2]; d = ai[3];
            if (a.Equals(0)) throw new Exception("Cubic cannot have leading coefficient zero.");
            Init();
            if (!solveLater) Solve();
        }

        double f, g, h;
        private void Init()
        {
            f = ((3 * c / a) - (b * b / (a * a))) / 3;
            g = ((2 * b * b * b / (a * a * a)) - (9 * b * c / (a * a)) + (27 * d / a)) / 27;
            h = (g * g / 4) + (f * f * f / 27);
        }
    
        bool _isSolved = false;
        double deltaPos = Math.Pow(10, -10);
        public void Solve()
        {
            double absvalh = Math.Abs(h);
            
            if ((f == 0 && g == 0 && h == 0) || (f < deltaPos && g < deltaPos && h < deltaPos)) {
                HasComplex = false;
                ThreeEqualRoots();
            } else if (h <= 0 || h <= deltaPos) {
                HasComplex = false;
                OnlyRealRoots();
            } else {
                HasComplex = true;
                OnlyOneRealRoot();
            }
        }

        public Root<double> GetAnyRealRoot()
        {
            if (!_isSolved) Solve();

            return Roots[0]; // 0 index is real root.
        }

        /// <summary>
        /// When h g.t. 0, as is the case here, all 3 roots are real.
        /// </summary>
        private void OnlyOneRealRoot()
        {
            double r = -(g / 2) + Math.Sqrt(h);
            double s = Math.Pow(r < 0 ? -r : r, Constants.OneThird);
            if (r < 0) s *= -1;

            double t = -(g / 2) - Math.Sqrt(h);
            double u = Math.Pow(t < 0 ? -t : t, Constants.OneThird);
            if (t < 0) u *= -1;

            List<Root<double>> roots = new List<Root<double>>() { new Root<double>(false), new Root<double>(true), new Root<double>(true) };

            roots[0].RealPart = (s + u) - (b / (3 * a));

            roots[1].RealPart = -((s + u) / 2) - (b / (3 * a));
            roots[1].ImaginaryPart = (s - u) * Constants.SquareRoot3 / 2;

            roots[2].RealPart = roots[1].RealPart;
            roots[2].ImaginaryPart = -roots[1].ImaginaryPart;

            Roots = roots;
        }

        /// <summary>
        /// When h l.e.q 0, as is the case here, all 3 roots are real.
        /// </summary>
        private void OnlyRealRoots()
        {
            double i = Math.Pow((g * g / 4) - h, Constants.OneHalf);
            double j = Math.Pow(i < 0 ? -i : i, Constants.OneThird);
            if (i < 0) j *= -1;
            double k = (g == 0 && i == 0 ? Math.Acos(-1 / 2) : Math.Acos(-g / (2 * i)));
            double l = -1 * j;
            double m = Math.Cos(k / 3);
            double n = Constants.SquareRoot3 * Math.Sin(k / 3);
            double p = -1 * (b / (3 * a));

            List<Root<double>> roots = new List<Root<double>>() { new Root<double>(false), new Root<double>(false), new Root<double>(false) };

            roots[0].RealPart = (2 * j * Math.Cos(k / 3)) - (b / (3 * a));
            roots[1].RealPart = (l * (m + n)) + p;
            roots[2].RealPart = (l * (m - n)) + p;

            Roots = roots;
        }

        /// <summary>
        /// When f,g,h are zero and the equation has three equal real roots.
        /// </summary>
        private void ThreeEqualRoots()
        {
            List<Root<double>> roots = new List<Root<double>>() { new Root<double>(false), new Root<double>(false), new Root<double>(false) };

            double x = d / a;

            double root = Math.Pow(x < 0 ? -x : x, Constants.OneThird);
            if (x > 0) root *= -1;
            roots.ForEach(y => { y.RealPart = root; });

            Roots = roots;
        }

    }

    public class Quartic
    {
        public List<Root<double>> Roots
        {
            get; private set;
        }

        public int NRealRoots { get; private set; }
        public int NComplexRoots { get; private set; }

        public bool HasComplexRoots
        {
            get; private set;
        }

        double a, b, c, d, e;

        public Quartic(double[] ai, bool solveLater = false)
        {
            a = ai[0]; b = ai[1]; c = ai[2]; d = ai[3]; e = ai[4];
            if (a.Equals(0)) throw new Exception("Quartic cannot have leading coefficient zero.");

            NRealRoots = 0;
            NComplexRoots = 0;

            b /= a;
            c /= a;
            d /= a;
            e /= a;

            if (!solveLater) Solve();
        }

        double p;
        private void SolveForP()
        {
            double[] ci = new double[4] { 8, -4 * c, (2 * b * d - 8 * e), (4 * c * e - (d * d) - (e * b * b)) };
            Cubic pSolver = new Cubic(ci);
            p = (pSolver.GetAnyRealRoot()).RealPart;
        }

        private Root<double> RHSQuadratic { get; set; }
        private bool _PositiveLeadingRhsCoeff = true;
        private void SolveRhs()
        {
            double _ri0 = (b * b / 4 + ((2 * p) - c));
            if (_ri0 < 0) _PositiveLeadingRhsCoeff = false;

            double[] ri = new double[3] { (b * b / 4 + ((2 * p) - c)), ((b * p) - d), (p * p) - e };
            if (!_PositiveLeadingRhsCoeff) { ri[0] *= -1; ri[1] *= -1; ri[2] *= -1; }

            Quadratic qr = new Quadratic(ri);
            RHSQuadratic = qr.Roots[0];
        }

        private Root<double> LHSQuadratic { get; set; }
        private void SolveLhs()
        {
            if (_PositiveLeadingRhsCoeff)
            {
                double[] ri = new double[3] { 1, (b / 2) - 1, (p + RHSQuadratic.RealPart) };
                Quadratic qr = new Quadratic(ri);
                Roots = new List<Root<double>>();
                Roots.Add(qr.Roots[0]); Roots.Add(qr.Roots[1]);

                if (qr.IsComplex) { NComplexRoots += 2; HasComplexRoots = true; }
                else NRealRoots += 2;

                ri = new double[3] { 1, (b / 2) + 1, (p - RHSQuadratic.RealPart) };
                qr = new Quadratic(ri);
                Roots.Add(qr.Roots[0]); Roots.Add(qr.Roots[1]);

                if (qr.IsComplex) { NComplexRoots += 2; HasComplexRoots = true; }
                else NRealRoots += 2;
            } else {
                //This is incomplete.
                NRealRoots = 1;
                NComplexRoots = 2;
            }
        }

        bool _isSolved = false;
        public void Solve()
        {
            if (_isSolved) return;

            SolveForP();
            SolveRhs();
            SolveLhs();

            _isSolved = true;
        }

    }
}