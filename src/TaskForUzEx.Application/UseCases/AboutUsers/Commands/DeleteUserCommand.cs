using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Application.Helpers;
using TaskForUzEx.Domain.Entities;
using TaskForUzEx.Domain.Exceptions;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Commands;

public class DeleteUserCommand : ICommand<User>
{
    public Guid Id { get; set; }
}
public class DeleteUserCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteUserCommand, User>
{
    public async Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id,cancellationToken)
            ?? throw new CustomException(404, "User not found");
        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.DeletedById = HttpContextHelper.UserId;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}