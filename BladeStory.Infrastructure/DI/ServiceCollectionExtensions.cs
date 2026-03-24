using BladeStory.Service.Interfaces;
using BladeStory.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

namespace BladeStory.Infrastructure.DI
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册游戏服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddSingleton<IGameInputService, GameInputService>();
            return services;
        }
    }
}
