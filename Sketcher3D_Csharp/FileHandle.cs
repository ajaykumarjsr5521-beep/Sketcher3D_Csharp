using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using GeometryEngine3D_Csharp;   // uses Shape, Triangulation, Point, Triangle

namespace Sketcher3D_Csharp
{
    public static class FileHandle
    {
        // ------------------------ Save .skt (your custom text) ------------------------
        public static bool SaveToFile(string fileName, IList<Shape> shapes)
        {
            try
            {
                using (var w = new StreamWriter(fileName))
                {
                    foreach (var s in shapes)
                        s.Save(w);                   // calls engine Save(TextWriter)
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ------------------------ Save for GNUPlot (.dat) ------------------------
        public static bool SaveToFileGNUPlot(string fileName, IList<Shape> shapes)
        {
            try
            {
                using (var w = new StreamWriter(fileName))
                {
                    foreach (var s in shapes)
                    {
                        w.WriteLine("#" + s.GetTypeName());
                        w.WriteLine("#" + s.GetName());
                        s.SaveForGnu(w);            // calls engine SaveForGnu(TextWriter)
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ------------------------ Read ASCII STL -> Triangulation ------------------------
        public static void ReadSTL(string fileName, Triangulation triangulation)
        {
            using (var reader = new StreamReader(fileName))
            {
                string line;
                var pts = new List<GeometryEngine3D_Csharp.Point>(3);
                var inv = CultureInfo.InvariantCulture;

                // Default normal in case STL is malformed
                GeometryEngine3D_Csharp.Point normal = new GeometryEngine3D_Csharp.Point(0, 0, 1);

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("facet normal", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 5)
                        {
                            double nx, ny, nz;
                            double.TryParse(parts[2], NumberStyles.Float, inv, out nx);
                            double.TryParse(parts[3], NumberStyles.Float, inv, out ny);
                            double.TryParse(parts[4], NumberStyles.Float, inv, out nz);
                            normal = new GeometryEngine3D_Csharp.Point(nx, ny, nz);
                        }
                    }
                    else if (line.StartsWith("vertex", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            double x = 0, y = 0, z = 0;
                            double.TryParse(parts[1], NumberStyles.Float, inv, out x);
                            double.TryParse(parts[2], NumberStyles.Float, inv, out y);
                            double.TryParse(parts[3], NumberStyles.Float, inv, out z);

                            pts.Add(new GeometryEngine3D_Csharp.Point(x, y, z));

                            if (pts.Count == 3)
                            {
                                int a = triangulation.AddPoint(pts[0]);
                                int b = triangulation.AddPoint(pts[1]);
                                int c = triangulation.AddPoint(pts[2]);
                                triangulation.AddTriangle(a, b, c);   // normal is computed by engine
                                pts.Clear();
                            }
                        }
                    }
                }
            }
        }

        // ------------------------ Write ASCII STL from shapes ------------------------
        public static bool WriteSTL(string fileName, IList<Shape> shapes)
        {
            try
            {
                var inv = CultureInfo.InvariantCulture;

                using (var w = new StreamWriter(fileName))
                {
                    w.WriteLine("solid sketcher");

                    foreach (var shape in shapes)
                    {
                        var tri = shape.GetTriangulation();
                        var points = tri.Points;
                        var triangles = tri.Triangles;
                        var normals = tri.Normals;

                        int ni = 0;
                        foreach (var t in triangles)
                        {
                            // Use stored normal if available; otherwise compute quickly
                            GeometryEngine3D_Csharp.Point n;
                            if (ni < normals.Count) n = normals[ni++];
                            else
                            {
                                var u = points[t.M2] - points[t.M1];
                                var v = points[t.M3] - points[t.M1];
                                double nx = u.Y * v.Z - u.Z * v.Y;
                                double ny = u.Z * v.X - u.X * v.Z;
                                double nz = u.X * v.Y - u.Y * v.X;
                                double len = Math.Sqrt(nx * nx + ny * ny + nz * nz); if (len == 0) len = 1;
                                n = new GeometryEngine3D_Csharp.Point(nx / len, ny / len, nz / len);
                            }

                            var p1 = points[t.M1];
                            var p2 = points[t.M2];
                            var p3 = points[t.M3];

                            w.WriteLine($"  facet normal {n.X.ToString(inv)} {n.Y.ToString(inv)} {n.Z.ToString(inv)}");
                            w.WriteLine("    outer loop");
                            w.WriteLine($"      vertex {p1.X.ToString(inv)} {p1.Y.ToString(inv)} {p1.Z.ToString(inv)}");
                            w.WriteLine($"      vertex {p2.X.ToString(inv)} {p2.Y.ToString(inv)} {p2.Z.ToString(inv)}");
                            w.WriteLine($"      vertex {p3.X.ToString(inv)} {p3.Y.ToString(inv)} {p3.Z.ToString(inv)}");
                            w.WriteLine("    endloop");
                            w.WriteLine("  endfacet");
                        }
                    }

                    w.WriteLine("endsolid sketcher");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
