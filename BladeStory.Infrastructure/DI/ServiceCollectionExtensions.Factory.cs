using BladeStory.Service.Factories;
using BladeStory.Service.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
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
            services.AddSingleton<ISceneFactory, SceneFactory>();
            services.AddSingleton<IEntityFactory, EntityFactory>(sp =>
            {
                var contentManager = sp.GetRequiredService<ContentManager>();
                return new EntityFactory(contentManager);
            });

            return services;
        }
    }
}
