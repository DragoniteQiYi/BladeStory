using BladeStory.Service.Factories;
using BladeStory.Service.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<ITiledMapRendererFactory>(sp =>
            {
                var graphicsDevice = sp.GetRequiredService<GraphicsDevice>();
                return new TiledMapRendererFactory(graphicsDevice);
            });
            services.AddSingleton<ISceneFactory>(sp =>
            {
                var tiledMapRendererFactory = sp.GetRequiredService<ITiledMapRendererFactory>();
                return new SceneFactory(tiledMapRendererFactory);
            });
            services.AddSingleton<IEntityFactory, EntityFactory>();

            return services;
        }
    }
}
