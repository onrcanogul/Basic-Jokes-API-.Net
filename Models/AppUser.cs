using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace JokesApi.Models
{
    public class AppUser : IdentityUser<int>
    {
        public List<Jokes> Jokes { get; set; } = new List<Jokes>();
    }
}
