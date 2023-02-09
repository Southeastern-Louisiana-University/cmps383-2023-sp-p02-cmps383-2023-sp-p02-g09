using Microsoft.AspNetCore.Identity;

namespace SP23.P02.Web.Features.Auth
{
    public class UserRole : IdentityUserRole<int>
        {
            public string Name { get; set; }
            public List<string> Permissions { get; set; }
            public List<string> Capabilities { get; set; }
            public List<User> Users { get; set; }
        public ICollection<UserRole> Roles { get; set; }    

            public UserRole(string name)
            {
                Name = name;
                Permissions = new List<string>();
                Capabilities = new List<string>();
                Users = new List<User>();
            }

            public void AddUser(User user)
            {
                Users.Add(user);
            }

            public void RemoveUser(User user)
            {
                Users.Remove(user);
            }

            public bool HasPermission(string permission)
            {
                return Permissions.Contains(permission);
            }
        }

     

}

