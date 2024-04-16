using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WindowsSystem_Backend.BL;
using WindowsSystem_Backend.BL.DTO;

using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DAL.Implementations;
using WindowsSystem_Backend.DAL.Interfaces;

using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly Bl bl = Factory.GetBL();

        private readonly IReadFromDataBase _readFromDataBase;

        private readonly IWriteToDataBase _writeToDataBase;

        public LibrariesController(DataContext dbContext)
        {
            _dbContext = dbContext;

            _readFromDataBase = new ReadFromDataBase(_dbContext);
            _writeToDataBase = new WriteToDataBase(_dbContext);
        }

        // GET - /api/libraries/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLibraryDto>>> GetLibraries()
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var libraries = await _readFromDataBase.GetLibrariesAsync();

            List<GetLibraryDto> librariesDto = (
                    from library in libraries
                    select bl.BlLibrary.GetLibraryDTOs(library, library.Movies, library.TvSeries)
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

            var library = await _readFromDataBase.GetLibraryByIdWithMediasAsync(id);

            if (library == null)
            {
                return NotFound();
            }

            List<Movie> movies = library.Movies;
            List<TvSeries> tvSeries = library.TvSeries;

            var libraryDto = bl.BlLibrary.GetLibraryDTOs(library, movies, tvSeries);
            return Ok(libraryDto);
        }

        // GET - /api/libraries/{id}/movies
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<MediaDto>>> GetLibraryMovies(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _readFromDataBase.GetLibraryByIdWithMoviesAsync(id);

            if (library == null)
            {
                return NotFound();
            }

            var movies = library.Movies;

            List<MediaDto> media = (
                from movie in movies
                select bl.BlMovie.GetMediaFromMovie(movie)
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

            var library = await _readFromDataBase.GetLibraryByIdWithTvSeriesAsync(id);

            if (library == null)
            {
                return NotFound();
            }

            var series = library.TvSeries;

            List<MediaDto> media = (
                from serie in series
                select bl.BlTvSeries.GetMediaFromTvSeries(serie)
            ).ToList();

            return Ok(media);
        }

        // GET - /api/libraries/search/name
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<GetLibraryDto>>> GetLibraryByTitle(string name)
        {
            // Get the libraries with a name matching the search term
            var libraries = await _readFromDataBase.GetLibrariesByNameAsync(name);
            libraries = libraries.ToList();

            var librariesDto = (
                    from library in libraries
                    select bl.BlLibrary.GetLibraryDTOs(library, new List<Movie> { }, new List<TvSeries> { })
                ).ToList();

            return Ok(librariesDto);
        }

        // GET - /api/libraries/search/?name=<name>&keywords=<keywords>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetLibraryDto>>> GetLibraryByTitleAndKeywords(string name, string keywords)
        {
            // Get the libraries with a name matching the search term
            var libraries = await _readFromDataBase.GetLibrariesByNameKeywordsAsync(name, keywords);
            libraries = libraries.ToList();

            var librariesDto = (
                    from library in libraries
                    select bl.BlLibrary.GetLibraryDTOs(library, new List<Movie> { }, new List<TvSeries> { })
                ).ToList();

            return Ok(librariesDto);
        }

        // POST - /api/libraries
        [HttpPost]
        public async Task<ActionResult<GetLibraryDto>> CreateLibrary(CreateLibraryDto createLibraryDto)
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

            await _writeToDataBase.AddLibraryAsync(library);

            var libraryDto = bl.BlLibrary.GetLibraryDTOs(library, new List<Movie> { }, new List<TvSeries> { });

            return Ok(libraryDto);
        }

        // POST - api/libraries/{libraryID}/movies
        [HttpPost("{libraryId}/movies")]
        public async Task<IActionResult> AddMovieToLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _readFromDataBase.GetLibraryByIdWithMoviesAsync(libraryId);

            if (library == null)
            {
                return NotFound();
            }

            List<Movie> movies = library.Movies;
            // check if the movie is already in the library
            var movie = movies.FirstOrDefault(movie => movie.ImdbID == imdbID);

            if (movie != null)
            {
                var mediaDto = bl.BlMovie.GetMediaFromMovie(movie);
                return Ok(mediaDto);
            }

            // Check if the movie already exists in the local database
            var existingMovie = await _readFromDataBase.GetMovieByImdbIdAsync(imdbID);

            if (existingMovie == null)
            {
                // Movie doesn't exist in the local database, fetch it from the OMDb API
                var movieFromService = await bl.BlMovie.GetMovieByImdbID(imdbID);

                if (movieFromService == null)
                {
                    return NotFound();
                }

                await _writeToDataBase.AddMovieAsync(movieFromService);
                existingMovie = movieFromService;
            }  
            
            await _writeToDataBase.UpdateLibraryAddMovieAsync(library, existingMovie);

            var media = bl.BlMovie.GetMediaFromMovie(existingMovie);
            return Ok(media);
        }

        // DELETE - api/libraries/{libraryId}/movies
        [HttpDelete("{libraryId}/movies")]
        public async Task<IActionResult> DeleteMovieFromLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _readFromDataBase.GetLibraryByIdWithMoviesAsync(libraryId);

            if (library == null)
            {
                return NotFound();
            }

            // Check if the movie is in the library
            var movie = library.Movies.FirstOrDefault(i => i.ImdbID == imdbID);
            if (movie == null)
            {
                return NotFound();
            }

            await _writeToDataBase.UpdateLibraryRemoveMovieAsync(library, movie);

            return NoContent();
        }

        // POST - api/libraries/{libraryId}/movies
        [HttpPost("{libraryId}/tvseries")]
        public async Task<IActionResult> AddSeriesToLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _readFromDataBase.GetLibraryByIdWithTvSeriesAsync(libraryId);

            if (library == null)
            {
                return NotFound();
            }

            var existingSeries = await _readFromDataBase.GetTvSerieByImdbIdAsync(imdbID);

            if (existingSeries == null)
            {
                // Movie doesn't exist in the local database, fetch it from the OMDb API
                var series = await bl.BlTvSeries.GetTvSeriesByImdbID(imdbID);

                if (series == null)
                {
                    return NotFound();
                }

                await _writeToDataBase.AddTvSerieAsync(series);
                existingSeries = series;
            }

            await _writeToDataBase.UpdateLibraryAddTvSerieAsync(library, existingSeries);

            var seriesDto = bl.BlTvSeries.GetMediaFromTvSeries(existingSeries);
            return Ok(seriesDto);
        }

        [HttpDelete("{libraryId}/tvseries")]
        public async Task<IActionResult> DeleteTvSeriesFromLibrary(int libraryId, string imdbID)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _readFromDataBase.GetLibraryByIdWithTvSeriesAsync(libraryId);

            if (library == null)
            {
                return NotFound();
            }

            var series = library.TvSeries.FirstOrDefault(i => i.ImdbID == imdbID);

            if (series == null)
            {
                return NotFound();
            }

            await _writeToDataBase.UpdateLibraryRemoveTvSerieAsync(library, series);

            return NoContent();
        }

        // PUT - api/libraries/{id}
        [HttpPut]
        public async Task<IActionResult> UpdateLibrary(int id, CreateLibraryDto libraryDto)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var library = await _readFromDataBase.GetLibraryByIdAsync(id);

            if (library == null)
            {
                return NotFound();
            }

            library.Name = libraryDto.Name;
            library.Keywords = libraryDto.Keywords;

            await _writeToDataBase.UpdateLibraryAsync(library);

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

            var library = await _readFromDataBase.GetLibraryByIdWithMediasAsync(id);
                    
            if (library == null)
            {
                return NotFound();
            }

            await _writeToDataBase.DeleteLibraryAsync(library);

            return NoContent();
        }
    }
}