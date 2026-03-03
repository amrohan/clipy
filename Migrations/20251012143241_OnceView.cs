using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clipy.Migrations
{
    /// <inheritdoc />
    public partial class OnceView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteAfterView",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Viewed",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteAfterView",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Viewed",
                table: "Notes");
        }
    }
}
