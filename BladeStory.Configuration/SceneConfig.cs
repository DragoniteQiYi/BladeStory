using BladeStory.Constant;

namespace BladeStory.Configuration
{
    [Serializable]
    public record SceneConfig
    {
        public required string Id {  get; set; }

        public SceneType Type { get; set; }

        public string? Background { get; set; }

        public string? TiledMap { get; set; }
        
        public string? Bgm { get; set; }
    }
}
