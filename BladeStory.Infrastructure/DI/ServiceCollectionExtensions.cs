using BladeStory.Service.Interfaces;
using BladeStory.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.ViewportAdapters;

namespace BladeStory.Infrastructure.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            // 创建 Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("GameSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            return services;
        }

        /// <summary>
        /// 注册游戏服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddSingleton<ISceneManager, SceneManager>();
            services.AddSingleton(sp =>
            {
                var viewportAdapter = sp.GetRequiredService<ViewportAdapter>();
                return new InputManager(viewportAdapter);
            });
            services.AddSingleton(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                return new AssetManager(contentManager);
            });
            services.AddSingleton(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                return new ConfigManager(contentManager);
            });
            services.AddSingleton(sp =>
            {
                var configManager = sp.GetRequiredService<ConfigManager>();
                return new SceneManager(configManager);
            });

            return services;
        }
    }
}
