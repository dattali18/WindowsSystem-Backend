using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Models;
using WindowsSystem_Backend.BL.BO;

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
        public async Task<ActionResult<IEnumerable<DO.Movie>>> GetMovies()
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
        public async Task<ActionResult<DO.Movie>> GetMovie(int id)
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

        [HttpGet("search/{imdbID}")]
        public async Task<ActionResult<DO.Movie>> GetMovie(string imdbID)
        {
            var str = await OMDbApiService.GetMovieByIDAsync(imdbID);
            var movie = BL.BL.GetMovieFromJson(str);

            if (movie == null)
            {
                return BadRequest();
            }

            return Ok(movie);
        }

        // GET - /api/movies/?s=[SEARCH_TERM]&y=[YEAR]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Media>>> GetMoviesBySearch(string s, int? y = null)
        {
            var str = await OMDbApiService.GetMoviesBySearchAsync(s, y);
            var movies = BL.BL.GetMovieObjFromJson(str);
            return Ok(movies);
        }

        // POST - api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(string id)
        {
            var str =  await OMDbApiService.GetMovieByIDAsync(id);
            var movie = BL.BL.GetMovieFromJson(str);

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
