using Domain.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        Task CustomBulkInsertAsync<T>(IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }
}