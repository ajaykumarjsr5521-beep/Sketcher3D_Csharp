using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Pyramid : Shape
    {
        private readonly double mBaseLength, mBaseWidth, mHeight;

        public Pyramid(string name, double baseLength, double baseWidth, double height) : base("Pyramid", name)
        {
            mBaseLength = baseLength; mBaseWidth = baseWidth; mHeight = height; Build();
        }

        public double GetLength() => mBaseLength;
        public double GetWidth() => mBaseWidth;
        public double GetHeight() => mHeight;

        public double GetSlantHeight()
        {
            Point p1 = new Point(0, 0, 0);
            Point h = new Point(0, 0, mHeight);
            return p1.Distance(h);
        }

        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} L {mBaseLength} W {mBaseWidth} H {mHeight}");

        protected override void Build()
        {
            double x = 0, y = 0, z = 0;

            double halfL = mBaseLength / 2.0;
            double halfW = mBaseWidth / 2.0;

            int p0 = mTriag.AddPoint(new Point(x + halfL, y + halfW, z));
            int p1 = mTriag.AddPoint(new Point(x + halfL, y - halfW, z));
            int p2 = mTriag.AddPoint(new Point(x - halfL, y - halfW, z));
            int p3 = mTriag.AddPoint(new Point(x - halfL, y + halfW, z));

            mTriag.AddTriangle(p0, p2, p3);
            mTriag.AddTriangle(p2, p0, p1);

            int apex = mTriag.AddPoint(new Point(x, y, z + mHeight));
            mTriag.AddTriangle(p1, p0, apex);
            mTriag.AddTriangle(p2, p1, apex);
            mTriag.AddTriangle(p3, p2, apex);
            mTriag.AddTriangle(p0, p3, apex);
        }

        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();
            double x = 0, y = 0, z = 0;

            double halfL = mBaseLength / 2.0;
            double halfW = mBaseWidth / 2.0;

            pts.Add(new Point(x + halfL, y + halfW, z));
            pts.Add(new Point(x + halfL, y - halfW, z));
            pts.Add(new Point(x - halfL, y - halfW, z));
            pts.Add(new Point(x - halfL, y + halfW, z));
            pts.Add(new Point(x + halfL, y + halfW, z));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x + halfL, y + halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x + halfL, y - halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x - halfL, y - halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x - halfL, y + halfW, z));
            pts.Add(new Point(x, y, z + mHeight));
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
