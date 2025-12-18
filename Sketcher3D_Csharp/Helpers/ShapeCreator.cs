using System;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public static class ShapeCreator
    {
        private static void Check(double v)
        {
            if (v <= 0) throw new ArgumentException("Dimension must be positive");
        }

        public static Cube CreateCube(string n, double s)
        { Check(s); return new Cube(n, s); }

        public static Sphere CreateSphere(string n, double r)
        { Check(r); return new Sphere(n, r); }

        public static Cylinder CreateCylinder(string n, double r, double h)
        { Check(r); Check(h); return new Cylinder(n, r, h); }

        public static Cone CreateCone(string n, double r, double h)
        { Check(r); Check(h); return new Cone(n, r, h); }

        public static Cuboid CreateCuboid(string n, double l, double w, double h)
        { Check(l); Check(w); Check(h); return new Cuboid(n, l, w, h); }

        public static Pyramid CreatePyramid(string n, double l, double w, double h)
        { Check(l); Check(w); Check(h); return new Pyramid(n, l, w, h); }
    }
}
