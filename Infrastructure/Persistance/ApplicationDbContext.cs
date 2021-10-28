using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options)
        : base(options) { }

        public virtual DbSet<Cloth> Cloths { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Shelf> Shelves { get; set; }

        public async Task BeginTransactionAsync()
        {
            await base.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (base.Database.CurrentTransaction != null)
                await base.Database.CommitTransactionAsync();
        }

        public bool IsCurrentTransactionNull()
        {
            return base.Database.CurrentTransaction == null;
        }

        public async Task RoleBackAsync()
        {
            if (base.Database.CurrentTransaction != null)
                await base.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}