using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Exercise1
{
    public class Cube : INotifyPropertyChanged
    {
        public string Currency { get; set; }
        [JsonConverter(typeof(CustomDecimalConverter))]
        public decimal Rate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
