using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a 3D Cone shape.
    /// Generates triangulated geometry for rendering and export.
    /// </summary>
    public class Cone : Shape
    {
        // Radius of the cone base
        private readonly double mRadius;

        // Height of the cone (base to apex)
        private readonly double mHeight;

        /// <summary>
        /// Creates a cone with given name, radius, and height.
        /// </summary>
        public Cone(string name, double radius, double height)
            : base("Cone", name)
        {
            mRadius = radius;
            mHeight = height;

            // Build triangulated geometry
            Build();
        }

        /// <summary>
        /// Returns base radius of the cone.
        /// </summary>
        public double GetRadius() => mRadius;

        /// <summary>
        /// Returns height of the cone.
        /// </summary>
        public double GetHeight() => mHeight;

        /// <summary>
        /// Returns slant height of the cone.
        /// √(radius² + height²)
        /// </summary>
        public double GetSlantHeight()
            => Math.Sqrt(mRadius * mRadius + mHeight * mHeight);

        /// <summary>
        /// Saves cone parameters in text format.
        /// Used for custom file export.
        /// </summary>
        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} R {mRadius} H {mHeight}");

        /// <summary>
        /// Builds triangulated mesh for the cone.
        /// Generates base triangles and side triangles.
        /// </summary>
        protected override void Build()
        {
            // Base center coordinates
            double x = 0, y = 0, z = 0;

            // Base center point
            Point origin = new Point(x, y, z);

            // Apex point (top of cone)
            Point apex = new Point(x, y, mHeight);

            // Stores indices of base ring points
            List<int> baseIdx = new List<int>();

            // Add base center and apex to triangulation
            int originInd = mTriag.AddPoint(origin);
            int apexInd = mTriag.AddPoint(apex);

            // Add first base ring point (angle = 0)
            baseIdx.Add(
                mTriag.AddPoint(
                    new Point(
                        x + mRadius * Math.Cos(0),
                        y + mRadius * Math.Sin(0),
                        z
                    )
                )
            );

            // Number of segments around the base
            int number = 72;

            // Angle step for each segment
            double dTheta = 2 * MathConstants.PI / number;

            // Generate base ring and triangles
            for (int i = 1; i <= number; i++)
            {
                double theta = i * dTheta;

                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                // Add next base point
                baseIdx.Add(
                    mTriag.AddPoint(
                        new Point(x + x_, y + y_, z)
                    )
                );

                // Base triangle (fan from center)
                mTriag.AddTriangle(
                    baseIdx[i - 1],
                    originInd,
                    baseIdx[i]
                );

                // Side triangle (connect base edge to apex)
                mTriag.AddTriangle(
                    baseIdx[i - 1],
                    baseIdx[i],
                    apexInd
                );
            }
        }

        /// <summary>
        /// Saves cone geometry in GNUPlot-compatible format.
        /// Used for visualization and debugging.
        /// </summary>
        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            double x = 0, y = 0, z = 0;

            int number = 72;
            double dTheta = 2 * MathConstants.PI / number;

            // ============================
            // Base circular ring
            // ============================
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                pts.Add(new Point(x + x_, y + y_, z));
            }

            // Close the loop
            pts.Add(new Point(
                x + mRadius * Math.Cos(0),
                y + mRadius * Math.Sin(0),
                z
            ));

            vec.Add(new List<Point>(pts));
            pts.Clear();

            // ============================
            // Lines from base ring to apex
            // ============================
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                // Base point
                pts.Add(new Point(x + x_, y + y_, z));

                // Apex point (vertical line)
                pts.Add(new Point(x, y, z + mRadius));
            }

            vec.Add(new List<Point>(pts));
            pts.Clear();

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
