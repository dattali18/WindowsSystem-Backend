using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.BL;
using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Models;
using WindowsSystem_Backend.BL.BO;

namespace WindowsSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public LibraryController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET - /api/libraries/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLibreryDTO>>> GetLibraries()
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var libraries = await _dbContext.Libraries
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .ToListAsync();

            List<GetLibreryDTO> librariesDto = (
                    from library in libraries
                    select BlLibrary.getLibreryDTOs(library, library.Movies, library.TvSeries)
                ).ToList();

            return Ok(librariesDto);
        }

        // GET - /api/libraries/
        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> GetLibrary(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            List<Movie> movies = library.Movies;
            List<TvSeries> tvSeries = library.TvSeries;


            return Ok(BlLibrary.getLibreryDTOs(library, movies, tvSeries));
        }

        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<Media>>> GetLibraryMovies(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.Movies)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (library == null)
            {
                return NotFound();
            }

            var movies = library.Movies;

            List<Media> media = (
                from movie in movies
                select BlMovie.getMediaFromMovie(movie)
            ).ToList();

            return Ok(media);
        }

        // GET - /api/libraries/
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraryByTitle(string name)
        {
            // Get the libraries with a name matching the search term
            var libraries = await _dbContext.Libraries
                .Where(l => l.Name == null ? false : l.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (libraries.Count == 0)
            {
                return NotFound();
            }

            return Ok(libraries);
        }

        [HttpPost]
        public async Task<ActionResult<Library>> CreateLibrary(CreateLibraryDTO createLibraryDto)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = new Library
            {
                Name = createLibraryDto.Name,
                Keywords = createLibraryDto.Keywords
            };

            _dbContext.Libraries.Add(library);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibrary), new { id = library.Id }, library);
        }

        [HttpPost("{libraryId}/movies")]
        public async Task<IActionResult> AddMovieToLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries.FindAsync(libraryId);
            if (library == null)
            {
                return NotFound();
            }

            List<Movie> movies = library!.Movies;

            // check if the movie is allready in the library
            if (movies.FirstOrDefault(movie => movie.ImdbID == imdbID) != null)
            {
                return Ok("The movie was allready in the library");
            }

            // Check if the movie already exists in the local database
            var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.ImdbID == imdbID);
            if (existingMovie == null)
            {
                // Movie doesn't exist in the local database, fetch it from the OMDb API
                var movieDetails = await OMDbApiService.GetMovieByIDAsync(imdbID);
                var movie = BL.BlJsonConversion.GetMovieFromJson(movieDetails);

                if (movie == null)
                {
                    return NotFound();
                }

                _dbContext.Movies.Add(movie);
                existingMovie = movie;
            }  
            
            library!.Movies.Add(existingMovie);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{libraryId}/tvseries")]
        public async Task<IActionResult> AddSeriesToLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries.FindAsync(libraryId);
            if (library == null)
            {
                return NotFound();
            }

            // Check if the movie already exists in the local database
            var existingSeries = await _dbContext.TvSeries.FirstOrDefaultAsync(movie => movie.ImdbID == imdbID);
            if (existingSeries == null)
            {
                // Movie doesn't exist in the local database, fetch it from the OMDb API
                var seriesDetails = await OMDbApiService.GetSeriesByIDAsync(imdbID);
                var series = BL.BlJsonConversion.GetTvSeriesFromJson(seriesDetails);

                if (series == null)
                {
                    return NotFound();
                }

                _dbContext.TvSeries.Add(series);
                existingSeries = series;
            }

            library.TvSeries.Add(existingSeries);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}