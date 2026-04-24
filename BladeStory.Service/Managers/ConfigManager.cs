using BladeStory.Service.Interfaces.Managers;
using BladeStory.Utility;
using Microsoft.Xna.Framework.Content;

namespace BladeStory.Service.Managers
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

        public T LoadConfig<T>(string configPath) 
            where T : class, new()
        {
#if DEBUG
            var configs = LoadFromJson<T>(configPath);
#pragma warning disable CS8603 // 可能返回 null 引用。
            return configs;
#pragma warning restore CS8603 // 可能返回 null 引用。
#else
            
#endif
        }

        /// <summary>
        /// 开发模式：直接读取JSON文件
        /// </summary>
        private T? LoadFromJson<T>(string configPath)
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
                return JsonHelper.DeserializeFromStream<T>(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR-[ConfigManager]: 读取配置文件失败 " + e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 开发模式：直接读取JSON文件
        /// </summary>
        private async Task<T?> LoadFromJsonAsync<T>(string configPath) 
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
