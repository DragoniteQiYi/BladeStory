using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled.Renderers;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
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
    }
}
