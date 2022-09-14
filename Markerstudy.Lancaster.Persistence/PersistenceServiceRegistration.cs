using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Markerstudy.Lancaster.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Markerstudy.Lancaster.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<LancasterDbContext>(opts =>
            //    opts.UseInMemoryDatabase("LancasterDatabase"));

            services.AddDbContext<LancasterDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LancasterDB"), x => x.MigrationsAssembly(typeof(LancasterDbContext).Assembly.FullName)));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            return services;
        }
    }
}
