using System.IO;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Abstract base class for all 3D shapes.
    /// Defines common properties and behavior such as
    /// triangulation, naming, and file export.
    /// </summary>
    public abstract class Shape
    {
        // Logical type of the shape (Cube, Cone, Cylinder, etc.)
        private readonly string _type;

        // User-defined name of the shape instance
        private readonly string _name;

        // Stores triangulated geometry (points and triangles)
        protected Triangulation mTriag = new Triangulation();

        /// <summary>
        /// Initializes base shape information.
        /// </summary>
        protected Shape(string type, string name)
        {
            _type = type;
            _name = name;
        }

        /// <summary>
        /// Builds the triangulated geometry of the shape.
        /// Must be implemented by derived shape classes.
        /// </summary>
        protected abstract void Build();

        /// <summary>
        /// Returns the user-defined name of the shape.
        /// </summary>
        public string GetName() => _name;

        /// <summary>
        /// Returns the type of the shape (e.g., Cube, Sphere).
        /// </summary>
        public string GetTypeName() => _type;

        /// <summary>
        /// Returns the triangulated mesh data of the shape.
        /// Used for rendering and export.
        /// </summary>
        public Triangulation GetTriangulation() => mTriag;

        /// <summary>
        /// Saves shape parameters in a custom text format.
        /// Implemented by each concrete shape.
        /// </summary>
        public abstract void Save(TextWriter w);

        /// <summary>
        /// Saves shape geometry in GNUPlot-compatible format.
        /// Used for visualization and debugging.
        /// </summary>
        public abstract void SaveForGnu(TextWriter w);
    }
}
