namespace JokesApi.Models
{
    public class CreateJokeDTO
    {
        public string JokeTitle { get; set; } = null!;
        public string JokeDescription { get; set; } = null!;
        public JokesTypes JokeType { get; set; }
    }
}