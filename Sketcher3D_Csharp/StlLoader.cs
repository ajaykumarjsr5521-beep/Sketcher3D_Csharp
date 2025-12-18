using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Media.Media3D;

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Loader for:
    /// - Binary STL
    /// - OBJ (v / f)
    /// No ASCII STL support by design.
    /// </summary>
    public static class StlLoader
    {
        /// <summary>
        /// Entry point called from MainWindow
        /// </summary>
        public static MeshGeometry3D Load(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            if (ext == ".stl")
                return LoadBinaryStl(filePath);

            if (ext == ".obj")
                return LoadObj(filePath);

            throw new NotSupportedException("Only Binary STL and OBJ are supported.");
        }

        // =====================================================
        // BINARY STL LOADER
        // =====================================================
        private static MeshGeometry3D LoadBinaryStl(string filePath)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            using (BinaryReader br = new BinaryReader(File.OpenRead(filePath)))
            {
                // Minimum valid binary STL size
                if (br.BaseStream.Length < 84)
                    throw new InvalidDataException("Invalid Binary STL file.");

                // Skip 80-byte header
                br.ReadBytes(80);

                uint triangleCount = br.ReadUInt32();

                long expectedSize = 84 + (long)triangleCount * 50;
                if (br.BaseStream.Length < expectedSize)
                    throw new InvalidDataException("Corrupted Binary STL file.");

                for (uint i = 0; i < triangleCount; i++)
                {
                    // Skip normal (3 floats)
                    br.ReadSingle();
                    br.ReadSingle();
                    br.ReadSingle();

                    // Read 3 vertices
                    for (int v = 0; v < 3; v++)
                    {
                        float x = br.ReadSingle();
                        float y = br.ReadSingle();
                        float z = br.ReadSingle();

                        mesh.Positions.Add(new Point3D(x, y, z));
                        mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
                    }

                    // Skip attribute byte count
                    br.ReadUInt16();
                }
            }

            return mesh;
        }

        // =====================================================
        // OBJ LOADER (v / f only, triangles)
        // =====================================================
        private static MeshGeometry3D LoadObj(string filePath)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            List<Point3D> vertices = new List<Point3D>();

            foreach (string raw in File.ReadLines(filePath))
            {
                string line = raw.Trim();

                // Vertex
                if (line.StartsWith("v "))
                {
                    string[] p = line.Split(
                        new[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);

                    double x = double.Parse(p[1], CultureInfo.InvariantCulture);
                    double y = double.Parse(p[2], CultureInfo.InvariantCulture);
                    double z = double.Parse(p[3], CultureInfo.InvariantCulture);

                    vertices.Add(new Point3D(x, y, z));
                }
                // Face (triangle only)
                else if (line.StartsWith("f "))
                {
                    string[] p = line.Split(
                        new[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);

                    // f v or f v/vt/vn
                    for (int i = 1; i <= 3; i++)
                    {
                        int idx = int.Parse(p[i].Split('/')[0]) - 1;
                        Point3D v = vertices[idx];

                        mesh.Positions.Add(v);
                        mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
                    }
                }
            }

            return mesh;
        }
    }
}
