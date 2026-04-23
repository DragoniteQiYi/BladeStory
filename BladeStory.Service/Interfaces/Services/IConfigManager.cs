namespace BladeStory.Service.Interfaces.Services
{
    public interface IConfigManager
    {
        T LoadConfig<T>(string configPath) where T : class, new();
    }
}
