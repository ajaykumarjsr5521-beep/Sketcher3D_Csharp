using System;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Sphere : Shape
    {
        private readonly double mRadius;

        public Sphere(string name, double radius)
            : base("Sphere", name)
        {
            mRadius = radius;
        }

        protected override void Build()
        {
            int stacks = 36;
            int slices = 72;

            for (int i = 0; i < stacks; i++)
            {
                double lat1 = MathConstants.PI * (-0.5 + (double)i / stacks);
                double lat2 = MathConstants.PI * (-0.5 + (double)(i + 1) / stacks);

                double z1 = mRadius * Math.Sin(lat1);
                double r1 = mRadius * Math.Cos(lat1);
                double z2 = mRadius * Math.Sin(lat2);
                double r2 = mRadius * Math.Cos(lat2);

                for (int j = 0; j < slices; j++)
                {
                    double lng1 = 2 * MathConstants.PI * j / slices;
                    double lng2 = 2 * MathConstants.PI * (j + 1) / slices;

                    int p1 = mTriag.AddPoint(new Point(r1 * Math.Cos(lng1), r1 * Math.Sin(lng1), z1));
                    int p2 = mTriag.AddPoint(new Point(r1 * Math.Cos(lng2), r1 * Math.Sin(lng2), z1));
                    int p3 = mTriag.AddPoint(new Point(r2 * Math.Cos(lng1), r2 * Math.Sin(lng1), z2));
                    int p4 = mTriag.AddPoint(new Point(r2 * Math.Cos(lng2), r2 * Math.Sin(lng2), z2));

                    mTriag.AddTriangle(p1, p2, p3);
                    mTriag.AddTriangle(p2, p4, p3);
                }
            }
        }

        public override void Save(TextWriter w) =>
            w.WriteLine($"{GetTypeName()} {GetName()} R {mRadius}");

        public override void SaveForGnu(TextWriter w) { }
    }
}
