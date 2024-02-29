using System.Security.Claims;
using System.Security.Principal;
using JokesApi.Database;
using JokesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace JokesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JokesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JokesContext _jokesContext;
        public JokesController(JokesContext jokesContext, UserManager<AppUser> userManager)
        {
            _jokesContext = jokesContext;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetJokes()
        {
            return Ok(await _jokesContext.Jokes.ToListAsync());
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJoke(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var joke = await _jokesContext.Jokes.FirstOrDefaultAsync(i => i.JokeId == id);
            if (joke == null)
            {
                return NotFound();
            }
            return Ok(joke);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CreateJoke(CreateJokeDTO model)
        {
            if (ModelState.IsValid)
            {
                var joke = new Jokes
                {
                    JokeTitle = model.JokeTitle,
                    JokeDescription = model.JokeDescription,
                    JokeType = model.JokeType,
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "")
                };
                _jokesContext.Add(joke);
                await _jokesContext.SaveChangesAsync();
                return CreatedAtAction("GetJoke", new { id = joke.JokeId }, joke);
            }
            return BadRequest();

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJoke(int? id, UpdateJokeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.JokeId)
            {
                return BadRequest();
            }
            var joke = await _jokesContext.Jokes.FirstOrDefaultAsync(j => j.JokeId == model.JokeId);
            if (joke == null)
            {
                return NotFound();
            }
            joke.JokeDescription = model.Description;
            joke.JokeTitle = model.Title;
            joke.JokeType = model.JokeType;
            _jokesContext.Update(joke);
            await _jokesContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJoke(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var joke = await _jokesContext.Jokes.FirstOrDefaultAsync(j => j.JokeId == id);
            if (joke == null)
            {
                return NotFound();
            }
            _jokesContext.Jokes.Remove(joke);
            await _jokesContext.SaveChangesAsync();
            return Ok();
        }


    }
}
