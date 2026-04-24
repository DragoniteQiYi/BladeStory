namespace BladeStory.Core.Tilemap
{
    public class MapLayerInfo
    {
        public string? Name { get; set; }
        public string? Type { get; set; } // "tile", "object", "image"
        public bool IsVisible { get; set; }
        public float Opacity { get; set; }
    }
}
