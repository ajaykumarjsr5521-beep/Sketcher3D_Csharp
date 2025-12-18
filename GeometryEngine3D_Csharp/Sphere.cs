using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a 3D Sphere.
    /// Generates triangulated mesh using latitude–longitude subdivision.
    /// </summary>
    public class Sphere : Shape
    {
        // Radius of the sphere
        private readonly double mRadius;

        /// <summary>
        /// Creates a sphere with given name and radius.
        /// </summary>
        public Sphere(string name, double radius)
            : base("Sphere", name)
        {
            mRadius = radius;

            // Build sphere geometry
            Build();
        }

        /// <summary>
        /// Returns the radius of the sphere.
        /// </summary>
        public double GetRadius() => mRadius;

        /// <summary>
        /// Saves sphere parameters in text format.
        /// Used for custom file export.
        /// </summary>
        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} R {mRadius}");

        /// <summary>
        /// Builds triangulated mesh for the sphere.
        /// Uses stacks (latitude) and slices (longitude).
        /// </summary>
        protected override void Build()
        {
            // Number of horizontal slices (latitude)
            int stacks = 36;

            // Number of vertical slices (longitude)
            int number = 72;

            // Loop through latitude bands
            for (int i = 0; i < stacks; i++)
            {
                // Latitude angles
                double iLat1 = MathConstants.PI * (-0.5 + (double)i / stacks);
                double iLat2 = MathConstants.PI * (-0.5 + (double)(i + 1) / stacks);

                // Z and radius at current and next latitude
                double z1 = mRadius * Math.Sin(iLat1);
                double r1 = mRadius * Math.Cos(iLat1);

                double z2 = mRadius * Math.Sin(iLat2);
                double r2 = mRadius * Math.Cos(iLat2);

                // Loop through longitude bands
                for (int j = 0; j < number; j++)
                {
                    // Longitude angles
                    double jLat1 = 2 * MathConstants.PI * j / number;
                    double jLat2 = 2 * MathConstants.PI * (j + 1) / number;

                    // Four vertices of the quad
                    int idx1 = mTriag.AddPoint(
                        new Point(r1 * Math.Cos(jLat1), r1 * Math.Sin(jLat1), z1));

                    int idx2 = mTriag.AddPoint(
                        new Point(r1 * Math.Cos(jLat2), r1 * Math.Sin(jLat2), z1));

                    int idx3 = mTriag.AddPoint(
                        new Point(r2 * Math.Cos(jLat1), r2 * Math.Sin(jLat1), z2));

                    int idx4 = mTriag.AddPoint(
                        new Point(r2 * Math.Cos(jLat2), r2 * Math.Sin(jLat2), z2));

                    // Two triangles per quad
                    mTriag.AddTriangle(idx1, idx2, idx3);
                    mTriag.AddTriangle(idx2, idx4, idx3);
                }
            }
        }

        /// <summary>
        /// Saves sphere geometry in GNUPlot-compatible format.
        /// Outputs latitude and longitude curves for wireframe visualization.
        /// </summary>
        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            int number = 72;

            // Angle increments
            double dTheta = MathConstants.PI / number;
            double dPhi = 2 * MathConstants.PI / number;

            double theta = 0, phi = 0;

            // ============================
            // Latitude curves
            // ============================
            for (int i = 0; i <= number; i++)
            {
                theta = i * dTheta;

                for (int j = 0; j <= number; j++)
                {
                    phi = j * dPhi;

                    double x_ = mRadius * Math.Sin(theta) * Math.Cos(phi);
                    double y_ = mRadius * Math.Sin(theta) * Math.Sin(phi);
                    double z_ = mRadius * Math.Cos(theta);

                    pts.Add(new Point(x_, y_, z_));
                }

                // Close loop
                pts.Add(new Point(
                    mRadius * Math.Sin(theta) * Math.Cos(0),
                    mRadius * Math.Sin(theta) * Math.Sin(0),
                    mRadius * Math.Cos(theta)));

                vec.Add(new List<Point>(pts));
                pts.Clear();
            }

            // ============================
            // Longitude curves (YZ plane)
            // ============================
            for (int i = 0; i <= number; i++)
            {
                theta = i * dTheta;

                for (int j = 0; j <= number; j++)
                {
                    phi = j * dPhi;

                    double x_ = mRadius * Math.Cos(theta);
                    double y_ = mRadius * Math.Sin(theta) * Math.Cos(phi);
                    double z_ = mRadius * Math.Sin(theta) * Math.Sin(phi);

                    pts.Add(new Point(x_, y_, z_));
                }

                // Close loop
                pts.Add(new Point(
                    mRadius * Math.Cos(theta),
                    mRadius * Math.Sin(theta) * Math.Cos(0),
                    mRadius * Math.Sin(theta) * Math.Sin(0)));

                vec.Add(new List<Point>(pts));
                pts.Clear();
            }

            // ============================
            // Longitude curves (XZ plane)
            // ============================
            for (int i = 0; i <= number; i++)
            {
                theta = i * dTheta;

                for (int j = 0; j <= number; j++)
                {
                    phi = j * dPhi;

                    double x_ = mRadius * Math.Sin(theta) * Math.Sin(phi);
                    double y_ = mRadius * Math.Cos(theta);
                    double z_ = mRadius * Math.Sin(theta) * Math.Cos(phi);

                    pts.Add(new Point(x_, y_, z_));
                }

                // Close loop
                pts.Add(new Point(
                    mRadius * Math.Sin(theta) * Math.Sin(0),
                    mRadius * Math.Cos(theta),
                    mRadius * Math.Sin(theta) * Math.Cos(0)));

                vec.Add(new List<Point>(pts));
                pts.Clear();
            }

            // ============================
            // Write all curve sets
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
