using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exercise1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _commitEdit = true;
        ObservableCollection<Cube> CurrenciesCollection = new ObservableCollection<Cube>();
        public MainWindow()
        {
            InitializeComponent();
            Currencies.ItemsSource = CurrenciesCollection;

            var currencies = ReadCurrenciesFromFile("currencies.json");

            foreach (var cube in currencies.Cubes)
            {
                CurrenciesCollection.Add(cube);
            }

            Date.Content = currencies.Time.ToString("dd/MM/yyyy");
        }

        private Currency? ReadCurrenciesFromFile(string file)
        {
            var jsonText = File.ReadAllText(file);

            var currencies = JsonSerializer.Deserialize<Currency>(jsonText, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });

            return currencies;
        }
        
        private void EditRowEvent(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (_commitEdit)
            {
                _commitEdit = false;
                // Without CommitEdit, it does not work when adding a new row
                Currencies.CommitEdit();

                SaveCollectionToFile();

                _commitEdit = true;
            }
        }

        private void SaveCollectionToFile()
        {
            var currencies = new Currency()
            {
                Time = DateTime.Now,
                Cubes = CurrenciesCollection.ToList()
            };

            var jsonText = JsonSerializer.Serialize(currencies, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });

            File.WriteAllText("currencies.json", jsonText);
        }
    }
}
