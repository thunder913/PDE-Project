using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Text.Json;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    private const string FILENAME = "C:\\Users\\andon\\source\\repos\\PRS_Exercises\\Exercise4\\resource.json";
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public int Add(int a, int b)
    {
        return a + b;
    }

    [WebMethod]
    public double FahrenheitToCelsius(double fahrenheit)
    {
        return (((fahrenheit - 32) / 9.0) * 5.0);
    }

    [WebMethod]
    public double CelsiusToFahrenheit(double celsius)
    {
        return (((celsius * 9.0) / 5.0) + 32);
    }

    [WebMethod]
    public List<KeyValuePair> GetKeyValuePairs()
    {
        var jsonText = File.ReadAllText(FILENAME);

        var kvps = JsonSerializer.Deserialize<List<KeyValuePair>>(jsonText);

        return kvps;
    }

    [WebMethod]
    public KeyValuePair AddKeyValuePair(string id, string value)
    {
        var jsonText = File.ReadAllText(FILENAME);

        var kvps = JsonSerializer.Deserialize<List<KeyValuePair>>(jsonText);

        if (kvps.Any(x => x.Key == id))
        {
            throw new ArgumentException();
        }

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException();
        }

        var added = new KeyValuePair() { Key = id, Value = value };
        kvps.Add(added);

        SaveDataToFile(kvps, FILENAME);

        return added;
    }

    [WebMethod]
    public KeyValuePair UpdateResource(string id, string value, bool isdel = false)
    {
        var jsonText = File.ReadAllText(FILENAME);

        var kvps = JsonSerializer.Deserialize<List<KeyValuePair>>(jsonText);

        var item = kvps.FirstOrDefault(x => x.Key == id);
        
        if (item == null)
        {
            throw new ArgumentNullException();
        }

        if (isdel)
        {
            kvps.Remove(item);
        }
        else
        {
            item.Value = value;
        }

        SaveDataToFile(kvps, FILENAME);
        return item;
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
