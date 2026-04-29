using BladeStory.Gameplay.Exploration.Controllers;
using BladeStory.Service.Interfaces.Managers;
using BladeStory.Service.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var playerCommandMiddleware = sp.GetRequiredService<PlayerCommandMiddleware>();
                var contentManager = sp.GetRequiredService<ContentManager>();
                var entityManager = sp.GetRequiredService<IEntityManager>();
                var sceneManager = sp.GetRequiredService<ISceneManager>();
                return new PlayerController(playerCommandMiddleware, contentManager, entityManager,
                    sceneManager);
            });

            return services;
        }
    }
}
