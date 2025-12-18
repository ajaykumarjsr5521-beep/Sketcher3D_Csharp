using System;
using System.Collections.Generic;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Stores triangulated geometry data.
    /// Manages unique vertices, triangle indices, and per-triangle normals.
    /// </summary>
    public class Triangulation
    {
        // List of unique vertices
        private readonly List<Point> _points = new List<Point>();

        // Normal vectors (one per triangle)
        private readonly List<Point> _normals = new List<Point>();

        // Triangle index list (topology)
        private readonly List<Triangle> _triangles = new List<Triangle>();

        // Maps Point → index to avoid duplicate vertices
        private readonly Dictionary<Point, int> _pointIndex =
            new Dictionary<Point, int>();

        /// <summary>
        /// Creates an empty triangulation.
        /// </summary>
        public Triangulation() { }

        /// <summary>
        /// Returns the list of unique vertices.
        /// </summary>
        public List<Point> Points => _points;

        /// <summary>
        /// Returns the list of triangles (index-based).
        /// </summary>
        public List<Triangle> Triangles => _triangles;

        /// <summary>
        /// Returns the list of triangle normals.
        /// </summary>
        public List<Point> Normals => _normals;

        /// <summary>
        /// Adds a point to the triangulation.
        /// If the point already exists, returns its existing index.
        /// </summary>
        public int AddPoint(Point p)
        {
            // Check if point already exists
            int existing;
            if (_pointIndex.TryGetValue(p, out existing))
                return existing;

            // Add new unique point
            int index = _points.Count;
            _points.Add(p);
            _pointIndex[p] = index;
            return index;
        }

        /// <summary>
        /// Adds a triangle using indices into the point list.
        /// Automatically computes and stores its normal.
        /// </summary>
        public void AddTriangle(int a, int b, int c)
        {
            Triangle t = new Triangle(a, b, c);
            _triangles.Add(t);

            // Compute and store triangle normal
            _normals.Add(CalculateNormal(t));
        }

        /// <summary>
        /// Calculates the normalized surface normal of a triangle
        /// using the cross product of two edges.
        /// </summary>
        private Point CalculateNormal(Triangle tri)
        {
            // Edge vectors
            Point u = _points[tri.M2] - _points[tri.M1];
            Point v = _points[tri.M3] - _points[tri.M1];

            // Cross product (u × v)
            double nx = u.Y * v.Z - u.Z * v.Y;
            double ny = u.Z * v.X - u.X * v.Z;
            double nz = u.X * v.Y - u.Y * v.X;

            // Normalize the vector
            double len = Math.Sqrt(nx * nx + ny * ny + nz * nz);
            if (len == 0) len = 1; // avoid division by zero

            return new Point(nx / len, ny / len, nz / len);
        }

        /// <summary>
        /// Returns vertex data formatted for OpenGL.
        /// Output format: x,y,z per vertex, 3 vertices per triangle.
        /// </summary>
        public List<float> GetDataForOpenGl()
        {
            List<float> data = new List<float>(_triangles.Count * 9);

            foreach (Triangle t in _triangles)
            {
                // Vertex 1
                data.Add((float)_points[t.M1].X);
                data.Add((float)_points[t.M1].Y);
                data.Add((float)_points[t.M1].Z);

                // Vertex 2
                data.Add((float)_points[t.M2].X);
                data.Add((float)_points[t.M2].Y);
                data.Add((float)_points[t.M2].Z);

                // Vertex 3
                data.Add((float)_points[t.M3].X);
                data.Add((float)_points[t.M3].Y);
                data.Add((float)_points[t.M3].Z);
            }

            return data;
        }

        /// <summary>
        /// Returns normal data formatted for OpenGL.
        /// One normal per triangle.
        /// </summary>
        public List<float> GetNormalForOpenGl()
        {
            List<float> data = new List<float>(_normals.Count * 3);

            foreach (Point n in _normals)
            {
                data.Add((float)n.X);
                data.Add((float)n.Y);
                data.Add((float)n.Z);
            }

            return data;
        }
    }
}
