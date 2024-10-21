using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Proyecto.Services
{
    public class CustomDateTimeConverter : JsonConverter
    {
        private static readonly string[] Formats = new[]
        {
            "yyyyMMdd'T'HHmmss'Z'",
            "yyyyMMdd'T'HHmmssfff'Z'",
            "yyyyMMdd'T'HHmmssK",
            "yyyyMMdd'T'HHmmssfffK"
        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var dateStr = ((string)reader.Value)?.Trim();
                dateStr = FixDateTimeString(dateStr);

                foreach (var format in Formats)
                {
                    if (DateTime.TryParseExact(dateStr, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime result))
                    {
                        return result;
                    }
                }
            }
            throw new JsonReaderException($"Error al convertir la cadena '{reader.Value}' en DateTime. Formatos esperados: {string.Join(", ", Formats)}.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            writer.WriteValue(date.ToString(Formats[0], CultureInfo.InvariantCulture));
        }

        private string FixDateTimeString(string dateStr)
        {
            dateStr = Regex.Replace(dateStr, @"[^\d:T+-Z]", "");

            var parts = dateStr.Split(new[] { 'T' }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var timeParts = parts[1].Split(':');
                if (timeParts.Length >= 3)
                {
                    var secondsPart = timeParts[2];
                    var seconds = secondsPart.Substring(0, 2);
                    var milliseconds = secondsPart.Length > 2 ? secondsPart.Substring(2) : "000";

                    milliseconds = milliseconds.Length > 3 ? milliseconds.Substring(0, 3) : milliseconds.PadRight(3, '0');

                    var timeStr = $"{timeParts[0]}:{timeParts[1]}:{seconds}";
                    if (milliseconds != "000")
                    {
                        timeStr += $".{milliseconds}";
                    }
                    timeStr += "Z";

                    dateStr = $"{parts[0]}T{timeStr}";
                }
            }

            if (!dateStr.EndsWith("Z"))
            {
                dateStr += "Z";
            }

            return dateStr;
        }
    }
}
