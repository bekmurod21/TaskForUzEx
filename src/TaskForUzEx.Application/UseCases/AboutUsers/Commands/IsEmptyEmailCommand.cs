using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskForUzEx.Application.Abstractions;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Commands;

public class IsEmptyEmailCommand : ICommand<bool>
{
    [EmailAddress]
    public string Email { get; set; }
}
public class IsEmptyEmailCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<IsEmptyEmailCommand, bool>
{
    public async Task<bool> Handle(IsEmptyEmailCommand request, CancellationToken cancellationToken)
    {
        return !await dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
    }
}