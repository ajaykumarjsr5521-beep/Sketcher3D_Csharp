using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a 3D Cube shape.
    /// Generates triangulated mesh data for rendering and export.
    /// </summary>
    public class Cube : Shape
    {
        // Length of one side of the cube
        private readonly double mSide;

        /// <summary>
        /// Creates a cube with the given name and side length.
        /// </summary>
        public Cube(string name, double side)
            : base("Cube", name)
        {
            mSide = side;

            // Build cube geometry
            Build();
        }

        /// <summary>
        /// Returns the side length of the cube.
        /// </summary>
        public double GetSide() => mSide;

        /// <summary>
        /// Saves cube parameters in text format.
        /// Used for custom file export.
        /// </summary>
        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} S {mSide}");

        /// <summary>
        /// Builds triangulated mesh for the cube.
        /// Each face is composed of two triangles.
        /// </summary>
        protected override void Build()
        {
            // Base corner of the cube
            double x = 0, y = 0, z = 0;

            // ============================
            // Bottom face (Z = 0)
            // ============================
            int p0 = mTriag.AddPoint(new Point(x, y, z));
            int p1 = mTriag.AddPoint(new Point(x + mSide, y, z));
            int p2 = mTriag.AddPoint(new Point(x + mSide, y + mSide, z));
            mTriag.AddTriangle(p0, p2, p1);

            int p3 = mTriag.AddPoint(new Point(x, y + mSide, z));
            mTriag.AddTriangle(p0, p3, p2);

            // ============================
            // Top face (Z = side)
            // ============================
            int p4 = mTriag.AddPoint(new Point(x, y, z + mSide));
            int p5 = mTriag.AddPoint(new Point(x + mSide, y, z + mSide));
            int p6 = mTriag.AddPoint(new Point(x + mSide, y + mSide, z + mSide));
            mTriag.AddTriangle(p4, p5, p6);

            int p7 = mTriag.AddPoint(new Point(x, y + mSide, z + mSide));
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
        /// Saves cube geometry in GNUPlot-compatible format.
        /// Outputs edges of the cube for visualization.
        /// </summary>
        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            double x = 0, y = 0, z = 0;

            // ============================
            // Bottom square
            // ============================
            pts.Add(new Point(x, y, z));
            pts.Add(new Point(x + mSide, y, z));
            pts.Add(new Point(x + mSide, y + mSide, z));
            pts.Add(new Point(x, y + mSide, z));
            pts.Add(new Point(x, y, z)); // close loop
            vec.Add(new List<Point>(pts)); pts.Clear();

            // ============================
            // Top square
            // ============================
            pts.Add(new Point(x, y, z + mSide));
            pts.Add(new Point(x + mSide, y, z + mSide));
            pts.Add(new Point(x + mSide, y + mSide, z + mSide));
            pts.Add(new Point(x, y + mSide, z + mSide));
            pts.Add(new Point(x, y, z + mSide)); // close loop
            vec.Add(new List<Point>(pts)); pts.Clear();

            // ============================
            // Vertical edges
            // ============================
            pts.Add(new Point(x, y, z));
            pts.Add(new Point(x, y, z + mSide));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x + mSide, y, z));
            pts.Add(new Point(x + mSide, y, z + mSide));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x + mSide, y + mSide, z));
            pts.Add(new Point(x + mSide, y + mSide, z + mSide));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x, y + mSide, z));
            pts.Add(new Point(x, y + mSide, z + mSide));
            vec.Add(new List<Point>(pts)); pts.Clear();

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
