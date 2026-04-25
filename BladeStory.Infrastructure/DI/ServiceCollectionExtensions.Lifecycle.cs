using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Middlewares;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartable(this IServiceCollection services)
        {
            services.AddSingleton(sp => (IStartable)sp.GetRequiredService<ISceneManager>());
            services.AddSingleton(sp => (IStartable)sp.GetRequiredService<IEntityManager>());
            services.AddSingleton(sp => (IStartable)sp.GetRequiredService<IUIManager>());
            
            return services;
        }

        public static IServiceCollection AddUpdatable(this IServiceCollection services)
        {
            services.AddSingleton(sp => (IUpdatable)sp.GetRequiredService<IInputManager>());
            services.AddSingleton(sp => (IUpdatable)sp.GetRequiredService<ITimeManager>());
            services.AddSingleton(sp => (IUpdatable)sp.GetRequiredService<IInputStateMiddleware>());
            services.AddSingleton(sp => (IUpdatable)sp.GetRequiredService<IUIManager>());

            return services;
        }
    }
}
