using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApprendreDotNet.DbCore;

namespace ApprendreDotNet.Extension
{
    public static class ApplicationServiceExtension
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            
        }
        public static IServiceCollection DbInjection(this IServiceCollection services, IConfiguration configuration) 
        {
            var configurationConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyAppDbContext>(options => options.UseNpgsql(configurationConnectionString));
            return services;
        }
        private static IServiceCollection ServiceInjection(this IServiceCollection services)
        {
            return services;
        }
    }
}
