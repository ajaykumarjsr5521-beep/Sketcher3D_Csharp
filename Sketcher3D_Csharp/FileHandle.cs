using System.Collections.Generic;
using System.IO;
using GeometryEngine3D_Csharp;

namespace Sketcher3D_Csharp
{
    public static class FileHandle
    {
        // ================= SAVE =================
        // Save pure geometry (engine level)
        public static void Save(string path, IEnumerable<Shape> shapes)
        {
            using (var w = new StreamWriter(path))
            {
                foreach (var s in shapes)
                {
                    // Each shape already knows how to save itself
                    s.Save(w);
                }
            }
        }

        // ================= LOAD =================
        // Simple demo loader (currently loads cubes only)
        public static void Load(string path,
                                ShapeManager manager)
        {
            manager.Clear();

            foreach (var line in File.ReadAllLines(path))
            {
                // TODO: extend to parse different shapes
                // For now: placeholder cube
                var cube = ShapeCreator.CreateCube("Loaded", 40);
                manager.Add(cube);
            }
        }
    }
}
