using System.Windows.Media.Media3D;     // WPF 3D types (MeshGeometry3D, Point3D)
using GeometryEngine3D_Csharp;          // Geometry engine types (Triangulation, Point, Triangle)

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Converts geometry-engine triangulation data
    /// into a WPF-compatible MeshGeometry3D.
    /// 
    /// Acts as the bridge between:
    /// - Geometry engine (pure math)
    /// - WPF renderer (visual representation)
    /// </summary>
    public static class TriangulationMeshBuilder
    {
        /// <summary>
        /// Builds a WPF MeshGeometry3D from a Triangulation object.
        /// </summary>
        public static MeshGeometry3D ToMesh(Triangulation tri)
        {
            // Create an empty WPF mesh
            MeshGeometry3D mesh = new MeshGeometry3D();

            // =====================================================
            // Copy vertex positions
            // =====================================================
            // Each engine Point becomes a WPF Point3D
            foreach (Point p in tri.Points)
            {
                mesh.Positions.Add(
                    new Point3D(p.X, p.Y, p.Z));
            }

            // =====================================================
            // Copy triangle index data
            // =====================================================
            // Each triangle refers to indices of the vertex list
            foreach (Triangle t in tri.Triangles)
            {
                mesh.TriangleIndices.Add(t.M1);
                mesh.TriangleIndices.Add(t.M2);
                mesh.TriangleIndices.Add(t.M3);
            }

            // Return completed mesh to renderer
            return mesh;
        }
    }
}
