using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.DAL.Interfaces
{
    public interface IReadFromDataBase
    {
        // Movies

        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<Movie?> GetMovieByImdbIdAsync(string imdbID);
        Task<Movie?> GetMovieByIdAsync(int id);

        // TV Series

        Task<IEnumerable<TvSeries>> GetTvSeriesAsync();
        Task<TvSeries?> GetTvSerieByImdbIdAsync(string imdbID);
        Task<TvSeries?> GetTvSerieByIdAsync(int id);

        // Libraries

        Task<IEnumerable<Library>> GetLibrariesAsync();
        Task<IEnumerable<Library>> GetLibrariesWithMediasAsync();
        Task<IEnumerable<Library>> GetLibrariesByNameAsync(string name);
        Task<IEnumerable<Library>> GetLibrariesByNameKeywordsAsync(string name, string keywords);
        Task<Library?> GetLibraryByIdAsync(int id);
        Task<Library?> GetLibraryByIdWithMediasAsync(int id);
        Task<Library?> GetLibraryByIdWithMoviesAsync(int id);
        Task<Library?> GetLibraryByIdWithTvSeriesAsync(int id);

    }
}