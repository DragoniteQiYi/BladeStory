using BladeStory.Service.Factories;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace BladeStory.Infrastructure.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var graphicsDevice = sp.GetRequiredService<GraphicsDevice>();
                return new TiledMapRenderer(graphicsDevice);
            });

            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            // 创建 Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("GameSettings.json", optional: false, reloadOnChange: false)
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
            services.AddSingleton<IInputManager>(sp =>
            {
                var viewportAdapter = sp.GetRequiredService<ViewportAdapter>();
                return new InputManager(viewportAdapter);
            });
            services.AddSingleton<IAssetManager>(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                return new AssetManager(contentManager);
            });
            services.AddSingleton<IConfigManager>(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                return new ConfigManager(contentManager);
            });
            services.AddSingleton<ISceneManager>(sp =>
            {
                var configManager = sp.GetRequiredService<IConfigManager>();
                var contentManager = sp.GetRequiredService<ContentManager>();
                var sceneFactory = sp.GetRequiredService<ISceneFactory>();
                var tiledMapRendererFactory = sp.GetRequiredService<ITiledMapRendererFactory>();
                return new SceneManager(configManager, contentManager, sceneFactory, tiledMapRendererFactory);
            });

            return services;
        }

        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<ITiledMapRendererFactory>(sp =>
            {
                var graphicsDevice = sp.GetRequiredService<GraphicsDevice>();
                return new TiledMapRendererFactory(graphicsDevice);
            });
            services.AddSingleton<ISceneFactory>(sp =>
            {
                var graphicsDevice = sp.GetRequiredService<GraphicsDevice>();
                var tiledMapRendererFactory = sp.GetRequiredService<ITiledMapRendererFactory>();
                return new SceneFactory(graphicsDevice, tiledMapRendererFactory);
            });

            return services;
        }
    }
}
