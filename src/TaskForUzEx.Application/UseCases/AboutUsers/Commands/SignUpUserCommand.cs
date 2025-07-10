using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Application.Helpers;
using TaskForUzEx.Domain.Entities;
using TaskForUzEx.Domain.Exceptions;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Commands;

public class SignUpUserCommand : ICommand<bool>
{
    public required string UserName { get; set; }
    public string Password { get; set; }
    [EmailAddress, Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
public class SignUpUserCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<SignUpUserCommand, bool>
{
    public async Task<bool> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.Users.AnyAsync(u => u.UserName == request.UserName ||
                                                                          u.Email == request.Email, cancellationToken);
        if(alreadyExists)
            throw new CustomException(409, "User with this username or email already exists!");

        var user = new User()
        {
            UserName = request.UserName,
            Password = request.Password.Hash(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        await dbContext.Users.AddAsync(user, cancellationToken);
        
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}