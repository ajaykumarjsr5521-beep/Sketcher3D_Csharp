using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Microsoft.Win32;

// Engine namespaces
using GeometryEngine3D_Csharp;
using EnginePoint = GeometryEngine3D_Csharp.Point;
using EngineTriangulation = GeometryEngine3D_Csharp.Triangulation;
using EngineShape = GeometryEngine3D_Csharp.Shape;

namespace Sketcher3D_Csharp
{
    public partial class MainWindow : Window
    {
        private readonly ShapeManager _shapeManager = new ShapeManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        // ---------- Helpers ----------
        private static GeometryModel3D MakeModel(MeshGeometry3D mesh, Color color)
        {
            var mat = new DiffuseMaterial(new SolidColorBrush(color));
            return new GeometryModel3D(mesh, mat) { BackMaterial = mat };
        }

        private void AddShapeToScene(EngineShape s, Color? color = null)
        {
            var mesh = TriangulationMeshBuilder.ToMesh(s.GetTriangulation());
            SceneRoot.Children.Add(new ModelVisual3D
            {
                Content = MakeModel(mesh, color ?? Colors.SkyBlue)
            });
        }

        private void AddTriToScene(EngineTriangulation tri, Color? color = null)
        {
            var mesh = TriangulationMeshBuilder.ToMesh(tri);
            SceneRoot.Children.Add(new ModelVisual3D
            {
                Content = MakeModel(mesh, color ?? Colors.LightGreen)
            });
        }

        private static void Info(Window owner, string m)
            => MessageBox.Show(owner, m, "Info");
        private static void Warn(Window owner, string m)
            => MessageBox.Show(owner, m, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        // ---------- Toolbar: create shapes ----------
        private void Cuboid_Click(object sender, RoutedEventArgs e)
        {
            double L, W, H;
            if (!InputDialogs.AskThree("Cuboid", "Length", 50, "Width", 30, "Height", 20, out L, out W, out H)) return;
            try
            {
                var s = ShapeCreator.CreateCuboid("Cuboid1", L, W, H);
                _shapeManager.AddShape(s);
                AddShapeToScene(s, Colors.CornflowerBlue);
            }
            catch (Exception ex) { Warn(this, ex.Message); }
        }

        private void Cube_Click(object sender, RoutedEventArgs e)
        {
            double side;
            if (!InputDialogs.AskOne("Cube", "Side", 40, out side)) return;
            try
            {
                var s = ShapeCreator.CreateCube("Cube1", side);
                _shapeManager.AddShape(s);
                AddShapeToScene(s, Colors.Orange);
            }
            catch (Exception ex) { Warn(this, ex.Message); }
        }

        private void Cylinder_Click(object sender, RoutedEventArgs e)
        {
            double r, h;
            if (!InputDialogs.AskTwo("Cylinder", "Radius", 20, "Height", 50, out r, out h)) return;
            try
            {
                var s = ShapeCreator.CreateCylinder("Cylinder1", r, h);
                _shapeManager.AddShape(s);
                AddShapeToScene(s, Colors.MediumSeaGreen);
            }
            catch (Exception ex) { Warn(this, ex.Message); }
        }

        private void Cone_Click(object sender, RoutedEventArgs e)
        {
            double r, h;
            if (!InputDialogs.AskTwo("Cone", "Radius", 20, "Height", 50, out r, out h)) return;
            try
            {
                var s = ShapeCreator.CreateCone("Cone1", r, h);
                _shapeManager.AddShape(s);
                AddShapeToScene(s, Colors.MediumVioletRed);
            }
            catch (Exception ex) { Warn(this, ex.Message); }
        }

        private void Sphere_Click(object sender, RoutedEventArgs e)
        {
            double r;
            if (!InputDialogs.AskOne("Sphere", "Radius", 25, out r)) return;
            try
            {
                var s = ShapeCreator.CreateSphere("Sphere1", r);
                _shapeManager.AddShape(s);
                AddShapeToScene(s, Colors.Goldenrod);
            }
            catch (Exception ex) { Warn(this, ex.Message); }
        }

        private void Pyramid_Click(object sender, RoutedEventArgs e)
        {
            double L, W, H;
            if (!InputDialogs.AskThree("Pyramid", "Base Length", 40, "Base Width", 40, "Height", 50, out L, out W, out H)) return;
            try
            {
                var s = ShapeCreator.CreatePyramid("Pyramid1", L, W, H);
                _shapeManager.AddShape(s);
                AddShapeToScene(s, Colors.SlateBlue);
            }
            catch (Exception ex) { Warn(this, ex.Message); }
        }

        // ---------- File menu ----------
        private void SaveSkt_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Sketch (*.skt)|*.skt" };
            if (dlg.ShowDialog() != true) return;

            var list = new List<EngineShape>(_shapeManager.GetShapes());
            var ok = FileHandle.SaveToFile(dlg.FileName, list);
            if (ok) Info(this, "Saved .skt"); else Warn(this, "Save failed");
        }

