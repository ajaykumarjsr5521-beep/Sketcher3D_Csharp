using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Sphere : Shape
    {
        private readonly double mRadius;

        public Sphere(string name, double radius) : base("Sphere", name)
        {
            mRadius = radius; Build();
        }

        public double GetRadius() => mRadius;

        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} R {mRadius}");

        protected override void Build()
        {
            int stacks = 36;
            int number = 72;

            for (int i = 0; i < stacks; i++)
            {
                double iLat1 = MathConstants.PI * (-0.5 + (double)i / stacks);
                double iLat2 = MathConstants.PI * (-0.5 + (double)(i + 1) / stacks);

                double z1 = mRadius * Math.Sin(iLat1);
                double r1 = mRadius * Math.Cos(iLat1);

                double z2 = mRadius * Math.Sin(iLat2);
                double r2 = mRadius * Math.Cos(iLat2);

                for (int j = 0; j < number; j++)
                {
                    double jLat1 = 2 * MathConstants.PI * j / number;
                    double jLat2 = 2 * MathConstants.PI * (j + 1) / number;

                    int idx1 = mTriag.AddPoint(new Point(r1 * Math.Cos(jLat1), r1 * Math.Sin(jLat1), z1));
                    int idx2 = mTriag.AddPoint(new Point(r1 * Math.Cos(jLat2), r1 * Math.Sin(jLat2), z1));
                    int idx3 = mTriag.AddPoint(new Point(r2 * Math.Cos(jLat1), r2 * Math.Sin(jLat1), z2));
                    int idx4 = mTriag.AddPoint(new Point(r2 * Math.Cos(jLat2), r2 * Math.Sin(jLat2), z2));

                    mTriag.AddTriangle(idx1, idx2, idx3);
                    mTriag.AddTriangle(idx2, idx4, idx3);
                }
            }
        }

        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();

            int number = 72;
            double dTheta = MathConstants.PI / number;
            double dPhi = 2 * MathConstants.PI / number;
            double phi = 0, theta = 0;

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
                pts.Add(new Point(mRadius * Math.Sin(theta) * Math.Cos(0), mRadius * Math.Sin(theta) * Math.Sin(0), mRadius * Math.Cos(theta)));
                vec.Add(new List<Point>(pts)); pts.Clear();
            }

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
                pts.Add(new Point(mRadius * Math.Cos(theta), mRadius * Math.Sin(theta) * Math.Cos(0), mRadius * Math.Sin(theta) * Math.Sin(0)));
                vec.Add(new List<Point>(pts)); pts.Clear();
            }

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
                pts.Add(new Point(mRadius * Math.Sin(theta) * Math.Sin(0), mRadius * Math.Cos(theta), mRadius * Math.Sin(theta) * Math.Cos(0)));
                vec.Add(new List<Point>(pts)); pts.Clear();
            }

            foreach (List<Point> list in vec)
            {
                foreach (Point p in list) p.WriteXYZ(outw);
                outw.WriteLine(); outw.WriteLine();
            }
            outw.WriteLine(); outw.WriteLine();
        }
    }
}
