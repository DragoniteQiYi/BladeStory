using BladeStory.Constant;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace BladeStory.Core.Tilemap
{
    public class MapObject
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public MapObjectKind Kind { get; set; }
        public Dictionary<string, string> Properties { get; set; } = [];

        // 多边形/折线的顶点（相对于对象位置）
        public Vector2[]? Points { get; set; }

        // 图块对象的全局ID
        public int? GlobalTileId { get; set; }

        // 获取属性辅助方法
        public string GetProperty(string key, string defaultValue = "")
        {
            return Properties.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public int GetPropertyInt(string key, int defaultValue = 0)
        {
            return Properties.TryGetValue(key, out var value) && int.TryParse(value, out var result)
                ? result : defaultValue;
        }

        public float GetPropertyFloat(string key, float defaultValue = 0f)
        {
            return Properties.TryGetValue(key, out var value) && float.TryParse(value, out var result)
                ? result : defaultValue;
        }

        public bool GetPropertyBool(string key, bool defaultValue = false)
        {
            return Properties.TryGetValue(key, out var value) && bool.TryParse(value, out var result)
                ? result : defaultValue;
        }

        public RectangleF GetBounds()
        {
            return new RectangleF(Position.X, Position.Y, Size.X, Size.Y);
        }
    }
}
