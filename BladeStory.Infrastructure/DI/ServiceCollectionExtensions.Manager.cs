using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Managers;
using BladeStory.Service.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using Myra.Graphics2D.UI;

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
            services.AddSingleton<ITileMapManager>(sp =>
            {
                var graphicsDevice = sp.GetRequiredService<GraphicsDevice>();
                var contentManager = sp.GetRequiredService<ContentManager>();
                return new TileMapManager(graphicsDevice, contentManager);
            });
            services.AddSingleton<IEntityManager>(sp =>
            {
                var entityFactory = sp.GetRequiredService<IEntityFactory>();
                var configManager = sp.GetRequiredService<IConfigManager>();
                return new EntityManager(entityFactory, configManager);
            });
            services.AddSingleton<ISceneManager>(sp =>
            {
                var configManager = sp.GetRequiredService<IConfigManager>();
                var contentManager = sp.GetRequiredService<ContentManager>();
                var sceneFactory = sp.GetRequiredService<ISceneFactory>();
                var tiledMapRendererFactory = sp.GetRequiredService<ITiledMapRendererFactory>();
                var tileMapManager = sp.GetRequiredService<ITileMapManager>();
                var entityManager = sp.GetRequiredService<IEntityManager>();
                var backgroundManager = sp.GetRequiredService<IBackgroundManager>();
                var audioManager = sp.GetRequiredService<IAudioManager>();
                var assetManager = sp.GetRequiredService<IAssetManager>();
                return new SceneManager(configManager, contentManager, sceneFactory,
                    tiledMapRendererFactory, tileMapManager, entityManager, backgroundManager,
                    audioManager, assetManager);
            });
            services.AddSingleton<ITimeManager>(sp =>
            {
                var game = sp.GetRequiredService<Game>();
                return new TimeManager(game);
            });
            services.AddSingleton<IUIManager>(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                var sceneManager = sp.GetRequiredService<ISceneManager>();
                var desktop = sp.GetRequiredService<Desktop>();
                return new UIManager(contentManager, sceneManager, desktop);
            });
            services.AddSingleton<IBackgroundManager>(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                var graphicsDevice = sp.GetRequiredService<GraphicsDevice>();
                return new BackgroundManager(contentManager, graphicsDevice);
            });
            services.AddSingleton<IAudioManager>(sp =>
            {
                var assetManager = sp.GetRequiredService<IAssetManager>();
                return new AudioManager(assetManager);
            });

            return services;
        }
    }
}
