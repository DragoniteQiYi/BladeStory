namespace BladeStory.Service.Interfaces
{
    public interface IConfigManager
    {
        T LoadConfig<T>(string configPath) where T : class, new();
    }
}
