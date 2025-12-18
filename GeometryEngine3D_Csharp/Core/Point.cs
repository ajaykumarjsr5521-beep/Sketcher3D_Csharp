using System;

namespace GeometryEngine3D_Csharp
{
    public class Point : IEquatable<Point>
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Point(double x = 0, double y = 0, double z = 0)
        {
            X = x; Y = y; Z = z;
        }

        public static Point operator -(Point a, Point b) =>
            new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public void WriteXYZ(System.IO.TextWriter w) =>
            w.WriteLine($"{X} {Y} {Z}");

        public bool Equals(Point p) =>
            p != null && X == p.X && Y == p.Y && Z == p.Z;

        public override bool Equals(object o) =>
            o is Point p && Equals(p);

        public override int GetHashCode()
        {
            unchecked
            {
                int h = 17;
                h = h * 31 + X.GetHashCode();
                h = h * 31 + Y.GetHashCode();
                h = h * 31 + Z.GetHashCode();
                return h;
            }
        }
    }
}
