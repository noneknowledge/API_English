using EnglishAPI.Data;
using EnglishAPI.Services;
using EnglishAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnglishAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EnglishWebContext _ctx;
        private readonly IConfiguration _config;

        public UserController(EnglishWebContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }
        [HttpGet("{uid}")]
        public async Task<IActionResult> GetUser(int uid)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(a=>a.UserId == uid);
            return Ok(_ctx.Users);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> getProfile()
        {
            try
            {
                var currentId = User.FindFirst("Id").Value;
                var profile = await _ctx.Users.FirstOrDefaultAsync(a => a.UserId.ToString() == currentId);
                if (profile == null)
                {
                    return BadRequest("Please try again");
                }
                else
                {
                    return Ok(profile);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User model)
        {
            var validName = await _ctx.Users.FirstOrDefaultAsync(a=>a.UserName == model.UserName);
            if (validName!=null)
            {
                return BadRequest();
            }
            var defaultA = await _ctx.Avatars.FirstOrDefaultAsync();
            model.AvatarId = defaultA.AvatarId;
            model.RandomKey = Mytool.GetRandom();
            model.Password = model.Password.ToSHA512Hash(model.RandomKey);
            
            _ctx.Add(model);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
           
            var loginUser = await _ctx.Users.Include(a=>a.Avatar).FirstOrDefaultAsync(a => a.UserName == vm.UserName);
            if(loginUser != null && loginUser.Password == vm.Password.ToSHA512Hash(loginUser.RandomKey))
            {
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", loginUser.UserId.ToString()),
                new Claim("Username", loginUser.UserName ),
                new Claim("Fullname", loginUser.FullName )
            }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                var tokenResponse = new TokenResponse() { avatarImage = loginUser.Avatar.Image, token = stringToken, userName =loginUser.UserName };
                return Ok(tokenResponse);
                //return Ok("login success");
            }
            return BadRequest("Sai tai khoan hoac mat khau");
        }
    }
}
