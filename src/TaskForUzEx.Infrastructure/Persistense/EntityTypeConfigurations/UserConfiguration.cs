using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskForUzEx.Application.Helpers;
using TaskForUzEx.Domain.Entities;
using TaskForUzEx.Domain.Enums;

namespace TaskForUzEx.Infrastructure.Persistense.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.IsDeleted);
        builder.HasQueryFilter(u => !u.IsDeleted);
        builder.HasData(new User()
        {
            Id = Guid.NewGuid(), Email = "boqiyev482@gmail.com", Password = "Shunchaki21".Hash(), FirstName = "Admin",
            Role = ERole.Admin
        });
    }
}