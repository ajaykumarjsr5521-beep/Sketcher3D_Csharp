using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Simple numeric input dialogs for 1–3 values.
    /// Pure WPF, no external dependencies.
    /// </summary>
    public static class InputDialogs
    {
        // ================= PUBLIC API =================

        public static bool AskOne(string title, string label,
                                  double defaultValue,
                                  out double v1)
        {
            var dlg = BuildWindow(title);
            var tb1 = AddRow(dlg, label, defaultValue);

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
                TryParse(tb2.Text, out v2))
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
                TryParse(tb2.Text, out v2) &&
                TryParse(tb3.Text, out v3))
                return true;

            v1 = v2 = v3 = 0;
            return false;
        }

        // ================= INTERNALS =================

        private class DialogState
        {
            public Window Window;
            public Grid Grid;
            public int Row;
        }

        private static DialogState BuildWindow(string title)
        {
            var window = new Window
            {
                Title = title,
                Width = 320,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.Height,
                Owner = Application.Current?.MainWindow
            };

            var grid = new Grid { Margin = new Thickness(12) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var ok = new Button { Content = "OK", MinWidth = 70, IsDefault = true, Margin = new Thickness(0, 0, 8, 0) };
            var cancel = new Button { Content = "Cancel", MinWidth = 70, IsCancel = true };

            ok.Click += (_, __) => window.DialogResult = true;
            cancel.Click += (_, __) => window.DialogResult = false;

            btnPanel.Children.Add(ok);
            btnPanel.Children.Add(cancel);

            var root = new DockPanel();
            DockPanel.SetDock(btnPanel, Dock.Bottom);

            root.Children.Add(btnPanel);
            root.Children.Add(grid);

            window.Content = root;

            return new DialogState { Window = window, Grid = grid };
        }

        private static TextBox AddRow(DialogState dlg, string label, double defaultValue)
        {
            dlg.Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var lbl = new Label
            {
                Content = label + ":",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 6)
            };

            var tb = new TextBox
            {
                Text = defaultValue.ToString(CultureInfo.InvariantCulture),
                Margin = new Thickness(0, 0, 0, 6)
            };

            Grid.SetRow(lbl, dlg.Row);
            Grid.SetColumn(lbl, 0);

            Grid.SetRow(tb, dlg.Row);
            Grid.SetColumn(tb, 1);

            dlg.Grid.Children.Add(lbl);
            dlg.Grid.Children.Add(tb);

            dlg.Row++;
            return tb;
        }

        private static bool ShowAndValidate(DialogState dlg, TextBox firstBox, out double value)
        {
            dlg.Window.Loaded += (_, __) =>
            {
                firstBox.Focus();
                firstBox.SelectAll();
            };

            if (dlg.Window.ShowDialog() != true)
            {
                value = 0;
                return false;
            }

            return TryParse(firstBox.Text, out value);
        }

        private static bool TryParse(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Float,
                       CultureInfo.InvariantCulture, out value)
                || double.TryParse(text, NumberStyles.Float,
                       CultureInfo.CurrentCulture, out value);
        }
    }
}
