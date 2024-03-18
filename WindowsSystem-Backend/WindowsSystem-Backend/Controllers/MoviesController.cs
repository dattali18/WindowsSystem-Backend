using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.Models;

namespace WindowsSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public MoviesController(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        // GET - /api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if(_dbContext == null)
            {
                return NotFound();
            }

            var movies = await _dbContext.Movies.ToListAsync();

            return Ok(movies);
        }

        // GET - /api/movies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // GET - /api/movies/?s=[SEARCH_TERM]&y=[YEAR]
        [HttpGet("search")]
        public async Task<ActionResult<string>> GetMoviesBySearch(string s, int? y = null)
        {
            var str = await OMDbApiService.GetMoviesBySearchAsync(s, y);

            return Ok(str);
        }
    }
}
