using System;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public static class ShapeCreator
    {
        private static bool Pos(double v) => v > 0;

        public static Cuboid CreateCuboid(string name, double l, double w, double h)
        { if (!Pos(l) || !Pos(w) || !Pos(h)) throw new ArgumentException("Invalid Cuboid"); return new Cuboid(name, l, w, h); }

        public static Cube CreateCube(string name, double side)
        { if (!Pos(side)) throw new ArgumentException("Invalid Cube"); return new Cube(name, side); }

        public static Sphere CreateSphere(string name, double r)
        { if (!Pos(r)) throw new ArgumentException("Invalid Sphere"); return new Sphere(name, r); }

        public static Cylinder CreateCylinder(string name, double r, double h)
        { if (!Pos(r) || !Pos(h)) throw new ArgumentException("Invalid Cylinder"); return new Cylinder(name, r, h); }

        public static Cone CreateCone(string name, double r, double h)
        { if (!Pos(r) || !Pos(h)) throw new ArgumentException("Invalid Cone"); return new Cone(name, r, h); }

        public static Pyramid CreatePyramid(string name, double bl, double bw, double h)
        { if (!Pos(bl) || !Pos(bw) || !Pos(h)) throw new ArgumentException("Invalid Pyramid"); return new Pyramid(name, bl, bw, h); }
    }
}
