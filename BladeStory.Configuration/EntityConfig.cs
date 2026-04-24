using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace BladeStory.Configuration
{
    [Serializable]
    public record EntityConfig
    {
        public string? Name;

        public Vector2Data? Position;

        public string? Texture;
    }
}
