using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Newtonsoft.Json;

namespace Periodis
{
    public class TableData
    {
        public required ColumnData Columns { get; set; }
        public required List<RowData> Row { get; set; }
    }

    public class ColumnData
    {
        public required List<string> Column { get; set; }
    }

    public class RowData
    {
        public required List<string> Cell { get; set; }
    }

    public class Root
    {
        public required TableData Table { get; set; }
    }

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            string filePath = "elements.json"; // relative to executable
            string json = File.ReadAllText(filePath);
            Root root = JsonConvert.DeserializeObject<Root>(json);

            // Access table
            var columns = root.Table.Columns.Column;         // List<string>
            var rows = root.Table.Row;                       // List<RowData>

            // Convert each row to Dictionary<string, string>
            List<Dictionary<string, string>> table = new();

            foreach (var row in rows)
            {
                var dict = new Dictionary<string, string>();
                for (int i = 0; i < columns.Count; i++)
                {
                    dict[columns[i]] = row.Cell[i];
                }
                table.Add(dict);
            }
            CreateGrid();
            AddElements();
        }

        private void CreateGrid()
        {
            for (int i = 0; i < 10; i++)  // 10 rows
                PeriodicTable.RowDefinitions.Add(new RowDefinition());
            for (int j = 0; j < 18; j++)  // 18 columns
                PeriodicTable.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private void AddElements()
        {
            AddElement("H", 0, 0);     // Hydrogen at column 1, row 0
            AddElement("He", 17, 0);   // Helium
            AddElement("Li", 0, 1);
            AddElement("Be", 1, 1);
            AddElement("B", 12, 1);
            AddElement("C", 13, 1);
            AddElement("N", 14, 1);
            AddElement("O", 15, 1);
            AddElement("F", 16, 1);
            AddElement("Ne", 17, 1);

            // ...continue for rest of the table
        }

        private void AddElement(string symbol, int col, int row)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Colors.SkyBlue),
                Margin = new Thickness(2),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Child = new TextBlock
                {
                    Text = symbol,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 18
                }
            };

            Grid.SetColumn(border, col);
            Grid.SetRow(border, row);
            PeriodicTable.Children.Add(border);
        }

    }
}
