using System.Collections.Generic;
using System.IO;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public static class FileHandle
    {
        public static void Save(string path, IEnumerable<Shape> shapes)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                foreach (Shape s in shapes)
                {
                    s.Save(w);
                }
            }
        }

        public static void Load(string path, ShapeManager manager)
        {
            manager.Clear();

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] p = line.Split(' ');
                string type = p[0];
                string name = p[1];

                try
                {
                    switch (type)
                    {
                        case "Cube":
                            manager.Add(
                                ShapeCreator.CreateCube(
                                    name,
                                    double.Parse(p[3])));
                            break;

                        case "Sphere":
                            manager.Add(
                                ShapeCreator.CreateSphere(
                                    name,
                                    double.Parse(p[3])));
                            break;

                        case "Cylinder":
                            manager.Add(
                                ShapeCreator.CreateCylinder(
                                    name,
                                    double.Parse(p[3]),
                                    double.Parse(p[5])));
                            break;

                        case "Cone":
                            manager.Add(
                                ShapeCreator.CreateCone(
                                    name,
                                    double.Parse(p[3]),
                                    double.Parse(p[5])));
                            break;

                        case "Cuboid":
                            manager.Add(
                                ShapeCreator.CreateCuboid(
                                    name,
                                    double.Parse(p[3]),
                                    double.Parse(p[5]),
                                    double.Parse(p[7])));
                            break;

                        case "Pyramid":
                            manager.Add(
                                ShapeCreator.CreatePyramid(
                                    name,
                                    double.Parse(p[3]),
                                    double.Parse(p[5]),
                                    double.Parse(p[7])));
                            break;
                    }
                }
                catch
                {
                    // ignore malformed line
                }
            }
        }
    }
}
