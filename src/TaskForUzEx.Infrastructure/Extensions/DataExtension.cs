using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskForUzEx.Infrastructure.Persistense;

namespace TaskForUzEx.Infrastructure.Extensions;

public static class DataExtension
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();
    }
}