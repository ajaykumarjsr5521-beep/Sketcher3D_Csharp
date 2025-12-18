using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Pyramid : Shape
    {
        private readonly double mLength, mWidth, mHeight;

        public Pyramid(string name, double l, double w, double h)
            : base("Pyramid", name)
        {
            mLength = l; mWidth = w; mHeight = h;
        }

        protected override void Build()
        {
            double hl = mLength / 2;
            double hw = mWidth / 2;

            int p0 = mTriag.AddPoint(new Point(hl, hw, 0));
            int p1 = mTriag.AddPoint(new Point(hl, -hw, 0));
            int p2 = mTriag.AddPoint(new Point(-hl, -hw, 0));
            int p3 = mTriag.AddPoint(new Point(-hl, hw, 0));

            int apex = mTriag.AddPoint(new Point(0, 0, mHeight));

            mTriag.AddTriangle(p0, p1, p2);
            mTriag.AddTriangle(p0, p2, p3);

            mTriag.AddTriangle(p0, apex, p1);
            mTriag.AddTriangle(p1, apex, p2);
            mTriag.AddTriangle(p2, apex, p3);
            mTriag.AddTriangle(p3, apex, p0);
        }

        public override void Save(TextWriter w) =>
            w.WriteLine($"{GetTypeName()} {GetName()} L {mLength} W {mWidth} H {mHeight}");

        public override void SaveForGnu(TextWriter w) { }
    }
}
