using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Domain.Entities;

namespace TaskForUzEx.Infrastructure.Persistense;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options),IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}