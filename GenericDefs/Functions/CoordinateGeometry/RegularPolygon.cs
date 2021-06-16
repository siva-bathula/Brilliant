using System;
using System.Collections.Generic;

namespace GenericDefs.Functions.CoordinateGeometry
{
    /// <summary>
    /// Assumes centre of polygon at origin.
    /// All angles in radians.
    /// </summary>
    public class RegularPolygon
    {
        public int N { get; set; }
        public int Radius { get; set; }
        Dictionary<int, Vertex> Vertices = new Dictionary<int, Vertex>();
        double InteriorAngle { get; set; }
        /// <summary>
        /// Angle formed by joining two adjacent vertices w.r.t. the centre of polygon.
        /// </summary>
        double SideAngle { get; set; }
        public RegularPolygon(int numberOfSides, int radius = 1)
        {
            N = numberOfSides;
            Radius = radius;
            InteriorAngle = Math.PI * 1.0 * (N - 2) / N;
            SideAngle = 2 * Math.PI * 1.0 / N;
            GenerateVertices();
            CalculateVertexLengths();
        }

        bool _verticesGenerated = false;
        void GenerateVertices()
        {
            if (_verticesGenerated) return;
            for (int i = 0; i < N; i++)
            {
                Vertices.Add(i + 1, new Vertex() { Id = i + 1, X = Radius * Math.Sin(i * SideAngle), Y = Radius * Math.Cos(i * SideAngle) });
            }
            _verticesGenerated = true;
        }

        public List<int> GetVertexIds()
        {
            return new List<int>(Vertices.Keys);
        }

        /// <summary>
        /// Distance between any two vertices. Key is the Absolute diff. between the two vertex Id's.
        /// </summary>
        public Dictionary<int, double> VertexLengths = new Dictionary<int, double>();
        bool _vertexLengthsGenerated = false;
        void CalculateVertexLengths()
        {
            if (_vertexLengthsGenerated) return;
            int vertexCount = N / 2;
            Vertex v1 = Vertices[1];
            for (int i = 2; i <= N; i++)
            {
                Vertex v2 = Vertices[i];
                int diff = Math.Abs(v2.Id - N) + v1.Id;
                diff = diff > vertexCount ? N - diff : diff;
                if (!VertexLengths.ContainsKey(diff))
                {
                    VertexLengths.Add(diff, Math.Sqrt(Math.Pow((v1.X - v2.X), 2) + Math.Pow((v1.Y - v2.Y), 2)));
                }
            }
            _vertexLengthsGenerated = true;
        }

        public class Vertex
        {
            internal int Id { get; set; }
            internal double X { get; set; }
            internal double Y { get; set; }
        }
    }
}