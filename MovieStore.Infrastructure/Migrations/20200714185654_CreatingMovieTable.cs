using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieStore.Infrastructure.Migrations
{
    public partial class CreatingMovieTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genre",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
                    Overview = table.Column<string>(maxLength: 4096, nullable: true),
                    Tagline = table.Column<string>(maxLength: 512, nullable: true),
                    Budget = table.Column<decimal>(nullable: true),
                    Revenue = table.Column<decimal>(nullable: true),
                    ImdbUrl = table.Column<string>(maxLength: 2084, nullable: true),
                    TmdbUrl = table.Column<string>(maxLength: 2084, nullable: true),
                    PosterUrl = table.Column<string>(maxLength: 2084, nullable: true),
                    BackdropUrl = table.Column<string>(maxLength: 2084, nullable: true),
                    OriginalLanguage = table.Column<string>(maxLength: 64, nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: true),
                    RunTime = table.Column<int>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(5, 2)", nullable: true, defaultValue: 9.9m),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genre",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }
    }
}
