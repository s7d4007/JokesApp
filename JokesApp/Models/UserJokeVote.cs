namespace JokesApp.Models
{
    public class UserJokeVote
    {
        public int Id { get; set; }

        // The ID of the User who voted
        public string UserId { get; set; } = string.Empty;

        // The ID of the Joke they voted on
        public int JokeId { get; set; }
    }
}