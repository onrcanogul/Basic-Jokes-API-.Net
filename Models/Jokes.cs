using System.ComponentModel.DataAnnotations;

namespace JokesApi.Models
{
    public enum JokesTypes
    {
        kid,
        teen,
        adult
    }
    public class Jokes
    {
        [Key]
        public int JokeId { get; set; }
        public string JokeTitle { get; set; } = null!;
        public string JokeDescription { get; set; } = null!;
        public JokesTypes JokeType { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
