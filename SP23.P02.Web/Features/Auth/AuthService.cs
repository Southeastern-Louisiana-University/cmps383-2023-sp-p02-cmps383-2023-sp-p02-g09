using Microsoft.AspNetCore.Identity;
using SP23.P02.Web.Data;

namespace SP23.P02.Web.Features.Auth
{
    public static class AuthService
    {
        public static IServiceCollection AddAuthenticationContext(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataContext>();
            return services;
        }
    }
}
