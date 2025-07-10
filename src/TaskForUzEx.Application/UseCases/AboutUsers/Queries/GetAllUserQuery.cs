using Microsoft.EntityFrameworkCore;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Application.DTOs.Users;
using TaskForUzEx.Application.Extensions;
using TaskForUzEx.Domain.Configurations;

namespace TaskForUzEx.Application.UseCases.AboutUsers.Queries;

public class GetAllUserQuery : IQuery<IEnumerable<GetUserDto>>
{
    public string Search { get; set; }
    public PaginationParams Params { get; set; }
}
public class GetAllUserQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetAllUserQuery, IEnumerable<GetUserDto>>
{
    public async Task<IEnumerable<GetUserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u => EF.Functions.Like(u.UserName,request.Search) ||
                                          EF.Functions.Like(u.Email,request.Search));
        }

        return await query
            .ToPagedList(request.Params)
            .Select(u => new GetUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role
            })
            .ToListAsync(cancellationToken);
    }
}