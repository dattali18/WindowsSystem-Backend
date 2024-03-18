using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindowsSystem_Backend.Migrations
{
    public partial class migrationv10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    ImdbID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TvSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    StartingYear = table.Column<int>(type: "int", nullable: true),
                    EndingYear = table.Column<int>(type: "int", nullable: true),
                    TotalSeasons = table.Column<int>(type: "int", nullable: true),
                    ImdbID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryMovie",
                columns: table => new
                {
                    LibrariesId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryMovie", x => new { x.LibrariesId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_LibraryMovie_Libraries_LibrariesId",
                        column: x => x.LibrariesId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTvSeries",
                columns: table => new
                {
                    LibrariesId = table.Column<int>(type: "int", nullable: false),
                    TvSeriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTvSeries", x => new { x.LibrariesId, x.TvSeriesId });
                    table.ForeignKey(
                        name: "FK_LibraryTvSeries_Libraries_LibrariesId",
                        column: x => x.LibrariesId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryTvSeries_TvSeries_TvSeriesId",
                        column: x => x.TvSeriesId,
                        principalTable: "TvSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMovie_MoviesId",
                table: "LibraryMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTvSeries_TvSeriesId",
                table: "LibraryTvSeries",
                column: "TvSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ImdbID",
                table: "Movies",
                column: "ImdbID",
                unique: true,
                filter: "[ImdbID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TvSeries_ImdbID",
                table: "TvSeries",
                column: "ImdbID",
                unique: true,
                filter: "[ImdbID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryMovie");

            migrationBuilder.DropTable(
                name: "LibraryTvSeries");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "TvSeries");
        }
    }
}
