using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Concretes;
using UserService.Infrastructure.Repositories.Interfaces;
using UserService.Infrastructure.Services.IdentityVerification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayerServiceConfiguration(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            services.AddDbContext<UserServiceDbContext>(options =>
               options.UseNpgsql(
                   configuration.GetConnectionString("Default"),
                   b =>
                   {
                       b.MigrationsAssembly(typeof(UserServiceDbContext).Assembly.FullName);
                       // Notification preferences are stored as jsonb lists.
                       b.ConfigureDataSource(ds => ds.EnableDynamicJson());
                   }));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserServiceDbContext>());

            services.AddIdentityVerification(configuration, isProduction);
        }

        private static void AddIdentityVerification(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            var section = configuration.GetSection(IdentityVerificationOptions.SectionName);
            var options = section.Get<IdentityVerificationOptions>() ?? new IdentityVerificationOptions();
            services.Configure<IdentityVerificationOptions>(section);

            if (options.Enabled && string.IsNullOrWhiteSpace(options.HashKey))
                throw new InvalidOperationException("IdentityVerification:HashKey is required when identity verification is enabled.");

            services.AddSingleton<TCIdentityNumberHasher>();

            switch (options.Provider)
            {
                case var p when p.Equals(IdentityVerificationOptions.None, StringComparison.OrdinalIgnoreCase):
                    services.AddSingleton<IIdentityVerifier, DisabledIdentityVerifier>();
                    break;
                case var p when p.Equals(IdentityVerificationOptions.DevChecksum, StringComparison.OrdinalIgnoreCase):
                    if (isProduction)
                        throw new InvalidOperationException("IdentityVerification:Provider=DevChecksum accepts made-up numbers and cannot be used in Production.");
                    services.AddSingleton<IIdentityVerifier, DevChecksumIdentityVerifier>();
                    break;
                // An institution with KPS access adds its verifier here, e.g. case "Kps".
                default:
                    throw new InvalidOperationException($"Unknown IdentityVerification:Provider '{options.Provider}'.");
            }
        }
    }
}
