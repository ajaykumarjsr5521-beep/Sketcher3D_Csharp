using System.Collections.Generic;      // Provides List<T> and IReadOnlyList<T>
using GeometryEngine3D_Csharp;         // Geometry engine Shape base class

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Manages all geometry shapes present in the scene.
    /// 
    /// Responsibilities:
    /// - Store created shapes
    /// - Provide read-only access to shapes
    /// - Clear scene data when required
    /// 
    /// This class acts as the bridge between
    /// the geometry engine and the UI layer.
    /// </summary>
    public class ShapeManager
    {
        // Internal list holding all shapes
        // Private to prevent uncontrolled modification
        private readonly List<Shape> _shapes =
            new List<Shape>();

        /// <summary>
        /// Adds a new shape to the manager.
        /// Called whenever a shape is created.
        /// </summary>
        public void Add(Shape shape)
        {
            // Store shape in internal collection
            _shapes.Add(shape);
        }

        /// <summary>
        /// Provides read-only access to stored shapes.
        /// Prevents external code from modifying the list directly.
        /// </summary>
        public IReadOnlyList<Shape> Shapes
        {
            get { return _shapes; }
        }

        /// <summary>
        /// Removes all shapes from the manager.
        /// Used when clearing or resetting the scene.
        /// </summary>
        public void Clear()
        {
            _shapes.Clear();
        }
    }
}
