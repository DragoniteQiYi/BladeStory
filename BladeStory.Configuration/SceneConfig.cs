namespace BladeStory.Configuration
{
    [Serializable]
    public class SceneConfig
    {
        public required string SceneId {  get; set; }

        public string? TiledMapId { get; set; }
        
        public string? BgmId { get; set; }
    }
}
