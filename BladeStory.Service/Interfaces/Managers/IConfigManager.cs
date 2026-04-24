namespace BladeStory.Service.Interfaces.Managers
{
    public interface IConfigManager
    {
        T LoadConfig<T>(string configPath) where T : class, new();
    }
}
