using System;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public static class ShapeCreator
    {
        private static bool Pos(double v) { return v > 0; }

        public static Cube CreateCube(string name, double side)
        {
            if (!Pos(side)) throw new ArgumentException();
            return new Cube(name, side);
        }

        public static Cuboid CreateCuboid(string name, double l, double w, double h)
        {
            if (!Pos(l) || !Pos(w) || !Pos(h)) throw new ArgumentException();
            return new Cuboid(name, l, w, h);
        }

        public static Cylinder CreateCylinder(string name, double r, double h)
        {
            if (!Pos(r) || !Pos(h)) throw new ArgumentException();
            return new Cylinder(name, r, h);
        }

        public static Cone CreateCone(string name, double r, double h)
        {
            if (!Pos(r) || !Pos(h)) throw new ArgumentException();
            return new Cone(name, r, h);
        }

        public static Sphere CreateSphere(string name, double r)
        {
            if (!Pos(r)) throw new ArgumentException();
            return new Sphere(name, r);
        }

        public static Pyramid CreatePyramid(string name, double l, double w, double h)
        {
            if (!Pos(l) || !Pos(w) || !Pos(h)) throw new ArgumentException();
            return new Pyramid(name, l, w, h);
        }
    }
}
