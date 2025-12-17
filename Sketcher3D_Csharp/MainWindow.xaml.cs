using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using GeometryEngine3D_Csharp;

// Alias to avoid Point ambiguity
using WpfPoint = System.Windows.Point;

namespace Sketcher3D_Csharp
{
    public partial class MainWindow : Window
    {
        private readonly ShapeManager _shapeManager = new ShapeManager();

        // -------- Scene Transforms --------
        private Transform3DGroup _sceneTransform;
        private AxisAngleRotation3D _rotX;
        private AxisAngleRotation3D _rotY;
        private TranslateTransform3D _pan;

        // -------- Mouse State --------
        private WpfPoint _lastPos;
        private bool _rotating;
        private bool _panning;

        public MainWindow()
        {
            InitializeComponent();
            InitSceneTransforms();
        }

        // ================= SCENE SETUP =================
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

        // ================= ADD SHAPE =================
        private void AddShape(Shape shape, Color color)
        {
            _shapeManager.Add(shape);

            MeshGeometry3D mesh =
                TriangulationMeshBuilder.ToMesh(shape.GetTriangulation());

            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material)
            {
                BackMaterial = material
            };

            SceneRoot.Children.Add(
                new ModelVisual3D { Content = model });
        }

        // ================= MOUSE CONTROLS =================
        private void View_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _lastPos = e.GetPosition(View);

            if (e.LeftButton == MouseButtonState.Pressed)
                _rotating = true;

            if (e.RightButton == MouseButtonState.Pressed)
                _panning = true;

            View.CaptureMouse();
        }

        private void View_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_rotating && !_panning)
                return;

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
            double zoom = e.Delta > 0 ? 0.9 : 1.1;

            Camera.Position = new Point3D(
                Camera.Position.X * zoom,
                Camera.Position.Y * zoom,
                Camera.Position.Z * zoom);
        }

        // ================= FILE MENU =================
        private void New_Click(object sender, RoutedEventArgs e)
        {
            SceneRoot.Children.Clear();
            InitSceneTransforms();
            _shapeManager.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SceneRoot.Children.Clear();
            _shapeManager.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt",
                FileName = "scene.txt"
            };

            if (dlg.ShowDialog() == true)
            {
                //Correct: save geometry, not visuals
                FileHandle.Save(dlg.FileName, _shapeManager.Shapes);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ================= SHAPE BUTTONS =================
        private void Cube_Click(object sender, RoutedEventArgs e)
            => AddShape(ShapeCreator.CreateCube("Cube", 50), Colors.Orange);

        private void Cuboid_Click(object sender, RoutedEventArgs e)
            => AddShape(
                ShapeCreator.CreateCuboid("Cuboid", 80, 40, 30),
                Colors.CornflowerBlue);

        private void Cylinder_Click(object sender, RoutedEventArgs e)
            => AddShape(
                ShapeCreator.CreateCylinder("Cylinder", 25, 80),
                Colors.MediumSeaGreen);

        private void Cone_Click(object sender, RoutedEventArgs e)
            => AddShape(
                ShapeCreator.CreateCone("Cone", 30, 80),
                Colors.Red);

        private void Sphere_Click(object sender, RoutedEventArgs e)
            => AddShape(
                ShapeCreator.CreateSphere("Sphere", 40),
                Colors.Goldenrod);

        private void Pyramid_Click(object sender, RoutedEventArgs e)
            => AddShape(
                ShapeCreator.CreatePyramid("Pyramid", 60, 60, 80),
                Colors.SlateBlue);
    }
}
