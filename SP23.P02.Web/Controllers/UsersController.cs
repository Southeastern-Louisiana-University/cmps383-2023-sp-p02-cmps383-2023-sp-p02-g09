using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SP23.P02.Web.Features.Auth;

namespace SP23.P02.Web.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;

            MapperConfigurationExpression cfg = new();

            cfg.CreateMap<IdentityUser, UserDto>();
            cfg.CreateMap<UserDto, IdentityUser>();
            cfg.CreateMap<UserDto, User>();
            cfg.CreateMap<CreateUserDto, User>();
            cfg.CreateMap<CreateUserDto, IdentityUser>();
            cfg.CreateMap<CreateUserDto, UserDto>();
            cfg.CreateMap<LoginDto, UserDto>();

            MapperConfiguration config = new(cfg);
            mapper = config.CreateMapper();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserDto createUserDto)
        {

            var currentUser = await userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (!currentUser.Roles.Any(role => role.Equals("Admin")))
            {
                return Unauthorized();
            }

            if (createUserDto.Roles == null || !createUserDto.Roles.Any())
            {
                return BadRequest();
            }

            var roles = roleManager.Roles.Where(r => createUserDto.Roles.Contains(r.Name)).ToList();
            if (roles.Count != createUserDto.Roles.Count())
            {
                return BadRequest();
            }

            if (await userManager.FindByNameAsync(createUserDto.UserName) != null)
            {
                return BadRequest();
            }

            var passwordValidator = new PasswordValidator<User>();
            var passwordValidationResult = await passwordValidator.ValidateAsync(userManager, null!, createUserDto.Password);
            if (!passwordValidationResult.Succeeded)
            {
                return BadRequest();
            }

            var user = mapper.Map<User>(createUserDto);
            var result = await userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            foreach (var role in roles)
            {
                result = await userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => r.Name).ToArray()
            };
            return Ok(userDto);
        }
    }
}
