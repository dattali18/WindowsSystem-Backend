using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindowsSystem_Backend.Migrations
{
    public partial class migrationv12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImdbID",
                table: "TvSeries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImdbID",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TvSeries_ImdbID",
                table: "TvSeries",
                column: "ImdbID",
                unique: true,
                filter: "[ImdbID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ImdbID",
                table: "Movies",
                column: "ImdbID",
                unique: true,
                filter: "[ImdbID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TvSeries_ImdbID",
                table: "TvSeries");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ImdbID",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "ImdbID",
                table: "TvSeries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImdbID",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
