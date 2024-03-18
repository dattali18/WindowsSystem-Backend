using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindowsSystem_Backend.Migrations
{
    public partial class migrationv11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "totalSeasons",
                table: "TvSeries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImdbID",
                table: "TvSeries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "time",
                table: "TvSeries",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImdbID",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "time",
                table: "Movies",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImdbID",
                table: "TvSeries");

            migrationBuilder.DropColumn(
                name: "time",
                table: "TvSeries");

            migrationBuilder.DropColumn(
                name: "ImdbID",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "time",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "totalSeasons",
                table: "TvSeries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
