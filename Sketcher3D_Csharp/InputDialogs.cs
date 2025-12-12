using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Tiny input dialog helper for 1–3 numeric values.
    /// Usage:
    ///   double a; if (InputDialogs.AskOne("Title","Label", 10, out a)) { ... }
    ///   double a,b; if (InputDialogs.AskTwo("Title","A",1,"B",2,out a,out b)) { ... }
    ///   double a,b,c; if (InputDialogs.AskThree("Title","A",1,"B",2,"C",3,out a,out b,out c)) { ... }
    /// </summary>
    public static class InputDialogs
    {
        // ---- Public API ----------------------------------------------------

        public static bool AskOne(string title, string label1, double def1,
                                  out double v1)
        {
            var dlg = BuildWindow(title);
            var tb1 = AddRow(dlg, label1, def1);

            if (ShowAndValidate(dlg, tb1, out v1))
                return true;

            v1 = 0;
            return false;
        }

        public static bool AskTwo(string title,
                                  string label1, double def1,
                                  string label2, double def2,
                                  out double v1, out double v2)
        {
            var dlg = BuildWindow(title);
            var tb1 = AddRow(dlg, label1, def1);
            var tb2 = AddRow(dlg, label2, def2);

            if (ShowAndValidate(dlg, tb1, out v1) &&
                ShowAndValidateValue(tb2, out v2))
                return true;

            v1 = v2 = 0;
            return false;
        }

        public static bool AskThree(string title,
                                    string label1, double def1,
                                    string label2, double def2,
                                    string label3, double def3,
                                    out double v1, out double v2, out double v3)
        {
            var dlg = BuildWindow(title);
            var tb1 = AddRow(dlg, label1, def1);
            var tb2 = AddRow(dlg, label2, def2);
            var tb3 = AddRow(dlg, label3, def3);

            if (ShowAndValidate(dlg, tb1, out v1) &&
                ShowAndValidateValue(tb2, out v2) &&
                ShowAndValidateValue(tb3, out v3))
                return true;

            v1 = v2 = v3 = 0;
            return false;
        }

        // ---- Internals -----------------------------------------------------

        private class DialogState
        {
            public Window Window;
            public Grid Grid;
            public Button Ok;
            public Button Cancel;
            public int NextRow = 0;
        }

        private static DialogState BuildWindow(string title)
        {
            var wnd = new Window
            {
                Title = title,
                Width = 360,
                Height = 220,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.Height,
                Owner = Application.Current?.MainWindow
            };

            var grid = new Grid { Margin = new Thickness(12) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 8, 0, 0) };
            var ok = new Button { Content = "OK", MinWidth = 70, Margin = new Thickness(0, 0, 8, 0), IsDefault = true };
            var cancel = new Button { Content = "Cancel", MinWidth = 70, IsCancel = true };
            btnPanel.Children.Add(ok);
            btnPanel.Children.Add(cancel);

            var outer = new DockPanel();
            DockPanel.SetDock(btnPanel, Dock.Bottom);
            outer.Children.Add(btnPanel);
            outer.Children.Add(grid);

            wnd.Content = outer;

            ok.Click += (s, e) => wnd.DialogResult = true;
            cancel.Click += (s, e) => wnd.DialogResult = false;

            return new DialogState { Window = wnd, Grid = grid, Ok = ok, Cancel = cancel };
        }

        private static TextBox AddRow(DialogState dlg, string label, double defVal)
        {
            dlg.Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var lb = new Label { Content = label + ":", Margin = new Thickness(0, 0, 8, 6), VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(lb, dlg.NextRow);
            Grid.SetColumn(lb, 0);
            dlg.Grid.Children.Add(lb);

            var tb = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 6),
                Text = defVal.ToString(CultureInfo.InvariantCulture)
            };
            Grid.SetRow(tb, dlg.NextRow);
            Grid.SetColumn(tb, 1);
            dlg.Grid.Children.Add(tb);

            dlg.NextRow++;
            return tb;
        }

        private static bool ShowAndValidate(DialogState dlg, TextBox firstBox, out double val1)
        {
            // focus first field
            dlg.Window.Loaded += (s, e) =>
            {
                firstBox.Focus();
                firstBox.SelectAll();
            };

            var res = dlg.Window.ShowDialog();
            if (res != true)
            {
                val1 = 0;
                return false;
            }
            return ShowAndValidateValue(firstBox, out val1);
        }

        private static bool ShowAndValidateValue(TextBox tb, out double value)
        {
            // Try invariant (dot) first, then current culture.
            if (!double.TryParse(tb.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                if (!double.TryParse(tb.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
                    return false;
            }
            return true;
        }
    }
}
