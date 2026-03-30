namespace BladeStory.Service.Interfaces
{
    public interface IConfigManager
    {
        T LoadConfg<T>(string configPath);
    }
}
