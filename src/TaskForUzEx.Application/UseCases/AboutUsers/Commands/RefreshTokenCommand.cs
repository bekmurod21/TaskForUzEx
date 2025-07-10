using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Application.Helpers;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Commands;

public class RefreshTokenCommand : ICommand<string>
{
}
public class RefreshTokenCommandHandler(IApplicationDbContext dbContext,IAuthService authService) : ICommandHandler<RefreshTokenCommand, string>
{
    public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == HttpContextHelper.UserId, cancellationToken)
            ?? throw new Exception("User not found");
        
        return authService.GenerateJwtToken(user);
    }
}