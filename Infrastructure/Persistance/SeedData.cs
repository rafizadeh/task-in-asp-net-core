using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Infrastructure.Persistance
{
    public static class SeedData
    {
        private static IConfiguration _configuration;
        public static void LoadInitialEntities(this IServiceCollection services)
        {
            var context = services.BuildServiceProvider().GetRequiredService<IApplicationDbContext>();
            _configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            context.InitialShelves();
        }
        private static async void InitialShelves(this IApplicationDbContext context)
        {
            if (!await context.Shelves.AnyAsync())
            {
                List<Shelf> shelves = new();

                for (int i = 1; i <= 1600; i++)
                {
                    shelves.Add(new Shelf
                    {
                        ShelfNo = i.ToString(),
                    });
                }

                await context.CustomBulkInsertAsync(shelves);
            }
        }
    }
}