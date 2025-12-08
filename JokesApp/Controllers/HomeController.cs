using System.Diagnostics;
using JokesApp.Data; // Ensure this matches your namespace
using JokesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JokesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Logic to get a random joke from the database
            var allJokes = await _context.Joke.ToListAsync();

            if (allJokes.Any())
            {
                var random = new Random();
                int index = random.Next(allJokes.Count);
                var randomJoke = allJokes[index];

                // Pass the joke to the View using ViewBag
                ViewBag.JokeOfTheDay = randomJoke.JokeQuestion;
                ViewBag.JokeOfTheDayAnswer = randomJoke.JokeAnswer;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}