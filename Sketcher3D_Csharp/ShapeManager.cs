using System.Collections.Generic;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public class ShapeManager
    {
        private readonly List<Shape> _shapes =
            new List<Shape>();

        public void Add(Shape shape)
        {
            _shapes.Add(shape);
        }

        public IReadOnlyList<Shape> Shapes
        {
            get { return _shapes; }
        }

        public void Clear()
        {
            _shapes.Clear();
        }
    }
}
