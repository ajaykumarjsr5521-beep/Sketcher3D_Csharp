using System;

namespace GeometryEngine3D_Csharp
{
    public class Point : IEquatable<Point>
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public Point() : this(0, 0, 0) { }
        public Point(double x, double y, double z) { X = x; Y = y; Z = z; }

        public double Distance(Point other)
        {
            double dx = X - other.X, dy = Y - other.Y, dz = Z - other.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public void SetX(double x) => X = x;
        public void SetY(double y) => Y = y;
        public void SetZ(double z) => Z = z;

        public void WriteXYZ(System.IO.TextWriter w) => w.WriteLine($"{X} {Y} {Z}");

        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public bool Equals(Point other) => other != null && X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public override bool Equals(object obj) => obj is Point p && Equals(p);

        // Works on .NET Framework and C# 7.3
        public override int GetHashCode()
        {
            unchecked
            {
                long hx = BitConverter.DoubleToInt64Bits(X);
                long hy = BitConverter.DoubleToInt64Bits(Y);
                long hz = BitConverter.DoubleToInt64Bits(Z);

                int h = 17;
                h = h * 31 + (int)(hx ^ (hx >> 32));
                h = h * 31 + (int)(hy ^ (hy >> 32));
                h = h * 31 + (int)(hz ^ (hz >> 32));
                return h;
            }
        }
    }
}
