using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JokesApi.Database;
using JokesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JokesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly JokesContext _jokesContext;
        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, JokesContext jokesContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _jokesContext = jokesContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userManager.Users.Select(u => new { u.Id, u.UserName }).ToListAsync());
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                return BadRequest(result.Errors);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(new { message = "There is no user for that email" });
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    return Ok(new { token = GenerateToken(user) });
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        private object GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJokeWithUserId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jokes = await _jokesContext.Jokes.Where(u => u.UserId == id).ToListAsync();
            if (jokes == null)
            {
                return NotFound();
            }
            return Ok(jokes);
        }
    }
}