using System.Windows.Media.Media3D;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public static class TriangulationMeshBuilder
    {
        public static MeshGeometry3D ToMesh(Triangulation tri)
        {
            var mesh = new MeshGeometry3D();
            foreach (var p in tri.Points)
                mesh.Positions.Add(new Point3D(p.X, p.Y, p.Z));
            foreach (var t in tri.Triangles)
            {
                mesh.TriangleIndices.Add(t.M1);
                mesh.TriangleIndices.Add(t.M2);
                mesh.TriangleIndices.Add(t.M3);
            }
            // normals optional in WPF; positions+indices are enough
            return mesh;
        }
    }
}
