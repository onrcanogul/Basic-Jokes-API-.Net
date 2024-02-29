namespace JokesApi.Models
{
    public class UpdateJokeDTO
    {
        public int JokeId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public JokesTypes JokeType { get; set; }
    }
}