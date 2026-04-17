using BladeStory.Infrastructure.DI;
using BladeStory.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BladeStory
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            var services = new ServiceCollection();

            // 1. 注册游戏实例
            services.AddSingleton(sp =>
            {
                return new MainGame(services);
            });

            // 2. 注册配置服务
            services.AddConfigurations();

            using var serviceProvider = services.BuildServiceProvider();
            using var game = serviceProvider.GetRequiredService<MainGame>();
            game.Run();
        }
    }
}