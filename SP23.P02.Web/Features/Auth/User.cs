using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SP23.P02.Web.Features.Auth
{

    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserRole> Roles { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string[]? Roles { get; set; }
    }

    public class CreateUserDto
    {
        
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string[]? Roles { get; set; }
    }

    public class LoginDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
