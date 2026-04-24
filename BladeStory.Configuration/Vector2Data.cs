using Microsoft.Xna.Framework;

namespace BladeStory.Configuration
{
    public record Vector2Data(float X, float Y)
    {
        public static implicit operator Vector2(Vector2Data d) => new(d.X, d.Y);
        public static implicit operator Vector2Data(Vector2 v) => new(v.X, v.Y);
    }
}
