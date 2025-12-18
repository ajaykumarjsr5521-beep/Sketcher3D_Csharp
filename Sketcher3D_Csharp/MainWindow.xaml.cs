using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using GeometryEngine3D_Csharp;

// Explicit aliases
using EnginePoint = GeometryEngine3D_Csharp.Point;
using WpfPoint = System.Windows.Point;

namespace Sketcher3D_Csharp
{
    public partial class MainWindow : Window
    {
        private readonly ShapeManager _shapeManager = new ShapeManager();

        // Scene transforms
        private Transform3DGroup _sceneTransform;
        private AxisAngleRotation3D _rotX;
        private AxisAngleRotation3D _rotY;
        private TranslateTransform3D _pan;

        // Mouse state
        private WpfPoint _lastPos;
        private bool _rotating;
        private bool _panning;

        public MainWindow()
        {
            InitializeComponent();
            InitSceneTransforms();
            ResetCamera();
        }

        // =====================================================
        // SCENE & CAMERA
        // =====================================================
        private void InitSceneTransforms()
        {
            _rotX = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
            _rotY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            _pan = new TranslateTransform3D();

            _sceneTransform = new Transform3DGroup();
            _sceneTransform.Children.Add(new RotateTransform3D(_rotX));
            _sceneTransform.Children.Add(new RotateTransform3D(_rotY));
            _sceneTransform.Children.Add(_pan);

            SceneRoot.Transform = _sceneTransform;
        }

        private void ResetCamera()
        {
            Camera.Position = new Point3D(0, 0, 150);
            Camera.LookDirection = new Vector3D(0, 0, -1);
            Camera.UpDirection = new Vector3D(0, 1, 0);
            Camera.FieldOfView = 45;
        }

        // =====================================================
        // ADD SHAPE / MESH
        // =====================================================
        private void AddShape(Shape shape, Color color)
        {
            _shapeManager.Add(shape);
            MeshGeometry3D mesh =
                TriangulationMeshBuilder.ToMesh(shape.GetTriangulation());
            AddMesh(mesh, color);
        }

        private void AddMesh(MeshGeometry3D mesh, Color color)
        {
            NormalizeMesh(mesh);
            FitCamera(mesh);

            var mat = new DiffuseMaterial(new SolidColorBrush(color));
            var model = new GeometryModel3D(mesh, mat);
            model.BackMaterial = mat;

            SceneRoot.Children.Add(new ModelVisual3D { Content = model });
        }

        private void NormalizeMesh(MeshGeometry3D mesh)
        {
            Rect3D b = mesh.Bounds;

            Vector3D center = new Vector3D(
                b.X + b.SizeX / 2,
                b.Y + b.SizeY / 2,
                b.Z + b.SizeZ / 2);

            for (int i = 0; i < mesh.Positions.Count; i++)
            {
                Point3D p = mesh.Positions[i];
                mesh.Positions[i] = new Point3D(
                    p.X - center.X,
                    p.Y - center.Y,
                    p.Z - center.Z);
            }

            double max = Math.Max(b.SizeX, Math.Max(b.SizeY, b.SizeZ));
            if (max <= 0) return;

            double scale = 50 / max;
            for (int i = 0; i < mesh.Positions.Count; i++)
            {
                Point3D p = mesh.Positions[i];
                mesh.Positions[i] = new Point3D(
                    p.X * scale,
                    p.Y * scale,
                    p.Z * scale);
            }
        }

        private void FitCamera(MeshGeometry3D mesh)
        {
            Rect3D b = mesh.Bounds;
            double size = Math.Max(b.SizeX, Math.Max(b.SizeY, b.SizeZ));
            if (size <= 0) size = 50;
            Camera.Position = new Point3D(0, 0, size * 2.5);
        }

        // =====================================================
        // FILE MENU
        // =====================================================
        private void New_Click(object sender, RoutedEventArgs e)
        {
            SceneRoot.Children.Clear();
            _shapeManager.Clear();
            InitSceneTransforms();
            ResetCamera();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SceneRoot.Children.Clear();
            _shapeManager.Clear();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // =====================================================
        // OPEN / SAVE
        // =====================================================
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "3D Files (*.obj;*.stl)|*.obj;*.stl";

            if (dlg.ShowDialog() != true) return;

            SceneRoot.Children.Clear();
            _shapeManager.Clear();

            string ext = Path.GetExtension(dlg.FileName).ToLower();
            if (ext == ".obj") LoadObj(dlg.FileName);
            else if (ext == ".stl") LoadBinaryStl(dlg.FileName);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "OBJ (*.obj)|*.obj|Binary STL (*.stl)|*.stl";

            if (dlg.ShowDialog() != true) return;

            string ext = Path.GetExtension(dlg.FileName).ToLower();
            if (ext == ".obj") SaveObj(dlg.FileName);
            else if (ext == ".stl") SaveBinaryStl(dlg.FileName);
        }

        // =====================================================
        // LOADERS
        // =====================================================
        private void LoadObj(string file)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            List<Point3D> verts = new List<Point3D>();

            foreach (string line in File.ReadAllLines(file))
            {
                if (line.StartsWith("v "))
                {
                    string[] p = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    verts.Add(new Point3D(
                        double.Parse(p[1]),
                        double.Parse(p[2]),
                        double.Parse(p[3])));
                }
                else if (line.StartsWith("f "))
                {
                    string[] p = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i <= 3; i++)
                    {
                        int idx = int.Parse(p[i].Split('/')[0]) - 1;
                        mesh.Positions.Add(verts[idx]);
                        mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
                    }
                }
            }

