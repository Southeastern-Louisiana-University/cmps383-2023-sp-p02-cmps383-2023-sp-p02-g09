using Microsoft.AspNetCore.Identity;

namespace SP23.P02.Web.Features.Auth
{
    public class UserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Role { get; set; }

        public virtual ICollection<Role> Roles { get;}
        public virtual ICollection<User> Users { get;}
    }
}
