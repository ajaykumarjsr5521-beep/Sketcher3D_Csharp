using System.Globalization;              // Used for culture-aware number formatting/parsing
using System.Windows;                    // Core WPF types (Window, Application)
using System.Windows.Controls;           // WPF UI controls (Grid, TextBox, Button, etc.)

namespace Sketcher3D_Csharp
{
    /// <summary>
    /// Utility class responsible for collecting numeric input from the user.
    /// 
    /// ✔ Used when creating shapes (dimensions like radius, height, length)
    /// ✔ Creates dialogs dynamically (no XAML required)
    /// ✔ Keeps UI logic isolated from geometry engine
    /// </summary>
    public static class InputDialogs
    {
        // =====================================================
        // PUBLIC API
        // These methods are called from MainWindow or UI logic
        // =====================================================

        /// <summary>
        /// Displays a dialog with ONE numeric input field.
        /// Example use: asking for cube side length or sphere radius.
        /// </summary>
        public static bool AskOne(string title,
                                  string label,
                                  double defaultValue,
                                  out double v1)
        {
            // Create the base dialog window
            var dlg = BuildWindow(title);

            // Add a single input row (Label + TextBox)
            var tb1 = AddRow(dlg, label, defaultValue);

            // Show dialog and validate the entered value
            if (ShowAndValidate(dlg, tb1, out v1))
                return true;    // User pressed OK and input is valid

            // If dialog cancelled or input invalid
            v1 = 0;
            return false;
        }

        /// <summary>
        /// Displays a dialog with TWO numeric input fields.
        /// Example use: cylinder radius & height, cuboid length & width.
        /// </summary>
        public static bool AskTwo(string title,
                                  string label1, double def1,
                                  string label2, double def2,
                                  out double v1, out double v2)
        {
            // Create dialog window
            var dlg = BuildWindow(title);

            // Add two rows of inputs
            var tb1 = AddRow(dlg, label1, def1);
            var tb2 = AddRow(dlg, label2, def2);

            // Validate both values
            if (ShowAndValidate(dlg, tb1, out v1) &&
                TryParse(tb2.Text, out v2))
                return true;

            // Reset values on failure
            v1 = v2 = 0;
            return false;
        }

        /// <summary>
        /// Displays a dialog with THREE numeric input fields.
        /// Example use: cuboid length, width & height.
        /// </summary>
        public static bool AskThree(string title,
                                    string label1, double def1,
                                    string label2, double def2,
                                    string label3, double def3,
                                    out double v1,
                                    out double v2,
                                    out double v3)
        {
            // Create dialog window
            var dlg = BuildWindow(title);

            // Add three input rows
            var tb1 = AddRow(dlg, label1, def1);
            var tb2 = AddRow(dlg, label2, def2);
            var tb3 = AddRow(dlg, label3, def3);

            // Validate all three values
            if (ShowAndValidate(dlg, tb1, out v1) &&
                TryParse(tb2.Text, out v2) &&
                TryParse(tb3.Text, out v3))
                return true;

            // Reset on cancel or invalid input
            v1 = v2 = v3 = 0;
            return false;
        }

        // =====================================================
        // INTERNAL IMPLEMENTATION DETAILS
        // These helpers are NOT visible outside this class
        // =====================================================

        /// <summary>
        /// Holds the current dialog construction state.
        /// Keeps track of:
        /// - The window instance
        /// - The grid layout
        /// - The current row index while adding controls
        /// </summary>
        private class DialogState
        {
            public Window Window;   // The dialog window shown to the user
            public Grid Grid;       // Grid used to layout labels and inputs
            public int Row;         // Current row index in the grid
        }

        /// <summary>
        /// Creates the dialog window and basic layout.
        /// This method does NOT add input fields.
        /// </summary>
        private static DialogState BuildWindow(string title)
        {
            // Create a modal dialog window
            var window = new Window
            {
                Title = title,
                Width = 320,
                ResizeMode = ResizeMode.NoResize,        // Fixed size dialog
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.Height,   // Height adapts to content
                Owner = Application.Current?.MainWindow // Block main window
            };

            // Grid for labels (column 0) and textboxes (column 1)
            var grid = new Grid { Margin = new Thickness(12) };

            grid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Auto });   // Label column

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)     // Input column
                });

            // Panel holding OK and Cancel buttons
            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };

            // OK button (default action when Enter is pressed)
            var ok = new Button
            {
                Content = "OK",
                MinWidth = 70,
                IsDefault = true,
                Margin = new Thickness(0, 0, 8, 0)
            };

            // Cancel button (Esc key support)
            var cancel = new Button
            {
                Content = "Cancel",
                MinWidth = 70,
                IsCancel = true
            };

            // Set dialog result based on button clicked
            ok.Click += (_, __) => window.DialogResult = true;
            cancel.Click += (_, __) => window.DialogResult = false;

            // Add buttons to panel
            btnPanel.Children.Add(ok);
            btnPanel.Children.Add(cancel);

            // Root container to place buttons at bottom
            var root = new DockPanel();
            DockPanel.SetDock(btnPanel, Dock.Bottom);

            root.Children.Add(btnPanel);
            root.Children.Add(grid);

            window.Content = root;

            return new DialogState
            {
                Window = window,
                Grid = grid,
                Row = 0
            };
        }

        /// <summary>
        /// Adds one input row consisting of:
        /// - A label
        /// - A numeric TextBox
        /// </summary>
        private static TextBox AddRow(DialogState dlg,
                                      string label,
                                      double defaultValue)
        {
            // Add a new row to the grid
            dlg.Grid.RowDefinitions.Add(
                new RowDefinition { Height = GridLength.Auto });

            // Create label describing the value
            var lbl = new Label
            {
                Content = label + ":",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 6)
            };

            // Create textbox for numeric input
            var tb = new TextBox
            {
                Text = defaultValue.ToString(CultureInfo.InvariantCulture),
                Margin = new Thickness(0, 0, 0, 6)
            };

            // Place controls into grid
            Grid.SetRow(lbl, dlg.Row);
            Grid.SetColumn(lbl, 0);

            Grid.SetRow(tb, dlg.Row);
            Grid.SetColumn(tb, 1);

            dlg.Grid.Children.Add(lbl);
            dlg.Grid.Children.Add(tb);

            // Move to next row for subsequent inputs
            dlg.Row++;

            return tb;
        }

        /// <summary>
        /// Shows the dialog and validates the first input field.
        /// Automatically focuses and selects text for fast editing.
        /// </summary>
        private static bool ShowAndValidate(DialogState dlg,
                                            TextBox firstBox,
                                            out double value)
        {
            // When dialog opens, focus first input
            dlg.Window.Loaded += (_, __) =>
            {
                firstBox.Focus();
                firstBox.SelectAll();
            };

            // Display dialog modally
            if (dlg.Window.ShowDialog() != true)
            {
                value = 0;
                return false;   // User cancelled
            }

            // Try to parse entered value
            return TryParse(firstBox.Text, out value);
        }

        /// <summary>
        /// Attempts to parse a string into a double.
        /// Supports:
        /// - Invariant culture (dot decimal)
        /// - Current culture (comma decimal)
        /// </summary>
        private static bool TryParse(string text, out double value)
        {
            return double.TryParse(text,
                                   NumberStyles.Float,
                                   CultureInfo.InvariantCulture,
                                   out value)
                || double.TryParse(text,
                                   NumberStyles.Float,
                                   CultureInfo.CurrentCulture,
                                   out value);
        }
    }
}