        private void SaveGnu_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "GNU Plot (*.dat)|*.dat" };
            if (dlg.ShowDialog() != true) return;

            var list = new List<EngineShape>(_shapeManager.GetShapes());
            var ok = FileHandle.SaveToFileGNUPlot(dlg.FileName, list);
            if (ok) Info(this, "Saved .dat for GNUPlot"); else Warn(this, "Save failed");
        }

        private void SaveStl_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "STL (*.stl)|*.stl" };
            if (dlg.ShowDialog() != true) return;

            var list = new List<EngineShape>(_shapeManager.GetShapes());
            var ok = FileHandle.WriteSTL(dlg.FileName, list);
            if (ok) Info(this, "Saved STL"); else Warn(this, "Save failed");
        }

        private void LoadStl_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "STL (*.stl)|*.stl" };
            if (dlg.ShowDialog() != true) return;

            var tri = new EngineTriangulation();
            try
            {
                FileHandle.ReadSTL(dlg.FileName, tri);
                if (tri.Points.Count > 0)
                {
                    AddTriToScene(tri, Colors.LightGreen);
                    Info(this, "STL loaded.");
                }
                else Warn(this, "No triangles found.");
            }
            catch (Exception ex) { Warn(this, "Load failed: " + ex.Message); }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Clear();
            SceneRoot.Children.Clear();
        }

        // ---------- Transforms (apply to last shape, add as new mesh) ----------
        private static EngineTriangulation FloatsToTriangulation(List<float> vec)
        {
            var tri = new EngineTriangulation();
            for (int i = 0; i + 8 < vec.Count; i += 9)
            {
                var p1 = new EnginePoint(vec[i], vec[i + 1], vec[i + 2]);
                var p2 = new EnginePoint(vec[i + 3], vec[i + 4], vec[i + 5]);
                var p3 = new EnginePoint(vec[i + 6], vec[i + 7], vec[i + 8]);
                int a = tri.AddPoint(p1), b = tri.AddPoint(p2), c = tri.AddPoint(p3);
                tri.AddTriangle(a, b, c);
            }
            return tri;
        }

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            var s = _shapeManager.GetLastShape(); if (s == null) { Warn(this, "No shape."); return; }
            double tx, ty, tz;
            if (!InputDialogs.AskThree("Translate", "X", 0, "Y", 0, "Z", 0, out tx, out ty, out tz)) return;

            var vec = s.GetTriangulation().GetDataForOpenGl();
            var tvec = Transformations.Translate(vec, tx, ty, tz);
            AddTriToScene(FloatsToTriangulation(tvec), Colors.LightSteelBlue);
        }

        private void Scale_Click(object sender, RoutedEventArgs e)
        {
            var s = _shapeManager.GetLastShape(); if (s == null) { Warn(this, "No shape."); return; }
            double sx, sy, sz;
            if (!InputDialogs.AskThree("Scale", "X", 1, "Y", 1, "Z", 1, out sx, out sy, out sz)) return;

            var vec = s.GetTriangulation().GetDataForOpenGl();
            var tvec = Transformations.Scale(vec, sx, sy, sz);
            AddTriToScene(FloatsToTriangulation(tvec), Colors.LightSteelBlue);
        }

        private void RotateX_Click(object sender, RoutedEventArgs e)
        {
            var s = _shapeManager.GetLastShape(); if (s == null) { Warn(this, "No shape."); return; }
            double deg; if (!InputDialogs.AskOne("Rotate X", "Degrees", 0, out deg)) return;

            var vec = s.GetTriangulation().GetDataForOpenGl();
            var tvec = Transformations.RotationX(vec, deg);
            AddTriToScene(FloatsToTriangulation(tvec), Colors.LightSteelBlue);
        }

        private void RotateY_Click(object sender, RoutedEventArgs e)
        {
            var s = _shapeManager.GetLastShape(); if (s == null) { Warn(this, "No shape."); return; }
            double deg; if (!InputDialogs.AskOne("Rotate Y", "Degrees", 0, out deg)) return;

            var vec = s.GetTriangulation().GetDataForOpenGl();
            var tvec = Transformations.RotationY(vec, deg);
            AddTriToScene(FloatsToTriangulation(tvec), Colors.LightSteelBlue);
        }

        private void RotateZ_Click(object sender, RoutedEventArgs e)
        {
            var s = _shapeManager.GetLastShape(); if (s == null) { Warn(this, "No shape."); return; }
            double deg; if (!InputDialogs.AskOne("Rotate Z", "Degrees", 0, out deg)) return;

            var vec = s.GetTriangulation().GetDataForOpenGl();
            var tvec = Transformations.RotationZ(vec, deg);
            AddTriToScene(FloatsToTriangulation(tvec), Colors.LightSteelBlue);
        }
    }
}
