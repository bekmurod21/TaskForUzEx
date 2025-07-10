using System.ComponentModel.DataAnnotations;
using TaskForUzEx.Domain.Commons;
using TaskForUzEx.Domain.Enums;

namespace TaskForUzEx.Domain.Entities;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public ERole Role { get; set; } = ERole.User;
}