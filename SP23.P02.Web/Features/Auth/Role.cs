using Microsoft.AspNetCore.Identity;

namespace SP23.P02.Web.Features.Auth
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<string> Users { get; set;}
    }
}
