using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cone : Shape
    {
        private readonly double mRadius;
        private readonly double mHeight;

        public Cone(string name, double radius, double height) : base("Cone", name)
        {
            mRadius = radius; mHeight = height;
            Build();
        }

        public double GetRadius() => mRadius;
        public double GetHeight() => mHeight;
        public double GetSlantHeight() => Math.Sqrt(mRadius * mRadius + mHeight * mHeight);

        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} R {mRadius} H {mHeight}");

        protected override void Build()
        {
            double x = 0, y = 0, z = 0;
            Point origin = new Point(x, y, z);
            Point apex = new Point(x, y, mHeight);

            List<int> baseIdx = new List<int>();
            int originInd = mTriag.AddPoint(origin);
            int apexInd = mTriag.AddPoint(apex);

            baseIdx.Add(mTriag.AddPoint(new Point(x + mRadius * Math.Cos(0), y + mRadius * Math.Sin(0), z)));

            int number = 72;
            double dTheta = 2 * MathConstants.PI / number;

            for (int i = 1; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);

                baseIdx.Add(mTriag.AddPoint(new Point(x + x_, y + y_, z)));

                mTriag.AddTriangle(baseIdx[i - 1], originInd, baseIdx[i]); // base
                mTriag.AddTriangle(baseIdx[i - 1], baseIdx[i], apexInd);   // side
            }
        }

        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();
            double x = 0, y = 0, z = 0;

            int number = 72;
            double dTheta = 2 * MathConstants.PI / number;

            // base ring
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);
                pts.Add(new Point(x + x_, y + y_, z));
            }
            pts.Add(new Point(x + mRadius * Math.Cos(0), y + mRadius * Math.Sin(0), z));
            vec.Add(new List<Point>(pts)); pts.Clear();

            // lines up from base ring (kept same as your C++)
            for (int i = 0; i <= number; i++)
            {
                double theta = i * dTheta;
                double x_ = mRadius * Math.Cos(theta);
                double y_ = mRadius * Math.Sin(theta);
                pts.Add(new Point(x + x_, y + y_, z));
                pts.Add(new Point(x, y, z + mRadius));
            }
            vec.Add(new List<Point>(pts)); pts.Clear();

            foreach (List<Point> list in vec)
            {
                foreach (Point p in list) p.WriteXYZ(outw);
                outw.WriteLine(); outw.WriteLine();
            }
            outw.WriteLine(); outw.WriteLine();
        }
    }
}
