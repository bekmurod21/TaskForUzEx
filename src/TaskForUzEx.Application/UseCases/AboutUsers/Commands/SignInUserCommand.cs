using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Application.Helpers;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Commands;

public class SignInUserCommand : ICommand<string>
{
    public string Login { get; set; }
    public string Password { get; set; }
}
public class SignInUserCommandHandler(IApplicationDbContext dbContext,IAuthService authService) : ICommandHandler<SignInUserCommand, string>
{
    public async Task<string> Handle(SignInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == request.Login || u.Email == request.Login, cancellationToken)
                   ?? throw new UnauthorizedAccessException("Email or UserName is incorrect");

        if(!PasswordHelper.Verify(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Password is incorrect");
        }
        
        return authService.GenerateJwtToken(user);
    }
}