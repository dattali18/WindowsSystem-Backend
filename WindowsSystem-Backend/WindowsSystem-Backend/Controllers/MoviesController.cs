using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.BL;

namespace WindowsSystem_Backend.Controllers
{
    /// <summary>
    /// Controller for managing movie-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        private readonly Bl bl = Factory.GetBL();

        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesController"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public MoviesController(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        // GET - /api/movies

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>A list of movie DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMovieDto>>> GetMovies()
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            // TODO: GetAllMoviesAsync()
            var movies = await _dbContext.Movies.ToListAsync();
            // TODO: move that into a function in the BL
            var moviesDto = (
                from movie in movies
                select bl.BlMovie.GetMovieDtoFromMovie(movie)
                ).ToList();

            return Ok(moviesDto);
        }

        // GET - /api/movies/{id}

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie.</param>
        /// <returns>The movie DTO.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMovieDto>> GetMovie(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }
            // TODO: GetMovieByIdAsync(id)
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = bl.BlMovie.GetMovieDtoFromMovie(movie);
            return Ok(movieDto);
        }

        // GET - /api/movies/search/{imdbID}

        /// <summary>
        /// Retrieves a movie by its IMDb ID.
        /// </summary>
        /// <param name="imdbID">The IMDb ID of the movie.</param>
        /// <returns>The movie DTO.</returns>
        [HttpGet("search/{imdbID}")]
        public async Task<ActionResult<GetMovieDto>> GetMovie(string imdbID)
        {
            var movie = await bl.BlMovie.GetMovieByImdbID(imdbID);

            if (movie == null)
            {
                return BadRequest();
            }

            var movieDto = bl.BlMovie.GetMovieDtoFromMovie(movie);
            return Ok(movieDto);
        }

        // GET - /api/movies/?s=[SEARCH_TERM]&y=[YEAR]

        /// <summary>
        /// Retrieves movies by search term and optional year.
        /// </summary>
        /// <param name="s">The search term.</param>
        /// <param name="y">The year (optional).</param>
        /// <returns>A list of media DTOs.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MediaDto>>> GetMoviesBySearch(string s, int? y = null)
        {
            var movies = await bl.BlMovie.GetMoviesBySearch(s, y);
            return Ok(movies);
        }

        // POST - api/movies

        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="imdbID">The IMDb ID of the movie.</param>
        /// <returns>The added movie DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<GetMovieDto>> PostMovie(string imdbID)
        {
            // check if the movie is already in the DB
            // TODO: GetMovieByImdbIDAsync(imdbID)
            var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.ImdbID == imdbID);
            if (existingMovie != null)
            {
                return Ok(bl.BlMovie.GetMovieDtoFromMovie(existingMovie));
            }

            var movie = await bl.BlMovie.GetMovieByImdbID(imdbID);

            if (movie == null)
            {
                return BadRequest();
            }

            // TODO: AddMovieAsync(movie)
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            var movieDto = bl.BlMovie.GetMovieDtoFromMovie(movie);
            return Ok(movieDto);
        }
    }
}
