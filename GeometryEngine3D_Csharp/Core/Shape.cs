using System.IO;

namespace GeometryEngine3D_Csharp
{
    public abstract class Shape
    {
        protected readonly Triangulation mTriag = new Triangulation();

        private readonly string _type;
        private readonly string _name;

        protected Shape(string type, string name)
        {
            _type = type;
            _name = name;
            Build();
        }

        protected abstract void Build();

        public string GetTypeName() => _type;
        public string GetName() => _name;
        public Triangulation GetTriangulation() => mTriag;

        public abstract void Save(TextWriter w);
        public abstract void SaveForGnu(TextWriter w);
    }
}
