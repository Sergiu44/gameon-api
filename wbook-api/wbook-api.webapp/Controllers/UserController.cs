using Infrastructure.Entities;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wbook_api.WebApp.Code.Utils;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            var id = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            var users = await _userService.GetUsers(id);
            return Ok(users);
        }

        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var id = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            var user = await _userService.GetCurrentUser(id);
            return Ok(user);
        }

        [HttpPut("put/password")]
        public IActionResult PutPasswordUser([FromBody] UserPasswordModel user)
        {
            var id = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _userService.PutChangePasswordUser(id, user);
            return Ok();
        }

        [HttpPut("put")]
        public IActionResult PutCurrentUSer([FromBody] UserPutModel user)
        {
            var id = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _userService.PutCurrentUser(id, user);
            return Ok();
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            _userService.DeleteUser(id);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            _userService.RegisterNewUser(model);

            var user = await _userService.Login(new LoginModel()
            {
                Password = model.Password,
                Email = model.Email,
                InvalidCredentials = false,
            });

            var token = LogIn(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                firstName = user.FirstName,
                lastName = user.LastName,
                isAdmin = user.IsAdmin
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Login(model);

            if (!user.IsAuthenticated)
            {
                model.InvalidCredentials = true;
                return Unauthorized();
            }

            var token = LogIn(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                firstName = user.FirstName,
                lastName = user.LastName,
                isAdmin = user.IsAdmin
            });
        }



        private JwtSecurityToken LogIn(UserModel userDto)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", userDto.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{userDto.LastName}"),
                new Claim(ClaimTypes.GivenName, $"{userDto.FirstName}"),
                new Claim("IsAuthenticated", $"{userDto.IsAuthenticated}"),
                new Claim("IsAdmin", $"{userDto.IsAdmin}"),
            };

            var token = GetToken(claims);

            return token;
        }


        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                );

            return token;
        }
    }
}
