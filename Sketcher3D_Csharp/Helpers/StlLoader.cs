using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Media.Media3D;

namespace Sketcher3D_Csharp
{
    public static class StlLoader
    {
        public static MeshGeometry3D Load(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();

            if (ext == ".stl")
                return LoadBinaryStl(path);

            if (ext == ".obj")
                return LoadObj(path);

            throw new NotSupportedException("Only STL and OBJ supported");
        }

        private static MeshGeometry3D LoadBinaryStl(string file)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            using (BinaryReader br = new BinaryReader(File.OpenRead(file)))
            {
                br.ReadBytes(80);
                uint count = br.ReadUInt32();

                for (uint i = 0; i < count; i++)
                {
                    br.ReadSingle();
                    br.ReadSingle();
                    br.ReadSingle();

                    for (int v = 0; v < 3; v++)
                    {
                        mesh.Positions.Add(new Point3D(
                            br.ReadSingle(),
                            br.ReadSingle(),
                            br.ReadSingle()));

                        mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
                    }
                    br.ReadUInt16();
                }
            }
            return mesh;
        }

        private static MeshGeometry3D LoadObj(string file)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            List<Point3D> verts = new List<Point3D>();

            foreach (string raw in File.ReadLines(file))
            {
                string line = raw.Trim();

                if (line.StartsWith("v "))
                {
                    string[] p = line.Split(' ');
                    verts.Add(new Point3D(
                        double.Parse(p[1], CultureInfo.InvariantCulture),
                        double.Parse(p[2], CultureInfo.InvariantCulture),
                        double.Parse(p[3], CultureInfo.InvariantCulture)));
                }
                else if (line.StartsWith("f "))
                {
                    string[] p = line.Split(' ');
                    for (int i = 1; i <= 3; i++)
                    {
                        int idx = int.Parse(p[i].Split('/')[0]) - 1;
                        mesh.Positions.Add(verts[idx]);
                        mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
                    }
                }
            }
            return mesh;
        }
    }
}
