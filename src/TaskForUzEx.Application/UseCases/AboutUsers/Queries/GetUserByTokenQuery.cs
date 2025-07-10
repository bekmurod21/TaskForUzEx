using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Application.DTOs.Users;
using TaskForUzEx.Application.Helpers;
using TaskForUzEx.Domain.Entities;
using TaskForUzEx.Domain.Exceptions;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Queries;

public class GetUserByTokenQuery : IQuery<GetUserForMeDto>
{
}
public class GetUserByTokenQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetUserByTokenQuery, GetUserForMeDto>
{
    public async Task<GetUserForMeDto> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == HttpContextHelper.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException();

        return new GetUserForMeDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
            CreatedAt = user.CreatedAt
        };
    }
}