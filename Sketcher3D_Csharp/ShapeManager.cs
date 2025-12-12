using System.Collections.Generic;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public class ShapeManager
    {
        private readonly List<Shape> _shapes = new List<Shape>();
        public void AddShape(Shape s) => _shapes.Add(s);
        public IReadOnlyList<Shape> GetShapes() => _shapes;
        public void Clear() => _shapes.Clear();
        public Shape GetLastShape() => _shapes.Count > 0 ? _shapes[_shapes.Count - 1] : null;
    }
}
