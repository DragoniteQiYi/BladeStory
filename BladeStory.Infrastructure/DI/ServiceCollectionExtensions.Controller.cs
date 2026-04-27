using BladeStory.Gameplay.Exploration.Controllers;
using BladeStory.Service.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                var playerCommandMiddleware = sp.GetRequiredService<PlayerCommandMiddleware>();
                return new PlayerController(playerCommandMiddleware);
            });

            return services;
        }
    }
}
