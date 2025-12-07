using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesApp.Data;
using JokesApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JokesApp.Controllers
{
    [Authorize]
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {

            var userLikes = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                userLikes = await _context.UserJokeVotes
                    .Where(v => v.UserId == userId)
                    .Select(v => v.JokeId)
                    .ToListAsync();
            }
            ViewBag.UserLikes = userLikes;
            // 1. Create a query targeting the Joke table
            var jokesQuery = from j in _context.Joke
                             select j;

            // 2. If the user typed something, add a "WHERE" filter to that query
            if (!string.IsNullOrEmpty(searchString))
            {
                // This looks for the string in both the question and the answer
                jokesQuery = jokesQuery.Where(s => s.JokeQuestion.Contains(searchString)
                                                || s.JokeAnswer.Contains(searchString));
            }

            // 3. Execute the query and send results to the view
            return View(await jokesQuery.ToListAsync());
        }

        // GET: Jokes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            if (ModelState.IsValid)
            {
                // NEW: Get the ID of the currently logged-in user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                joke.OwnerID = userId; // Assign the ID to the joke
                _context.Add(joke);
                await _context.SaveChangesAsync();
                TempData["success"] = "Joke added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            // --- SECURITY CHECK START ---
            // Check if the current user's ID matches the joke's OwnerID
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (joke.OwnerID != currentUserId)
            {
                // If they don't match, return an 'Unauthorized' error page
                return Unauthorized();
            }
            // --- SECURITY CHECK END ---

            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (joke.OwnerID != currentUserId)
            {
                return Unauthorized();
            }

            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.Joke.FindAsync(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (joke == null || joke.OwnerID != currentUserId)
            {
                return Unauthorized();
            }

            if (joke != null)
            {
                _context.Joke.Remove(joke);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
            return _context.Joke.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Check if this user has already voted for THIS joke
            var existingVote = await _context.UserJokeVotes
                .FirstOrDefaultAsync(v => v.UserId == currentUserId && v.JokeId == id);

            if (existingVote == null)
            {
                // 2. No vote found? Let's add one!
                var joke = await _context.Joke.FindAsync(id);
                if (joke != null)
                {
                    joke.Likes++;

                    // 3. Record the vote so they can't do it again
                    _context.UserJokeVotes.Add(new UserJokeVote
                    {
                        UserId = currentUserId,
                        JokeId = id
                    });

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                // Optional: Implement "Unlike" here by removing the vote and decreasing the count
                TempData["error"] = "You've already liked this joke!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
