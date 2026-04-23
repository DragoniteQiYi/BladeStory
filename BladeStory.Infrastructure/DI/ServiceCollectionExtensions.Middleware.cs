using BladeStory.Service.Interfaces.Middlewares;
using BladeStory.Service.Interfaces.Services;
using BladeStory.Service.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddSingleton<IInputStateMiddleware>(sp =>
            {
                var inputManager = sp.GetRequiredService<IInputManager>();
                return new InputStateMiddleware(inputManager);
            });
            services.AddSingleton(sp =>
            {
                var inputStateMiddleware = sp.GetRequiredService<IInputStateMiddleware>();
                return new PlayerCommandMiddleware(inputStateMiddleware);
            });

            return services;
        }
    }
}
