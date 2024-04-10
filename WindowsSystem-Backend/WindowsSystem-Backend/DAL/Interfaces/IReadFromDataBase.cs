using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.DAL.Interfaces
{
    public interface IReadFromDataBase
    {
        // Movies

        Task<IEnumerable<Movie>> GetMoviesAsync();

        Task<Movie?> GetMovieByImdbIDAsync(string imdbID);

        Task<Movie?> GetMovieByIdAsync(int id);

        // TV Series

        Task<IEnumerable<TvSeries>> GetTvSeriesAsync();

        Task<TvSeries?> GetTvSerieByImdbIDAsync(string imdbID);

        Task<TvSeries?> GetTvSerieByIdAsync(int id);

        // Libraries

        Task<IEnumerable<Library>> GetLibrariesAsync();
        Task<IEnumerable<Library>> GetLibrariesWithMediasAsync();

        Task<Library?> GetLibraryByIdAsync(int id);

        Task<Library?> GetLibraryByIdWithMediasAsync(int id);

        Task<Library?> GetLibraryByIdWithMoviesAsync(int id);

        Task<Library?> GetLibraryByIdWithTvSeriesAsync(int id);

        // with medias
        Task<IEnumerable<Library>> GetLibrariesByNameAsync(string name);
    }
}