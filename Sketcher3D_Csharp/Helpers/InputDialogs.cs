using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Lightweight numeric input dialogs.
    /// Used for shape dimension input.
    /// </summary>
    public static class InputDialogs
    {
        public static bool AskOne(string title,
                                  string label,
                                  double def,
                                  out double v1)
        {
            var w = BuildWindow(title);
            var t1 = AddRow(w.Grid, label, def);

            if (Show(w.Window, t1, out v1))
                return true;

            v1 = 0;
            return false;
        }

        public static bool AskTwo(string title,
                                  string l1, double d1,
                                  string l2, double d2,
                                  out double v1,
                                  out double v2)
        {
            var w = BuildWindow(title);
            var t1 = AddRow(w.Grid, l1, d1);
            var t2 = AddRow(w.Grid, l2, d2);

            if (Show(w.Window, t1, out v1) &&
                TryParse(t2.Text, out v2))
                return true;

            v1 = v2 = 0;
            return false;
        }

        public static bool AskThree(string title,
                                    string l1, double d1,
                                    string l2, double d2,
                                    string l3, double d3,
                                    out double v1,
                                    out double v2,
                                    out double v3)
        {
            var w = BuildWindow(title);
            var t1 = AddRow(w.Grid, l1, d1);
            var t2 = AddRow(w.Grid, l2, d2);
            var t3 = AddRow(w.Grid, l3, d3);

            if (Show(w.Window, t1, out v1) &&
                TryParse(t2.Text, out v2) &&
                TryParse(t3.Text, out v3))
                return true;

            v1 = v2 = v3 = 0;
            return false;
        }

        // =====================================================

        private class Dialog
        {
            public Window Window;
            public Grid Grid;
        }

        private static Dialog BuildWindow(string title)
        {
            var win = new Window
            {
                Title = title,
                Width = 300,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.Height,
                Owner = Application.Current.MainWindow
            };

            var grid = new Grid { Margin = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            var ok = new Button { Content = "OK", Width = 70, IsDefault = true };
            var cancel = new Button { Content = "Cancel", Width = 70, IsCancel = true };

            ok.Click += (_, __) => win.DialogResult = true;
            cancel.Click += (_, __) => win.DialogResult = false;

            var btns = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btns.Children.Add(ok);
            btns.Children.Add(cancel);

            var root = new DockPanel();
            DockPanel.SetDock(btns, Dock.Bottom);
            root.Children.Add(btns);
            root.Children.Add(grid);

            win.Content = root;

            return new Dialog { Window = win, Grid = grid };
        }

        private static TextBox AddRow(Grid g, string label, double def)
        {
            g.RowDefinitions.Add(new RowDefinition());

            var l = new Label { Content = label + ":" };
            var t = new TextBox
            {
                Text = def.ToString(CultureInfo.InvariantCulture)
            };

            int r = g.RowDefinitions.Count - 1;
            Grid.SetRow(l, r);
            Grid.SetColumn(l, 0);
            Grid.SetRow(t, r);
            Grid.SetColumn(t, 1);

            g.Children.Add(l);
            g.Children.Add(t);

            return t;
        }

        private static bool Show(Window w, TextBox first, out double v)
        {
            w.Loaded += (_, __) =>
            {
                first.Focus();
                first.SelectAll();
            };

            if (w.ShowDialog() != true)
            {
                v = 0;
                return false;
            }

            return TryParse(first.Text, out v);
        }

        private static bool TryParse(string s, out double v)
        {
            return double.TryParse(
                s,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out v);
        }
    }
}
