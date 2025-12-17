using System;                           // Provides ArgumentException
using GeometryEngine3D_Csharp;          // Geometry engine shape classes

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Factory-style helper class used to create valid Shape objects.
    /// 
    /// Responsibilities:
    /// - Centralize shape creation logic
    /// - Validate dimensions before creating shapes
    /// - Prevent invalid geometry from entering the engine
    /// </summary>
    public static class ShapeCreator
    {
        /// <summary>
        /// Checks whether a dimension value is positive.
        /// Used for validating shape parameters.
        /// </summary>
        private static bool Pos(double v)
        {
            // Geometry dimensions must be greater than zero
            return v > 0;
        }

        /// <summary>
        /// Creates a cube with the given side length.
        /// </summary>
        public static Cube CreateCube(string name, double side)
        {
            // Validate side length
            if (!Pos(side))
                throw new ArgumentException("Cube side must be positive");

            // Create and return cube
            return new Cube(name, side);
        }

        /// <summary>
        /// Creates a cuboid (rectangular box).
        /// </summary>
        public static Cuboid CreateCuboid(string name,
                                          double l,
                                          double w,
                                          double h)
        {
            // Validate all dimensions
            if (!Pos(l) || !Pos(w) || !Pos(h))
                throw new ArgumentException("Cuboid dimensions must be positive");

            return new Cuboid(name, l, w, h);
        }

        /// <summary>
        /// Creates a cylinder with radius and height.
        /// </summary>
        public static Cylinder CreateCylinder(string name,
                                              double r,
                                              double h)
        {
            // Validate radius and height
            if (!Pos(r) || !Pos(h))
                throw new ArgumentException("Cylinder radius and height must be positive");

            return new Cylinder(name, r, h);
        }

        /// <summary>
        /// Creates a cone with radius and height.
        /// </summary>
        public static Cone CreateCone(string name,
                                      double r,
                                      double h)
        {
            // Validate radius and height
            if (!Pos(r) || !Pos(h))
                throw new ArgumentException("Cone radius and height must be positive");

            return new Cone(name, r, h);
        }

        /// <summary>
        /// Creates a sphere with a given radius.
        /// </summary>
        public static Sphere CreateSphere(string name,
                                          double r)
        {
            // Validate radius
            if (!Pos(r))
                throw new ArgumentException("Sphere radius must be positive");

            return new Sphere(name, r);
        }

        /// <summary>
        /// Creates a pyramid with rectangular base.
        /// </summary>
        public static Pyramid CreatePyramid(string name,
                                            double l,
                                            double w,
                                            double h)
        {
            // Validate all dimensions
            if (!Pos(l) || !Pos(w) || !Pos(h))
                throw new ArgumentException("Pyramid dimensions must be positive");

            return new Pyramid(name, l, w, h);
        }
    }
}
