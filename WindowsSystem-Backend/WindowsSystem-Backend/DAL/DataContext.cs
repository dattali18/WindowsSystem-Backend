using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; } = null!;

        public DbSet<TvSeries> TvSeries { get; set; } = null!;

        public DbSet<Library> Libraries { get; set; } = null!;
    }
}
