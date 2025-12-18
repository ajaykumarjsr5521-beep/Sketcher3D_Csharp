using System;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cone : Shape
    {
        private readonly double mRadius;
        private readonly double mHeight;

        public Cone(string name, double radius, double height)
            : base("Cone", name)
        {
            mRadius = radius;
            mHeight = height;
        }

        protected override void Build()
        {
            int slices = 72;
            double dTheta = 2 * MathConstants.PI / slices;

            int apex = mTriag.AddPoint(new Point(0, 0, mHeight));
            int center = mTriag.AddPoint(new Point(0, 0, 0));

            int prev = -1, first = -1;

            for (int i = 0; i <= slices; i++)
            {
                double t = i * dTheta;
                int p = mTriag.AddPoint(
                    new Point(mRadius * Math.Cos(t),
                              mRadius * Math.Sin(t),
                              0));

                if (i == 0) first = p;
                if (prev != -1)
                {
                    mTriag.AddTriangle(prev, p, center); // base
                    mTriag.AddTriangle(prev, apex, p);   // side
                }
                prev = p;
            }
        }

        public override void Save(TextWriter w) =>
            w.WriteLine($"{GetTypeName()} {GetName()} R {mRadius} H {mHeight}");

        public override void SaveForGnu(TextWriter w) { }
    }
}
