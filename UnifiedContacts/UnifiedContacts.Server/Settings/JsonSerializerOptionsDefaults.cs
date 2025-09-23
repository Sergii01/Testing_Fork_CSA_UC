using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdminPortal.Helper
{
    public class JsonStringNumericConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader))
                    {
                        return jsonDoc.RootElement.GetRawText();
                    }
                case JsonTokenType.String:
                default:
                    return reader.GetString() ?? "";
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }

    public static class JsonSerializerOptionsDefaults
    {
        /// <summary>
        /// Creates JsonSerilizerOption object with the default JSON options, which are also set to be used by the DotNet request/response handler.
        /// Options set are "PropertyNamingPolicy", "PropertyNameCaseInsensitive" and Converters.
        /// </summary>
        /// <returns>Default JSON options</returns>
        public static JsonSerializerOptions GetDefaultOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new JsonStringNumericConverter() },
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
        }
    }
}
