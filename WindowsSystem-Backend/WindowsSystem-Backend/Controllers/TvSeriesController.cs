using Microsoft.AspNetCore.Mvc;

using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.BL;

using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DAL.Interfaces;
using WindowsSystem_Backend.DAL.Implementations;

namespace WindowsSystem_Backend.Controllers
{
    /// <summary>
    /// Controller for managing tv-series-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TvSeriesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        private readonly Bl bl = Factory.GetBL();

        private readonly IReadFromDataBase _readFromDataBase;

        private readonly IWriteToDataBase _writeToDataBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvSeriesController"/> class.
        /// </summary>
        /// <param name="dataContext"></param>
        public TvSeriesController(DataContext dataContext)
        {
            _dbContext = dataContext;

            _readFromDataBase = new ReadFromDataBase(_dbContext);
            _writeToDataBase = new WriteToDataBase(_dbContext);
        }

        // GET - api/TvSeries

        /// <summary>
        /// Retrieves all Tv Series
        /// </summary>
        /// <returns>A list of TvSeriesDTOs</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTvSeriesDto>>> GetTvSeries()
        {

            var Series = await _readFromDataBase.GetTvSeriesAsync();
            var seriesDTO = (
                from series in Series
                select bl.BlTvSeries.GetTvSeriesDtoFromTvSeries(series)
                ).ToList();

            return Ok(seriesDTO);
        }

        // GET - /api/TvSeries/{id}

        /// <summary>
        /// Retrieves a series by its ID.
        /// </summary>
        /// <param name="id">The ID of the series.</param>
        /// <returns>The series DTO.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTvSeriesDto>> GetTvSeries(int id)
        {
            var series = await _readFromDataBase.GetTvSerieByIdAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            var seriesDto = bl.BlTvSeries.GetTvSeriesDtoFromTvSeries(series);
            return Ok(seriesDto);
        }

        // GET - /api/TvSeries/search/{imdbID}

        /// <summary>
        /// Retrieves a series by its IMDb ID.
        /// </summary>
        /// <param name="imdbID">The IMDb ID of the series.</param>
        /// <returns>The series DTO.</returns>
        [HttpGet("search/{imdbID}")]
        public async Task<ActionResult<GetTvSeriesDto>> GetTvSeries(string imdbID)
        {
            var series = await bl.BlTvSeries.GetTvSeriesByImdbID(imdbID);

            if (series == null)
            {
                return NotFound();
            }

            var seriesDto = bl.BlTvSeries.GetTvSeriesDtoFromTvSeries(series);
            return Ok(seriesDto);
        }

        // GET - /api/TvSeries/?s=[SEARCH_TERM]&y=[YEAR]

        /// <summary>
        /// Retrieves series by search term and optional year.
        /// </summary>
        /// <param name="s">The search term.</param>
        /// <param name="y">The year (optional).</param>
        /// <returns>A list of media DTOs.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MediaDto>>> GetTvSeriesBySearch(string s, int? y = null)
        {
            var series = await bl.BlTvSeries.GetTvSeriesBySearch(s, y);
            return Ok(series);
        }

        // POST - api/TvSeries

        /// <summary>
        /// Adds a new series to the database.
        /// </summary>
        /// <param name="imdbID">The IMDb ID of the series.</param>
        /// <returns>The added series DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<GetTvSeriesDto>> PostTvSeries(string imdbID)
        {
            var existingSeries = await _readFromDataBase.GetTvSerieByImdbIdAsync(imdbID);

            if (existingSeries != null) 
            {
                return Ok(bl.BlTvSeries.GetTvSeriesDtoFromTvSeries(existingSeries));
            }

            var series = await bl.BlTvSeries.GetTvSeriesByImdbID(imdbID);

            if (series == null)
            {
                return BadRequest();
            }

            await _writeToDataBase.AddTvSerieAsync(series);
            var seriesDto = bl.BlTvSeries.GetTvSeriesDtoFromTvSeries(series);
            return Ok(seriesDto);
        }
    }
}
