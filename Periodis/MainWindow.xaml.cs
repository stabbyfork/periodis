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
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Documents;

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
        readonly static Dictionary<int, (int Row, int Col)> periodicPositions = new()
        {
            {1, (0, 0)}, {2, (0, 17)},
            {3, (1, 0)}, {4, (1, 1)}, {5, (1, 12)}, {6, (1, 13)}, {7, (1, 14)}, {8, (1, 15)}, {9, (1, 16)}, {10, (1, 17)},
            {11, (2, 0)}, {12, (2, 1)}, {13, (2, 12)}, {14, (2, 13)}, {15, (2, 14)}, {16, (2, 15)}, {17, (2, 16)}, {18, (2, 17)},
            {19, (3, 0)}, {20, (3, 1)}, {21, (3, 2)}, {22, (3, 3)}, {23, (3, 4)}, {24, (3, 5)}, {25, (3, 6)},
            {26, (3, 7)}, {27, (3, 8)}, {28, (3, 9)}, {29, (3, 10)}, {30, (3, 11)}, {31, (3, 12)}, {32, (3, 13)},
            {33, (3, 14)}, {34, (3, 15)}, {35, (3, 16)}, {36, (3, 17)},
            {37, (4, 0)}, {38, (4, 1)}, {39, (4, 2)}, {40, (4, 3)}, {41, (4, 4)}, {42, (4, 5)}, {43, (4, 6)},
            {44, (4, 7)}, {45, (4, 8)}, {46, (4, 9)}, {47, (4, 10)}, {48, (4, 11)}, {49, (4, 12)}, {50, (4, 13)},
            {51, (4, 14)}, {52, (4, 15)}, {53, (4, 16)}, {54, (4, 17)},
            {55, (5, 0)}, {56, (5, 1)}, {57, (8, 2)}, {58, (8, 3)}, {59, (8, 4)}, {60, (8, 5)}, {61, (8, 6)}, {62, (8, 7)},
            {63, (8, 8)}, {64, (8, 9)}, {65, (8, 10)}, {66, (8, 11)}, {67, (8, 12)}, {68, (8, 13)}, {69, (8, 14)},
            {70, (8, 15)}, {71, (8, 16)}, {72, (5, 3)}, {73, (5, 4)}, {74, (5, 5)}, {75, (5, 6)}, {76, (5, 7)},
            {77, (5, 8)}, {78, (5, 9)}, {79, (5, 10)}, {80, (5, 11)}, {81, (5, 12)}, {82, (5, 13)}, {83, (5, 14)},
            {84, (5, 15)}, {85, (5, 16)}, {86, (5, 17)},
            {87, (6, 0)}, {88, (6, 1)}, {89, (9, 2)}, {90, (9, 3)}, {91, (9, 4)}, {92, (9, 5)}, {93, (9, 6)}, {94, (9, 7)},
            {95, (9, 8)}, {96, (9, 9)}, {97, (9, 10)}, {98, (9, 11)}, {99, (9, 12)}, {100, (9, 13)}, {101, (9, 14)},
            {102, (9, 15)}, {103, (9, 16)}, {104, (6, 3)}, {105, (6, 4)}, {106, (6, 5)}, {107, (6, 6)}, {108, (6, 7)},
            {109, (6, 8)}, {110, (6, 9)}, {111, (6, 10)}, {112, (6, 11)}, {113, (6, 12)}, {114, (6, 13)}, {115, (6, 14)},
            {116, (6, 15)}, {117, (6, 16)}, {118, (6, 17)}
        };

        int? selectedElement = null;
        int? hoveredElement = null;

        public MainWindow()
        {
            this.InitializeComponent();
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Assets\\elements.json");
            Root? root = JsonConvert.DeserializeObject<Root>(json) ?? throw new Exception("Error deserializing JSON");

            var columns = root.Table.Columns.Column;
            var rows = root.Table.Row;

            List<Dictionary<string, string>> table = [];

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
            foreach (var element in table)
            {
                int atomicNumber = int.Parse(element["AtomicNumber"]);

                if (periodicPositions.TryGetValue(atomicNumber, out var pos))
                {
                    AddElement(element, pos.Row, pos.Col);
                }
            }

        }

        private void CreateGrid()
        {
            for (int i = 0; i < 10; i++)
                PeriodicTable.RowDefinitions.Add(new RowDefinition());
            for (int j = 0; j < 18; j++)
                PeriodicTable.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private async Task<Dictionary<int, (string Title, string Description, int CID)>> GetTitlesAndDescriptionsAsync(List<int> cids)
        {
            var result = new Dictionary<int, (string Title, string Description, int CID)>();

            if (cids == null || cids.Count == 0)
                return result;

            string cidList = string.Join(",", cids);
            string url = $"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/cid/{cidList}/description/JSON";

            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;

                if (root.TryGetProperty("InformationList", out var infoList) &&
                    infoList.TryGetProperty("Information", out var infos))
                {
                    var tempData = new Dictionary<int, (string? Title, string? Description)>();

                    foreach (var item in infos.EnumerateArray())
                    {
                        int cid = item.GetProperty("CID").GetInt32();

                        tempData.TryGetValue(cid, out var existing);

                        string? title = existing.Title;
                        string? description = existing.Description;

                        if (item.TryGetProperty("Title", out var titleEl))
                            title = titleEl.GetString();

                        if (item.TryGetProperty("Description", out var descEl))
                            description = descEl.GetString();

                        tempData[cid] = (title, description);
                    }

                    foreach (var pair in tempData)
                    {
                        string finalTitle = pair.Value.Title ?? "(No Title Found)";
                        string finalDescription = pair.Value.Description ?? "(No Description Found)";

                        result[pair.Key] = (finalTitle, finalDescription, pair.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (var cid in cids)
                {
                    result[cid] = ("Error", $"Error fetching data: {ex.Message}", -1);
                }
            }

            return result;
        }

        private async void SearchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
            {
                return;
            }
            string input = SearchBox.Text;

            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    ResultText.Text = "Sending request...";

                    using var httpClient = new HttpClient();

                    string? selectedSearchType = SearchSelector.SelectedItem.ToString();
                    string inputType = selectedSearchType switch
                    {
                        "Formula" => "compound/fastformula",
                        _ => "compound/name"
                    };
                    inputType += $"/{input}/cids/TXT?MaxRecords=256";
                    string apiUrl = $"https://pubchem.ncbi.nlm.nih.gov/rest/pug/{inputType}";

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    ResultsPanel.Children.Clear();

                    var cidLines = result
                        .Split(["\r\n", "\n", "\r", ",", " "], StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.TryParse(s, out int cid) ? cid : -1)
                        .Where(cid => cid > 0)
                        .Distinct()
                        .Take(256)
                        .ToList();
                    System.Diagnostics.Debug.Print(result);
                    if (cidLines.Count == 0)
                    {
                        ResultsPanel.Children.Add(new TextBlock { Text = "No valid CIDs found." });
                        return;
                    }
                    var data = await GetTitlesAndDescriptionsAsync(cidLines);

                    foreach (var pair in data)
                    {
                        string title = pair.Value.Title;
                        string description = pair.Value.Description;

                        ResultsPanel.Children.Add(new TextBlock
                        {
                            
                            Text = $"{title}: {description} (https://pubchem.ncbi.nlm.nih.gov/compound/{pair.Value.CID})",
                            FontSize = 16,
                            Margin = new Thickness(0, 4, 0, 4),
                            TextWrapping = TextWrapping.WrapWholeWords,
                            MaxWidth = 200,
                        });
                    }
                    ResultText.Text = "Complete";
                }
                catch (Exception ex)
                {
                    ResultText.Text = $"Error: {ex.Message}";
                }
            }
        }

        private void AddElement(Dictionary<string, string> element, int row, int col)
        {
            string group = element["GroupBlock"];
            Windows.UI.Color tileColor = group switch
            {
                "Metalloid" => Colors.Turquoise,
                "Lanthanide" => Colors.SaddleBrown,
                "Actinide" => Colors.DeepPink,
                "Transition metal" => Colors.Red,
                "Post-transition metal" => Colors.Blue,
                "Alkaline earth metal" => Colors.SandyBrown,
                "Noble gas" => Colors.MediumPurple,
                "Alkali metal" => Colors.Brown,
                "Nonmetal" => Colors.LightGreen,
                "Halogen" => Colors.DeepSkyBlue,
                _ => Colors.SlateGray
            };

            var border = new Border
            {
                Background = new SolidColorBrush(tileColor),
                Margin = new Thickness(2),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Child = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = element["AtomicNumber"],
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            Padding = new Thickness(18, 3, 18, 3),
                            FontSize = 12,
                            Foreground = new SolidColorBrush(Colors.White)
                        },
                        new TextBlock
                        {
                            Text = element["Symbol"],
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Padding = new Thickness(18, 3, 18, 3),
                            FontSize = 20,
                            Foreground = new SolidColorBrush(Colors.White)
                        },
                        new TextBlock
                        {
                            Text = (Math.Round(double.Parse(element["AtomicMass"]), 2)).ToString(),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Padding = new Thickness(18, 3, 18, 3),
                            FontSize = 12,
                            Foreground = new SolidColorBrush(Colors.White)
                        }
                    }
                }
            };
            border.PointerEntered += (s, e) =>
            {
                var point = e.GetCurrentPoint(RootCanvas);
                Point position = point.Position;

                Canvas.SetLeft(InfoPanel, position.X + 10);
                Canvas.SetTop(InfoPanel, position.Y + 10);

                ElementName.Text = element["Name"];
                ElementDetails.Text = $"Group: {element["GroupBlock"]}\nAtomic mass: {element["AtomicMass"]}\nStandard state: {element["StandardState"]}\nMelting point: {element["MeltingPoint"]}\nBoiling point: {element["BoilingPoint"]}\nElectron configuration: {element["ElectronConfiguration"]}\nElectronegativity: {element["Electronegativity"]}\nAtomic radius: {element["AtomicRadius"]}\nIonization energy: {element["IonizationEnergy"]}\nElectron affinity: {element["ElectronAffinity"]}\nOxidation states: {element["OxidationStates"]}\nDensity: {element["Density"]}\nYear discovered: {element["YearDiscovered"]}";
                InfoPanel.Visibility = Visibility.Visible;
                hoveredElement = int.Parse(element["AtomicNumber"]);
            };

            border.PointerExited += (s, e) =>
            {
                InfoPanel.Visibility = Visibility.Collapsed;
                hoveredElement = null;
            };

            border.PointerMoved += (s, e) =>
            {
                Point position = e.GetCurrentPoint(RootCanvas).Position;

                Canvas.SetLeft(InfoPanel, position.X + 10);
                Canvas.SetTop(InfoPanel, position.Y + 10);
            };


            Grid.SetColumn(border, col);
            Grid.SetRow(border, row);
            PeriodicTable.Children.Add(border);
        }
    }
}
