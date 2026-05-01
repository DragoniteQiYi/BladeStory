using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace BladeStory.Configuration
{
    [Serializable]
    public record EntityConfig
    {
        public required string Name { get; set; }

        public string? Texture { get; set; }

        [JsonPropertyName("boundsOffset")]
        public Vector2 BoundsOffset { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }
    }
}
