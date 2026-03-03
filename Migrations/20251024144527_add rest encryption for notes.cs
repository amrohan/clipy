using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clipy.Migrations
{
    /// <inheritdoc />
    public partial class addrestencryptionfornotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isEncrypted",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isEncrypted",
                table: "Notes");
        }
    }
}
