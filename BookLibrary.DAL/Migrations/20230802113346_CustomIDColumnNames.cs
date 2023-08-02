using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrary.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CustomIDColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Genres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Books",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Authors",
                newName: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Genres",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Authors",
                newName: "ID");
        }
    }
}
