using TaskForUzEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskForUzEx.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}