using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a rectangular-base pyramid.
    /// Generates triangulated mesh data for rendering and export.
    /// </summary>
    public class Pyramid : Shape
    {
        // Base dimensions and height of the pyramid
        private readonly double mBaseLength;
        private readonly double mBaseWidth;
        private readonly double mHeight;

        /// <summary>
        /// Creates a pyramid with given name, base size, and height.
        /// </summary>
        public Pyramid(string name, double baseLength, double baseWidth, double height)
            : base("Pyramid", name)
        {
            mBaseLength = baseLength;
            mBaseWidth = baseWidth;
            mHeight = height;

            // Build pyramid geometry
            Build();
        }

        /// <summary>
        /// Returns base length (X direction).
        /// </summary>
        public double GetLength() => mBaseLength;

        /// <summary>
        /// Returns base width (Y direction).
        /// </summary>
        public double GetWidth() => mBaseWidth;

        /// <summary>
        /// Returns height of the pyramid (Z direction).
        /// </summary>
        public double GetHeight() => mHeight;

        /// <summary>
        /// Returns slant height from base center to apex.
        /// </summary>
        public double GetSlantHeight()
        {
            Point p1 = new Point(0, 0, 0);
            Point h = new Point(0, 0, mHeight);
            return p1.Distance(h);
        }

        /// <summary>
        /// Saves pyramid parameters in text format.
        /// Used for custom file export.
        /// </summary>
        public override void Save(TextWriter outw)
            => outw.WriteLine(
                $"{GetTypeName()} {GetName()} L {mBaseLength} W {mBaseWidth} H {mHeight}"
            );

        /// <summary>
        /// Builds triangulated mesh for the pyramid.
        /// Base is divided into two triangles.
        /// Each side face is one triangle connecting to the apex.
        /// </summary>
        protected override void Build()
        {
            // Base reference coordinates
            double x = 0, y = 0, z = 0;

            // Half dimensions for centering the base at origin
            double halfL = mBaseLength / 2.0;
            double halfW = mBaseWidth / 2.0;

            // ============================
            // Base vertices (centered)
            // ============================
            int p0 = mTriag.AddPoint(new Point(x + halfL, y + halfW, z));
            int p1 = mTriag.AddPoint(new Point(x + halfL, y - halfW, z));
            int p2 = mTriag.AddPoint(new Point(x - halfL, y - halfW, z));
            int p3 = mTriag.AddPoint(new Point(x - halfL, y + halfW, z));

            // Base face (two triangles)
            mTriag.AddTriangle(p0, p2, p3);
            mTriag.AddTriangle(p2, p0, p1);

            // ============================
            // Apex and side faces
            // ============================
            int apex = mTriag.AddPoint(new Point(x, y, z + mHeight));

            mTriag.AddTriangle(p1, p0, apex);
            mTriag.AddTriangle(p2, p1, apex);
            mTriag.AddTriangle(p3, p2, apex);
            mTriag.AddTriangle(p0, p3, apex);
        }

        /// <summary>
        /// Saves pyramid geometry in GNUPlot-compatible format.
        /// Outputs base edges and lines from base to apex.
        /// </summary>
        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            double x = 0, y = 0, z = 0;
            double halfL = mBaseLength / 2.0;
            double halfW = mBaseWidth / 2.0;

            // ============================
            // Base rectangle
            // ============================
            pts.Add(new Point(x + halfL, y + halfW, z));
            pts.Add(new Point(x + halfL, y - halfW, z));
            pts.Add(new Point(x - halfL, y - halfW, z));
            pts.Add(new Point(x - halfL, y + halfW, z));
            pts.Add(new Point(x + halfL, y + halfW, z)); // close loop
            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Side edges (base to apex)
            // ============================
            pts.Add(new Point(x + halfL, y + halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            pts.Add(new Point(x + halfL, y - halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            pts.Add(new Point(x - halfL, y - halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts));
            pts.Clear();

            pts.Add(new Point(x - halfL, y + halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
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
