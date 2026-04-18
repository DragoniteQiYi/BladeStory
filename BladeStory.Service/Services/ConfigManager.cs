using BladeStory.Service.Interfaces;
using BladeStory.Utility;
using Microsoft.Xna.Framework.Content;

namespace BladeStory.Service.Services
{
    public class ConfigManager : IConfigManager
    {
        private readonly ContentManager _contentManager;

        private static readonly string BASE_PATH
            = AppDomain.CurrentDomain.BaseDirectory;

        public ConfigManager(ContentManager contentManager) 
        {
            _contentManager = contentManager;

            Console.WriteLine($"[ConfigManager]: 配置管理模块初始化成功");
        }

        public async Task<T> LoadConfig<T>(string configPath) 
            where T : class, new()
        {
#if DEBUG
            var configs = await LoadFromJson<T>(configPath);
            return configs;
#else
            
#endif
        }

        /// <summary>
        /// 开发模式：直接读取JSON文件
        /// </summary>
        private async Task<T?> LoadFromJson<T>(string configPath) 
            where T : class, new()
        {
            try
            {
                var path = Path.Combine(BASE_PATH, configPath);
                if (!File.Exists(path))
                {
                    Console.WriteLine("ERROR-[ConfigManager]: 目标配置文件不存在");
                    return null;
                }
                using var stream = File.OpenRead(path);
                return await JsonHelper.DeserializeFromStreamAsync<T>(stream);
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR-[ConfigManager]: 读取配置文件失败 " + e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}
