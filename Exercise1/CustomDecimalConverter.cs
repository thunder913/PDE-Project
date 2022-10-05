using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CustomDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                if (decimal.TryParse(reader.GetString(), System.Globalization.NumberStyles.Any, 
                    CultureInfo.InvariantCulture, out decimal decimalValue))
                {
                    return decimalValue;
                }

                throw new JsonException("Invalid value"); // some exception
            case JsonTokenType.Number:
                return reader.GetDecimal();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value);
}