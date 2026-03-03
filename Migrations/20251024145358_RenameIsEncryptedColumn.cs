using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clipy.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsEncryptedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isEncrypted",
                table: "Notes",
                newName: "IsEncrypted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEncrypted",
                table: "Notes",
                newName: "isEncrypted");
        }
    }
}
