using Microsoft.Xna.Framework;

namespace BladeStory.Configuration
{
    [Serializable]
    public record EntityConfig
    {
        public required string Name { get; set; }

        public string? Texture { get; set; }
    }
}