            AddMesh(mesh, Colors.LightGray);
        }

        private void LoadBinaryStl(string file)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            using (BinaryReader br = new BinaryReader(File.OpenRead(file)))
            {
                br.ReadBytes(80);
                uint count = br.ReadUInt32();

                for (uint i = 0; i < count; i++)
                {
                    br.ReadSingle(); br.ReadSingle(); br.ReadSingle();

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

            AddMesh(mesh, Colors.LightGray);
        }

        // =====================================================
        // SAVE
        // =====================================================
        private void SaveObj(string file)
        {
            using (StreamWriter w = new StreamWriter(file))
            {
                foreach (Shape s in _shapeManager.Shapes)
                {
                    Triangulation t = s.GetTriangulation();

                    foreach (EnginePoint p in t.Points)
                        w.WriteLine("v {0} {1} {2}", p.X, p.Y, p.Z);

                    foreach (Triangle tri in t.Triangles)
                        w.WriteLine("f {0} {1} {2}",
                            tri.M1 + 1, tri.M2 + 1, tri.M3 + 1);
                }
            }
        }

        private void SaveBinaryStl(string file)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Create(file)))
            {
                bw.Write(new byte[80]);

                int total = 0;
                foreach (Shape s in _shapeManager.Shapes)
                    total += s.GetTriangulation().Triangles.Count;

                bw.Write(total);

                foreach (Shape s in _shapeManager.Shapes)
                {
                    Triangulation t = s.GetTriangulation();
                    foreach (Triangle tri in t.Triangles)
                    {
                        bw.Write(0f); bw.Write(0f); bw.Write(0f);
                        WriteVertex(bw, t.Points[tri.M1]);
                        WriteVertex(bw, t.Points[tri.M2]);
                        WriteVertex(bw, t.Points[tri.M3]);
                        bw.Write((ushort)0);
                    }
                }
            }
        }

        private void WriteVertex(BinaryWriter bw, EnginePoint p)
        {
            bw.Write((float)p.X);
            bw.Write((float)p.Y);
            bw.Write((float)p.Z);
        }

        // =====================================================
        // SHAPE BUTTONS (FIXES YOUR BUILD ERRORS)
        // =====================================================
        private void Cube_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new DimensionDialog("Side") { Owner = this };
            if (dlg.ShowDialog() == true)
                AddShape(ShapeCreator.CreateCube("Cube", dlg.Side), Colors.Orange);
        }

        private void Cuboid_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new DimensionDialog("Length", "Width", "Height") { Owner = this };
            if (dlg.ShowDialog() == true)
                AddShape(
                    ShapeCreator.CreateCuboid("Cuboid",
                        dlg.ShapeLength, dlg.ShapeWidth, dlg.ShapeHeight),
                    Colors.CornflowerBlue);
        }

        private void Cylinder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new DimensionDialog("Radius", "Height") { Owner = this };
            if (dlg.ShowDialog() == true)
                AddShape(
                    ShapeCreator.CreateCylinder("Cylinder",
                        dlg.Radius, dlg.ShapeHeight),
                    Colors.MediumSeaGreen);
        }

        private void Cone_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new DimensionDialog("Radius", "Height") { Owner = this };
            if (dlg.ShowDialog() == true)
                AddShape(
                    ShapeCreator.CreateCone("Cone",
                        dlg.Radius, dlg.ShapeHeight),
                    Colors.Red);
        }

        private void Sphere_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new DimensionDialog("Radius") { Owner = this };
            if (dlg.ShowDialog() == true)
                AddShape(
                    ShapeCreator.CreateSphere("Sphere", dlg.Radius),
                    Colors.Goldenrod);
        }

        private void Pyramid_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new DimensionDialog("Length", "Width", "Height") { Owner = this };
            if (dlg.ShowDialog() == true)
                AddShape(
                    ShapeCreator.CreatePyramid("Pyramid",
                        dlg.ShapeLength, dlg.ShapeWidth, dlg.ShapeHeight),
                    Colors.SlateBlue);
        }

        // =====================================================
        // MOUSE CONTROLS
        // =====================================================
        private void View_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _lastPos = e.GetPosition(View);
            _rotating = e.LeftButton == MouseButtonState.Pressed;
            _panning = e.RightButton == MouseButtonState.Pressed;
            View.CaptureMouse();
        }

        private void View_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_rotating && !_panning) return;

            WpfPoint pos = e.GetPosition(View);
            Vector delta = pos - _lastPos;

            if (_rotating)
            {
                _rotY.Angle += delta.X * 0.5;
                _rotX.Angle += delta.Y * 0.5;
            }

            if (_panning)
            {
                _pan.OffsetX += delta.X * 0.2;
                _pan.OffsetY -= delta.Y * 0.2;
            }

            _lastPos = pos;
        }

        private void View_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _rotating = false;
            _panning = false;
            View.ReleaseMouseCapture();
        }

        private void View_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Camera.Position = new Point3D(
                Camera.Position.X,
                Camera.Position.Y,
                Camera.Position.Z + (e.Delta > 0 ? -10 : 10));
        }
    }
}
