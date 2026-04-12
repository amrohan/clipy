using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clipy.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notes_Code",
                table: "Notes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notes_Code",
                table: "Notes");
        }
    }
}
