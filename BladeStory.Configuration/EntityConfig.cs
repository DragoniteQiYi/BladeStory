using Microsoft.Xna.Framework;

namespace BladeStory.Configuration
{
    [Serializable]
    public record EntityConfig
    {
        public string? Name;

        public Vector2 Position;

        public string? Texture;
    }
}
