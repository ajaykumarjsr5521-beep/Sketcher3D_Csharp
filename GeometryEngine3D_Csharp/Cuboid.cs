using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a 3D Cuboid (rectangular box).
    /// Generates triangulated mesh data for rendering and export.
    /// </summary>
    public class Cuboid : Shape
    {
        // Dimensions of the cuboid
        private readonly double mLength;
        private readonly double mWidth;
        private readonly double mHeight;

        /// <summary>
        /// Creates a cuboid with given name and dimensions.
        /// </summary>
        public Cuboid(string name, double length, double width, double height)
            : base("Cuboid", name)
        {
            mLength = length;
            mWidth = width;
            mHeight = height;

            // Build cuboid geometry
            Build();
        }

        /// <summary>
        /// Returns the length (X direction).
        /// </summary>
        public double GetLength() => mLength;

        /// <summary>
        /// Returns the width (Y direction).
        /// </summary>
        public double GetWidth() => mWidth;

        /// <summary>
        /// Returns the height (Z direction).
        /// </summary>
        public double GetHeight() => mHeight;

        /// <summary>
        /// Saves cuboid parameters in text format.
        /// Used for custom file export.
        /// </summary>
        public override void Save(TextWriter outw)
            => outw.WriteLine(
                $"{GetTypeName()} {GetName()} L {mLength} W {mWidth} H {mHeight}"
            );

        /// <summary>
        /// Builds triangulated mesh for the cuboid.
        /// Each rectangular face is represented by two triangles.
        /// </summary>
        protected override void Build()
        {
            // Base corner of the cuboid
            double x = 0, y = 0, z = 0;

            // ============================
            // Bottom face (Z = 0)
            // ============================
            int p0 = mTriag.AddPoint(new Point(x, y, z));
            int p1 = mTriag.AddPoint(new Point(x + mLength, y, z));
            int p2 = mTriag.AddPoint(new Point(x + mLength, y + mWidth, z));
            mTriag.AddTriangle(p0, p2, p1);

            int p3 = mTriag.AddPoint(new Point(x, y + mWidth, z));
            mTriag.AddTriangle(p0, p3, p2);

            // ============================
            // Top face (Z = height)
            // ============================
            int p4 = mTriag.AddPoint(new Point(x, y, z + mHeight));
            int p5 = mTriag.AddPoint(new Point(x + mLength, y, z + mHeight));
            int p6 = mTriag.AddPoint(new Point(x + mLength, y + mWidth, z + mHeight));
            mTriag.AddTriangle(p4, p5, p6);

            int p7 = mTriag.AddPoint(new Point(x, y + mWidth, z + mHeight));
            mTriag.AddTriangle(p4, p6, p7);

            // ============================
            // Side faces
            // ============================

            // Back face
            mTriag.AddTriangle(p7, p6, p2);
            mTriag.AddTriangle(p7, p2, p3);

            // Front face
            mTriag.AddTriangle(p0, p1, p5);
            mTriag.AddTriangle(p0, p5, p4);

            // Right face
            mTriag.AddTriangle(p5, p1, p2);
            mTriag.AddTriangle(p5, p2, p6);

            // Left face
            mTriag.AddTriangle(p0, p4, p7);
            mTriag.AddTriangle(p0, p7, p3);
        }

        /// <summary>
        /// Saves cuboid geometry in GNUPlot-compatible format.
        /// Outputs only edges for wireframe visualization.
        /// </summary>
        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            double x = 0, y = 0, z = 0;

            // ============================
            // Bottom rectangle
            // ============================
            pts.Add(new Point(x, y, z));
            pts.Add(new Point(x + mLength, y, z));
            pts.Add(new Point(x + mLength, y + mWidth, z));
            pts.Add(new Point(x, y + mWidth, z));
            pts.Add(new Point(x, y, z)); // close loop
            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Top rectangle
            // ============================
            pts.Add(new Point(x, y, z + mHeight));
            pts.Add(new Point(x + mLength, y, z + mHeight));
            pts.Add(new Point(x + mLength, y + mWidth, z + mHeight));
            pts.Add(new Point(x, y + mWidth, z + mHeight));
            pts.Add(new Point(x, y, z + mHeight)); // close loop
            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Vertical edges
            // ============================
            pts.Add(new Point(x, y, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            pts.Add(new Point(x + mLength, y, z));
            pts.Add(new Point(x + mLength, y, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            pts.Add(new Point(x + mLength, y + mWidth, z));
            pts.Add(new Point(x + mLength, y + mWidth, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            pts.Add(new Point(x, y + mWidth, z));
            pts.Add(new Point(x, y + mWidth, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Write all edge sets
            // ============================
            foreach (List<Point> list in vec)
            {
                foreach (Point p in list)
                    p.WriteXYZ(outw);

                outw.WriteLine();
                outw.WriteLine();
            }

            outw.WriteLine();
            outw.WriteLine();
        }
    }
}
