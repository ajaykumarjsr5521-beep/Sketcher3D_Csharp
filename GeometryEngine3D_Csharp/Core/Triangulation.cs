using System;
using System.Collections.Generic;

namespace GeometryEngine3D_Csharp
{
    public class Triangulation
    {
        private readonly List<Point> _points = new List<Point>();
        private readonly List<Triangle> _triangles = new List<Triangle>();
        private readonly Dictionary<Point, int> _index = new Dictionary<Point, int>();

        public List<Point> Points { get { return _points; } }
        public List<Triangle> Triangles { get { return _triangles; } }

        public int AddPoint(Point p)
        {
            int idx;
            if (_index.TryGetValue(p, out idx))
                return idx;

            idx = _points.Count;
            _points.Add(p);
            _index[p] = idx;
            return idx;
        }

        public void AddTriangle(int a, int b, int c)
        {
            _triangles.Add(new Triangle(a, b, c));
        }
    }
}
