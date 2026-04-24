using BladeStory.Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace BladeStory
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            var services = new ServiceCollection();

            using var game = new MainGame(services);

            // 1. 注册游戏实例
            services.AddSingleton<Game>(game);

            // 2. 注册配置服务
            services.AddConfigurations();

            game.Run();
        }
    }
}