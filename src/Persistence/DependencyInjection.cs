using Application.Interfaces.Repositories;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddRepositories();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfig = configuration["FirebaseDataBase"];
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", dbConfig);
            services.AddSingleton(s => FirestoreDb.Create("cartoncaps-ca15c"));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddTransient<IReferralRepository, ReferralRepository>();
        }
    }
}
