using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Services;
using BladeStory.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.ViewportAdapters;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册游戏服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGameManagers(this IServiceCollection services)
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
                var gameObjectFactory = sp.GetRequiredService<IEntityFactory>();
                return new SceneManager(configManager, contentManager,
                    sceneFactory, tiledMapRendererFactory, gameObjectFactory);
            });
            services.AddSingleton<ITimeManager>(sp =>
            {
                var game = sp.GetRequiredService<Game>();
                return new TimeManager(game);
            });

            return services;
        }
    }
}
