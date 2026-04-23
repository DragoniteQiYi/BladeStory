using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Middlewares;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUpdatable(this IServiceCollection services)
        {
            services.AddSingleton(sp => (IUpdate)sp.GetRequiredService<IInputManager>());
            services.AddSingleton(sp => (IUpdate)sp.GetRequiredService<ITimeManager>());
            services.AddSingleton(sp => (IUpdate)sp.GetRequiredService<IInputStateMiddleware>());

            return services;
        }
    }
}
