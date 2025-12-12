using System.Collections.Generic;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cuboid : Shape
    {
        private readonly double mLength, mWidth, mHeight;

        public Cuboid(string name, double length, double width, double height) : base("Cuboid", name)
        {
            mLength = length; mWidth = width; mHeight = height; Build();
        }

        public double GetLength() => mLength;
        public double GetWidth() => mWidth;
        public double GetHeight() => mHeight;

        public override void Save(TextWriter outw)
            => outw.WriteLine($"{GetTypeName()} {GetName()} L {mLength} W {mWidth} H {mHeight}");

        protected override void Build()
        {
            double x = 0, y = 0, z = 0;

            int p0 = mTriag.AddPoint(new Point(x, y, z));
            int p1 = mTriag.AddPoint(new Point(x + mLength, y, z));
            int p2 = mTriag.AddPoint(new Point(x + mLength, y + mWidth, z));
            mTriag.AddTriangle(p0, p2, p1);

            int p3 = mTriag.AddPoint(new Point(x, y + mWidth, z));
            mTriag.AddTriangle(p0, p3, p2);

            int p4 = mTriag.AddPoint(new Point(x, y, z + mHeight));
            int p5 = mTriag.AddPoint(new Point(x + mLength, y, z + mHeight));
            int p6 = mTriag.AddPoint(new Point(x + mLength, y + mWidth, z + mHeight));
            mTriag.AddTriangle(p4, p5, p6);

            int p7 = mTriag.AddPoint(new Point(x, y + mWidth, z + mHeight));
            mTriag.AddTriangle(p4, p6, p7);

            mTriag.AddTriangle(p7, p6, p2);
            mTriag.AddTriangle(p7, p2, p3);

            mTriag.AddTriangle(p0, p1, p5);
            mTriag.AddTriangle(p0, p5, p4);

            mTriag.AddTriangle(p5, p1, p2);
            mTriag.AddTriangle(p5, p2, p6);

            mTriag.AddTriangle(p0, p4, p7);
            mTriag.AddTriangle(p0, p7, p3);
        }

        public override void SaveForGnu(TextWriter outw)
        {
            List<List<Point>> vec = new List<List<Point>>();
            List<Point> pts = new List<Point>();
            double x = 0, y = 0, z = 0;

            pts.Add(new Point(x, y, z));
            pts.Add(new Point(x + mLength, y, z));
            pts.Add(new Point(x + mLength, y + mWidth, z));
            pts.Add(new Point(x, y + mWidth, z));
            pts.Add(new Point(x, y, z));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x, y, z + mHeight));
            pts.Add(new Point(x + mLength, y, z + mHeight));
            pts.Add(new Point(x + mLength, y + mWidth, z + mHeight));
            pts.Add(new Point(x, y + mWidth, z + mHeight));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x, y, z));
            pts.Add(new Point(x, y, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x + mLength, y, z));
            pts.Add(new Point(x + mLength, y, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x + mLength, y + mWidth, z));
            pts.Add(new Point(x + mLength, y + mWidth, z + mHeight));
            vec.Add(new List<Point>(pts)); pts.Clear();

            pts.Add(new Point(x, y + mWidth, z));
            pts.Add(new Point(x, y + mWidth, z + mHeight));
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
