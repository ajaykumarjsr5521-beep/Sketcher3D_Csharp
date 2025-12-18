using System;
using System.IO;

namespace GeometryEngine3D_Csharp
{
    public class Cylinder : Shape
    {
        private readonly double mRadius, mHeight;

        public Cylinder(string name, double radius, double height)
            : base("Cylinder", name)
        {
            mRadius = radius;
            mHeight = height;
        }

        protected override void Build()
        {
            int slices = 72;
            double dTheta = 2 * MathConstants.PI / slices;

            int bottomC = mTriag.AddPoint(new Point(0, 0, 0));
            int topC = mTriag.AddPoint(new Point(0, 0, mHeight));

            int prevB = -1, prevT = -1;

            for (int i = 0; i <= slices; i++)
            {
                double t = i * dTheta;
                double x = mRadius * Math.Cos(t);
                double y = mRadius * Math.Sin(t);

                int b = mTriag.AddPoint(new Point(x, y, 0));
                int tP = mTriag.AddPoint(new Point(x, y, mHeight));

                if (prevB != -1)
                {
                    mTriag.AddTriangle(prevB, b, bottomC);
                    mTriag.AddTriangle(prevT, topC, tP);
                    mTriag.AddTriangle(prevB, prevT, b);
                    mTriag.AddTriangle(b, prevT, tP);
                }

                prevB = b;
                prevT = tP;
            }
        }

        public override void Save(TextWriter w) =>
            w.WriteLine($"{GetTypeName()} {GetName()} R {mRadius} H {mHeight}");

        public override void SaveForGnu(TextWriter w) { }
    }
}
