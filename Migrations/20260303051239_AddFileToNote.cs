using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clipy.Migrations
{
    /// <inheritdoc />
    public partial class AddFileToNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Notes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Notes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Notes");
        }
    }
}
