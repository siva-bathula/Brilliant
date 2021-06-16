using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDefs.Functions.Geometry
{
    public class Triangle
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double BC { get; set; }
        public double AC { get; set; }
        public double AB { get; set; }
    }

    public enum TriangleType
    {
        Acute,
        Right,
        Obtuse,
        Invalid
    }

    public class PropertiesOfTriangle
    {
        public static double GetArea(double a, double b, double c)
        {
            if (!IsValidTriangle(a, b, c))
            {
                throw new Exception(string.Format("This triangle is invalid. a:{0}, b:{1}, c:{2}", a, b, c));
            }

            double s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        public static bool IsValidTriangle(double a, double b, double c)
        {
            if (a == 0 || b == 0 || c == 0) { return false; }
            if ((a + b <= c) || (b + c <= a) || (a + c <= b)) { return false; }
            return true;
        }

        /// <summary>
        /// Returns angles in degrees.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<double> GetAnglesOfTriangle(double a, double b, double c)
        {
            if (!IsValidTriangle(a, b, c))
            {
                throw new Exception(string.Format("This triangle is invalid. a:{0}, b:{1}, c:{2}", a, b, c));
            }

            return GetAnglesGivenSideLengths(a, b, c);
        }

        /// <summary>
        /// Returns angles in radians.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<double> GetAnglesOfTriangleInRadians(double a, double b, double c)
        {
            if (!IsValidTriangle(a, b, c))
            {
                throw new Exception(string.Format("This triangle is invalid. a:{0}, b:{1}, c:{2}", a, b, c));
            }

            return new List<double>() {
                Math.Acos((b * b + c * c - a * a) / (2 * c * b)),
                Math.Acos((a * a + c * c - b * b) / (2 * a * c)),
                Math.Acos((a * a + b * b - c * c) / (2 * a * b))
            };
        }

        private static List<double> GetAnglesGivenSideLengths(double a, double b, double c)
        {
            return new List<double>() {
                180 * Math.Acos((b * b + c * c - a * a) / (2 * c * b)) / Math.PI,
                180 * Math.Acos((a * a + c * c - b * b) / (2 * a * c)) / Math.PI,
                180 * Math.Acos((a * a + b * b - c * c) / (2 * a * b)) / Math.PI
            };
        }

        public static Triangle GetIsoscelesTriangle(double sideLength, double incAngle)
        {
            Triangle t = new Triangle();
            t.A = incAngle;
            t.B = (180.0 - incAngle) / 2.0;
            t.C = (180.0 - incAngle) / 2.0;
            t.AC = sideLength;
            t.AB = sideLength;
            t.BC = sideLength * Math.Sqrt(2 * Math.Pow(Math.Sin(incAngle), 2));
            return t;
        }

        public static double GetThirdSide(double a1, double a2, double incAngle)
        {
            return Math.Sqrt((a1 * a1 + a2 * a2) - 2 * a1 * a2 * Math.Pow(Math.Cos(incAngle), 2));
        }

        /// <summary>
        /// Returns the area of a triangle formed by two given sides and an included angle(in degrees).
        /// </summary>
        /// <param name="a">Side of length 'a'</param>
        /// <param name="b">Side of length 'b'</param>
        /// <param name="C">Included angle as measured in degrees.</param>
        /// <returns></returns>
        public static double GetAreaWithIncludedAngle(double a, double b, double C)
        {
            return 0.5 * a * b * Math.Sin(C * Math.PI / 180);
        }

        public static bool IsAcuteAngledTriangle(double a, double b, double c)
        {
            return GetTriangleType(a,b,c) == TriangleType.Acute;
        }

        public static TriangleType GetTriangleType(double a, double b, double c)
        {
            if (!IsValidTriangle(a, b, c)) {
                return TriangleType.Invalid;
            }
            double error = 0.0000001;
            double rtAngle = 90 - error;
            List<double> angles = GetAnglesGivenSideLengths(a, b, c);
            double maxAngle = angles.Max();
            if (maxAngle < rtAngle) { return TriangleType.Acute; }
            else if(maxAngle > rtAngle && maxAngle <= 90) { return TriangleType.Right; }
            else return TriangleType.Obtuse;
        }

        public static double GetCircumRadius(double a, double b, double c)
        {
            return a * b * c / (4 * GetArea(a, b, c));
        }
    }
}