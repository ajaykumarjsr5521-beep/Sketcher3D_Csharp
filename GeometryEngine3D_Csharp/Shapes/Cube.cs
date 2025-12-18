using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cube : Shape
    {
        private readonly double mSide;

        public Cube(string name, double side)
            : base("Cube", name)
        {
            mSide = side;
        }

        protected override void Build()
        {
            double s = mSide;

            int p0 = mTriag.AddPoint(new Point(0, 0, 0));
            int p1 = mTriag.AddPoint(new Point(s, 0, 0));
            int p2 = mTriag.AddPoint(new Point(s, s, 0));
            int p3 = mTriag.AddPoint(new Point(0, s, 0));

            int p4 = mTriag.AddPoint(new Point(0, 0, s));
            int p5 = mTriag.AddPoint(new Point(s, 0, s));
            int p6 = mTriag.AddPoint(new Point(s, s, s));
            int p7 = mTriag.AddPoint(new Point(0, s, s));

            mTriag.AddTriangle(p0, p1, p2); mTriag.AddTriangle(p0, p2, p3);
            mTriag.AddTriangle(p4, p6, p5); mTriag.AddTriangle(p4, p7, p6);
            mTriag.AddTriangle(p0, p4, p5); mTriag.AddTriangle(p0, p5, p1);
            mTriag.AddTriangle(p1, p5, p6); mTriag.AddTriangle(p1, p6, p2);
            mTriag.AddTriangle(p2, p6, p7); mTriag.AddTriangle(p2, p7, p3);
            mTriag.AddTriangle(p3, p7, p4); mTriag.AddTriangle(p3, p4, p0);
        }

        public override void Save(TextWriter w) =>
            w.WriteLine($"{GetTypeName()} {GetName()} S {mSide}");

        public override void SaveForGnu(TextWriter w) { }
    }
}
