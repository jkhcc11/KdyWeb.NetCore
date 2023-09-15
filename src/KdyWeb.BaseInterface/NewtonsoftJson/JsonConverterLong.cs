using System;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// NewtonsoftJson Long类型处理
    /// </summary>
    public class JsonConverterLong : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue("");
                return;
            }

            writer.WriteValue(value + "");
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == null ||
                reader.Value == null ||
                (reader.ValueType != typeof(long) &&
                reader.ValueType != typeof(string)))
            {
                return null;
            }

            long.TryParse(reader.Value.ToString(), out long value);
            return value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long) ||
                   objectType == typeof(string);
        }
    }
}
