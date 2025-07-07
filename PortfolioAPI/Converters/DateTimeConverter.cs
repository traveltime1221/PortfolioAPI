namespace PortfolioAPI.Converters
{
    using System;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string[] _formats = { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss" };

        // 修正 ReadJson 方法簽名
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString(); // 正確使用 Utf8JsonReader 的 GetString 方法
            DateTime result;

            // 使用 TryParseExact 解析不同格式的日期
            if (!DateTime.TryParseExact(dateString, _formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                throw new JsonException("Invalid date format.");
            }

            return result;
        }

        // 修正 WriteJson 方法簽名
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss")); // 格式化日期
        }
    }
}