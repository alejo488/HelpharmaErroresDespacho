using System.Text.Json;
using System.Text.Json.Serialization;

namespace Models.Converter
{
    public class FlexibleBoolConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,

                JsonTokenType.String => ParseString(reader.GetString()),
                JsonTokenType.Number => reader.GetInt32() == 1,
                JsonTokenType.Null => null,

                _ => throw new JsonException($"No se puede convertir {reader.TokenType} a bool")
            };
        }

        private static bool? ParseString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (bool.TryParse(value, out var result))
                return result;

            return value.ToLower() switch
            {
                "1" => true,
                "0" => false,
                "ok" => true,
                "si" => true,
                "no" => false,
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteBooleanValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
