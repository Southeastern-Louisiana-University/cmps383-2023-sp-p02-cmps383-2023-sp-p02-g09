using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SP23.P02.Web.Features.Auth
{

    public class User : IdentityUser<int>
    {
        public object Roles { get; internal set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string[]? Roles { get; set; }
        public virtual ICollection<Role> Role { get;set; }
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
