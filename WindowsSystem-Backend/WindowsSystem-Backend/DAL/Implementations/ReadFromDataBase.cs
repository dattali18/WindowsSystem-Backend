using WindowsSystem_Backend.DAL.Interfaces;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.DAL;
using Microsoft.EntityFrameworkCore;

namespace WindowsSystem_Backend.DAL.Implementations
{
    public class ReadFromDataBase : IReadFromDataBase
    {
        private readonly DataContext _dbContext;

        public ReadFromDataBase(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Movies

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var movies = await _dbContext.Movies.ToListAsync();
            return movies;
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);
            return movie;
        }

        public async Task<Movie?> GetMovieByImdbIdAsync(string imdbID)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.ImdbID == imdbID);
            return movie;
        }

        // TV Series

        public async Task<IEnumerable<TvSeries>> GetTvSeriesAsync()
        {
            var tvSeries = await _dbContext.TvSeries.ToListAsync();
            return tvSeries;
        }

        public async Task<TvSeries?> GetTvSerieByIdAsync(int id)
        {
            var tvSerie = await _dbContext.TvSeries.FindAsync(id);
            return tvSerie;
        }

        public async Task<TvSeries?> GetTvSerieByImdbIdAsync(string imdbID)
        {
            var tvSerie = await _dbContext.TvSeries.FirstOrDefaultAsync(t => t.ImdbID == imdbID);
            return tvSerie;
        }

        // Libraries

        public async Task<IEnumerable<Library>> GetLibrariesAsync()
        {
            var libraries = await _dbContext.Libraries
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .ToListAsync();
            return libraries;
        }

        public async Task<IEnumerable<Library>> GetLibrariesWithMediasAsync()
        {
            var libraries = await _dbContext.Libraries
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .ToListAsync();
            return libraries;
        }

        public async Task<IEnumerable<Library>> GetLibrariesByNameAsync(string name)
        {
            var libraries = await _dbContext.Libraries
                .Where(l => l.Name == null ? false : l.Name.ToLower().Contains(name.ToLower()))
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .ToListAsync();
            return libraries;
        }


        public async Task<Library?> GetLibraryByIdAsync(int id)
        {
            var library = await _dbContext.Libraries.FindAsync(id);
            return library;
        }

        public async Task<Library?> GetLibraryByIdWithMediasAsync(int id)
        {
            var library = await _dbContext.Libraries
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .FirstOrDefaultAsync(l => l.Id == id);
            return library;
        }

        public async Task<Library?> GetLibraryByIdWithMoviesAsync(int id)
        {
            var library = await _dbContext.Libraries
                .Include(l => l.Movies)
                .FirstOrDefaultAsync(l => l.Id == id);
            return library;
        }

        public async Task<Library?> GetLibraryByIdWithTvSeriesAsync(int id)
        {
            var library = await _dbContext.Libraries
                .Include(l => l.TvSeries)
                .FirstOrDefaultAsync(l => l.Id == id);
            return library;
        }

        public async Task<IEnumerable<Library>> GetLibrariesByNameKeywordsAsync(string name, string keywords)
        {
            var libraries = await _dbContext.Libraries
                .Where(l => l.Name == null ? false : l.Name.ToLower().Contains(name.ToLower()))
                .Where(l => l.Keywords == null ? false : l.Keywords.ToLower().Contains(keywords.ToLower()))
                .Include(l => l.Movies)
                .Include(l => l.TvSeries)
                .ToListAsync();
            return libraries;
        }
    }
}
