using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a 3D Cylinder shape.
    /// Generates triangulated mesh data for rendering and export.
    /// </summary>
    public class Cylinder : Shape
    {
        // Radius of the cylinder base
        private readonly double mRadius;

        // Height of the cylinder
        private readonly double mHeight;

        /// <summary>
        /// Creates a cylinder with given name, radius, and height.
        /// </summary>
        public Cylinder(string name, double radius, double height)
            : base("Cylinder", name)
        {
            mRadius = radius;
            mHeight = height;

            // Build cylinder geometry
            Build();
        }

        /// <summary>
        /// Returns the radius of the cylinder.
        /// </summary>
        public double GetRadius() => mRadius;

        /// <summary>
        /// Returns the height of the cylinder.
        /// </summary>
        public double GetHeight() => mHeight;

        /// <summary>
        /// Saves cylinder parameters in text format.
        /// Used for custom file export.
        /// </summary>
        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} R {mRadius} H {mHeight}");

        /// <summary>
        /// Builds triangulated mesh for the cylinder.
        /// Includes base, top, and curved side surface.
        /// </summary>
        protected override void Build()
        {
            // Base reference coordinates
            double x = 0, y = 0, z = 0;

            // Center of base and top circles
            Point baseCenter = new Point(x, y, z);
            Point topCenter = new Point(x, y, z + mHeight);

            // Indices for base and top ring vertices
            List<int> bIdx = new List<int>();
            List<int> tIdx = new List<int>();

            // Add center points
            int baseC = mTriag.AddPoint(baseCenter);
            int topC = mTriag.AddPoint(topCenter);

            // Add first ring point (angle = 0)
            bIdx.Add(
                mTriag.AddPoint(
                    new Point(
                        x + mRadius * Math.Cos(0),
                        y + mRadius * Math.Sin(0),
                        z
                    )
                )
            );

            tIdx.Add(
                mTriag.AddPoint(
                    new Point(
                        x + mRadius * Math.Cos(0),
                        y + mRadius * Math.Sin(0),
                        z + mHeight
                    )
                )
            );

            // Number of circular segments
            int number = 72;

            // Angle increment
            double dTheta = 2 * MathConstants.PI / number;

            // Generate rings and triangles
            for (int i = 1; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                // Add base and top ring points
                bIdx.Add(mTriag.AddPoint(new Point(x + x_, y + y_, z)));
                tIdx.Add(mTriag.AddPoint(new Point(x + x_, y + y_, z + mHeight)));

                // Base face triangle
                mTriag.AddTriangle(bIdx[i], bIdx[i - 1], baseC);

                // Side surface (two triangles per segment)
                mTriag.AddTriangle(bIdx[i - 1], bIdx[i], tIdx[i]);
                mTriag.AddTriangle(bIdx[i - 1], tIdx[i], tIdx[i - 1]);

                // Top face triangle
                mTriag.AddTriangle(topC, tIdx[i - 1], tIdx[i]);
            }
        }

        /// <summary>
        /// Saves cylinder geometry in GNUPlot-compatible format.
        /// Outputs circular rings and vertical lines for visualization.
        /// </summary>
        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            double x = 0, y = 0, z = 0;

            int number = 72;
            double dTheta = 2 * MathConstants.PI / number;

            // ============================
            // Base circle
            // ============================
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                pts.Add(new Point(x + x_, y + y_, z));
            }

            // Close loop
            pts.Add(new Point(
                x + mRadius * Math.Cos(0),
                y + mRadius * Math.Sin(0),
                z
            ));

            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Top circle
            // ============================
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                pts.Add(new Point(x + x_, y + y_, z + mHeight));
            }

            // Close loop
            pts.Add(new Point(
                x + mRadius * Math.Cos(0),
                y + mRadius * Math.Sin(0),
                z + mHeight
            ));

            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Vertical side lines
            // ============================
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                pts.Add(new Point(x + x_, y + y_, z));
                pts.Add(new Point(x + x_, y + y_, z + mHeight));

                vec.Add(new List<Point>(pts));
                pts.Clear();
            }

            // ============================
            // Write all point sets
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
