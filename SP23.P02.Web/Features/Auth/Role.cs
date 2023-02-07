using Microsoft.AspNetCore.Identity;

namespace SP23.P02.Web.Features.Auth
{
    public class Role : IdentityRole<int>
    {
        public int UserId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; } = string.Empty;
        public virtual ICollection<User> Users { get; set; }
    }
}
