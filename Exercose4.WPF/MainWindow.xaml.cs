using Exercose4.WPF.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Exercose4.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<KeyValuePair> KeyValuePairsCollection = new ObservableCollection<KeyValuePair>();
        public MainWindow()
        {
            InitializeComponent();
            KeyValuePairsGrid.ItemsSource = KeyValuePairsCollection;

        }
        WebServiceSoapClient soapClient = new WebServiceSoapClient();
        private void HelloWorldButtonClicked(object sender, RoutedEventArgs e)
        {
            HelloWorldLabel.Content = soapClient.HelloWorld();
        }

        private void OnCelsiusToFahrentheitButtonClick(object sender, RoutedEventArgs e)
        {
            var celsiusTemp = CelsiusTextBox.Text;

            if (double.TryParse(celsiusTemp, out var result))
            {
                var response = soapClient.CelsiusToFahrenheit(result).ToString("0.00");
                FahrenheitResultBox.Text = response;
            }
            else
            {
                FahrenheitResultBox.Text = "Error!!";
            }

        }

        private void OnFahrenheitToCelsiusButtonClick(object sender, RoutedEventArgs e)
        {
            var fahrenheitTemp = FahrenheitTextBox.Text;

            if (double.TryParse(fahrenheitTemp, out var result))
            {
                CelsiusResultBox.Text = soapClient.FahrenheitToCelsius(result).ToString("0.00");
            }
            else
            {
                CelsiusResultBox.Text = "Error!!";
            }
        }

        private void OnLoadDataButtonClick(object sender, RoutedEventArgs e)
        {
            var kvps = soapClient.GetKeyValuePairs();
            
            KeyValuePairsCollection.Clear();
            foreach (var kvp in kvps)
            {
                KeyValuePairsCollection.Add(kvp);
            }
        }

        private void AddButtonClickEvent(object sender, RoutedEventArgs e)
        {
            ResponseText.Text = "";
            var kvp = new KeyValuePair()
            {
                Key = Id_TextBox.Text,
                Value = Value_TextBox.Text
            };

            try
            {
                var response = soapClient.AddKeyValuePair(kvp.Key, kvp.Value);
                KeyValuePairsCollection.Add(response);
                ResponseText.Text = "Successfully added a key value pair!";
            }
            catch(Exception)
            {
                ResponseText.Text = "Something went wrong, make sure your id is not already added and that you have entered a value. Try again!";
            }
        }

        private void EditButtonClickEvent(object sender, RoutedEventArgs e)
        {
            var kvp = new KeyValuePair()
            {
                Key = Id_TextBox.Text,
                Value = Value_TextBox.Text
            };


            try
            {
                var response = soapClient.UpdateResource(kvp.Key, kvp.Value, false);
                
                var itemToRemove = KeyValuePairsCollection.FirstOrDefault(x => x.Key == response.Key);
                KeyValuePairsCollection.Remove(itemToRemove);
                ResponseText.Text = $"Successfully updated the item.";
                KeyValuePairsCollection.Add(response);
            }
            catch (Exception)
            {
                ResponseText.Text = "Something went wrong, maybe the id you entered does not exist. Try again!";
            }
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

        private void DeleteButtonClickEvent(object sender, RoutedEventArgs e)
        {
            var kvp = new KeyValuePair()
            {
                Key = Id_TextBox.Text,
                Value = Value_TextBox.Text
            };

            try
            {
                var response = soapClient.UpdateResource(kvp.Key, kvp.Value, true);

                var itemToRemove = KeyValuePairsCollection.FirstOrDefault(x => x.Key == response.Key);
                KeyValuePairsCollection.Remove(itemToRemove);
                ResponseText.Text = $"Successfully deleted the item.";
            }
            catch (Exception)
            {
                ResponseText.Text = "Something went wrong, maybe the id you entered does not exist. Try again!";
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
