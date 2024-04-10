using WindowsSystem_Backend.DAL.Interfaces;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.DAL.Implementations
{
    public class WriteToDataBase : IWriteToDataBase
    {
        private readonly DataContext _dbContext;

        public WriteToDataBase(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Movies

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> UpdateMovieAsync(Movie movie)
        {
            _dbContext.Movies.Update(movie);
            await _dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
        }

        // TV Series

        public async Task<TvSeries> AddTvSerieAsync(TvSeries tvSerie)
        {
            _dbContext.TvSeries.Add(tvSerie);
            await _dbContext.SaveChangesAsync();
            return tvSerie;
        }

        public async Task<TvSeries> UpdateTvSerieAsync(TvSeries tvSerie)
        {
            _dbContext.TvSeries.Update(tvSerie);
            await _dbContext.SaveChangesAsync();
            return tvSerie;
        }

        public async Task DeleteTvSerieAsync(TvSeries tvSerie)
        {
            _dbContext.TvSeries.Remove(tvSerie);
            await _dbContext.SaveChangesAsync();
        }

        // Libraries

        public async Task<Library> AddLibraryAsync(Library library)
        {
            _dbContext.Libraries.Add(library);
            await _dbContext.SaveChangesAsync();
            return library;
        }

        public async Task<Library> UpdateLibraryAsync(Library library)
        {
            _dbContext.Libraries.Update(library);
            await _dbContext.SaveChangesAsync();
            return library;
        }

        public async Task<Library> UpdateLibraryAddMovieAsync(Library library, Movie movie)
        {
            library.Movies.Add(movie);
            _dbContext.Libraries.Update(library);
            await _dbContext.SaveChangesAsync();
            return library;
        }

        public async Task<Library> UpdateLibraryRemoveMovieAsync(Library library, Movie movie)
        {
            library.Movies.Remove(movie);
            _dbContext.Libraries.Update(library);
            await _dbContext.SaveChangesAsync();
            return library;
        }

        public async Task<Library> UpdateLibraryAddTvSerieAsync(Library library, TvSeries tvSerie)
        {
            library.TvSeries.Add(tvSerie);
            _dbContext.Libraries.Update(library);
            await _dbContext.SaveChangesAsync();
            return library;
        }

        public async Task<Library> UpdateLibraryRemoveTvSerieAsync(Library library, TvSeries tvSerie)
        {
            library.TvSeries.Remove(tvSerie);
            _dbContext.Libraries.Update(library);
            await _dbContext.SaveChangesAsync();
            return library;
        }

        public async Task DeleteLibraryAsync(Library library)
        {
            _dbContext.Libraries.Remove(library);
            await _dbContext.SaveChangesAsync();
        }
    }
}