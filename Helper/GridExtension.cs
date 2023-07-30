using System;
using System.Windows.Controls;
using System.Windows;

namespace WPF_GridExtension.Helper;
public static class GridExtension {
    public static readonly DependencyProperty RowDefinitionsProperty =
                DependencyProperty.RegisterAttached("RowDefinitions", typeof(string), typeof(GridExtension), new PropertyMetadata(OnRowAndColumnDefinitionsChanged));

    public static void SetRowDefinitions(Grid grid, string value) => grid.SetValue(RowDefinitionsProperty, value);

    public static string GetRowDefinitions(Grid grid) => (string)grid.GetValue(RowDefinitionsProperty);

    public static readonly DependencyProperty ColumnDefinitionsProperty =
        DependencyProperty.RegisterAttached("ColumnDefinitions", typeof(string), typeof(GridExtension), new PropertyMetadata(OnRowAndColumnDefinitionsChanged));

    public static void SetColumnDefinitions(Grid grid, string value) => grid.SetValue(ColumnDefinitionsProperty, value);

    public static string GetColumnDefinitions(Grid grid) => (string)grid.GetValue(ColumnDefinitionsProperty);

    private static void OnRowAndColumnDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is not Grid grid || e.NewValue is not string newValue)
            return;

        if (e.Property == RowDefinitionsProperty)
            grid.RowDefinitions.Clear();
        else if (e.Property == ColumnDefinitionsProperty)
            grid.ColumnDefinitions.Clear();

        string[] sizes = newValue.Split(',');
        foreach (var size in sizes) {
            if (e.Property == RowDefinitionsProperty)
                grid.RowDefinitions.Add(new RowDefinition { Height = ParseGridLength(size) });
            else if (e.Property == ColumnDefinitionsProperty)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = ParseGridLength(size) });

        }
    }

    private static GridLength ParseGridLength(string input) {
        return input.Trim().ToLower() switch {
            "auto" => GridLength.Auto,
            "*" => new GridLength(1, GridUnitType.Star),
            var s when IsMultipleStar(s, out double value) => new GridLength(value, GridUnitType.Star),
            _ => GridLength.Auto,
        };
    }

    private static bool IsMultipleStar(string s, out double value) {
        value = 0;
        return s.EndsWith("*") && double.TryParse(s.AsSpan(0, s.Length - 1), out value) && value > 0;
    }

    public static readonly DependencyProperty AutoGridProperty =
        DependencyProperty.RegisterAttached("AutoGrid", typeof(bool), typeof(GridExtension), new PropertyMetadata(false, OnAutoGridChanged));

    public static void SetAutoGrid(UIElement element, bool value) => element.SetValue(AutoGridProperty, value);

    public static bool GetAutoGrid(UIElement element) => (bool)element.GetValue(AutoGridProperty);

    private static void OnAutoGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is not Grid grid || !(bool)e.NewValue)
            return;

        grid.Loaded += Grid_Loaded;
    }

    private static void Grid_Loaded(object? sender, EventArgs e) {
        if (sender is Grid grid) {
            var name = grid.Name;
            grid.Loaded -= Grid_Loaded;

            int currentRow = 0;
            int currentColumn = 0;

            int rowCount = grid.RowDefinitions.Count;
            int columnCount = grid.ColumnDefinitions.Count;

            if (rowCount == 0 && columnCount > 0) {
                rowCount = (int)Math.Ceiling((double)grid.Children.Count / columnCount);
                for (int i = 0; i < rowCount; i++) {
                    grid.RowDefinitions.Add(new RowDefinition());
                }
            } else if (rowCount > 0 && columnCount == 0) {
                columnCount = (int)Math.Ceiling((double)grid.Children.Count / rowCount);
                for (int i = 0; i < columnCount; i++) {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            } else if (rowCount == 0 && columnCount == 0) {
                throw new InvalidOperationException("You should define columns or rows to use AutoGrid");
            }

            foreach (UIElement child in grid.Children) {

                if (currentRow >= rowCount)
                    throw new InvalidOperationException("Too many children for the grid");

                if (Grid.GetRow(child) != 0 || Grid.GetColumn(child) != 0)
                    continue;

                Grid.SetRow(child, currentRow);
                Grid.SetColumn(child, currentColumn);

                currentColumn++;
                if (currentColumn >= columnCount) {
                    currentColumn = 0;
                    currentRow++;
                }
            }
        }
    }
}
