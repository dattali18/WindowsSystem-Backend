using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.DAL.Interfaces
{
    public interface IWriteToDataBase
    {
        // Movies

        Task<Movie> AddMovieAsync(Movie movie);

        Task<Movie> UpdateMovieAsync(Movie movie);

        Task DeleteMovieAsync(int id);

        // TV Series

        Task<TvSeries> AddTvSerieAsync(TvSeries tvSerie);

        Task<TvSeries> UpdateTvSerieAsync(TvSeries tvSerie);

        Task DeleteTvSerieAsync(int id);

        // Libraries

        Task<Library> AddLibraryAsync(Library library);

        Task<Library> UpdateLibraryAsync(Library library);

        Task<Library> UpdateLibraryAddMovieAsync(Library library, Movie movie);

        Task<Library> UpdateLibraryRemoveMovieAsync(Library library, Movie movie);

        Task<Library> UpdateLibraryAddTvSerieAsync(Library library, TvSeries tvSerie);

        Task<Library> UpdateLibraryRemoveTvSerieAsync(Library library, TvSeries tvSerie);

        Task DeleteLibraryAsync(int id);
    }
}