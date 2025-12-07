using System.ComponentModel.DataAnnotations;

namespace JokesApp.Models
{
    public class Joke
    {
        // Primary Key: Every model needs a unique ID.
        public int Id { get; set; }

        // The setup part of the joke.
        // [Required] is a Data Annotation that tells the database this field cannot be null.
        [Required]
        [Display(Name = "Joke Setup")] // This is what shows up on forms/labels.
        public string JokeQuestion { get; set; } = string.Empty;

        // The punchline.
        [Required]
        [Display(Name = "Punch Line")]
        public string JokeAnswer { get; set; } = string.Empty;

        public string? OwnerID { get; set; }

        public int Likes { get; set; } = 0; // Initialize to zero
        // Constructor: A simple way to initialize the object if needed.
        public Joke(){}
    }
}