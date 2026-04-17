namespace BladeStory.Configuration
{
    [Serializable]
    public class SceneConfig
    {
        public required string Id {  get; set; }

        public string? TiledMap { get; set; }
        
        public string? Bgm { get; set; }
    }
}
