using System.IO;

namespace GeometryEngine3D_Csharp
{
    public abstract class Shape
    {
        private readonly string _type;
        private readonly string _name;

        protected Triangulation mTriag = new Triangulation();

        protected Shape(string type, string name)
        {
            _type = type;
            _name = name;
        }

        protected abstract void Build();

        public string GetName() => _name;
        public string GetTypeName() => _type;
        public Triangulation GetTriangulation() => mTriag;

        public abstract void Save(TextWriter w);
        public abstract void SaveForGnu(TextWriter w);
    }
}
