using GenericDefs.Functions.CoordinateGeometry;
using System.Collections.Generic;
using GenericDefs.Classes;
using System;
using System.Linq;
using GenericDefs.Functions.Geometry;
using GenericDefs.DotNet;
using System.Numerics;

namespace GenericDefs.Functions.Combinatorics
{
    public class Polygon
    {
        /// <summary>
        /// Gets a count of all acute angled triangled triangles formed by joining the vertices of a regular polygon.
        /// Counts only triangles with vertices as that of the vertices of the polygon.
        /// </summary>
        /// <param name="p"></param>
        public static long CountAllAcuteAngleTriangles(RegularPolygon p)
        {
            Dictionary<int, double> VL = p.VertexLengths;
            List<int> vIds = p.GetVertexIds();
            int vertexCount = p.N / 2;
            List<IList<int>> combinations = Combinations.GetAllCombinations(p.GetVertexIds(), 3);
            
            UniqueArrangements<int> uaAcuteDiff = new UniqueArrangements<int>();
            UniqueArrangements<int> uaRightDiff = new UniqueArrangements<int>();
            UniqueArrangements<int> uaObtuseDiff = new UniqueArrangements<int>();

            SimpleCounter scAcute = new SimpleCounter();
            SimpleCounter scRight = new SimpleCounter();
            SimpleCounter scObtuse = new SimpleCounter();
            foreach (IList<int> c in combinations)
            {
                var s = c.OrderBy(x => x).ToList();
                int v1 = s[0], v2 = s[1], v3 = s[2];
                int diff1 = Math.Abs(v2 - p.N) + v1;
                diff1 = diff1 > vertexCount ? p.N - diff1 : diff1;
                int diff2 = Math.Abs(v3 - p.N) + v2;
                diff2 = diff2 > vertexCount ? p.N - diff2 : diff2;
                int diff3 = Math.Abs(v3 - p.N) + v1;
                diff3 = diff3 > vertexCount ? p.N - diff3 : diff3;

                List<int> diffList = new List<int>() { diff1, diff2, diff3 };
                diffList.Sort();

                if (uaAcuteDiff.Contains(diffList)) {
                    scAcute.Increment();
                } else if (uaRightDiff.Contains(diffList)) {
                    scRight.Increment();
                } else if (uaObtuseDiff.Contains(diffList)) {
                    scObtuse.Increment();
                } else {
                    TriangleType tType = PropertiesOfTriangle.GetTriangleType(VL[diff1], VL[diff2], VL[diff3]);
                    if (tType == TriangleType.Acute) {
                        scAcute.Increment();
                        uaAcuteDiff.Add(diffList);
                    } else if (tType == TriangleType.Right) {
                        scRight.Increment();
                        uaRightDiff.Add(diffList);
                    } else if (tType == TriangleType.Obtuse) {
                        scObtuse.Increment();
                        uaObtuseDiff.Add(diffList);
                    }
                }
            }

            BigInteger sum = scObtuse.GetCount() + scRight.GetCount() + scAcute.GetCount();
            return (long)scAcute.GetCount();
        }
    }
}