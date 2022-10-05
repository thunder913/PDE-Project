using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Exercise2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _commitEdit = true;
        ObservableCollection<Person> PeopleCollection = new ObservableCollection<Person>();
        public MainWindow()
        {
            InitializeComponent();

            People.ItemsSource = PeopleCollection;

            var people = ReadPeopleFromFile("people.json");

            if (people.Person != null)
            {
                foreach (var person in people.Person)
                {
                    PeopleCollection.Add(person);
                }
            }
        }

        private ArrayOfPerson ReadPeopleFromFile(string file)
        {
            var jsonText = File.ReadAllText(file);

            var root = JsonSerializer.Deserialize<Root>(jsonText, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });

            return root.ArrayOfPerson;
        }

        private void EditRowEvent(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (_commitEdit)
            {
                _commitEdit = false;
                // Without CommitEdit, it does not work when adding a new row
                People.CommitEdit();

                SaveCollectionToFile();

                _commitEdit = true;
            }
        }

        private void SaveCollectionToFile()
        {
            var people = new Root()
            {
                ArrayOfPerson = new ArrayOfPerson()
                {
                    Person = PeopleCollection
                }
            };

            var jsonText = JsonSerializer.Serialize(people, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });

            File.WriteAllText("people.json", jsonText);
        }
    }
}
