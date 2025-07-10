using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Commands;

public class IsEmptyUserNameCommand : ICommand<bool>
{
    public string UserName { get; set; }
}
public class IsEmptyUserNameCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<IsEmptyUserNameCommand, bool>
{
    public async Task<bool> Handle(IsEmptyUserNameCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserName))
            return true;
        return !await dbContext.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken);
    }
}