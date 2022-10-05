using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace Exercise3.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private const string URL = "http://localhost:51407/Service1.svc/";
        ObservableCollection<KeyValuePair> KeyValuePairsCollection = new ObservableCollection<KeyValuePair>();
        private HttpClient httpClient { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            this.httpClient = new HttpClient();
            KeyValuePairsGrid.ItemsSource = KeyValuePairsCollection;
        }

        private async void OnLoadDataButtonClick(object sender, RoutedEventArgs e)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(URL + "getResource");

            var jsonResponse = await this.DeserializeResponseAsync(response);

            var kvps = JsonSerializer.Deserialize<List<KeyValuePair>>(jsonResponse);

            KeyValuePairsCollection.Clear();
            foreach (var kvp in kvps)
            {
                KeyValuePairsCollection.Add(kvp);
            }
        }

        private async void AddButtonClickEvent(object sender, RoutedEventArgs e)
        {
            var kvp = new KeyValuePair()
            {
                Key = Id_TextBox.Text,
                Value = Value_TextBox.Text
            };

            var response = await httpClient.PostAsync(URL + $"addResource?id={kvp.Key}&value={kvp.Value}", default);

            var jsonResponse = await this.DeserializeResponseAsync(response);
            
            try
            {
                var kvpReturned = JsonSerializer.Deserialize<KeyValuePair>(jsonResponse);

                if (kvpReturned.Key == null)
                {
                    var errorMessage = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse);
                    ResponseBox.Text = errorMessage.Error;
                }
                else
                {
                    ResponseBox.Text = $"Added successfully and returned \r\n{kvpReturned}\r\n";
                    KeyValuePairsCollection.Add(kvpReturned);
                }
            }
            catch (Exception)
            {
                ResponseBox.Text = $"Something went wrong, try again!";
            }
        }

        private async void EditButtonClickEvent(object sender, RoutedEventArgs e)
        {
            var kvp = new KeyValuePair()
            {
                Key = Id_TextBox.Text,
                Value = Value_TextBox.Text
            };
            
            var response = await httpClient.PostAsync(URL + $"updateResource?id={kvp.Key}&value={kvp.Value}", default);
            
            var jsonResponse = await this.DeserializeResponseAsync(response);


            try
            {
                var kvpReturned = JsonSerializer.Deserialize<KeyValuePair>(jsonResponse);

                if (kvpReturned.Key == null)
                {
                    var errorMessage = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse);
                    ResponseBox.Text = errorMessage.Error;
                }
                else
                {
                    var itemToRemove = KeyValuePairsCollection.FirstOrDefault(x => x.Key == kvpReturned.Key);
                    KeyValuePairsCollection.Remove(itemToRemove);
                    ResponseBox.Text = $"Successfully updated the item and returned \r\n{kvpReturned}\r\n";
                    KeyValuePairsCollection.Add(kvpReturned);
                }
            }
            catch (Exception)
            {
                ResponseBox.Text = $"Something went wrong, try again!";
            }
        }

        private async void DeleteButtonClickEvent(object sender, RoutedEventArgs e)
        {
            var kvp = new KeyValuePair()
            {
                Key = Id_TextBox.Text,
                Value = Value_TextBox.Text
            };

            var response = await httpClient.PostAsync(URL + $"updateResource?id={kvp.Key}&value={kvp.Value}&isDel=true", default);

            var jsonResponse = await this.DeserializeResponseAsync(response);

            try
            {
                var kvpReturned = JsonSerializer.Deserialize<KeyValuePair>(jsonResponse);

                if (kvpReturned.Key == null)
                {
                    var errorMessage = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse);
                    ResponseBox.Text = errorMessage.Error;
                }
                else
                {
                    var itemToRemove = KeyValuePairsCollection.FirstOrDefault(x => x.Key == kvpReturned.Key);
                    KeyValuePairsCollection.Remove(itemToRemove);
                    ResponseBox.Text = $"Successfully removed the item and returned \r\n{kvpReturned}\r\n";
                }
            }
            catch (Exception)
            {
                ResponseBox.Text = $"Something went wrong, try again!";
            }
        }

        private async Task<string> DeserializeResponseAsync(HttpResponseMessage response)
        {
            var jsonText = await response.Content.ReadAsStringAsync();
            var validJson = jsonText.Replace("\\", "");
            validJson = validJson.Substring(1, validJson.Length - 2);

            return validJson;
        }

        private void DataGridMouseDoubleClickEvent(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;

            var kvp = row.Item as KeyValuePair;
            Id_TextBox.Text = kvp.Key;
            Value_TextBox.Text = kvp.Value;
        }
    }
}
