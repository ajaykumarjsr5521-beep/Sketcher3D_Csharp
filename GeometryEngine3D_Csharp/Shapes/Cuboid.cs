using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cuboid : Shape
    {
        private readonly double mLength, mWidth, mHeight;

        public Cuboid(string name, double l, double w, double h)
            : base("Cuboid", name)
        {
            mLength = l; mWidth = w; mHeight = h;
        }

        protected override void Build()
        {
            double l = mLength, w = mWidth, h = mHeight;

            int p0 = mTriag.AddPoint(new Point(0, 0, 0));
            int p1 = mTriag.AddPoint(new Point(l, 0, 0));
            int p2 = mTriag.AddPoint(new Point(l, w, 0));
            int p3 = mTriag.AddPoint(new Point(0, w, 0));

            int p4 = mTriag.AddPoint(new Point(0, 0, h));
            int p5 = mTriag.AddPoint(new Point(l, 0, h));
            int p6 = mTriag.AddPoint(new Point(l, w, h));
            int p7 = mTriag.AddPoint(new Point(0, w, h));

            mTriag.AddTriangle(p0, p1, p2); mTriag.AddTriangle(p0, p2, p3);
            mTriag.AddTriangle(p4, p6, p5); mTriag.AddTriangle(p4, p7, p6);
            mTriag.AddTriangle(p0, p4, p5); mTriag.AddTriangle(p0, p5, p1);
            mTriag.AddTriangle(p1, p5, p6); mTriag.AddTriangle(p1, p6, p2);
            mTriag.AddTriangle(p2, p6, p7); mTriag.AddTriangle(p2, p7, p3);
            mTriag.AddTriangle(p3, p7, p4); mTriag.AddTriangle(p3, p4, p0);
        }

        public override void Save(TextWriter w) =>
            w.WriteLine($"{GetTypeName()} {GetName()} L {mLength} W {mWidth} H {mHeight}");

        public override void SaveForGnu(TextWriter w) { }
    }
}
