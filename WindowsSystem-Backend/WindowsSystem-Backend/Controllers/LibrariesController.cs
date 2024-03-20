using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.BL;
using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Models;

namespace WindowsSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public LibrariesController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET - /api/libraries/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLibreryDto>>> GetLibraries()
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var libraries = await _dbContext.Libraries
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .ToListAsync();

            List<GetLibreryDto> librariesDto = (
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

        // GET - /api/libraries/{id}/movies
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<MediaDto>>> GetLibraryMovies(int id)
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

            List<MediaDto> media = (
                from movie in movies
                select BlMovie.getMediaFromMovie(movie)
            ).ToList();

            return Ok(media);
        }

        // GET - /api/libraries/{id}/tvseries
        [HttpGet("{id}/tvseries")]
        public async Task<ActionResult<IEnumerable<MediaDto>>> GetLibraryTvSeries(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.TvSeries)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (library == null)
            {
                return NotFound();
            }

            var series = library.TvSeries;

            List<MediaDto> media = (
                from serie in series
                select BlTvSeries.getMediaFromTvSeries(serie)
            ).ToList();

            return Ok(media);
        }

        // GET - /api/libraries/search/name
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<GetLibreryDto>>> GetLibraryByTitle(string name)
        {
            // Get the libraries with a name matching the search term
            var libraries = await _dbContext.Libraries
                .Where(l => l.Name == null ? false : l.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (libraries.Count == 0)
            {
                return NotFound();
            }

            var librariesDto = (
                    from libary in libraries
                    select BlLibrary.getLibreryDTOs(libary, new List<Movie> { }, new List<TvSeries> { })
                ).ToList();

            return Ok(librariesDto);
        }

        // POST - /api/libraries
        [HttpPost]
        public async Task<ActionResult<GetLibreryDto>> CreateLibrary(CreateLibraryDto createLibraryDto)
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

            var libraryDto = BL.BlLibrary.getLibreryDTOs(library, new(), new());

            return CreatedAtAction(nameof(GetLibrary), new { id = library.Id }, libraryDto);
        }

        // POST - api/libraries/{librariyId}/movies
        [HttpPost("{libraryId}/movies")]
        public async Task<IActionResult> AddMovieToLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.Movies)
                .FirstOrDefaultAsync(i => i.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }

            List<Movie> movies = library.Movies;

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
                var movieDetails = await OmdbbApiService.GetMovieByIDAsync(imdbID);
                var movie = BL.BlJsonConversion.GetMovieFromJson(movieDetails);

                if (movie == null)
                {
                    return NotFound();
                }

                _dbContext.Movies.Add(movie);
                existingMovie = movie;
            }  
            
            library.Movies.Add(existingMovie);
            await _dbContext.SaveChangesAsync();

            return Ok(BlMovie.getMediaFromMovie(existingMovie));
        }

        // DELETE - api/libraries/{librariyId}/movies
        [HttpDelete("{libraryId}/movies")]
        public async Task<IActionResult> DeleteMovieFromLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.Movies)
                .FirstOrDefaultAsync(i => i.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }

            var movie = library.Movies.FirstOrDefault(i => i.ImdbID == imdbID);
            if (movie == null)
            {
                return NotFound();
            }

            library.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // POST - api/libraries/{librariyId}/movies
        [HttpPost("{libraryId}/tvseries")]
        public async Task<IActionResult> AddSeriesToLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.TvSeries)
                .FirstOrDefaultAsync(i => i.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }

            // Check if the movie already exists in the local database
            var existingSeries = await _dbContext.TvSeries.FirstOrDefaultAsync(movie => movie.ImdbID == imdbID);
            if (existingSeries == null)
            {
                // Movie doesn't exist in the local database, fetch it from the OMDb API
                var seriesDetails = await OmdbbApiService.GetSeriesByIDAsync(imdbID);
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

            return Ok(BlTvSeries.getMediaFromTvSeries(existingSeries));
        }

        [HttpDelete("{libraryId}/tvseries")]
        public async Task<IActionResult> DeleteTvSeriesFromLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries
                .Include(l => l.TvSeries)
                .FirstOrDefaultAsync(i => i.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }

            var series = library.TvSeries.FirstOrDefault(i => i.ImdbID == imdbID);
            if (series == null)
            {
                return NotFound();
            }

            library.TvSeries.Remove(series);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // PUT - api/libraries/{id}
        [HttpPut]
        public async Task<IActionResult> updateLibrary(int id, CreateLibraryDto libraryDto)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _dbContext.Libraries.FindAsync(id);
            if (library == null)
            {
                return NotFound();
            }

            library.Name = libraryDto.Name;
            library.Keywords = libraryDto.Keywords;

            await _dbContext.SaveChangesAsync();

            return Ok(libraryDto);
        }

        // DELETE - api/libraries/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteLibrary(int id)
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

            library.Movies.RemoveAll(l => true);
            library.TvSeries.RemoveAll(l => true);

            _dbContext.Libraries.Remove(library);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}