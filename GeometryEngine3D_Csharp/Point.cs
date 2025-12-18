using System;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a point in 3D space.
    /// Used as a vertex in triangulation and geometry calculations.
    /// </summary>
    public class Point : IEquatable<Point>
    {
        // X, Y, Z coordinates of the point
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        /// <summary>
        /// Creates a point at the origin (0,0,0).
        /// </summary>
        public Point() : this(0, 0, 0) { }

        /// <summary>
        /// Creates a point with given coordinates.
        /// </summary>
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Computes Euclidean distance between this point and another point.
        /// </summary>
        public double Distance(Point other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            double dz = Z - other.Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Updates X coordinate.
        /// </summary>
        public void SetX(double x) => X = x;

        /// <summary>
        /// Updates Y coordinate.
        /// </summary>
        public void SetY(double y) => Y = y;

        /// <summary>
        /// Updates Z coordinate.
        /// </summary>
        public void SetZ(double z) => Z = z;

        /// <summary>
        /// Writes point coordinates as "X Y Z".
        /// Used for file export (GNUPlot / debug).
        /// </summary>
        public void WriteXYZ(System.IO.TextWriter w)
            => w.WriteLine($"{X} {Y} {Z}");

        /// <summary>
        /// Subtracts two points to produce a vector.
        /// </summary>
        public static Point operator -(Point a, Point b)
            => new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Compares two points for coordinate equality.
        /// Required for dictionary and hash-based collections.
        /// </summary>
        public bool Equals(Point other)
            => other != null &&
               X.Equals(other.X) &&
               Y.Equals(other.Y) &&
               Z.Equals(other.Z);

        /// <summary>
        /// Overrides Object.Equals for correct value comparison.
        /// </summary>
        public override bool Equals(object obj)
            => obj is Point p && Equals(p);

        /// <summary>
        /// Generates a hash code based on X, Y, Z values.
        /// Enables use of Point as a dictionary key.
        /// Works on .NET Framework and C# 7.3.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                // Convert doubles to bit patterns for stable hashing
                long hx = BitConverter.DoubleToInt64Bits(X);
                long hy = BitConverter.DoubleToInt64Bits(Y);
                long hz = BitConverter.DoubleToInt64Bits(Z);

                // Combine hashes using prime multipliers
                int h = 17;
                h = h * 31 + (int)(hx ^ (hx >> 32));
                h = h * 31 + (int)(hy ^ (hy >> 32));
                h = h * 31 + (int)(hz ^ (hz >> 32));

                return h;
            }
        }
    }
}
