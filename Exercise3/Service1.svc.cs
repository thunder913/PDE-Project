using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Exercise3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static string FILENAME = "C:\\Users\\andon\\source\\repos\\PRS_Exercises\\Exercise3\\resource.json";
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string AddIt(int num1, int num2)
        {
            return Convert.ToString(num1 + num2);
        }

        public string getResource()
        {
            var keyValuePairs = ReadKeyValuePairsFromFile(FILENAME);

            var jsonText = JsonSerializer.Serialize(keyValuePairs, new JsonSerializerOptions());

            return jsonText;
        }

        public string addResource(string id, string value)
        {
            var keyValuePairs = ReadKeyValuePairsFromFile(FILENAME);

            if (keyValuePairs.Any(x => x.Key == id))
            {
                return "{\"Error\": \"Id already exists\"}";
            }

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(value))
            {
                return "{\"Error\": \"Id or value cannot be empty\"}";
            }

            var added = new KeyValuePair() { Key = id, Value = value };

            keyValuePairs.Add(added);
            
            SaveDataToFile(keyValuePairs, FILENAME);

            return JsonSerializer.Serialize(added, new JsonSerializerOptions());

        }

        public string updateResource(string id, string value, bool isdel = false)
        {
            var keyValuePairs = ReadKeyValuePairsFromFile(FILENAME);

            var item = keyValuePairs.FirstOrDefault(x => x.Key == id);
            if (item == null)
            {
                return "{\"Error\": \"Id does not exists\"}";
            }

            if (isdel)
            {
                keyValuePairs.Remove(item);
            }
            else
            {
                item.Value = value;
            }

            SaveDataToFile(keyValuePairs, FILENAME);
            return JsonSerializer.Serialize(item, new JsonSerializerOptions());
        }

        private List<KeyValuePair> ReadKeyValuePairsFromFile(string file)
        {
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            
            var jsonText = File.ReadAllText(file);
            
            var keyValuePairs = JsonSerializer.Deserialize<List<KeyValuePair>>(jsonText, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });

            return keyValuePairs;
        }

        private void SaveDataToFile(List<KeyValuePair> keyValuePairs, string file)
        {
            var jsonText = JsonSerializer.Serialize(keyValuePairs, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });

            File.WriteAllText(file, jsonText);
        }
    }
}
