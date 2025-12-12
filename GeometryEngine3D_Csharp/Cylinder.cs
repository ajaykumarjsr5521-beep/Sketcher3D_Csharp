using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cylinder : Shape
    {
        private readonly double mRadius, mHeight;

        public Cylinder(string name, double radius, double height) : base("Cylinder", name)
        {
            mRadius = radius; mHeight = height; Build();
        }

        public double GetRadius() => mRadius;
        public double GetHeight() => mHeight;

        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} R {mRadius} H {mHeight}");

        protected override void Build()
        {
            double x = 0, y = 0, z = 0;
            var baseCenter = new Point(x, y, z);
            var topCenter = new Point(x, y, z + mHeight);

            var bIdx = new List<int>();
            var tIdx = new List<int>();

            int baseC = mTriag.AddPoint(baseCenter);
            int topC = mTriag.AddPoint(topCenter);

            bIdx.Add(mTriag.AddPoint(new Point(x + mRadius * Math.Cos(0), y + mRadius * Math.Sin(0), z)));
            tIdx.Add(mTriag.AddPoint(new Point(x + mRadius * Math.Cos(0), y + mRadius * Math.Sin(0), z + mHeight)));

            int number = 72;
            double dTheta = 2 * MathConstants.PI / number;

            for (int i = 1; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                bIdx.Add(mTriag.AddPoint(new Point(x + x_, y + y_, z)));
                tIdx.Add(mTriag.AddPoint(new Point(x + x_, y + y_, z + mHeight)));

                mTriag.AddTriangle(bIdx[i], bIdx[i - 1], baseC);
                mTriag.AddTriangle(bIdx[i - 1], bIdx[i], tIdx[i]);
                mTriag.AddTriangle(bIdx[i - 1], tIdx[i], tIdx[i - 1]);
                mTriag.AddTriangle(topC, tIdx[i - 1], tIdx[i]);
            }
        }

        public override void SaveForGnu(TextWriter outw)
        {
            var vec = new List<List<Point>>();
            var pts = new List<Point>();
            double x = 0, y = 0, z = 0;

            int number = 72;
            double dTheta = 2 * MathConstants.PI / number;

            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);
                pts.Add(new Point(x + x_, y + y_, z));
            }
            pts.Add(new Point(x + mRadius * Math.Cos(0), y + mRadius * Math.Sin(0), z));
            vec.Add(new List<Point>(pts)); pts.Clear();

            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);
                pts.Add(new Point(x + x_, y + y_, z + mHeight));
            }
            pts.Add(new Point(x + mRadius * Math.Cos(0), y + mRadius * Math.Sin(0), z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);
                pts.Add(new Point(x + x_, y + y_, z));
                pts.Add(new Point(x + x_, y + y_, z + mHeight));
                vec.Add(new List<Point>(pts)); pts.Clear();
            }

            foreach (var list in vec)
            {
                foreach (var p in list) p.WriteXYZ(outw);
                outw.WriteLine(); outw.WriteLine();
            }
            outw.WriteLine(); outw.WriteLine();
        }
    }
}

