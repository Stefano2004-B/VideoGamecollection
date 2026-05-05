using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGameCollection.Migrations
{
    /// <inheritdoc />
    public partial class UserAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "VideoGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "VideoGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VideoGames");
        }
    }
}
