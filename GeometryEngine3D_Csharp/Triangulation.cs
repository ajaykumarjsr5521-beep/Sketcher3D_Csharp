using System;
using System.Collections.Generic;

namespace GeometryEngine3D_Csharp
{
    public class Triangulation
    {
        private readonly List<Point> _points = new List<Point>();
        private readonly List<Point> _normals = new List<Point>();
        private readonly List<Triangle> _triangles = new List<Triangle>();
        private readonly Dictionary<Point, int> _pointIndex = new Dictionary<Point, int>();

        public Triangulation() { }

        public List<Point> Points => _points;
        public List<Triangle> Triangles => _triangles;
        public List<Point> Normals => _normals;

        public int AddPoint(Point p)
        {
            int existing;
            if (_pointIndex.TryGetValue(p, out existing))
                return existing;

            int index = _points.Count;
            _points.Add(p);
            _pointIndex[p] = index;
            return index;
        }

        public void AddTriangle(int a, int b, int c)
        {
            Triangle t = new Triangle(a, b, c);
            _triangles.Add(t);
            _normals.Add(CalculateNormal(t));
        }

        private Point CalculateNormal(Triangle tri)
        {
            Point u = _points[tri.M2] - _points[tri.M1];
            Point v = _points[tri.M3] - _points[tri.M1];

            double nx = u.Y * v.Z - u.Z * v.Y;
            double ny = u.Z * v.X - u.X * v.Z;
            double nz = u.X * v.Y - u.Y * v.X;

            double len = Math.Sqrt(nx * nx + ny * ny + nz * nz);
            if (len == 0) len = 1;
            return new Point(nx / len, ny / len, nz / len);
        }

        public List<float> GetDataForOpenGl()
        {
            List<float> data = new List<float>(_triangles.Count * 9);
            foreach (Triangle t in _triangles)
            {
                data.Add((float)_points[t.M1].X);
                data.Add((float)_points[t.M1].Y);
                data.Add((float)_points[t.M1].Z);

                data.Add((float)_points[t.M2].X);
                data.Add((float)_points[t.M2].Y);
                data.Add((float)_points[t.M2].Z);

                data.Add((float)_points[t.M3].X);
                data.Add((float)_points[t.M3].Y);
                data.Add((float)_points[t.M3].Z);
            }
            return data;
        }

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
