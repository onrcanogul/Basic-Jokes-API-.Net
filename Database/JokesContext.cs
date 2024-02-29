using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using JokesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JokesApi.Database
{
    public class JokesContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public JokesContext(DbContextOptions<JokesContext> options) : base(options)
        {

        }
        public DbSet<Jokes> Jokes { get; set; }
    }
}
