using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BladeStory.Utility
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), // 👈 允许驼峰/小写匹配
                new Vector2JsonConverter()
            }
        };

        /// <summary>
        /// 读取JSON到指定类型
        /// </summary>
        public static T? Deserialize<T>(ReadOnlySpan<byte> jsonData, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<T>(jsonData, options ?? DefaultOptions);
        }

        /// <summary>
        /// 从UTF-8 JSON字节流读取（支持ArrayPool重用）
        /// </summary>
        public static T? DeserializeFromStream<T>(Stream utf8Json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<T>(utf8Json, options ?? DefaultOptions);
        }

        /// <summary>
        /// 异步从流中读取JSON
        /// </summary>
        public static async ValueTask<T?> DeserializeFromStreamAsync<T>(
            Stream utf8Json,
            JsonSerializerOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(utf8Json, options ?? DefaultOptions, cancellationToken);
        }

        /// <summary>
        /// 使用Utf8JsonReader手动读取（最高性能，手动控制）
        /// </summary>
        public static T? DeserializeManual<T>(ReadOnlySpan<byte> jsonData) where T : new()
        {
            var reader = new Utf8JsonReader(jsonData);

            if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            T result = new T();
            var properties = typeof(T).GetProperties();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propName = reader.GetString()!;
                    reader.Read();

                    // 这里可以根据属性类型手动解析，性能最优
                    // 需要为每个类型实现具体的解析逻辑
                }
            }

            return result;
        }

        /// <summary>
        /// 使用JsonNode进行动态/部分读取
        /// </summary>
        public static JsonNode? ParseToNode(string json)
        {
            return JsonNode.Parse(json);
        }
    }
}
