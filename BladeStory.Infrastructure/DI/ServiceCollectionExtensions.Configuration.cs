using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BladeStory.Infrastructure.DI
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            // 创建 Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("GameSettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            return services;
        }
    }
}
