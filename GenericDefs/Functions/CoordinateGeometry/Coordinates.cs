using GenericDefs.Functions.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericDefs.Functions.CoordinateGeometry
{
    public class TwoDCoordinates<T>
    {
        public T X;
        public T Y;
        public TwoDCoordinates(T x, T y) {
            X = x;
            Y = y;
        }
    }

    public class Functions
    {
        public static bool IsValidQuadrilateral(List<List<int>> points)
        {
            List<IList<List<int>>> combinations = Classes.Combinations.GetAllCombinations(points, 3);
            foreach(IList<List<int>> c in combinations) {
                if (Collinear(c[0], c[1], c[2])) return false;
            }

            return true;
        }

        public static double TwoDDistanceBetweenPoints(int[] c1, int[] c2)
        {
            return Math.Sqrt(Math.Pow((c1[0] - c2[0]), 2) + Math.Pow((c1[1] - c2[1]), 2));
        }

        public static List<IList<IList<int>>> Collinear(IList<IList<int>> points)
        {
            List<IList<IList<int>>> combinations = Classes.Combinations.GetAllCombinations(points, 3);
            List<IList<IList<int>>> retVal = new List<IList<IList<int>>>();
            combinations.ForEach(c =>
            {
                if (Collinear(c[0], c[1], c[2])) retVal.Add(c);
            });
            return retVal;
        }

        public static bool Collinear(IList<int> c1, IList<int> c2, IList<int> c3)
        {
            return (c1[1] - c2[1]) * (c1[0] - c3[0]) == (c1[1] - c3[1]) * (c1[0] - c2[0]);
        }
    }

    public class CoordinatesOfTriangle
    {
        /// <summary>
        /// Calculates area of a triangle if valid triangle, else returns zero.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <returns></returns>
        public static double GetArea(int[] c1, int[] c2, int[] c3)
        {
            double d1 = Functions.TwoDDistanceBetweenPoints(c1, c2);
            double d2 = Functions.TwoDDistanceBetweenPoints(c2, c3);
            double d3 = Functions.TwoDDistanceBetweenPoints(c1, c3);

            if (PropertiesOfTriangle.IsValidTriangle(d1, d2, d3))
                return PropertiesOfTriangle.GetArea(d1, d2, d3);

            return 0;
        }
    }

    public class RectangularBoundary<T> {
        public T Xmin;
        public T Xmax;
        public T Ymin;
        public T Ymax;
        public RectangularBoundary(T Xmin, T Xmax, T Ymin, T Ymax)
        {
            this.Xmin = Xmin;
            this.Xmax = Xmax;
            this.Ymin = Ymin;
            this.Ymax = Ymax;
        }

        bool IsTop(T y) {
            if (y.Equals(Ymax)) return true;
            return false;
        }

        bool IsBottom(T y)
        {
            if (y.Equals(Ymin)) return true;
            return false;
        }

        bool IsLeft(T x)
        {
            if (x.Equals(Xmax)) return true;
            return false;
        }

        bool IsRight(T x)
        {
            if (x.Equals(Xmin)) return true;
            return false;
        }

        bool IsWithinRange(T x, T y) {
            if (((IComparable<T>)x).CompareTo(Xmax) <= 0 && ((IComparable<T>)x).CompareTo(Xmin) >= 0) {
                if (((IComparable<T>)y).CompareTo(Ymax) <= 0 && ((IComparable<T>)y).CompareTo(Ymin) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsWithinBoundary(T x, T y)
        {
            if (((IComparable<T>)x).CompareTo(Xmax) < 0 && ((IComparable<T>)x).CompareTo(Xmin) > 0)
            {
                if (((IComparable<T>)y).CompareTo(Ymax) < 0 && ((IComparable<T>)y).CompareTo(Ymin) > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}