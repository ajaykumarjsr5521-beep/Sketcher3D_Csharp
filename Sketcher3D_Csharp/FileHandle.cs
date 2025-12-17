using System.Collections.Generic;      // Provides IEnumerable<T> and other collections
using System.IO;                       // Provides file read/write support
using GeometryEngine3D_Csharp;         // Access to Shape and engine-level classes

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Handles saving and loading of geometry data.
    /// Works at engine level (not UI / rendering).
    /// </summary>
    public static class FileHandle
    {
        // ================= SAVE =================
        // Saves all shapes to a file in text format
        public static void Save(string path, IEnumerable<Shape> shapes)
        {
            // StreamWriter opens the file and automatically closes it after use
            using (var w = new StreamWriter(path))
            {
                // Loop through every shape in the scene
                foreach (var s in shapes)
                {
                    // Each shape knows how to save itself
                    // (polymorphism: Cube, Sphere, Cylinder, etc.)
                    s.Save(w);
                }
            }
        }

        // ================= LOAD =================
        // Loads shapes from a file and stores them in ShapeManager
        // NOTE: This is a basic placeholder implementation
        public static void Load(string path, ShapeManager manager)
        {
            // Clear existing shapes before loading new ones
            manager.Clear();

            // Read file line-by-line
            foreach (var line in File.ReadAllLines(path))
            {
                // TODO:
                // Parse 'line' to detect shape type and dimensions
                // Example future format:
                // Cube name=Box side=40

                // Temporary placeholder:
                // Always create a cube for each line
                var cube = ShapeCreator.CreateCube("Loaded", 40);

                // Add the created shape to the manager
                manager.Add(cube);
            }
        }
    }
}
