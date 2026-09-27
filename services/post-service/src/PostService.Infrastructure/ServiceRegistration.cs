using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostService.Infrastructure.Data;
using PostService.Infrastructure.Repositories;

namespace PostService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayerServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostServiceDbContext>(options =>
               options.UseNpgsql(
                   configuration.GetConnectionString("Default"),
                   b => b.MigrationsAssembly(typeof(PostServiceDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PostServiceDbContext>());
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IHospitalRepository, HospitalRepository>();
            services.AddScoped<IPostOwnerRepository, PostOwnerRepository>();
        }

    }
}
