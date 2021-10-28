using Application.Common.Interfaces;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                string whichConnection = configuration["Database:Connected"];
                options.UseSqlServer(configuration[$"Database:{whichConnection}"]);
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}