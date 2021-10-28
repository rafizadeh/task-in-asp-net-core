using Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Cloth> Cloths { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Shelf> Shelves { get; set; }

        Task CommitAsync();
        Task RoleBackAsync();
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        bool IsCurrentTransactionNull();
    }
}