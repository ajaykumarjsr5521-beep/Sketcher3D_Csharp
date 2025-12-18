using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Sketcher3D_Csharp
{
    public partial class DimensionDialog : Window
    {
        // Shape-specific properties
        public double Side { get; private set; }
        public double Radius { get; private set; }
        public double ShapeHeight { get; private set; }
        public double ShapeLength { get; private set; }
        public double ShapeWidth { get; private set; }

        private enum Field
        {
            None, Side, Radius, Height, Length, Width
        }

        private Field _f1, _f2, _f3;

        public DimensionDialog(string f1, string f2 = null, string f3 = null)
        {
            InitializeComponent();
            Setup(Row1, Label1, f1, out _f1);
            Setup(Row2, Label2, f2, out _f2);
            Setup(Row3, Label3, f3, out _f3);
        }

        private void Setup(FrameworkElement row, TextBlock label, string name, out Field field)
        {
            if (name == null)
            {
                row.Visibility = Visibility.Collapsed;
                field = Field.None;
                return;
            }

            label.Text = name;

            if (name == "Side") field = Field.Side;
            else if (name == "Radius") field = Field.Radius;
            else if (name == "Height") field = Field.Height;
            else if (name == "Length") field = Field.Length;
            else if (name == "Width") field = Field.Width;
            else field = Field.None;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!Read(Box1, _f1)) return;
            if (!Read(Box2, _f2)) return;
            if (!Read(Box3, _f3)) return;
            DialogResult = true;
        }

        private bool Read(TextBox box, Field field)
        {
            if (field == Field.None) return true;

            double v;
            if (!double.TryParse(box.Text, NumberStyles.Any,
                CultureInfo.InvariantCulture, out v))
            {
                MessageBox.Show("Invalid number");
                return false;
            }

            if (field == Field.Side) Side = v;
            else if (field == Field.Radius) Radius = v;
            else if (field == Field.Height) ShapeHeight = v;
            else if (field == Field.Length) ShapeLength = v;
            else if (field == Field.Width) ShapeWidth = v;

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
