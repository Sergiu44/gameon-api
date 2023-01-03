using Infrastructure.Common.DTOs;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            _userService.RegisterNewUser(model);

            var user = await _userService.Login(new LoginModel()
            {
                Password = model.Password,
                Email = model.Email,
                AreCredentialsInvalid = false,
                Disabled = false
            });

            var token = LogIn(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                firstName = user.FirstName,
                lastName = user.LastName
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Login(model);

            if (!user.Authenticated)
            {
                model.AreCredentialsInvalid = true;
                return Unauthorized();
            }

            var token = LogIn(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                firstName = user.FirstName,
                lastName = user.LastName
            });
        }



        private JwtSecurityToken LogIn(CurrentUserDto userDto)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", userDto.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{userDto.LastName}"),
                new Claim(ClaimTypes.GivenName, $"{userDto.FirstName}"),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim("Disabled", $"{userDto.Disabled}")
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
