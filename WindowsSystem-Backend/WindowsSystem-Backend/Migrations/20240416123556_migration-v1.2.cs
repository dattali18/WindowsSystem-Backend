using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindowsSystem_Backend.Migrations
{
    public partial class migrationv12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingYear",
                table: "TvSeries");

            migrationBuilder.DropColumn(
                name: "StartingYear",
                table: "TvSeries");

            migrationBuilder.AddColumn<string>(
                name: "Years",
                table: "TvSeries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Years",
                table: "TvSeries");

            migrationBuilder.AddColumn<int>(
                name: "EndingYear",
                table: "TvSeries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartingYear",
                table: "TvSeries",
                type: "int",
                nullable: true);
        }
    }
}
