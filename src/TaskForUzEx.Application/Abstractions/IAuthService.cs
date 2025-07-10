using TaskForUzEx.Domain.Entities;

namespace TaskForUzEx.Application.Abstractions;

public interface IAuthService
{
    string GenerateJwtToken(User user);
}