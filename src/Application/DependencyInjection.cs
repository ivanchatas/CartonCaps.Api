using System.Reflection;
using Application.Interfaces.Services;
using Application.Resources;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExternalUrls>(
                "ExternalUrls",
                options => configuration.GetSection("ExternalUrls"));

            var assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assembly);

            services.Services();

            return services;
        }

        public static IServiceCollection Services(this IServiceCollection services) 
        {
            services.AddTransient<IReferralService, ReferralService>();
            return services;
        }


    }
}
