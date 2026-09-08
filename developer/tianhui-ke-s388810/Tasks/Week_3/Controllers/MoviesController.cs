using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheaterAdminWebApi.Data;
using TheaterAdminWebApi.Models;

namespace TheaterAdminWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            // Join in the category name manually since Category is [JsonIgnore]d on the
            // Movie model (avoids the client having to make a second request per movie).
            var movies = await _context.Movies
                .Include(m => m.Category)
                .Select(m => new Movie
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    ContactEmail = m.ContactEmail,
                    Language = m.Language,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category!.Name
                })
                .ToListAsync();

            return movies;
        }

        // GET: api/movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Category)
                .Where(m => m.Id == id)
                .Select(m => new Movie
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    ContactEmail = m.ContactEmail,
                    Language = m.Language,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category!.Name
                })
                .FirstOrDefaultAsync();

            if (movie == null) return NotFound();
            return movie;
        }

        // POST: api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Confirm the category actually exists before saving — the [Required] attribute
            // only checks CategoryId is non-zero, not that it references a real row.
            bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == movie.CategoryId);
            if (!categoryExists) return BadRequest(new { error = "Selected category does not exist." });

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id) return BadRequest("Id in URL does not match Id in body.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == movie.CategoryId);
            if (!categoryExists) return BadRequest(new { error = "Selected category does not exist." });

            _context.Entry(movie).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Movies.AnyAsync(m => m.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
