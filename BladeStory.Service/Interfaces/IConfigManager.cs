namespace BladeStory.Service.Interfaces
{
    public interface IConfigManager
    {
        Task<T> LoadConfig<T>(string configPath) where T : class, new();
    }
}
