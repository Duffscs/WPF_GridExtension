# WPF GridExtension Helper

This is a helper class named [GridExtension](Helper/GridExtension.cs) that provides some attached properties for extending the functionality of the WPF Grid control. It allows you to set row and column definitions using simple comma-separated strings and automatically arrange the child elements within the grid.

## How to Use

To use the `GridExtension`, follow these steps:

1. Copy the `GridExtension.cs` file into your WPF project.
2. Make sure the namespace of the `GridExtension` class is correctly added to your XAML file like this.

```xaml
xmlns:helper="clr-namespace:WPF_GridExtension.Helper"
```

### Attached Properties

#### RowDefinitions

- Allows you to set row definitions for the `Grid` using a comma-separated string.
- Example usage:

```xaml
<Grid helper:GridExtension.RowDefinitions="Auto, *, 100">
    <!-- Add child elements here -->
</Grid>
```

#### ColumnDefinitions

- Allows you to set column definitions for the Grid using a comma-separated string.
- Example usage:
```xaml
<Grid helper:GridExtension.ColumnDefinitions="2*, 3*">
    <!-- Add child elements here -->
</Grid>
```

#### AutoGrid

- When set to `True`, this property will automatically place child elements within the grid based on their order of addition at the time the component is loaded.

- **Important**: Before using `AutoGrid`, you must explicitly define either the `RowDefinitions` or `ColumnDefinitions`. The `AutoGrid` property relies on at least one of these properties to be defined to determine the layout.
If both are missing, an `InvalidOperationException` will be thrown during runtime.

- If you manually set the `Grid.Row` or `Grid.Column` attached property for any child element, it will not be affected by the automatic placement mechanism of AutoGrid

- Example usage:

```xaml
<Grid helper:GridExtension.RowDefinitions="Auto, *, 100" helper:GridExtension.AutoGrid="True">
  <!-- Add child elements here, they will be placed automatically -->
</Grid>
```
or
```xaml
<Grid helper:GridExtension.ColumnDefinitions="2*, 3*" helper:GridExtension.AutoGrid="True">
    <!-- Add child elements here, they will be placed automatically -->
</Grid>
```
or both

```xaml
<Grid helper:GridExtension.RowDefinitions="Auto, *, 100" helper:GridExtension.ColumnDefinitions="2*, 3*" helper:GridExtension.AutoGrid="True">
    <!-- Add child elements here, they will be placed automatically -->
</Grid>
```

## Contribution

Contributions to this project are welcome. Feel free to create pull requests or raise issues for any improvements, bug fixes, or feature requests.

## License
Feel free to do whatever you want with the code! You are welcome to use, modify, distribute, and incorporate the code into your projects without any restrictions. Happy coding!

![License](license.jpg "I stole your code")

**Disclaimer: The code is provided "as is" without any warranty of any kind, express or implied. The author(s) of this project shall not be liable for any damages, including but not limited to direct, indirect, special, incidental, or consequential damages or losses arising out of the use or inability to use the code. Use at your own risk.**
