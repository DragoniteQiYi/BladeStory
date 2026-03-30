using BladeStory.Service.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.Text.Json;

namespace BladeStory.Service.Services
{
    public class ConfigManager : IConfigManager
    {
        private readonly ContentManager _contentManager;

        private const string CONFIG_PATH = "Configs/";

        public ConfigManager(ContentManager contentManager) 
        {
            _contentManager = contentManager; 
        }

        public T LoadConfig<T>(string configPath) where T : class, new()
        {
#if DEBUG
            return LoadFromJson<T>(configPath);
#else

#endif
        }

        /// <summary>
        /// 开发模式：直接读取JSON文件
        /// </summary>
        private T LoadFromJson<T>(string configPath) where T : class, new()
        {
            try
            {
                
            }
            catch 
            {

            }
        }
    }
}
