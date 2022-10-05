using PRS_Exercises;
using System.Text.Json;

Console.WriteLine("");
var option = Console.ReadLine();

var jsonText = File.ReadAllText("currencies.json");

var currencies = JsonSerializer.Deserialize<Currency>(jsonText, new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
});

foreach (var item in currencies.Cubes)
{
    Console.WriteLine(item.Currency);
}