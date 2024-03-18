using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Models;
using WindowsSystem_Backend.BL.DTO;

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

        // GET - /api/movies/search/{imdbID}
        [HttpGet("search/{imdbID}")]
        public async Task<ActionResult<Movie>> GetMovie(string imdbID)
        {
            var str = await OMDbApiService.GetMovieByIDAsync(imdbID);
            var movie = BL.BlJsonConversion.GetMovieFromJson(str);

            if (movie == null)
            {
                return BadRequest();
            }

            return Ok(movie);
        }

        // GET - /api/movies/?s=[SEARCH_TERM]&y=[YEAR]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MediaDto>>> GetMoviesBySearch(string s, int? y = null)
        {
            var str = await OMDbApiService.GetMoviesBySearchAsync(s, y);
            var movies = BL.BlJsonConversion.GetMovieObjFromJson(str);
            return Ok(movies);
        }

        // POST - api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(string imdbID)
        {
            // check if the movie is allready in the DB
            var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.ImdbID == imdbID);
            if (existingMovie != null)
            {
                return Ok();
            }

            var str =  await OMDbApiService.GetMovieByIDAsync(imdbID);
            var movie = BL.BlJsonConversion.GetMovieFromJson(str);

            if (movie == null)
            {
                return BadRequest();
            }

            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            return Ok(movie);
        }
    }
}
