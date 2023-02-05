using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SP23.P02.Web.Features.Auth;

namespace SP23.P02.Web.Controllers
{
    [ApiController]
    [Route("/api/authentication")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

            MapperConfigurationExpression cfg = new();

            cfg.CreateMap<IdentityUser, UserDto>();
            cfg.CreateMap<IdentityUser, User>();
            cfg.CreateMap<UserDto, IdentityUser>();
            cfg.CreateMap<UserDto, User>();
            cfg.CreateMap<CreateUserDto, User>();
            cfg.CreateMap<CreateUserDto, IdentityUser>();
            cfg.CreateMap<CreateUserDto, UserDto>();
            cfg.CreateMap<LoginDto, UserDto>();

            MapperConfiguration config = new(cfg);
            mapper = config.CreateMapper();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var identityUser = await userManager.FindByNameAsync(loginDto.UserName);
            
            if (identityUser == null)
            {
                return BadRequest(new { message = "Invalid username." });
            }
            var user = mapper.Map<User>(identityUser);
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            var roles = await userManager.GetRolesAsync(identityUser);
            var userDto = mapper.Map<UserDto>(loginDto);
            userDto.Roles = roles.ToArray();
            return Ok(userDto);
        }
        [HttpGet("/me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);
            var userDto = mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToArray();
            return Ok(userDto);
        }
        [HttpPost("/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
